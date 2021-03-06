using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.Distribution;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using com.Sconit.Service.Criteria;
using System.Linq;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution.Impl
{
    [Transactional]
    public class InProcessLocationMgr : InProcessLocationBaseMgr, IInProcessLocationMgr
    {
        public IInProcessLocationDetailMgrE inProcessLocationDetailMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IInProcessLocationTrackMgrE inProcessLocationTrackMgrE { get; set; }
        public IShipAddressMgrE shipAddressMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IOrderDetailMgrE orderDetailMgrE { get; set; }

        private string[] OrderHead2InProcessLocationCloneField = new string[]
            {
                "PartyFrom",
                "PartyTo",
                "ShipFrom",
                "ShipTo",
                "DockDescription",
                "IsShipScanHu",
                "IsReceiptScanHu",
                "IsAutoReceive",
                "CompleteLatency",
                "GoodsReceiptGapTo",
                "AsnTemplate",
                "ReceiptTemplate",
                "HuTemplate",
                "IsAsnUniqueReceipt"
            };

        private string[] PickList2InProcessLocationCloneField = new string[]
            {
                "OrderType",
                "PartyFrom",
                "PartyTo",
                "ShipFrom",
                "ShipTo",
                "DockDescription",
                "IsShipScanHu",
                "IsReceiptScanHu",
                "IsAutoReceive",
                "CompleteLatency",
                "GoodsReceiptGapTo",
                "AsnTemplate",
                "ReceiptTemplate",
                "HuTemplate",
                "IsAsnUniqueReceipt"
            };

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation GenerateInProcessLocation(OrderHead orderHead)
        {
            InProcessLocation inProcessLocation = new InProcessLocation();

            CloneHelper.CopyProperty(orderHead, inProcessLocation, OrderHead2InProcessLocationCloneField);

            return inProcessLocation;
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateInProcessLocation(InProcessLocation inProcessLocation, User user)
        {
            CreateInProcessLocation(inProcessLocation, user, BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateInProcessLocation(InProcessLocation inProcessLocation, User user, string type)
        {
            if (inProcessLocation.InProcessLocationDetails == null || inProcessLocation.InProcessLocationDetails.Count == 0)
            {
                throw new BusinessErrorException("InProcessLocation.Error.InProcessLocationDetailsEmpty");
            }

            IList<InProcessLocationDetail> targetInProcessLocationDetailList = new List<InProcessLocationDetail>();

            string orderType = null;
            Routing routing = null;
            Party partyFrom = null;
            Party partyTo = null;
            ShipAddress shipFrom = null;
            ShipAddress shipTo = null;
            string dockDescription = null;
            bool? isShipScanHu = null;
            bool isAllShipCreateHu = true;
            bool hasShipCreateHu = false;
            bool? isReceiptScanHu = null;
            string createHuOption = null;
            bool? isAutoReceive = null;
            decimal? completeLatency = null;
            string grGapTo = null;
            string asnTemplate = null;
            string receiptTemplate = null;
            string huTemplate = null;
            bool? needPrintAsn = null;
            bool? isAsnUniqueReceipt = null;
            bool? isGoodsReceiveFIFO = null;
            BillAddress billFrom = null;
            BillAddress billTo = null;

            #region 判断OrderHead的Type、PartyFrom, PartyTo, ShipFrom, ShipTo, DockDescription、Routing, IsReceiptScanHu, CreateHuOption、IsAutoReceipt、CompleteLatency、GrGapTo、AsnTemplate、ReceiptTemplate,HuTemplate是否一致
            foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocation.InProcessLocationDetails)
            {
                if (inProcessLocationDetail.OrderLocationTransaction.BackFlushMethod
                    != BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED)  //过滤掉投料回冲的投入
                {
                    targetInProcessLocationDetailList.Add(inProcessLocationDetail);
                }

                OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                //判断OrderHead的Type是否一致
                if (orderType == null)
                {
                    orderType = orderHead.Type;
                }
                else if (orderHead.Type != orderType)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.OrderTypeNotEqual");
                }

                //判断OrderHead的PartyFrom是否一致
                if (partyFrom == null)
                {
                    partyFrom = orderHead.PartyFrom;
                }
                else if (orderHead.PartyFrom.Code != partyFrom.Code)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.PartyFromNotEqual");
                }

                //判断OrderHead的PartyFrom是否一致
                if (partyTo == null)
                {
                    partyTo = orderHead.PartyTo;
                }
                else if (orderHead.PartyTo.Code != partyTo.Code)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.PartyToNotEqual");
                }

                //判断OrderHead的BillFrom是否一致
                if (billFrom == null)
                {
                    billFrom = orderHead.BillFrom;
                }
                else if (orderHead.BillFrom == null || orderHead.BillFrom.Code != billFrom.Code)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.BillFromNotEqual");
                }
                //判断OrderHead的BillTo是否一致
                if (billTo == null)
                {
                    billTo = orderHead.BillTo;
                }
                else if (orderHead.BillTo == null || orderHead.BillTo.Code != billTo.Code)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.BillToNotEqual");
                }

                //判断OrderHead的ShipFrom是否一致
                if (shipFrom == null)
                {
                    shipFrom = orderHead.ShipFrom;
                }
                else if (!AddressHelper.IsAddressEqual(orderHead.ShipFrom, shipFrom))
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.ShipFromNotEqual");
                }

                //判断OrderHead的ShipTo是否一致
                if (shipTo == null)
                {
                    shipTo = orderHead.ShipTo;
                }
                else if (!AddressHelper.IsAddressEqual(orderHead.ShipTo, shipTo))
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.ShipToNotEqual");
                }

                //判断OrderHead的DockDescription是否一致
                if (dockDescription == null)
                {
                    dockDescription = orderHead.DockDescription;
                }
                else if (orderHead.DockDescription != dockDescription)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.DockDescriptionNotEqual");
                }

                //判断OrderHead的Routing是否一致
                if (routing == null)
                {
                    routing = orderHead.Routing;
                }
                else
                {
                    if (!RoutingHelper.IsRoutingEqual(orderHead.Routing, routing))
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.RoutingNotEqual");
                    }
                }

                //判断OrderHead的IsShipScanHu是否一致
                if (isShipScanHu == null)
                {
                    isShipScanHu = orderHead.IsShipScanHu;
                }
                else if (orderHead.IsShipScanHu != isShipScanHu)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.IsShipScanHuNotEqual");
                }

                if (!isShipScanHu.Value)
                {
                    if (orderHead.CreateHuOption != BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_GI)
                    {
                        isAllShipCreateHu = false;
                    }
                    else
                    {
                        hasShipCreateHu = true;
                    }
                }

                //判断OrderHead的IsReceiptScanHu是否一致
                if (isReceiptScanHu == null)
                {
                    isReceiptScanHu = orderHead.IsReceiptScanHu;
                }
                else if (orderHead.IsReceiptScanHu != isReceiptScanHu)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.IsReceiptScanHuNotEqual");
                }

                //判断OrderHead的CreateHuOption是否一致
                if (createHuOption == null)
                {
                    createHuOption = orderHead.CreateHuOption;
                }
                else
                {
                    if (orderHead.CreateHuOption != createHuOption)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.CreateHuOptionNotEqual");
                    }
                }

                //判断OrderHead的IsAutoReceipt是否一致
                if (isAutoReceive == null)
                {
                    isAutoReceive = orderHead.IsAutoReceive;
                }
                else if (orderHead.IsAutoReceive != isAutoReceive)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.IsAutoReceiveNotEqual");
                }

                //判断OrderHead的NeedPrintAsn是否一致
                if (needPrintAsn == null)
                {
                    needPrintAsn = orderHead.NeedPrintAsn;
                }
                else if (orderHead.NeedPrintAsn != needPrintAsn)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.NeedPrintAsnNotEqual");
                }

                //判断OrderHead的CompleteLatency是否一致
                if (completeLatency == null)
                {
                    completeLatency = orderHead.CompleteLatency;
                }
                else
                {
                    if (orderHead.CompleteLatency.HasValue && orderHead.CompleteLatency != completeLatency)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.CompleteLatencyNotEqual");
                    }
                }

                //判断OrderHead的GoodsReceiptGapTo是否一致
                if (grGapTo == null)
                {
                    grGapTo = orderHead.GoodsReceiptGapTo;
                }
                else
                {
                    if (orderHead.GoodsReceiptGapTo != null && orderHead.GoodsReceiptGapTo != grGapTo)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.GoodsReceiptGapToNotEqual");
                    }
                }

                //判断OrderHead的AsnTemplate是否一致
                if (asnTemplate == null)
                {
                    asnTemplate = orderHead.AsnTemplate;
                }
                else
                {
                    if (orderHead.AsnTemplate != null && orderHead.AsnTemplate != asnTemplate)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.AsnTemplateNotEqual");
                    }
                }

                //判断OrderHead的ReceiptTemplate是否一致
                if (receiptTemplate == null)
                {
                    receiptTemplate = orderHead.ReceiptTemplate;
                }
                else
                {
                    if (orderHead.ReceiptTemplate != null && orderHead.ReceiptTemplate != receiptTemplate)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.ReceiptTemplateNotEqual");
                    }
                }

                //判断OrderHead的HuTemplate是否一致
                if (huTemplate == null)
                {
                    huTemplate = orderHead.HuTemplate;
                }
                else
                {
                    if (orderHead.HuTemplate != null && orderHead.HuTemplate != huTemplate)
                    {
                        throw new BusinessErrorException("Order.Error.ShipOrder.HuTemplateNotEqual");
                    }
                }

                //判断OrderHead的IsAsnUniqueReceipt是否一致
                if (isAsnUniqueReceipt == null)
                {
                    isAsnUniqueReceipt = orderHead.IsAsnUniqueReceipt;
                }
                else if (orderHead.IsAsnUniqueReceipt != isAsnUniqueReceipt)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.IsAsnUniqueReceiptNotEqual");
                }

                //判断OrderHead的PartyFrom是否一致
                if (isGoodsReceiveFIFO == null)
                {
                    isGoodsReceiveFIFO = orderHead.IsGoodsReceiveFIFO;
                }
                else if (isGoodsReceiveFIFO != orderHead.IsGoodsReceiveFIFO)
                {
                    throw new BusinessErrorException("Order.Error.ShipOrder.IsGoodsReceiveFIFONotEqual");
                }
            }

            if (isShipScanHu.HasValue && !isShipScanHu.Value && !isAllShipCreateHu && hasShipCreateHu)
            {
                throw new BusinessErrorException("Order.Error.ShipOrder.NotAllShipCreateHu");
            }
            #endregion

            #region 创建ASN头
            DateTime dateTimeNow = DateTime.Now;

            if (orderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION ||
                orderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
            {
                inProcessLocation.IpNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_ASN);
            }
            else
            {
                inProcessLocation.IpNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_ASN_ELSE);
            }
            inProcessLocation.OrderType = orderType; //
            inProcessLocation.Type = type;
            inProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            inProcessLocation.PartyFrom = partyFrom;
            inProcessLocation.PartyTo = partyTo;
            inProcessLocation.ShipFrom = shipFrom;
            inProcessLocation.ShipTo = shipTo;
            inProcessLocation.DockDescription = dockDescription;
            inProcessLocation.IsShipScanHu = isShipScanHu.HasValue ? isShipScanHu.Value : false;
            inProcessLocation.IsDetailContainHu = (isShipScanHu.HasValue ? isShipScanHu.Value : false) || isAllShipCreateHu;
            inProcessLocation.IsReceiptScanHu = isReceiptScanHu.HasValue ? isReceiptScanHu.Value : false;
            inProcessLocation.IsAutoReceive = isAutoReceive.HasValue ? isAutoReceive.Value : false;
            inProcessLocation.CompleteLatency = completeLatency;
            inProcessLocation.GoodsReceiptGapTo = grGapTo;
            inProcessLocation.AsnTemplate = asnTemplate;
            inProcessLocation.ReceiptTemplate = receiptTemplate;
            inProcessLocation.NeedPrintAsn = needPrintAsn.HasValue ? needPrintAsn.Value : false;
            inProcessLocation.IsAsnUniqueReceipt = isAsnUniqueReceipt.HasValue ? isAsnUniqueReceipt.Value : false;
            inProcessLocation.BillFrom = billFrom;
            inProcessLocation.BillTo = billTo;

            if (orderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION || orderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
            {
                inProcessLocation.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
            }

            inProcessLocation.CreateUser = user;
            inProcessLocation.CreateDate = dateTimeNow;
            inProcessLocation.LastModifyUser = user;
            inProcessLocation.LastModifyDate = dateTimeNow;

            this.CreateInProcessLocation(inProcessLocation);
            #endregion

            inProcessLocation.InProcessLocationDetails = null;   //清空Asn明细，稍后填充
            IList<InventoryTransaction> allInventoryTransactionList = new List<InventoryTransaction>(); //缓存InventoryTransaction，供出库FIFO使用
            if (targetInProcessLocationDetailList != null && targetInProcessLocationDetailList.Count > 0)
            {
                #region HU处理/出库/创建ASN明细
                if (type == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL)
                {
                    IList<LocationLotDetail> inspectLocationLotDetailList = new List<LocationLotDetail>();
                    foreach (InProcessLocationDetail inProcessLocationDetail in targetInProcessLocationDetailList)
                    {
                        OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                        OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                        OrderHead orderHead = orderDetail.OrderHead;

                        if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML
                            && orderHead.CreateHuOption == BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_GI
                            && inProcessLocationDetail.HuId == null)   //如果订单设置为发货时创建Hu，但是发货时已经扫描过Hu了，按已扫描处理
                        {
                            #region 发货时创建Hu
                            if (orderLocationTransaction.Location != null)
                            {
                                //发货出库
                                IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(inProcessLocationDetail, user);
                                IListHelper.AddRange<InventoryTransaction>(allInventoryTransactionList, inventoryTransactionList);
                            }

                            //创建Hu
                            IList<Hu> huList = this.huMgrE.CreateHu(inProcessLocationDetail, user);

                            //按照Hu创建ASN明细
                            this.inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocation, orderLocationTransaction, huList);
                            #endregion
                        }
                        else
                        {
                            #region 发货时不创建Hu

                            #region 更新Hu上的OrderNo
                            if (inProcessLocationDetail.HuId != null && inProcessLocationDetail.HuId != string.Empty)
                            {
                                Hu hu = this.huMgrE.LoadHu(inProcessLocationDetail.HuId);
                                if (hu.OrderNo == null)
                                {
                                    hu.OrderNo = orderHead.OrderNo;
                                    this.huMgrE.UpdateHu(hu);
                                }
                            }
                            #endregion

                            if (orderLocationTransaction.Location != null)
                            {
                                #region 有来源库位
                                //发货出库
                                IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(inProcessLocationDetail, user);
                                IListHelper.AddRange<InventoryTransaction>(allInventoryTransactionList, inventoryTransactionList);

                                //按出库明细创建ASN明细
                                inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocation, orderLocationTransaction, inventoryTransactionList);

                                //销售退货报验, 是否检验
                                if (orderDetail.NeedInspection && orderHead.NeedInspection
                                        && orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                                        && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
                                {
                                    foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                                    {
                                        if (inventoryTransaction.Qty > 0)
                                        {
                                            if (inventoryTransaction.Location.Code != BusinessConstants.SYSTEM_LOCATION_REJECT)
                                            {
                                                LocationLotDetail locationLotDetail = this.locationLotDetailMgrE.LoadLocationLotDetail(inventoryTransaction.LocationLotDetailId);
                                                locationLotDetail.CurrentInspectQty = inventoryTransaction.Qty;
                                                inspectLocationLotDetailList.Add(locationLotDetail);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 没有来源库位
                                //根据发货明细创建ASN明细
                                inProcessLocationDetail.InProcessLocation = inProcessLocation;
                                this.inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocationDetail);
                                inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                                #endregion
                            }
                            #endregion
                        }
                    }

                    #region 检验
                    if (inspectLocationLotDetailList.Count > 0)
                    {
                        //对于没有Hu的，如果收货时已经回冲了负数库存，也就是库存数量和待检验数量不一致可能会有问题
                        this.inspectOrderMgrE.CreateInspectOrder(inspectLocationLotDetailList, user);
                    }
                    #endregion
                }
                else
                {
                    #region 为差异建立明细
                    foreach (InProcessLocationDetail inProcessLocationDetail in targetInProcessLocationDetailList)
                    {
                        inProcessLocationDetail.InProcessLocation = inProcessLocation;
                        this.inProcessLocationDetailMgrE.CreateInProcessLocationDetail(inProcessLocationDetail);
                        inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                    }
                    #endregion
                }
                #endregion
            }

            #region 检验发货FIFO，暂时不考虑拣货单占用库存
            if (isGoodsReceiveFIFO.HasValue && isGoodsReceiveFIFO.Value
                && allInventoryTransactionList.Count > 0)
            {
                //allInventoryTransactionList
                var query = from a in allInventoryTransactionList
                            where a.Hu != null
                            group a by new
                            {
                                LocationCode = a.Location.Code,
                                ItemCode = a.Item.Code
                            } into g
                            select new
                            {
                                LocationCode = g.Key.LocationCode,
                                ItemCode = g.Key.ItemCode,
                                list = g.ToList()
                            };

                if (query != null && query.Count() > 0)
                {
                    foreach (var q in query)
                    {
                        InventoryTransaction inventoryTransaction = q.list.OrderByDescending(i => i.Hu.ManufactureDate).FirstOrDefault();
                        IList<string> huIdList = q.list.Where(i => i.Hu.ManufactureDate < inventoryTransaction.Hu.ManufactureDate).Select(i => i.Hu.HuId).ToList();

                        if (!this.locationLotDetailMgrE.CheckGoodsIssueFIFO(inventoryTransaction.Location.Code, inventoryTransaction.Item.Code, inventoryTransaction.Hu.ManufactureDate, huIdList))
                        {
                            throw new BusinessErrorException("MasterData.InventoryOut.LotNoIsOld",
                                    inventoryTransaction.Item.Code,
                                    inventoryTransaction.Hu.HuId,
                                    inventoryTransaction.Hu.LotNo,
                                    inventoryTransaction.Location.Code);
                        }
                    }
                }
            }
            #endregion

            #region 创建ASN追踪
            if (orderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                && routing != null)
            {
                IList<InProcessLocationTrack> inProcessLocationTrackList =
                    this.inProcessLocationTrackMgrE.CreateIInProcessLocationTrack(inProcessLocation, routing);

                #region 设置默认进入第一个Activity
                if (inProcessLocationTrackList != null && inProcessLocationTrackList.Count > 0)
                {
                    IListHelper.Sort<InProcessLocationTrack>(inProcessLocationTrackList, "Operation");

                    inProcessLocation.CurrentOperation = inProcessLocationTrackList[0].Operation;
                    inProcessLocation.CurrentActivity = inProcessLocationTrackList[0].Activity;

                    this.UpdateInProcessLocation(inProcessLocation);
                }
                #endregion
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseInProcessLocation(InProcessLocation inProcessLocation, User user)
        {
            CloseInProcessLocation(inProcessLocation, user, true);
        }

        [Transaction(TransactionMode.Requires)]
        public void CloseInProcessLocation(InProcessLocation inProcessLocation, User user, bool handleGap)
        {

            InProcessLocation oldInProcessLocation = this.LoadInProcessLocation(inProcessLocation.IpNo, true);

            if (oldInProcessLocation.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                throw new BusinessErrorException("InProcessLocation.Error.StatusErrorWhenClose", oldInProcessLocation.Status, inProcessLocation.IpNo);
            }

            oldInProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            oldInProcessLocation.LastModifyDate = DateTime.Now;
            oldInProcessLocation.LastModifyUser = user;
            oldInProcessLocation.ReferenceOrderNo = inProcessLocation.ReferenceOrderNo;
            oldInProcessLocation.Disposition = inProcessLocation.Disposition;

            this.UpdateInProcessLocation(oldInProcessLocation);

            #region 查找差异
            if (handleGap && oldInProcessLocation.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                IList<InProcessLocationDetail> gapInProcessLocationDetailList = new List<InProcessLocationDetail>();
                foreach (InProcessLocationDetail inProcessLocationDetail in oldInProcessLocation.InProcessLocationDetails)
                {
                    if (!inProcessLocationDetail.ReceivedQty.HasValue || ((inProcessLocationDetail.Qty > 0) && (inProcessLocationDetail.Qty != inProcessLocationDetail.ReceivedQty)))
                    {
                        InProcessLocationDetail gapInProcessLocationDetail = new InProcessLocationDetail();
                        if (inProcessLocationDetail.ReceivedQty.HasValue)
                        {
                            gapInProcessLocationDetail.Qty = inProcessLocationDetail.ReceivedQty.Value - inProcessLocationDetail.Qty;
                        }
                        else
                        {
                            gapInProcessLocationDetail.Qty = 0 - inProcessLocationDetail.Qty;
                        }
                        gapInProcessLocationDetail.OrderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                        gapInProcessLocationDetail.LotNo = inProcessLocationDetail.LotNo;
                        gapInProcessLocationDetail.IsConsignment = inProcessLocationDetail.IsConsignment;
                        gapInProcessLocationDetail.PlannedBill = inProcessLocationDetail.PlannedBill;
                        gapInProcessLocationDetailList.Add(gapInProcessLocationDetail);
                    }
                }
                this.RecordInProcessLocationGap(gapInProcessLocationDetailList, oldInProcessLocation.GoodsReceiptGapTo, user);
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseInProcessLocation(InProcessLocation inProcessLocation, User user)
        {
            InProcessLocation oldInProcessLocation = this.LoadInProcessLocation(inProcessLocation.IpNo, true);

            #region 查找差异
            IList<InProcessLocationDetail> gapInProcessLocationDetailList = new List<InProcessLocationDetail>();
            bool allClose = true;
            if (oldInProcessLocation.InProcessLocationDetails != null && oldInProcessLocation.InProcessLocationDetails.Count > 0)
            {
                foreach (InProcessLocationDetail inProcessLocationDetail in oldInProcessLocation.InProcessLocationDetails)
                {
                    if (!inProcessLocationDetail.ReceivedQty.HasValue || Math.Abs(inProcessLocationDetail.Qty) > Math.Abs(inProcessLocationDetail.ReceivedQty.Value))
                    {
                        //有未收满的IpDetail
                        allClose = false;
                        break;
                    }
                    else if (inProcessLocationDetail.ReceivedQty.HasValue && Math.Abs(inProcessLocationDetail.Qty) < Math.Abs(inProcessLocationDetail.ReceivedQty.Value))
                    {
                        //超收，记录差异
                        InProcessLocationDetail gapInProcessLocationDetail = new InProcessLocationDetail();

                        gapInProcessLocationDetail.Qty = inProcessLocationDetail.ReceivedQty.Value - inProcessLocationDetail.Qty;
                        gapInProcessLocationDetail.OrderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                        gapInProcessLocationDetail.LotNo = inProcessLocationDetail.LotNo;
                        gapInProcessLocationDetail.IsConsignment = inProcessLocationDetail.IsConsignment;
                        gapInProcessLocationDetail.PlannedBill = inProcessLocationDetail.PlannedBill;
                        gapInProcessLocationDetailList.Add(gapInProcessLocationDetail);
                    }
                }
            }

            if (allClose)
            {
                oldInProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                oldInProcessLocation.LastModifyDate = DateTime.Now;
                oldInProcessLocation.LastModifyUser = user;
                oldInProcessLocation.ReferenceOrderNo = inProcessLocation.ReferenceOrderNo;
                oldInProcessLocation.Disposition = inProcessLocation.Disposition;

                this.UpdateInProcessLocation(oldInProcessLocation);
                this.RecordInProcessLocationGap(gapInProcessLocationDetailList, oldInProcessLocation.GoodsReceiptGapTo, user);
            }
            else
            {
                oldInProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
                oldInProcessLocation.LastModifyDate = DateTime.Now;
                oldInProcessLocation.LastModifyUser = user;
                oldInProcessLocation.ReferenceOrderNo = inProcessLocation.ReferenceOrderNo;
                oldInProcessLocation.Disposition = inProcessLocation.Disposition;

                this.UpdateInProcessLocation(oldInProcessLocation);
            }
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation LoadInProcessLocation(string ipNo, string userCode)
        {
            return this.LoadInProcessLocation(ipNo, this.userMgrE.CheckAndLoadUser(userCode), false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation LoadInProcessLocation(string ipNo, User user)
        {
            return this.LoadInProcessLocation(ipNo, user, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation LoadInProcessLocation(string ipNo, string userCode, bool includeDetail)
        {
            return this.LoadInProcessLocation(ipNo, this.userMgrE.CheckAndLoadUser(userCode), includeDetail);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation LoadInProcessLocation(string ipNo, User user, bool includeDetail)
        {
            InProcessLocation inProcessLocation = this.LoadInProcessLocation(ipNo, true);
            this.CheckAsnOperationAuthrize(inProcessLocation, user, new List<string>());
            if (includeDetail)
            {
                if (inProcessLocation.InProcessLocationDetails != null
                    && inProcessLocation.InProcessLocationDetails.Count > 0)
                {
                }
            }
            return inProcessLocation;
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation LoadInProcessLocation(String ipNo, bool includeDetail)
        {
            InProcessLocation inProcessLocation = this.LoadInProcessLocation(ipNo);

            if (inProcessLocation == null)
            {
                throw new BusinessErrorException("InProcessLocation.Error.IpNoExists", ipNo);
            }
            if (includeDetail && inProcessLocation.InProcessLocationDetails != null && inProcessLocation.InProcessLocationDetails.Count > 0)
            {

            }
            return inProcessLocation;
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateInProcessLocation(InProcessLocation ip, int op, User currentUser)
        {
            UpdateInProcessLocation(ip.IpNo, op, currentUser);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateInProcessLocation(string ipNo, int op, string userCode)
        {
            UpdateInProcessLocation(ipNo, op, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateInProcessLocation(InProcessLocation ip, int op, string userCode)
        {
            UpdateInProcessLocation(ip.IpNo, op, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateInProcessLocation(string ipNo, int op, User currentUser)
        {
            InProcessLocation ip = this.LoadInProcessLocation(ipNo);
            if (this.CheckAsnOperationAuthrize(ip, currentUser, new List<string>()))
            {

                IList<InProcessLocationTrack> ipTrackList = inProcessLocationTrackMgrE.GetInProcessLocationTrack(ip.IpNo, op);
                if (ipTrackList.Count > 0)
                {
                    InProcessLocationTrack ipTrack = ipTrackList[0];
                    ip.CurrentOperation = op;
                    ip.CurrentActivity = ipTrack.Activity;
                    ip.LastModifyDate = DateTime.Now;
                    ip.LastModifyUser = currentUser;
                    base.UpdateInProcessLocation(ip);

                    ipTrack.ActiveDate = DateTime.Now;
                    ipTrack.ActiveUser = currentUser;
                    inProcessLocationTrackMgrE.UpdateInProcessLocationTrack(ipTrack);

                }
            }

        }


        [Transaction(TransactionMode.Requires)]
        public void UpdateInProcessLocation(string ipNo, string deliverType, string transportCompany, string expressNo, string deliverDate, User currentUser)
        {
            InProcessLocation ip = this.LoadInProcessLocation(ipNo);
            if (this.CheckAsnOperationAuthrize(ip, currentUser, new List<string>()))
            {
                if (expressNo != null && expressNo.Trim().Length > 0)
                {
                    ip.ExpressNo = expressNo;
                }

                if (deliverType != null && deliverType.Trim().Length > 0)
                {
                    ip.DeliverType = deliverType.Trim();
                }

                if (transportCompany != null && transportCompany.Trim().Length > 0)
                {
                    ip.TransportCompany = transportCompany.Trim();
                }

                if (deliverDate != null && deliverDate.Trim().Length > 0)
                {
                    ip.DeliverDate = DateTime.Parse(deliverDate);
                }

                ip.LastModifyDate = DateTime.Now;
                ip.LastModifyUser = currentUser;
                base.UpdateInProcessLocation(ip);


            }

        }


        [Transaction(TransactionMode.Requires)]
        public void ResolveInPorcessLocationGap(InProcessLocation inProcessLocation, string grGapTo, User user)
        {
            InProcessLocation gapInProcessLocation = this.LoadInProcessLocation(inProcessLocation.IpNo, true);

            #region 检查类型和状态
            if (gapInProcessLocation.Type != BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP)
            {
                throw new BusinessErrorException("InProcessLocation.Error.ResolveGap.TypeError", gapInProcessLocation.IpNo, gapInProcessLocation.Type);
            }

            if (gapInProcessLocation.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                throw new BusinessErrorException("InProcessLocation.Error.ResolveGap.StatusError", gapInProcessLocation.IpNo, gapInProcessLocation.Status);
            }
            #endregion

            foreach (InProcessLocationDetail gapInProcessLocationDetail in gapInProcessLocation.InProcessLocationDetails)
            {
                if (grGapTo == BusinessConstants.CODE_MASTER_GR_GAP_TO_GI)
                {
                    #region 调整发货方库存
                    OrderLocationTransaction orderLocationTransaction = gapInProcessLocationDetail.OrderLocationTransaction;
                    if (orderLocationTransaction.Location != null)
                    {
                        //发货出库
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(gapInProcessLocationDetail, user);
                    }
                    #endregion

                    #region 关闭差异
                    gapInProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    gapInProcessLocation.LastModifyDate = DateTime.Now;
                    gapInProcessLocation.LastModifyUser = user;

                    this.UpdateInProcessLocation(gapInProcessLocation);
                    #endregion
                }
                else
                {
                    throw new TechnicalException("unspecified GRGapTo " + grGapTo);
                }
            }
        }


        [Transaction(TransactionMode.Requires)]
        public void ResolveInPorcessLocationNormal(InProcessLocation inProcessLocation, string grGapTo, User user)
        {
            InProcessLocation nmlInProcessLocation = this.LoadInProcessLocation(inProcessLocation.IpNo, true);

            #region 检查类型和状态
            if (nmlInProcessLocation.Type != BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL)
            {
                throw new BusinessErrorException("InProcessLocation.Error.ResolveNormal.TypeError", nmlInProcessLocation.IpNo, nmlInProcessLocation.Type);
            }

            if (nmlInProcessLocation.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                throw new BusinessErrorException("InProcessLocation.Error.ResolveGap.StatusError", nmlInProcessLocation.IpNo, nmlInProcessLocation.Status);
            }
            #endregion

            foreach (InProcessLocationDetail nmlInProcessLocationDetail in nmlInProcessLocation.InProcessLocationDetails)
            {
                if (grGapTo == BusinessConstants.CODE_MASTER_GR_GAP_TO_GI)
                {
                    #region 调整发货方库存
                    OrderLocationTransaction orderLocationTransaction = nmlInProcessLocationDetail.OrderLocationTransaction;
                    if (orderLocationTransaction.OrderDetail.DefaultLocationFrom != null)
                    {
                        //发货出库,负数
                        decimal receivedQty = nmlInProcessLocationDetail.ReceivedQty.HasValue ? (decimal)nmlInProcessLocationDetail.ReceivedQty.Value : 0;
                        decimal backQty = nmlInProcessLocationDetail.Qty - (nmlInProcessLocationDetail.ReceivedQty.HasValue ? (decimal)nmlInProcessLocationDetail.ReceivedQty.Value : 0);
                        nmlInProcessLocationDetail.Qty = 0 - backQty;
                        IList<InventoryTransaction> inventoryTransactionList = this.locationMgrE.InventoryOut(nmlInProcessLocationDetail, user);

                        //退回数量
                        nmlInProcessLocationDetail.Qty = receivedQty;
                        inProcessLocationDetailMgrE.UpdateInProcessLocationDetail(nmlInProcessLocationDetail);

                        OrderLocationTransaction oldOrderLocationTransaction = orderLocationTransactionMgrE.LoadOrderLocationTransaction(orderLocationTransaction.Id);
                        oldOrderLocationTransaction.AccumulateQty -= backQty;
                        orderLocationTransactionMgrE.UpdateOrderLocationTransaction(oldOrderLocationTransaction);

                        OrderDetail oldOrderDetail = orderDetailMgrE.LoadOrderDetail(oldOrderLocationTransaction.OrderDetail.Id);
                        oldOrderDetail.ShippedQty -= backQty / oldOrderLocationTransaction.UnitQty;
                        orderDetailMgrE.UpdateOrderDetail(oldOrderDetail);
                    }
                    #endregion

                    #region 关闭ASN
                    nmlInProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    nmlInProcessLocation.LastModifyDate = DateTime.Now;
                    nmlInProcessLocation.LastModifyUser = user;

                    this.UpdateInProcessLocation(nmlInProcessLocation);
                    #endregion
                }
                else
                {
                    throw new TechnicalException("unspecified GRGapTo " + grGapTo);
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation CheckAndLoadInProcessLocation(string ipNo)
        {
            InProcessLocation inProcessLocation = this.LoadInProcessLocation(ipNo, true);
            if (inProcessLocation == null)
            {
                throw new BusinessErrorException("Common.Business.Error.EntityNotExist", ipNo);
            }
            return inProcessLocation;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocation> GetInProcessLocation(string userCode, int firstRow, int maxRows, params string[] orderTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(InProcessLocation));
            criteria.Add(Expression.Or(
                Expression.Eq("CreateUser.Code", userCode),
                Expression.Eq("LastModifyUser.Code", userCode)));
            if (orderTypes.Length == 1)
            {
                criteria.Add(Expression.Eq("OrderType", orderTypes[0]));
            }
            else
            {
                criteria.Add(Expression.In("OrderType", orderTypes));
            }
            criteria.Add(Expression.Ge("CreateDate", DateTime.Today));
            criteria.AddOrder(Order.Desc("IpNo"));
            IList<InProcessLocation> inProcessLocationList = criteriaMgrE.FindAll<InProcessLocation>(criteria, firstRow, maxRows);
            if (inProcessLocationList.Count > 0)
            {
                return inProcessLocationList;
            }
            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public void ApproveInPorcessLocation(InProcessLocation inProcessLocation, string userCode)
        {
            ApproveInPorcessLocation(inProcessLocation, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Unspecified)]
        public void ApproveInPorcessLocation(InProcessLocation inProcessLocation, User user)
        {
            ApproveInPorcessLocation(inProcessLocation.IpNo, user);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void ApproveInPorcessLocation(string ipNo, string userCode)
        {
            ApproveInPorcessLocation(ipNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Unspecified)]
        public void ApproveInPorcessLocation(string ipNo, User user)
        {
            InProcessLocation inProcessLocation = this.LoadInProcessLocation(ipNo);
            inProcessLocation.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
          //  inProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
            inProcessLocation.ApprovedDate = DateTime.Now;
            inProcessLocation.ApprovedUser = user;
            inProcessLocation.LastModifyDate = DateTime.Now;
            inProcessLocation.LastModifyUser = user;

            //判断自动收货
            if (inProcessLocation.IsAutoReceive)
            {
                inProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            }
            else
            {
                inProcessLocation.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
            }

            this.UpdateInProcessLocation(inProcessLocation);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void RejectInPorcessLocation(InProcessLocation inProcessLocation, string userCode)
        {
            RejectInPorcessLocation(inProcessLocation.IpNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Unspecified)]
        public void RejectInPorcessLocation(InProcessLocation inProcessLocation, User user)
        {
            RejectInPorcessLocation(inProcessLocation.IpNo, user);
        }

        [Transaction(TransactionMode.Unspecified)]
        public void RejectInPorcessLocation(string ipNo, string userCode)
        {
            RejectInPorcessLocation(ipNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Unspecified)]
        public void RejectInPorcessLocation(string ipNo, User user)
        {
            InProcessLocation inProcessLocation = this.LoadInProcessLocation(ipNo);
            if (inProcessLocation.Type == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP)
            {
                this.ResolveInPorcessLocationGap(inProcessLocation, BusinessConstants.CODE_MASTER_GR_GAP_TO_GI, user);
            }
            else if (inProcessLocation.Type == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_NORMAL)
            {
                this.ResolveInPorcessLocationNormal(inProcessLocation, BusinessConstants.CODE_MASTER_GR_GAP_TO_GI, user);
            }

            inProcessLocation.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_REJECTED;
            inProcessLocation.LastModifyDate = DateTime.Now;
            inProcessLocation.LastModifyUser = user;
            this.UpdateInProcessLocation(inProcessLocation);
        }


        #endregion Customized Methods

        #region Private Methods
        private bool CheckAsnOperationAuthrize(InProcessLocation inProcessLocation, User user)
        {
            IList<string> asnOperationList = new List<string>();
            return CheckAsnOperationAuthrize(inProcessLocation, user, asnOperationList);
        }

        private bool CheckAsnOperationAuthrize(InProcessLocation inProcessLocation, User user, IList<string> asnOperationList)
        {
            bool partyFromAuthrized = false;
            //bool partyToAuthrized = false;
            int asnOperationAuthrizedQty = 0;
            foreach (Permission permission in user.Permissions)
            {
                if (permission.Code == inProcessLocation.PartyFrom.Code)
                {
                    partyFromAuthrized = true;
                }

                //if (permission.Code == inProcessLocation.PartyTo.Code)
                //{
                //    partyToAuthrized = true;
                //}

                foreach (string asnOperation in asnOperationList)
                {
                    if (permission.Code == asnOperation)
                    {
                        asnOperationAuthrizedQty++;
                        break;
                    }
                }

                //if (partyFromAuthrized && partyToAuthrized && (asnOperationAuthrizedQty == asnOperationList.Count))
                if (partyFromAuthrized && (asnOperationAuthrizedQty == asnOperationList.Count))
                {
                    break;
                }
            }

            //if (!(partyFromAuthrized && partyToAuthrized))
            if (!partyFromAuthrized)
            {
                //没有该asn的操作权限
                if (inProcessLocation.IpNo != null)
                {
                    throw new BusinessErrorException("Asn.Error.NoAuthrization", inProcessLocation.IpNo);
                }
                else
                {
                    throw new BusinessErrorException("Asn.Error.NoCreatePermission2", inProcessLocation.ShipFrom.Party.Code, inProcessLocation.ShipTo.Party.Code);
                }
            }

            return (asnOperationAuthrizedQty == asnOperationList.Count);
        }

        private void RecordInProcessLocationGap(IList<InProcessLocationDetail> gapInProcessLocationDetailList, string grGapTo, User user)
        {
            #region 处理收货差异
            if (gapInProcessLocationDetailList != null && gapInProcessLocationDetailList.Count > 0)
            {
                InProcessLocation gapInProcessLocation = new InProcessLocation();
                gapInProcessLocation.InProcessLocationDetails = gapInProcessLocationDetailList;

                if (grGapTo == BusinessConstants.CODE_MASTER_GR_GAP_TO_IPGAP)
                {
                    #region 记录IP差异
                    gapInProcessLocation.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
                    this.CreateInProcessLocation(gapInProcessLocation, user, BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP);
                    #endregion
                }
                else if (grGapTo == BusinessConstants.CODE_MASTER_GR_GAP_TO_GI)
                {
                    #region 调整发货方数量
                    this.CreateInProcessLocation(gapInProcessLocation, user, BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP);

                    this.ResolveInPorcessLocationGap(gapInProcessLocation, BusinessConstants.CODE_MASTER_GR_GAP_TO_GI, user);

                    //更新OrderDetail和OrderLocationTransaction的发货信息
                    foreach (InProcessLocationDetail gapInProcessLocationDetail in gapInProcessLocationDetailList)
                    {
                        this.orderDetailMgrE.RecordOrderShipQty(gapInProcessLocationDetail.OrderLocationTransaction, gapInProcessLocationDetail, false);
                    }
                    #endregion
                }
                else
                {
                    throw new TechnicalException("unspecified GRGapTo " + grGapTo);
                }
            }
            #endregion
        }

        private void FindLocItemMaxManufactureDate(IList<InventoryTransaction> inventoryTransactionList, IDictionary<string, InventoryTransaction> locItemDateDic)
        {
            if (inventoryTransactionList != null && inventoryTransactionList.Count > 0)
            {
                foreach (InventoryTransaction inventoryTransaction in inventoryTransactionList)
                {
                    if (inventoryTransaction.Hu != null)
                    {
                        string locItem = inventoryTransaction.Location.Code + "$$$" + inventoryTransaction.Item.Code;

                        if (!locItemDateDic.ContainsKey(locItem))
                        {
                            locItemDateDic.Add(locItem, inventoryTransaction);
                        }
                        else if (locItemDateDic[locItem].Hu.ManufactureDate.CompareTo(inventoryTransaction.Hu.ManufactureDate) < 0)
                        {
                            locItemDateDic[locItem] = inventoryTransaction;
                        }
                    }
                }
            }
        }
        #endregion
    }
}



#region Extend Interface

namespace com.Sconit.Service.Ext.Distribution.Impl
{
    [Transactional]
    public partial class InProcessLocationMgrE : com.Sconit.Service.Distribution.Impl.InProcessLocationMgr, IInProcessLocationMgrE
    {

    }
}
#endregion