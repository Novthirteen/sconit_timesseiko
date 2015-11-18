using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IPaymentMgr : IPaymentBaseMgr
    {
        #region Customized Methods

        void CreatePayment(Payment payment, User user);

        void DeletePayment(string paymentNo, User user);

        void DeleteBillPayment(string paymentNo, IList<BillPayment> billPaymentList, User user);

        Payment LoadPayment(string paymentNo, bool includeBillPayment);

        void UpdatePayment(Payment payment, string userCode);

        void UpdatePayment(Payment payment, User user);
        
        void UpdatePayment(string paymentNo, IList<BillPayment> billPaymentList, User user);

        void AddBillPayment(string paymentNo, IList<Bill> billList, User user);
        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPaymentMgrE : com.Sconit.Service.MasterData.IPaymentMgr
    {
    }
}

#endregion Extend Interface