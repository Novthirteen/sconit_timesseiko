using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;

public partial class Reports_OrderTracking_List : ListModuleBase
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
    

    public override void UpdateView()
    {
        this.isExport = false;
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.isExport = true;
        this.ExportXLS(GV_List, "OrderTracking.xls");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                //this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyFrom}";
                this.GV_List.Columns[2].Visible = false;
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                //this.GV_List.Columns[1].HeaderText = "${Reports.ActFunds.PartyTo}";
                this.GV_List.Columns[1].Visible = false;
            }
        }
    }

    protected void lbtnBilledAmount_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 1;
        OnBilledAmountClick(orderNo, itemCode, range);
    }

    protected void lbtnRecQty_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 2;
        OnRecQtyClick(orderNo, itemCode, range);
    }


    private void OnRecQtyClick(string orderNo, string itemCode, int range)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ReceiptDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ReceiptDetail))
            .SetProjection(Projections.Count("Id"));
        
        IDictionary<string, string> alias = new Dictionary<string, string>();
        selectCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCriteria.CreateAlias("olt.OrderDetail", "olto");
        selectCriteria.CreateAlias("olto.OrderHead", "oltoo");
        selectCriteria.CreateAlias("olt.Item", "i");

        selectCountCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCountCriteria.CreateAlias("olt.OrderDetail", "olto");
        selectCountCriteria.CreateAlias("olto.OrderHead", "oltoo");
        selectCountCriteria.CreateAlias("olt.Item", "i");

        alias.Add("OrderLocationTransaction", "olt");
        alias.Add("olt.OrderDetail", "olto");
        alias.Add("olto.OrderHead", "oltoo");
        alias.Add("olt.Item", "i");
  
        selectCriteria.Add(Expression.Eq("oltoo.OrderNo", orderNo));
        selectCountCriteria.Add(Expression.Eq("oltoo.OrderNo", orderNo));

        selectCriteria.Add(Expression.Eq("i.Code", itemCode));
        selectCountCriteria.Add(Expression.Eq("i.Code", itemCode));
        
        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias, range }), null);
    }

    private void OnBilledAmountClick(string orderNo, string itemCode, int range)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(BillDetView));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BillDetView))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();
        selectCriteria.CreateAlias("OrderHead", "oh");
        selectCriteria.CreateAlias("Item", "i");
        selectCriteria.CreateAlias("Bill", "b");

        selectCountCriteria.CreateAlias("OrderHead", "oh");
        selectCountCriteria.CreateAlias("Item", "i");
        selectCountCriteria.CreateAlias("Bill", "b");

        alias.Add("OrderHead", "oh");
        alias.Add("Item", "i");
        alias.Add("Bill", "b");

        selectCriteria.Add(Expression.Eq("oh.OrderNo", orderNo));
        selectCountCriteria.Add(Expression.Eq("oh.OrderNo", orderNo));

        selectCriteria.Add(Expression.Eq("i.Code", itemCode));
        selectCountCriteria.Add(Expression.Eq("i.Code", itemCode));

        selectCriteria.Add(Expression.Not(Expression.Eq("b.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID)));
        selectCriteria.Add(Expression.Not(Expression.Eq("b.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)));

        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias, range }), null);
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderTrackingView afav = (OrderTrackingView)e.Row.DataItem;
            
            this.SetLinkButton(e.Row, "lblRecQty", afav.RecQty != null && afav.RecQty.Value != 0);
            //this.SetLinkButton(e.Row, "lblNoRecQty", afav.NoRecQty != null && afav.NoRecQty.Value != 0);
            this.SetLinkButton(e.Row, "lbtnBilledAmount", afav.BilledAmount != null && afav.BilledAmount.Value != 0);
            //this.SetLinkButton(e.Row, "lbtnNoBilledAmount", afav.NoBilledAmount != null && afav.NoBilledAmount.Value != 0);

            if (isExport)
            {
                e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }
    
    
}
