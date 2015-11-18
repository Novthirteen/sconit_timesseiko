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
using System.Collections.Generic;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;

public partial class Finance_Payment_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;

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
    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ddlStatus.DataSource = this.GetAllStatus();
            this.ddlStatus.DataBind();
            this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


        if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            this.ltlPartyCode.Text = "${MasterData.Payment.Customer}:";
            this.tbPartyCode.ServicePath = "CustomerMgr.service";
            this.tbPartyCode.ServiceMethod = "GetAllCustomer";
        }

        if (this.IsSupplier)
        {
            this.tbPartyCode.ServicePath = "PartyMgr.service";
            this.tbPartyCode.ServiceMethod = "GetFromParty";
            this.tbPartyCode.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT + ",string:" + this.CurrentUser.Code;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("PaymentNo"))
        {
            this.tbPaymentNo.Text = actionParameter["PaymentNo"];
        }

        if (actionParameter.ContainsKey("VoucherNo"))
        {
            this.tbVoucherNo.Text = actionParameter["VoucherNo"];
        }

        if (actionParameter.ContainsKey("Status"))
        {
            this.ddlStatus.Text = actionParameter["Status"];
        }
        /*
        if (actionParameter.ContainsKey("ExtPaymentNo"))
        {
            this.tbExtPaymentNo.Text = actionParameter["ExtPaymentNo"];
        }
         */
        if (actionParameter.ContainsKey("Currency"))
        {
            this.tbCurrency.Text = actionParameter["Currency"];
        }
        if (actionParameter.ContainsKey("StartDate"))
        {
            this.tbStartDate.Text = actionParameter["StartDate"];
        }
        if (actionParameter.ContainsKey("EndDate"))
        {
            this.tbEndDate.Text = actionParameter["EndDate"];
        }
        if (actionParameter.ContainsKey("PayType"))
        {
            this.ddlPayType.Text = actionParameter["PayType"];
        }
    }

    protected override void DoSearch()
    {
        string paymentNo = this.tbPaymentNo.Text != string.Empty ? this.tbPaymentNo.Text.Trim() : string.Empty;
        string currency = this.tbCurrency.Text != string.Empty ? this.tbCurrency.Text.Trim() : string.Empty;
        string startDate = this.tbStartDate.Text != string.Empty ? this.tbStartDate.Text.Trim() : string.Empty;
        string endDate = this.tbEndDate.Text != string.Empty ? this.tbEndDate.Text.Trim() : string.Empty;
        string voucherNo = this.tbVoucherNo.Text != string.Empty ? this.tbVoucherNo.Text.Trim() : string.Empty;
        string status = this.ddlStatus.SelectedValue;
        string partyCode = this.tbPartyCode.Text != string.Empty ? this.tbPartyCode.Text.Trim() : string.Empty;
        string payType = this.ddlPayType.Text != string.Empty ? this.ddlPayType.Text.Trim() : string.Empty;
        //string extPaymentNo = this.tbExtPaymentNo.Text != string.Empty ? this.tbExtPaymentNo.Text.Trim() : string.Empty;
        //string paymentDate = this.tbPaymentDate.Text != string.Empty ? this.tbPaymentDate.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Payment));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Payment)).SetProjection(Projections.Count("PaymentNo"));

            selectCriteria.CreateAlias("Party", "pf");
            selectCountCriteria.CreateAlias("Party", "pf");

            if (paymentNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("PaymentNo", paymentNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("PaymentNo", paymentNo, MatchMode.Anywhere));
            }
            if (voucherNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("VoucherNo", voucherNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("VoucherNo", voucherNo, MatchMode.Anywhere));
            }
            if (status != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Status", status));
                selectCountCriteria.Add(Expression.Eq("Status", status));
            }
            if (partyCode != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pf.Code", partyCode));
                selectCountCriteria.Add(Expression.Eq("pf.Code", partyCode));
            }
            else
            {
                SecurityHelper.SetPartySearchCriteria(selectCriteria,"pf.Code", this.CurrentUser.Code);
                SecurityHelper.SetPartySearchCriteria(selectCountCriteria, "pf.Code", this.CurrentUser.Code);
            }
            if (currency != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Currency.Code", currency, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Currency.Code", currency, MatchMode.Anywhere));
            }
            if (payType != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("PayType", payType));
                selectCountCriteria.Add(Expression.Eq("PayType", payType));
            }

            /*
            if (extPaymentNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("ExtPaymentNo", extPaymentNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("ExtPaymentNo", extPaymentNo, MatchMode.Anywhere));
            }*/
            if (startDate != string.Empty)
            {
                selectCriteria.Add(Expression.Ge("PaymentDate", DateTime.Parse(startDate)));
                selectCountCriteria.Add(Expression.Ge("PaymentDate", DateTime.Parse(startDate)));
            }
            if (endDate != string.Empty)
            {
                selectCriteria.Add(Expression.Lt("PaymentDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
                selectCountCriteria.Add(Expression.Lt("PaymentDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
            }

            selectCriteria.Add(Expression.Eq("TransType", ModuleType));
            selectCountCriteria.Add(Expression.Eq("TransType", ModuleType));

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    public IList<CodeMaster> GetAllStatus()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        statusGroup.Add(new CodeMaster());//空行
        if (!this.IsSupplier)
        {
            //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        }
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
        //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID));

        return statusGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
}
