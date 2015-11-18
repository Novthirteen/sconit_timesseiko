using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Collections;

public partial class Inventory_Stocktaking_ResultDetail : ListModuleBase
{
     
    public override void UpdateView()
    {
        //this.GV_List.Execute();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    public void UpdateView(IList<CycleCountResult> cycleCountResultList, bool isShortage, bool isProfit, bool isEqual)
    {
        this.GV_List.DataSource = cycleCountResultList;
        this.GV_List.DataBind();

        if (isShortage)
        {
            this.GV_List.Columns[6].Visible = false;
            this.GV_List.Columns[7].Visible = true;
        }

        if (isProfit)
        {
            this.GV_List.Columns[6].Visible = true;
            this.GV_List.Columns[7].Visible = false;
        }

        if (isEqual)
        {
            this.GV_List.Columns[6].Visible = false;
            this.GV_List.Columns[7].Visible = true;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }
}