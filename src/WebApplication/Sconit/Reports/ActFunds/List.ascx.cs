using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.View;

public partial class Reports_ActFunds_List : ListModuleBase
{

    public event EventHandler DetailEvent;

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
        this.isExport = false;
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.isExport = true;
        this.ExportXLS(GV_List, "ActFunds.xls");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyFrom}";
                this.GV_List.Columns[5].HeaderText = "${Reports.ActFunds.Buyer}";

            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyTo}";
                this.GV_List.Columns[5].HeaderText = "${Reports.ActFunds.Salesman}";

            }
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        TextBox tbUser = (TextBox)e.Row.FindControl("tbUser");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Bill bill = (Bill)e.Row.DataItem;
            if (bill.BillDetails != null && bill.BillDetails.Count > 0)
            {
                tbUser.Text = bill.BillDetails[0].ActingBill.OrderHead.CreateUser.Code;
            }

            if (isExport)
            {
                this.SetLinkButton(e.Row, "lbtnBillNo", bill.BillNo != null && bill.BillNo.Length > 0);
            }
        }
    }

    protected void lbtnBillDetail_Click(object sender, EventArgs e)
    {
        string billNo = ((LinkButton)sender).CommandArgument;

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(BillDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BillDetail))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.CreateAlias("Bill", "b");
        selectCountCriteria.CreateAlias("Bill", "b");
        alias.Add("Bill", "b");

        selectCriteria.Add(Expression.Eq("b.BillNo", billNo));
        selectCountCriteria.Add(Expression.Eq("b.BillNo", billNo));

        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias }), null);
    }

    
}
