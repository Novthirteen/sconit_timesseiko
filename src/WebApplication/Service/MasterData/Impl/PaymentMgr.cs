using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PaymentMgr : PaymentBaseMgr, IPaymentMgr
    {

        public IUserMgrE userMgrE { get; set; }
        public IBillPaymentMgrE billPaymentMgrE { get; set; }
        public INumberControlMgrE numberControlMgrE { get; set; }
        public IBillMgrE billMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void UpdatePayment(Payment payment, string userCode)
        {
            this.UpdatePayment(payment, this.userMgrE.CheckAndLoadUser(userCode));
        }

        /**
         * 用于付款-账单关联时候的保存按钮
         * 
         */
        [Transaction(TransactionMode.Requires)]
        public void UpdatePayment(string paymentNo, IList<BillPayment> billPaymentList, User user)
        {

            Payment payment = this.LoadPayment(paymentNo);

            decimal thisBackwashAmount = 0;// billPaymentList.Sum(bp => bp.BackwashAmount);

            if (payment.Amount < thisBackwashAmount)
            {
                throw new BusinessErrorException("MasterData.Payment.Return.Blunt.Amount.Must.Be.Less.Than.Or.Equal.To.Payment");
            }


            Bill oldBill = null;
            if (billPaymentList != null)
            {
                foreach (BillPayment billPayment in billPaymentList)
                {
                    BillPayment oldBillPayment = this.billPaymentMgrE.LoadBillPayment(billPayment.Id);
                    oldBill = oldBillPayment.Bill;

                    thisBackwashAmount += billPayment.BackwashAmount.Value;

                    //本次回冲金额不能大于bill的未付款数
                    if (Math.Round(billPayment.BackwashAmount.Value,2) > (Math.Round(oldBill.NoBackwashAmount, 2) + oldBillPayment.BackwashAmount))
                    {
                        throw new BusinessErrorException("MasterData.Payment.The.Return.Value.Is.Not.Rush.To.Payment", oldBill.BillNo);
                    }

                    if (oldBill.BackwashAmount.HasValue)
                    {
                        //原来保存的值退回去
                        oldBill.BackwashAmount -= oldBillPayment.BackwashAmount;

                        //加上本次回冲金额
                        oldBill.BackwashAmount += billPayment.BackwashAmount;
                    }
                    else
                    {
                        oldBill.BackwashAmount = 0;
                    }

                    DateTime dateTimeNow = DateTime.Now;
                    if (oldBill.BackwashAmount != null
                            && oldBill.AmountAfterDiscount != null
                            && oldBill.BackwashAmount.Value != 0
                            && oldBill.AmountAfterDiscount.Value != 0
                            && Math.Round(oldBill.BackwashAmount.Value, 2) == Math.Round(oldBill.AmountAfterDiscount.Value, 2))
                    {
                        oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                        oldBill.CloseDate = dateTimeNow;
                        oldBill.CloseUser = user;
                    }
                    else
                    {
                        oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
                        oldBill.SubmitDate = dateTimeNow;
                        oldBill.EffectiveDate = dateTimeNow;
                        oldBill.SubmitUser = user;
                        oldBill.CloseDate = null;
                        oldBill.CloseUser = null;
                    }

                    oldBill.LastModifyUser = user;
                    oldBill.LastModifyDate = dateTimeNow;
                    billMgrE.UpdateBill(oldBill);

                    oldBillPayment.BackwashAmount = billPayment.BackwashAmount;
                    billPaymentMgrE.UpdateBillPayment(oldBillPayment);

                }
            }

            if (thisBackwashAmount > payment.Amount)
            {
                throw new BusinessErrorException("MasterData.Payment.Return.Blunt.Amount.Must.Be.Less.Than.Or.Equal.To.Payment", payment.PaymentNo);
            }

            payment.BackwashAmount = thisBackwashAmount;
            this.UpdatePayment(payment, user);
        }


        [Transaction(TransactionMode.Requires)]
        public void UpdatePayment(Payment payment, User user)
        {
            //check
            Payment oldPayment = this.CheckAndLoadPayment(payment.PaymentNo);

            oldPayment.BackwashAmount = payment.BackwashAmount;

            if (oldPayment.BackwashAmount != 0 && oldPayment.Amount != 0 
                    && Math.Round(oldPayment.BackwashAmount.Value,2) == Math.Round(oldPayment.Amount,2))
            {
                oldPayment.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            }
            else
            {
                oldPayment.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            }

            oldPayment.LastModifyUser = user;
            oldPayment.LastModifyDate = DateTime.Now;
            this.UpdatePayment(oldPayment);
        }

        /**
         * 
         *  用于付款-账单中,移除账单按钮 
         * 
         * 
         */
        [Transaction(TransactionMode.Requires)]
        public void DeleteBillPayment(string paymentNo, IList<BillPayment> billPaymentList, User user)
        {

            decimal? backwashAmount = 0;//billPaymentList.Sum(bp => bp.BackwashAmount);

            foreach (BillPayment billPayment in billPaymentList)
            {
                BillPayment oldBillPayment = billPaymentMgrE.LoadBillPayment(billPayment.Id, true);

                backwashAmount += oldBillPayment.BackwashAmount;

                Bill oldBill = oldBillPayment.Bill;


                if (oldBill.BackwashAmount.HasValue)
                {
                    oldBill.BackwashAmount -= oldBillPayment.BackwashAmount;
                }
                else
                {
                    oldBill.BackwashAmount = 0;
                }

                DateTime dateTimeNow = DateTime.Now;

                if (oldBill.BackwashAmount != null
                     && oldBill.AmountAfterDiscount != null
                     && oldBill.BackwashAmount.Value != 0
                     && oldBill.AmountAfterDiscount.Value != 0
                     && Math.Round(oldBill.BackwashAmount.Value,2) == Math.Round(oldBill.AmountAfterDiscount.Value,2))
                {
                    oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    oldBill.CloseDate = dateTimeNow;
                    oldBill.CloseUser = user;
                }
                else
                {
                    oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
                    oldBill.SubmitDate = dateTimeNow;
                    oldBill.EffectiveDate = dateTimeNow;
                    oldBill.SubmitUser = user;
                    oldBill.CloseDate = null;
                    oldBill.CloseUser = null;
                }

                oldBill.LastModifyUser = user;
                oldBill.LastModifyDate = dateTimeNow;
                billMgrE.UpdateBill(oldBill);

                this.billPaymentMgrE.DeleteBillPayment(oldBillPayment);
            }

            //this.billPaymentMgrE.DeleteBillPayment(billPaymentList);


            Payment payment = this.LoadPayment(paymentNo);

            if (payment.BackwashAmount < backwashAmount)
            {
                payment.BackwashAmount = 0;
            }
            else
            {
                payment.BackwashAmount -= backwashAmount;
            }

            if (payment.BackwashAmount.Value != 0 && payment.Amount != 0
                    && Math.Round(payment.BackwashAmount.Value,2) == Math.Round(payment.Amount,2))
            {
                payment.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
            }
            else
            {
                payment.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            }


            payment.LastModifyUser = user;
            payment.LastModifyDate = DateTime.Now;
            this.UpdatePayment(payment);

            //return payment;
        }

        [Transaction(TransactionMode.Requires)]
        public void DeletePayment(string paymentNo, User user)
        {
            Payment payment = this.LoadPayment(paymentNo, true);
            IList<BillPayment> billPaymentList = payment.BillPayments;
            DateTime dateTimeNow = DateTime.Now;
            foreach (BillPayment billPayment in billPaymentList)
            {
                Bill bill = billPayment.Bill;
                bill.BackwashAmount -= billPayment.BackwashAmount;

                if (bill.BackwashAmount == null
                    || !bill.BackwashAmount.HasValue
                    || Math.Round(bill.BackwashAmount.Value,2) < Math.Round(bill.AmountAfterDiscount.Value,2))
                {
                    bill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
                    bill.SubmitDate = dateTimeNow;
                    bill.EffectiveDate = dateTimeNow;
                    bill.SubmitUser = user;
                    bill.CloseDate = null;
                    bill.CloseUser = null;
                }
                else
                {
                    bill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                    bill.CloseDate = dateTimeNow;
                    bill.CloseUser = user;
                }
                bill.LastModifyUser = user;
                bill.LastModifyDate = DateTime.Now;

                billMgrE.UpdateBill(bill);
            }

            this.DeletePayment(paymentNo);
        }

        [Transaction(TransactionMode.Requires)]
        public void AddBillPayment(string paymentNo, IList<Bill> billList, User user)
        {

            Payment payment = this.LoadPayment(paymentNo);
            decimal bckwashAmount = 0;
            DateTime dateTimeNow = DateTime.Now;
            foreach (Bill bill in billList)
            {
                BillPayment billPayment = new BillPayment();
                Bill oldBill = billMgrE.LoadBill(bill.BillNo);


                if (Math.Round(bill.ThisBackwashAmount,2) > Math.Round(oldBill.NoBackwashAmount, 2))
                {
                    throw new BusinessErrorException("MasterData.Payment.The.Return.Value.Is.Not.Rush.To.Payment", bill.BillNo);
                }

                if (!oldBill.BackwashAmount.HasValue)
                {
                    oldBill.BackwashAmount = 0;
                }

                oldBill.BackwashAmount += bill.ThisBackwashAmount;

                if (oldBill.BackwashAmount != null && oldBill.AmountAfterDiscount != null
                    && oldBill.BackwashAmount.HasValue && oldBill.AmountAfterDiscount.HasValue
                    && Math.Round(oldBill.BackwashAmount.Value, 2) != 0 && Math.Round(oldBill.AmountAfterDiscount.Value, 2) != 0
                    && Math.Round(oldBill.BackwashAmount.Value, 2) == Math.Round(oldBill.AmountAfterDiscount.Value, 2))
                {
                    oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                }
                else
                {
                    oldBill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
                }


                oldBill.LastModifyDate = dateTimeNow;
                oldBill.LastModifyUser = user;
                billMgrE.UpdateBill(oldBill);

                billPayment.Bill = oldBill;
                billPayment.BackwashAmount = bill.ThisBackwashAmount;
                bckwashAmount += billPayment.BackwashAmount.Value;

                billPayment.Payment = payment;

                billPayment.LastModifyDate = dateTimeNow;
                billPayment.LastModifyUser = user;
                billPayment.CreateDate = dateTimeNow;
                billPayment.CreateUser = user;
                billPaymentMgrE.CreateBillPayment(billPayment);
            }

            if (Math.Round(bckwashAmount, 2) > Math.Round(payment.NoBackwashAmount, 2))
            {
                throw new BusinessErrorException("MasterData.Payment.Return.Blunt.Amount.Must.Be.Less.Than.Or.Equal.To.Payment");
            }

            if (!payment.BackwashAmount.HasValue)
            {
                payment.BackwashAmount = 0;
            }
            payment.BackwashAmount += bckwashAmount;

            this.UpdatePayment(payment, user);
        }


        [Transaction(TransactionMode.Requires)]
        public void CreatePayment(Payment payment, User user)
        {
            DateTime dateTimeNow = DateTime.Now;

            #region 创建Payment
            payment.PaymentNo = numberControlMgrE.GenerateNumber(BusinessConstants.CODE_PREFIX_PAY);
            payment.BackwashAmount = 0;
            payment.CreateUser = user;
            payment.CreateDate = dateTimeNow;
            payment.LastModifyUser = user;
            payment.LastModifyDate = dateTimeNow;
            payment.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;

            this.CreatePayment(payment);
            #endregion
        }



        [Transaction(TransactionMode.Unspecified)]
        public Payment LoadPayment(string paymentNo, bool includeBillPayment)
        {
            Payment payment = this.LoadPayment(paymentNo);

            if (includeBillPayment && payment.BillPayments != null && payment.BillPayments.Count > 0)
            {

            }


            return payment;
        }


        [Transaction(TransactionMode.Unspecified)]
        public Payment CheckAndLoadPayment(string paymentNo)
        {
            Payment payment = this.LoadPayment(paymentNo);
            if (payment != null)
            {
                return payment;
            }
            else
            {
                throw new BusinessErrorException("Payment.Error.PaymentNoNotExist", paymentNo);
            }
        }


        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PaymentMgrE : com.Sconit.Service.MasterData.Impl.PaymentMgr, IPaymentMgrE
    {
    }
}

#endregion Extend Class