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
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;

public partial class Finance_Payment_List : ListModuleBase
{
    public EventHandler EditEvent;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.GV_List.Columns[2].HeaderText = "${MasterData.Payment.Customer}";
            }
        }
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string paymentNo = ((LinkButton)sender).CommandArgument;
            EditEvent(paymentNo, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string paymentNo = ((LinkButton)sender).CommandArgument;
        try
        {
            Payment payment = ThePaymentMgr.LoadPayment(paymentNo, true);
            if (payment.BillPayments == null || payment.BillPayments.Count == 0)
            {
                ThePaymentMgr.DeletePayment(paymentNo, this.CurrentUser);
                ShowSuccessMessage("MasterData.Payment.DeletePayment.Successfully", paymentNo);
                UpdateView();
            }
            else
            {
                this.ShowWarningMessage("MasterData.Payment.WarningMessage.BillPaymentNotEmpty", paymentNo);
            }
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.Payment.DeletePayment.Fail", paymentNo);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Payment payment = (Payment)e.Row.DataItem;
            if (payment.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.Visible = false;
            }

            e.Row.Cells[1].Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor=this.style.borderColor");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            e.Row.Cells[1].Attributes.Add("title", GetDetail(payment));
        }


    }

    private string GetDetail(Payment payment)
    {
        string detail = "";
        if (payment == null
                || payment.BillPayments == null
                || payment.BillPayments.Count == 0
                || payment.BillPayments[0] == null
                || payment.BillPayments[0].Bill == null
                || payment.BillPayments[0].Bill.BillDetails == null
                || payment.BillPayments[0].Bill.BillDetails[0] == null
                || payment.BillPayments[0].Bill.BillDetails[0].ActingBill == null
                || payment.BillPayments[0].Bill.BillDetails[0].ActingBill.OrderHead == null) return detail;

        detail += "cssbody=[obbd] cssheader=[obhd] header=[" + payment.PaymentNo + "] body=[<table width=100%>";
        foreach (BillPayment bp in payment.BillPayments)
        {
            foreach (BillDetail bd in bp.Bill.BillDetails)
            {
                OrderHead oh = bd.ActingBill.OrderHead;
                string orderNo = oh.OrderNo;
                string partyFrom = oh.PartyFrom.Name;
                string partyTo = oh.PartyTo.Name;
                string createUser = oh.CreateUser.Name;
                detail += "<tr><td>" + orderNo + "</td><td>" + partyFrom + "</td><td>" + partyTo + "</td><td>" + createUser + "</td></tr>";

            }
        }
        /*
         * System.Collections.Generic.IList<OrderDetail> ods = orderHead.OrderDetails;
        
        foreach (OrderDetail od in ods)
        {
            string ItemCode = od.Item.Code;
            string ItemDesc = od.Item.Description.Replace("[", "&#91;").Replace("]", "&#93;");
            string OrderQty = od.OrderedQty.ToString("0.########");
            string Uom = od.Uom.Code;
            string RecQty = od.ReceivedQty == null ? "0" : od.ReceivedQty.Value.ToString("0.########");
            string TotalAmount = od.TotalAmountTo != null && od.TotalAmountTo.HasValue ? od.TotalAmountTo.Value.ToString("0.00") : "0";
            detail += "<tr><td>" + ItemCode + "</td><td>" + ItemDesc + "</td><td>" + OrderQty + "</td><td>" + Uom + "</td><td>" + RecQty + "</td><td>" + TotalAmount + "</td></tr>";
        }
         * */
        detail += "</table>]";


        return detail;
    }
}
