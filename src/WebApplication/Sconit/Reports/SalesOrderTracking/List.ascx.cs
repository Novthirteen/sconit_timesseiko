using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;

public partial class Reports_SalesOrderTracking_List : ListModuleBase
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
        this.ExportXLS(GV_List, "SalesOrderTracking.xls");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ( e.Row.RowType == DataControlRowType.DataRow)
        {
            SalesOrderTrackingView safav = (SalesOrderTrackingView)e.Row.DataItem;
            
            this.SetLinkButton(e.Row, "lblDeliverQty", safav.DeliverQty != null && safav.DeliverQty.Value != 0);
            //this.SetLinkButton(e.Row, "lblNoDeliverQty", safav.NoDeliverQty != null && safav.NoDeliverQty.Value != 0);
            this.SetLinkButton(e.Row, "lblOrderedQty", safav.OrderedQty != null && safav.OrderedQty.Value != 0);
            this.SetLinkButton(e.Row, "lbtnBilledAmount", safav.BilledAmount != null && safav.BilledAmount.Value != 0);
            //this.SetLinkButton(e.Row, "lbtnNoBilledAmount", safav.NoBilledAmount != null && safav.NoBilledAmount.Value != 0);

            if (isExport)
            {
                e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }

    

    protected void lbtnOrderQty_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 4;
        OnOrderQtyClick(orderNo, itemCode, range);
    }

    protected void lbtnBilledAmount_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 1;
        OnBilledAmountClick(orderNo, itemCode, range);
    }

    protected void lbtnDeliverQty_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 2;
        OnDeliverQtyClick(orderNo, itemCode, range);
    }


    //已定数
    protected void lbtnOrderedQty_Click(object sender, EventArgs e)
    {
        string[] arg = ((LinkButton)sender).CommandArgument.Split(',');
        string orderNo = arg[0];
        string itemCode = arg[1];
        int range = 3;
        OnOrderedQtyClick(orderNo, itemCode, range);
    }

    private void OnOrderQtyClick(string orderNo, string itemCode, int range)
    {
    
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderDetail))
            .SetProjection(Projections.Count("Id"));

        selectCriteria.CreateAlias("OrderHead", "oh");
        selectCriteria.CreateAlias("Item", "i");
        selectCountCriteria.CreateAlias("OrderHead", "oh");
        selectCountCriteria.CreateAlias("Item", "i");

        IDictionary<string, string> alias = new Dictionary<string, string>();
        alias.Add("OrderHead", "oh");
        alias.Add("Item", "i");

        selectCriteria.Add(Expression.Eq("OrderHead.OrderNo", orderNo));
        selectCountCriteria.Add(Expression.Eq("OrderHead.OrderNo", orderNo));

        selectCriteria.Add(Expression.Eq("Item.Code", itemCode));
        selectCountCriteria.Add(Expression.Eq("Item.Code", itemCode));


        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias, range }), null);

    }


    private void OnOrderedQtyClick(string orderNo, string itemCode, int range)
    {
        /*
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderTracer));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderTracer))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.Add(Expression.Eq("Code", orderNo));
        selectCountCriteria.Add(Expression.Eq("Code", orderNo));

        selectCriteria.Add(Expression.Eq("Item", itemCode));
        selectCountCriteria.Add(Expression.Eq("Item", itemCode));
        */

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetTracerView));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(OrderDetTracerView))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        selectCriteria.Add(Expression.Eq("OrderTracerCode", orderNo));
        selectCountCriteria.Add(Expression.Eq("OrderTracerCode", orderNo));

        selectCriteria.Add(Expression.Eq("OrderTracerItem", itemCode));
        selectCountCriteria.Add(Expression.Eq("OrderTracerItem", itemCode));


        DetailEvent((new object[] { selectCriteria, selectCountCriteria, alias, range }), null);

    }

    private void OnDeliverQtyClick(string orderNo, string itemCode, int range)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(InProcessLocationDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(InProcessLocationDetail))
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
}
