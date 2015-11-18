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
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using LeanEngine.Entity;

public partial class Order_GoodsReceipt_OrderReceipt_List : ListModuleBase
{
    public EventHandler EditEvent;

    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    public override void UpdateView()
    {
        this.GV_List.Columns[1].HeaderText = OrderHelper.GetOrderLabel(this.ModuleType);
        this.GV_List.Columns[4].HeaderText = OrderHelper.GetOrderPartyFromLabel(this.ModuleType);
        this.GV_List.Columns[5].HeaderText = OrderHelper.GetOrderPartyToLabel(this.ModuleType);
        this.GV_List.Columns[8].HeaderText = OrderHelper.GetOrderCreateUserLabel(this.ModuleType);
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            this.GV_List.Columns[6].Visible = false;

        }
        this.GV_List.Execute();


    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string orderNo = ((LinkButton)sender).CommandArgument;
            EditEvent(orderNo, e);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
            HiddenField hfModuleSubType = (HiddenField)e.Row.FindControl("hfModuleSubType");
            if (hfModuleSubType.Value == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
            {
                lbtnEdit.Text = "${Common.Button.Return}";
            }

            OrderHead orderHead = (OrderHead)e.Row.DataItem;

            //销售单号
            string hql = @"select Distinct ot.Code from OrderTracer ot ,OrderLocationTransaction olt inner join olt.OrderDetail od inner join od.OrderHead oh where ot.RefOrderLocTransId = olt.Id and  oh.OrderNo = ? ";

            IList<string> orderNoList = this.TheCriteriaMgr.FindAllWithHql<string>(hql, new object[] { orderHead.OrderNo });

            if (orderNoList != null)
            {
                string orderNos = "";
                for (int i = 0; i < orderNoList.Count; i++)
                {
                    if (i != 0)
                    {
                        orderNos += ",";
                    }
                    orderNos += orderNoList[i];
                }
                if (orderNos.Length > 0)
                {
                    Label lblOrderNos = (Label)e.Row.FindControl("lblOrderNos");
                    lblOrderNos.Text = orderNos;
                }
            }

            e.Row.Cells[1].Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor=this.style.borderColor");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            e.Row.Cells[1].Attributes.Add("title", GetDetail(orderHead));

        }
    }


    private string GetDetail(OrderHead orderHead)
    {
        string detail = "";
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
}
