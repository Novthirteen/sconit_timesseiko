using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IBillPaymentMgr : IBillPaymentBaseMgr
    {
        #region Customized Methods

       
        IList<BillPayment> GetBillPaymentByPaymentNo(string paymentNo);

        BillPayment LoadBillPayment(int id, bool includeBill);
        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IBillPaymentMgrE : com.Sconit.Service.MasterData.IBillPaymentMgr
    {
    }
}

#endregion Extend Interface