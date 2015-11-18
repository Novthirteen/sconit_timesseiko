using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.View;
using com.Sconit.Persistence.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class BillDetViewBaseMgr : SessionBase, IBillDetViewBaseMgr
    {
        public IBillDetViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateBillDetView(BillDetView entity)
        {
            entityDao.CreateBillDetView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual BillDetView LoadBillDetView(Int32 id)
        {
            return entityDao.LoadBillDetView(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<BillDetView> GetAllBillDetView()
        {
            return entityDao.GetAllBillDetView();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateBillDetView(BillDetView entity)
        {
            entityDao.UpdateBillDetView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillDetView(Int32 id)
        {
            entityDao.DeleteBillDetView(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillDetView(BillDetView entity)
        {
            entityDao.DeleteBillDetView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillDetView(IList<Int32> pkList)
        {
            entityDao.DeleteBillDetView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBillDetView(IList<BillDetView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteBillDetView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
