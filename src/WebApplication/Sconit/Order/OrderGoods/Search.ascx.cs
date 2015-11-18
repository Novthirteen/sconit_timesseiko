using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;

public partial class Order_OrderGoods_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.tbRegion.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION + ",string:" + this.CurrentUser.Code;
        //this.tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT + ",string:" + this.CurrentUser.Code;
        this.tbPartyTo.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION + ",string:" + this.CurrentUser.Code;
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:false,bool:false,bool:false,bool:false,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;
    }

    protected override void InitPageParameter(IDictionary<string, string> parameters)
    {
    }

    protected override void DoSearch()
    {
        string flowCode = this.tbFlow.Text.Trim();
        string customerCode = this.tbPartyTo.Text.Trim();
        string orderNo = this.tbOrderNo.Text.Trim();
        DateTime? startDate = null;
        if (this.tbStartDate.Text.Trim() != string.Empty)
        {
            startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
        }
        DateTime? endDate = null;
        if (this.tbEndDate.Text.Trim() != string.Empty)
        {
            endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
        }
        List<LeanEngine.Entity.Orders> orders = this.TheLeanEngineMgr.GenerateLeanEngineOrder(flowCode, customerCode, orderNo, startDate, endDate);
        this.SearchEvent(orders, null);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }
}
