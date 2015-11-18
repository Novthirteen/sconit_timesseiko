using System;
using System.Web.UI.HtmlControls;
using com.Sconit.CasClient.Utils;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using System.Threading;
using System.Globalization;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using System.Drawing;
using System.Web;
using com.Sconit.Service.Ext.MasterData;

public partial class Top : System.Web.UI.Page
{
    private IEntityPreferenceMgr TheEntityPreferenceMgr
    {
        get
        {
            return ServiceLocator.GetService<IEntityPreferenceMgr>("EntityPreferenceMgr.service");
        }
    }

    private User CurrentUser
    {
        get
        {
            return (new SessionHelper(this.Page)).CurrentUser;
        }
    }

    private ICodeMasterMgr TheCodeMasterMgr
    {
        get
        {
            return ServiceLocator.GetService<ICodeMasterMgr>("CodeMasterMgr.service");
        }
    }

    protected override void InitializeCulture()
    {
        if (this.CurrentUser.UserLanguage == null || this.CurrentUser.UserLanguage.Trim() == string.Empty)
        {
            string userLanguage = TheCodeMasterMgr.GetDefaultCodeMaster(BusinessConstants.CODE_MASTER_LANGUAGE).Value;
            this.CurrentUser.UserLanguage = userLanguage;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(userLanguage);
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.CurrentUser.UserLanguage);
        }

        //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["OEM"];
        if (cookie != null)
        {
            string imagePath = (cookie.Value == null || cookie.Value.Trim() == string.Empty) ? "Images/" : cookie.Value.Trim();
            this.LeftImage.ImageUrl = imagePath + "Logo_Lit.png";
        }
        this.Info.ForeColor = Color.Black;

        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
        this.Info.Text = TheEntityPreferenceMgr.LoadEntityPreference("CompanyName").Value;

        if (Session[AbstractCasModule.CONST_CAS_PRINCIPAL] != null)
        {
            this.logoffHL.NavigateUrl = "https://sso.hoternet.cn:8443/cas/logout?service=http://demo.sconit.com/Logoff.aspx";
            this.logoffHL.Target = "_parent";
        }
        else
        {
            this.logoffHL.NavigateUrl = "~/Logoff.aspx";
            this.logoffHL.Target = "_parent";
        }

        IItemMgrE TheItemMgr = ServiceLocator.GetService<IItemMgrE>("ItemMgr.service");

        this.data.Value = TheItemMgr.GetCacheAllItemString();
    }
}
