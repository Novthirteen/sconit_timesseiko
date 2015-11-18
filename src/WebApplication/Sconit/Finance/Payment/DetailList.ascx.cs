using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Web;

public partial class Finance_DetailList_List : ModuleBase
{

    public event EventHandler BackEvent;
    public event EventHandler EditRefreshEvent;

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
    public string PartyCode
    {
        get
        {
            return (string)ViewState["PartyCode"];
        }
        set
        {
            ViewState["PartyCode"] = value;
        }
    }



    public string CurrencyCode
    {
        get
        {
            return (string)ViewState["CurrencyCode"];
        }
        set
        {
            ViewState["CurrencyCode"] = value;
        }
    }

    public string PaymentNo
    {
        get
        {
            return (string)ViewState["PaymentNo"];
        }
        set
        {
            ViewState["PaymentNo"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucNewSearch.BackEvent += new EventHandler(AddBack_Render);
        this.ucNewSearch.ModuleType = this.ModuleType;

        if (!IsPostBack)
        {
            if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.lgdTitle.InnerText = "${MasterData.Bill.SOBill}:";
            }

            if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.Gv_List.Columns[3].HeaderText = "${MasterData.Payment.Customer}";
            }
        }
    }


    protected void AddBack_Render(object sender, EventArgs e)
    {
        this.ucNewSearch.Visible = false;
        //this.Parent.FindControl("FV_Payment").DataBind();

        if (this.EditRefreshEvent != null)
        {
            this.EditRefreshEvent(this.PaymentNo, e);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BillPayment billPayment = (BillPayment)e.Row.DataItem;

            //TextBox tbThisBackwashAmount = e.Row.FindControl("tbThisBackwashAmount") as TextBox;
            TextBox tbBillBackwashAmount = e.Row.FindControl("tbBillBackwashAmount") as TextBox;
            
            //if (tbThisBackwashAmount.Text != null && tbThisBackwashAmount.Text.Trim().Length > 0 && decimal.Parse(tbThisBackwashAmount.Text.Trim()) != 0)
            //{
            tbBillBackwashAmount.Text = (billPayment.Bill.BackwashAmount - billPayment.BackwashAmount).Value.ToString("0.00");
                
                //tbThisBackwashAmount.Text = billPayment.Bill.NoBackwashAmount.ToString("F2");
            //}

        }
          
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
        }
    }

    protected void btnAddBill_Click(object sender, EventArgs e)
    {
        IDictionary<string, string> actionParameter = new Dictionary<string, string>();
        actionParameter.Add("PartyCode", this.PartyCode);
        actionParameter.Add("CurrencyCode", this.CurrencyCode);
        this.ucNewSearch.QuickSearch(actionParameter);
        this.ucNewSearch.Visible = true;
    }

    protected void btnDeleteBill_Click(object sender, EventArgs e)
    {
        try
        {
            IList<BillPayment> billPaymentList = PopulateData(true);
            ThePaymentMgr.DeleteBillPayment(this.PaymentNo, billPaymentList, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Payment.DeleteBillSuccessfully");

            if (this.EditRefreshEvent != null)
            {
                this.EditRefreshEvent(this.PaymentNo, e);
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    public IList<BillPayment> PopulateData(bool isChecked)
    {
        if (this.Gv_List.Rows != null && this.Gv_List.Rows.Count > 0)
        {
            IList<BillPayment> billPaymentList = new List<BillPayment>();

            foreach (GridViewRow row in this.Gv_List.Rows)
            {
                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked || !isChecked)
                {
                    HiddenField hfId = row.FindControl("hfId") as HiddenField;
                    TextBox tbThisBackwashAmount = row.FindControl("tbThisBackwashAmount") as TextBox;
                    BillPayment billPayment = TheBillPaymentMgr.LoadBillPayment(int.Parse(hfId.Value), true);


                    if (tbThisBackwashAmount.Text == null || tbThisBackwashAmount.Text.Trim().Length == 0 || tbThisBackwashAmount.Text.Trim().Equals("0"))
                    {
                        //billPayment.BackwashAmount = billPayment.Bill.AmountAfterDiscount;
                    }
                    else
                    {
                        billPayment.BackwashAmount = decimal.Parse(tbThisBackwashAmount.Text.Trim());
                    }

                    billPaymentList.Add(billPayment);
                }
            }
            return billPaymentList;
        }

        return null;
    }

    public void UpdateView(Payment payment)
    {

        #region 初始化弹出窗口
        this.PartyCode = payment.Party.Code;
        this.CurrencyCode = payment.Currency.Code;
        this.ucNewSearch.InitPageParameter(true, payment);
        #endregion

        IList<BillPayment> billPayments = payment.BillPayments;
        this.PaymentNo = payment.PaymentNo;
        this.Gv_List.DataSource = billPayments;
        this.Gv_List.DataBind();
    }

}
