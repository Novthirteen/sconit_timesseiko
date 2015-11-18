using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Utility;
using System.Collections;
using com.Sconit.Entity.MasterData;
using NHibernate.Transform;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Reports_SalePerformance_List : ReportModuleBase
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.GV_List.Columns[1].HeaderText = "${Common.Business.Supplier}";
            this.GV_List.Columns[5].HeaderText = "${Common.Business.Region}";
            //this.GV_List.Columns[2].Visible = false;
            //this.GV_List.Columns[14].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[1].HeaderText = "${Common.Business.Region}";
            this.GV_List.Columns[5].HeaderText = "${Common.Business.Customer}";
            //this.GV_List.Columns[3].Visible = false;
            //this.GV_List.Columns[13].Visible = false;
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SalePerformance spf = (SalePerformance)e.Row.DataItem;
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT && spf.PartyFrom != null)
            {

                Label lblPartyFrom = (Label)e.Row.FindControl("lblPartyFrom");
                lblPartyFrom.Text = spf.PartyFrom.Code;
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION && spf.PartyTo != null)
            {
                Label lblPartyTo = (Label)e.Row.FindControl("lblPartyTo");
                lblPartyTo.Text = spf.PartyTo.Code;
            }

            this.SetLinkButton(e.Row, "lbtnIncludeTaxTotalPrice", spf.IncludeTaxTotalPrice != 0);
            this.SetLinkButton(e.Row, "lbtnOrderedQty", spf.OrderedQty != 0);

            if (isExport)
            {
                e.Row.Cells[7].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[11].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[13].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }

    public override void InitPageParameter(object sender)
    {
        this._criteriaParam = (CriteriaParam)sender;
        this.SetCriteria();
        this.UpdateView();
    }

    public void Export()
    {
        this.isExport = true;
        this.ExportXLS(GV_List);
    }

    protected void lbtnDetail_Click(object sender, EventArgs e)
    {
        int orderDetailId = Int32.Parse(((LinkButton)sender).CommandArgument);

        OrderDetail orderDetail = TheOrderDetailMgr.LoadOrderDetail(orderDetailId);

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));

        selectCriteria.CreateAlias("OrderHead", "oh");
        selectCriteria.CreateAlias("Item", "i");
        selectCriteria.CreateAlias("Item.ItemCategory", "ii");
        selectCriteria.CreateAlias("OrderHead.BillFrom", "bf");
        selectCriteria.CreateAlias("OrderHead.BillTo", "bt");
        selectCriteria.CreateAlias("OrderHead.Currency", "c");
        selectCriteria.CreateAlias("OrderHead.PartyFrom", "pf");
        selectCriteria.CreateAlias("OrderHead.PartyTo", "pt");
        selectCriteria.CreateAlias("OrderHead.CreateUser", "ohc");
        IDictionary<string, string> alias = new Dictionary<string, string>();

        alias.Add("OrderHead", "oh");
        alias.Add("Item", "i");
        alias.Add("Item.ItemCategory", "ii");
        alias.Add("OrderHead.BillFrom", "bf");
        alias.Add("OrderHead.BillTo", "bo");
        alias.Add("OrderHead.Currency", "c");
        alias.Add("OrderHead.PartyFrom", "pf");
        alias.Add("OrderHead.PartyTo", "pt");
        alias.Add("OrderHead.CreateUser", "ohc");
        #region Customize

        OrderHelper.SetActiveOrderStatusCriteria(selectCriteria, "oh.Status");//状态
        #endregion

        #region Select Parameters

        CriteriaHelper.SetStartDateCriteria(selectCriteria, "oh.CreateDate", this._criteriaParam);
        CriteriaHelper.SetEndDateCriteria(selectCriteria, "oh.CreateDate", this._criteriaParam);
        CriteriaHelper.SetItemCriteria(selectCriteria, "i.Code", this._criteriaParam);
        CriteriaHelper.SetItemCategoryCriteria(selectCriteria, "ii.Code", this._criteriaParam);

        if (this._criteriaParam.Currency != null && this._criteriaParam.Currency != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("c.Code", this._criteriaParam.Currency));
        }

        selectCriteria.Add(Expression.Eq("c.Code", orderDetail.OrderHead.Currency.Code));


        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(selectCriteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));


        if (this._criteriaParam.ClassifiedParty)//区域/供应商
        {
            selectCriteria.Add(Expression.Eq("pf.Code", orderDetail.OrderHead.PartyFrom.Code));
            selectCountCriteria.Add(Expression.Eq("pf.Code", orderDetail.OrderHead.PartyFrom.Code));
        }

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT && this._criteriaParam.ClassifiedBillTo)
        {
            selectCriteria.Add(Expression.Eq("bt.Code", orderDetail.OrderHead.BillTo.Code));//抬头
            selectCountCriteria.Add(Expression.Eq("bt.Code", orderDetail.OrderHead.BillTo.Code));//抬头
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION && this._criteriaParam.ClassifiedBillFrom)
        {
            selectCriteria.Add(Expression.Eq("bf.Code", orderDetail.OrderHead.BillFrom.Code));//抬头
            selectCountCriteria.Add(Expression.Eq("bf.Code", orderDetail.OrderHead.BillFrom.Code));//抬头
        }

        if (this._criteriaParam.ClassifiedUser)
        {
            selectCriteria.Add(Expression.Eq("ohc.Code", orderDetail.OrderHead.CreateUser.Code));//销售员 采购员
            selectCountCriteria.Add(Expression.Eq("ohc.Code", orderDetail.OrderHead.CreateUser.Code));//销售员 采购员
        }

        if (this._criteriaParam.ClassifiedCustomer)//客户/区域
        {
            selectCriteria.Add(Expression.Eq("pt.Code", orderDetail.OrderHead.PartyTo.Code));
            selectCountCriteria.Add(Expression.Eq("pt.Code", orderDetail.OrderHead.PartyTo.Code));
        }

        if (this._criteriaParam.ClassifiedItemCategory)
        {
            selectCriteria.Add(Expression.Eq("ii.Code", orderDetail.Item.ItemCategory.Code));//产品类
            selectCountCriteria.Add(Expression.Eq("ii.Code", orderDetail.Item.ItemCategory.Code));//产品类
        }

        if (this._criteriaParam.ClassifiedItem)
        {
            selectCriteria.Add(Expression.Eq("i.Code", orderDetail.Item.Code));//产品
            selectCountCriteria.Add(Expression.Eq("i.Code", orderDetail.Item.Code));//产品
        }

        if (this._criteriaParam.PartyFrom != null && this._criteriaParam.PartyFrom.Length > 0)
        {
            selectCriteria.Add(Expression.In("pf.Code", this._criteriaParam.PartyFrom));
            selectCountCriteria.Add(Expression.In("pf.Code", this._criteriaParam.PartyFrom));
        }
        else 
        {
            #region partyFrom
            SecurityHelper.SetPartyFromSearchCriteria(
                selectCriteria, selectCountCriteria, null, this.ModuleType, this.CurrentUser.Code);
            #endregion
        }

        if (this._criteriaParam.PartyTo != null && this._criteriaParam.PartyTo.Length > 0)
        {
            selectCriteria.Add(Expression.In("pt.Code", this._criteriaParam.PartyTo));
            selectCountCriteria.Add(Expression.In("pt.Code", this._criteriaParam.PartyTo));
        }
        else 
        {
            #region partyTo
            SecurityHelper.SetPartyToSearchCriteria(
                selectCriteria, selectCountCriteria, null, this.ModuleType, this.CurrentUser.Code);
            #endregion
        }

        selectCriteria.Add(Expression.Eq("oh.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
        selectCountCriteria.Add(Expression.Eq("oh.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT));
            selectCountCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT));
            if (this._criteriaParam.BillTo != null)
            {
                selectCriteria.Add(Expression.In("bt.Code", this._criteriaParam.BillTo));
                selectCountCriteria.Add(Expression.In("bt.Code", this._criteriaParam.BillTo));
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
            selectCountCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
            if (this._criteriaParam.BillFrom != null)
            {
                selectCriteria.Add(Expression.In("bf.Code", this._criteriaParam.BillFrom));
                selectCountCriteria.Add(Expression.In("bf.Code", this._criteriaParam.BillFrom));
            }
        }

        #endregion

        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias }), null);
    }

    public override void UpdateView()
    {

        this.isExport = false;

        //区域 抬头 销售员 客户 产品类 产品
        GV_List.Columns[1].Visible = this._criteriaParam.ClassifiedParty;//区域|供应商
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            GV_List.Columns[2].Visible = false;
            this.GV_List.Columns[3].Visible = this._criteriaParam.ClassifiedBillTo;//抬头
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[3].Visible = false;
            GV_List.Columns[2].Visible = this._criteriaParam.ClassifiedBillFrom;//抬头
        }
        GV_List.Columns[4].Visible = this._criteriaParam.ClassifiedUser;//销售员
        GV_List.Columns[5].Visible = this._criteriaParam.ClassifiedCustomer;//客户|区域
        GV_List.Columns[6].Visible = this._criteriaParam.ClassifiedItemCategory;//产品类
        GV_List.Columns[7].Visible = this._criteriaParam.ClassifiedItem;//物料代码
        GV_List.Columns[8].Visible = this._criteriaParam.ClassifiedItem;//品名
        GV_List.Columns[9].Visible = this._criteriaParam.ClassifiedItem;//规格型号
        GV_List.Columns[10].Visible = this._criteriaParam.ClassifiedItem;//品牌
        //this.GV_List.Execute();
    }

    protected override void SetCriteria()
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));

        selectCriteria.CreateAlias("OrderHead", "oh");
        selectCriteria.CreateAlias("Item", "i");
        selectCriteria.CreateAlias("Item.ItemCategory", "ii");
        selectCriteria.CreateAlias("OrderHead.BillFrom", "bf");
        selectCriteria.CreateAlias("OrderHead.BillTo", "bt");
        selectCriteria.CreateAlias("OrderHead.Currency", "c");
        selectCriteria.CreateAlias("OrderHead.PartyFrom", "pf");
        selectCriteria.CreateAlias("OrderHead.PartyTo", "pt");
        selectCriteria.CreateAlias("OrderHead.CreateUser", "ohc");

        #region Customize

        OrderHelper.SetActiveOrderStatusCriteria(selectCriteria, "oh.Status");//状态
        #endregion

        #region Select Parameters

        CriteriaHelper.SetStartDateCriteria(selectCriteria, "oh.CreateDate", this._criteriaParam);
        CriteriaHelper.SetEndDateCriteria(selectCriteria, "oh.CreateDate", this._criteriaParam);
        CriteriaHelper.SetItemCriteria(selectCriteria, "i.Code", this._criteriaParam);
        CriteriaHelper.SetItemCategoryCriteria(selectCriteria, "ii.Code", this._criteriaParam);

        if (this._criteriaParam.UserCode != null && this._criteriaParam.UserCode != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ohc.Code", this._criteriaParam.UserCode, MatchMode.Anywhere));
        }

        if (this._criteriaParam.Currency != null && this._criteriaParam.Currency != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("c.Code", this._criteriaParam.Currency));
        }

        selectCriteria.Add(Expression.Eq("oh.SubType", BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT));
            if (this._criteriaParam.BillTo != null)
            {
                selectCriteria.Add(Expression.In("bt.Code", this._criteriaParam.BillTo));
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            selectCriteria.Add(Expression.Eq("oh.Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION));
            if (this._criteriaParam.BillFrom != null)
            {
                selectCriteria.Add(Expression.In("bf.Code", this._criteriaParam.BillFrom));
            }
        }

        #endregion

        #region Projections
        ProjectionList projectionList = Projections.ProjectionList()
            .Add(Projections.Max("Id").As("Id"))
            .Add(Projections.Sum("OrderedQty").As("OrderedQty"))
            .Add(Projections.Sum("UnitPrice").As("UnitPrice"))
            .Add(Projections.Sum("UnitPriceAfterDiscount").As("UnitPriceAfterDiscount"))
            .Add(Projections.Sum("IncludeTaxPrice").As("IncludeTaxPrice"))
            .Add(Projections.Sum("IncludeTaxTotalPrice").As("IncludeTaxTotalPrice"));

        projectionList.Add(Projections.GroupProperty("oh.Currency").As("Currency"));//币种

        if (this._criteriaParam.ClassifiedParty)
        {
            projectionList.Add(Projections.GroupProperty("oh.PartyFrom").As("PartyFrom"));//区域/供应商
        }

        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT && this._criteriaParam.ClassifiedBillTo)
        {
            projectionList.Add(Projections.GroupProperty("oh.BillTo").As("BillTo"));//抬头
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION && this._criteriaParam.ClassifiedBillFrom)
        {
            projectionList.Add(Projections.GroupProperty("oh.BillFrom").As("BillFrom"));//抬头
        }

        if (this._criteriaParam.ClassifiedUser)
        {
            projectionList.Add(Projections.GroupProperty("oh.CreateUser").As("CreateUser"));//销售员
        }

        if (this._criteriaParam.ClassifiedCustomer)
        {
            projectionList.Add(Projections.GroupProperty("oh.PartyTo").As("PartyTo"));//客户/区域
        }

        if (this._criteriaParam.ClassifiedItemCategory)
        {
            projectionList.Add(Projections.GroupProperty("i.ItemCategory").As("ItemCategory"));//产品类
        }

        if (this._criteriaParam.ClassifiedItem)
        {
            projectionList.Add(Projections.GroupProperty("Item").As("Item"));//产品
        }

        selectCriteria.SetProjection(projectionList);

        #endregion


        selectCriteria.SetResultTransformer(Transformers.AliasToBean(typeof(SalePerformance)));
        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(selectCriteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));

        if (this._criteriaParam.PartyFrom != null && this._criteriaParam.PartyFrom.Length > 0)
        {
            selectCriteria.Add(Expression.In("pf.Code", this._criteriaParam.PartyFrom));
            selectCountCriteria.Add(Expression.In("pf.Code", this._criteriaParam.PartyFrom));
        }
        else 
        {
            #region partyFrom
            SecurityHelper.SetPartyFromSearchCriteria(
                selectCriteria, selectCountCriteria, null, this.ModuleType, this.CurrentUser.Code);
            #endregion
        }

        if (this._criteriaParam.PartyTo != null && this._criteriaParam.PartyTo.Length > 0)
        {
            selectCriteria.Add(Expression.In("pt.Code", this._criteriaParam.PartyTo));
            selectCountCriteria.Add(Expression.In("pt.Code", this._criteriaParam.PartyTo));
        }
        else 
        {
            #region partyTo
            SecurityHelper.SetPartyToSearchCriteria(
                selectCriteria, selectCountCriteria, null, this.ModuleType, this.CurrentUser.Code);
            #endregion
        }

        IList<SalePerformance> salePerformanceList = TheCriteriaMgr.FindAll<SalePerformance>(selectCriteria);

        this.GV_List.DataSource = salePerformanceList;
        this.GV_List.DataBind();
        if (salePerformanceList == null || salePerformanceList.Count == 0)
        {
            this.tabTotalAmount.Visible = false;
            this.lblNoRecordFound.Visible = true;
        }
        else
        {

            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                ||this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                decimal? totalAmount = salePerformanceList.Sum(spl => spl.IncludeTaxTotalPrice);
                this.tbTotalAmount.Text = totalAmount.HasValue ? totalAmount.Value.ToString("###,##0.00") : "0.00";
            }
            
            this.tabTotalAmount.Visible = true;
            this.lblNoRecordFound.Visible = false;
        }
    }
}
