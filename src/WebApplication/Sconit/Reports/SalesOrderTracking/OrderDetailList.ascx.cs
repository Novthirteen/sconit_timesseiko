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

public partial class Reports_SalesOrderTracking_OrderDetailList : ListModuleBase
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
        /*
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.GV_List.Columns[12].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[11].Visible = false;
        }
         */
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {/*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;
            TextBox tbUnitPrice = e.Row.FindControl("tbUnitPrice") as TextBox;
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                if (orderDetail.TotalAmountFrom != null)
                {
                    tbUnitPrice.Text = (orderDetail.TotalAmountFrom / orderDetail.OrderedQty).Value.ToString("###,##0.00");
                }
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                if (orderDetail.TotalAmountTo != null)
                {
                    tbUnitPrice.Text = (orderDetail.TotalAmountTo / orderDetail.OrderedQty).Value.ToString("###,##0.00");
                }
            }
            //  lblNoRecQty.Text = (orderDetail.OrderedQty - (orderDetail.ReceivedQty.HasValue ? orderDetail.ReceivedQty.Value : 0)).ToString("0.00");
        }
      */
    }
}
