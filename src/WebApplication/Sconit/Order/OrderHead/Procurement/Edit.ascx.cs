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
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Distribution;
using System.IO;
using System.Collections.Generic;

public partial class Order_OrderHead_Edit : EditModuleBase
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

    private string OrderNo
    {
        get
        {
            return (string)ViewState["OrderNo"];
        }
        set
        {
            ViewState["OrderNo"] = value;
        }
    }

    //不合格品退货
    public bool IsReject
    {
        get
        {
            return (bool)ViewState["IsReject"];
        }
        set
        {
            ViewState["IsReject"] = value;
        }
    }


    public void InitPageParameter(string orderNo)
    {
        this.OrderNo = orderNo;
        this.ODS_Order.SelectParameters["orderNo"].DefaultValue = this.OrderNo;
        this.UpdateView();
        OrderHead oH = TheOrderHeadMgr.LoadOrderHead(orderNo, true);

        #region 根据ModuleSubType状态调整显示的文字
        if (this.ModuleSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
        {
            this.btnReceive.Text = "${Common.Button.Return}";
        }

        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDetail.SaveEvent += new System.EventHandler(this.SaveRender);

        this.ucDetail.ReceiveEvent += new System.EventHandler(this.ReceiveRender);

        if (!IsPostBack)
        {
            this.ucDetail.ModuleType = this.ModuleType;
            this.ucDetail.ModuleSubType = this.ModuleSubType;
            this.cbShowDiscount.Checked = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_IS_SHOW_DISCOUNT).Value);

        }

        this.ucDetail.IsReject = this.IsReject;


    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
        if (tbWinTime.Text.Trim() == string.Empty)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.WinTime.Required");
            return;
        }

        DateTime winTime = Convert.ToDateTime(tbWinTime.Text.Trim());

        this.ucDetail.SaveCallBack();
    }

    protected void btnRecalculate_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheOrderMgr.RecalculatePrice(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Recalculate.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            CodeMstrLabel cmlStatus = this.FV_Order.FindControl("cmlStatus") as CodeMstrLabel;
            if (cmlStatus.Value == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.ucDetail.SaveCallBack();
            }
            this.TheOrderMgr.ReleaseOrder(this.OrderNo, this.CurrentUser, true);
            this.FV_Order.DataBind();
            UpdateView();
            //this.ShowSuccessMessage("订单" + this.OrderNo + "提交成功！");
            ShowSuccessMessage("MasterData.Order.OrderHead.Submit.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheOrderMgr.StartOrder(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Start.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    protected void btnReceive_Click(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
        if (orderHead.IsReceiptScanHu)
        {
            this.Session["Temp_Session_OrderNo"] = this.OrderNo;
            Response.Redirect("~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Procurement");
        }
        else
        {
            this.ucDetail.ReceiveCallBack();

        }

    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.ManualCompleteOrder(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Complete.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.DeleteOrder(this.OrderNo, this.CurrentUser);

            if (this.BackEvent != null)
            {
                this.BackEvent(this, e);
                this.PageCleanup();
            }
            ShowSuccessMessage("MasterData.Order.OrderHead.Delete.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
            this.PageCleanup();
        }
    }

    private void SaveRender(object sender, EventArgs e)
    {
        OrderHead orderHead = (OrderHead)sender;

        TextBox tbExtOrderNo = (TextBox)this.FV_Order.FindControl("tbExtOrderNo");
        TextBox tbRefOrderNo = (TextBox)this.FV_Order.FindControl("tbRefOrderNo");
        TextBox tbConfirmOrderNo = (TextBox)this.FV_Order.FindControl("tbConfirmOrderNo");
        TextBox tbSettlement = (TextBox)this.FV_Order.FindControl("tbSettlement");
        TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
        TextBox tbCustomer = (TextBox)this.FV_Order.FindControl("tbCustomer");
        TextBox tbRelatedOrderNo = (TextBox)this.FV_Order.FindControl("tbRelatedOrderNo");
        if (orderHead.ApprovalStatus != BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED)
        {
            if (tbExtOrderNo != null && tbExtOrderNo.Text.Trim() != string.Empty)
            {
                orderHead.ExternalOrderNo = tbExtOrderNo.Text.Trim();
            }
            if (tbRefOrderNo != null && tbRefOrderNo.Text.Trim() != string.Empty)
            {
                orderHead.ReferenceOrderNo = tbRefOrderNo.Text.Trim();
            }
            if (tbSettlement != null && tbSettlement.Text.Trim() != string.Empty)
            {
                orderHead.Settlement = tbSettlement.Text.Trim();
            }
            if (tbCustomer != null && tbCustomer.Text.Trim() != string.Empty)
            {
                orderHead.Customer = tbCustomer.Text.Trim();
            }
            if (tbRelatedOrderNo != null && tbRelatedOrderNo.Text.Trim() != string.Empty)
            {
                orderHead.RelatedOrderNo = tbRelatedOrderNo.Text.Trim();
            }
        }
        if (tbConfirmOrderNo != null && tbConfirmOrderNo.Text.Trim() != string.Empty)
        {
            orderHead.ConfirmOrderNo = tbConfirmOrderNo.Text.Trim();
        }

        if (tbWinTime != null && tbWinTime.Text.Trim() != string.Empty)
        {
            orderHead.WindowTime = DateTime.Parse(tbWinTime.Text.Trim());
        }
        try
        {
            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                orderHead.ApprovalStatus = BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
                TheOrderMgr.UpdateOrder(orderHead, this.CurrentUser, true);
            }
            else
            {
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = this.CurrentUser;
                TheOrderHeadMgr.UpdateOrderHead(orderHead);
            }
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Update.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    private void ReceiveRender(object sender, EventArgs e)
    {
        OrderHead orderHead = this.ucDetail.PopulateReceiveOrder();
        try
        {
            Receipt receipt = TheOrderMgr.ReceiveOrder(orderHead.OrderDetails, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Receive.Successfully", this.OrderNo);


        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void FV_Order_DataBound(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
        #region 控件可以输入
        TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
        TextBox tbExtOrderNo = (TextBox)this.FV_Order.FindControl("tbExtOrderNo");
        TextBox tbRefOrderNo = (TextBox)this.FV_Order.FindControl("tbRefOrderNo");
        TextBox tbRelatedOrderNo = (TextBox)this.FV_Order.FindControl("tbRelatedOrderNo");
        TextBox tbCustomer = (TextBox)this.FV_Order.FindControl("tbCustomer");
        TextBox tbSettlement = (TextBox)this.FV_Order.FindControl("tbSettlement");
        string userLanguage = this.CurrentUser.UserLanguage;
        tbWinTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd',lang:'" + this.CurrentUser.UserLanguage + "'})");
        this.cbShowDiscount.Visible = (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);

        if (orderHead.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED)
        {
            tbExtOrderNo.Attributes.Add("onfocus", "this.blur();");
            tbSettlement.Attributes.Add("onfocus", "this.blur();");
            tbRefOrderNo.Attributes.Add("onfocus", "this.blur();");
            tbRelatedOrderNo.Attributes.Add("onfocus", "this.blur();");
            tbCustomer.Attributes.Add("onfocus", "this.blur();");
        }

        #endregion
    }

    private void UpdateView()
    {
        if (this.OrderNo != null)
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
            string orderStatus = orderHead.Status;



            #region 根据订单状态显示按钮
            if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.btnEdit.Visible = true;
                this.btnRecalculate.Visible = true;
                //this.btnSubmit.Visible = true;
                this.btnApprove.Visible = orderHead.ApprovalStatus== BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
                this.btnReject.Visible = orderHead.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = true;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = false;
                this.btnExport.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                this.btnEdit.Visible = true;
                this.btnRecalculate.Visible = true;
                // this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = true;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
            {
                this.btnEdit.Visible = false;
                this.btnRecalculate.Visible = false;
                // this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                this.btnEdit.Visible = true;
                //计算是否有收过货
                orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo, true);
                IList<OrderDetail> orderDetails = orderHead.OrderDetails;
                decimal? receivedQty = orderDetails.Where(od => od.ReceivedQty.HasValue).Sum(od => od.ReceivedQty);
                if (receivedQty.HasValue && receivedQty.Value > 0)
                {
                    this.btnRecalculate.Visible = false;
                }
                else
                {
                    this.btnRecalculate.Visible = true;
                }
                //  this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = true;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = true;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }

            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
            {
                this.btnEdit.Visible = false;
                this.btnRecalculate.Visible = false;
                // this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                this.btnEdit.Visible = false;
                this.btnRecalculate.Visible = false;
                // this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID)
            {
                this.btnEdit.Visible = false;
                this.btnRecalculate.Visible = false;
                //  this.btnSubmit.Visible = false;
                this.btnApprove.Visible = false;
                this.btnReject.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
                this.BtnVoid.Visible = false;
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            #endregion

            this.FV_Order.DataBind();
            this.ucDetail.InitPageParameter(this.OrderNo);
        }
    }

    private void PageCleanup()
    {
        this.OrderNo = null;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
        string orderTemplate = orderHead.OrderTemplate;
        if (orderTemplate == null || orderTemplate.Length == 0)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
        }
        else
        {
            string printUrl = TheReportMgr.WriteToFile(orderTemplate, this.OrderNo);
            Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
        string orderTemplate = orderHead.OrderTemplate;
        if (orderTemplate == null || orderTemplate.Length == 0)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
        }
        else
        {
            TheReportMgr.WriteToClient(orderTemplate, this.OrderNo, "order.xls");
        }

    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.VoidOrder(this.OrderNo, this.CurrentUser);
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Void.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.ApproveOrder(this.OrderNo, CurrentUser);
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Approve.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.RejectOrder(this.OrderNo, this.CurrentUser);
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Reject.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void cbShowDiscount_CheckChanged(Object sender, EventArgs e)
    {
        this.ucDetail.IsShowDiscount = this.cbShowDiscount.Checked;
        this.ucDetail.InitPageParameter();
    }
}
