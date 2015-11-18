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
    public class OrderTrackingViewBaseMgr : SessionBase, IOrderTrackingViewBaseMgr
    {
        public IOrderTrackingViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateOrderTrackingView(OrderTrackingView entity)
        {
            entityDao.CreateOrderTrackingView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual OrderTrackingView LoadOrderTrackingView(Int32 id)
        {
            return entityDao.LoadOrderTrackingView(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<OrderTrackingView> GetAllOrderTrackingView()
        {
            return entityDao.GetAllOrderTrackingView();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateOrderTrackingView(OrderTrackingView entity)
        {
            entityDao.UpdateOrderTrackingView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderTrackingView(Int32 id)
        {
            entityDao.DeleteOrderTrackingView(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderTrackingView(OrderTrackingView entity)
        {
            entityDao.DeleteOrderTrackingView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderTrackingView(IList<Int32> pkList)
        {
            entityDao.DeleteOrderTrackingView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderTrackingView(IList<OrderTrackingView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteOrderTrackingView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
