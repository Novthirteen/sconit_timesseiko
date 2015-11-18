using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using System.Globalization;
using System.Threading;
using com.Sconit.Entity;

public partial class Nav : System.Web.UI.Page
{
    private bool result = false;

    private User CurrentUser
    {
        get
        {
            return (new SessionHelper(this.Page)).CurrentUser;
        }
    }

    private ILanguageMgrE TheLanguageMgr
    {
        get
        {
            return ServiceLocator.GetService<ILanguageMgrE>("LanguageMgr.service");
        }
    }

    private IUserPreferenceMgrE TheUserPreferenceMgr
    {
        get
        {
            return ServiceLocator.GetService<IUserPreferenceMgrE>("UserPreferenceMgr.service");
        }
    }

    private IEntityPreferenceMgrE TheEntityPreferenceMgr
    {
        get
        {
            return ServiceLocator.GetService<IEntityPreferenceMgrE>("EntityPreference.service");
        }
    }

    private string language;

    #region 多语言
    protected override void InitializeCulture()
    {
        this.language = this.CurrentUser.UserLanguage;
        if (this.language == null || this.language.Trim() == string.Empty)
        {
            this.language = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE).Value;
            this.CurrentUser.UserLanguage = this.language;

            UserPreference usrpf = new UserPreference();
            usrpf.User = this.CurrentUser;
            usrpf.Code = BusinessConstants.CODE_MASTER_LANGUAGE;
            usrpf.Value = this.language;
            TheUserPreferenceMgr.CreateUserPreference(usrpf);
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.language);

        //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }
    #endregion

    #region 主题
    protected override void OnPreInit(EventArgs e)
    {
        if (Request.Cookies["ThemePage"] == null)
        {
            this.Page.Theme = "Default";
        }
        else
        {
            this.Page.Theme = Request.Cookies["ThemePage"].Value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
        else
        {
            this.TreeView1.TreeNodeDataBound += new TreeNodeEventHandler(TreeView1_TreeNodeDataBound);
            this.id_hideUser.Value = this.CurrentUser.Code;

            if (!Page.IsPostBack)
            {
                result = true;
            }
        }
    }

    protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        TreeNode treeNode = e.Node;
        SiteMapNode siteMapNode = (SiteMapNode)treeNode.DataItem;

        if (siteMapNode.Url == string.Empty)
        {
            e.Node.SelectAction = TreeNodeSelectAction.Expand;
        }

        #region 生成Icon
        //string ImageIco = "~/Images/Nav/" + siteMapNode.Description + ".png";
        string ImageIco = siteMapNode.Description;
        if (File.Exists(Server.MapPath(ImageIco)))
        {
            treeNode.ImageUrl = ImageIco;
        }
        else
        {
            treeNode.ImageUrl = "~/Images/Nav/Default.png";
        }
        #endregion

        #region 判断权限
        if (this.CurrentUser.Code.ToLower() == "su" || HasPermission(siteMapNode))
        {
            string text = TheLanguageMgr.TranslateContent(treeNode.Text, this.language);
            //string toolTip = TheLanguageMgr.TranslateContent(siteMapNode.ResourceKey, this.language);
            treeNode.ToolTip = "";//siteMapNode.Key;// siteMapNode.Key;
            treeNode.Text = "&nbsp;" + text;
        }
        else
        {
            try
            {
                treeNode.Parent.ChildNodes.Remove(treeNode);
            }
            catch (Exception)
            {
                this.TreeView1.Nodes.Remove(treeNode);
            }
        }
        #endregion

        if (result)
        {
            try
            {
                this.TreeView1.Nodes[0].Expand();
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (this.Session["Current_User"] == null)
        {
            this.Response.Redirect("~/Logoff.aspx");
        }
    }

    private bool HasPermission(SiteMapNode siteMapNode)
    {
        string url = siteMapNode.Url.Trim();
        url = url.Contains("Main.aspx") ? ("~/" + url.Remove(0, siteMapNode.Url.IndexOf("Main.aspx"))) : string.Empty;

        if (this.CurrentUser.HasPermission(url))
        {
            return true;
        }
        else
        {
            if (siteMapNode.ChildNodes != null && siteMapNode.ChildNodes.Count > 0)
            {
                foreach (SiteMapNode childSiteMapNode in siteMapNode.ChildNodes)
                {
                    if (HasPermission(childSiteMapNode))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}