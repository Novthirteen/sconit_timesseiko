using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Utility;

public partial class Order_OrderHead_List : ListModuleBase
{
    public EventHandler EditEvent;
    public bool isGroup
    {
        get { return ViewState["isGroup"] == null ? true : (bool)ViewState["isGroup"]; }
        set { ViewState["isGroup"] = value; }
    }
    public bool isExport
    {
        get { return ViewState["isExport"] == null ? false : (bool)ViewState["isExport"]; }
        set { ViewState["isExport"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        this.isExport = false;
        if (isGroup)
        {
            this.GV_List.Execute();
            this.GV_List.Visible = true;
            this.gp.Visible = true;
            this.GV_List_Detail.Visible = false;
            this.gp_Detail.Visible = false;
        }
        else
        {
            this.GV_List_Detail.Execute();
            this.GV_List.Visible = false;
            this.GV_List_Detail.Visible = true;
            this.gp.Visible = false;
            this.gp_Detail.Visible = true;
            HiddenColumns(this.GV_List_Detail);
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string orderNo = ((LinkButton)sender).CommandArgument;
            EditEvent(orderNo, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string orderNo = ((LinkButton)sender).CommandArgument;
        try
        {
            TheOrderMgr.DeleteOrder(orderNo, this.CurrentUser);
            ShowSuccessMessage("Order.DeleteOrder.Successfully", orderNo);
            UpdateView();
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        OrderHead orderHead = (OrderHead)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!isExport)
            {
                e.Row.Cells[1].Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor=this.style.borderColor");
                e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=e");
                e.Row.Cells[1].Attributes.Add("title", GetDetail(orderHead));
            }
            if (orderHead.Status == null ||
                orderHead.Status == string.Empty ||
                orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                if (lbtnDelete != null)
                {
                    lbtnDelete.Visible = true;
                }
            }
            Label lblWinTime = (Label)e.Row.FindControl("lblWinTime");
            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
                || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT
                || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                lblWinTime.ForeColor = OrderHelper.GetWinTimeColor(orderHead.StartTime, orderHead.WindowTime);
            }

            Label tbOrderExcludeTaxPrice = (Label)e.Row.FindControl("tbOrderExcludeTaxPrice");
            tbOrderExcludeTaxPrice.Text = orderHead.OrderExcludeTaxPrice.ToString("0.00");

            Label tbOrderIncludeTaxPrice = (Label)e.Row.FindControl("tbOrderIncludeTaxPrice");
            tbOrderIncludeTaxPrice.Text = orderHead.OrderIncludeTaxPrice.ToString("0.00");
            
        }
    }

    protected void GV_List_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
             
            OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;
            Label lblUnitPrice = (Label)e.Row.FindControl("lblUnitPrice");
            Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
            
            if (orderDetail.UnitPriceAfterDiscount.HasValue)
            {
                lblUnitPrice.Text = orderDetail.UnitPriceAfterDiscount.Value.ToString("0.00");//orderDetail.TotalAmountFrom / orderDetail.OrderedQty).Value.ToString("0.00");

                if(orderDetail.OrderedQty != 0)
                {
                    lblTotalAmount.Text = (orderDetail.UnitPriceAfterDiscount * orderDetail.OrderedQty).Value.ToString("0.00");
                }else
                {
                    lblTotalAmount.Text = string.Empty;
                }
            }
            else if(orderDetail.UnitPrice.HasValue)
            {
                lblUnitPrice.Text = orderDetail.UnitPrice.Value.ToString("0.00");

                if(orderDetail.OrderedQty != 0)
                {
                    lblTotalAmount.Text = (orderDetail.UnitPrice * orderDetail.OrderedQty).Value.ToString("0.00");
                }else
                {
                    lblTotalAmount.Text = string.Empty;
                }
            }else
            {
                lblUnitPrice.Text = string.Empty;
                lblTotalAmount.Text = string.Empty;
            }
            
            if (isExport)
            {
                e.Row.Cells[8].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[9].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[10].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[11].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }

    public void Export()
    {
        string dateTime = DateTime.Now.ToString("ddhhmmss");
        this.isExport = true;
        if (isGroup)
        {
            if (GV_List.FindPager().RecordCount > 0)
            {
                GV_List.Columns.RemoveAt(GV_List.Columns.Count - 1);
            }
            this.ExportXLS(GV_List, "ProcurementGroup" + dateTime + ".xls");
        }
        else
        {
            this.ExportXLS(GV_List_Detail, "ProcurementDetail" + dateTime + ".xls");
        }
    }

    private string GetDetail(OrderHead orderHead)
    {
        string detail = string.Empty;
        detail += "cssbody=[obbd] cssheader=[obhd] header=[" + orderHead.OrderNo + " | " + (orderHead.Flow == null ? string.Empty : orderHead.Flow) + "] body=[<table width=100%>";
        System.Collections.Generic.IList<OrderDetail> ods = orderHead.OrderDetails;
        foreach (OrderDetail od in ods)
        {
            string ItemCode = od.Item.Code;
            string ItemSpec = od.Item.Spec.Replace("[", "&#91;").Replace("]", "&#93;");
            string OrderQty = od.OrderedQty.ToString("0.########");
            string Uom = od.Uom.Code;
            string RecQty = od.ReceivedQty == null ? "0" : od.ReceivedQty.Value.ToString("0.########");
            string TotalAmount = od.UnitPriceAfterDiscount.HasValue ? od.UnitPriceAfterDiscount.Value.ToString("F2") : "0";// od.IncludeTaxTotalPrice == null ? "0" : od.IncludeTaxTotalPrice.ToString("F2");
            detail += "<tr><td>" + ItemCode + "</td><td>" + ItemSpec + "</td><td>" + OrderQty + "</td><td>" + Uom + "</td><td>" + RecQty + "</td><td>" + TotalAmount + "</td></tr>";
        }
        detail += "</table>]";
        return detail;
    }

    private void HiddenColumns(GridView gridView)
    {
        bool lblReferenceItemHasValue = false;

        foreach (GridViewRow row in gridView.Rows)
        {
            Label lblReferenceItem = (Label)row.FindControl("lblReferenceItem");
            if (lblReferenceItem.Text != string.Empty)
            {
                lblReferenceItemHasValue = true;
                break;
            }
        }
        gridView.Columns[11].Visible = lblReferenceItemHasValue;
    }
}
