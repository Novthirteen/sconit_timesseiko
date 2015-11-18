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
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Service.Ext.MasterData;

public partial class Order_GoodsReceipt_AsnReceipt_List : ListModuleBase
{
    public EventHandler ViewEvent;
    public EventHandler EditEvent;
    public EventHandler CloseEvent;

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }
    public string Action
    {
        get { return (string)ViewState["Action"]; }
        set { ViewState["Action"] = value; }
    }
    public string AsnType
    {
        get { return (string)ViewState["AsnType"]; }
        set { ViewState["AsnType"] = value; }
    }
    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }
    public bool IsExport
    {
        get { return ViewState["IsExport"] != null ? (bool)ViewState["IsExport"] : false; }
        set { ViewState["IsExport"] = value; }
    }
    public bool IsGroup
    {
        get { return ViewState["IsGroup"] != null ? (bool)ViewState["IsGroup"] : false; }
        set { ViewState["IsGroup"] = value; }
    }

    public override void UpdateView()
    {
        InitUI();
        if (!IsExport)
        {
            if (IsGroup)
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
            }
        }
        else
        {
            string dateTime = DateTime.Now.ToString("ddhhmmss");

            if (IsGroup)
            {
                if (GV_List.Rows.Count > 0)
                {
                    GV_List.Columns.RemoveAt(GV_List.Columns.Count - 1);
                }
                this.ExportXLS(GV_List, "ASNGroup" + dateTime + ".xls");
            }
            else
            {
                this.ExportXLS(GV_List_Detail, "ASNGroup" + dateTime + ".xls");
            }
        }
    }

    private void InitUI()
    {
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.GV_List_Detail.Columns[9].Visible = false; //来源库位

        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {

            this.GV_List_Detail.Columns[10].Visible = false; //目的库位
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (this.Action == "Receive")
            {
                this.GV_List.Columns[2].Visible = false;  //类型
                this.GV_List.Columns[9].Visible = false;  //状态
            }
            if (IsSupplier)
            {
                this.GV_List.Columns[2].Visible = false;  //类型
                this.GV_List.Columns[3].Visible = false;  //供应商
                this.GV_List.Columns[4].Visible = false;  //发货地址
            }
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        if (ViewEvent != null)
        {
            string ipNo = ((LinkButton)sender).CommandArgument;
            ViewEvent(ipNo, e);
        }
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string ipNo = ((LinkButton)sender).CommandArgument;
            EditEvent(ipNo, e);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InProcessLocation ip = (InProcessLocation)e.Row.DataItem;

            //e.Row.FindControl("lbtnEdit").Visible = false;
            e.Row.FindControl("lbtnEdit").Visible = this.AsnType != BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP && ip.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE && this.Action != "View";
            e.Row.FindControl("lbtnView").Visible = this.Action == "View";

            e.Row.Cells[1].Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor=this.style.borderColor");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            e.Row.Cells[1].Attributes.Add("title", GetDetail(ip));
        }
    }

    private string GetDetail(InProcessLocation ip)
    {

        if (ip.InProcessLocationDetails == null
           || ip.InProcessLocationDetails.Count == 0
            || ip.InProcessLocationDetails[0].OrderLocationTransaction == null
            || ip.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail == null
            || ip.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead == null)
            return string.Empty;

        OrderHead orderHead = ip.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead;

        string detail = "";
        detail += "cssbody=[obbd] cssheader=[obhd] header=[" + orderHead.OrderNo + " | " + (orderHead.Flow == null ? string.Empty : orderHead.Flow) + "] body=[<table width=100%>";
        System.Collections.Generic.IList<OrderDetail> ods = orderHead.OrderDetails;



        foreach (InProcessLocationDetail ipDetail in ip.InProcessLocationDetails)
        {
            if (ipDetail.OrderLocationTransaction != null && ipDetail.OrderLocationTransaction.OrderDetail != null)
            {
                OrderDetail od = ipDetail.OrderLocationTransaction.OrderDetail;
                string ItemCode = od.Item.Code;
                string ItemSpec = od.Item.Spec.Replace("[", "&#91;").Replace("]", "&#93;");
                string OrderQty = od.OrderedQty.ToString("0.########");
                string Uom = od.Uom.Code;
                string RecQty = od.ReceivedQty == null ? "0" : od.ReceivedQty.Value.ToString("0.########");
                string TotalAmount = od.UnitPriceAfterDiscount.HasValue ? od.UnitPriceAfterDiscount.Value.ToString("F2") : "0";// od.IncludeTaxTotalPrice == null ? "0" : od.IncludeTaxTotalPrice.ToString("F2");
                detail += "<tr><td>" + ItemCode + "</td><td>" + ItemSpec + "</td><td>" + OrderQty + "</td><td>" + Uom + "</td><td>" + RecQty + "</td><td>" + TotalAmount + "</td></tr>";
            }
        }
        detail += "</table>]";
        return detail;
    }

    protected void GV_List_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (IsExport && e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }
}
