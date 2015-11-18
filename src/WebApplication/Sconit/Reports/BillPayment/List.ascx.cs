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

public partial class Reports_BillPayment_List : ReportModuleBase
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


    protected override void SetCriteria()
    {
    }

    public override void InitPageParameter(object sender)
    {
        DetachedCriteria criteria = (DetachedCriteria)sender;
        IList<Bill> billList = TheCriteriaMgr.FindAll<Bill>(criteria);

        this.GV_List.DataSource = billList;
        this.GV_List.DataBind();
        if (billList == null || billList.Count == 0)
        {
            this.tabTotalAmount.Visible = false;
            this.lblNoRecordFound.Visible = true;
        }
        else
        {

            decimal? totalBillAmount = billList.Where(spl => spl.AmountAfterDiscount != null).Sum(spl => Math.Round(spl.AmountAfterDiscount.Value, 2));
            this.tbTotalBillAmount.Text = totalBillAmount.HasValue ? totalBillAmount.Value.ToString("###,##0.00") : "0.00";

            decimal? backwashAmount = billList.Where(spl => spl.BackwashAmount != null).Sum(spl => Math.Round(spl.BackwashAmount.Value, 2));
            this.tbBackwashAmount.Text = backwashAmount.HasValue ? backwashAmount.Value.ToString("###,##0.00") : "0.00";

            decimal? noBackwashAmount = billList.Where(spl => spl.NoBackwashAmount != null).Sum(spl => Math.Round(spl.NoBackwashAmount, 2));
            this.tbNoBackwashAmount.Text = noBackwashAmount.HasValue ? noBackwashAmount.Value.ToString("###,##0.00") : "0.00";

            this.tabTotalAmount.Visible = true;
            this.lblNoRecordFound.Visible = false;
        }

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
        this.ExportXLS(GV_List, "BillPayment.xls");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyFrom}";
                this.GV_List.Columns[5].HeaderText = "${MasterData.Bill.PayableDate}";
                this.GV_List.Columns[9].HeaderText = "${Reports.BillPayment.BackwashAmountPO}";
                this.GV_List.Columns[10].HeaderText = "${Reports.BillPayment.NoBackwashAmountPO}";

                this.lblBackwashAmount.Text = "${Reports.BillPayment.TotalBackwashAmountPO}:";
                this.lblNoBackwashAmount.Text = "${Reports.BillPayment.TotalNoBackwashAmountPO}:";

            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyTo}";
                this.GV_List.Columns[5].HeaderText = "${MasterData.Bill.ReceivableDate}";
                this.GV_List.Columns[9].HeaderText = "${Reports.BillPayment.BackwashAmountSO}";
                this.GV_List.Columns[10].HeaderText = "${Reports.BillPayment.NoBackwashAmountSO}";

                this.lblBackwashAmount.Text = "${Reports.BillPayment.TotalBackwashAmountSO}:";
                this.lblNoBackwashAmount.Text = "${Reports.BillPayment.TotalNoBackwashAmountSO}:";
            }
        }
    }



    protected void lbtnBackwashAmount_Click(object sender, EventArgs e)
    {
        string billNo = ((LinkButton)sender).CommandArgument;

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(BillPayment));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BillPayment))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();
        selectCriteria.CreateAlias("Bill", "b");
        selectCriteria.CreateAlias("Payment", "p");
        selectCountCriteria.CreateAlias("Bill", "b");
        selectCountCriteria.CreateAlias("Payment", "p");
        alias.Add("Bill", "b");
        alias.Add("Payment", "p");
        selectCriteria.Add(Expression.Eq("b.BillNo", billNo));
        selectCountCriteria.Add(Expression.Eq("b.BillNo", billNo));

        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias }), null);
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Bill bill = (Bill)e.Row.DataItem;
            if (isExport)
            {
                e.Row.Cells[8].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[9].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[10].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                this.SetLinkButton(e.Row, "lblBackwashAmount", bill.BackwashAmount != null && bill.BackwashAmount.Value != 0);
            }
            else
            {
                if (bill.BackwashAmount != null && bill.BackwashAmount.Value == 0)
                {
                    LinkButton linkButton = (LinkButton)e.Row.FindControl("lblBackwashAmount");
                    linkButton.Text = string.Empty;
                }
            }
        }
    }



}
