using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.Production;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Criteria;
using NHibernate.Expression;
using com.Sconit.Service.Distribution;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Distribution;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ProductLineInProcessLocationDetailMgr : ProductLineInProcessLocationDetailBaseMgr, IProductLineInProcessLocationDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationTransactionMgrE locationTransactionMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IOrderPlannedBackflushMgrE orderPlannedBackflushMgrE { get; set; }
        public IInProcessLocationDetailMgrE inProcessLocationDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetail(string prodLineCode, string status)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ProductLineInProcessLocationDetail>();

            criteria.Add(Expression.Eq("ProductLine.Code", prodLineCode));
            criteria.Add(Expression.Eq("Status", status));

            criteria.AddOrder(Order.Asc("Id"));

            return this.criteriaMgrE.FindAll<ProductLineInProcessLocationDetail>(criteria);
        }

        public IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetailGroupByItem(string prodLineCode, string status)
        {
            IList<ProductLineInProcessLocationDetail> plIpGroupList = new List<ProductLineInProcessLocationDetail>();
            IList<ProductLineInProcessLocationDetail> plIpList = GetProductLineInProcessLocationDetail(prodLineCode, status);
            foreach (ProductLineInProcessLocationDetail plIpDetail in plIpList)
            {
                bool isExist = false;
                foreach (ProductLineInProcessLocationDetail plIpGroupDetail in plIpGroupList)
                {
                    if (plIpGroupDetail.Item.Code == plIpDetail.Item.Code)
                    {
                        isExist = true;
                        plIpGroupDetail.Qty += plIpDetail.Qty;
                        plIpGroupDetail.BackflushQty += plIpGroupDetail.BackflushQty;
                        break;
                    }
                }
                if (!isExist)
                {
                    ProductLineInProcessLocationDetail newPlIpDetail = new ProductLineInProcessLocationDetail();
                    newPlIpDetail.Item = plIpDetail.Item;
                    newPlIpDetail.Qty = plIpDetail.Qty;
                    newPlIpDetail.BackflushQty = plIpDetail.BackflushQty;
                    plIpGroupList.Add(newPlIpDetail);
                }

            }

            return plIpGroupList;
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn(string prodLineCode, IList<MaterialIn> materialInList, User user)
        {
            Flow flow = this.flowMgrE.CheckAndLoadFlow(prodLineCode);
            IList<BomDetail> bomDetailList = this.flowMgrE.GetBatchFeedBomDetail(flow);

            IList<MaterialIn> noneZeroMaterialInList = new List<MaterialIn>();
            DateTime dateTimeNow = DateTime.Now;

            if (materialInList != null && materialInList.Count > 0)
            {
                foreach (MaterialIn materialIn in materialInList)
                {
                    if (materialIn.Qty != 0)
                    {
                        noneZeroMaterialInList.Add(materialIn);
                    }

                    #region 查找物料是否是生产线上投料的
                    if (bomDetailList != null && bomDetailList.Count > 0)
                    {
                        bool findMatch = false;
                        foreach (BomDetail bomDetail in bomDetailList)
                        {
                            if (bomDetail.Item.Code == materialIn.RawMaterial.Code)
                            {
                                findMatch = true;
                                break;
                            }
                        }

                        if (!findMatch)
                        {
                            throw new BusinessErrorException("MasterData.Production.Feed.Error.NotContainMaterial", materialIn.RawMaterial.Code, prodLineCode);
                        }
                    }
                    else
                    {
                        throw new BusinessErrorException("MasterData.Production.Feed.Error.NoFeedMaterial", prodLineCode);
                    }
                    #endregion
                }
            }

            if (noneZeroMaterialInList.Count == 0)
            {
                throw new BusinessErrorException("Order.Error.ProductLineInProcessLocationDetailEmpty");
            }

            foreach (MaterialIn materialIn in noneZeroMaterialInList)
            {
                #region 出库
                IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(materialIn, user, flow);
                #endregion

                #region 入生产线物料
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    ProductLineInProcessLocationDetail productLineInProcessLocationDetail = new ProductLineInProcessLocationDetail();
                    productLineInProcessLocationDetail.ProductLine = flow;
                    productLineInProcessLocationDetail.Operation = materialIn.Operation;
                    productLineInProcessLocationDetail.Item = inventoryTransaction.Item;
                    productLineInProcessLocationDetail.HuId = inventoryTransaction.Hu != null ? inventoryTransaction.Hu.HuId : null;
                    productLineInProcessLocationDetail.LotNo = inventoryTransaction.Hu != null ? inventoryTransaction.Hu.LotNo : null;
                    productLineInProcessLocationDetail.Qty = 0 - inventoryTransaction.Qty;
                    productLineInProcessLocationDetail.IsConsignment = inventoryTransaction.IsConsignment;
                    productLineInProcessLocationDetail.PlannedBill = inventoryTransaction.PlannedBill;
                    productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                    productLineInProcessLocationDetail.LocationFrom = inventoryTransaction.Location;
                    productLineInProcessLocationDetail.CreateDate = dateTimeNow;
                    productLineInProcessLocationDetail.CreateUser = user;
                    productLineInProcessLocationDetail.LastModifyDate = dateTimeNow;
                    productLineInProcessLocationDetail.LastModifyUser = user;

                    this.CreateProductLineInProcessLocationDetail(productLineInProcessLocationDetail);

                    //记录库存事务
                    this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_MATERIAL_IN, user, BusinessConstants.IO_TYPE_IN);
                }
                #endregion
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialIn(Flow prodLine, IList<MaterialIn> materialInList, User user)
        {
            this.RawMaterialIn(prodLine.Code, materialInList, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(string prodLineCode, User user)
        {
            this.RawMaterialBackflush(prodLineCode, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(string prodLineCode, IDictionary<string, decimal> itemQtydic, User user)
        {
            Flow flow = this.flowMgrE.CheckAndLoadFlow(prodLineCode);
            DateTime dateTimeNow = DateTime.Now;

            IList<ProductLineInProcessLocationDetail> productLineInProcessLocationDetailList =
                this.GetProductLineInProcessLocationDetail(prodLineCode, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);

            IList<ProductLineInProcessLocationDetail> targetProductLineInProcessLocationDetailList = new List<ProductLineInProcessLocationDetail>();

            #region 根据剩余数量计算回冲零件数量，添加到待处理列表
            if (itemQtydic != null && itemQtydic.Count > 0)
            {
                foreach (string itemCode in itemQtydic.Keys)
                {
                    decimal remainQty = itemQtydic[itemCode];   //剩余投料量
                    decimal inQty = 0;                     //总投料量
                    IList<ProductLineInProcessLocationDetail> currentProductLineInProcessLocationDetailList = new List<ProductLineInProcessLocationDetail>();
                    foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
                    {
                        if (productLineInProcessLocationDetail.Item.Code == itemCode)
                        {
                            inQty += (productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty);
                            currentProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                        }
                    }

                    if (remainQty > inQty)
                    {
                        throw new BusinessErrorException("MasterData.Production.Feed.Error.RemainQtyGtFeedQty", itemCode);
                    }

                    decimal backflushQty = inQty - remainQty;  //本次回冲量

                    #region 设定本次回冲数量
                    if (backflushQty > 0)
                    {
                        foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in currentProductLineInProcessLocationDetailList)
                        {
                            if (backflushQty - (productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty) > 0)
                            {
                                productLineInProcessLocationDetail.CurrentBackflushQty = productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
                                productLineInProcessLocationDetail.BackflushQty = productLineInProcessLocationDetail.Qty;
                                backflushQty -= productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
                                targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                            }
                            else
                            {
                                productLineInProcessLocationDetail.CurrentBackflushQty = backflushQty;
                                productLineInProcessLocationDetail.BackflushQty += backflushQty;
                                backflushQty = 0;
                                targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            //为了多次回冲，注掉此处代码
            //#region 处理未填写剩余数量的投料，全部添加到待处理列表
            //foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in productLineInProcessLocationDetailList)
            //{
            //    bool isUsed = false;
            //    foreach (string itemCode in itemQtydic.Keys)
            //    {
            //        if (productLineInProcessLocationDetail.Item.Code == itemCode)
            //        {
            //            isUsed = true;
            //            break;
            //        }
            //    }

            //    //未填写剩余数量的全部回冲
            //    if (!isUsed)
            //    {
            //        productLineInProcessLocationDetail.CurrentBackflushQty = productLineInProcessLocationDetail.Qty - productLineInProcessLocationDetail.BackflushQty;
            //        productLineInProcessLocationDetail.BackflushQty = productLineInProcessLocationDetail.Qty;
            //        targetProductLineInProcessLocationDetailList.Add(productLineInProcessLocationDetail);
            //    }
            //}
            //#endregion

            if (targetProductLineInProcessLocationDetailList != null && targetProductLineInProcessLocationDetailList.Count > 0)
            {
                IList<OrderPlannedBackflush> orderPlannedBackflushList = this.orderPlannedBackflushMgrE.GetActiveOrderPlannedBackflush(prodLineCode);
                IDictionary<int, OrderPlannedBackflush> backFlushedorderPlannedDic = new Dictionary<int, OrderPlannedBackflush>();

                if (orderPlannedBackflushList == null || orderPlannedBackflushList.Count == 0)
                {
                    throw new BusinessErrorException("MasterData.Production.Feed.Error.NoWO", prodLineCode);
                }

                #region 物料回冲
                foreach (ProductLineInProcessLocationDetail productLineInProcessLocationDetail in targetProductLineInProcessLocationDetailList)
                {
                    #region 查找匹配的待回冲OrderLocationTransaction
                    IList<OrderPlannedBackflush> matchedOrderPlannedBackflushList = new List<OrderPlannedBackflush>();
                    decimal totalBaseQty = 0; //回冲分配基数
                    if (orderPlannedBackflushList != null && orderPlannedBackflushList.Count > 0)
                    {
                        foreach (OrderPlannedBackflush orderPlannedBackflush in orderPlannedBackflushList)
                        {
                            OrderLocationTransaction orderLocationTransaction = orderPlannedBackflush.OrderLocationTransaction;
                            if (productLineInProcessLocationDetail.Item.Code == orderLocationTransaction.Item.Code
                                && ((productLineInProcessLocationDetail.Operation.HasValue && productLineInProcessLocationDetail.Operation.Value == orderLocationTransaction.Operation)
                                || !productLineInProcessLocationDetail.Operation.HasValue))
                            {
                                matchedOrderPlannedBackflushList.Add(orderPlannedBackflush);
                                totalBaseQty += orderPlannedBackflush.PlannedQty;

                                if (!backFlushedorderPlannedDic.ContainsKey(orderPlannedBackflush.Id))
                                {
                                    backFlushedorderPlannedDic.Add(orderPlannedBackflush.Id, orderPlannedBackflush);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 更新生产线上的物料
                    if (productLineInProcessLocationDetail.Qty == productLineInProcessLocationDetail.BackflushQty)
                    {
                        productLineInProcessLocationDetail.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    }
                    productLineInProcessLocationDetail.LastModifyDate = dateTimeNow;
                    productLineInProcessLocationDetail.LastModifyUser = user;

                    this.UpdateProductLineInProcessLocationDetail(productLineInProcessLocationDetail);

                    //记录库存事务
                    //this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR, user, BusinessConstants.IO_TYPE_OUT);
                    //this.locationTransactionMgrE.RecordLocationTransaction(productLineInProcessLocationDetail, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR, user, BusinessConstants.IO_TYPE_OUT);
                    #endregion

                    if (matchedOrderPlannedBackflushList.Count > 0)
                    {
                        #region 物料回冲
                        EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                        int amountDecimalLength = int.Parse(entityPreference.Value);

                        decimal remainTobeBackflushQty = productLineInProcessLocationDetail.CurrentBackflushQty;  //剩余待回冲数量
                        decimal unitQty = Math.Round(remainTobeBackflushQty / totalBaseQty, amountDecimalLength);  //单位基数的回冲数量

                        for (int i = 0; i < matchedOrderPlannedBackflushList.Count; i++)
                        {
                            #region 更新匹配的OrderLocationTransaction
                            OrderPlannedBackflush matchedOrderPlannedBackflush = matchedOrderPlannedBackflushList[i];
                            OrderLocationTransaction matchedOrderLocationTransaction = matchedOrderPlannedBackflush.OrderLocationTransaction;

                            bool isLastestRecord = (i == (matchedOrderPlannedBackflushList.Count - 1));
                            decimal currentTotalBackflushQty = 0;

                            if (!matchedOrderLocationTransaction.AccumulateQty.HasValue)
                            {
                                matchedOrderLocationTransaction.AccumulateQty = 0;
                            }

                            if (!isLastestRecord)
                            {
                                decimal currentBackflushQty = matchedOrderPlannedBackflush.PlannedQty * unitQty;
                                currentTotalBackflushQty += currentBackflushQty;
                                matchedOrderLocationTransaction.AccumulateQty += currentBackflushQty;
                                remainTobeBackflushQty -= currentBackflushQty;
                            }
                            else
                            {
                                currentTotalBackflushQty += remainTobeBackflushQty;
                                matchedOrderLocationTransaction.AccumulateQty += remainTobeBackflushQty;
                                remainTobeBackflushQty = 0;
                            }

                            this.orderLocationTransactionMgrE.UpdateOrderLocationTransaction(matchedOrderLocationTransaction);
                            #endregion

                            #region 新增/更新AsnDetail
                            InProcessLocationDetail inProcessLocationDetail = null;
                            if (productLineInProcessLocationDetail.HuId == null || productLineInProcessLocationDetail.HuId.Trim() == string.Empty)
                            {
                                inProcessLocationDetail = this.inProcessLocationDetailMgrE.GetNoneHuAndIsConsignmentInProcessLocationDetail(matchedOrderPlannedBackflush.InProcessLocation, matchedOrderPlannedBackflush.OrderLocationTransaction);
                                if (inProcessLocationDetail != null)
                                {
                                    inProcessLocationDetail.Qty += currentTotalBackflushQty;

                                    this.inProcessLocationDetailMgrE.UpdateInProcessLocationDetail(inProcessLocationDetail);
                                }
                            }

                            if (inProcessLocationDetail == null)
                            {
                                inProcessLocationDetail = new InProcessLocationDetail();
                                inProcessLocationDetail.InProcessLocation = matchedOrderPlannedBackflush.InProcessLocation;
                                inProcessLocationDetail.OrderLocationTransaction = matchedOrderPlannedBackflush.OrderLocationTransaction;
                                inProcessLocationDetail.HuId = productLineInProcessLocationDetail.HuId;
                                inProcessLocationDetail.LotNo = productLineInProcessLocationDetail.LotNo;
                                inProcessLocationDetail.IsConsignment = productLineInProcessLocationDetail.IsConsignment;
                                inProcessLocationDetail.PlannedBill = productLineInProcessLocationDetail.PlannedBill;
                                inProcessLocationDetail.Qty = currentTotalBackflushQty;

                                this.inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocationDetail);

                                matchedOrderPlannedBackflush.InProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                            }
                            #endregion

                            #region 新增库存事务
                            this.locationTransactionMgrE.RecordWOBackflushLocationTransaction(
                                matchedOrderPlannedBackflush.OrderLocationTransaction, productLineInProcessLocationDetail.HuId,
                                productLineInProcessLocationDetail.LotNo, currentTotalBackflushQty,
                                matchedOrderPlannedBackflush.InProcessLocation.IpNo, user, productLineInProcessLocationDetail.LocationFrom);
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region 没有匹配的OrderLocationTransaction
                        //退回原库位
                        this.locationMgrE.InventoryIn(productLineInProcessLocationDetail, user);
                        #endregion
                    }
                }
                #endregion

                #region 关闭OrderPlannedBackflush
                foreach (OrderPlannedBackflush orderPlannedBackflush in backFlushedorderPlannedDic.Values)
                {
                    orderPlannedBackflush.IsActive = false;
                    this.orderPlannedBackflushMgrE.UpdateOrderPlannedBackflush(orderPlannedBackflush);
                }
                #endregion
            }
            else
            {
                throw new BusinessErrorException("MasterData.Production.Feed.Error.NoFeed", prodLineCode);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(Flow prodLine, User user)
        {
            this.RawMaterialBackflush(prodLine.Code, null, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void RawMaterialBackflush(Flow prodLine, IDictionary<string, decimal> itemQtydic, User user)
        {
            this.RawMaterialBackflush(prodLine.Code, itemQtydic, user);
        }       

        #endregion Customized Methods

        #region Private Methods

        #endregion
    }
}


#region 扩展


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ProductLineInProcessLocationDetailMgrE : com.Sconit.Service.MasterData.Impl.ProductLineInProcessLocationDetailMgr, IProductLineInProcessLocationDetailMgrE
    {

    }
}


#endregion