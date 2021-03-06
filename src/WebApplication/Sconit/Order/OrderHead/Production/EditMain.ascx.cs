﻿using System;
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
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

public partial class Order_OrderHead_EditMain : EditModuleBase
{
    public event EventHandler BackEvent;

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

    public string ModuleSubType
    {
        get
        {
            return (string)ViewState["ModuleSubType"];
        }
        set
        {
            ViewState["ModuleSubType"] = value;
        }
    }

    //新品
    public bool NewItem
    {
        get
        {
            return (bool)ViewState["NewItem"];
        }
        set
        {
            ViewState["NewItem"] = value;
        }
    }

    //报废
    public bool IsScrap
    {
        get
        {
            return (bool)ViewState["IsScrap"];
        }
        set
        {
            ViewState["IsScrap"] = value;
        }
    }

    //原材料回用
    public bool IsReuse
    {
        get
        {
            return (bool)ViewState["IsReuse"];
        }
        set
        {
            ViewState["IsReuse"] = value;
        }
    }

    public void InitPageParameter(string orderNo)
    {
     
        this.ucEdit.InitPageParameter(orderNo);
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucTabNavigator.lblMstrClickEvent += new System.EventHandler(this.TabMstrClick_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.Back_Render);
      
        if (!IsPostBack)
        {
            this.ucTabNavigator.ModuleType = this.ModuleType;
            this.ucTabNavigator.ModuleSubType = this.ModuleSubType;
            this.ucEdit.ModuleType = this.ModuleType;
            this.ucEdit.ModuleSubType = this.ModuleSubType;
         
          
            this.ucEdit.IsScrap = this.IsScrap;
        
            this.ucEdit.IsReuse = this.IsReuse;
        
        }
    }

    protected void Back_Render(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

   
    protected void TabMstrClick_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = true;
      
    }

   

}
