using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Collections;

public partial class Reports_SalePerformance_Detail : ListModuleBase
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
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.GV_List.Columns[1].HeaderText = "${MasterData.Order.OrderHead.OrderNo.Procurement}";
            //this.GV_List.Columns[12].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[1].HeaderText = "${MasterData.Order.OrderHead.OrderNo.Distribution}";
            //this.GV_List.Columns[11].Visible = false;
        }
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
                if (orderDetail.IncludeTaxAmountFrom != null)
                {
                    tbUnitPrice.Text = (orderDetail.IncludeTaxAmountFrom / orderDetail.OrderedQty).ToString("###,##0.00");
                }
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                if (orderDetail.IncludeTaxAmountTo != null)
                {
                    tbUnitPrice.Text = (orderDetail.IncludeTaxAmountTo / orderDetail.OrderedQty).ToString("###,##0.00");
                }
            }
            //  lblNoRecQty.Text = (orderDetail.OrderedQty - (orderDetail.ReceivedQty.HasValue ? orderDetail.ReceivedQty.Value : 0)).ToString("0.##");
        }
      */
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }
}