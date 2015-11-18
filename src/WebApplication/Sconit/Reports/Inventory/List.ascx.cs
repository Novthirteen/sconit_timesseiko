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
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using com.Sconit.Utility;
using System.Collections.Generic;

public partial class MasterData_Reports_Inventory_List : ReportModuleBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //UpdateView();
        }
    }

    public override void UpdateView()
    {
        //this.GV_List.Execute();

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LocationLotDetailView lv = (LocationLotDetailView)e.Row.DataItem;
            if (lv.Currency != null)
            {
                ((Label)e.Row.FindControl("lblCurrency")).Text = lv.Currency.Name;
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                lblAmount.Text = lv.Amount.ToString("###,##0.00");
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
        this.ExportXLS(GV_List);
    }

    protected override void SetCriteria()
    {
        DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationLotDetailView));
        criteria.CreateAlias("Location", "l");

        #region Customize
        SecurityHelper.SetRegionSearchCriteria(criteria, "l.Region.Code", this.CurrentUser.Code); //区域权限
        #endregion

        #region Select Parameters
        CriteriaHelper.SetLocationCriteria(criteria, "Location.Code", this._criteriaParam);
        CriteriaHelper.SetItemCriteria(criteria, "Item.Code", this._criteriaParam);
        CriteriaHelper.SetLotNoCriteria(criteria, "LotNo", this._criteriaParam);

        #endregion

        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(criteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));
        //SetSearchCriteria(criteria, selectCountCriteria);

        IList<LocationLotDetailView> locationLotDetailViewList = TheCriteriaMgr.FindAll<LocationLotDetailView>(criteria);
        IList<LocationLotDetailView> locationLotDetailViewListT = new List<LocationLotDetailView>();
        foreach (LocationLotDetailView lv in locationLotDetailViewList)
        {
            PriceListDetail priceListDetail = ThePriceListDetailMgr.GetLastestPriceListDetail(BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE, lv.Item.Code);
            if (priceListDetail != null)
            {

                if (this._criteriaParam.Currency != null && this._criteriaParam.Currency != string.Empty && this._criteriaParam.Currency != priceListDetail.Currency.Code)
                {
                    //locationLotDetailViewList.Remove(lv);
                }
                else
                {
                    lv.Currency = priceListDetail.Currency;
                    decimal qty = lv.Qty.HasValue ? lv.Qty.Value : 0;
                    if (lv.Item.Uom.Code != priceListDetail.Item.Uom.Code)
                    {
                        lv.Amount = TheUomConversionMgr.ConvertUomQty(lv.Item, lv.Item.Uom, qty, priceListDetail.Uom) * priceListDetail.UnitPrice;
                    }
                    else
                    {
                        lv.Amount = qty * priceListDetail.UnitPrice;
                    }

                    locationLotDetailViewListT.Add(lv);
                }
            }
        }
        this.GV_List.DataSource = locationLotDetailViewListT;
        this.GV_List.DataBind();


        decimal totalAmount = locationLotDetailViewListT.Sum(lv => lv.Amount);

        if (locationLotDetailViewListT.Count > 0)
        {
            tabTotalAmount.Visible = true;
            tbTotalAmount.Text = totalAmount.ToString("###,##0.00");

        }
        else
        {
            tabTotalAmount.Visible = false;
        }
    }
}
