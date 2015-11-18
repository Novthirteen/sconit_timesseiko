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
using com.Sconit.Control;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using System.Collections.Generic;

public partial class Finance_Payment_Edit : EditModuleBase
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
    public string PaymentNo
    {
        get
        {
            return (string)ViewState["PaymentNo"];
        }
        set
        {
            ViewState["PaymentNo"] = value;
        }
    }

    

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDetail.ModuleType = this.ModuleType;
        this.ucDetail.EditRefreshEvent += new System.EventHandler(this.EditRefresh_Render);



        if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            this.lgd.InnerText = "${MasterData.Payment.SOPayment}";
        }
        else if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            this.lgd.InnerText = "${MasterData.Payment.POPayment}";
        }


    }

    //在明细中点击添加或者移除时候的事件
    void EditRefresh_Render(object sender, EventArgs e)
    {
        
        string paymentNo = (string)sender;
        this.PaymentNo = paymentNo;
        Payment payment = ThePaymentMgr.LoadPayment(this.PaymentNo, true);
        this.FV_Payment.DataBind();
        UpdateView(payment);
        this.ucDetail.UpdateView(payment);
    }


    private void UpdateView(Payment payment)
    {
        
        #region 根据状态显示按钮
       
        if (payment.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            this.btnSave.Visible = true;
            this.btnDelete.Visible = true;
        }
        else if (payment.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
        {
            this.btnSave.Visible = true;
            this.btnDelete.Visible = true;
        }
        else if (payment.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
        {
            this.btnSave.Visible = false;
            this.btnDelete.Visible = false;
        }
        else if (payment.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {
            this.btnSave.Visible = false;
            this.btnDelete.Visible = false;
        }
        
        #endregion
        /*
        #region 根据状态隐藏/显示字段
        if (payment.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            TextBox tbExtPaymentNo = this.FV_Payment.FindControl("tbExtPaymentNo") as TextBox;
            tbExtPaymentNo.ReadOnly = true;
        }
        else
        {
            TextBox tbExtPaymentNo = this.FV_Payment.FindControl("tbExtPaymentNo") as TextBox;
            tbExtPaymentNo.ReadOnly = false;
        }
        #endregion
        */
        this.ucDetail.ModuleType = this.ModuleType;
        
    }

    protected void FV_Payment_DataBound(object sender, EventArgs e)
    {
        Payment payment = (Payment)((FormView)(sender)).DataItem;
        UpdateView(payment);

        this.ucDetail.UpdateView(payment);
    }

    public void InitPageParameter(String paymentNo)
    {
        this.PaymentNo = paymentNo;
        this.ODS_Payment.SelectParameters["paymentNo"].DefaultValue = paymentNo;

        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            //            this.Gv_List.Columns[2].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            Literal lblParty = this.FV_Payment.FindControl("lblParty") as Literal;
            lblParty.Text = "${MasterData.Payment.Customer}:";

            //          this.Gv_List.Columns[1].Visible = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            IList<BillPayment> billPaymentList = this.ucDetail.PopulateData(false);
            /*
            Payment payment = this.ThePaymentMgr.LoadPayment(this.PaymentNo);
            
            payment.BillPayments = billPaymentList;

            //回冲数
            decimal? thisBackwashAmount = billPaymentList.Sum(bp => bp.BackwashAmount);
            if (payment.Amount < backwashAmount.Value)
            {
                this.ShowWarningMessage("MasterData.Payment.Return.Blunt.Amount.Must.Be.Less.Than.Or.Equal.To.Payment");
            }else
            {
                payment.ThisBackwashAmount = thisBackwashAmount;
            */
            this.ThePaymentMgr.UpdatePayment(this.PaymentNo, billPaymentList, this.CurrentUser);

            this.ShowSuccessMessage("MasterData.Payment.UpdateSuccessfully", this.PaymentNo);
            this.FV_Payment.DataBind();
            //}
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
            Payment payment = ThePaymentMgr.LoadPayment(this.PaymentNo,true);
            if (payment.BillPayments == null || payment.BillPayments.Count == 0)
            {
                this.ThePaymentMgr.DeletePayment(this.PaymentNo,this.CurrentUser);
                this.ShowSuccessMessage("MasterData.Payment.DeleteSuccessfully", this.PaymentNo);
                this.BackEvent(null, e);
            }
            else
            {
                this.ShowWarningMessage("MasterData.Payment.WarningMessage.BillPaymentNotEmpty", this.PaymentNo);
            }
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


}
