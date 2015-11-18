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

public partial class Reports_ActFundsAging_Detail : EditModuleBase
{

    public event EventHandler BillDetailEvent;

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
        this.ucDetail.DetailEvent += new EventHandler(ucList_DetailEvent);
        if (!IsPostBack)
        {
            this.ucDetail.ModuleType = this.ModuleType;
        }
    }

    public void SetSearchCriteria(DetachedCriteria SelectCriteria, DetachedCriteria SelectCountCriteria, IDictionary<string, string> alias)
    {
        this.ucDetail.SetSearchCriteria(SelectCriteria, SelectCountCriteria, alias);
        this.ucDetail.UpdateView();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }


    void ucList_DetailEvent(object sender, EventArgs e)
    {
        this.Visible = false;
        BillDetailEvent(sender, e);
    }
}