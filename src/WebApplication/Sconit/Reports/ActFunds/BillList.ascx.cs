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

public partial class Reports_ActFunds_BillList : ListModuleBase
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
            this.GV_List.Columns[8].HeaderText = "${MasterData.Bill.PayableDate}";
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[8].HeaderText = "${MasterData.Bill.ReceivableDate}";
        }
    }

    public void InitPageParameter(object sender)
    {
        DetachedCriteria selectCriteria = (DetachedCriteria)sender;
        IList<BillDetail> billDetailList = TheCriteriaMgr.FindAll<BillDetail>(selectCriteria);
        this.GV_List.DataSource = billDetailList;
        this.GV_List.DataBind();
        this.UpdateView();
    }

    public override void UpdateView()
    {
        //this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }
}