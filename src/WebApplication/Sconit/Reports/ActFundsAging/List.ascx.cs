using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity.MasterData;
using System.Text;

public partial class Reports_ActFundsAging_List : ListModuleBase
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
        this.ExportXLS(GV_List);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.BillAging.PartyFrom}";

            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.BillAging.PartyTo}";

            }
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ActFundsAgingView afav = (ActFundsAgingView)e.Row.DataItem;

            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount1", afav.NoBackwashAmount1 != null && afav.NoBackwashAmount1.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount2", afav.NoBackwashAmount2 != null && afav.NoBackwashAmount2.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount3", afav.NoBackwashAmount3 != null && afav.NoBackwashAmount3.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount4", afav.NoBackwashAmount4 != null && afav.NoBackwashAmount4.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount5", afav.NoBackwashAmount5 != null && afav.NoBackwashAmount5.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount6", afav.NoBackwashAmount6 != null && afav.NoBackwashAmount6.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount7", afav.NoBackwashAmount7 != null && afav.NoBackwashAmount7.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount8", afav.NoBackwashAmount8 != null && afav.NoBackwashAmount8.Value != 0);
            this.SetLinkButton(e.Row, "lbtnNoBackwashAmount9", afav.NoBackwashAmount9 != null && afav.NoBackwashAmount9.Value != 0);

            if (isExport)
            {
                for (int i = 2; i <= 10; i++)
                {
                    e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
        }
    }

    private void OnDetailClick(string id, int range)
    {
        Bill bill = TheBillMgr.LoadBill(id, true);

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Bill));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Bill))
            .SetProjection(Projections.Count("BillNo"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.CreateAlias("BillAddress", "ba");
        selectCountCriteria.CreateAlias("BillAddress", "ba");
        alias.Add("BillAddress", "ba");

        #region 账龄区间处理
        DateTime dateTimeNow = DateTime.Now;
        switch (range)
        {
            case 1:
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-30)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-30)));
                break;
            case 2:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-30)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-60)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-30)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-60)));
                break;
            case 3:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-60)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-90)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-60)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-90)));
                break;
            case 4:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-90)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-120)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-90)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-120)));
                break;
            case 5:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-120)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-150)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-120)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-150)));
                break;
            case 6:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-150)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-180)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-150)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-180)));
                break;
            case 7:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-180)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-210)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-180)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-210)));
                break;
            case 8:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-210)));
                selectCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-360)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-210)));
                selectCountCriteria.Add(Expression.Ge("EffectiveDate", dateTimeNow.AddDays(-360)));
                break;
            case 9:
                selectCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-360)));
                selectCountCriteria.Add(Expression.Lt("EffectiveDate", dateTimeNow.AddDays(-360)));
                break;
        }
        #endregion


        selectCriteria.Add(Expression.Like("Currency.Code", bill.Currency.Code, MatchMode.Anywhere));
        selectCountCriteria.Add(Expression.Like("Currency.Code", bill.Currency.Code, MatchMode.Anywhere));

        selectCriteria.Add(Expression.Eq("ba.Code", bill.BillAddress.Code));
        selectCountCriteria.Add(Expression.Eq("ba.Code", bill.BillAddress.Code));

        selectCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        selectCountCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));


        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias }), null);
    }

    protected void lbtnDetail1_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 1);
    }

    protected void lbtnDetail2_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 2);
    }

    protected void lbtnDetail3_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 3);
    }

    protected void lbtnDetail4_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 4);
    }

    protected void lbtnDetail5_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 5);
    }

    protected void lbtnDetail6_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 6);
    }

    protected void lbtnDetail7_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 7);
    }

    protected void lbtnDetail8_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 8);
    }

    protected void lbtnDetail9_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, 9);
    }
}
