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
    public class BillPaymentBaseMgr : SessionBase, IBillPaymentBaseMgr
    {
        public IBillPaymentDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateBillPayment(BillPayment entity)
        {
            entityDao.CreateBillPayment(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual BillPayment LoadBillPayment(Int32 id)
        {
            return entityDao.LoadBillPayment(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<BillPayment> GetAllBillPayment()
        {
            return entityDao.GetAllBillPayment();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateBillPayment(BillPayment entity)
        {
            entityDao.UpdateBillPayment(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillPayment(Int32 id)
        {
            entityDao.DeleteBillPayment(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillPayment(BillPayment entity)
        {
            entityDao.DeleteBillPayment(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillPayment(IList<Int32> pkList)
        {
            entityDao.DeleteBillPayment(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillPayment(IList<BillPayment> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteBillPayment(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
