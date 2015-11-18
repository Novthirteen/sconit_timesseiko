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
    public class ActFundsAgingViewBaseMgr : SessionBase, IActFundsAgingViewBaseMgr
    {
        public IActFundsAgingViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateActFundsAgingView(ActFundsAgingView entity)
        {
            entityDao.CreateActFundsAgingView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ActFundsAgingView LoadActFundsAgingView(String id)
        {
            return entityDao.LoadActFundsAgingView(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ActFundsAgingView> GetAllActFundsAgingView()
        {
            return entityDao.GetAllActFundsAgingView();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateActFundsAgingView(ActFundsAgingView entity)
        {
            entityDao.UpdateActFundsAgingView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteActFundsAgingView(String id)
        {
            entityDao.DeleteActFundsAgingView(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteActFundsAgingView(ActFundsAgingView entity)
        {
            entityDao.DeleteActFundsAgingView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteActFundsAgingView(IList<String> pkList)
        {
            entityDao.DeleteActFundsAgingView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteActFundsAgingView(IList<ActFundsAgingView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteActFundsAgingView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
