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

public partial class Finance_Bill_NewList : ListModuleBase
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

    private int DecimalLength
    {
        get
        {
            return (int)ViewState["DecimalLength"];
        }
        set
        {
            ViewState["DecimalLength"] = value;
        }
    }

    private decimal TotalDetailAmount
    {
        get
        {
            return (decimal)ViewState["TotalDetailAmount"];
        }
        set
        {
            ViewState["TotalDetailAmount"] = value;
        }
    }


    public void BindDataSource(IList<ActingBill> actingBillList)
    {
        //Button btnAddDetail = this.Parent.FindControl("btnAddDetail") as Button;
        if (actingBillList != null)
        {
            var q = from a in actingBillList
                    orderby a.ExternalReceiptNo
                    select a;

            this.GV_List.DataSource = q.ToList<ActingBill>();
            //btnAddDetail.Visible = true;
        }
        else
        {

            //btnAddDetail.Visible = false;
            this.GV_List.DataSource = actingBillList;
        }
        this.UpdateView();

    }

    public IList<ActingBill> PopulateSelectedData()
    {
        if (this.GV_List.Rows != null && this.GV_List.Rows.Count > 0)
        {
            IList<ActingBill> actingBillList = new List<ActingBill>();
            foreach (GridViewRow row in this.GV_List.Rows)
            {

                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked)
                {
                    HiddenField hfId = row.FindControl("hfId") as HiddenField;
                    TextBox tbQty = row.FindControl("tbQty") as TextBox;
                    TextBox tbDiscount = row.FindControl("tbDiscount") as TextBox;

                    ActingBill actingBill = new ActingBill();
                    actingBill.Id = int.Parse(hfId.Value);
                    actingBill.CurrentBillQty = decimal.Parse(tbQty.Text);
                    actingBill.CurrentDiscount = tbDiscount.Text == string.Empty ? decimal.Zero : decimal.Parse(tbDiscount.Text);

                    actingBillList.Add(actingBill);
                }
            }
            return actingBillList;
        }

        return null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                this.GV_List.Columns[3].Visible = false;
                this.GV_List.Columns[5].Visible = false;
            }
            else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.GV_List.Columns[1].HeaderText = "${MasterData.ActingBill.Customer}";
                this.GV_List.Columns[2].HeaderText = "${MasterData.ActingBill.OrderNo.Distribution}";
                this.GV_List.Columns[4].Visible = false;
            }

            EntityPreference entityPreference = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            DecimalLength = int.Parse(entityPreference.Value);

        }
        TotalDetailAmount = 0;
    }

    public override void UpdateView()
    {
        this.GV_List.DataBind();
        if (this.GV_List.DataSource != null)
        {
            this.lblNoRecordFound.Visible = false;
            this.tblTotalDetailAmount.Visible = true;

            this.tbTotalDetailAmount.Text = TotalDetailAmount.ToString("F2"); ;
        }
        else
        {
            this.lblNoRecordFound.Visible = true;
            this.tblTotalDetailAmount.Visible = false;
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ActingBill actingBill = (ActingBill)e.Row.DataItem;

            decimal billAmount = actingBill.BillAmount;
            decimal unitPrice = actingBill.ListPrice;

            decimal remailQty = actingBill.BillQty - actingBill.BilledQty;
            decimal remailAmount = billAmount - actingBill.BilledAmount;
            decimal discount = unitPrice * remailQty - remailAmount;

            TextBox tbQty = e.Row.FindControl("tbQty") as TextBox;
            TextBox tbDiscountRate = e.Row.FindControl("tbDiscountRate") as TextBox;
            TextBox tbDiscount = e.Row.FindControl("tbDiscount") as TextBox;
            TextBox tbAmount = e.Row.FindControl("tbAmount") as TextBox;

            tbQty.Text = remailQty.ToString("F2");
            if (unitPrice != 0 && remailQty != 0)
            {
                tbDiscountRate.Text = (Math.Round(discount / (unitPrice * remailQty), this.DecimalLength, MidpointRounding.AwayFromZero) * 100).ToString("F2");
            }
            tbDiscount.Text = discount.ToString("F2");
            tbAmount.Text = remailAmount.ToString("F2");
            tbAmount.Attributes["oldValue"] = tbAmount.Text;

            tbQty.Attributes.Add("onchange", "qtyChanged(this);");

            TotalDetailAmount += remailAmount;

        }
    }
}
