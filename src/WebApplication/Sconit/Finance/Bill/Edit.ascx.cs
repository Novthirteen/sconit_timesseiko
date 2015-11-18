using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using System.Collections;

public partial class Finance_Bill_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

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

    private int DecimalLength
    {
        get
        {
            return (int)ViewState["DecimalLength"];
        }
        set
        {
            ViewState["DecimalLength"] = value;
        }
    }

    public string PartyCode
    {
        get
        {
            return (string)ViewState["PartyCode"];
        }
        set
        {
            ViewState["PartyCode"] = value;
        }
    }

    public string BillNo
    {
        get
        {
            return (string)ViewState["BillNo"];
        }
        set
        {
            ViewState["BillNo"] = value;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Bill bill = this.TheBillMgr.LoadBill(this.BillNo, true);
        IList<object> list = new List<object>();
        if (bill != null)
        {
            list.Add(bill);
            list.Add(bill.BillDetails);
        }
        string barCodeUrl = "";
        if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            barCodeUrl = TheReportMgr.WriteToFile("Bill.xls", list);
        }
        else
        {
            barCodeUrl = TheReportMgr.WriteToFile("BillMarket.xls", list);
        }

        Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");
        this.ShowSuccessMessage("MasterData.Bill.Print.Successful");
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        Bill bill = this.TheBillMgr.LoadBill(this.BillNo, true);
        IList<object> list = new List<object>();
        if (bill != null)
        {
            list.Add(bill);
            list.Add(bill.BillDetails);
        }

        if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            TheReportMgr.WriteToClient("Bill.xls", bill.BillNo, "Bill.xls");
        }
        else
        {
            TheReportMgr.WriteToClient("BillMarket.xls", list, "BillMarket.xls");
        }
    }

    public void InitPageParameter(string billNo)
    {
        this.BillNo = billNo;
        this.ODS_Bill.SelectParameters["billNo"].DefaultValue = billNo;
        this.ODS_Bill.SelectParameters["includeDetail"].DefaultValue = true.ToString();

        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            this.Gv_List.Columns[2].Visible = false;
            this.Gv_List.Columns[4].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            Literal lblParty = this.FV_Bill.FindControl("lblParty") as Literal;
            lblParty.Text = "${MasterData.Bill.Customer}:";

            this.Gv_List.Columns[1].HeaderText = "${MasterData.Order.OrderHead.OrderNo.Distribution}";
            this.Gv_List.Columns[3].Visible = false;
        }
        Bill bill = TheBillMgr.LoadBill(billNo,true);
        UpdateView(bill);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucNewSearch.BackEvent += new EventHandler(AddBack_Render);

        if (!IsPostBack)
        {
            this.ucNewSearch.ModuleType = this.ModuleType;
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                this.legend.InnerText = "${MasterData.Bill.POBill}";
            }
            else
            {
                this.legend.InnerText = "${MasterData.Bill.SOBill}";
            }
        }
   }

    protected void FV_Bill_DataBound(object sender, EventArgs e)
    {
        Bill bill = (Bill)((FormView)(sender)).DataItem;
        Literal ltlFixtureDate = (Literal)this.FV_Bill.FindControl("ltlFixtureDate");
        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            ltlFixtureDate.Text = "${MasterData.Bill.PayableDate}";
        }
        else
        {
            ltlFixtureDate.Text = "${MasterData.Bill.ReceivableDate}";
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BillDetail billDetail = (BillDetail)e.Row.DataItem;

            TextBox tbAmount = (TextBox)e.Row.FindControl("tbAmount");
            tbAmount.Attributes["oldValue"] = tbAmount.Text;

            if (billDetail.Bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                TextBox tbQty = (TextBox)e.Row.FindControl("tbQty");
                TextBox tbDiscountRate = (TextBox)e.Row.FindControl("tbDiscountRate");
                TextBox tbDiscount = (TextBox)e.Row.FindControl("tbDiscount");

                tbQty.ReadOnly = true;
                tbDiscountRate.ReadOnly = true;
                tbDiscount.ReadOnly = true;
            }

            //显示已开票数的时候去掉本次开票数
            TextBox tbBilledQty = (TextBox)e.Row.FindControl("tbBilledQty");
            tbBilledQty.Text = (billDetail.ActingBill.BilledQty - billDetail.BilledQty).ToString("0.########");

            if (e.Row.RowIndex == (((IList)this.Gv_List.DataSource).Count - 1)
                && billDetail.Bill.AmountAfterDiscount != null && billDetail.Bill.AmountAfterDiscount.HasValue)
            {
                decimal t = billDetail.Bill.BillDetails.Where(bd => bd.Id != billDetail.Id).Sum(bd => bd.Amount);
                tbAmount.Text = (Math.Round(billDetail.Bill.AmountAfterDiscount.Value,2) - t).ToString("F2");
            }
        }
    }
    /*
    protected void lbRefBillNo_Click(object sender, EventArgs e)
    {
        string refBillNo = ((LinkButton)(sender)).CommandArgument;
        InitPageParameter(refBillNo);
    }
    */
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            this.SaveBill();
            this.ShowSuccessMessage("MasterData.Bill.UpdateSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private Bill SaveBill()
    {
        IList<BillDetail> billDetailList = PopulateData(false);
        Bill bill = this.TheBillMgr.LoadBill(this.BillNo);
        TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
        if (tbExternalBillNo.Text.Trim() != string.Empty)
        {
            bill.ExternalBillNo = tbExternalBillNo.Text.Trim();
        }
        else
        {
            bill.ExternalBillNo = null;
        }

        TextBox tbFixtureDate = this.FV_Bill.FindControl("tbFixtureDate") as TextBox;
        if (tbFixtureDate.Text.Trim() != string.Empty)
        {
            bill.FixtureDate = DateTime.Parse(tbFixtureDate.Text.Trim());
        }
        else
        {
            bill.FixtureDate = null;
        }

        if (this.tbTotalDiscount.Text.Trim() != string.Empty)
        {
            bill.Discount = decimal.Parse(this.tbTotalDiscount.Text.Trim());
        }
        else
        {
            bill.Discount = null;
        }
        bill.BillDetails = billDetailList;
        this.TheBillMgr.UpdateBill(bill, this.CurrentUser);

        return bill;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Bill bill = this.SaveBill();
            if (bill.ExternalBillNo == null || bill.ExternalBillNo.Trim() == string.Empty)
            {
                this.ShowErrorMessage("MasterData.Bill.Error.ExternalBillNoRequired");
                return;
            }
            if (bill.FixtureDate == null || !bill.FixtureDate.HasValue)
            {
                if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
                {
                    this.ShowErrorMessage("MasterData.Bill.Error.PayableDateNoRequired");
                }
                else
                {
                    this.ShowErrorMessage("MasterData.Bill.Error.ReceivableDateNoRequired");
                }
                return;
            }

            this.TheBillMgr.ReleaseBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.ReleaseSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.DeleteBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.DeleteSuccessfully", this.BillNo);
            this.BackEvent(this, e);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.CloseBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.CloseSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.CancelBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.CancelSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.VoidBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.VoidSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
        }
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        IDictionary<string, string> actionParameter = new Dictionary<string, string>();
        actionParameter.Add("PartyCode", this.PartyCode);
        this.ucNewSearch.QuickSearch(actionParameter);
        this.ucNewSearch.Visible = true;
    }

    protected void btnDeleteDetail_Click(object sender, EventArgs e)
    {
        try
        {
            IList<BillDetail> billDetailList = PopulateData(true);
            this.TheBillMgr.DeleteBillDetail(billDetailList, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.DeleteBillDetailSuccessfully");
            //this.FV_Bill.DataBind();
            this.InitPageParameter(this.BillNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void AddBack_Render(object sender, EventArgs e)
    {
        this.ucNewSearch.Visible = false;
        this.InitPageParameter(this.BillNo);
        //this.FV_Bill.DataBind();
    }

    private void UpdateView(Bill bill)
    {
        #region 根据状态显示按钮
        if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            this.btnSave.Visible = true;
            this.btnSubmit.Visible = true;
            this.btnDelete.Visible = true;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = true;
            this.btnDeleteDetail.Visible = true;
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else
            {
                this.btnPrint.Visible = false;
                this.btnExport.Visible = false;
            }
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            if (!bill.BackwashAmount.HasValue || bill.BackwashAmount.Value == 0)
            {
                this.btnCancel.Visible = true;
            }
            else
            {
                this.btnCancel.Visible = false;
            }
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.btnPrint.Visible = true;
                this.btnExport.Visible = true;
            }
            else
            {
                this.btnPrint.Visible = false;
                this.btnExport.Visible = false;
            }
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            this.btnPrint.Visible = false;
            this.btnExport.Visible = false;
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnPrint.Visible = false;
            this.btnExport.Visible = false;
            if (bill.BillType == BusinessConstants.CODE_MASTER_BILL_TYPE_VALUE_CANCEL)
            {
                this.btnVoid.Visible = false; ;
            }
            else
            {
                this.btnVoid.Visible = true;
            }
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            this.btnPrint.Visible = false;
            this.btnExport.Visible = false;
        }
        #endregion

        #region 根据状态隐藏/显示字段
        if (bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            this.Gv_List.Columns[0].Visible = false;
            this.tbTotalDiscountRate.ReadOnly = true;
            this.tbTotalDiscount.ReadOnly = true;

            TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
            tbExternalBillNo.ReadOnly = true;

            TextBox tbFixtureDate = this.FV_Bill.FindControl("tbFixtureDate") as TextBox;
            tbFixtureDate.ReadOnly = true;
        }
        else
        {
            this.Gv_List.Columns[0].Visible = true;
            this.tbTotalDiscountRate.ReadOnly = false;
            this.tbTotalDiscount.ReadOnly = false;

            TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
            tbExternalBillNo.ReadOnly = false;

            TextBox tbFixtureDate = this.FV_Bill.FindControl("tbFixtureDate") as TextBox;
            tbFixtureDate.ReadOnly = false;

        }

        Literal ltlFixtureDate = (Literal)this.FV_Bill.FindControl("ltlFixtureDate");
        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            ltlFixtureDate.Text = "${MasterData.Bill.PayableDate}";
        }
        else
        {
            ltlFixtureDate.Text = "${MasterData.Bill.ReceivableDate}";
        }

        #endregion

        #region 给总金额和折扣赋值
        this.tbTotalAmount.Text = (bill.Amount.HasValue ? bill.Amount.Value : 0).ToString("F2");
        this.tbTotalDetailAmount.Text = (bill.AmountAfterDiscount.HasValue ? bill.AmountAfterDiscount.Value : 0).ToString("F2");
        this.tbTotalDiscount.Text = (bill.Discount.HasValue ? bill.Discount.Value : 0).ToString("F2");
        this.tbTotalDiscountRate.Text = (bill.TotalBillDiscountRate.HasValue ? bill.TotalBillDiscountRate.Value : 0).ToString("F2");
        #endregion

        #region 初始化弹出窗口
        this.PartyCode = bill.BillAddress.Party.Code;
        this.ucNewSearch.InitPageParameter(true, bill);
        #endregion

        UpdateDetailView(bill.BillDetails);
    }

    private void UpdateDetailView(IList<BillDetail> billDetailList)
    {
        this.Gv_List.DataSource = billDetailList;
        this.Gv_List.DataBind();
    }

    private IList<BillDetail> PopulateData(bool isChecked)
    {
        if (this.Gv_List.Rows != null && this.Gv_List.Rows.Count > 0)
        {
            IList<BillDetail> billDetailList = new List<BillDetail>();
            foreach (GridViewRow row in this.Gv_List.Rows)
            {
                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked || !isChecked)
                {
                    HiddenField hfId = row.FindControl("hfId") as HiddenField;
                    TextBox tbQty = row.FindControl("tbQty") as TextBox;
                    TextBox tbDiscount = row.FindControl("tbDiscount") as TextBox;

                    BillDetail billDetail = new BillDetail();
                    billDetail.Id = int.Parse(hfId.Value);
                    billDetail.BilledQty = decimal.Parse(tbQty.Text);
                    billDetail.Discount = decimal.Parse(tbDiscount.Text);

                    billDetailList.Add(billDetail);
                }
            }
            return billDetailList;
        }

        return null;
    }
}
