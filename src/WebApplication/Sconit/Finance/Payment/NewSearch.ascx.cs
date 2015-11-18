using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

public partial class Finance_Payment_NewSearch : SearchModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

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

    private string PaymentNo
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

    public void InitPageParameter(bool isPopup, Payment payment)
    {
        if (isPopup)
        {
            this.PaymentNo = payment.PaymentNo;
            this.tbPartyCode.Visible = false;
            this.ltlParty.Text = payment.Party.Name;
            this.ltlParty.Visible = true;

            this.tbCurrencyCode.Visible = false;
            this.ltlCurrency.Text = payment.Currency.Name;
            this.ltlCurrency.Visible = true;

            //this.btnConfirm.Visible = false;
            this.btnBack.Visible = false;
            this.btnAddPayment.Visible = true;
            this.btnClose.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.ddlStatus.DataSource = this.GetAllStatus();
            //this.ddlStatus.DataBind();

            this.PageCleanUp();
        }

        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            this.ltlPartyCode.Text = "${MasterData.Bill.Customer}:";
            //this.ltlExternalBillNo.Text = "${MasterData.ActingBill.ExternalReceiptNo}:";

            this.tbPartyCode.ServicePath = "CustomerMgr.service";
            this.tbPartyCode.ServiceMethod = "GetAllCustomer";
        }

        this.ucNewList.ModuleType = this.ModuleType;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void DoSearch()
    {
        string partyCode = this.tbPartyCode.Text != string.Empty ? this.tbPartyCode.Text.Trim() : string.Empty;
        string externalBillNo = this.tbExternalBillNo.Text != string.Empty ? this.tbExternalBillNo.Text.Trim() : string.Empty;
        string startDate = this.tbStartDate.Text != string.Empty ? this.tbStartDate.Text.Trim() : string.Empty;
        string endDate = this.tbEndDate.Text != string.Empty ? this.tbEndDate.Text.Trim() : string.Empty;
        string billNo = this.tbBillNo.Text != string.Empty ? this.tbBillNo.Text.Trim() : string.Empty;
        string currencyCode = this.tbCurrencyCode.Text != string.Empty ? this.tbCurrencyCode.Text.Trim() : string.Empty;
        //string status = this.ddlStatus.Text != string.Empty ? this.ddlStatus.Text.Trim() : string.Empty;
        string status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;

        IList<Bill> billList = TheBillMgr.GetBill(billNo, status, partyCode, currencyCode, this.ModuleType, externalBillNo, this.PaymentNo, startDate, endDate);

        this.ucNewList.BindDataSource(billList != null && billList.Count > 0 ? billList : null);
        this.ucNewList.Visible = true;
        
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(this, null);
    }

    protected void btnAddPayment_Click(object sender, EventArgs e)
    {
        try
        {
            IList<Bill> billList = this.ucNewList.PopulateSelectedData();
            if (billList != null && billList.Count > 0)
            {
                this.ThePaymentMgr.AddBillPayment(this.PaymentNo, billList, this.CurrentUser);
                this.ShowSuccessMessage("MasterData.Payment.AddBillPaymentSuccessfully");
                BackEvent(this, null);
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnNamedQuery_Click(object sender, EventArgs e)
    {
        IDictionary<string, string> actionParameter = new Dictionary<string, string>();
        if (this.tbStartDate.Text != string.Empty)
        {
            actionParameter.Add("StartDate", this.tbStartDate.Text);
        }
        if (this.tbEndDate.Text != string.Empty)
        {
            actionParameter.Add("EndDate", this.tbEndDate.Text);
        }
        if (this.tbPartyCode.Text != string.Empty)
        {
            actionParameter.Add("PartyCode", this.tbPartyCode.Text);
        }
        if (this.tbExternalBillNo.Text != string.Empty)
        {
            actionParameter.Add("ExternalBillNo", this.tbExternalBillNo.Text);
        }
        if (this.tbBillNo.Text != string.Empty)
        {
            actionParameter.Add("tbBillNo", this.tbBillNo.Text);
        }
        if (this.tbCurrencyCode.Text != string.Empty)
        {
            actionParameter.Add("CurrencyCode", this.tbCurrencyCode.Text);
        }
        /*
        if (this.ddlStatus.Text != string.Empty)
        {
            actionParameter.Add("Status", this.ddlStatus.Text);
        }
        */
        //this.SaveNamedQuery(this.tbNamedQuery.Text, actionParameter);
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("StartDate"))
        {
            this.tbStartDate.Text = actionParameter["StartDate"];
        }
        if (actionParameter.ContainsKey("EndDate"))
        {
            this.tbEndDate.Text = actionParameter["EndDate"];
        }
        if (actionParameter.ContainsKey("PartyCode"))
        {
            this.tbPartyCode.Text = actionParameter["PartyCode"];
        }
        if (actionParameter.ContainsKey("ExternalBillNo"))
        {
            this.tbExternalBillNo.Text = actionParameter["ExternalBillNo"];
        }
        if (actionParameter.ContainsKey("tbBillNo"))
        {
            this.tbBillNo.Text = actionParameter["tbBillNo"];
        }
        if (actionParameter.ContainsKey("CurrencyCode"))
        {
            this.tbCurrencyCode.Text = actionParameter["CurrencyCode"];
        }
        /*
        if (actionParameter.ContainsKey("Status"))
        {
            this.ddlStatus.Text = actionParameter["Status"];
        }
        */

    }

    private void PageCleanUp()
    {
        this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.tbPartyCode.Text = string.Empty;
        this.tbExternalBillNo.Text = string.Empty;
        this.tbBillNo.Text = string.Empty;
        this.tbCurrencyCode.Text = string.Empty;
        //this.ddlStatus.Text = string.Empty;
        this.ucNewList.Visible = false;
    }

    /*
    public IList<CodeMaster> GetAllStatus()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        //statusGroup.Add(new CodeMaster());//空行
        //if (!this.IsSupplier)
        //{
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        //}
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID));

        return statusGroup;
    }
    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
     */
}
