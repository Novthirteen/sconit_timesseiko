﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity;
using com.Sconit.Utility;

public partial class Reports_SalePerformance_Search : SearchModuleBase
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
        
        this.tbPartyFrom.ServiceParameter = "string:" + this.ModuleType + ",string:" + this.CurrentUser.Code;
        this.tbPartyTo.ServiceParameter = "string:" + this.ModuleType + ",string:" + this.CurrentUser.Code;

        this.lblPartyFrom.Text = OrderHelper.GetOrderPartyFromLabel(this.ModuleType) + ":";
        this.lblPartyTo.Text = OrderHelper.GetOrderPartyToLabel(this.ModuleType) + ":";

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.lblCreateUser.Text = "${Reports.SalePerformance.Buyer}";
            this.cbClassifiedCreateUser.Text = "${Reports.SalePerformance.Buyer}";
            this.cbClassifiedParty.Text = "${Common.Business.Supplier}";
            this.cbClassifiedCustomer.Text = "${Common.Business.Region}";
            this.tbBillFrom.ServiceParameter = "string:#tbPartyTo";
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.lblCreateUser.Text = "${Reports.SalePerformance.Salesman}";
            this.cbClassifiedCreateUser.Text = "${Reports.SalePerformance.Salesman}";
            this.cbClassifiedParty.Text = "${Common.Business.Region}";
            this.cbClassifiedCustomer.Text = "${Common.Business.Customer}";
			this.tbBillFrom.ServiceParameter = "string:#tbPartyFrom";
        }

        if (!IsPostBack)
        {
            this.tbStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
            object param = this.CollectParam();
            if (param != null)
                ExportEvent(param, null);
        }
    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object criteriaParam = CollectParam();
            SearchEvent(criteriaParam, null);
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("PartyFrom"))
        {
            this.tbPartyFrom.Text = actionParameter["PartyFrom"];
        }
        if (actionParameter.ContainsKey("PartyTo"))
        {
            this.tbPartyTo.Text = actionParameter["PartyTo"];
        }
        if (actionParameter.ContainsKey("BillFrom"))
        {
            this.tbBillFrom.Text = actionParameter["BillFrom"];
        }
        if (actionParameter.ContainsKey("StartDate"))
        {
            this.tbStartDate.Text = actionParameter["StartDate"];
        }
        if (actionParameter.ContainsKey("EndDate"))
        {
            this.tbEndDate.Text = actionParameter["EndDate"];
        }
        if (actionParameter.ContainsKey("Item"))
        {
            this.tbItem.Text = actionParameter["Item"];
        }
        if (actionParameter.ContainsKey("CreateUser"))
        {
            this.tbCreateUser.Text = actionParameter["CreateUser"];
        }
        if (actionParameter.ContainsKey("Currency"))
        {
            this.tbCurrency.Text = actionParameter["Currency"];
        }
    }

    private CriteriaParam CollectParam()
    {
        CriteriaParam criteriaParam = new CriteriaParam();
        criteriaParam.PartyFrom = this.tbPartyFrom.Text.Trim() != string.Empty ? new string[] { this.tbPartyFrom.Text.Trim() } : null;
        criteriaParam.PartyTo = this.tbPartyTo.Text.Trim() != string.Empty ? new string[] { this.tbPartyTo.Text.Trim() } : null;
        criteriaParam.Currency = this.tbCurrency.Text.Trim() != string.Empty ?  this.tbCurrency.Text.Trim()  : null;

        if (this.tbStartDate.Text.Trim() != string.Empty)
            criteriaParam.StartDate = DateTime.Parse(this.tbStartDate.Text);
        if (this.tbEndDate.Text.Trim() != string.Empty)
            criteriaParam.EndDate = DateTime.Parse(this.tbEndDate.Text);

        if (this.tbItem.Text.Trim() != string.Empty)
            criteriaParam.Item = this.tbItem.Text.Trim();

        if (this.tbItemCategory.Text.Trim() != string.Empty)
            criteriaParam.ItemCategory = this.tbItemCategory.Text.Trim();

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            criteriaParam.BillTo = this.tbBillFrom.Text.Trim() != string.Empty ? new string[] { this.tbBillFrom.Text.Trim() } : null;
            criteriaParam.ClassifiedBillTo = this.cbClassifiedBillFrom.Checked;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            criteriaParam.BillFrom = this.tbBillFrom.Text.Trim() != string.Empty ? new string[] { this.tbBillFrom.Text.Trim() } : null;
            criteriaParam.ClassifiedBillFrom = this.cbClassifiedBillFrom.Checked;
        }

        if (this.tbCreateUser.Text.Trim() != string.Empty)
        {
            criteriaParam.UserCode = this.tbCreateUser.Text.Trim();
        }
        
        criteriaParam.ClassifiedUser = this.cbClassifiedCreateUser.Checked;
        criteriaParam.ClassifiedCustomer = this.cbClassifiedCustomer.Checked;
        criteriaParam.ClassifiedItem = this.cbClassifiedItem.Checked;
        criteriaParam.ClassifiedItemCategory = this.cbClassifiedItemCategory.Checked;
        criteriaParam.ClassifiedParty = this.cbClassifiedParty.Checked;
        return criteriaParam;
    }
}
