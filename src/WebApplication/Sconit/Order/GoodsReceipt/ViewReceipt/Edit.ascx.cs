using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class Order_GoodsReceipt_ViewReceipt_Edit : EditModuleBase
{
    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    public string ReceiptNo
    {
        get { return (string)ViewState["ReceiptNo"]; }
        set { ViewState["ReceiptNo"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }

    public void InitPageParameter(string receiptNo)
    {
        this.ODS_Receipt.SelectParameters["code"].DefaultValue = receiptNo;
        this.ReceiptNo = receiptNo;
        this.FV_Receipt.DataBind();
    }

    protected void FV_DataBound(object sender, EventArgs e)
    {
        Literal ltlRefOrderNo2 = (Literal)((FormView)(sender)).FindControl("ltlRefOrderNo2");
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            ltlRefOrderNo2.Text = "客户收货确认";
        }
        else
        {
            ltlRefOrderNo2.Text = "供应商发货单号";
        }
    }
}
