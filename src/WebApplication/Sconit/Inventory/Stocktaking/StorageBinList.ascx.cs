using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;

public partial class Inventory_Stocktaking_StorageBinList : ModuleBase
{

    public string Location
    {
        get { return (string)ViewState["Location"]; }
        set { ViewState["Location"] = value; }
    }
    protected IList<StorageBin> StorageBinList
    {
        get
        {
            return (IList<StorageBin>)ViewState["StorageBinList"];
        }
        set
        {
            ViewState["StorageBinList"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            StorageBin storageBin = (StorageBin)e.Row.DataItem;
            Label lblBinCode = (Label)(e.Row.FindControl("lblBinCode"));
            Controls_TextBox tbBinCode = (Controls_TextBox)(e.Row.FindControl("tbBinCode"));
            if (storageBin.IsBlank)
            {
                lblBinCode.Visible = false;
                tbBinCode.Visible = true;
                tbBinCode.ServiceParameter = "string:" + this.Location;
                tbBinCode.DataBind();
            }
            e.Row.FindControl("lbtnAdd").Visible = storageBin.IsBlank;
            e.Row.FindControl("lbtnDelete").Visible = !storageBin.IsBlank;
        }
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)(((LinkButton)sender).Parent.Parent);

        string code = ((Controls_TextBox)row.FindControl("tbBinCode")).Text.Trim();
        if (code == string.Empty)
        {
            ShowErrorMessage("MasterData.Location.Bin.Required.Code");
            return;
        }
        var count = StorageBinList.Where(i => i.Code == code).Count();
        if (count > 0)
        {
            ShowErrorMessage("Common.Business.Error.EntityExist", code);
            return;
        }

        StorageBin bin = TheStorageBinMgr.LoadStorageBin(code);
        this.StorageBinList.Add(bin);
        BindData(true);
    }
    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        foreach (StorageBin bin in this.StorageBinList)
        {
            if (bin.Code == code)
            {
                this.StorageBinList.Remove(bin);
                break;
            }
        }

        BindData(true);
    }

    public void InitPageParameter(CycleCount cycleCount)
    {
        this.Location = cycleCount.Location.Code;

        this.StorageBinList = new List<StorageBin>();
    
        if (cycleCount.Bins != null && cycleCount.Bins != string.Empty)
        {
            string[] binArr = cycleCount.Bins.Split('|');
            foreach (string bin in binArr)
            {
                StorageBin storageBin = TheStorageBinMgr.LoadStorageBin(bin);
                storageBin.IsBlank = false;
                this.StorageBinList.Add(storageBin);
             
            }
        }
        this.GV_List.Columns[3].Visible = cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
        BindData(cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE);
    }

    private void BindData(bool includeBlank)
    {
        IList<StorageBin> binList = new List<StorageBin>();
        IListHelper.AddRange<StorageBin>(binList, this.StorageBinList);
        if (includeBlank)
        {
            StorageBin newBin = new StorageBin();
            newBin.IsBlank = true;
            binList.Add(newBin);
        }
        this.GV_List.DataSource = binList;
        this.GV_List.DataBind();
    }

    public string GetBins()
    {
        string bins = string.Empty;
        foreach (StorageBin bin in this.StorageBinList)
        {
            if (bins == string.Empty)
            {
                bins += bin.Code;
            }
            else
            {
                bins += "|" + bin.Code;
            }

        }
        return bins;
    }
}
