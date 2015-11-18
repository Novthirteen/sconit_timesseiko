using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;

public partial class Order_ReceiptNotes_List : ListModuleBase
{
    public EventHandler ViewEvent;
    public EventHandler AdjustEvent;

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

    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }
    public bool IsExport
    {
        get { return ViewState["IsExport"] != null ? (bool)ViewState["IsExport"] : false; }
        set { ViewState["IsExport"] = value; }
    }
    public bool IsGroup
    {
        get { return ViewState["IsGroup"] != null ? (bool)ViewState["IsGroup"] : false; }
        set { ViewState["IsGroup"] = value; }
    }

    public override void UpdateView()
    {
        if (!IsExport)
        {
            if (IsGroup)
            {
                this.GV_List.Execute();
                this.GV_List.Visible = true;
                this.gp.Visible = true;
                this.GV_List_Detail.Visible = false;
                this.gp_Detail.Visible = false;
            }
            else
            {
                for (int i = 0; i < this.GV_List.Columns.Count; i++)
                {
                    this.GV_List.Columns[i].Visible = true;
                }
                this.GV_List_Detail.Execute();
                this.GV_List.Visible = false;
                this.GV_List_Detail.Visible = true;
                this.gp.Visible = false;
                this.gp_Detail.Visible = true;
                //int[] alwaysShow = new int[] {12};
                //com.Sconit.Utility.GridViewHelper.HiddenColumns(this.GV_List_Detail, alwaysShow);
            }
        }
        else
        {
            string dateTime = DateTime.Now.ToString("ddhhmmss");

            if (IsGroup)
            {
                if (GV_List.Rows.Count > 0)
                {
                    GV_List.Columns.RemoveAt(GV_List.Columns.Count - 1);
                }
                this.ExportXLS(GV_List, "RceiptGroup" + dateTime + ".xls");
            }
            else
            {
                for (int i = 0; i < this.GV_List.Columns.Count; i++)
                {
                    this.GV_List.Columns[i].Visible = true;
                }
                this.ExportXLS(GV_List_Detail, "RceiptGroup" + dateTime + ".xls");
                //int[] alwaysShow = new int[] { 12 };
                //com.Sconit.Utility.GridViewHelper.HiddenColumns(this.GV_List_Detail, alwaysShow);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitialUI();
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        if (ViewEvent != null)
        {
            string receiptNo = ((LinkButton)sender).CommandArgument;
            ViewEvent(receiptNo, e);
        }
    }

    protected void lbtnAdjust_Click(object sender, EventArgs e)
    {
        if (AdjustEvent != null)
        {
            string receiptNo = ((LinkButton)sender).CommandArgument;
            AdjustEvent(receiptNo, e);
        }
    }

    public void Export()
    {
        if (GV_List.FindPager().RecordCount > 0)
        {
            GV_List.Columns.RemoveAt(GV_List.Columns.Count - 1);
        }
        this.ExportXLS(GV_List, "ReceiptNotes.xls");

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.ModuleSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.FindControl("lbtnAdjust").Visible = true;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Receipt receipt = (Receipt)e.Row.DataItem;

            //采购单 销售单号 
            //string hql = @"select Distinct oh.OrderNo,ot.Code from ReceiptDetail rd inner join rd.Receipt rt inner join rd.OrderLocationTransaction olt inner join olt.OrderDetail od inner join od.OrderHead oh,OrderTracer ot  where ot.RefOrderLocTransId = olt.Id and  rt.ReceiptNo = ? ";

            //采购单
            string hql = @"select Distinct oh.OrderNo from ReceiptDetail rd inner join rd.Receipt rt inner join rd.OrderLocationTransaction olt inner join olt.OrderDetail od inner join od.OrderHead oh  where  rt.ReceiptNo = ? ";

            IList<string> procurementNoList = this.TheCriteriaMgr.FindAllWithHql<string>(hql, new object[] { receipt.ReceiptNo });
            if (procurementNoList != null)
            {
                string procurementNos = "";
                for (int i = 0; i < procurementNoList.Count; i++)
                {
                    if (i != 0)
                    {
                        procurementNos += ",";
                    }
                    procurementNos += procurementNoList[i];


                    //销售单
                    hql = @"select Distinct ot.Code from OrderTracer ot ,OrderLocationTransaction olt inner join olt.OrderDetail od inner join od.OrderHead oh where ot.RefOrderLocTransId = olt.Id and  oh.OrderNo = ? ";

                    IList<string> distributionNoList = this.TheCriteriaMgr.FindAllWithHql<string>(hql, new object[] { procurementNoList[i] });


                    if (distributionNoList != null)
                    {
                        string distributionNos = "";
                        for (int j = 0; j < distributionNoList.Count; j++)
                        {

                            if (j != 0)
                            {
                                distributionNos += ",";
                            }

                            distributionNos += distributionNoList[j];
                        }
                        if (distributionNos.Length > 0)
                        {
                            Label lblDistributionNos = (Label)e.Row.FindControl("lblDistributionNos");
                            lblDistributionNos.Text = distributionNos;
                        }
                    }
                }
                if (procurementNos.Length > 0)
                {
                    Label lblProcurementNos = (Label)e.Row.FindControl("lblProcurementNos");
                    lblProcurementNos.Text = procurementNos;
                }
            }



        }
    }

    private void InitialUI()
    {
        if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            GV_List.Columns[4].Visible = false;//ReferenceIpNo
            GV_List.Columns[5].Visible = false;//ExternalReceiptNo
            GV_List.Columns[8].HeaderText = "${Common.Business.Region}";//PartyTo
            GV_List.Columns[11].HeaderText = "${Receipt.CreaterUser.Procurement}";//CreateUser
            GV_List.Columns[10].HeaderText = "${Receipt.CreaterDate.Procurement}";//CreateDate

            GV_List_Detail.Columns[2].Visible = false; //asn号
            GV_List_Detail.Columns[10].Visible = false; //来源库位
            GV_List_Detail.Columns[12].Visible = false; //发货数
            GV_List_Detail.Columns[14].Visible = false; //次品数
            GV_List_Detail.Columns[15].Visible = false; //废品数
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            GV_List.Columns[4].Visible = false;//ASN No
            GV_List.Columns[5].Visible = false;//ExternalReceiptNo
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
        {
            GV_List_Detail.Columns[11].Visible = false; //目的库位
            GV_List_Detail.Columns[14].Visible = false; //次品数
            GV_List_Detail.Columns[15].Visible = false; //废品数
        }
        if (IsSupplier)
        {
            GV_List.Columns[5].Visible = false;//ExternalReceiptNo
            GV_List.Columns[6].Visible = false;//PartyFrom.Name
            GV_List.Columns[7].Visible = false;//ShipFrom.Address
        }
    }

    protected void GV_List_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (IsExport && e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

}
