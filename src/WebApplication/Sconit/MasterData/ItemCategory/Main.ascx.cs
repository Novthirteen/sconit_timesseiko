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

public partial class MasterData_ItemCategory_Main : MainModuleBase
{

    private string itemCategoryCode = string.Empty;

    protected void GV_ItemCategory_OnDataBind(object sender, EventArgs e)
    {
        this.fldGV.Visible = true;
        if (((GridView)(sender)).Rows.Count == 0)
        {
            this.lblResult.Visible = true;
        }
        else
        {
            this.lblResult.Visible = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.GV_ItemCategory.DataBind();
        this.fldSearch.Visible = true;
        this.fldInsert.Visible = false;
        this.fldGV.Visible = true;
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        this.fldSearch.Visible = false;
        this.fldGV.Visible = false;
        this.fldInsert.Visible = true;
        ((TextBox)(this.FV_ItemCategory.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemCategory.FindControl("tbDesc1"))).Text = string.Empty;
        //((TextBox)(this.FV_ItemCategory.FindControl("tbDesc2"))).Text = string.Empty;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.fldSearch.Visible = true;
        this.fldGV.Visible = true;
        this.fldInsert.Visible = false;
        //this.GV_ItemCategory.DataBind();
    }

    protected void ODS_GV_ItemCategory_OnUpdating(object source, ObjectDataSourceMethodEventArgs e)
    {
        ItemCategory itemCategory = (ItemCategory)e.InputParameters[0];
        itemCategory.Desc1 = itemCategory.Desc1.Trim();
        /*
        if (itemCategory.Desc2 == null)
        {
            itemCategory.Desc2 = string.Empty;
        }
        else
        {
            itemCategory.Desc2 = itemCategory.Desc2.Trim();
        }
         */ 
    }
    protected void ODS_ItemCategory_Inserting(object source, ObjectDataSourceMethodEventArgs e)
    {
        string code = ((TextBox)(this.FV_ItemCategory.FindControl("tbCode"))).Text;
        string desc1 = ((TextBox)(this.FV_ItemCategory.FindControl("tbDesc1"))).Text;
        //string desc2 = ((TextBox)(this.FV_ItemCategory.FindControl("tbDesc2"))).Text;
        if (code == null || code.Trim() == string.Empty) 
        {
            ShowWarningMessage("MasterData.ItemCategory.Code.Empty", "");
            e.Cancel = true;
            return;
        }
        if (desc1 == null || desc1.Trim() == string.Empty)
        {
            ShowWarningMessage("MasterData.ItemCategory.Desc1.Empty", "");
            e.Cancel = true;
            return;
        }
        /*
        if (desc2 == null || desc2.Trim() == string.Empty)
        {
            ShowWarningMessage("MasterData.ItemCategory.Desc2.Empty", "");
            e.Cancel = true;
            return;
        }
         */
        if (TheItemCategoryMgr.LoadItemCategory(code) == null)
        {
            ShowSuccessMessage("MasterData.ItemCategory.AddItemCategory.Successfully", code);
        }
        else 
        {
            e.Cancel = true;
            ShowErrorMessage("MasterData.ItemCategory.CodeExist", code);
            return;
        }
    }

    protected void ODS_ItemCategory_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        btnSearch_Click(this, null);
    }

    protected void ODS_GV_ItemCategory_OnDeleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemCategory itemCategory = (ItemCategory)e.InputParameters[0];
        itemCategoryCode = itemCategory.Code;

        int itemCount = TheItemMgr.GetItemCount(itemCategoryCode);

        if (itemCount > 0)
        {
            ShowErrorMessage("MasterData.ItemCategory.DeleteItemCategory.Fail.Reference.Item", itemCategoryCode);
            e.Cancel = true;
            return;
        }
    }

    protected void ODS_GV_ItemCategory_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            ShowSuccessMessage("MasterData.ItemCategory.DeleteItemCategory.Successfully", itemCategoryCode);
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.ItemCategory.DeleteItemCategory.Fail", itemCategoryCode);
        }
    }

}
