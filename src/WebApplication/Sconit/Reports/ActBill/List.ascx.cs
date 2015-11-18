using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using NHibernate.Expression;

public partial class Reports_ActBill_List : ListModuleBase
{
    public bool isExport
    {
        get { return ViewState["isExport"] == null ? false : (bool)ViewState["isExport"]; }
        set { ViewState["isExport"] = value; }
    }
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

    public void InitPageParameter(object sender)
    {
        DetachedCriteria selectCriteria = (DetachedCriteria)sender;
        IList<ActingBillView> actingBillViewList = TheCriteriaMgr.FindAll<ActingBillView>(selectCriteria);

        if (actingBillViewList == null || actingBillViewList.Count == 0)
        {
            this.tabTotalAmount.Visible = false;
            this.lblNoRecordFound.Visible = true;
        }
        else
        {
            this.tbTotalPrice.Text = actingBillViewList.Sum(abv => abv.Amount).ToString("###,##0.00");
        }

        this.GV_List.DataSource = actingBillViewList;
        this.GV_List.DataBind();

        this.UpdateView();
    }

    public override void UpdateView()
    {
        this.isExport = false;
        //this.GV_List.Execute();
    }

    public void Export()
    {
        this.isExport = true;
        this.ExportXLS(GV_List);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActBill.PartyFrom}";
                this.GV_List.Columns[4].Visible = false;
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActBill.PartyTo}";
                this.GV_List.Columns[3].Visible = false;
            }
        }
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblParty = (Label)e.Row.FindControl("lblParty");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //ActingBillView actingBillView = (ActingBillView)e.Row.DataItem;
            if (isExport)
            {
                e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[9].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[11].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }
}
