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
using com.Sconit.Utility;
using com.Sconit.Entity.MasterData;

public partial class Reports_OrderTracking_Search : SearchModuleBase
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
            this.lblParty.Text = "${Reports.OrderTracking.PartyFrom}:";
            this.tbParty.ServicePath = "SupplierMgr.service";
            this.tbParty.ServiceMethod = "GetSupplier";
            this.tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.lblParty.Text = "${Reports.OrderTracking.PartyTo}:";
            this.tbParty.ServicePath = "CustomerMgr.service";
            this.tbParty.ServiceMethod = "GetCustomer";
            this.tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;
        }

        if (!IsPostBack)
        {
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
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderTrackingView));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderTrackingView))
            .SetProjection(Projections.Count("OrderHead"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.CreateAlias("OrderHead", "oh");
        selectCountCriteria.CreateAlias("OrderHead", "oh");

        alias.Add("OrderHead", "oh");

        if (this.tbOrderNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("oh.OrderNo", this.tbOrderNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("oh.OrderNo", this.tbOrderNo.Text.Trim()));
        }

        if (this.tbItem.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("Item.Code", this.tbItem.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("Item.Code", this.tbItem.Text.Trim()));
        }

        #region party设置
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            selectCriteria.CreateAlias("OrderHead.PartyFrom", "pf");
            selectCountCriteria.CreateAlias("OrderHead.PartyFrom", "pf");

            alias.Add("OrderHead.PartyFrom", "pf");

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
            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT));
            selectCountCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT));
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            selectCriteria.CreateAlias("OrderHead.PartyTo", "pt");
            selectCountCriteria.CreateAlias("OrderHead.PartyTo", "pt");

            alias.Add("OrderHead.PartyTo", "pt");

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

            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
            selectCountCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
        }
        #endregion

        if (this.tbCurrency.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("oh.Currency.Code", this.tbCurrency.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("oh.Currency.Code", this.tbCurrency.Text.Trim(), MatchMode.Anywhere));
        }

        if (this.tbEffectiveDateFrom.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("oh.CreateDate", DateTime.Parse(this.tbEffectiveDateFrom.Text.Trim())));
            selectCountCriteria.Add(Expression.Ge("oh.CreateDate", DateTime.Parse(this.tbEffectiveDateFrom.Text.Trim())));
        }

        if (this.tbEffectiveDateTo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("oh.CreateDate", DateTime.Parse(this.tbEffectiveDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
            selectCountCriteria.Add(Expression.Le("oh.CreateDate", DateTime.Parse(this.tbEffectiveDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
        }

        selectCriteria.Add(Expression.Eq("oh.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
        selectCountCriteria.Add(Expression.Eq("oh.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
        OrderHelper.SetActiveOrderStatusCriteria(selectCriteria, "oh.Status");//状态
        OrderHelper.SetActiveOrderStatusCriteria(selectCountCriteria, "oh.Status");//状态


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
}