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
using com.Sconit.Entity;
using com.Sconit.Web;

public partial class MRP_Main : MainModuleBase
{
    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucTabNavigator.lbPlanningClickEvent += new EventHandler(this.TabPlanningClick_Render);
        this.ucTabNavigator.lbImportClickEvent += new EventHandler(this.TabImportClick_Render);

        if (!IsPostBack)
        {
            this.ucPlanSchedule.Visible = true;
            this.ucPlanImport.Visible = false;

            this.ModuleType = this.ModuleParameter["ModuleType"];
            this.ucPlanSchedule.ModuleType = this.ModuleType;
            this.ucPlanImport.ModuleType = this.ModuleType;
        }
    }

    //The event handler when user click tab "Planning" 
    void TabPlanningClick_Render(object sender, EventArgs e)
    {
        this.ucPlanSchedule.Visible = true;
        this.ucPlanImport.Visible = false;
    }

    //The event handler when user click tab "Import" 
    void TabImportClick_Render(object sender, EventArgs e)
    {
        this.ucPlanSchedule.Visible = false;
        this.ucPlanImport.Visible = true;
    }
}
