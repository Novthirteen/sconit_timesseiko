using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class BillPaymentMgr : BillPaymentBaseMgr, IBillPaymentMgr
    {

        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<BillPayment> GetBillPaymentByPaymentNo(string paymentNo)
        {

            DetachedCriteria criteria = DetachedCriteria.For(typeof(BillPayment));
            if (paymentNo != string.Empty)
            {
                criteria.Add(Expression.Eq("Payment.PaymentNo", paymentNo));
            }

            //criteria.Add(Expression.Eq("TransactionType", transType));

            return this.criteriaMgrE.FindAll<BillPayment>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BillPayment LoadBillPayment(int id, bool includeBill)
        {
            BillPayment billPayment = this.LoadBillPayment(id);

            if (includeBill && billPayment.Bill != null)
            {
                //, bool includeBillDetails
                //if (includeBillDetails && billPayment.Bill.BillDetails != null && billPayment.Bill.BillDetails.Count > 0)
                //{
                //}
            }


            return billPayment;
        }

        #endregion Customized Methods
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class BillPaymentMgrE : com.Sconit.Service.MasterData.Impl.BillPaymentMgr, IBillPaymentMgrE
    {

    }
}

#endregion Extend Class