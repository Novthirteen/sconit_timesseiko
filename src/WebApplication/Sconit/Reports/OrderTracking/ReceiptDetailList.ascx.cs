using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

public partial class Reports_OrderTracking_ReceiptDetailList : ListModuleBase
{
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

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          //  OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;
          //  Label lblNoRecQty = e.Row.FindControl("lblNoRecQty") as Label;
          //  lblNoRecQty.Text = (orderDetail.OrderedQty - (orderDetail.ReceivedQty.HasValue ? orderDetail.ReceivedQty.Value : 0)).ToString("0.##");
        }
    }
}
