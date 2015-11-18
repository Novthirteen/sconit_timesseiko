using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IBillPaymentBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateBillPayment(BillPayment entity);

        BillPayment LoadBillPayment(Int32 id);

        IList<BillPayment> GetAllBillPayment();
    
        void UpdateBillPayment(BillPayment entity);

        void DeleteBillPayment(Int32 id);
    
        void DeleteBillPayment(BillPayment entity);
    
        void DeleteBillPayment(IList<Int32> pkList);
    
        void DeleteBillPayment(IList<BillPayment> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
