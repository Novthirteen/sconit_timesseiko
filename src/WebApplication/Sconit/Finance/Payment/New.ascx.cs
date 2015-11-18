using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Control;
using com.Sconit.Entity;

public partial class Finance_Payment_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private Payment payment;

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

        if (!IsPostBack)
        {

        }

        if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            Literal ltlPartyCode = this.FV_Payment.FindControl("ltlPartyCode") as Literal;
            Controls_TextBox tbPartyCode = this.FV_Payment.FindControl("tbPartyCode") as Controls_TextBox;

            ltlPartyCode.Text = "${MasterData.Payment.Customer}:";
            tbPartyCode.ServicePath = "CustomerMgr.service";
            tbPartyCode.ServiceMethod = "GetAllCustomer";
        }
    }

    protected void ODS_Payment_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        payment = (Payment)e.InputParameters[0];

    }

    protected void ODS_Payment_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(payment.PaymentNo, e);
            ShowSuccessMessage("MasterData.Payment.AddPayment.Successfully", payment.PaymentNo);
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        RequiredFieldValidator rfvParty = this.FV_Payment.FindControl("rfvParty") as RequiredFieldValidator;
        RequiredFieldValidator rfvCurrency = this.FV_Payment.FindControl("rfvCurrency") as RequiredFieldValidator;
        if (rfvParty.IsValid && rfvCurrency.IsValid)
        {
            TextBox tbPaymentDate = (TextBox)(this.FV_Payment.FindControl("tbPaymentDate"));
            //TextBox tbInvoiceNo = (TextBox)(this.FV_Payment.FindControl("tbInvoiceNo"));
            //TextBox tbExtPaymentNo = (TextBox)(this.FV_Payment.FindControl("tbExtPaymentNo"));
            TextBox tbAmount = (TextBox)(this.FV_Payment.FindControl("tbAmount"));

            TextBox tbVoucherNo = (TextBox)(this.FV_Payment.FindControl("tbVoucherNo"));

            Controls_TextBox tbPartyCode = (Controls_TextBox)(this.FV_Payment.FindControl("tbPartyCode"));
            Controls_TextBox tbCurrency = (Controls_TextBox)(this.FV_Payment.FindControl("tbCurrency"));
            CheckBox cbContinuousCreate = (CheckBox)(this.FV_Payment.FindControl("cbContinuousCreate"));
            CodeMstrDropDownList ddlPayType = (CodeMstrDropDownList)(this.FV_Payment.FindControl("ddlPayType"));

            Payment payment = new Payment();
            payment.PaymentDate = DateTime.Parse(tbPaymentDate.Text.Trim());
            //payment.ExtPaymentNo = tbExtPaymentNo.Text != null ? tbExtPaymentNo.Text.Trim() : string.Empty;
            payment.ExtPaymentNo = string.Empty;
            payment.Amount = decimal.Parse(tbAmount.Text.Trim());
            payment.Party = ThePartyMgr.LoadParty(tbPartyCode.Text);
            payment.Currency = TheCurrencyMgr.LoadCurrency(tbCurrency.Text);
            payment.TransType = this.ModuleType;
            payment.PayType = ddlPayType.Text;
            payment.VoucherNo = tbVoucherNo.Text;
            ThePaymentMgr.CreatePayment(payment, this.CurrentUser);

            if (cbContinuousCreate.Checked)
            {
                if (NewEvent != null)
                {
                    NewEvent(payment.PaymentNo, e);
                    ShowSuccessMessage("MasterData.Payment.AddPayment.Successfully", payment.PaymentNo);
                }
            }
            else
            {
                if (CreateEvent != null)
                {
                    CreateEvent(payment.PaymentNo, e);
                    ShowSuccessMessage("MasterData.Payment.AddPayment.Successfully", payment.PaymentNo);
                }
            }
        }

    }

    public void PageCleanup()
    {
        //((TextBox)(this.FV_Payment.FindControl("tbPaymentNo"))).Text = string.Empty;
        ((TextBox)(this.FV_Payment.FindControl("tbPaymentDate"))).Text = string.Empty;
        //((TextBox)(this.FV_Payment.FindControl("tbInvoiceNo"))).Text = string.Empty;
        //((TextBox)(this.FV_Payment.FindControl("tbExtPaymentNo"))).Text = string.Empty;
        ((TextBox)(this.FV_Payment.FindControl("tbAmount"))).Text = string.Empty;
        ((TextBox)(this.FV_Payment.FindControl("tbVoucherNo"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Payment.FindControl("tbPartyCode"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Payment.FindControl("tbCurrency"))).Text = string.Empty;
        ((CheckBox)(this.FV_Payment.FindControl("cbContinuousCreate"))).Checked = false;
    }


}
