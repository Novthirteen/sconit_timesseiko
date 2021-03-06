﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Utility;
using com.Sconit.Entity.MasterData;

public partial class Reports_BillPayment_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler ExportEvent;

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
            this.lblParty.Text = "${Reports.ActBill.PartyFrom}:";
            this.tbParty.ServicePath = "SupplierMgr.service";
            this.tbParty.ServiceMethod = "GetSupplier";
            this.tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.lblParty.Text = "${Reports.ActBill.PartyTo}:";
            this.tbParty.ServicePath = "CustomerMgr.service";
            this.tbParty.ServiceMethod = "GetCustomer";
            this.tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;
        }

        if (!IsPostBack)
        {
            this.ddlStatus.DataSource = this.GetAllStatus();
            this.ddlStatus.DataBind();

            this.tbEffectiveDateFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            this.tbEffectiveDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        this.DoSearch();

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        if (ExportEvent != null)
        {
            object[] param = this.CollectParam();
            if (param != null)
                ExportEvent(param, null);
        }

    }

    protected override void DoSearch()
    {

        if (SearchEvent != null)
        {
            object[] param = CollectParam();
            if (param != null)
                SearchEvent(param, null);
        }
    }

    private object[] CollectParam()
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Bill));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Bill))
            .SetProjection(Projections.Count("BillNo"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.CreateAlias("BillAddress", "ba");
        selectCountCriteria.CreateAlias("BillAddress", "ba");

        alias.Add("BillAddress", "ba");

        if (this.tbBillNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("BillNo", this.tbBillNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("BillNo", this.tbBillNo.Text.Trim()));
        }

        if (this.tbExternalBillNo.Text.Trim() != string.Empty)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                selectCriteria.Add(Expression.Eq("ExternalBillNo", this.tbExternalBillNo.Text.Trim()));
                selectCountCriteria.Add(Expression.Eq("ExternalBillNo", this.tbExternalBillNo.Text.Trim()));
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                selectCriteria.Add(Expression.Eq("ExternalBillNo", this.tbExternalBillNo.Text.Trim()));
                selectCountCriteria.Add(Expression.Eq("ExternalBillNo", this.tbExternalBillNo.Text.Trim()));
            }
        }

        string status = this.ddlStatus.SelectedValue;
        if (status != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("Status", status));
            selectCountCriteria.Add(Expression.Eq("Status", status));
        }

        #region party设置

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            selectCriteria.CreateAlias("ba.Party", "pf");
            selectCountCriteria.CreateAlias("ba.Party", "pf");

            alias.Add("BillAddress.Party", "pf");

            if (this.tbParty.Text.Trim() != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pf.Code", this.tbParty.Text.Trim()));
                selectCountCriteria.Add(Expression.Eq("pf.Code", this.tbParty.Text.Trim()));
            }
            else
            {
                SecurityHelper.SetPartyFromSearchCriteria(
                    selectCriteria, selectCountCriteria, (this.tbParty != null ? this.tbParty.Text : null), this.ModuleType, this.CurrentUser.Code);
            }
            selectCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));
            selectCountCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            selectCriteria.CreateAlias("ba.Party", "pt");
            selectCountCriteria.CreateAlias("ba.Party", "pt");

            alias.Add("BillAddress.Party", "pt");

            if (this.tbParty.Text.Trim() != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pt.Code", this.tbParty.Text.Trim()));
                selectCountCriteria.Add(Expression.Eq("pt.Code", this.tbParty.Text.Trim()));
            }
            else
            {
                SecurityHelper.SetPartyToSearchCriteria(
                selectCriteria, selectCountCriteria, (this.tbParty != null ? this.tbParty.Text : null), this.ModuleType, this.CurrentUser.Code);
            }

            selectCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_SO));
            selectCountCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_SO));
        }

        #endregion

        if (this.tbCurrency.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Currency.Code", this.tbCurrency.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Currency.Code", this.tbCurrency.Text.Trim(), MatchMode.Anywhere));
        }


        if (this.tbEffectiveDateFrom.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("EffectiveDate", DateTime.Parse(this.tbEffectiveDateFrom.Text.Trim())));
            selectCountCriteria.Add(Expression.Ge("EffectiveDate", DateTime.Parse(this.tbEffectiveDateFrom.Text.Trim())));
        }

        if (this.tbEffectiveDateTo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("EffectiveDate", DateTime.Parse(this.tbEffectiveDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
            selectCountCriteria.Add(Expression.Le("EffectiveDate", DateTime.Parse(this.tbEffectiveDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
        }

        selectCriteria.Add(Expression.Or(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT), Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)));
        selectCountCriteria.Add(Expression.Or(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT), Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)));

        return new object[] { selectCriteria, selectCountCriteria, alias };

    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //if (actionParameter.ContainsKey("Location"))
        //{
        //    this.tbLocation.Text = actionParameter["Location"];
        //}
        //if (actionParameter.ContainsKey("Item"))
        //{
        //    this.tbItem.Text = actionParameter["Item"];
        //}
        //if (actionParameter.ContainsKey("EffDate"))
        //{
        //    this.tbEffDate.Text = actionParameter["EffDate"];
        //}
    }


    public IList<CodeMaster> GetAllStatus()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
        return statusGroup;
    }
    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
}