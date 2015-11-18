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
    public class OrderDetTracerViewBaseMgr : SessionBase, IOrderDetTracerViewBaseMgr
    {
        public IOrderDetTracerViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateOrderDetTracerView(OrderDetTracerView entity)
        {
            entityDao.CreateOrderDetTracerView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual OrderDetTracerView LoadOrderDetTracerView(Int32 id)
        {
            return entityDao.LoadOrderDetTracerView(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<OrderDetTracerView> GetAllOrderDetTracerView()
        {
            return entityDao.GetAllOrderDetTracerView();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateOrderDetTracerView(OrderDetTracerView entity)
        {
            entityDao.UpdateOrderDetTracerView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderDetTracerView(Int32 id)
        {
            entityDao.DeleteOrderDetTracerView(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderDetTracerView(OrderDetTracerView entity)
        {
            entityDao.DeleteOrderDetTracerView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderDetTracerView(IList<Int32> pkList)
        {
            entityDao.DeleteOrderDetTracerView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderDetTracerView(IList<OrderDetTracerView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteOrderDetTracerView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}
