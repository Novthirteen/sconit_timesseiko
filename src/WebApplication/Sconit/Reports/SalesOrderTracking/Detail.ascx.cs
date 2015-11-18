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

public partial class Reports_SalesOrderTracking_Detail : EditModuleBase
{
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
        if (!IsPostBack)
        {
            this.ucBillList.ModuleType = this.ModuleType;
            this.ucOrderTracerList.ModuleType = this.ModuleType;
            this.ucIpDetList.ModuleType = this.ModuleType;
            this.ucOrderDetailList.ModuleType = this.ModuleType;
        }
    }

    public void SetSearchCriteria(DetachedCriteria SelectCriteria, DetachedCriteria SelectCountCriteria, IDictionary<string, string> alias, int range)
    {
        switch (range)
        {
            case 1:
                this.ucOrderTracerList.Visible = false;
                this.ucIpDetList.Visible = false;
                this.ucOrderDetailList.Visible = false;
                this.ucBillList.Visible = true;
                this.ucBillList.SetSearchCriteria(SelectCriteria, SelectCountCriteria, alias);
                this.ucBillList.UpdateView();
                break;
            case 2:
                this.ucOrderTracerList.Visible = false;
                this.ucBillList.Visible = false;
                this.ucOrderDetailList.Visible = false;
                this.ucIpDetList.Visible = true;
                this.ucIpDetList.SetSearchCriteria(SelectCriteria, SelectCountCriteria, alias);
                this.ucIpDetList.UpdateView();
                break;
            case 3:
                this.ucIpDetList.Visible = false;
                this.ucBillList.Visible = false;
                this.ucOrderDetailList.Visible = false;
                this.ucOrderTracerList.Visible = true;
                this.ucOrderTracerList.SetSearchCriteria(SelectCriteria, SelectCountCriteria, alias);
                this.ucOrderTracerList.UpdateView();
                break;
            case 4:
                this.ucIpDetList.Visible = false;
                this.ucBillList.Visible = false;
                this.ucOrderDetailList.Visible = true;
                this.ucOrderTracerList.Visible = false;
                this.ucOrderDetailList.SetSearchCriteria(SelectCriteria, SelectCountCriteria, alias);
                this.ucOrderDetailList.UpdateView();
                break;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }
}