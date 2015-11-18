using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class InspectOrderMgr : InspectOrderBaseMgr, IInspectOrderMgr
    {
        public INumberControlMgrE numberControlMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IInspectOrderDetailMgrE inspectOrderDetailMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo)
        {
            return this.CheckAndLoadInspectOrder(inspectOrderNo, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo, bool includeDetail)
        {
            InspectOrder inspectOrder = this.LoadInspectOrder(inspectOrderNo);
            if (inspectOrder == null)
            {
                throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.InspectOrderNoNotExist", inspectOrderNo);
            }

            if (includeDetail && inspectOrder.InspectOrderDetails != null && inspectOrder.InspectOrderDetails.Count > 0)
            {
            }

            return inspectOrder;
        }



        [Transaction(TransactionMode.Requires)]
        public InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, User user)
        {
            return CreateInspectOrder(locationLotDetailList, user, null, null, true);
        }

        [Transaction(TransactionMode.Requires)]
        public InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, User user, string ipNo, string receiptNo, bool isSeperated)
        {
            IList<LocationLotDetail> noneZeroLocationLotDetailList = new List<LocationLotDetail>();

            bool? isDetailHasHu = null;
            if (locationLotDetailList != null && locationLotDetailList.Count > 0)
            {
                foreach (LocationLotDetail locationLotDetail in locationLotDetailList)
                {
                    if (locationLotDetail.Location.Code == BusinessConstants.SYSTEM_LOCATION_INSPECT
                        || locationLotDetail.Location.Code == BusinessConstants.SYSTEM_LOCATION_REJECT)
                    {
                        throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.LocationFrom", locationLotDetail.Location.Code);
                    }

                    if (locationLotDetail.CurrentInspectQty > 0)
                    {
                        if (isDetailHasHu == null)
                        {
                            isDetailHasHu = (locationLotDetail.Hu != null);
                        }
                        else if (isDetailHasHu != (locationLotDetail.Hu != null))
                        {
                            throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.NotAllDetailHasHu");
                        }
                        noneZeroLocationLotDetailList.Add(locationLotDetail);
                    }
                }
            }

            if (noneZeroLocationLotDetailList.Count == 0)
            {
                throw new BusinessErrorException("Order.Error.Inspection.DetailEmpty");
            }

            #region 创建检验单头
            DateTime dateTimeNow = DateTime.Now;
            InspectOrder inspectOrder = new InspectOrder();
            inspectOrder.InspectNo = this.numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_INSPECTION);
            inspectOrder.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            inspectOrder.CreateUser = user;
            inspectOrder.CreateDate = dateTimeNow;
            inspectOrder.LastModifyUser = user;
            inspectOrder.LastModifyDate = dateTimeNow;
            inspectOrder.IsDetailHasHu = isDetailHasHu.Value;
            inspectOrder.IpNo = ipNo;
            inspectOrder.ReceiptNo = receiptNo;
            inspectOrder.IsSeperated = isSeperated;

            this.CreateInspectOrder(inspectOrder);
            #endregion

            #region 创建检验明细
            foreach (LocationLotDetail locationLotDetail in noneZeroLocationLotDetailList)
            {
                //零件出库
                this.locationMgrE.InspectOut(locationLotDetail, user, false, inspectOrder.InspectNo, this.locationMgrE.GetInspectLocation());

                //入待验库位
                IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InspectIn(locationLotDetail, this.locationMgrE.GetInspectLocation(), user, false, inspectOrder.InspectNo);

                if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
                {
                    foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                    {
                        InspectOrderDetail inspectOrderDetail = new InspectOrderDetail();
                        inspectOrderDetail.InspectOrder = inspectOrder;
                        inspectOrderDetail.InspectQty = inventoryTransaction.Qty;
                        inspectOrderDetail.LocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                        inspectOrderDetail.LocationFrom = locationLotDetail.Location;
                        inspectOrderDetail.LocationTo = locationLotDetail.InspectQualifyLocation != null ? locationLotDetail.InspectQualifyLocation : locationLotDetail.Location;

                        this.inspectOrderDetailMgrE.CreateInspectOrderDetail(inspectOrderDetail);

                        inspectOrder.AddInspectOrderDetail(inspectOrderDetail);
                    }
                }
            }
            #endregion

            return inspectOrder;
        }

        [Transaction(TransactionMode.Requires)]
        public InspectOrder CreateInspectOrder(Location location, IDictionary<string, decimal> itemQtyDic, User user)
        {
            return CreateInspectOrder(location.Code, itemQtyDic, user);
        }

        [Transaction(TransactionMode.Requires)]
        public InspectOrder CreateInspectOrder(string locationCode, IDictionary<string, decimal> itemQtyDic, User user)
        {
            if (locationCode == BusinessConstants.SYSTEM_LOCATION_INSPECT
                       || locationCode == BusinessConstants.SYSTEM_LOCATION_REJECT)
            {
                throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.LocationFrom", locationCode);
            }

            #region 创建检验单头
            DateTime dateTimeNow = DateTime.Now;
            InspectOrder inspectOrder = new InspectOrder();
            inspectOrder.InspectNo = this.numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_INSPECTION);
            inspectOrder.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            inspectOrder.CreateUser = user;
            inspectOrder.CreateDate = dateTimeNow;
            inspectOrder.LastModifyUser = user;
            inspectOrder.LastModifyDate = dateTimeNow;
            inspectOrder.IsDetailHasHu = false;
            inspectOrder.IsSeperated = true;

            this.CreateInspectOrder(inspectOrder);
            #endregion

            #region 创建检验明细
            if (itemQtyDic != null && itemQtyDic.Count > 0)
            {
                Location location = this.locationMgrE.CheckAndLoadLocation(locationCode);

                foreach (string itemCode in itemQtyDic.Keys)
                {
                    if (itemQtyDic[itemCode] == 0)
                    {
                        continue;
                    }

                    Item item = this.itemMgrE.CheckAndLoadItem(itemCode);

                    //零件出库
                    IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InspectOut(location, item, itemQtyDic[itemCode], user, inspectOrder.InspectNo, this.locationMgrE.GetInspectLocation());

                    //入待验库位
                    foreach (InventoryTransaction outInventoryTransaction in inventoryTransactionList)
                    {
                        IList<InventoryTransaction> inInventoryTransactionList = this.locationMgrE.InspectIn(item, 0 - outInventoryTransaction.Qty, user, inspectOrder.InspectNo, outInventoryTransaction.PlannedBill, location);

                        if (inInventoryTransactionList != null && inInventoryTransactionList.Count > 0)
                        {
                            foreach (InventoryTransaction inInventoryTransaction in inInventoryTransactionList)
                            {
                                InspectOrderDetail inspectOrderDetail = new InspectOrderDetail();
                                inspectOrderDetail.InspectOrder = inspectOrder;
                                inspectOrderDetail.InspectQty = itemQtyDic[itemCode];
                                inspectOrderDetail.LocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inInventoryTransaction.LocationLotDetailId);
                                inspectOrderDetail.LocationFrom = location;
                                inspectOrderDetail.LocationTo = location;


                                this.inspectOrderDetailMgrE.CreateInspectOrderDetail(inspectOrderDetail);

                                inspectOrder.AddInspectOrderDetail(inspectOrderDetail);
                            }
                        }
                    }
                }
            }
            #endregion

            return inspectOrder;
        }

        [Transaction(TransactionMode.Requires)]
        public InspectOrder CreateFgInspectOrder(string locationCode, IDictionary<string, decimal> itemFgQtyDic, User user)
        {
            #region 创建检验单头
            DateTime dateTimeNow = DateTime.Now;
            InspectOrder inspectOrder = new InspectOrder();
            inspectOrder.InspectNo = this.numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_INSPECTION);
            inspectOrder.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            inspectOrder.CreateUser = user;
            inspectOrder.CreateDate = dateTimeNow;
            inspectOrder.LastModifyUser = user;
            inspectOrder.LastModifyDate = dateTimeNow;
            inspectOrder.IsDetailHasHu = false;
            inspectOrder.IsSeperated = true;

            this.CreateInspectOrder(inspectOrder);
            #endregion

            #region 创建检验明细
            if (itemFgQtyDic != null && itemFgQtyDic.Count > 0)
            {
                Location location = this.locationMgrE.CheckAndLoadLocation(locationCode);
                string itemCode = string.Empty;
                string fgCode = string.Empty;

                foreach (string itemFgCode in itemFgQtyDic.Keys)
                {
                    if (itemFgQtyDic[itemFgCode] == 0)
                    {
                        continue;
                    }
                    string[] itemFg = itemFgCode.Split('-');
                    itemCode = itemFg[0];
                    fgCode = itemFg[1];

                    Item item = this.itemMgrE.CheckAndLoadItem(itemCode);

                    //零件出库
                    this.locationMgrE.InspectOut(location, item, itemFgQtyDic[itemFgCode], user, inspectOrder.InspectNo, this.locationMgrE.GetInspectLocation());

                    //入待验库位
                    IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InspectIn(item, itemFgQtyDic[itemFgCode], user, inspectOrder.InspectNo, null, location);

                    if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
                    {
                        foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                        {
                            InspectOrderDetail inspectOrderDetail = new InspectOrderDetail();
                            inspectOrderDetail.InspectOrder = inspectOrder;
                            inspectOrderDetail.InspectQty = itemFgQtyDic[itemFgCode];
                            inspectOrderDetail.LocationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                            inspectOrderDetail.LocationFrom = location;
                            inspectOrderDetail.LocationTo = location;
                            inspectOrderDetail.FinishGoods = this.itemMgrE.LoadItem(fgCode);


                            this.inspectOrderDetailMgrE.CreateInspectOrderDetail(inspectOrderDetail);

                            inspectOrder.AddInspectOrderDetail(inspectOrderDetail);
                        }
                    }
                }
            }
            #endregion

            return inspectOrder;
        }

        [Transaction(TransactionMode.Requires)]
        public void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, string userCode)
        {
            ProcessInspectOrder(inspectOrderDetailList, userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, User user)
        {
            #region 过滤检验数量为0的检验明细
            IList<InspectOrderDetail> noneZeroInspectOrderDetailList = new List<InspectOrderDetail>();

            if (inspectOrderDetailList != null && inspectOrderDetailList.Count > 0)
            {
                foreach (InspectOrderDetail inspectOrderDetail in inspectOrderDetailList)
                {
                    if (inspectOrderDetail.CurrentQualifiedQty > 0 || inspectOrderDetail.CurrentRejectedQty > 0)
                    {
                        noneZeroInspectOrderDetailList.Add(inspectOrderDetail);
                    }
                }
            }

            if (noneZeroInspectOrderDetailList.Count == 0)
            {
                throw new BusinessErrorException("Order.Error.Inspection.DetailEmpty");
            }
            #endregion

            #region 循环检验单明细
            IDictionary<string, InspectOrder> cachedInspectOrderDic = new Dictionary<string, InspectOrder>();
            foreach (InspectOrderDetail inspectOrderDetail in noneZeroInspectOrderDetailList)
            {
                #region 缓存检验单头
                InspectOrder inspectOrder = inspectOrderDetail.InspectOrder;
                if (!cachedInspectOrderDic.ContainsKey(inspectOrder.InspectNo))
                {
                    cachedInspectOrderDic.Add(inspectOrder.InspectNo, inspectOrder);
                }
                #endregion

                #region 检验数量
                InspectOrderDetail oldInspectOrderDetail = this.inspectOrderDetailMgrE.LoadInspectOrderDetail(inspectOrderDetail.Id);
                oldInspectOrderDetail.Disposition = inspectOrderDetail.Disposition;
                oldInspectOrderDetail.CurrentQualifiedQty = inspectOrderDetail.CurrentQualifiedQty;
                oldInspectOrderDetail.CurrentRejectedQty = inspectOrderDetail.CurrentRejectedQty;
                decimal totalInspectedQty = (oldInspectOrderDetail.QualifiedQty.HasValue ? oldInspectOrderDetail.QualifiedQty.Value : 0)
                                          + (oldInspectOrderDetail.RejectedQty.HasValue ? oldInspectOrderDetail.RejectedQty.Value : 0)
                                          + oldInspectOrderDetail.CurrentQualifiedQty
                                          + oldInspectOrderDetail.CurrentRejectedQty;

                if (oldInspectOrderDetail.LocationLotDetail.Hu != null
                    && oldInspectOrderDetail.InspectQty != totalInspectedQty)
                {
                    //有条码的只能检验一次
                    throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.HuInspectQtyNotMatch", oldInspectOrderDetail.LocationLotDetail.Hu.HuId);
                }

                if (oldInspectOrderDetail.InspectQty < totalInspectedQty)
                {
                    throw new BusinessErrorException("MasterData.Inventory.Inspect.Error.InspectQtyExcceed", oldInspectOrderDetail.LocationLotDetail.Item.Code);
                }
                #endregion

                #region 合格品
                if (oldInspectOrderDetail.CurrentQualifiedQty > 0)
                {
                    oldInspectOrderDetail.LocationLotDetail.CurrentInspectQty = oldInspectOrderDetail.CurrentQualifiedQty;

                    //出待验库位
                    this.locationMgrE.InspectOut(oldInspectOrderDetail.LocationLotDetail, user, true, inspectOrder.InspectNo, oldInspectOrderDetail.LocationTo);

                    //入合格品库位
                    this.locationMgrE.InspectIn(oldInspectOrderDetail.LocationLotDetail, oldInspectOrderDetail.LocationTo, oldInspectOrderDetail.CurrentStorageBin, user, true, inspectOrder.InspectNo);

                    //更新合格品数量
                    if (!oldInspectOrderDetail.QualifiedQty.HasValue)
                    {
                        oldInspectOrderDetail.QualifiedQty = 0;
                    }
                    oldInspectOrderDetail.QualifiedQty += oldInspectOrderDetail.CurrentQualifiedQty;

                }
                #endregion

                #region 不合格品
                if (oldInspectOrderDetail.CurrentRejectedQty > 0)
                {
                    oldInspectOrderDetail.LocationLotDetail.CurrentInspectQty = oldInspectOrderDetail.CurrentRejectedQty;

                    //出待验库位
                    this.locationMgrE.InspectOut(oldInspectOrderDetail.LocationLotDetail, user, false, inspectOrder.InspectNo, this.locationMgrE.GetRejectLocation());

                    //入不合格品库位
                    this.locationMgrE.InspectIn(oldInspectOrderDetail.LocationLotDetail, this.locationMgrE.GetRejectLocation(), user, false, inspectOrder.InspectNo);

                    //更新不合格品数量
                    if (!oldInspectOrderDetail.RejectedQty.HasValue)
                    {
                        oldInspectOrderDetail.RejectedQty = 0;
                    }
                    oldInspectOrderDetail.RejectedQty += oldInspectOrderDetail.CurrentRejectedQty;
                }
                #endregion

                #region 更新检验单明细
                this.inspectOrderDetailMgrE.UpdateInspectOrderDetail(oldInspectOrderDetail);
                #endregion
            }
            #endregion

            #region 更新检验单
            DateTime dataTimeNow = DateTime.Now;
            foreach (InspectOrder oldInspectOrder in cachedInspectOrderDic.Values)
            {
                InspectOrder inspectOrder = this.LoadInspectOrder(oldInspectOrder.InspectNo);
                inspectOrder.LastModifyUser = user;
                inspectOrder.LastModifyDate = dataTimeNow;

                bool allClose = true;
                IList<InspectOrderDetail> detailList = inspectOrderDetailMgrE.GetInspectOrderDetail(inspectOrder);
                foreach (InspectOrderDetail inspectOrderDetail in detailList)
                {
                    if (inspectOrderDetail.InspectQty !=
                        (inspectOrderDetail.QualifiedQty.HasValue ? inspectOrderDetail.QualifiedQty.Value : 0)
                        + (inspectOrderDetail.RejectedQty.HasValue ? inspectOrderDetail.RejectedQty.Value : 0))
                    {
                        allClose = false;
                        break;
                    }
                }

                if (allClose)
                {
                    inspectOrder.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                }

                this.UpdateInspectOrder(inspectOrder);
            }
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail)
        {
            return this.LoadInspectOrder(inspectNo, includeDetail, false);
        }


        [Transaction(TransactionMode.Unspecified)]
        public InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail, bool isSort)
        {
            InspectOrder inspectOrder = this.LoadInspectOrder(inspectNo);

            if (includeDetail && inspectOrder.InspectOrderDetails != null && inspectOrder.InspectOrderDetails.Count > 0)
            {
                if (isSort)
                {
                    InspectOrderDetailComparer inspectOrderDetailComparer = new InspectOrderDetailComparer();

                    IList<InspectOrderDetail> inspectOrderDetails = inspectOrder.InspectOrderDetails;
                    List<InspectOrderDetail> targetInspectOrderDetails = new List<InspectOrderDetail>();
                    foreach (InspectOrderDetail inspectOrderDetail in inspectOrderDetails)
                    {
                        targetInspectOrderDetails.Add(inspectOrderDetail);
                    }
                    targetInspectOrderDetails.Sort(inspectOrderDetailComparer);
                    inspectOrderDetails.Clear();
                    foreach (InspectOrderDetail targetInspectOrderDetail in targetInspectOrderDetails)
                    {
                        inspectOrderDetails.Add(targetInspectOrderDetail);
                    }
                }
            }
            return inspectOrder;
        }

        #endregion Customized Methods
    }

    class InspectOrderDetailComparer : IComparer<InspectOrderDetail>
    {
        public int Compare(InspectOrderDetail x, InspectOrderDetail y)
        {

            if (x.LocationLotDetail.Item.Code == y.LocationLotDetail.Item.Code
                 && (x.LocationLotDetail.Hu == null
                        || (x.LocationLotDetail.Hu.Uom.Code == y.LocationLotDetail.Hu.Uom.Code
                 && x.LocationLotDetail.Hu.UnitCount == y.LocationLotDetail.Hu.UnitCount))
                 && x.LocationFrom.Code == y.LocationFrom.Code
                 && x.LocationTo.Code == y.LocationTo.Code
                 )
            {
                return 0;
            }
            if (x.LocationLotDetail.Item.Code != y.LocationLotDetail.Item.Code)
            {
                return string.Compare(x.LocationLotDetail.Item.Code, y.LocationLotDetail.Item.Code);
            }
            if (x.LocationLotDetail.Hu != null && x.LocationLotDetail.Hu.Uom.Code != y.LocationLotDetail.Hu.Uom.Code)
            {
                return string.Compare(x.LocationLotDetail.Hu.Uom.Code, y.LocationLotDetail.Hu.Uom.Code);
            }
            if (x.LocationLotDetail.Hu != null && x.LocationLotDetail.Hu.UnitCount != y.LocationLotDetail.Hu.UnitCount)
            {
                return x.LocationLotDetail.Hu.UnitCount.CompareTo(y.LocationLotDetail.Hu.UnitCount);
            }
            if (x.LocationFrom.Code != y.LocationFrom.Code)
            {
                return string.Compare(x.LocationFrom.Code, y.LocationFrom.Code);
            }

            return string.Compare(x.LocationTo.Code, y.LocationTo.Code);


        }
    }


}


#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class InspectOrderMgrE : com.Sconit.Service.MasterData.Impl.InspectOrderMgr, IInspectOrderMgrE
    {

    }


}
#endregion