using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

public partial class Finance_Payment_NewList : ListModuleBase
{
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




    public void BindDataSource(IList<Bill> billList)
    {
        //if (billList != null)
        //{
            /*
            var q = from a in billList
                    orderby a.ExternalReceiptNo
                    select a;

            this.GV_List.DataSource = q.ToList<Bill>();
            */
            this.GV_List.DataSource = billList;
            this.UpdateView();
        //}
    }

    public IList<Bill> PopulateSelectedData()
    {
        if (this.GV_List.Rows != null && this.GV_List.Rows.Count > 0)
        {
            IList<Bill> billList = new List<Bill>();
            foreach (GridViewRow row in this.GV_List.Rows)
            {

                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked)
                {
                    HiddenField hfBillNo = row.FindControl("hfBillNo") as HiddenField;
                    TextBox tbThisBackwashAmount = row.FindControl("tbThisBackwashAmount") as TextBox;

                    Bill bill = new Bill();
                    bill.BillNo = hfBillNo.Value;
                    if (tbThisBackwashAmount.Text != null && tbThisBackwashAmount.Text.Trim().Length > 0 && decimal.Parse(tbThisBackwashAmount.Text.Trim()) >= 0)
                    {
                        bill.ThisBackwashAmount = decimal.Parse(tbThisBackwashAmount.Text.Trim());
                    }
                    else
                    {
                        bill.ThisBackwashAmount = 0;
                    }


                    billList.Add(bill);
                }
            }
            return billList;
        }

        return null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                
            }
            else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.GV_List.Columns[1].HeaderText = "${MasterData.Bill.Customer}";
            }
            
           
        }
    }

    public override void UpdateView()
    {
        this.GV_List.DataBind();
        if (this.GV_List.DataSource != null)
        {
            this.lblNoRecordFound.Visible = false;
            Button btnAddPayment = this.Parent.FindControl("btnAddPayment") as Button;
            btnAddPayment.Visible = true;

            //this.tblTotalDetailAmount.Visible = true;

            //this.tbTotalDetailAmount.Text = TotalDetailAmount.ToString("F2"); ;
        }
        else
        {
            this.lblNoRecordFound.Visible = true;
            Button btnAddPayment = this.Parent.FindControl("btnAddPayment") as Button;
            btnAddPayment.Visible = false;

            //this.tblTotalDetailAmount.Visible = false;
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Bill bill = (Bill)e.Row.DataItem;
        }
    }
}
