using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Procurement;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PlannedBillMgr : PlannedBillBaseMgr, IPlannedBillMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }

        [Transaction(TransactionMode.Unspecified)]
        public IList<PlannedBill> GetUnSettledPlannedBill(OrderHead orderHead)
        {
            return this.GetUnSettledPlannedBill(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<PlannedBill> GetUnSettledPlannedBill(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<PlannedBill>();
            criteria.Add(Expression.Eq("OrderNo", orderNo));
            criteria.Add(Expression.NotEqProperty("PlannedQty", "ActingQty"));
            return this.criteriaMgrE.FindAll<PlannedBill>(criteria);
        }

        #region Customized Methods
        [Transaction(TransactionMode.Requires)]
        public PlannedBill CreatePlannedBill(ReceiptDetail receiptDetail, User user)
        {
            Receipt receipt = receiptDetail.Receipt;

            OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            //DateTime dateTimeNow = DateTime.Now;
            //decimal plannedAmount = 0;

            PlannedBill plannedBill = new PlannedBill();
            plannedBill.OrderNo = orderHead.OrderNo;
            plannedBill.ExternalReceiptNo = receipt.ExternalReceiptNo;        //记录客户回单号
            plannedBill.ReceiptNo = receipt.ReceiptNo;
            plannedBill.Item = orderDetail.Item;
            plannedBill.SettleTerm = orderDetail.DefaultBillSettleTerm;
            plannedBill.PlannedQty =
                receiptDetail.ReceivedQty.HasValue ? receiptDetail.ReceivedQty.Value : 0;         //设置待结算数量默认值
            plannedBill.Uom = orderDetail.Uom;                                                  //单位为订单单位
            plannedBill.UnitCount = orderDetail.UnitCount;
            plannedBill.UnitQty = orderLocationTransaction.UnitQty;                                 //UnitQty沿用OrderLocationTransaction
            plannedBill.PlannedAmount = receiptDetail.PlannedAmount;
            plannedBill.CreateDate = receipt.CreateDate;
            plannedBill.CreateUser = user;
            plannedBill.LastModifyDate = receipt.CreateDate;
            plannedBill.LastModifyUser = user;
            plannedBill.IsAutoBill = orderHead.IsAutoBill;
            plannedBill.HuId = receiptDetail.HuId;
            plannedBill.LotNo = receiptDetail.LotNo;
            if (orderLocationTransaction.Location != null)
            {
                plannedBill.LocationFrom = orderLocationTransaction.Location.Code;
            }
            plannedBill.IpNo = receipt.ReferenceIpNo;
            plannedBill.ReferenceItemCode = orderDetail.ReferenceItemCode;

            //plannedBill.BillAddress = orderDetail.DefaultBillAddress;
            plannedBill.PriceList = orderDetail.DefaultPriceList;
            plannedBill.IsProvisionalEstimate =      //暂估价格处理，没有找到价格也认为是暂估价格
                     orderDetail.UnitPrice.HasValue ? orderDetail.IsProvisionalEstimate : true;
            plannedBill.ListPrice = orderDetail.UnitPrice.HasValue ? orderDetail.UnitPrice.Value : 0;
            plannedBill.UnitPrice = orderDetail.IncludeTaxPrice;
            plannedBill.PlannedAmount = orderDetail.IncludeTaxPrice * plannedBill.PlannedQty;
            plannedBill.Currency = orderDetail.OrderHead.Currency;
            plannedBill.IsIncludeTax = orderDetail.IsIncludeTax;
            plannedBill.TaxCode = orderDetail.TaxCode;
            if (orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
            {
                plannedBill.TransactionType = BusinessConstants.BILL_TRANS_TYPE_PO;
                plannedBill.BillAddress = orderDetail.DefaultBillFrom;
            }
            else if (orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                plannedBill.TransactionType = BusinessConstants.BILL_TRANS_TYPE_SO;
                plannedBill.BillAddress = orderDetail.DefaultBillTo;
            }
            else
            {
                throw new TechnicalException("Only SO and PO/SubContract can create planned bill.");
            }
 

            //if (orderDetail.Uom.Code != plannedBill.Uom.Code)
            //{
            //    //订单单位和采购单位不一致，需要更改UnitQty和PlannedQty值
            //    plannedBill.UnitQty = this.uomConversionMgrE.ConvertUomQty(orderDetail.Item, orderDetail.Uom, plannedBill.UnitQty, plannedBill.Uom);
            //    plannedBill.PlannedQty = plannedBill.PlannedQty * plannedBill.UnitQty;
            //}

            
            this.CreatePlannedBill(plannedBill);

            return plannedBill;
        }

        #endregion Customized Methods
    }
}


#region 扩展




namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PlannedBillMgrE : com.Sconit.Service.MasterData.Impl.PlannedBillMgr, IPlannedBillMgrE
    {

    }
}
#endregion
