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
    public class SalesOrderTrackingViewBaseMgr : SessionBase, ISalesOrderTrackingViewBaseMgr
    {
        public ISalesOrderTrackingViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            entityDao.CreateSalesOrderTrackingView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual SalesOrderTrackingView LoadSalesOrderTrackingView(Int32 id)
        {
            return entityDao.LoadSalesOrderTrackingView(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<SalesOrderTrackingView> GetAllSalesOrderTrackingView()
        {
            return entityDao.GetAllSalesOrderTrackingView();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            entityDao.UpdateSalesOrderTrackingView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSalesOrderTrackingView(Int32 id)
        {
            entityDao.DeleteSalesOrderTrackingView(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            entityDao.DeleteSalesOrderTrackingView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSalesOrderTrackingView(IList<Int32> pkList)
        {
            entityDao.DeleteSalesOrderTrackingView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSalesOrderTrackingView(IList<SalesOrderTrackingView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteSalesOrderTrackingView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
