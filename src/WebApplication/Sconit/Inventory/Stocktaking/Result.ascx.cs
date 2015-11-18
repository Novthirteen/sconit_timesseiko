using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class Inventory_Stocktaking_Result : ListModuleBase
{
    public string Code
    {
        get { return (string)ViewState["Code"]; }
        set { ViewState["Code"] = value; }
    }

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Code != null && Code.Length > 0)
        {
            UpdateView();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        BoundGridView();
        Export();
    }

    public void Export()
    {
        this.isExport = true;
        this.ExportXLS(GV_List);
    }

    public override void UpdateView()
    {
        CycleCount cycleCount = TheCycleCountMgr.LoadCycleCount(Code);
        if (cycleCount.IsScanHu)
        {
            tbBinCode.ServiceParameter = "string:" + cycleCount.Location.Code;
            tbBinCode.DataBind();
            tbBinCode.Visible = true;
            lblBinCode.Visible = true;
            this.GV_List.Columns[7].Visible = true;
            this.GV_List.Columns[8].Visible = true;
            this.GV_List.Columns[9].Visible = true;
            this.GV_List.Columns[10].Visible = false;
            this.GV_List.Columns[11].Visible = false;
            this.GV_List.Columns[12].Visible = false;
        }
        else
        {
            this.GV_List.Columns[1].Visible = false;
            this.GV_List.Columns[7].Visible = false;
            this.GV_List.Columns[8].Visible = false;
            this.GV_List.Columns[9].Visible = false;
            this.GV_List.Columns[10].Visible = true;
            this.GV_List.Columns[11].Visible = true;
            this.GV_List.Columns[12].Visible = true;
            tbBinCode.Visible = false;
            lblBinCode.Visible = false;
        }

        if (cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
        {
            this.btnAdjust.Visible = true;
            this.btnClose.Visible = true;
        }
        else
        {
            this.btnAdjust.Visible = false;
            this.btnClose.Visible = false;
        }
        this.isExport = false;
    }

    protected void lbtnShortageQty_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, true, false, false);
    }
    protected void lbtnProfitQty_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, false, true, false);
    }
    protected void lbtnEqualQty_Click(object sender, EventArgs e)
    {
        OnDetailClick(((LinkButton)sender).CommandArgument, false, false, true);
    }

    private void OnDetailClick(string binItem, bool isShortage, bool isProfit, bool isEqual)
    {
        string[] argument = binItem.Split('|');

        IList<CycleCountResult> cycleCountResultList = TheCycleCountMgr.ListCycleCountResultDetail(this.Code, isShortage, isProfit, isEqual, argument[0], argument[1]);

        this.ucDetail.UpdateView(cycleCountResultList, isShortage, isProfit, isEqual);
        this.ucDetail.Visible = true;
        //this.ucDetail.UpdateView();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            TheCycleCountMgr.ManualCloseCycleCount(Code, this.CurrentUser);
            UpdateView();
            ShowSuccessMessage("Common.Business.Result.Close.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnAdjust_Click(object sender, EventArgs e)
    {
        try
        {
            TheCycleCountMgr.ProcessCycleCountResult(Code, this.CurrentUser);
            UpdateView();
            ShowSuccessMessage("Common.Business.Result.Adjust.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BoundGridView();
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    public void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.isExport && e.Row.RowType == DataControlRowType.DataRow)
        {
            CycleCountResult ccr = (CycleCountResult)e.Row.DataItem;

            e.Row.Cells[0].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[2].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[6].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[7].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[8].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[9].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[10].Attributes.Add("style", "vnd.ms-excel.numberformat:@");

            this.SetLinkButton(e.Row, "lblShortageQty", ccr.ShortageQty != null && ccr.ShortageQty != 0);
            this.SetLinkButton(e.Row, "lblProfitQty", ccr.ProfitQty != null && ccr.ProfitQty != 0);
            this.SetLinkButton(e.Row, "lblEqualQty", ccr.EqualQty != null && ccr.EqualQty != 0);
        }
    }

    private void BoundGridView()
    {
        IList<string> binCodeList = new List<string>();
        if (this.tbBinCode.Text != null && this.tbBinCode.Text.Trim().Length > 0)
        {
            binCodeList.Add(this.tbBinCode.Text);
        }
        IList<string> itemCodeList = new List<string>();
        if (this.tbItemCode.Text != null && this.tbItemCode.Text.Trim().Length > 0)
        {
            itemCodeList.Add(this.tbItemCode.Text);
        }
        IList<CycleCountResult> cycleCountResultList = TheCycleCountMgr.ListCycleCountResult(Code, cbShortage.Checked, this.cbProfit.Checked, cbEqual.Checked, binCodeList, itemCodeList);

        this.GV_List.DataSource = cycleCountResultList;
        this.GV_List.DataBind();
    }

    public void clean()
    {
        this.btnAdjust.Visible = false;
        this.btnClose.Visible = false;
        this.tbBinCode.Text = string.Empty;
        this.tbItemCode.Text = string.Empty;
        this.cbEqual.Checked = true;
        this.cbProfit.Checked = true;
        this.cbShortage.Checked = true;
        this.GV_List.DataSource = null;
        this.GV_List.DataBind();
    }

}
