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
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Utility;
using Geekees.Common.Controls;

public partial class Order_OrderHead_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler BtnImportClick;

    private IDictionary<string, string> parameter = new Dictionary<string, string>();
    private List<string> statusList = new List<string>();
    private List<string> typeList = new List<string>();

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    public string ModuleSubType
    {
        get { return (string)ViewState["ModuleSubType"]; }
        set { ViewState["ModuleSubType"] = value; }
    }

    public int StatusGroupId
    {
        get { return (int)ViewState["StatusGroupId"]; }
        set { ViewState["StatusGroupId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbPartyFrom.ServiceParameter = "string:" + this.ModuleType + ",string:" + this.CurrentUser.Code;
        this.tbPartyTo.ServiceParameter = "string:" + this.ModuleType + ",string:" + this.CurrentUser.Code;
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:true,bool:true,bool:false,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;

        if (!IsPostBack)
        {
            if (this.StatusGroupId == 7)
            {
                this.btnNew.Visible = false;
                this.btnImport.Visible = false;
            }
            IList<CodeMaster> statusList = GetStatusGroup(this.StatusGroupId);

            IList<CodeMaster> orderSubTypeList = GetorderSubTypeGroup(this.ModuleType);
            orderSubTypeList.Insert(0, new CodeMaster()); //添加空选项
            this.ddlSubType.DataSource = orderSubTypeList;
            this.ddlSubType.DataBind();

            GenerateTree();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            NewEvent(sender, e);
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (BtnImportClick != null)
        {
            BtnImportClick(sender, e);
        }
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (SearchEvent != null)
        {
            if (btn == this.btnExport)
            {
                FillParameter();
                if (this.rblListFormat.SelectedValue == "Detail")
                {
                    object[] criteriaParam = CriteriaHelper.CollectDetailParam(this.parameter, statusList, typeList, false);
                    Array.Resize<object>(ref criteriaParam, 5);
                    criteriaParam[4] = true;
                    SearchEvent(criteriaParam, null);
                }
                else
                {
                    object[] criteriaParam = CriteriaHelper.CollectMasterParam(this.parameter, statusList, typeList, false);
                    Array.Resize<object>(ref criteriaParam, 5);
                    criteriaParam[4] = true;
                    SearchEvent(criteriaParam, null);
                }
            }
            else
            {
                DoSearch();
            }
        }
    }

    protected override void DoSearch()
    {
        FillParameter();
        if (this.rblListFormat.SelectedValue == "Detail")
        {
            object[] criteriaParam = CriteriaHelper.CollectDetailParam(this.parameter, statusList, typeList, false);
            Array.Resize<object>(ref criteriaParam, 5);
            criteriaParam[4] = false;
            SearchEvent(criteriaParam, null);
        }
        else
        {
            object[] criteriaParam = CriteriaHelper.CollectMasterParam(this.parameter, statusList, typeList, false);
            Array.Resize<object>(ref criteriaParam, 5);
            criteriaParam[4] = false;
            SearchEvent(criteriaParam, null);
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        this.parameter = actionParameter;

        if (actionParameter.ContainsKey("OrderNo"))
        {
            this.tbOrderNo.Text = actionParameter["OrderNo"];
        }
        if (actionParameter.ContainsKey("Priority"))
        {
            this.ddlPriority.SelectedValue = actionParameter["Priority"];
        }
        if (actionParameter.ContainsKey("PartyFrom"))
        {
            this.tbPartyFrom.Text = actionParameter["PartyFrom"];
        }
        if (actionParameter.ContainsKey("PartyTo"))
        {
            this.tbPartyTo.Text = actionParameter["PartyTo"];
        }
        if (actionParameter.ContainsKey("LocFrom"))
        {
            this.tbLocFrom.Text = actionParameter["LocFrom"];
        }
        if (actionParameter.ContainsKey("LocTo"))
        {
            this.tbLocTo.Text = actionParameter["LocTo"];
        }
    }

    private void FillParameter()
    {
        typeList.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION);
        typeList.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER);
        typeList.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING);

        #region status
        List<ASTreeViewNode> nodes = this.astvMyTree.GetCheckedNodes();
        foreach (ASTreeViewNode node in nodes)
        {
            statusList.Add(node.NodeValue);
        }
        if (statusList.Count > 0)
        {
        }
        else if (this.parameter.ContainsKey("Status"))
        {
            statusList.Add(this.parameter["Status"]);
        }
        else
        {
            if (this.StatusGroupId == 7)
            {
                statusList.Add(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);
                statusList.Add(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT);
                statusList.Add(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS);
            }
            else
            {
                #region 根据StatusGroupId限制查询订单的状态
                foreach (CodeMaster status in GetStatusGroup(this.StatusGroupId))
                {
                    statusList.Add(status.Value);
                }
                #endregion
            }
        }
        #endregion

        this.ModuleSubType = this.ddlSubType.SelectedValue;

        this.parameter.Clear();
        this.parameter.Add("OrderNo", this.tbOrderNo.Text.Trim());
        this.parameter.Add("Flow", this.tbFlow.Text.Trim());
        this.parameter.Add("PartyFrom", this.tbPartyFrom.Text.Trim());
        this.parameter.Add("PartyTo", this.tbPartyTo.Text.Trim());
        this.parameter.Add("ModuleType", this.ModuleType);
        this.parameter.Add("LocationFrom", this.tbLocFrom.Text.Trim());
        this.parameter.Add("LocationTo", this.tbLocTo.Text.Trim());
        this.parameter.Add("ModuleSubType", this.ModuleSubType);
        this.parameter.Add("Priority", this.ddlPriority.SelectedValue);
        this.parameter.Add("CreateUser", this.tbCreateUser.Text.Trim());
        this.parameter.Add("StartDate", this.tbStartDate.Text.Trim());
        this.parameter.Add("EndDate", this.tbEndDate.Text.Trim());
        this.parameter.Add("CurrentUser", this.CurrentUser.Code);
        this.parameter.Add("ApprovalStatus", this.ddlApprovalStatus.SelectedValue);
        this.parameter.Add("BillFrom", this.tbBillFrom.Text.Trim());
    }

    private IList<CodeMaster> GetStatusGroup(int statusGroupId)
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();
        switch (statusGroupId)
        {
            case 1:   //新建
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                break;
            case 2:   //发货
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                break;
            case 3:   //收货
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                break;
            case 4:   //All
            case 7:   //首页/订单跟踪
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE));
                //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID));
                break;
            case 5:   //生产上线/取消
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
                break;
            default:
                break;
        }

        return statusGroup;
    }

    private IList<CodeMaster> GetorderSubTypeGroup(string moduleType)
    {
        IList<CodeMaster> orderSubTypeGroup = new List<CodeMaster>();
        if (moduleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
            || moduleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            orderSubTypeGroup.Add(GetorderSubType(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
            orderSubTypeGroup.Add(GetorderSubType(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN));
            orderSubTypeGroup.Add(GetorderSubType(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ));
        }
        else if (moduleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            orderSubTypeGroup.Add(GetorderSubType(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML));
            orderSubTypeGroup.Add(GetorderSubType(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO));
        }
        else
        {

        }

        return orderSubTypeGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }

    private CodeMaster GetorderSubType(string type)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE, type);
    }

    private void GenerateTree()
    {
        IList<CodeMaster> statusList = GetStatusGroup(this.StatusGroupId);
        foreach (CodeMaster status in statusList)
        {
            this.astvMyTree.RootNode.AppendChild(new ASTreeViewLinkNode(status.Description, status.Value, string.Empty));
        }
        if (this.StatusGroupId == 7 || this.StatusGroupId == 4)
        {
            this.astvMyTree.RootNode.ChildNodes[0].CheckedState = ASTreeViewCheckboxState.Checked;
            this.astvMyTree.RootNode.ChildNodes[1].CheckedState = ASTreeViewCheckboxState.Checked;
            this.astvMyTree.RootNode.ChildNodes[2].CheckedState = ASTreeViewCheckboxState.Checked;
            this.astvMyTree.InitialDropdownText = this.astvMyTree.RootNode.ChildNodes[0].NodeText + "," + this.astvMyTree.RootNode.ChildNodes[1].NodeText + "," + this.astvMyTree.RootNode.ChildNodes[2].NodeText;
            this.astvMyTree.DropdownText = this.astvMyTree.RootNode.ChildNodes[0].NodeText + "," + this.astvMyTree.RootNode.ChildNodes[1].NodeText + "," + this.astvMyTree.RootNode.ChildNodes[2].NodeText;
        }
    }

  
}
