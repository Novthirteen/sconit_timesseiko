using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.Production;
using com.Sconit.Service.Distribution;
using com.Sconit.Service.Procurement;
using com.Sconit.Service.Production;
using com.Sconit.Utility;
using com.Sconit.Service.Criteria;
using NHibernate.Expression;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Type;
using System.Linq;
namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderMgr : IOrderMgr
    {
        #region 变量
        public IUserMgrE userMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IOrderDetailMgrE orderDetailMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IOrderOperationMgrE orderOperationMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IFlowDetailMgrE flowDetailMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }
        public IInProcessLocationDetailMgrE inProcessLocationDetailMgrE { get; set; }
        public IAutoOrderTrackMgrE autoOrderTrackMgrE { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IFlowBindingMgrE flowBindingMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IItemKitMgrE itemKitMgrE { get; set; }
        public IWorkingHoursMgrE workingHoursMgrE { get; set; }
        public IOrderBindingMgrE orderBindingMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IPickListResultMgrE pickListResultMgrE { get; set; }
        public IShiftMgrE shiftMgrE { get; set; }
        public IOrderPlannedBackflushMgrE orderPlannedBackflushMgrE { get; set; }
        public IActingBillMgrE actingBillMgrE { get; set; }
        public IPlannedBillMgrE plannedBillMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IPriceListDetailMgrE priceListDetailMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IOrderTracerMgrE orderTracerMgrE { get; set; }

        private string[] FlowHead2OrderHeadCloneFields = new string[] 
            { 
                "Type",
                "PartyFrom",
                "PartyTo",
                "ShipFrom",
                "ShipTo",
                "LocationFrom",
                "LocationTo",
                "BillFrom",
                "BillTo",
                "PriceList",
                "DockDescription",
                "Carrier",
                "CarrierBillAddress",
                "Routing",
                "IsAutoRelease",
                "IsAutoStart",
                "IsAutoShip",
                "IsAutoReceive",
                "IsAutoBill",
                "StartLatency",
                "CompleteLatency",
                "NeedPrintOrder",
                "NeedPrintAsn",
                "NeedPrintReceipt",
                "GoodsReceiptGapTo",
                "AllowExceed",
                "AllowCreateDetail",
                "OrderTemplate",
                "AsnTemplate",
                "ReceiptTemplate",
                "HuTemplate",
                "CheckDetailOption",
                "Currency",
                "IsShowPrice",
                "BillSettleTerm",
                "FulfillUnitCount",
                "IsShipScanHu",
                "IsReceiptScanHu",
                "AutoPrintHu",
                "IsOddCreateHu",
                "CreateHuOption",
                "IsAutoCreatePickList",
                "NeedInspection",
                "IsGoodsReceiveFIFO",
                "AntiResolveHu",
                "MaxOnlineQty",
                "AllowRepeatlyExceed",
                "IsPickFromBin",
                "IsShipByOrder",
                "IsAsnUniqueReceipt",
                "Settlement"
            };

        private string[] OrderHead2OrderHeadCloneFields = new string[] 
            { 
                "ReferenceOrderNo",
                "ExternalOrderNo",
                "Sequence",
                "StartTime",
                "WindowTime",
                "Priority",
                "Type",
                "PartyFrom",
                "PartyTo",
                "ShipFrom",
                "ShipTo",
                "LocationFrom",
                "LocationTo",
                "BillFrom",
                "BillTo",
                "PriceList",
                "DockDescription",
                "Carrier",
                "CarrierBillAddress",
                "Routing",
                "IsAutoRelease",
                "IsAutoStart",
                "IsAutoShip",
                "IsAutoReceive",
                "IsAutoBill",
                "StartLatency",
                "CompleteLatency",
                "NeedPrintOrder",
                "NeedPrintAsn",
                "NeedPrintReceipt",
                "GoodsReceiptGapTo",
                "AllowExceed",
                "AllowCreateDetail",
                "OrderTemplate",
                "AsnTemplate",
                "ReceiptTemplate",
                "HuTemplate",
                "CancelReason",
                "Memo",
                "CheckDetailOption",
                "Currency",
                "IsShowPrice",
                "Discount",
                "BillSettleTerm",
                "FulfillUnitCount",
                "IsShipScanHu",
                "IsReceiptScanHu",
                "AutoPrintHu",
                "IsOddCreateHu",
                "CreateHuOption",
                "IsAutoCreatePickList",
                "NeedInspection",
                "IsGoodsReceiveFIFO",
                "AntiResolveHu",
                "MaxOnlineQty",
                "AllowRepeatlyExceed",
                "IsPickFromBin",
                "IsShipByOrder",
                "IsAsnUniqueReceipt",
                "ApprovalStatus",
                "Settlement",
                "ConfirmOrderNo",
                "Customer",
                "RelatedOrderNo"
            };
        #endregion



        #region IOrderMgr接口实现
        [Transaction(TransactionMode.Unspecified)]
        public OrderHead LoadOrder(string orderNo, string userCode)
        {
            return this.LoadOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead LoadOrder(string orderNo, User user)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            //OrderHelper.CheckOrderOperationAuthrize(orderHead, user, new List<string>());
            return orderHead;
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrder(OrderHead orderHead, string userCode)
        {
            UpdateOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrder(OrderHead orderHead, string userCode, bool updateDetail)
        {
            UpdateOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode), updateDetail);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrder(OrderHead orderHead, User user)
        {
            UpdateOrder(orderHead, user, false);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrder(OrderHead orderHead, User user, bool updateDetail)
        {
            OrderHead oldOrderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderHead.OrderNo);
            //if (!OrderHelper.CheckOrderOperationAuthrize(oldOrderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoEditPermission", orderHead.OrderNo);
            //}

            if (oldOrderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                bool isReGenerateOrderLocTrans = false;
                bool isReGenerateOrderOperation = false;

                if (updateDetail
                    && (!EntityHelper.EntityPropertyEquals(oldOrderHead, orderHead, "LocationFrom.Code")
                    || !EntityHelper.EntityPropertyEquals(oldOrderHead, orderHead, "LocationTo.Code")))
                {
                    //订单头的LocationFrom或LocationTo发生变化，重新生成OrderLocTrans
                    isReGenerateOrderLocTrans = true;
                }

                if (!EntityHelper.EntityPropertyEquals(oldOrderHead, orderHead, "Routing.Code"))
                {
                    ////订单头的Routing发生变化，重新生成OrderOperation
                    isReGenerateOrderOperation = true;
                }

                //同步OrderHead，以后都用oldOrderHead操作，不然会造成Session中包含同两个相同对象的错误
                CloneHelper.CopyProperty(orderHead, oldOrderHead, OrderHead2OrderHeadCloneFields); //复制订单的字段
                oldOrderHead.OrderDetails = orderHead.OrderDetails;

                if (isReGenerateOrderLocTrans)
                {
                    //todo
                }

                if (isReGenerateOrderOperation)
                {
                    //todo
                }

                if (updateDetail)
                {
                    //更新订单明细数量,折扣
                    UpdateOrderQty(oldOrderHead.OrderDetails, user);
                }
                else
                {
                    orderHead.LastModifyDate = DateTime.Now;
                    orderHead.LastModifyUser = user;
                }

                this.orderHeadMgrE.UpdateOrderHead(oldOrderHead);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", oldOrderHead.Status, oldOrderHead.OrderNo);
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode)
        {
            return TransferFlow2Order(flowCode, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, false, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode, string orderSubType)
        {
            return TransferFlow2Order(flowCode, orderSubType, false, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow)
        {
            return TransferFlow2Order(flow, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, false, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow, string orderSubType)
        {
            return TransferFlow2Order(flow, orderSubType, false, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode, bool isGenerateOrderSubsidiary)
        {
            return TransferFlow2Order(flowCode, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, isGenerateOrderSubsidiary, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode, string orderSubType, bool isGenerateOrderSubsidiary)
        {
            return TransferFlow2Order(flowCode, orderSubType, isGenerateOrderSubsidiary, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow, bool isGenerateOrderSubsidiary)
        {
            return TransferFlow2Order(flow, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, isGenerateOrderSubsidiary, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow, string orderSubType, bool isGenerateOrderSubsidiary)
        {
            return TransferFlow2Order(flow, orderSubType, isGenerateOrderSubsidiary, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode, bool isGenerateOrderSubsidiary, DateTime startTime)
        {
            return TransferFlow2Order(flowCode, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, isGenerateOrderSubsidiary, startTime);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(string flowCode, string orderSubType, bool isGenerateOrderSubsidiary, DateTime startTime)
        {
            Flow flow = this.flowMgrE.LoadFlow(flowCode);
            return TransferFlow2Order(flow, orderSubType, isGenerateOrderSubsidiary, startTime);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow, bool isGenerateOrderSubsidiary, DateTime startTime)
        {
            return TransferFlow2Order(flow, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML, isGenerateOrderSubsidiary, startTime);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead TransferFlow2Order(Flow flow, string orderSubType, bool isGenerateOrderSubsidiary, DateTime startTime)
        {
            #region 创建OrderHead
            OrderHead orderHead = new OrderHead();
            CloneHelper.CopyProperty(flow, orderHead, FlowHead2OrderHeadCloneFields);
            orderHead.SubType = orderSubType;
            if (orderHead.PriceList != null)
            {
                orderHead.IsIncludeTax = orderHead.PriceList.IsIncludeTax;
            }
            orderHead.TaxCode = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_TAX_RATE).Value;
            if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO
                && flow.ReturnRouting != null)     //返工，使用ReturnRouting
            {
                orderHead.Routing = flow.ReturnRouting;
            }
            //if (orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN
            //    || orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
            //{
            //    //退货和次品库位设置
            //    if (flow.LocationFrom != null && flow.LocationFrom.ActingLocation != null)
            //    {
            //        orderHead.LocationFrom = flow.LocationFrom.ActingLocation;
            //    }

            //    if (flow.LocationTo != null && flow.LocationTo.ActingLocation != null)
            //    {
            //        orderHead.LocationTo = flow.LocationTo.ActingLocation;
            //    }
            //}
            if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
            {
                orderHead.LocationTo = this.locationMgrE.GetRejectLocation();
            }
            orderHead.StartTime = startTime;
            orderHead.Flow = flow.Code;
            orderHead.SubType = orderSubType;
            orderHead.Priority = orderHead.Priority == null ? BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL : orderHead.Priority;
            #endregion

            #region 创建OrderDetail
            IList<FlowDetail> flowDetaiList = new List<FlowDetail>();
            if (flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
                IListHelper.AddRange<FlowDetail>(flowDetaiList, flow.FlowDetails);
            }

            //根据引用路线创建订单明细
            if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                Flow referenceFlow = this.flowMgrE.LoadFlow(flow.ReferenceFlow);
                if (flow.Routing != null && referenceFlow.Routing != null
                    && !flow.Routing.Equals(referenceFlow.Routing))
                {
                    throw new BusinessErrorException("Flow.Error.ReferenceFlowRoutingNotEqual", flow.Code, referenceFlow.Code);
                }

                if (referenceFlow.FlowDetails != null && referenceFlow.FlowDetails.Count > 0)
                {
                    IListHelper.AddRange<FlowDetail>(flowDetaiList, referenceFlow.FlowDetails);
                }
            }

            //先根据序号排序，序号在生成订单的时候会重新生成
            flowDetaiList = IListHelper.Sort<FlowDetail>(flowDetaiList, "Sequence");
            foreach (FlowDetail flowDetail in flowDetaiList)
            {
                if (orderHead.Flow == flowDetail.Flow.Code)
                {
                    this.orderDetailMgrE.GenerateOrderDetail(orderHead, flowDetail, false);
                }
                else
                {
                    this.orderDetailMgrE.GenerateOrderDetail(orderHead, flowDetail, true);
                }
            }
            #endregion

            if (isGenerateOrderSubsidiary)
            {
                orderHead.StartTime = startTime;
                this.orderHeadMgrE.GenerateOrderHeadSubsidiary(orderHead);
            }

            return orderHead;
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(IList<OrderHead> orderHeadList, string userCode)
        {
            if (orderHeadList != null && orderHeadList.Count > 0)
            {
                foreach (OrderHead orderHead in orderHeadList)
                {
                    this.CreateOrder(orderHead, userCode);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(OrderHead orderHead, string userCode)
        {
            CreateOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(OrderHead orderHead, User user)
        {
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoCreatePermission");
            //}

            DateTime dateTimeNow = DateTime.Now;
            //过滤OrderQty数量为0的明细
            OrderHelper.FilterZeroOrderQty(orderHead);
            #region 整包校验,快速的不考虑
            if (!(orderHead.IsAutoRelease && orderHead.IsAutoStart))
            {
                if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                {
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        if (orderDetail.OrderHead.FulfillUnitCount && orderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                        {
                            if (orderDetail.OrderedQty % orderDetail.UnitCount != 0)
                            {
                                throw new BusinessErrorException("Order.Error.NotFulfillUnitCount", orderDetail.Item.Code);
                            }
                        }
                    }
                }
            }
            #endregion

            //生成OrderLocationTransaction和OrderOperation的记录
            this.orderHeadMgrE.GenerateOrderHeadSubsidiary(orderHead);

            #region 创建OrderHead
            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                orderHead.OrderNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_ORDER);
            }
            else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
            {
                orderHead.OrderNo = numberControlMgrE.GenerateNumber(orderHead.PartyFrom.Code);
            }
            else
            {
                orderHead.OrderNo = numberControlMgrE.GenerateNumber("ORD");
            }
            orderHead.CreateUser = user;
            orderHead.CreateDate = dateTimeNow;
            orderHead.LastModifyUser = user;
            orderHead.LastModifyDate = dateTimeNow;
            orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;

            orderHeadMgrE.CreateOrderHead(orderHead);
            #endregion

            #region 创建OrderDetail
            if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    CreateOrderDetailSubsidiary(orderDetail);
                }
            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailEmpty");
            }
            #endregion

            //分配订单头折扣
            this.AllocateOrderHeadDicount(orderHead);

            #region 创建OrderOP
            if (orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
            {
                foreach (OrderOperation orderOperation in orderHead.OrderOperations)
                {
                    this.orderOperationMgrE.CreateOrderOperation(orderOperation);
                }
            }
            #endregion

            #region 创建OrderBinding
            IList<FlowBinding> flowBindingList = this.flowBindingMgrE.GetFlowBinding(orderHead.Flow);
            if (flowBindingList != null && flowBindingList.Count > 0)
            {
                foreach (FlowBinding flowBinding in flowBindingList)
                {
                    this.orderBindingMgrE.CreateOrderBinding(orderHead, flowBinding.SlaveFlow, flowBinding.BindingType);
                }
            }
            #endregion

            #region 判断订单绑定
            this.CreateBindingOrder(orderHead, user, BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_CREATE);
            #endregion

            #region 判断自动Release
            if (orderHead.IsAutoRelease)
            {
                ReleaseOrder(orderHead, user, true);
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(string flowCode, User user, IList<Hu> huList)
        {
            Flow flow = flowMgrE.CheckAndLoadFlow(flowCode);
            CreateOrder(flow, user, huList);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(string flowCode, string userCode, IList<Hu> huList)
        {
            Flow flow = flowMgrE.CheckAndLoadFlow(flowCode);
            CreateOrder(flow, userMgrE.CheckAndLoadUser(userCode), huList);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(Flow flow, string userCode, IList<Hu> huList)
        {
            CreateOrder(flow, userMgrE.CheckAndLoadUser(userCode), huList);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateOrder(Flow flow, User user, IList<Hu> huList)
        {

            #region 初始化订单头
            OrderHead orderHead = this.TransferFlow2Order(flow);


            IList<OrderDetail> targetOrderDetailList = orderHead.OrderDetails;

            orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
            orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO;
            orderHead.WindowTime = DateTime.Now;
            orderHead.StartTime = DateTime.Now;

            orderHead.IsAutoRelease = false;
            orderHead.IsAutoStart = true;
            orderHead.IsAutoShip = false;
            orderHead.IsAutoReceive = false;
            #endregion

            #region 合并OrderDetailList
            if (huList != null && huList.Count > 0)
            {
                IList<OrderDetail> newOrderDetailList = new List<OrderDetail>();
                foreach (Hu hu in huList)
                {
                    bool findMatch = false;

                    #region 在FlowDetail转换的OrderDetail里面查找匹配项
                    foreach (OrderDetail targetOrderDetail in targetOrderDetailList)
                    {
                        if (hu.Item.Code == targetOrderDetail.Item.Code
                            && hu.Uom.Code == targetOrderDetail.Uom.Code)
                        {
                            targetOrderDetail.RequiredQty += hu.Qty;
                            targetOrderDetail.OrderedQty += hu.Qty;
                            findMatch = true;
                            break;
                        }
                    }
                    #endregion

                    if (!findMatch)
                    {
                        #region 没有找到匹配项，从新增匹配项中找
                        foreach (OrderDetail newOrderDetail in newOrderDetailList)
                        {
                            if (hu.Item.Code == newOrderDetail.Item.Code
                            && hu.Uom.Code == newOrderDetail.Uom.Code)
                            {
                                newOrderDetail.RequiredQty += hu.Qty;
                                newOrderDetail.OrderedQty += hu.Qty;

                                findMatch = true;

                                break;
                            }
                        }
                        #endregion

                        if (!findMatch)
                        {
                            #region 还没有找到匹配项,报错
                            throw new BusinessErrorException("OrderDetail.Item.NotInFlow");
                            #endregion
                        }
                    }
                }

                if (newOrderDetailList.Count > 0)
                {
                    #region 合并新增的OrderDetail
                    int seqInterval = int.Parse(this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL).Value);
                    int maxSeq = 0;
                    foreach (OrderDetail targetOrderDetail in targetOrderDetailList)
                    {
                        if (targetOrderDetail.Sequence > maxSeq)
                        {
                            maxSeq = targetOrderDetail.Sequence;
                        }
                    }

                    foreach (OrderDetail newOrderDetail in newOrderDetailList)
                    {
                        maxSeq += seqInterval;
                        newOrderDetail.Sequence = maxSeq;

                        orderHead.AddOrderDetail(newOrderDetail);
                    }
                    #endregion
                }
            }

            #endregion

            #region 创建订单
            CreateOrder(orderHead, user);
            #endregion

            #region 更新订单bom数量为负
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {

                IList<OrderLocationTransaction> orderLocTransList = orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                foreach (OrderLocationTransaction ordeLocTrans in orderLocTransList)
                {
                    if (ordeLocTrans.Item.Code == ordeLocTrans.OrderDetail.Item.Code)
                    {
                        continue;
                    }
                    else
                    {
                        ordeLocTrans.OrderedQty = 0 - ordeLocTrans.OrderedQty;
                        orderLocationTransactionMgrE.UpdateOrderLocationTransaction(ordeLocTrans);
                    }
                }
            }

            #endregion

            ReleaseReuseOrder(orderHead, user, huList);


        }

        [Transaction(TransactionMode.Requires)]
        public void AddOrderDetail(OrderDetail orderDetail, string userCode)
        {
            AddOrderDetail(orderDetail, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void AddOrderDetail(OrderDetail orderDetail, User user)
        {
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderDetail.OrderHead.OrderNo);
            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER_DETAIL))
            //{
            //    throw new BusinessErrorException("OrderDetail.Error.NoEditPermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                {
                    //检验orderDetail序号是否重复
                    orderHead.AddOrderDetail(orderDetail);
                }

                this.orderDetailMgrE.GenerateOrderDetailSubsidiary(orderDetail);

                //新增OrderDetail
                orderDetailMgrE.CreateOrderDetail(orderDetail);

                if (orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                {
                    //新增OrderLocationTransaction
                    foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                    {
                        orderLocationTransactionMgrE.CreateOrderLocationTransaction(orderLocationTransaction);
                    }
                }

                if (orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
                {
                    foreach (OrderOperation orderOperation in orderHead.OrderOperations)
                    {
                        //判断需要新增那些Op
                        if (orderOperation.Id == 0)
                        {
                            this.orderOperationMgrE.CreateOrderOperation(orderOperation);
                        }
                    }
                }

                //更新LastModifyDate、LastModifyUser
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                orderHeadMgrE.UpdateOrderHead(orderHead);

                //重新分摊订单头折扣
                this.AllocateOrderHeadDicount(orderHead);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, orderDetail.OrderHead.OrderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrderDetail(OrderDetail orderDetail, string userCode)
        {
            UpdateOrderDetail(orderDetail, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrderDetail(OrderDetail orderDetail, User user)
        {
            OrderDetail oldOrderDetail = orderDetailMgrE.LoadOrderDetail(orderDetail.Id);
            //检验权限
            //if (!OrderHelper.CheckOrderOperationAuthrize(oldOrderDetail.OrderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER_DETAIL))
            //{
            //    throw new BusinessErrorException("OrderDetail.Error.NoEditPermission", oldOrderDetail.OrderHead.OrderNo);
            //}
            orderDetail.Id = 0;
            orderDetail.OrderedQty = oldOrderDetail.OrderedQty;
            orderDetail.RequiredQty = oldOrderDetail.RequiredQty;
            this.DeleteOrderDetail(oldOrderDetail, user);
            this.AddOrderDetail(orderDetail, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateOrderQty(IList<OrderDetail> orderDetailList, string userCode)
        {
            UpdateOrderQty(orderDetailList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        /**
         * 保存订单数量 
         */
        [Transaction(TransactionMode.Requires)]
        public void UpdateOrderQty(IList<OrderDetail> orderDetailList, User user)
        {
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                IDictionary<string, OrderHead> cachedOrderHead = new Dictionary<string, OrderHead>(); //缓存出现过的OrderHead，一般来说只会有一个

                foreach (OrderDetail orderDetail in orderDetailList)
                {
                    if (!cachedOrderHead.ContainsKey(orderDetail.OrderHead.OrderNo))
                    {
                        OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderDetail.OrderHead.OrderNo);
                        //检验权限
                        //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER_DETAIL))
                        //{
                        //    throw new BusinessErrorException("OrderDetail.Error.NoEditPermission", orderHead.OrderNo);
                        //}

                        //检验订单是否Create状态
                        if (orderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
                        {
                            throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, orderDetail.OrderHead.OrderNo);
                        }

                        //整包下单检查
                        if (orderHead.FulfillUnitCount)
                        {
                            if (orderDetail.OrderedQty % orderDetail.UnitCount != 0)
                            {
                                throw new BusinessErrorException("Order.Error.NotFulfillUnitCount", orderDetail.Item.Code);
                            }
                        }

                        //缓存
                        cachedOrderHead.Add(orderHead.OrderNo, orderHead);
                    }

                    //更新OrderDetail数量,折扣
                    OrderDetail targetOrderDetail = orderDetailMgrE.LoadOrderDetail(orderDetail.Id);
                    targetOrderDetail.RequiredQty = orderDetail.RequiredQty;
                    targetOrderDetail.OrderedQty = orderDetail.OrderedQty;
                    targetOrderDetail.Discount = orderDetail.Discount;
                    orderDetailMgrE.UpdateOrderDetail(targetOrderDetail);

                    if (targetOrderDetail.OrderLocationTransactions != null && targetOrderDetail.OrderLocationTransactions.Count > 0)
                    {
                        //更新OrderLocationTransaction数量
                        foreach (OrderLocationTransaction orderLocationTransaction in targetOrderDetail.OrderLocationTransactions)
                        {
                            orderLocationTransaction.OrderedQty = orderDetail.OrderedQty * orderLocationTransaction.UnitQty;
                            orderLocationTransactionMgrE.UpdateOrderLocationTransaction(orderLocationTransaction);
                        }
                    }
                }

                //更新订单头LastModifyDate、LastModifyUser
                foreach (OrderHead orderHead in cachedOrderHead.Values)
                {
                    orderHead.LastModifyDate = DateTime.Now;
                    orderHead.LastModifyUser = user;
                    orderHeadMgrE.UpdateOrderHead(orderHead);

                    //重新分配订单折扣
                    this.AllocateOrderHeadDicount(orderHead);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderDetail(OrderDetail orderDetail, string userCode)
        {
            DeleteOrderDetail(orderDetail.Id, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderDetail(OrderDetail orderDetail, User user)
        {
            DeleteOrderDetail(orderDetail.Id, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderDetail(int orderDetailId, string userCode)
        {
            DeleteOrderDetail(orderDetailId, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderDetail(int orderDetailId, User user)
        {
            OrderDetail orderDetail = orderDetailMgrE.LoadOrderDetail(orderDetailId);
            OrderHead orderHead = orderDetail.OrderHead;

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_DELETE_ORDER_DETAIL))
            //{
            //    throw new BusinessErrorException("OrderDetail.Error.NoDeletePermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {

                //删除OrderLocationTransaction
                IList<int> unusedList = new List<int>();
                if (orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                {
                    orderLocationTransactionMgrE.DeleteOrderLocationTransaction(orderDetail.OrderLocationTransactions);

                    foreach (OrderLocationTransaction orderLocationTransactions in orderDetail.OrderLocationTransactions)
                    {
                        if (orderLocationTransactions.Operation != 0)
                        {
                            unusedList.Add(orderLocationTransactions.Operation);
                        }
                    }
                }

                //删除OrderDetail
                orderDetailMgrE.DeleteOrderDetail(orderDetailId);
                orderHead.OrderDetails.Remove(orderDetail);

                //更新OrderOp
                if (unusedList.Count > 0)
                {
                    orderHead.RemoveOrderDetailBySequence(orderDetail.Sequence);
                    this.orderOperationMgrE.TryDeleteOrderOperation(orderHead, unusedList);
                }

                //更新LastModifyDate、LastModifyUser
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                orderHeadMgrE.UpdateOrderHead(orderHead);

                //重新分配订单头折扣
                this.AllocateOrderHeadDicount(orderHead);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, orderDetail.OrderHead.OrderNo);
            }
        }



        [Transaction(TransactionMode.Requires)]
        public void AddOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction, string userCode)
        {
            AddOrderLocationTransaction(orderLocationTransaction, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void AddOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction, User user)
        {

            OrderDetail orderDetail = orderDetailMgrE.LoadOrderDetail(orderLocationTransaction.OrderDetail.Id);
            OrderHead orderHead = orderDetail.OrderHead;

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_EDIT_ORDER_DETAIL))
            //{
            //    throw new BusinessErrorException("OrderDetail.Error.NoEditPermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {

                //添加OrderLocationTransaction
                IList<int> unusedList = new List<int>();
                orderLocationTransactionMgrE.CreateOrderLocationTransaction(orderLocationTransaction);
                orderDetail.AddOrderLocationTransaction(orderLocationTransaction);

                //更新OrderOp
                this.orderOperationMgrE.TryAddOrderOperation(orderHead, orderLocationTransaction.Operation, null);

                //更新LastModifyDate、LastModifyUser
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                orderHeadMgrE.UpdateOrderHead(orderHead);

            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, orderDetail.OrderHead.OrderNo);
            }
        }




        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction, string userCode)
        {
            DeleteOrderLocationTransaction(orderLocationTransaction.Id, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderLocationTransaction(OrderLocationTransaction orderLocationTransaction, User user)
        {
            DeleteOrderLocationTransaction(orderLocationTransaction.Id, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderLocationTransaction(int orderLocationTransactionId, string userCode)
        {
            DeleteOrderLocationTransaction(orderLocationTransactionId, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderLocationTransaction(int orderLocationTransactionId, User user)
        {
            OrderLocationTransaction orderLocationTransaction = orderLocationTransactionMgrE.LoadOrderLocationTransaction(orderLocationTransactionId);
            OrderDetail orderDetail = orderDetailMgrE.LoadOrderDetail(orderLocationTransaction.OrderDetail.Id);
            OrderHead orderHead = orderDetail.OrderHead;

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_DELETE_ORDER_DETAIL))
            //{
            //    throw new BusinessErrorException("OrderDetail.Error.NoDeletePermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {


                //删除OrderLocationTransaction
                IList<int> unusedList = new List<int>();
                orderLocationTransactionMgrE.DeleteOrderLocationTransaction(orderLocationTransactionId);
                orderDetail.RemoveOrderLocationTransaction(orderLocationTransaction);
                if (orderLocationTransaction.Operation != 0)
                {
                    unusedList.Add(orderLocationTransaction.Operation);
                }


                //更新OrderOp
                if (unusedList.Count > 0)
                {
                    this.orderOperationMgrE.TryDeleteOrderOperation(orderHead, unusedList);
                }

                //更新LastModifyDate、LastModifyUser
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                orderHeadMgrE.UpdateOrderHead(orderHead);

            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, orderDetail.OrderHead.OrderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrder(OrderHead orderHead, string userCode)
        {
            DeleteOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrder(OrderHead orderHead, User user)
        {
            DeleteOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrder(string orderNo, string userCode)
        {
            DeleteOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrder(string orderNo, User user)
        {
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderNo);

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_DELETE_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoDeletePermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                {
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {

                        //删除OrderLocationTransaction
                        if (orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                        {
                            foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                            {
                                if (orderLocationTransaction.IOType == BusinessConstants.IO_TYPE_OUT)
                                {
                                    DetachedCriteria criteria = DetachedCriteria.For<OrderTracer>();
                                    criteria.Add(Expression.Eq("RefOrderLocTransId", orderLocationTransaction.Id));
                                    IList<OrderTracer> orderTracerList = this.criteriaMgrE.FindAll<OrderTracer>(criteria);
                                    if (orderTracerList != null && orderTracerList.Count > 0)
                                    {
                                        this.orderTracerMgrE.DeleteOrderTracer(orderTracerList);
                                    }
                                    break;
                                }
                            }

                            orderLocationTransactionMgrE.DeleteOrderLocationTransaction(orderDetail.OrderLocationTransactions);
                        }
                    }

                    //删除OrderDetail
                    orderDetailMgrE.DeleteOrderDetail(orderHead.OrderDetails);
                }

                if (orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
                {
                    //删除OrderOperation
                    orderOperationMgrE.DeleteOrderOperation(orderHead.OrderOperations);
                }
                if (orderHead.OrderBindings != null && orderHead.OrderBindings.Count > 0)
                {
                    //删除OrderBinding
                    orderBindingMgrE.DeleteOrderBinding(orderHead.OrderBindings);
                }

                //删除OrderHead
                orderHeadMgrE.DeleteOrderHead(orderNo);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenDelete", orderHead.Status, orderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(OrderHead orderHead, string userCode)
        {
            ReleaseOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode), false);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(OrderHead orderHead, User user)
        {
            ReleaseOrder(orderHead.OrderNo, user, false);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(string orderNo, string userCode)
        {
            ReleaseOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode), false);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(string orderNo, User user)
        {
            ReleaseOrder(orderNo, user, false);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(OrderHead orderHead, string userCode, bool autoHandleAbstractItem)
        {
            ReleaseOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode), autoHandleAbstractItem);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(OrderHead orderHead, User user, bool autoHandleAbstractItem)
        {
            ReleaseOrder(orderHead.OrderNo, user, autoHandleAbstractItem);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(string orderNo, string userCode, bool autoHandleAbstractItem)
        {
            ReleaseOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode), autoHandleAbstractItem);
        }

        [Transaction(TransactionMode.Requires)]
        public void ReleaseOrder(string orderNo, User user, bool autoHandleAbstractItem)
        {
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderNo);
            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_SUBMIT_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoReleasePermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
            {
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    #region 处理抽象件
                    string unHandledAbstractItemIds = string.Empty;
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        IList<OrderLocationTransaction> abstractOrderLocationTransactionList = new List<OrderLocationTransaction>();
                        foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                        {
                            if (orderLocationTransaction.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_A)
                            {
                                abstractOrderLocationTransactionList.Add(orderLocationTransaction);
                            }
                        }

                        foreach (OrderLocationTransaction orderLocationTransaction in abstractOrderLocationTransactionList)
                        {
                            if (autoHandleAbstractItem)
                            {
                                //替换抽象零件
                                this.orderLocationTransactionMgrE.AutoReplaceAbstractItem(orderLocationTransaction);
                            }
                            else
                            {
                                if (unHandledAbstractItemIds != string.Empty)
                                {
                                    unHandledAbstractItemIds = orderLocationTransaction.Item.Code;
                                }
                                else
                                {
                                    unHandledAbstractItemIds = " ," + orderLocationTransaction.Item.Code;
                                }
                            }
                        }
                    }

                    if (unHandledAbstractItemIds != string.Empty)
                    {
                        throw new BusinessErrorException("Order.Warning.UnhandleAbstractItem", unHandledAbstractItemIds);
                    }
                    #endregion

                    #region 处理选装件
                    IList<int> unusedOpList = new List<int>();
                    IList<OrderLocationTransaction> deleteOrderLocationTransactionList = new List<OrderLocationTransaction>();
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        IList<OrderLocationTransaction> optionalOrderLocationTransactionList = new List<OrderLocationTransaction>();
                        foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                        {
                            if (orderLocationTransaction.BomDetail != null
                                && orderLocationTransaction.BomDetail.StructureType == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O
                                && !orderLocationTransaction.IsAssemble)
                            {
                                optionalOrderLocationTransactionList.Add(orderLocationTransaction);
                            }
                        }

                        foreach (OrderLocationTransaction orderLocationTransaction in optionalOrderLocationTransactionList)
                        {
                            //选装件没有安装，删除orderLocationTransaction;
                            deleteOrderLocationTransactionList.Add(orderLocationTransaction);
                            orderDetail.RemoveOrderLocationTransaction(orderLocationTransaction);
                            if (orderLocationTransaction.Operation != 0)
                            {
                                unusedOpList.Add(orderLocationTransaction.Operation);
                            }
                        }
                    }

                    this.orderLocationTransactionMgrE.DeleteOrderLocationTransaction(deleteOrderLocationTransactionList);
                    this.orderOperationMgrE.TryDeleteOrderOperation(orderHead, unusedOpList);  //删除orderLocationTransaction对应的Op
                    #endregion
                }

                DateTime nowDate = DateTime.Now;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
                orderHead.ReleaseDate = nowDate;
                orderHead.ReleaseUser = user;
                orderHead.LastModifyDate = nowDate;
                orderHead.LastModifyUser = user;

                this.orderHeadMgrE.UpdateOrderHead(orderHead);

                #region 判断自动Start
                if (orderHead.IsAutoStart)
                {
                    StartOrder(orderHead, user);
                }
                #endregion

                #region 处理路线绑定
                this.CreateBindingOrder(orderHead, user, BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_SUBMIT);
                #endregion
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenSubmit", orderHead.Status, orderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void StartOrder(OrderHead orderHead, string userCode)
        {
            StartOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void StartOrder(string orderNo, string userCode)
        {
            StartOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void StartOrder(OrderHead orderHead, User user)
        {
            StartOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void StartOrder(string orderNo, User user)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            Flow flow = this.flowMgrE.LoadFlow(orderHead.Flow);

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_START_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoStartPermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                #region 检查生产单最大上线数量
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                    && orderHead.Flow != null && flow.MaxOnlineQty > 0
                    && this.GetInPorcessWOCount(orderHead.Flow, user) >= flow.MaxOnlineQty)
                {
                    throw new BusinessErrorException("Order.Error.ExcceedMaxOnlineQty");
                }
                #endregion

                DateTime nowDate = DateTime.Now;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
                orderHead.StartDate = nowDate;
                orderHead.StartUser = user;
                orderHead.LastModifyDate = nowDate;
                orderHead.LastModifyUser = user;

                this.orderHeadMgrE.UpdateOrderHead(orderHead);

                #region 判断自动PickList、Ship
                if (orderHead.IsAutoShip && orderHead.IsAutoReceive
                    && orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    if (orderHead.CompleteLatency.HasValue && orderHead.CompleteLatency.Value > 0)
                    {
                        //todo 收货延迟，记录到Quratz表中
                        throw new NotImplementedException("Complete Latency Not Implemented");
                    }
                    else
                    {
                        //立即收货
                        Receipt receipt = new Receipt();
                        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                        {
                            ReceiptDetail receiptDetail = new ReceiptDetail();
                            receiptDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];
                            receiptDetail.HuId = orderDetail.HuId;
                            receiptDetail.ReceivedQty = orderDetail.OrderedQty;
                            receiptDetail.Receipt = receipt;

                            #region 生产自动收货，找Out的OrderLocTrans，填充MaterialFulshBack
                            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                            {
                                IList<OrderLocationTransaction> orderLocTransList = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                                foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                                {
                                    MaterialFlushBack material = new MaterialFlushBack();
                                    material.OrderLocationTransaction = orderLocTrans;
                                    if (orderLocTrans.UnitQty != 0)
                                    {
                                        material.Qty = orderLocTrans.OrderedQty / orderLocTrans.UnitQty;
                                    }
                                    receiptDetail.AddMaterialFlushBack(material);
                                }
                            }
                            #endregion
                            receipt.AddReceiptDetail(receiptDetail);
                        }

                        ReceiveOrder(receipt, user);
                    }
                }
                else if (orderHead.IsAutoShip)
                {
                    if (orderHead.StartLatency.HasValue && orderHead.StartLatency.Value > 0)
                    {
                        //todo 上线延迟，记录到Quratz表中
                        throw new NotImplementedException("Start Latency Not Implemented");
                    }
                    else
                    {
                        //立即上线
                        IList<InProcessLocationDetail> inProcessLocationDetailList = new List<InProcessLocationDetail>();
                        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                        {
                            InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                            inProcessLocationDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT)[0];
                            inProcessLocationDetail.Qty = orderDetail.OrderedQty;

                            inProcessLocationDetailList.Add(inProcessLocationDetail);
                        }

                        ShipOrder(inProcessLocationDetailList, user);
                    }
                }
                else if (orderHead.IsAutoCreatePickList
                    && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)  //过滤掉退货和调整
                {
                    IList<OrderLocationTransaction> orderLocationTransactionList = new List<OrderLocationTransaction>();
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        IList<OrderLocationTransaction> outOrderLocationTransactionList = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                        foreach (OrderLocationTransaction orderLocationTransaction in outOrderLocationTransactionList)
                        {
                            orderLocationTransaction.CurrentShipQty = orderLocationTransaction.OrderedQty;
                        }
                        IListHelper.AddRange<OrderLocationTransaction>(orderLocationTransactionList, outOrderLocationTransactionList);
                    }

                    this.pickListMgrE.CreatePickList(orderLocationTransactionList, user);
                }
                #endregion
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenStart", orderHead.Status, orderNo);
            }
        }



        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseScrapOrder(string orderNo, string userCode)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReleaseScrapOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseScrapOrder(OrderHead orderHead, string userCode)
        {
            return ReleaseScrapOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseScrapOrder(string orderNo, User currentUser)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReleaseScrapOrder(orderHead, currentUser);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseScrapOrder(OrderHead orderHead, User currentUser)
        {
            ReleaseOrder(orderHead, currentUser);
            Receipt receipt = ReceiveScrapOrder(orderHead, currentUser);
            ManualCompleteOrder(orderHead, currentUser);
            return receipt;
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseReuseOrder(string orderNo, string userCode, IList<Hu> huList)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReleaseReuseOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode), huList);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseReuseOrder(OrderHead orderHead, string userCode, IList<Hu> huList)
        {
            return ReleaseReuseOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode), huList);
        }


        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseReuseOrder(string orderNo, User currentUser, IList<Hu> huList)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReleaseReuseOrder(orderHead, currentUser, huList);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReleaseReuseOrder(OrderHead orderHead, User currentUser, IList<Hu> huList)
        {
            if (huList.Count == 0)
            {
                throw new BusinessErrorException("Hu.Error.DetailEmpty");
            }

            #region 更新成品的库位为条码的库位
            Location location = null;
            foreach (Hu hu in huList)
            {
                LocationLotDetail locationLotDetail = locationLotDetailMgrE.GetHuLocationLotDetail(hu.HuId)[0];
                if (location == null)
                {
                    location = locationLotDetail.Location;
                }
                if (location != locationLotDetail.Location)
                {
                    throw new BusinessErrorException("Hu.Error.Location.NotEqual");
                }
            }


            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                IList<OrderLocationTransaction> orderLocTransList = orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                foreach (OrderLocationTransaction orderLoctrans in orderLocTransList)
                {
                    if (orderLoctrans.Item.Code == orderDetail.Item.Code)
                    {
                        orderLoctrans.Location = location;
                        orderLocationTransactionMgrE.UpdateOrderLocationTransaction(orderLoctrans);
                    }
                }
            }
            #endregion

            ReleaseOrder(orderHead, currentUser);
            Receipt receipt = ReceiveReuseOrder(orderHead, currentUser, huList);
            ManualCompleteOrder(orderHead, currentUser);
            return receipt;
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveScrapOrder(string orderNo, string userCode)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);

            return ReceiveScrapOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveScrapOrder(string orderNo, User user)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReceiveScrapOrder(orderHead, user);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveScrapOrder(OrderHead orderHead, string userCode)
        {
            return ReceiveScrapOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveScrapOrder(OrderHead orderHead, User user)
        {
            IList<ReceiptDetail> receiptDetailList = new List<ReceiptDetail>();
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                ReceiptDetail receiptDetail = new ReceiptDetail();
                receiptDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];
                receiptDetail.HuId = orderDetail.HuId;
                receiptDetail.ScrapQty = orderDetail.OrderedQty;

                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    IList<OrderLocationTransaction> orderLocTransList = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                    foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                    {
                        MaterialFlushBack material = new MaterialFlushBack();
                        material.OrderLocationTransaction = orderLocTrans;

                        if (orderLocTrans.UnitQty != 0)
                        {
                            material.Qty = orderLocTrans.OrderedQty / orderLocTrans.UnitQty;
                        }
                        receiptDetail.AddMaterialFlushBack(material);
                    }
                }
                receiptDetailList.Add(receiptDetail);
            }


            return ReceiveOrder(receiptDetailList, user, null, null, null, true, false);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveReuseOrder(string orderNo, User user, IList<Hu> huList)
        {
            OrderHead orderHead = this.orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            return ReceiveReuseOrder(orderHead, user, huList);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveReuseOrder(OrderHead orderHead, User user, IList<Hu> huList)
        {
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                decimal qty = orderDetail.OrderedQty;
                foreach (Hu hu in huList)
                {
                    if (orderDetail.Item.Code == hu.Item.Code && orderDetail.Uom.Code == hu.Uom.Code)
                    {
                        qty = qty - hu.Qty;
                    }
                }
                if (qty != 0)
                {
                    throw new BusinessErrorException("OrderDetail.Item.Qty.NotMatch", orderDetail.Item.Code);
                }

            }

            IList<ReceiptDetail> receiptDetailList = new List<ReceiptDetail>();
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                ReceiptDetail receiptDetail = new ReceiptDetail();
                receiptDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];
                receiptDetail.HuId = orderDetail.HuId;
                receiptDetail.ScrapQty = orderDetail.OrderedQty;

                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    IList<OrderLocationTransaction> orderLocTransList = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);
                    foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                    {

                        MaterialFlushBack material = new MaterialFlushBack();
                        material.OrderLocationTransaction = orderLocTrans;
                        if (orderLocTrans.OrderDetail.Item.Code == orderLocTrans.Item.Code)
                        {
                            foreach (Hu hu in huList)
                            {
                                if (hu.Item.Code == orderLocTrans.Item.Code && hu.Uom.Code == orderLocTrans.OrderDetail.Uom.Code)
                                {
                                    material = new MaterialFlushBack();
                                    material.OrderLocationTransaction = orderLocTrans;
                                    material.HuId = hu.HuId;
                                    if (orderLocTrans.UnitQty != 0)
                                    {
                                        material.Qty = hu.Qty;
                                    }
                                    receiptDetail.AddMaterialFlushBack(material);
                                }
                            }
                        }
                        else
                        {
                            if (orderLocTrans.UnitQty != 0)
                            {
                                material.Qty = orderLocTrans.OrderedQty / orderLocTrans.UnitQty;
                            }
                            receiptDetail.AddMaterialFlushBack(material);
                        }
                    }
                }
                receiptDetailList.Add(receiptDetail);
            }


            return ReceiveOrder(receiptDetailList, user, null, null, null, true, false);
        }


        [Transaction(TransactionMode.Requires)]
        public void CancelOrder(OrderHead orderHead, string userCode)
        {
            CancelOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelOrder(OrderHead orderHead, User user)
        {
            CancelOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelOrder(string orderNo, string userCode)
        {
            CancelOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelOrder(string orderNo, User user)
        {
            CancelOrder(orderNo, user, null);
        }

        [Transaction(TransactionMode.Requires)]
        public void CancelOrder(string orderNo, User user, string cancelReason)
        {
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderNo);
            orderHead.CancelReason = cancelReason;
            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                DateTime nowDate = DateTime.Now;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL;
                orderHead.CancelDate = nowDate;
                orderHead.CancelUser = user;
                orderHead.LastModifyDate = nowDate;
                orderHead.LastModifyUser = user;

                this.orderHeadMgrE.UpdateOrderHead(orderHead);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenCancel", orderHead.Status, orderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCompleteOrder(string[] flowCodeArray)
        {
            DateTime nowDate = DateTime.Now;
            foreach (string flowCode in flowCodeArray)
            {
                DetachedCriteria criteria = DetachedCriteria.For<OrderHead>();

                criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                criteria.Add(Expression.Eq("Flow", flowCode));
                criteria.Add(Expression.Lt("WindowTime", nowDate));
                IList<OrderHead> orderHeadList = this.criteriaMgrE.FindAll<OrderHead>(criteria);

                foreach (OrderHead orderHead in orderHeadList)
                {
                    if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                        || (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER && orderHead.WindowTime.AddMinutes(30) < nowDate))
                    {
                        ManualCompleteOrder(orderHead, userMgrE.LoadUser(BusinessConstants.SYSTEM_USER_MONITOR));
                    }
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseOrder()
        {
            IList<OrderHead> orderHeadList = orderHeadMgrE.GetOrderHead(BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE);
            foreach (OrderHead orderHead in orderHeadList)
            {
                TryCloseOrder(orderHead, userMgrE.LoadUser(BusinessConstants.SYSTEM_USER_MONITOR));
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseOrder(OrderHead orderHead, string userCode)
        {
            TryCloseOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseOrder(OrderHead orderHead, User user)
        {
            TryCloseOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseOrder(string orderNo, string userCode)
        {
            TryCloseOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCloseOrder(string orderNo, User user)
        {
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderNo);

            //权限校验
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_CLOSE_ORDER))
            //{
            //    return;
            //    //throw new BusinessErrorException("Order.Error.NoClosePermission", orderHead.OrderNo);
            //}

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
            {
                #region 存在未关闭ASN，不可以关闭
                IList<InProcessLocationDetail> ipLocDetailList = inProcessLocationDetailMgrE.GetInProcessLocationDetail(orderHead, BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);
                if (ipLocDetailList != null && ipLocDetailList.Count > 0)
                {
                    return;
                }
                #endregion

                #region 存在PlanBill未结算，不可以关闭
                IList<PlannedBill> plannedBillList = this.plannedBillMgrE.GetUnSettledPlannedBill(orderNo);
                if (plannedBillList != null && plannedBillList.Count > 0)
                {
                    return;
                }
                #endregion

                #region 存在ActingBill未开票，不可以关闭
                IList<ActingBill> actingBillList = this.actingBillMgrE.GetUnBilledActingBill(orderNo);
                if (actingBillList != null && actingBillList.Count > 0)
                {
                    return;
                }
                #endregion

                #region 存在BillDetail未关闭，不可以关闭
                DetachedCriteria criteria = DetachedCriteria.For<BillDetail>();
                criteria.CreateAlias("Bill", "b");
                if (actingBillList.Count == 1)
                {
                    criteria.Add(Expression.Eq("ActingBill", actingBillList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG("ActingBill", actingBillList));
                }
                criteria.Add(Expression.Or(
                    Expression.Not(Expression.Eq("b.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)),
                    Expression.Not(Expression.Eq("b.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID))
                    ));
                IList<BillDetail> billDetailList = this.criteriaMgrE.FindAll<BillDetail>(criteria);

                if (billDetailList != null && billDetailList.Count > 0)
                {
                    return;
                }
                #endregion

                #region 生产有投料未回冲，不能关闭
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    DetachedCriteria criteria2 = DetachedCriteria.For<OrderPlannedBackflush>()
                        .SetProjection(Projections.Count("Id"));

                    criteria2.CreateAlias("OrderLocationTransaction", "olt");
                    criteria2.CreateAlias("olt.OrderDetail", "od");
                    criteria2.CreateAlias("od.OrderHead", "oh");

                    criteria2.Add(Expression.Eq("IsActive", true));
                    criteria2.Add(Expression.Eq("oh.OrderNo", orderNo));

                    IList list = criteriaMgrE.FindAll(criteria2);
                    if (int.Parse(list[0].ToString()) > 0)
                    {
                        return;
                    }
                }
                #endregion

                DateTime nowDate = DateTime.Now;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                orderHead.CloseDate = nowDate;
                orderHead.CloseUser = user;
                orderHead.LastModifyDate = nowDate;
                orderHead.LastModifyUser = user;

                this.orderHeadMgrE.UpdateOrderHead(orderHead);
            }
            else
            {
                return;
                //throw new BusinessErrorException("Order.Error.StatusErrorWhenClose", orderHead.Status, orderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(IList<OrderDetail> orderDetailList, string userCode)
        {
            return ShipOrder(orderDetailList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(IList<OrderDetail> orderDetailList, User user)
        {
            InProcessLocation inProcessLocation = new InProcessLocation();
            foreach (OrderDetail orderDetail in orderDetailList)
            {
                InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                inProcessLocationDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT)[0];
                inProcessLocationDetail.Qty = orderDetail.CurrentShipQty;
                inProcessLocationDetail.InProcessLocation = inProcessLocation;

                inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
            }

            return ShipOrder(inProcessLocation, user);
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(IList<InProcessLocationDetail> inProcessLocationDetailList, string userCode)
        {
            return ShipOrder(inProcessLocationDetailList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(IList<InProcessLocationDetail> inProcessLocationDetailList, User user)
        {
            if (inProcessLocationDetailList != null && inProcessLocationDetailList.Count > 0)
            {

                InProcessLocation inProcessLocation = new InProcessLocation();
                inProcessLocation.InProcessLocationDetails = inProcessLocationDetailList;

                foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
                {
                    inProcessLocationDetail.InProcessLocation = inProcessLocation;
                }

                return ShipOrder(inProcessLocation, user);
            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailShipEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(string pickListNo, string userCode)
        {
            return ShipOrder(pickListNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(string pickListNo, User user)
        {
            PickList pickList = this.pickListMgrE.CheckAndLoadPickList(pickListNo);

            //订单关闭后，拣货单也应该能够发货
            if (pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS
                && pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE
                && pickList.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                throw new BusinessErrorException("Order.Error.PickUp.StatusErrorWhenShip", pickList.Status, pickList.PickListNo);
            }

            PickListHelper.CheckAuthrize(pickList, user);

            InProcessLocation inProcessLocation = new InProcessLocation();

            foreach (PickListDetail pickListDetail in pickList.PickListDetails)
            {
                OrderLocationTransaction orderLocationTransaction = pickListDetail.OrderLocationTransaction;

                foreach (PickListResult pickListResult in pickListDetail.PickListResults)
                {
                    InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                    inProcessLocationDetail.HuId = pickListResult.LocationLotDetail.Hu.HuId;
                    inProcessLocationDetail.LotNo = pickListResult.LocationLotDetail.LotNo;
                    inProcessLocationDetail.OrderLocationTransaction = orderLocationTransaction;
                    inProcessLocationDetail.Qty = pickListResult.Qty / orderLocationTransaction.UnitQty; //订单单位
                    inProcessLocationDetail.InProcessLocation = inProcessLocation;

                    inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                }
            }

            inProcessLocation = ShipOrder(inProcessLocation, user, false);

            #region 关闭捡货单
            pickList.LastModifyDate = DateTime.Now;
            pickList.LastModifyUser = user;
            pickList.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;

            this.pickListMgrE.UpdatePickList(pickList);
            #endregion

            return inProcessLocation;
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(PickList pickList, string userCode)
        {
            return ShipOrder(pickList.PickListNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(PickList pickList, User user)
        {
            return ShipOrder(pickList.PickListNo, user);
        }


        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(InProcessLocation inProcessLocation, string userCode)
        {
            return ShipOrder(inProcessLocation, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public InProcessLocation ShipOrder(InProcessLocation inProcessLocation, User user)
        {
            return ShipOrder(inProcessLocation, user, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<OrderDetail> orderDetailList, string userCode)
        {
            return ReceiveOrder(orderDetailList, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<OrderDetail> orderDetailList, User user)
        {
            return ReceiveOrder(orderDetailList, user, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<OrderDetail> orderDetailList, string userCode, bool isOddCreateHu)
        {
            return ReceiveOrder(orderDetailList, this.userMgrE.CheckAndLoadUser(userCode), isOddCreateHu);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<OrderDetail> orderDetailList, User user, bool isOddCreateHu)
        {
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                foreach (OrderDetail orderDetail in orderDetailList)
                {
                    ReceiptDetail receiptDetail = new ReceiptDetail();
                    receiptDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];
                    receiptDetail.HuId = orderDetail.HuId;
                    receiptDetail.ReceivedQty = orderDetail.CurrentReceiveQty;
                    receiptDetail.RejectedQty = orderDetail.CurrentRejectQty;
                    receiptDetail.ScrapQty = orderDetail.CurrentScrapQty;
                    receiptDetail.PutAwayBinCode = orderDetail.PutAwayBinCode;
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }

                return ReceiveOrder(receipt, user, null, true, isOddCreateHu);
            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode, InProcessLocation inProcessLocation)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode, InProcessLocation inProcessLocation, string externalReceiptNo)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList, bool createIp)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, createIp, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, string userCode, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList, bool createIp, bool isOddCreateHu)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, createIp, isOddCreateHu);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, string userCode)
        {
            return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), null, true, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, string userCode, IList<WorkingHours> workingHoursList)
        {
            return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, true, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, string userCode, IList<WorkingHours> workingHoursList, bool createIp)
        {
            return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, createIp, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, string userCode, IList<WorkingHours> workingHoursList, bool createIp, bool isOddCreateHu)
        {
            return ReceiveOrder(receipt, this.userMgrE.CheckAndLoadUser(userCode), workingHoursList, createIp, isOddCreateHu);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user, InProcessLocation inProcessLocation)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user, InProcessLocation inProcessLocation, string externalReceiptNo)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, null, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, workingHoursList, true, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList, bool createIp)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, workingHoursList, createIp, true);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(IList<ReceiptDetail> receiptDetailList, User user, InProcessLocation inProcessLocation, string externalReceiptNo, IList<WorkingHours> workingHoursList, bool createIp, bool isOddCreateHu)
        {
            if (receiptDetailList != null && receiptDetailList.Count > 0)
            {
                Receipt receipt = new Receipt();
                if (inProcessLocation != null)
                {
                    receipt.AddInProcessLocation(inProcessLocation);
                }
                receipt.ExternalReceiptNo = externalReceiptNo;
                foreach (ReceiptDetail receiptDetail in receiptDetailList)
                {
                    receiptDetail.Receipt = receipt;

                    receipt.AddReceiptDetail(receiptDetail);
                }
                return ReceiveOrder(receipt, user, workingHoursList, createIp, isOddCreateHu);

            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, User user)
        {
            return ReceiveOrder(receipt, user, null, true, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, User user, IList<WorkingHours> workingHoursList)
        {
            return ReceiveOrder(receipt, user, workingHoursList, true, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, User user, IList<WorkingHours> workingHoursList, bool createIp)
        {
            return ReceiveOrder(receipt, user, workingHoursList, createIp, true);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt ReceiveOrder(Receipt receipt, User user, IList<WorkingHours> workingHoursList, bool createIp, bool isOddCreateHu)
        {
            #region 变量定义
            IDictionary<string, OrderHead> cachedOrderHead = new Dictionary<string, OrderHead>();  //缓存用到的OrderHead
            DateTime dateTimeNow = DateTime.Now;
            #endregion

            #region 判断全0收货
            if (receipt != null && receipt.ReceiptDetails != null && receipt.ReceiptDetails.Count > 0)
            {
                //判断全0收货
                IList<ReceiptDetail> nonZeroReceiptDetailList = new List<ReceiptDetail>();
                foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
                {
                    if (!receiptDetail.ReceivedQty.HasValue)
                    {
                        receiptDetail.ReceivedQty = 0;
                    }

                    if (!receiptDetail.RejectedQty.HasValue)
                    {
                        receiptDetail.RejectedQty = 0;
                    }


                    if (!receiptDetail.ScrapQty.HasValue)
                    {
                        receiptDetail.ScrapQty = 0;
                    }


                    if (receiptDetail.ReceivedQty.Value != 0
                        || receiptDetail.RejectedQty.Value != 0
                        || receiptDetail.ScrapQty.Value != 0)
                    {
                        nonZeroReceiptDetailList.Add(receiptDetail);
                    }
                }

                if (nonZeroReceiptDetailList.Count == 0)
                {
                    throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
                }

                receipt.ReceiptDetails = nonZeroReceiptDetailList;
            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailReceiveEmpty");
            }
            #endregion

            #region 为未发货就收货创建ASN
            if ((receipt.InProcessLocations == null || receipt.InProcessLocations.Count == 0)
                && createIp)
            {
                InProcessLocation inProcessLocation = new InProcessLocation();

                #region 循环收货列表，并添加到发货列表中
                foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
                {
                    OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
                    OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                    OrderHead orderHead = orderDetail.OrderHead;

                    if (receiptDetail.MaterialFlushBack != null && receiptDetail.MaterialFlushBack.Count > 0)
                    {
                        #region 根据物料回冲创建ASN，只适应生产，其它情况没有测试
                        foreach (MaterialFlushBack materialFlushBack in receiptDetail.MaterialFlushBack)
                        {
                            InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                            inProcessLocationDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.LoadOrderLocationTransaction(materialFlushBack.OrderLocationTransaction.Id);
                            inProcessLocationDetail.HuId = materialFlushBack.HuId;
                            inProcessLocationDetail.LotNo = materialFlushBack.LotNo;
                            inProcessLocationDetail.Qty = materialFlushBack.Qty;
                            inProcessLocationDetail.InProcessLocation = inProcessLocation;

                            inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 根据out的OrderLocationTransaction自动创建ASN
                        IList<OrderLocationTransaction> orderLocationTransactionList =
                            this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_OUT);

                        foreach (OrderLocationTransaction orderLocTrans in orderLocationTransactionList)
                        {
                            #region 直接Copy收货项至发货项
                            InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                            if (orderHead.IsShipScanHu)
                            {
                                //只有发货扫描条码才复制条码信息
                                inProcessLocationDetail.HuId = receiptDetail.HuId;
                                inProcessLocationDetail.LotNo = receiptDetail.LotNo;
                            }
                            inProcessLocationDetail.OrderLocationTransaction = orderLocTrans;
                            inProcessLocationDetail.Qty =
                                receiptDetail.ReceivedQty.Value + receiptDetail.RejectedQty.Value + receiptDetail.ScrapQty.Value;
                            inProcessLocationDetail.InProcessLocation = inProcessLocation;

                            inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion

                #region 发货
                DoShipOrder(inProcessLocation, user, true);

                receipt.AddInProcessLocation(inProcessLocation);
                #endregion
            }
            #endregion

            #region 更新订单信息
            EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_NO_PRICE_LIST_RECEIPT);
            string taxCode = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_TAX_RATE).Value;
            foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            {
                OrderLocationTransaction orderLocationTransaction = this.orderLocationTransactionMgrE.LoadOrderLocationTransaction(receiptDetail.OrderLocationTransaction.Id);
                receiptDetail.OrderLocationTransaction = orderLocationTransaction;
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                #region 判断OrderHead状态并缓存
                if (!cachedOrderHead.ContainsKey(orderHead.OrderNo))
                {
                    //检查权限
                    //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_RECEIVE_ORDER))
                    //{
                    //    throw new BusinessErrorException("Order.Error.NoReceivePermission", orderHead.OrderNo);
                    //}

                    //判断OrderHead状态，只要有ASN就都可以收货
                    if (!(orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS
                        || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE
                        || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE))
                    {
                        throw new BusinessErrorException("Order.Error.StatusErrorWhenReceive", orderHead.Status, orderHead.OrderNo);
                    }

                    //缓存OrderHead
                    cachedOrderHead.Add(orderHead.OrderNo, orderHead);
                }
                #endregion

                #region 整包装收货判断,快速的不考虑
                if (orderHead.FulfillUnitCount && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML
                   && !(orderHead.IsAutoRelease && orderHead.IsAutoStart))
                {
                    if (receiptDetail.ReceivedQty % orderDetail.UnitCount != 0)
                    {
                        //不是整包装
                        throw new BusinessErrorException("Order.Error.NotFulfillUnitCountGrGi", orderDetail.Item.Code);
                    }
                }
                #endregion

                #region 是否过量收货判断
                if (orderDetail.OrderHead.SubType != BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)
                {
                    EntityPreference allowExceedentityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ALLOW_EXCEED_GI_GR);
                    bool allowExceedGiGR = bool.Parse(allowExceedentityPreference.Value); //企业属性，允许过量发货和收货


                    //检查Received(已收数)不能大于等于OrderedQty(订单数)
                    //if (!(orderHead.AllowExceed && allowExceedGiGR) && orderDetail.ReceivedQty.HasValue)
                    //{
                    //    if ((orderDetail.OrderedQty > 0 && orderDetail.ReceivedQty.Value >= orderDetail.OrderedQty)
                    //        || (orderDetail.OrderedQty < 0 && orderDetail.ReceivedQty.Value <= orderDetail.OrderedQty))
                    //    {
                    //        throw new BusinessErrorException("Order.Error.ReceiveExcceed", orderHead.OrderNo, orderDetail.Item.Code);
                    //    }
                    //}

                    if (!(orderHead.AllowExceed && allowExceedGiGR))   //不允许过量收货
                    {
                        //检查AccumulateQty(已收数) + CurrentReceiveQty(本次收货数)不能大于OrderedQty(订单数)
                        orderDetail.ReceivedQty = orderDetail.ReceivedQty.HasValue ? orderDetail.ReceivedQty.Value : 0;
                        if ((orderDetail.OrderedQty > 0 && orderDetail.ReceivedQty + receiptDetail.ReceivedQty > orderDetail.OrderedQty)
                            || (orderDetail.OrderedQty < 0 && orderDetail.ReceivedQty + receiptDetail.ReceivedQty < orderDetail.OrderedQty))
                        {
                            throw new BusinessErrorException("Order.Error.ReceiveExcceed", orderHead.OrderNo, orderDetail.Item.Code);
                        }
                    }
                }
                #endregion

                #region 采购收货是否有价格单判断
                if ((orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
                    && !bool.Parse(entityPreference.Value))
                {
                    if (orderDetail.UnitPrice == Decimal.Zero)
                    {
                        //重新查找一次价格
                        PriceListDetail priceListDetail = priceListDetailMgrE.GetLastestPriceListDetail(
                            orderDetail.DefaultPriceList,
                            orderDetail.Item,
                            orderHead.StartTime,
                            orderHead.Currency,
                            orderDetail.Uom);

                        if (priceListDetail != null)
                        {
                            orderDetail.UnitPrice = priceListDetail.UnitPrice;
                            orderDetail.IsProvisionalEstimate = priceListDetail.IsProvisionalEstimate;
                            orderDetail.IsIncludeTax = priceListDetail.IsIncludeTax;
                            orderDetail.TaxCode = taxCode;
                            //priceListDetail.TaxCode;
                        }
                        else
                        {
                            throw new BusinessErrorException("Order.Error.NoPriceListReceipt", orderDetail.Item.Code);
                        }
                    }
                }
                #endregion

                #region 计算PlannedAmount，用ReceiptDetail缓存本次收货产生的PlannedAmount金额，在创建PlannedBill时使用
                if (!orderDetail.ReceivedQty.HasValue)
                {
                    orderDetail.ReceivedQty = 0;
                }

                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
                {
                    CalculatePlannedAmount(orderDetail, receiptDetail, orderDetail.IncludeTaxTotalPrice);
                }
                #endregion

                #region 记录OrderDetail的累计收货量
                orderDetail.ReceivedQty += receiptDetail.ReceivedQty.Value;
                orderDetail.RejectedQty = receiptDetail.RejectedQty.Value + (orderDetail.RejectedQty == null ? 0 : orderDetail.RejectedQty);
                orderDetail.ScrapQty = receiptDetail.ScrapQty.Value + (orderDetail.ScrapQty == null ? 0 : orderDetail.ScrapQty);
                this.orderDetailMgrE.UpdateOrderDetail(orderDetail);
                #endregion

                #region 记录OrderLocationTransaction的累计收货量

                #region 成品
                if (!orderLocationTransaction.AccumulateQty.HasValue)
                {
                    orderLocationTransaction.AccumulateQty = 0;
                }
                orderLocationTransaction.AccumulateQty += receiptDetail.ReceivedQty.Value * orderLocationTransaction.UnitQty;
                #endregion

                #region 次品
                if (!orderLocationTransaction.AccumulateRejectQty.HasValue)
                {
                    orderLocationTransaction.AccumulateRejectQty = 0;
                }
                orderLocationTransaction.AccumulateRejectQty += receiptDetail.RejectedQty.Value * orderLocationTransaction.UnitQty;
                #endregion

                #region 废品
                if (!orderLocationTransaction.AccumulateScrapQty.HasValue)
                {
                    orderLocationTransaction.AccumulateScrapQty = 0;
                }
                orderLocationTransaction.AccumulateScrapQty += receiptDetail.ScrapQty.Value * orderLocationTransaction.UnitQty;
                #endregion

                this.orderLocationTransactionMgrE.UpdateOrderLocationTransaction(orderLocationTransaction);
                #endregion
            }
            #endregion

            #region 创建收货单
            this.receiptMgrE.CreateReceipt(receipt, user, isOddCreateHu);
            #endregion

            #region 记录工时
            if (workingHoursList != null)
            {
                foreach (WorkingHours workingHours in workingHoursList)
                {
                    workingHours.Receipt = receipt;
                    workingHours.LastModifyDate = DateTime.Now;
                    workingHours.LastModifyUser = user;
                    this.workingHoursMgrE.CreateWorkingHours(workingHours);
                }
            }
            #endregion

            #region 更新订单头信息
            foreach (OrderHead orderHead in cachedOrderHead.Values)
            {
                orderHead.LastModifyUser = user;
                orderHead.LastModifyDate = dateTimeNow;
                this.orderHeadMgrE.UpdateOrderHead(orderHead);
                if (!orderHead.AllowRepeatlyExceed)
                {
                    TryCompleteOrder(orderHead, user);
                }
            }
            #endregion

            #region 处理委外加工
            foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
            {
                OrderLocationTransaction orderLocationTransaction = receiptDetail.OrderLocationTransaction;
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;
                Flow flow = this.flowMgrE.LoadFlow(orderHead.Flow);
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
                {
                    Flow productionFlow = this.flowMgrE.LoadFlow(flow.ReferenceFlow, true);

                    OrderHead productionOrderHead = this.TransferFlow2Order(productionFlow, false);

                    foreach (FlowDetail productionFlowDetail in productionFlow.FlowDetails)
                    {
                        if (productionFlowDetail.DefaultLocationTo != null
                            && receiptDetail.OrderLocationTransaction.Location.Code   //目的库位相同
                            == productionFlowDetail.DefaultLocationTo.Code
                            && receiptDetail.OrderLocationTransaction.Item.Code == productionFlowDetail.Item.Code)
                        {
                            OrderDetail productionOrderDetail = this.orderDetailMgrE.TransferFlowDetail2OrderDetail(productionFlowDetail);

                            #region 合并相同的productionOrderDetail
                            bool findMatch = false;
                            if (productionOrderHead.OrderDetails != null && productionOrderHead.OrderDetails.Count > 0)
                            {
                                foreach (OrderDetail addProductionOrderDetail in productionOrderHead.OrderDetails)
                                {
                                    if (productionOrderDetail.Item.Code == addProductionOrderDetail.Item.Code
                                       && productionOrderDetail.Uom.Code == addProductionOrderDetail.Uom.Code
                                       && productionOrderDetail.UnitCount == addProductionOrderDetail.UnitCount
                                       && LocationHelper.IsLocationEqual(productionOrderDetail.DefaultLocationFrom, addProductionOrderDetail.DefaultLocationFrom)
                                       && LocationHelper.IsLocationEqual(productionOrderDetail.DefaultLocationTo, addProductionOrderDetail.DefaultLocationTo))
                                    {
                                        decimal addQty = receiptDetail.ReceivedQty.Value;
                                        if (addProductionOrderDetail.Uom.Code != orderDetail.Uom.Code)
                                        {
                                            addProductionOrderDetail.OrderedQty += this.uomConversionMgrE.ConvertUomQty(addProductionOrderDetail.Item, orderDetail.Uom, addQty, productionOrderDetail.Uom);
                                        }
                                        else
                                        {
                                            addProductionOrderDetail.OrderedQty += addQty;
                                        }
                                        findMatch = true;
                                    }
                                }
                            }
                            #endregion

                            if (!findMatch)
                            {
                                productionOrderDetail.OrderHead = productionOrderHead;
                                productionOrderDetail.OrderedQty = receiptDetail.ReceivedQty.Value;
                                if (productionOrderDetail.Uom.Code != orderDetail.Uom.Code)
                                {
                                    productionOrderDetail.OrderedQty = this.uomConversionMgrE.ConvertUomQty(productionOrderDetail.Item, orderDetail.Uom, receiptDetail.ReceivedQty.Value, productionOrderDetail.Uom);
                                }

                                productionOrderHead.AddOrderDetail(productionOrderDetail);
                            }
                        }
                    }

                    if (productionOrderHead.OrderDetails != null && productionOrderHead.OrderDetails.Count > 0)
                    {
                        productionOrderHead.IsAutoRelease = true;
                        productionOrderHead.IsAutoStart = true;
                        productionOrderHead.IsAutoShip = true;
                        productionOrderHead.IsAutoReceive = true;
                        productionOrderHead.StartLatency = 0;
                        productionOrderHead.CompleteLatency = 0;
                        productionOrderHead.StartTime = orderHead.StartTime;
                        productionOrderHead.WindowTime = orderHead.WindowTime;
                        productionOrderHead.ReferenceOrderNo = orderHead.OrderNo;
                        productionOrderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                        productionOrderHead.IsSubcontract = true;

                        this.CreateOrder(productionOrderHead, user);
                    }
                }
            }
            #endregion

            #region 处理路线绑定
            foreach (OrderHead orderHead in cachedOrderHead.Values)
            {
                this.CreateBindingOrder(orderHead, user,
                    BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_RECEIVE_ASYN,
                    BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_RECEIVE_SYN);
            }
            #endregion

            return receipt;
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt QuickReceiveOrder(string flowCode, IList<OrderDetail> orderDetailList, string userCode)
        {
            DateTime dateTimeNow = DateTime.Now;
            return this.QuickReceiveOrder(flowCode, orderDetailList, userCode, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML,
                dateTimeNow, dateTimeNow, false, null, null);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt QuickReceiveOrder(string flowCode, IList<OrderDetail> orderDetailList, string userCode, string orderSubType, DateTime winTime, DateTime startTime, bool isUrgent, string referenceOrderNo, string externalOrderNo)
        {
            Flow flow = this.flowMgrE.CheckAndLoadFlow(flowCode, true);
            User user = this.userMgrE.CheckAndLoadUser(userCode);
            return this.QuickReceiveOrder(flow, orderDetailList, user, orderSubType, winTime, startTime, isUrgent, referenceOrderNo, externalOrderNo);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt QuickReceiveOrder(Flow flow, IList<OrderDetail> orderDetailList, User user)
        {
            DateTime dateTimeNow = DateTime.Now;
            return this.QuickReceiveOrder(flow, orderDetailList, user, BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML,
               dateTimeNow, dateTimeNow, false, null, null);
        }

        [Transaction(TransactionMode.Requires)]
        public Receipt QuickReceiveOrder(Flow flow, IList<OrderDetail> orderDetailList, User user, string orderSubType, DateTime winTime, DateTime startTime, bool isUrgent, string referenceOrderNo, string externalOrderNo)
        {
            if (flow.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new TechnicalException("QuickReceiveOrder not support Production");
            }

            #region 缓存上架信息
            IDictionary<string, string> huIdStorageBinDic = new Dictionary<string, string>();
            foreach (OrderDetail sourceOrderDetail in orderDetailList)
            {
                if (sourceOrderDetail.HuId != null && sourceOrderDetail.HuId.Trim() != string.Empty &&
                    sourceOrderDetail.PutAwayBinCode != null && sourceOrderDetail.PutAwayBinCode.Trim() != string.Empty)
                {
                    if (!huIdStorageBinDic.ContainsKey(sourceOrderDetail.HuId.Trim()))
                    {
                        huIdStorageBinDic.Add(sourceOrderDetail.HuId.Trim(), sourceOrderDetail.PutAwayBinCode.Trim());
                    }
                    else
                    {
                        if (huIdStorageBinDic[sourceOrderDetail.HuId.Trim()] != sourceOrderDetail.PutAwayBinCode.Trim())
                        {
                            throw new BusinessErrorException("Common.Business.Error.OneHuCannotInTwoBin");
                        }
                    }
                }
            }
            #endregion

            #region 初始化订单头
            OrderHead orderHead = this.TransferFlow2Order(flow, orderSubType);

            #region 从不合格品库位退货，收货扫描一定要设为False
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                //如果不是所有明细的目的库位都是Reject，可能有问题。
                if (orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN &&
                    orderDetailList[0].DefaultLocationTo != null &&
                    orderDetailList[0].DefaultLocationTo.Code == BusinessConstants.SYSTEM_LOCATION_REJECT)
                {
                    orderHead.IsReceiptScanHu = false;
                    orderHead.LocationTo = this.locationMgrE.GetRejectLocation();
                }
            }
            #endregion

            IList<OrderDetail> targetOrderDetailList = orderHead.OrderDetails;

            if (targetOrderDetailList == null)
            {
                targetOrderDetailList = new List<OrderDetail>();
            }

            orderHead.SubType = orderSubType;
            orderHead.WindowTime = winTime;
            orderHead.StartTime = startTime;
            orderHead.ReferenceOrderNo = referenceOrderNo;
            orderHead.ExternalOrderNo = externalOrderNo;
            if (isUrgent)
            {
                orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_URGENT;
            }
            else
            {
                orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
            }

            orderHead.IsAutoRelease = true;
            orderHead.IsAutoStart = true;
            orderHead.IsAutoCreatePickList = false;
            orderHead.IsAutoShip = false;
            orderHead.IsAutoReceive = false;
            #endregion

            #region 合并OrderDetailList
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                IList<OrderDetail> newOrderDetailList = new List<OrderDetail>();
                foreach (OrderDetail sourceOrderDetail in orderDetailList)
                {
                    bool findMatch = false;

                    #region 在FlowDetail转换的OrderDetail里面查找匹配项
                    foreach (OrderDetail targetOrderDetail in targetOrderDetailList)
                    {
                        if (sourceOrderDetail.Item.Code == targetOrderDetail.Item.Code
                            && sourceOrderDetail.Uom.Code == targetOrderDetail.Uom.Code
                            && sourceOrderDetail.UnitCount == targetOrderDetail.UnitCount
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationFrom, targetOrderDetail.DefaultLocationFrom)
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationTo, targetOrderDetail.DefaultLocationTo))
                        {
                            targetOrderDetail.RequiredQty += sourceOrderDetail.OrderedQty;
                            targetOrderDetail.OrderedQty += sourceOrderDetail.OrderedQty;

                            findMatch = true;

                            break;
                        }
                    }
                    #endregion

                    if (!findMatch)
                    {
                        #region 没有找到匹配项，从新增匹配项中找
                        foreach (OrderDetail newOrderDetail in newOrderDetailList)
                        {
                            if (sourceOrderDetail.Item.Code == newOrderDetail.Item.Code
                            && sourceOrderDetail.Uom.Code == newOrderDetail.Uom.Code
                            && sourceOrderDetail.UnitCount == newOrderDetail.UnitCount
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationFrom, newOrderDetail.DefaultLocationFrom)
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationTo, newOrderDetail.DefaultLocationTo))
                            {
                                newOrderDetail.RequiredQty += sourceOrderDetail.OrderedQty;
                                newOrderDetail.OrderedQty += sourceOrderDetail.OrderedQty;

                                findMatch = true;

                                break;
                            }
                        }
                        #endregion

                        if (!findMatch)
                        {
                            #region 还没有找到匹配项，新增到newOrderDetailList中
                            OrderDetail clonedSourceOrderDetail = CloneHelper.DeepClone<OrderDetail>(sourceOrderDetail);
                            clonedSourceOrderDetail.OrderHead = orderHead;
                            newOrderDetailList.Add(clonedSourceOrderDetail);
                            #endregion
                        }
                    }
                }

                if (newOrderDetailList.Count > 0)
                {
                    #region 合并新增的OrderDetail
                    int seqInterval = int.Parse(this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL).Value);
                    int maxSeq = 0;
                    foreach (OrderDetail targetOrderDetail in targetOrderDetailList)
                    {
                        if (targetOrderDetail.Sequence > maxSeq)
                        {
                            maxSeq = targetOrderDetail.Sequence;
                        }
                    }

                    foreach (OrderDetail newOrderDetail in newOrderDetailList)
                    {
                        maxSeq += seqInterval;
                        newOrderDetail.Sequence = maxSeq;

                        orderHead.AddOrderDetail(newOrderDetail);
                    }
                    #endregion
                }
            }

            #endregion

            #region 创建订单
            this.CreateOrder(orderHead, user);
            #endregion

            #region 发货
            IList<InProcessLocationDetail> inProcessLocationDetailList = new List<InProcessLocationDetail>();
            foreach (OrderDetail sourceOrderDetail in orderDetailList)
            {
                foreach (OrderDetail targetOrderDetail in orderHead.OrderDetails)
                {
                    if (sourceOrderDetail.Item.Code == targetOrderDetail.Item.Code
                        && sourceOrderDetail.Uom.Code == targetOrderDetail.Uom.Code
                        && sourceOrderDetail.UnitCount == targetOrderDetail.UnitCount
                        && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationFrom, targetOrderDetail.DefaultLocationFrom)
                        && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationTo, targetOrderDetail.DefaultLocationTo))
                    {

                        if (orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ
                            && sourceOrderDetail.HuId != null && sourceOrderDetail.HuId.Trim() != string.Empty)
                        {
                            #region 处理按条码的调整，去掉条码，只调整原库位数量
                            InProcessLocationDetail rtnInProcessLocationDetail = new InProcessLocationDetail();

                            rtnInProcessLocationDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(targetOrderDetail.Id, BusinessConstants.IO_TYPE_OUT)[0];
                            Hu hu = this.huMgrE.CheckAndLoadHu(sourceOrderDetail.HuId);
                            rtnInProcessLocationDetail.LotNo = hu.LotNo;
                            rtnInProcessLocationDetail.Qty = 0 - hu.Qty;
                            inProcessLocationDetailList.Add(rtnInProcessLocationDetail);

                            InProcessLocationDetail adjInProcessLocationDetail = new InProcessLocationDetail();
                            adjInProcessLocationDetail.OrderLocationTransaction = rtnInProcessLocationDetail.OrderLocationTransaction;
                            adjInProcessLocationDetail.LotNo = hu.LotNo;
                            adjInProcessLocationDetail.Qty = sourceOrderDetail.OrderedQty + hu.Qty;
                            inProcessLocationDetailList.Add(adjInProcessLocationDetail);
                            #endregion
                        }
                        else
                        {
                            InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();

                            inProcessLocationDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(targetOrderDetail.Id, BusinessConstants.IO_TYPE_OUT)[0];
                            inProcessLocationDetail.HuId = sourceOrderDetail.HuId;
                            if (inProcessLocationDetail.HuId != null && inProcessLocationDetail.HuId.Trim() != string.Empty)
                            {
                                Hu hu = this.huMgrE.CheckAndLoadHu(inProcessLocationDetail.HuId);
                                inProcessLocationDetail.LotNo = hu.LotNo;

                                //设置退货上架库格
                                if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
                                {
                                    if (huIdStorageBinDic.ContainsKey(hu.HuId))
                                    {
                                        inProcessLocationDetail.ReturnPutAwaySorageBinCode = huIdStorageBinDic[hu.HuId];
                                    }
                                }
                            }
                            inProcessLocationDetail.Qty = sourceOrderDetail.OrderedQty;

                            inProcessLocationDetailList.Add(inProcessLocationDetail);

                            break;
                        }
                    }

                }
            }

            InProcessLocation inProcessLocation = this.ShipOrder(inProcessLocationDetailList, user);
            #endregion

            #region 为收货调整重新赋条码，增加目的库位的条码数量
            if (orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)
            {
                foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocation.InProcessLocationDetails)
                {
                    foreach (OrderDetail sourceOrderDetail in orderDetailList)
                    {
                        if (sourceOrderDetail.HuId != null && sourceOrderDetail.HuId.Trim() != string.Empty)
                        {
                            if (sourceOrderDetail.Item.Code == inProcessLocationDetail.OrderLocationTransaction.OrderDetail.Item.Code
                            && sourceOrderDetail.Uom.Code == inProcessLocationDetail.OrderLocationTransaction.OrderDetail.Uom.Code
                            && sourceOrderDetail.UnitCount == inProcessLocationDetail.OrderLocationTransaction.OrderDetail.UnitCount
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationFrom, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.DefaultLocationFrom)
                            && LocationHelper.IsLocationEqual(sourceOrderDetail.DefaultLocationTo, inProcessLocationDetail.OrderLocationTransaction.OrderDetail.DefaultLocationTo))
                            {

                                Hu hu = this.huMgrE.CheckAndLoadHu(sourceOrderDetail.HuId);
                                inProcessLocationDetail.HuId = hu.HuId;
                                inProcessLocationDetail.LotNo = hu.LotNo;
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 收货
            if (orderSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
            {
                return ReceiveFromInProcessLocation(inProcessLocation, user, huIdStorageBinDic, externalOrderNo);
            }
            else
            {
                return ReceiveFromInProcessLocation(inProcessLocation, user, null, externalOrderNo);
            }
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualCompleteOrder(string orderNo, string userCode)
        {
            this.ManualCompleteOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualCompleteOrder(string orderNo, User user)
        {
            OrderHead orderHead = this.orderHeadMgrE.LoadOrderHead(orderNo);

            //检查权限
            //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_COMPLETE_ORDER))
            //{
            //    throw new BusinessErrorException("Order.Error.NoCompletePermission", orderHead.OrderNo);
            //}

            //检查状态
            if (orderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenComplete", orderHead.Status, orderHead.OrderNo);
            }

            //#region 检查是否有未关闭的ASN
            //IList<InProcessLocationDetail> inProcessLocationDetailList =
            //    this.inProcessLocationDetailMgrE.GetInProcessLocationDetail(orderHead);

            //if (inProcessLocationDetailList != null && inProcessLocationDetailList.Count > 0)
            //{
            //    foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
            //    {
            //        if (inProcessLocationDetail.InProcessLocation.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            //        {
            //            throw new BusinessErrorException("Order.Error.OrderComplete.UnclosedASN", orderNo);
            //        }
            //    }
            //}
            //#endregion

            DateTime nowDate = DateTime.Now;
            orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE;
            orderHead.CompleteDate = nowDate;
            orderHead.CompleteUser = user;
            orderHead.LastModifyDate = nowDate;
            orderHead.LastModifyUser = user;

            this.orderHeadMgrE.UpdateOrderHead(orderHead);
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualCompleteOrder(OrderHead orderHead, string userCode)
        {
            this.ManualCompleteOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ManualCompleteOrder(OrderHead orderHead, User user)
        {
            this.ManualCompleteOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCompleteOrder(OrderHead orderHead, User user)
        {
            OrderHead oldOrderHead = this.orderHeadMgrE.LoadOrderHead(orderHead.OrderNo, true);
            bool isComplete = true;
            if (oldOrderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                #region 物流完工，发货数大于等于订单数
                foreach (OrderDetail orderDetail in oldOrderHead.OrderDetails)
                {
                    if (orderDetail.ShippedQty.HasValue)
                    {
                        if (orderDetail.OrderedQty > 0 && (orderDetail.OrderedQty > orderDetail.ShippedQty.Value)
                            || orderDetail.OrderedQty < 0 && (orderDetail.OrderedQty < orderDetail.ShippedQty.Value))
                        {
                            isComplete = false;
                            break;
                        }
                    }
                    else
                    {
                        isComplete = false;
                        break;
                    }
                }


                #endregion
            }
            else
            {
                #region 生产完工，收货数大于等于订单数
                foreach (OrderDetail orderDetail in oldOrderHead.OrderDetails)
                {
                    if (orderDetail.ReceivedQty.HasValue)
                    {
                        if (orderDetail.OrderedQty > 0 && (orderDetail.OrderedQty > orderDetail.ReceivedQty.Value)
                            || orderDetail.OrderedQty < 0 && (orderDetail.OrderedQty < orderDetail.ReceivedQty.Value))
                        {
                            isComplete = false;
                            break;
                        }
                    }
                    else
                    {
                        isComplete = false;
                        break;
                    }
                }
                #endregion
            }

            if (isComplete)
            {
                DateTime nowDate = DateTime.Now;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE;
                orderHead.CompleteDate = nowDate;
                orderHead.CompleteUser = user;
                orderHead.LastModifyDate = nowDate;
                orderHead.LastModifyUser = user;

                this.orderHeadMgrE.UpdateOrderHead(orderHead);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void TryCompleteOrder(IList<OrderHead> orderHeadList, User user)
        {
            foreach (OrderHead orderHead in orderHeadList)
            {
                TryCompleteOrder(orderHead, user);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void VoidOrder(OrderHead orderHead, string userCode)
        {
            VoidOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void VoidOrder(string orderNo, string userCode)
        {
            OrderHead orderHead = this.orderHeadMgrE.LoadOrderHead(orderNo, true);
            VoidOrder(orderHead, this.userMgrE.CheckAndLoadUser(userCode));
        }


        [Transaction(TransactionMode.Requires)]
        public void VoidOrder(string orderNo, User currentUser)
        {
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderNo);

            #region 存在未关闭ASN，先把asn退回
            DetachedCriteria criteria = DetachedCriteria.For<InProcessLocationDetail>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.CreateAlias("olt.OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            criteria.CreateAlias("InProcessLocation", "ip");
            criteria.Add(Expression.Eq("oh.OrderNo", orderNo));
            criteria.Add(Expression.In("ip.Status", new string[]{BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE,BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS}));


            IList<InProcessLocationDetail> ipDetailList = this.criteriaMgrE.FindAll<InProcessLocationDetail>(criteria);
           
            IList<string> ipNoList = ipDetailList.Select(ip => ip.InProcessLocation.IpNo).Distinct().ToList();
            
            if (ipNoList != null && ipNoList.Count > 0)
            {
                foreach (string ipNo in ipNoList)
                {
                  InProcessLocation ip = inProcessLocationMgrE.LoadInProcessLocation(ipNo);
                  inProcessLocationMgrE.ResolveInPorcessLocationNormal(ip, ip.GoodsReceiptGapTo, currentUser);
                }
                // throw new BusinessErrorException("Order.Error.ExistsCreateASN", orderNo, ipLocDetailList[0].InProcessLocation.IpNo);
            }
            #endregion

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE ||
                orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE ||
                orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {


                IList<OrderDetail> receivedOrderDetailList = new List<OrderDetail>();
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    decimal receivedQty = orderDetail.ReceivedQty.HasValue ? (decimal)orderDetail.ReceivedQty.Value : 0;
                    if (receivedQty > 0)
                    {
                        orderDetail.CurrentReceiveQty = 0 - receivedQty;
                        orderDetail.CurrentShipQty = 0 - receivedQty;
                        receivedOrderDetailList.Add(orderDetail);
                    }
                }
                if (receivedOrderDetailList.Count > 0)
                {
                    if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS ||
                        orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT ||
                        orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
                    {
                        #region 采购,已收的生成负数的记录,收货
                        this.ReceiveOrder(receivedOrderDetailList, currentUser);
                        #endregion
                    }
                    else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
                    {
                        #region 销售,发负数的货
                        orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS;
                        orderHead.IsAutoReceive = true;
                        this.ShipOrder(receivedOrderDetailList, currentUser);
                        #endregion
                    }
                }


                #region 更新订单
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = currentUser;
                orderHead.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID;
                this.orderHeadMgrE.UpdateOrderHead(orderHead);
                #endregion
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenDelete", orderHead.Status, orderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void VoidOrder(OrderHead orderHead, User currentUser)
        {
            VoidOrder(orderHead.OrderNo, currentUser);
        }

        [Transaction(TransactionMode.Requires)]
        public void RejectOrder(OrderHead orderHead, string userCode)
        {
            RejectOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }
        [Transaction(TransactionMode.Requires)]
        public void RejectOrder(OrderHead orderHead, User user)
        {
            RejectOrder(orderHead.OrderNo, user);
        }
        [Transaction(TransactionMode.Requires)]
        public void RejectOrder(string orderNo, string userCode)
        {
            RejectOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }
        public void RejectOrder(string orderNo, User user)
        {
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderNo);

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {

                orderHead.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_REJECTED;
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                this.orderHeadMgrE.UpdateOrderHead(orderHead);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", orderHead.Status, orderHead.OrderNo);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void ApproveOrder(OrderHead orderHead, string userCode)
        {
            ApproveOrder(orderHead.OrderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ApproveOrder(string orderNo, string userCode)
        {
            ApproveOrder(orderNo, this.userMgrE.CheckAndLoadUser(userCode));
        }

        [Transaction(TransactionMode.Requires)]
        public void ApproveOrder(OrderHead orderHead, User user)
        {
            ApproveOrder(orderHead.OrderNo, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void ApproveOrder(string orderNo, User user)
        {
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderNo);

            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {

                orderHead.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
                orderHead.IsAutoStart = true;
                orderHead.ApprovedUser = user;
                orderHead.ApprovedDate = DateTime.Now;
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = user;
                this.orderHeadMgrE.UpdateOrderHead(orderHead);
                this.ReleaseOrder(orderHead, user);
            }
            else
            {
                throw new BusinessErrorException("Order.Error.StatusErrorWhenModify", orderHead.Status, orderHead.OrderNo);
            }
        }

        #region Convert Object Methods
        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation ConvertOrderLocTransToInProcessLocation(IList<OrderLocationTransaction> orderLocTransList)
        {
            InProcessLocation inProcessLocation = new InProcessLocation();
            if (orderLocTransList != null && orderLocTransList.Count > 0)
            {
                inProcessLocation = inProcessLocationMgrE.GenerateInProcessLocation(orderLocTransList[0].OrderDetail.OrderHead);
                foreach (OrderLocationTransaction orderLocTrans in orderLocTransList)
                {
                    if (orderLocTrans.OrderDetail.RemainShippedQty > 0)
                    {
                        InProcessLocationDetail inProcessLocationDetail = new InProcessLocationDetail();
                        inProcessLocationDetail.OrderLocationTransaction = orderLocTrans;
                        inProcessLocationDetail.QtyToShip = orderLocTrans.OrderDetail.RemainShippedQty;
                        inProcessLocationDetail.Qty = inProcessLocationDetail.QtyToShip;
                        inProcessLocationDetail.InProcessLocation = inProcessLocation;

                        inProcessLocation.AddInProcessLocationDetail(inProcessLocationDetail);
                    }
                }
            }

            return inProcessLocation;
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation ConvertOrderToInProcessLocation(string orderNo)
        {
            InProcessLocation inProcessLocation = new InProcessLocation();
            IList<OrderLocationTransaction> orderLocTransList = orderLocationTransactionMgrE.GetOrderLocationTransaction(orderNo, BusinessConstants.IO_TYPE_OUT);

            return this.ConvertOrderLocTransToInProcessLocation(orderLocTransList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public InProcessLocation ConvertPickListToInProcessLocation(string pickListNo)
        {
            IList<InProcessLocationDetail> ipDetList = new List<InProcessLocationDetail>();
            IList<PickListResult> pickListResultList = pickListResultMgrE.GetPickListResult(pickListNo);
            if (pickListResultList != null && pickListResultList.Count > 0)
            {
                foreach (PickListResult pickListResult in pickListResultList)
                {
                    InProcessLocationDetail ipDet = new InProcessLocationDetail();
                    ipDet.OrderLocationTransaction = pickListResult.PickListDetail.OrderLocationTransaction;
                    ipDet.HuId = pickListResult.LocationLotDetail.Hu.HuId;
                    ipDet.LotNo = pickListResult.LocationLotDetail.LotNo;
                    ipDet.QtyToShip = pickListResult.Qty;//单位换算
                    ipDet.Qty = pickListResult.Qty;

                    ipDetList.Add(ipDet);
                }
            }

            InProcessLocation ip = inProcessLocationMgrE.GenerateInProcessLocation(ipDetList[0].OrderLocationTransaction.OrderDetail.OrderHead);
            ip.InProcessLocationDetails = ipDetList;

            return ip;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt ConvertOrderDetailToReceipt(IList<OrderDetail> orderDetailList)
        {
            Receipt receipt = new Receipt();
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderDetailList)
                {
                    ReceiptDetail receiptDetail = new ReceiptDetail();
                    receiptDetail.OrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];
                    if (orderDetail.OrderHead.IsReceiptScanHu)   //只有收货扫描条码赋值
                    {
                        receiptDetail.HuId = orderDetail.HuId;
                    }
                    //receiptDetail.ShippedQty = orderDetail.RemainShippedQty;  ShippedQty都是后台赋值的，从InProcessLocationDetail中取值
                    //receiptDetail.ReceivedQty = receiptDetail.ShippedQty;
                    receiptDetail.ReceivedQty = orderDetail.RemainShippedQty;
                    receiptDetail.Receipt = receipt;

                    //if (receiptDetail.ShippedQty != 0)//过滤已收满数量
                    if (receiptDetail.ReceivedQty != 0)//过滤已收满数量
                        receipt.AddReceiptDetail(receiptDetail);
                }
            }

            return receipt;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt ConvertInProcessLocationToReceipt(InProcessLocation inProcessLocation)
        {
            return ConvertInProcessLocationToReceipt(inProcessLocation, null, null);
        }

        public Receipt ConvertInProcessLocationToReceipt(InProcessLocation inProcessLocation, IDictionary<string, string> huIdStorageBinDic)
        {
            return ConvertInProcessLocationToReceipt(inProcessLocation, huIdStorageBinDic, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Receipt ConvertInProcessLocationToReceipt(InProcessLocation inProcessLocation, IDictionary<string, string> huIdStorageBinDic, string externalOrderNo)
        {
            Receipt receipt = new Receipt();
            receipt.ExternalReceiptNo = externalOrderNo;
            receipt.AddInProcessLocation(inProcessLocation);
            if (inProcessLocation.InProcessLocationDetails != null && inProcessLocation.InProcessLocationDetails.Count > 0)
            {
                foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocation.InProcessLocationDetails)
                {
                    OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                    OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                    OrderHead orderHead = orderDetail.OrderHead;

                    OrderLocationTransaction inOrderLocationTransaction = this.orderLocationTransactionMgrE.GetOrderLocationTransaction(orderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];

                    bool isMerge = false;
                    if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Count > 0 && !inProcessLocation.IsReceiptScanHu)
                    {
                        //如果收货不扫描条码，收货项需要根据发货项进行合并
                        foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
                        {
                            if (inOrderLocationTransaction.Id == receiptDetail.OrderLocationTransaction.Id)
                            {
                                //if (inProcessLocationDetail.IsConsignment == receiptDetail.IsConsignment
                                //    && inProcessLocationDetail.PlannedBill == receiptDetail.PlannedBill) {
                                //    throw new BusinessErrorException("寄售库存，不能按按数量进行收货。");
                                //}

                                isMerge = true;
                                receiptDetail.ShippedQty += inProcessLocationDetail.Qty;
                                receiptDetail.ReceivedQty += inProcessLocationDetail.Qty;
                                break;
                            }
                        }
                    }

                    if (!isMerge)
                    {
                        ReceiptDetail receiptDetail = new ReceiptDetail();

                        receiptDetail.OrderLocationTransaction = inOrderLocationTransaction;
                        receiptDetail.Id = inProcessLocationDetail.Id;
                        receiptDetail.ShippedQty = inProcessLocationDetail.Qty;
                        receiptDetail.ReceivedQty = inProcessLocationDetail.Qty;
                        if (inProcessLocation.IsReceiptScanHu)   //只有按条码收货才Copy条码信息
                        {
                            receiptDetail.HuId = inProcessLocationDetail.HuId;
                            receiptDetail.LotNo = inProcessLocationDetail.LotNo;

                            //上架库位赋值
                            if (inOrderLocationTransaction.OrderDetail.OrderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML
                                && huIdStorageBinDic != null
                                && receiptDetail.HuId != null
                                && receiptDetail.HuId.Trim() != string.Empty
                                && huIdStorageBinDic.ContainsKey(receiptDetail.HuId.Trim()))
                            {
                                receiptDetail.PutAwayBinCode = huIdStorageBinDic[receiptDetail.HuId.Trim()];
                            }
                        }
                        receiptDetail.IsConsignment = inProcessLocationDetail.IsConsignment;
                        receiptDetail.PlannedBill = inProcessLocationDetail.PlannedBill;
                        receiptDetail.Receipt = receipt;

                        if (receiptDetail.ShippedQty != 0)//过滤已收满数量
                        {
                            receipt.AddReceiptDetail(receiptDetail);
                        }
                    }
                }
            }

            return receipt;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> ConvertTransformerToInProcessLocationDetail(List<Transformer> transformerList)
        {
            return this.ConvertTransformerToInProcessLocationDetail(transformerList, false);
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList<InProcessLocationDetail> ConvertTransformerToInProcessLocationDetail(List<Transformer> transformerList, bool includeZero)
        {
            IList<InProcessLocationDetail> ipDetList = new List<InProcessLocationDetail>();
            InProcessLocationDetail ipDet = new InProcessLocationDetail();
            if (transformerList != null && transformerList.Count > 0)
            {
                foreach (Transformer transformer in transformerList)
                {
                    OrderLocationTransaction orderLocTrans = orderLocationTransactionMgrE.LoadOrderLocationTransaction(transformer.OrderLocTransId);
                    if (transformer.TransformerDetails != null && transformer.TransformerDetails.Count > 0)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            ipDet = new InProcessLocationDetail();
                            ipDet.OrderLocationTransaction = orderLocTrans;
                            ipDet.HuId = transformerDetail.HuId;
                            ipDet.LotNo = transformerDetail.LotNo;
                            ipDet.QtyToShip = transformerDetail.Qty;
                            ipDet.Qty = transformerDetail.CurrentQty;

                            if (ipDet.Qty != 0 || includeZero)
                                ipDetList.Add(ipDet);
                        }
                    }
                    else
                    {
                        ipDet = new InProcessLocationDetail();
                        ipDet.OrderLocationTransaction = orderLocTrans;
                        ipDet.QtyToShip = transformer.Qty;
                        ipDet.Qty = transformer.CurrentQty;

                        if (ipDet.Qty != 0 || includeZero)
                            ipDetList.Add(ipDet);
                    }
                }
            }

            return ipDetList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ReceiptDetail> ConvertTransformerToReceiptDetail(List<Transformer> transformerList)
        {
            return this.ConvertTransformerToReceiptDetail(transformerList, false);
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList<ReceiptDetail> ConvertTransformerToReceiptDetail(List<Transformer> transformerList, bool includeZero)
        {
            IList<ReceiptDetail> recDetList = new List<ReceiptDetail>();
            ReceiptDetail recDet = new ReceiptDetail();
            if (transformerList != null && transformerList.Count > 0)
            {
                foreach (Transformer transformer in transformerList)
                {
                    OrderLocationTransaction orderLocTrans = orderLocationTransactionMgrE.LoadOrderLocationTransaction(transformer.OrderLocTransId);
                    if (transformer.TransformerDetails != null && transformer.TransformerDetails.Count > 0
                        && orderLocTrans.OrderDetail.OrderHead.IsReceiptScanHu)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            recDet = new ReceiptDetail();
                            recDet.OrderLocationTransaction = orderLocTrans;
                            recDet.HuId = transformerDetail.HuId;
                            recDet.LotNo = transformerDetail.LotNo;
                            recDet.ShippedQty = transformerDetail.Qty;
                            recDet.ReceivedQty = transformerDetail.CurrentQty;
                            recDet.PutAwayBinCode = transformerDetail.StorageBinCode;

                            if (recDet.ReceivedQty != 0 || includeZero)
                                recDetList.Add(recDet);
                        }
                    }
                    else
                    {
                        recDet = new ReceiptDetail();
                        recDet.OrderLocationTransaction = orderLocTrans;
                        recDet.ShippedQty = transformer.Qty;
                        recDet.ReceivedQty = transformer.CurrentQty;

                        if (recDet.ReceivedQty != 0 || includeZero)
                            recDetList.Add(recDet);
                    }
                }
            }

            return recDetList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderHead> ConvertShiftPlanScheduleToOrders(IList<ShiftPlanSchedule> shiftPlanScheduleList)
        {
            IList<OrderHead> orderHeadList = new List<OrderHead>();
            if (shiftPlanScheduleList != null && shiftPlanScheduleList.Count > 0)
            {
                foreach (ShiftPlanSchedule sps in shiftPlanScheduleList)
                {
                    decimal orderLotSize = sps.FlowDetail.OrderLotSize.HasValue ? sps.FlowDetail.OrderLotSize.Value : 0;
                    IList<decimal> reqQtyList = OrderHelper.SplitByOrderLotSize(sps.PlanQty, orderLotSize);
                    foreach (decimal reqQty in reqQtyList)
                    {
                        double leadTime = sps.FlowDetail.Flow.LeadTime.HasValue ? Convert.ToDouble(sps.FlowDetail.Flow.LeadTime.Value) : 0;
                        OrderHead oh = new OrderHead();
                        oh = this.TransferFlow2Order(sps.FlowDetail.Flow.Code);
                        oh.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                        oh.StartTime = shiftMgrE.GetShiftStartTime(sps.ReqDate, sps.Shift.Code);
                        oh.WindowTime = shiftMgrE.GetShiftEndTime(sps.ReqDate, sps.Shift.Code);

                        oh.GetOrderDetailByFlowDetailIdAndItemCode(sps.FlowDetail.Id, sps.FlowDetail.Item.Code).RequiredQty = reqQty;
                        oh.GetOrderDetailByFlowDetailIdAndItemCode(sps.FlowDetail.Id, sps.FlowDetail.Item.Code).OrderedQty = reqQty;
                        orderHeadList.Add(oh);
                    }
                }
            }

            OrderHelper.FilterZeroOrderQty(orderHeadList);
            return orderHeadList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderHead> ConvertFlowPlanToOrders(IList<FlowPlan> flowPlanList)
        {
            IList<OrderHead> orderHeadList = new List<OrderHead>();
            if (flowPlanList != null && flowPlanList.Count > 0)
            {
                foreach (FlowPlan flowPlan in flowPlanList)
                {
                    bool isExist = false;
                    foreach (OrderHead orderHead in orderHeadList)
                    {
                        if (orderHead.Flow.Trim().ToUpper() == flowPlan.FlowDetail.Flow.Code.Trim().ToUpper())
                        {
                            orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowPlan.FlowDetail.Id, flowPlan.FlowDetail.Item.Code).RequiredQty = flowPlan.PlanQty;
                            orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowPlan.FlowDetail.Id, flowPlan.FlowDetail.Item.Code).OrderedQty = flowPlan.PlanQty;
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist)
                    {
                        double leadTime = flowPlan.FlowDetail.Flow.LeadTime.HasValue ? Convert.ToDouble(flowPlan.FlowDetail.Flow.LeadTime.Value) : 0;

                        OrderHead oh = new OrderHead();
                        oh = this.TransferFlow2Order(flowPlan.FlowDetail.Flow.Code);
                        oh.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                        oh.StartTime = flowPlan.ReqDate.AddHours(-leadTime);
                        oh.WindowTime = flowPlan.ReqDate;

                        oh.GetOrderDetailByFlowDetailIdAndItemCode(flowPlan.FlowDetail.Id, flowPlan.FlowDetail.Item.Code).RequiredQty = flowPlan.PlanQty;
                        oh.GetOrderDetailByFlowDetailIdAndItemCode(flowPlan.FlowDetail.Id, flowPlan.FlowDetail.Item.Code).OrderedQty = flowPlan.PlanQty;
                        orderHeadList.Add(oh);
                    }
                }
            }

            OrderHelper.FilterZeroOrderQty(orderHeadList);
            return orderHeadList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderHead> ConvertOrderDetailToOrders(IList<OrderDetail> orderDetailList)
        {
            if (orderDetailList == null || orderDetailList.Count == 0)
                throw new BusinessErrorException("Common.Business.Warn.Empty");

            IList<OrderHead> orderHeadList = new List<OrderHead>();
            foreach (OrderDetail orderDetail in orderDetailList)
            {
                OrderHead oh = this.TransferFlow2Order(orderDetail.FlowDetail.Flow.Code);
                oh.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                oh.StartTime = orderDetail.OrderHead.StartTime;
                oh.WindowTime = orderDetail.OrderHead.WindowTime;
                oh.GetOrderDetailBySequence(orderDetail.FlowDetail.Sequence).RequiredQty = orderDetail.RequiredQty;
                oh.GetOrderDetailBySequence(orderDetail.FlowDetail.Sequence).OrderedQty = orderDetail.RequiredQty;

                orderHeadList.Add(oh);
            }

            return orderHeadList;
        }

        #endregion

        #endregion

        #region private方法
        private InProcessLocation ShipOrder(InProcessLocation inProcessLocation, User user, bool checkOrderStatus)
        {
            #region 变量定义
            IList<InProcessLocationDetail> nonZeroInProcessLocationDetailList = new List<InProcessLocationDetail>(); //存储非0发货项
            IDictionary<string, OrderHead> cachedOrderHead = new Dictionary<string, OrderHead>();  //缓存用到的OrderHead
            #endregion

            #region 判断是否全0发货
            if (inProcessLocation.InProcessLocationDetails != null && inProcessLocation.InProcessLocationDetails.Count > 0)
            {
                //判断全0发货
                foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocation.InProcessLocationDetails)
                {
                    if (inProcessLocationDetail.Qty != 0)
                    {
                        nonZeroInProcessLocationDetailList.Add(inProcessLocationDetail);
                    }
                }

                if (nonZeroInProcessLocationDetailList.Count == 0)
                {
                    throw new BusinessErrorException("OrderDetail.Error.OrderDetailShipEmpty");
                }
                else
                {
                    inProcessLocation.InProcessLocationDetails = nonZeroInProcessLocationDetailList;
                }
            }
            else
            {
                throw new BusinessErrorException("OrderDetail.Error.OrderDetailShipEmpty");
            }
            #endregion

            #region 发货
            DoShipOrder(inProcessLocation, user, checkOrderStatus);
            #endregion

            #region 判断自动收货
            if (inProcessLocation.IsAutoReceive)
            {
                if (inProcessLocation.CompleteLatency.HasValue && inProcessLocation.CompleteLatency.Value > 0)
                {
                    //todo 收货延迟，记录到Quratz表中
                    throw new NotImplementedException("Complete Latency Not Implemented");
                }
                else
                {
                    //立即收货
                    ReceiveFromInProcessLocation(inProcessLocation, user, null, null);
                }
            }
            #endregion

            return inProcessLocation;
        }

        private void DoShipOrder(InProcessLocation inProcessLocation, User user, bool checkOrderStatus)
        {
            #region 变量定义
            IDictionary<string, OrderHead> cachedOrderHead = new Dictionary<string, OrderHead>();  //缓存用到的OrderHead
            IList<InProcessLocationDetail> batchFeedInProcessLocationDetailList = new List<InProcessLocationDetail>();
            #endregion

            #region 更新订单信息
            foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocation.InProcessLocationDetails)
            {
                OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;
                OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
                OrderHead orderHead = orderDetail.OrderHead;

                if (orderLocationTransaction.BackFlushMethod != BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED)
                {
                    #region 判断OrderHead状态并缓存
                    if (!cachedOrderHead.ContainsKey(orderHead.OrderNo))
                    {
                        //检查权限
                        //if (!OrderHelper.CheckOrderOperationAuthrize(orderHead, user, BusinessConstants.ORDER_OPERATION_SHIP_ORDER))
                        //{
                        //    throw new BusinessErrorException("Order.Error.NoShipPermission", orderHead.OrderNo);
                        //}

                        //判断OrderHead状态
                        if (checkOrderStatus && orderHead.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                        {
                            throw new BusinessErrorException("Order.Error.StatusErrorWhenShip", orderHead.Status, orderHead.OrderNo);
                        }

                        #region 整包装收货判断,快速的不要判断
                        if (orderHead.FulfillUnitCount && orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                            && orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML && !(orderHead.IsAutoRelease && orderHead.IsAutoStart))
                        {
                            if (inProcessLocationDetail.Qty % orderDetail.UnitCount != 0)
                            {
                                //不是整包装
                                throw new BusinessErrorException("Order.Error.NotFulfillUnitCountGrGi", orderDetail.Item.Code);
                            }
                        }
                        #endregion

                        //缓存OrderHead
                        cachedOrderHead.Add(orderHead.OrderNo, orderHead);
                    }
                    #endregion

                    #region 更新OrderLocationTransaction、OrderDetail数量
                    this.orderDetailMgrE.RecordOrderShipQty(orderLocationTransaction, inProcessLocationDetail, true);
                    #endregion
                }
                else
                {
                    //只有普通的工单才记录待回冲表，返工不记录。
                    if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML)
                    {

                        batchFeedInProcessLocationDetailList.Add(inProcessLocationDetail);
                    }
                }
            }
            #endregion

            #region 创建ASN
            this.inProcessLocationMgrE.CreateInProcessLocation(inProcessLocation, user);
            #endregion

            #region 新增投料回冲计划数
            if (batchFeedInProcessLocationDetailList.Count > 0)
            {
                foreach (InProcessLocationDetail inProcessLocationDetail in batchFeedInProcessLocationDetailList)
                {
                    OrderLocationTransaction orderLocationTransaction = inProcessLocationDetail.OrderLocationTransaction;

                    OrderPlannedBackflush orderPlannedBackflush = new OrderPlannedBackflush();
                    orderPlannedBackflush.OrderLocationTransaction = orderLocationTransaction;
                    orderPlannedBackflush.InProcessLocation = inProcessLocation;
                    orderPlannedBackflush.IsActive = true;
                    orderPlannedBackflush.PlannedQty = orderLocationTransaction.UnitQty * inProcessLocationDetail.Qty;

                    this.orderPlannedBackflushMgrE.CreateOrderPlannedBackflush(orderPlannedBackflush);
                }
            }
            #endregion

            #region 更新订单头信息
            DateTime dateTimeNow = DateTime.Now;
            foreach (OrderHead orderHead in cachedOrderHead.Values)
            {
                orderHead.LastModifyUser = user;
                orderHead.LastModifyDate = dateTimeNow;
                this.orderHeadMgrE.UpdateOrderHead(orderHead);
            }
            #endregion
        }

        private IList<OrderHead> CreateBindingOrder(OrderHead orderHead, User user, params string[] bindingTypes)
        {
            DateTime dateTimeNow = DateTime.Now;
            IList<OrderHead> orderHeadList = new List<OrderHead>();
            IList<OrderBinding> orderBindingList = this.orderBindingMgrE.GetOrderBinding(orderHead, bindingTypes);

            if (orderBindingList != null && orderBindingList.Count > 0)
            {
                foreach (OrderBinding orderBinding in orderBindingList)
                {
                    OrderHead bindedOrderHead = this.TransferFlow2Order(orderBinding.BindedFlow);
                    bindedOrderHead.OrderDetails = new List<OrderDetail>();

                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        IList<FlowDetail> bindedFlowDetailList = this.flowBindingMgrE.GetBindedFlowDetail(
                            orderDetail, orderBinding.BindedFlow.Code);

                        if (bindedFlowDetailList != null && bindedFlowDetailList.Count > 0)
                        {
                            foreach (FlowDetail bindedFlowDetail in bindedFlowDetailList)
                            {
                                OrderDetail binedOrderDetail = bindedOrderHead.GetOrderDetailBySequence(bindedFlowDetail.Sequence);
                                if (binedOrderDetail != null)
                                {
                                    #region 合并相同的零件
                                    //对于生产，相同的零件可以出现在不同的工序里，绑定的时候需要合并
                                    if (binedOrderDetail.Uom.Code != bindedFlowDetail.Uom.Code)
                                    {
                                        decimal qty = this.uomConversionMgrE.ConvertUomQty(binedOrderDetail.Item, bindedFlowDetail.Uom, bindedFlowDetail.OrderedQty, binedOrderDetail.Uom);
                                        binedOrderDetail.OrderedQty += qty;
                                        binedOrderDetail.RequiredQty += qty;
                                    }
                                    else
                                    {
                                        binedOrderDetail.OrderedQty += bindedFlowDetail.OrderedQty;
                                        binedOrderDetail.RequiredQty += bindedFlowDetail.OrderedQty;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    bool isReferenceFlow = (bindedFlowDetail.Flow.Code != orderBinding.BindedFlow.Code);
                                    IList<OrderDetail> newOrderDetailList = this.orderDetailMgrE.GenerateOrderDetail(bindedOrderHead, bindedFlowDetail, isReferenceFlow);
                                    if (newOrderDetailList.Count == 1)
                                    {
                                        newOrderDetailList[0].OrderedQty = bindedFlowDetail.OrderedQty;
                                        newOrderDetailList[0].RequiredQty = bindedFlowDetail.OrderedQty;
                                    }
                                    else if (newOrderDetailList.Count > 1)
                                    {
                                        #region 处理套件的RequiredQty、OrderedQty
                                        IList<ItemKit> itemKitList = this.itemKitMgrE.GetChildItemKit(bindedFlowDetail.Item.Code);

                                        decimal? convertRate = null;
                                        foreach (ItemKit itemKit in itemKitList)
                                        {
                                            if (!convertRate.HasValue)
                                            {
                                                if (itemKit.ParentItem.Uom.Code != bindedFlowDetail.Uom.Code)
                                                {
                                                    convertRate = this.uomConversionMgrE.ConvertUomQty(orderDetail.Item, bindedFlowDetail.Uom, bindedFlowDetail.OrderedQty, itemKit.ParentItem.Uom);
                                                }
                                                else
                                                {
                                                    convertRate = 1;
                                                }
                                            }

                                            foreach (OrderDetail newOrderDetail in newOrderDetailList)
                                            {
                                                if (newOrderDetail.Item.Code == itemKit.ChildItem.Code)
                                                {
                                                    newOrderDetail.RequiredQty = bindedFlowDetail.OrderedQty * itemKit.Qty * convertRate.Value;
                                                    newOrderDetail.OrderedQty = bindedFlowDetail.OrderedQty * itemKit.Qty * convertRate.Value;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                    }

                    if (bindedOrderHead.OrderDetails.Count > 0)
                    {
                        Decimal leadTime = orderBinding.BindedFlow.LeadTime.HasValue ? orderBinding.BindedFlow.LeadTime.Value : 0;
                        bindedOrderHead.ReferenceOrderNo = orderHead.OrderNo;
                        bindedOrderHead.StartTime = dateTimeNow;
                        bindedOrderHead.WindowTime = orderHead.StartTime.AddHours(double.Parse(leadTime.ToString()));
                        bindedOrderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                        if (orderBinding.BindingType == BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_RECEIVE_SYN)
                        {
                            bindedOrderHead.IsAutoRelease = true;
                            bindedOrderHead.IsAutoStart = true;
                            bindedOrderHead.IsAutoShip = true;
                            bindedOrderHead.IsAutoReceive = true;
                            bindedOrderHead.StartLatency = 0;
                            bindedOrderHead.CompleteLatency = 0;
                        }
                        else if (orderBinding.BindingType == BusinessConstants.CODE_MASTER_BINDING_TYPE_VALUE_RECEIVE_ASYN)
                        {
                            bindedOrderHead.IsAutoRelease = true;
                            bindedOrderHead.StartLatency = 0;
                        }
                        this.CreateOrder(bindedOrderHead, user);

                        orderBinding.BindedOrderHead = bindedOrderHead;
                        this.orderBindingMgrE.UpdateOrderBinding(orderBinding);

                        orderHeadList.Add(bindedOrderHead);
                    }
                }
            }

            return orderHeadList;
        }

        private void CreateOrderDetailSubsidiary(OrderDetail orderDetail)
        {
            //CheckDetOpt选项
            if (orderDetailMgrE.CheckOrderDet(orderDetail))
            {
                orderDetailMgrE.CreateOrderDetail(orderDetail);

                if (orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                {
                    foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                    {
                        orderLocationTransactionMgrE.CreateOrderLocationTransaction(orderLocationTransaction);
                    }
                }

                if (orderDetail.OrderTracers != null && orderDetail.OrderTracers.Count > 0)
                {
                    foreach (OrderTracer orderTracer in orderDetail.OrderTracers)
                    {
                        foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                        {
                            if (orderLocationTransaction.IOType == BusinessConstants.IO_TYPE_OUT)
                            {
                                orderTracer.RefOrderLocTransId = orderLocationTransaction.Id;
                                break;
                            }
                        }

                        orderTracerMgrE.CreateOrderTracer(orderTracer);
                    }
                }
            }
            else
            {
                throw new BusinessErrorException("Master.CheckDetOpt.Error");
            }
        }

        /**
        * 分摊OrderHead头折扣 
        */
        private void AllocateOrderHeadDicount(OrderHead orderHead)
        {

            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING
                || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                #region 折扣分摊至明细
                EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                int decimalLength = int.Parse(entityPreference.Value);

                IList<OrderDetail> noneZeroUnitPriceFromOrderDetailList = new List<OrderDetail>();
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.UnitPrice != Decimal.Zero)
                    {
                        noneZeroUnitPriceFromOrderDetailList.Add(orderDetail);
                    }
                }

                decimal orderDiscount = orderHead.Discount.HasValue ? orderHead.Discount.Value : 0;
                decimal remainDiscount = orderDiscount;
                for (int i = 0; i < noneZeroUnitPriceFromOrderDetailList.Count; i++)
                {
                    OrderDetail orderDetail = orderDetailMgrE.LoadOrderDetail(noneZeroUnitPriceFromOrderDetailList[i].Id);

                    if (i < noneZeroUnitPriceFromOrderDetailList.Count - 1)
                    {
                        if (orderHead.OrderDetailAmountAfterDiscount != Decimal.Zero)
                        {
                            orderDetail.HeadDiscount = Math.Round(orderDiscount * orderDetail.OrderDetailAmountAfterDiscount / orderHead.OrderDetailAmountAfterDiscount, decimalLength, MidpointRounding.AwayFromZero);
                        }

                        if (orderDetail.HeadDiscount.HasValue)
                        {
                            remainDiscount -= orderDetail.HeadDiscount.Value;
                        }
                    }
                    else
                    {
                        orderDetail.HeadDiscount = remainDiscount;
                    }

                    if (orderDetail.UnitPrice.HasValue)
                    {
                        orderDetail.UnitPriceAfterDiscount = (orderDetail.UnitPrice * orderDetail.OrderedQty
                           - (orderDetail.Discount.HasValue ? orderDetail.Discount.Value : 0)
                           - (orderDetail.HeadDiscount.HasValue ? orderDetail.HeadDiscount.Value : 0))
                           / orderDetail.OrderedQty;

                        if (orderDetail.UnitPriceAfterDiscount != null && orderDetail.UnitPriceAfterDiscount.HasValue)
                        {
                            orderDetail.UnitPriceAfterDiscount = Math.Round(orderDetail.UnitPriceAfterDiscount.Value, decimalLength, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            orderDetail.UnitPriceAfterDiscount = 0;
                        }
                    }
                    orderDetail.IsIncludeTax = orderDetail.DefaultPriceList.IsIncludeTax;

                    this.orderDetailMgrE.UpdateOrderDetail(orderDetail);
                }
                #endregion
            }
        }

        private void CalculatePlannedAmount(OrderDetail orderDetail, ReceiptDetail receiptDetail, decimal? totalAmount)
        {
            if (totalAmount.HasValue && totalAmount.Value != 0)
            {
                EntityPreference decimalLengthEntityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
                int decimalLength = int.Parse(decimalLengthEntityPreference.Value);
                decimal actualUnitPrice = Math.Round((totalAmount.Value / orderDetail.OrderedQty), decimalLength, MidpointRounding.AwayFromZero);

                if (orderDetail.ReceivedQty.Value + receiptDetail.ReceivedQty.Value > orderDetail.OrderedQty)
                {
                    //已收数 + 本次收货数大于订单数量
                    if (orderDetail.ReceivedQty.Value >= orderDetail.OrderedQty)
                    {
                        //已收数大于订单数量，PlannedAmount = 剩余订单Amount + 实际单价 * 超出订单的数量
                        receiptDetail.PlannedAmount = totalAmount.Value - actualUnitPrice * orderDetail.ReceivedQty.Value;
                        receiptDetail.PlannedAmount += actualUnitPrice * (orderDetail.ReceivedQty.Value + receiptDetail.ReceivedQty.Value - orderDetail.OrderedQty);
                    }
                    else
                    {
                        //已收数大于订单数量，PlannedAmount = 实际单价 * 本次收货数量
                        receiptDetail.PlannedAmount = actualUnitPrice * receiptDetail.ReceivedQty.Value;
                    }
                }
                else if (orderDetail.ReceivedQty + receiptDetail.ReceivedQty.Value == orderDetail.OrderedQty)
                {
                    //已收数 + 本次收货数正好等于订单数量
                    receiptDetail.PlannedAmount = totalAmount.Value - actualUnitPrice * orderDetail.ReceivedQty.Value;
                }
                else
                {
                    //已收数 + 本次收货数小于订单数量
                    receiptDetail.PlannedAmount = actualUnitPrice * receiptDetail.ReceivedQty.Value;
                }
            }
        }

        private Receipt ReceiveFromInProcessLocation(InProcessLocation inProcessLocation, User user, IDictionary<string, string> huIdStorageBinDic, string externalOrderNo)
        {
            if (inProcessLocation.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new TechnicalException("production can't auto receive after shipping raw marterial");
            }

            Receipt receipt = this.ConvertInProcessLocationToReceipt(inProcessLocation, huIdStorageBinDic, externalOrderNo);
            return this.ReceiveOrder(receipt, user);
        }

        private int GetInPorcessWOCount(string flowCode, User user)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderHead>();
            criteria.SetProjection(Projections.Count("OrderNo"));

            criteria.Add(Expression.Eq("Flow", flowCode));
            criteria.Add(Expression.Eq("StartUser", user));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));

            return this.criteriaMgrE.FindAll<int>(criteria)[0];
        }

        [Transaction(TransactionMode.Requires)]
        public void RecalculatePrice(string orderNo, User user)
        {
            if (orderNo != null && orderNo.Length > 0)
            {
                DateTime dateTimeNow = DateTime.Now;

                OrderHead orderHead = orderHeadMgrE.LoadOrderHead(orderNo, true);

                //订单新疆状态，并且未同意
                if (orderHead != null
                    //&& orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
                    //&& orderHead.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING
                    && (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                            || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING))
                {

                    IList<OrderDetail> orderDetails = orderHead.OrderDetails;
                    //计算是否有收过货
                    decimal? receivedQty = orderDetails.Where(od => od.ReceivedQty.HasValue).Sum(od => od.ReceivedQty);
                    if (receivedQty.HasValue && receivedQty.Value > 0)
                    {
                        throw new BusinessErrorException("Order.Error.ReceivedOrdersForGoodsCanNotRepricing", orderHead.OrderNo);
                    }

                    if (orderHead.Flow != null && orderHead.Flow.Trim().Length > 0)
                    {
                        Flow flow = this.flowMgrE.LoadFlow(orderHead.Flow);
                        if (flow != null)
                        {
                            if (flow.Currency != null)
                            {
                                orderHead.Currency = flow.Currency;
                            }
                            if (flow.PriceList != null)
                            {
                                orderHead.PriceList = flow.PriceList;
                            }
                        }
                    }


                    string taxCodeT = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_TAX_RATE).Value;
                    PriceList priceList = orderHead.PriceList;
                    orderHead.TaxCode = taxCodeT;
                    orderHead.IsIncludeTax = priceList.IsIncludeTax;

                    foreach (OrderDetail orderDetail in orderDetails)
                    {
                        if (orderDetail.DefaultPriceList != null)
                        {
                            PriceListDetail priceListDetail = priceListDetailMgrE.GetLastestPriceListDetail(orderDetail.DefaultPriceList, orderDetail.Item, orderHead.StartTime, orderHead.Currency, orderDetail.Uom);
                            orderDetail.IsProvisionalEstimate = priceListDetail == null ? true : priceListDetail.IsProvisionalEstimate;
                            if (priceListDetail != null)
                            {
                                orderDetail.UnitPrice = priceListDetail.UnitPrice;
                                orderDetail.TaxCode = taxCodeT;
                                orderDetail.IsIncludeTax = priceListDetail.IsIncludeTax;
                                orderDetail.Discount = 0;
                                orderDetailMgrE.UpdateOrderDetail(orderDetail);
                            }
                        }
                    }

                    orderHead.LastModifyDate = dateTimeNow;
                    orderHead.LastModifyUser = user;
                    orderHeadMgrE.UpdateOrderHead(orderHead);

                    //重新分摊订单头折扣
                    this.AllocateOrderHeadDicount(orderHead);
                }
            }
        }

        #endregion
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderMgrE : com.Sconit.Service.MasterData.Impl.OrderMgr, IOrderMgrE
    {

    }
}

#endregion