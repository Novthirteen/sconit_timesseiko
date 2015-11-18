using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;

public partial class Warehouse_InProcessLocation_ViewMain : MainModuleBase
{
    public event EventHandler SaveEvent;
    public event EventHandler BackEvent;
    public event EventHandler CloseEvent;
    public event EventHandler RefreshListEvent;


    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }
    public string IpNo
    {
        get { return (string)ViewState["IpNo"]; }
        set { ViewState["IpNo"] = value; }
    }
    public bool IsPrintASN
    {
        get { return (bool)ViewState["IsPrintASN"]; }
        set { ViewState["IsPrintASN"] = value; }
    }
    private string Action
    {
        get { return (string)ViewState["Action"]; }
        set { ViewState["Action"] = value; }
    }

    public string AsnType
    {
        get { return (string)ViewState["AsnType"]; }
        set { ViewState["AsnType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDetailList.BackEvent += new EventHandler(this.DetailListBack_Render);

        if (!IsPostBack)
        {
            this.ucEdit.AsnType = this.AsnType;
        }
    }

    public void InitPageParameter(string ipNo)
    {
        this.InitPageParameter(ipNo, false);
    }

    public void InitPageParameter(string ipNo, string action)
    {
        InProcessLocation ip = TheInProcessLocationMgr.LoadInProcessLocation(ipNo, true);
        InitPageParameter(ip, false, action);
    }

    public void InitPageParameter(string ipNo, bool printASN)
    {
        InProcessLocation ip = TheInProcessLocationMgr.LoadInProcessLocation(ipNo, true);
        this.InitPageParameter(ip, printASN);
    }

    public void InitPageParameter(InProcessLocation ip)
    {
        this.InitPageParameter(ip, false, "View");
    }

    public void InitPageParameter(InProcessLocation ip, bool printASN)
    {
        InitPageParameter(ip, printASN, "View");
    }

    public void InitPageParameter(InProcessLocation ip, bool printASN, string action)
    {
        this.IpNo = ip.IpNo;
        this.Action = action;
        this.ucEdit.Action = action;
        this.IsPrintASN = printASN;
        this.btnApprove.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
        this.btnReject.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_PENDING;
        this.btnPrint.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
        this.btnExport.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;

        if (ip.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {

            this.btnAdjustLocFrom.Visible = true;
            if (this.AsnType == BusinessConstants.CODE_MASTER_INPROCESS_LOCATION_TYPE_VALUE_GAP)
            {
                this.btnAdjustLocFrom.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
                this.btnAdjustLocTo.Visible = ip.ApprovalStatus == BusinessConstants.CODE_MASTER_APPROVALSTATUS_APPROVED;
                this.btnClose.Visible = false;
            }
            else
            {
                this.btnAdjustLocFrom.Text = "${Common.Button.Cancel}";
                this.btnAdjustLocTo.Visible = false;
                if (ip.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    this.btnAdjustLocFrom.Visible = false;
                    this.btnClose.Visible = true;
                }
                else
                {
                    this.btnAdjustLocFrom.Visible = true;
                    this.btnClose.Visible = false;
                }
            }
        }
        else
        {
            this.btnUpdate.Visible = false;
            this.btnClose.Visible = false;
            this.btnAdjustLocFrom.Visible = false;
            this.btnAdjustLocTo.Visible = false;
        }

        if (this.btnClose.Visible)
        {
            this.ucEdit.Action = "Close";
        }
        this.ucEdit.InitPageParameter(ip.IpNo);
        this.ucDetailList.InitPageParameter(ip.IpNo);
        if (printASN)
        {
            this.PrintASN(ip,false);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (RefreshListEvent != null)
        {
            this.ucEdit.UpdateInProcessLocation();
            this.RefreshListEvent(sender, e);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        this.PrintASN(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        this.PrintASN(true);
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.ucEdit.CloseInProcessLocation();
        this.Visible = false;
        ShowSuccessMessage("InprocessLocation.Close.Successfully");
        if (CloseEvent != null)
        {
            CloseEvent(this, null);
        }
    }

    protected void btnAdjustLocFrom_Click(object sender, EventArgs e)
    {
        try
        {
            this.ucEdit.AdjustInProcessLocationFrom();
            this.Visible = false;
            ShowSuccessMessage("InprocessLocation.AdjustLocFrom.Successfully");
            if (CloseEvent != null)
            {
                CloseEvent(this, null);
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnAdjustLocTo_Click(object sender, EventArgs e)
    {
        try
        {
            string recno = this.ucEdit.AdjustInProcessLocationTo();
            this.Visible = false;
            ShowSuccessMessage("InprocessLocation.AdjustLocTo.Successfully", recno);
            if (CloseEvent != null)
            {
                CloseEvent(this, null);
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    void DetailListBack_Render(object sender, EventArgs e)
    {
        this.Visible = false;
        if (BackEvent != null)
        {
            BackEvent(this, null);
        }
    }

    private void PrintASN(bool isExport)
    {
        InProcessLocation inProcessLocation = TheInProcessLocationMgr.LoadInProcessLocation(IpNo, true);
        this.PrintASN(inProcessLocation, isExport);
    }
    private void PrintASN(InProcessLocation inProcessLocation, bool isExport)
    {
        // inProcessLocation.InProcessLocationDetails = TheInProcessLocationDetailMgr.SummarizeInProcessLocationDetails(inProcessLocation.InProcessLocationDetails);
        if (inProcessLocation.AsnTemplate == null || inProcessLocation.AsnTemplate == string.Empty)
        {
            ShowErrorMessage("ASN.PrintError.NoASNTemplate");
            return;
        }
        IList<object> list = new List<object>();
        list.Add(inProcessLocation);
        list.Add(inProcessLocation.InProcessLocationDetails);
        if (isExport)
        {
            TheReportMgr.WriteToClient(inProcessLocation.AsnTemplate, list, inProcessLocation.AsnTemplate);
        }
        else
        {
            //报表url
            string asnUrl = TheReportMgr.WriteToFile(inProcessLocation.AsnTemplate, list);
            //客户端打印
            //如果在UpdatePanel中调用JavaScript需要使用 ScriptManager.RegisterClientScriptBlock
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + asnUrl + "'); </script>", false);
            Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + asnUrl + "'); </script>");
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            TheInProcessLocationMgr.ApproveInPorcessLocation(this.IpNo, this.CurrentUser);

            ShowSuccessMessage("MasterData.InprocessLocation.Approve.Successfully", this.IpNo);
            this.InitPageParameter(this.IpNo);
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
            TheInProcessLocationMgr.RejectInPorcessLocation(this.IpNo, this.CurrentUser);
            ShowSuccessMessage("MasterData.InprocessLocation.Reject.Successfully", this.IpNo);
            this.InitPageParameter(this.IpNo);
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

}
