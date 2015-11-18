using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PaymentBaseMgr : SessionBase, IPaymentBaseMgr
    {
        public IPaymentDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreatePayment(Payment entity)
        {
            entityDao.CreatePayment(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Payment LoadPayment(String paymentNo)
        {
            return entityDao.LoadPayment(paymentNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Payment> GetAllPayment()
        {
            return entityDao.GetAllPayment();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdatePayment(Payment entity)
        {
            entityDao.UpdatePayment(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePayment(String paymentNo)
        {
            entityDao.DeletePayment(paymentNo);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePayment(Payment entity)
        {
            entityDao.DeletePayment(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePayment(IList<String> pkList)
        {
            entityDao.DeletePayment(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePayment(IList<Payment> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeletePayment(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
