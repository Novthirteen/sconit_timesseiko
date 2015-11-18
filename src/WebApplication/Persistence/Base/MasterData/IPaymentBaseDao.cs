using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.MasterData
{
    public interface IPaymentBaseDao
    {
        #region Method Created By CodeSmith

        void CreatePayment(Payment entity);

        Payment LoadPayment(String paymentNo);
  
        IList<Payment> GetAllPayment();
  
        void UpdatePayment(Payment entity);
        
        void DeletePayment(String paymentNo);
    
        void DeletePayment(Payment entity);
    
        void DeletePayment(IList<String> pkList);
    
        void DeletePayment(IList<Payment> entityList);    
        #endregion Method Created By CodeSmith
    }
}
