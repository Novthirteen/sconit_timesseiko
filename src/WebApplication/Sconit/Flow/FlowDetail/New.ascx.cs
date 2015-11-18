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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using System.Collections.Generic;
using com.Sconit.Utility;

public partial class MasterData_FlowDetail_New : ModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler EditEvent;

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

    public string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    private FlowDetail flowDetail;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            this.FV_FlowDetail.FindControl("trBom").Visible = true;
        }
    }

    public void InitPageParameter(string flowCode)
    {
        this.FlowCode = flowCode;
        PageCleanup();
    }

    protected void ODS_FlowDetail_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        flowDetail = (FlowDetail)e.InputParameters[0];
        Flow flow = TheFlowMgr.LoadFlow(FlowCode, true);

        flowDetail.Flow = flow;

        //seq
        if (flowDetail.Sequence == 0)
        {
            int seqInterval = int.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL).Value);
            flowDetail.Sequence = seqInterval + FlowHelper.GetMaxFlowSeq(flow);
        }
        Controls_TextBox tbItemCode = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbItemCode"));
        Controls_TextBox tbUom = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbUom"));
        if (tbItemCode != null && tbItemCode.Text.Trim() != string.Empty)
        {
            flowDetail.Item = TheItemMgr.LoadItem(tbItemCode.Text.Trim());

            TextBox tbBrand = (TextBox)(this.FV_FlowDetail.FindControl("tbBrand"));
            if (tbBrand.Text.Trim() == string.Empty)
            {
                flowDetail.Brand = flowDetail.Item.Brand;
            }
            TextBox tbManufacturer = (TextBox)(this.FV_FlowDetail.FindControl("tbManufacturer"));
            if (tbManufacturer.Text.Trim() == string.Empty)
            {
                flowDetail.Manufacturer = flowDetail.Item.Manufacturer;
            }
        }

        if (tbUom != null && tbUom.Text.Trim() != string.Empty)
        {
            flowDetail.Uom = TheUomMgr.LoadUom(tbUom.Text.Trim());
        }

        com.Sconit.Control.CodeMstrDropDownList ddlRoundUpOpt = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlRoundUpOpt");
        if (ddlRoundUpOpt.SelectedIndex != -1)
        {
            flowDetail.RoundUpOption = ddlRoundUpOpt.SelectedValue;
        }

        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }
            if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {

                com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");
                if (ddlBarCodeType.SelectedIndex != -1)
                {
                    flowDetail.BarCodeType = ddlBarCodeType.SelectedValue;
                }

            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {

            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            if (ddlOddShipOption.SelectedIndex != -1)
            {

                flowDetail.OddShipOption = ddlOddShipOption.SelectedValue;

            }
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            Controls_TextBox tbBom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbBom");
            if (tbBom != null && tbBom.Text.Trim() != string.Empty)
            {
                flowDetail.Bom = TheBomMgr.LoadBom(tbBom.Text.Trim());
            }

            TextBox tbBatchSize = (TextBox)this.FV_FlowDetail.FindControl("tbBatchSize");
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            if (tbBatchSize.Text.Trim() != string.Empty)
            {
                flowDetail.BatchSize = decimal.Parse(tbBatchSize.Text.Trim());
            }
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }

            com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");
            if (ddlBarCodeType.SelectedIndex != -1)
            {
                flowDetail.BarCodeType = ddlBarCodeType.SelectedValue;
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            if (ddlOddShipOption.SelectedIndex != -1)
            {
                flowDetail.OddShipOption = ddlOddShipOption.SelectedValue;
            }
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }

    protected void ODS_FlowDetail_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

        if (EditEvent != null)
        {
            EditEvent(flowDetail.Id, e);
            decimal unitCount = flowDetail.UnitCount;
            decimal orderLotSize = flowDetail.OrderLotSize == null ? 0 : (decimal)flowDetail.OrderLotSize;
            if (unitCount != 0 && orderLotSize != 0 && orderLotSize % unitCount != 0)
            {
                ShowWarningMessage("MasterData.Flow.FlowDetail.AddFlowDetail.Successfully.UC.Not.Divisible", flowDetail.Sequence.ToString());
            }
            else
            {
                ShowSuccessMessage("MasterData.Flow.FlowDetail.AddFlowDetail.Successfully", flowDetail.Sequence.ToString());
            }
        }

    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        Flow flow = TheFlowMgr.LoadFlow(this.FlowCode, true);
        string itemCode = ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbItemCode"))).Text;
        string uomCode = ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbUom"))).Text;
        decimal unitCount = decimal.Parse(((TextBox)(this.FV_FlowDetail.FindControl("tbUC"))).Text);
        string locFrom = string.Empty;
        string locTo = string.Empty;

        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
            locFrom = string.Empty;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            locTo = string.Empty;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {

            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
        }

        IList<FlowDetail> flowDetailList = flow.FlowDetails;
        if (flowDetailList != null && flowDetailList.Count > 0)
        {
            foreach (FlowDetail flowDetail in flowDetailList)
            {
                string defaultLocFrom = flowDetail.DefaultLocationFrom == null ? string.Empty : flowDetail.DefaultLocationFrom.Code;
                string defaultLocTo = flowDetail.DefaultLocationTo == null ? string.Empty : flowDetail.DefaultLocationTo.Code;
                if (flowDetail.Item.Code == itemCode && flowDetail.Uom.Code == uomCode && defaultLocFrom == locFrom && defaultLocTo == locTo && flowDetail.UnitCount == unitCount)
                {
                    args.IsValid = false;
                    ((CustomValidator)(this.FV_FlowDetail.FindControl("cvItemCheck"))).ErrorMessage = "${MasterData.Flow.FlowDetail.ItemCode.Exists}";
                    break;
                }
            }
        }
        if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty && args.IsValid)
        {
            IList<FlowDetail> refFlowDetailList = TheFlowDetailMgr.GetFlowDetail(flow.ReferenceFlow);
            if (refFlowDetailList != null && refFlowDetailList.Count > 0)
            {
                foreach (FlowDetail flowDetail in refFlowDetailList)
                {
                    string defaultLocFrom = flowDetail.DefaultLocationFrom == null ? string.Empty : flowDetail.DefaultLocationFrom.Code;
                    string defaultLocTo = flowDetail.DefaultLocationTo == null ? string.Empty : flow.LocationTo.Code;
                    if (flowDetail.Item.Code == itemCode && flowDetail.Uom.Code == uomCode && defaultLocFrom == locFrom && defaultLocTo == locTo && flowDetail.UnitCount == unitCount)
                    {
                        args.IsValid = false;
                        ((CustomValidator)(this.FV_FlowDetail.FindControl("cvItemCheck"))).ErrorMessage = "${MasterData.Flow.FlowDetail.ItemCode.Exists}";
                        break;
                    }
                }
            }
        }
    }

    protected void checkSeqExists(object source, ServerValidateEventArgs args)
    {
        String seq = ((TextBox)(this.FV_FlowDetail.FindControl("tbSeq"))).Text.Trim();

        IList<FlowDetail> flowDetailList = TheFlowDetailMgr.GetFlowDetail(this.FlowCode, true);
        if (flowDetailList != null && flowDetailList.Count > 0)
        {
            foreach (FlowDetail flowDetail in flowDetailList)
            {
                if (flowDetail.Sequence == int.Parse(seq))
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }

    private void PageCleanup()
    {
        ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbRefItemCode"))).Text = string.Empty;
        ((Controls_TextBox)this.FV_FlowDetail.FindControl("tbItemCode")).Text = string.Empty;
        ((Controls_TextBox)this.FV_FlowDetail.FindControl("tbUom")).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbUC"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbSeq"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbSafeStock"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbMaxStock"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbMinLotSize"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbOrderLotSize"))).Text = string.Empty;
        ((TextBox)(this.FV_FlowDetail.FindControl("tbOrderGoodsReceiptLotSize"))).Text = string.Empty;

        ((CheckBox)(this.FV_FlowDetail.FindControl("cbIsAutoCreate"))).Checked = true;

        Literal lblOddShipOption = (Literal)this.FV_FlowDetail.FindControl("lblOddShipOption");
        DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
        if (ddlOddShipOption.Visible)
        {
            ddlOddShipOption.SelectedIndex = 0;
        }

        com.Sconit.Control.CodeMstrDropDownList ddlRoundUpOpt = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlRoundUpOpt");
        ddlRoundUpOpt.DataBind();

        Flow flow = TheFlowMgr.LoadFlow(FlowCode);

        Controls_TextBox tbRefItemCode = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbRefItemCode"));
        tbRefItemCode.ServiceParameter = "string:#tbItemCode,string:" + flow.PartyFrom.Code + ",string:" + flow.PartyTo.Code;
        tbRefItemCode.DataBind();
        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            this.FV_FlowDetail.FindControl("fdProcurement").Visible = true;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            tbLocTo.ServiceParameter = "string:" + flow.PartyTo.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = string.Empty;
            if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {

                Literal lblBarCodeType = ((Literal)this.FV_FlowDetail.FindControl("lblBarCodeType"));
                com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");

                lblBarCodeType.Visible = true;
                ddlBarCodeType.Visible = true;
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {

            lblOddShipOption.Visible = true;
            ddlOddShipOption.Visible = true;

            this.FV_FlowDetail.FindControl("fdDistribution").Visible = true;
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            tbLocFrom.ServiceParameter = "string:" + flow.PartyFrom.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = string.Empty;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            this.FV_FlowDetail.FindControl("fdProduction").Visible = true;
            this.FV_FlowDetail.FindControl("trBom").Visible = true;

            ((Controls_TextBox)this.FV_FlowDetail.FindControl("tbBom")).Text = string.Empty;
            ((TextBox)(this.FV_FlowDetail.FindControl("tbBatchSize"))).Text = string.Empty;

            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            tbLocFrom.ServiceParameter = "string:" + flow.PartyFrom.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = string.Empty;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            tbLocTo.ServiceParameter = "string:" + flow.PartyTo.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = string.Empty;

            ((Literal)this.FV_FlowDetail.FindControl("lblNeedInspect")).Visible = true;
            ((CheckBox)(this.FV_FlowDetail.FindControl("cbNeedInspect"))).Visible = true;

            Literal lblBarCodeType = ((Literal)this.FV_FlowDetail.FindControl("lblBarCodeType"));
            com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");


            lblBarCodeType.Visible = true;
            ddlBarCodeType.Visible = true;

            ddlBarCodeType.Code = "FGBarCodeType";
            ddlBarCodeType.DataBind();

        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            lblOddShipOption.Visible = true;
            ddlOddShipOption.Visible = true;
            this.FV_FlowDetail.FindControl("fdTransfer").Visible = true;
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            tbLocFrom.ServiceParameter = "string:" + flow.PartyFrom.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = string.Empty;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            tbLocTo.ServiceParameter = "string:" + flow.PartyTo.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = string.Empty;

        }
    }


}
