using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using LeanEngine.Entity;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using System.Collections;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

public partial class Order_OrderGoods_List : ListModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public Queue<PFlow> flowQueue { get; set; }
    public int Count { get; set; }

    public override void UpdateView()
    {

    }

    public void UpdateView(List<Orders> orders)
    {
        #region 过滤掉没有需求的Flow
        this.LV_List.DataSource = null;
        if (orders != null && orders.Count > 0)
        {
            List<Orders> targetOrders = new List<Orders>();
            foreach (Orders order in orders)
            {
                if (order.ItemFlows != null && order.ItemFlows.Count > 0)
                {
                    targetOrders.Add(order);
                }
            }

            if (targetOrders.Count > 0)
            {
                this.LV_List.DataSource = targetOrders;
            }
        }
        this.flowQueue = new Queue<PFlow>();
        this.Count = -1;
        this.LV_List.DataBind();
        #endregion
    }

    public void LV_List_OnItemDataBound(object sender, ListViewItemEventArgs e)
    {
        Orders boundOrder = (Orders)((ListViewDataItem)(e.Item)).DataItem;

        PFlow flow = new PFlow();
        flowQueue.Enqueue(flow);

        flow.FlowCode = boundOrder.Flow.Code;
        flow.PartyFrom = boundOrder.Flow.PartyFrom;

        foreach (ItemFlow itemFlow in boundOrder.ItemFlows)
        {
            IList<string> orderedPOList = new List<string>();
            PItem item = new PItem();
            item.Code = itemFlow.Item;
            Item i = TheItemMgr.LoadItem(item.Code);
            item.Desc = i.Desc1;
            item.Spec = i.Spec;

            var demand = itemFlow.OrderTracers.Where(o => o.TracerType == LeanEngine.Utility.Enumerators.TracerType.Demand).ToList();
            if (demand.Count != null && demand.Count > 0)
            {
                item.SaveStock = demand[0].Qty;
            }

            var onhandInv = itemFlow.OrderTracers.Where(o => o.TracerType == LeanEngine.Utility.Enumerators.TracerType.OnhandInv).ToList();
            if (onhandInv.Count != null && onhandInv.Count > 0)
            {
                item.Inventory = onhandInv[0].Qty;
            }

            var orderIss = itemFlow.OrderTracers.Where(o => o.TracerType == LeanEngine.Utility.Enumerators.TracerType.OrderIss).ToList();
            if (orderIss != null && orderIss.Count > 0)
            {
                foreach (OrderTracer ot in orderIss)
                {
                    string hql = @"select od from OrderDetail od inner join od.OrderHead oh where od.Item.Code = ? and oh.OrderNo = ? '";

                    IList<OrderDetail> orderDetailList = this.TheCriteriaMgr.FindAllWithHql<OrderDetail>(hql, new object[] { ot.Item, ot.Code });

                    if (orderDetailList != null && orderDetailList.Count > 0)
                    {
                        OrderDetail orderDetail = orderDetailList[0];
                        OrderHead orderHead = orderDetail.OrderHead;
                        POrder pOrder = new POrder();

                        pOrder.OrderNo = orderHead.OrderNo;
                        pOrder.CustomerCode = orderHead.PartyTo.Code;
                        pOrder.CustomerName = orderHead.PartyTo.Name;
                        pOrder.NeedPrepayment = orderHead.PartyTo.BoolField1.HasValue ? orderHead.PartyTo.BoolField1.Value : false;
                        pOrder.HasPrepayed = orderHead.BoolField1;
                        pOrder.RequiredQty = orderDetail.OrderedQty;
                        pOrder.ShippedQty = orderDetail.ShippedQty.HasValue ? orderDetail.ShippedQty.Value : 0;
                        pOrder.DeliverDate = orderHead.WindowTime;


                        hql = @"select oh.OrderNo, ot.Qty from OrderTracer as ot, OrderLocationTransaction as olt inner join olt.OrderDetail as od inner join od.OrderHead as oh where olt.Id = ot.RefOrderLocTransId and ot.OrderDetail.Id = ?";
                        IList<object[]> qtyList = this.TheCriteriaMgr.FindAllWithHql<object[]>(hql, new object[] { orderDetail.Id });
                        if (qtyList != null && qtyList.Count > 0)
                        {
                            foreach (object[] obj in qtyList)
                            {
                                orderedPOList.Add((string)obj[0]);
                                pOrder.OrderedQty += (decimal)obj[1];
                            }
                        }
                        else
                        {
                            pOrder.OrderedQty = 0;
                        }

                        if (pOrder.RequiredQty > pOrder.ShippedQty && pOrder.RequiredQty > pOrder.OrderedQty)
                        {
                            item.AddOrder(pOrder);
                        }
                    }
                }
            }

            var orderRct = itemFlow.OrderTracers.Where(o => o.TracerType == LeanEngine.Utility.Enumerators.TracerType.OrderRct).ToList();
            if (orderRct != null && orderRct.Count > 0)
            {
                foreach (OrderTracer ot in orderRct)
                {
                    if (!orderedPOList.Contains(ot.Code))
                    {
                        item.OrderedQty += ot.Qty;
                    }
                }
            }

            if ((item.OrderList != null && item.OrderList.Count > 0) || (item.SaveStock > (item.Inventory + item.OrderedQty)))
            {
                flow.AddItem(item);
            }
        }
    }

    protected void lbPur_Click(object sender, EventArgs e)
    {
        HiddenField hf_Flow = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_Flow");
        HiddenField hf_OrderNo = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_OrderNo");
        HiddenField hf_OrderItem = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_OrderItem");
        TextBox tb_CurrQty = (TextBox)((WebControl)sender).NamingContainer.FindControl("tb_CurrQty");
        HiddenField hf_Item = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_Item");
        TextBox tb_InventoryQty = (TextBox)((WebControl)sender).NamingContainer.FindControl("tb_InventoryQty");

        string flowCode = hf_Flow.Value.Split(',')[1];
        com.Sconit.Entity.MasterData.Flow flow = this.TheFlowMgr.LoadFlow(flowCode, true);
        OrderHead orderHead = this.TheOrderMgr.TransferFlow2Order(flow);
        orderHead.StartTime = DateTime.Now;
        orderHead.WindowTime = flow.LeadTime.HasValue ? DateTime.Now.AddHours(double.Parse(flow.LeadTime.Value.ToString())) : DateTime.Now;
        orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
        orderHead.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;

        if (hf_OrderNo != null && hf_OrderItem != null)
        {
            string[] orderNoArry = hf_OrderNo.Value.Split(',');
            string[] orderItemArry = hf_OrderItem.Value.Split(',');
            string[] orderQtyArry = tb_CurrQty.Text.Split(',');
            for (int i = 0; i < orderQtyArry.Length; i++)
            {
                string soOrderQty = orderQtyArry[i];
                if (soOrderQty != null && soOrderQty != string.Empty)
                {
                    int iSoOrderQty = 0;
                    try
                    {
                        iSoOrderQty = int.Parse(soOrderQty);
                    }
                    catch (FormatException)
                    {
                        break;
                    }
                    string soOrderNo = orderNoArry[i + 1];
                    string soItem = orderItemArry[i + 1];

                    if (iSoOrderQty != 0)
                    {
                        if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                        {
                            OrderDetail orderDetail = orderHead.OrderDetails.SingleOrDefault(j => j.Item.Code == soItem);

                            orderDetail.RequiredQty += iSoOrderQty;
                            orderDetail.OrderedQty += iSoOrderQty;

                            com.Sconit.Entity.Procurement.OrderTracer orderTracer = new com.Sconit.Entity.Procurement.OrderTracer();
                            orderTracer.Code = soOrderNo;
                            orderTracer.Item = soItem;
                            orderTracer.OrderDetail = TheOrderDetailMgr.GetOrderDetail(soOrderNo, soItem)[0];
                            orderTracer.OrderedQty = iSoOrderQty;
                            orderTracer.Qty = iSoOrderQty;
                            //OrderLocationTransaction poOrderLocationTransaction = TheOrderLocationTransactionMgr.GetOrderLocationTransaction(soOrderNo, soItem, BusinessConstants.IO_TYPE_IN)[0];

                            //orderTracer.RefOrderLocTransId = poOrderLocationTransaction.Id;
                            orderTracer.ReqTime = DateTime.Now;
                            orderTracer.TracerType = LeanEngine.Utility.Enumerators.TracerType.OrderRct.ToString();

                            orderDetail.AddOrderTracer(orderTracer);

                            //orderHead.AddOrderDetail(orderDetail);
                        }
                    }
                }
            }
        }

        string[] invItemArry = hf_Item.Value.Split(',');
        string[] invQtyArry = tb_InventoryQty.Text.Split(',');
        for (int i = 0; i < invQtyArry.Length - 1; i++)
        {
            string invItem = invItemArry[i + 1];
            string invQty = invQtyArry[i + 1];

            if (invQty != null && invQty != string.Empty)
            {
                int iInvQty = 0;
                try
                {
                    iInvQty = int.Parse(invQty);
                }
                catch (FormatException)
                {
                    break;
                }

                if (iInvQty != 0)
                {
                    if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                    {
                        OrderDetail orderDetail = orderHead.OrderDetails.SingleOrDefault(j => j.Item.Code == invItem);

                        orderDetail.RequiredQty += iInvQty;
                        orderDetail.OrderedQty += iInvQty;
                    }
                }
            }
        }

        try
        {
            TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);

            this.Session["Temp_Session_OrderNo"] = orderHead.OrderNo;
            string url = "~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-4";
            Response.Redirect(url);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void lbPur2_Click(object sender, EventArgs e)
    {
        HiddenField hf_Flow = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_Flow");
        HiddenField hf_Item2 = (HiddenField)((WebControl)sender).NamingContainer.FindControl("hf_Item2");
        TextBox tb_InventoryQty2 = (TextBox)((WebControl)sender).NamingContainer.FindControl("tb_InventoryQty2");

        string flowCode = hf_Flow.Value.Split(',')[1];
        com.Sconit.Entity.MasterData.Flow flow = this.TheFlowMgr.LoadFlow(flowCode, true);
        OrderHead orderHead = this.TheOrderMgr.TransferFlow2Order(flow);
        orderHead.StartTime = DateTime.Now;
        orderHead.WindowTime = flow.LeadTime.HasValue ? DateTime.Now.AddHours(double.Parse(flow.LeadTime.Value.ToString())) : DateTime.Now;
        orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
        orderHead.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;

        string[] invItemArry = hf_Item2.Value.Split(',');
        string[] invQtyArry = tb_InventoryQty2.Text.Split(',');
        for (int i = 0; i < invQtyArry.Length; i++)
        {
            string invItem = invItemArry[i + 1];
            string invQty = invQtyArry[i];

            if (invQty != null && invQty != string.Empty)
            {
                int iInvQty = 0;
                try
                {
                    iInvQty = int.Parse(invQty);
                }
                catch (FormatException)
                {
                    break;
                }

                if (iInvQty != 0)
                {
                    if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
                    {
                        OrderDetail orderDetail = orderHead.OrderDetails.SingleOrDefault(j => j.Item.Code == invItem);

                        orderDetail.RequiredQty += iInvQty;
                        orderDetail.OrderedQty += iInvQty;
                    }
                }
            }
        }

        try
        {
            TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);

            this.Session["Temp_Session_OrderNo"] = orderHead.OrderNo;
            string url = "~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-4";
            Response.Redirect(url);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }
}

public class PFlow
{
    public string FlowCode { get; set; }
    public string PartyFrom { get; set; }
    public IList<PItem> ItemList { get; set; }

    public void AddItem(PItem item)
    {
        if (ItemList == null)
        {
            ItemList = new List<PItem>();
        }

        ItemList.Add(item);
    }

    public int RowCount
    {
        get
        {
            int rowCount = 0;
            if (ItemList != null)
            {
                foreach (PItem item in ItemList)
                {
                    rowCount += item.RowCount;
                }
            }
            return rowCount;
        }
    }
}

public class PItem
{
    public string Code { get; set; }
    public string Spec { get; set; }
    public string Desc { get; set; }
    public decimal Inventory { get; set; }
    public decimal SaveStock { get; set; }
    public decimal OrderedQty { get; set; }
    public IList<POrder> OrderList { get; set; }

    public void AddOrder(POrder order)
    {
        if (OrderList == null)
        {
            OrderList = new List<POrder>();
        }

        OrderList.Add(order);
    }

    public int RowCount
    {
        get
        {
            if (OrderList != null && OrderList.Count > 0)
            {
                return OrderList.Count;
            }
            else
            {
                return 1;
            }
        }
    }
}

public class POrder
{
    public string OrderNo { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public bool NeedPrepayment { get; set; }
    public bool HasPrepayed { get; set; }
    public decimal RequiredQty { get; set; }
    public decimal ShippedQty { get; set; }
    public decimal OrderedQty { get; set; }
    public DateTime DeliverDate { get; set; }
}
