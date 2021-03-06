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
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;

public partial class Order_OrderView_LocTransMain : MainModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler UpdateRoutingEvent;

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

        #region OrderLocationInTransaction
        this.ucLocInTransList.OrderNo = orderNo;
        this.ucLocInTransList.Visible = true;
        this.ucLocInTransList.IOType = BusinessConstants.IO_TYPE_IN;
        this.ucLocInTransList.UpdateView();
        #endregion

        #region OrderLocationOutTransaction
        this.ucLocOutTransList.OrderNo = orderNo;
        this.ucLocOutTransList.Visible = true;
        this.ucLocOutTransList.IOType = BusinessConstants.IO_TYPE_OUT;
        this.ucLocOutTransList.UpdateView();
        #endregion

        #region 保存按钮
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            OrderHead orderhead = TheOrderHeadMgr.LoadOrderHead(orderNo);
            if (orderhead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.btnSave.Visible = true;
            }
            else
            {
                this.btnSave.Visible = false;
            }
        }
        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucLocOutTransList.EditEvent += new System.EventHandler(this.Edit_Render);
        this.ucLocOutTransList.UpdateRoutingEvent += new System.EventHandler(this.UpdateRouting_Render);
        this.ucAbstractItemBomDetail.SaveEvent += new System.EventHandler(this.AbstractItemSave_Render);
        if (!IsPostBack)
        {
            this.ucLocInTransList.ModuleType = this.ModuleType;
            this.ucLocInTransList.ModuleSubType = this.ModuleSubType;

            this.ucLocOutTransList.ModuleType = this.ModuleType;
            this.ucLocOutTransList.ModuleSubType = this.ModuleSubType;

            if (this.ModuleType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                this.IsScrap = false;
                this.IsReuse = false;
            }

            this.ucLocInTransList.IsScrap = this.IsScrap;
            this.ucLocOutTransList.IsScrap = this.IsScrap;
            this.ucLocInTransList.IsReuse = this.IsReuse;
            this.ucLocOutTransList.IsReuse = this.IsReuse;
       
        }
      

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.ucLocOutTransList.SaveAllDetail();
    }

    protected void UpdateRouting_Render(object sender, EventArgs e)
    {
        UpdateRoutingEvent(sender, e);
       
    }

    protected void Edit_Render(object sender, EventArgs e)
    {
        this.ucAbstractItemBomDetail.Visible = true;
        this.ucAbstractItemBomDetail.LocTransId = (int)((object[])sender)[1];
        this.ucAbstractItemBomDetail.InitPageParameter((string)((object[])sender)[0]);
    }

    protected void AbstractItemSave_Render(object sender, EventArgs e)
    {
        InitPageParameter((string)sender);
    }

    public IList<Hu> GetLocTransHuList()
    {
        return this.ucLocOutTransList.huList;
    }
}
