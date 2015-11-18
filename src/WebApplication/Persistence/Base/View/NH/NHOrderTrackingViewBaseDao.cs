using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.View;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.View.NH
{
    public class NHOrderTrackingViewBaseDao : NHDaoBase, IOrderTrackingViewBaseDao
    {
        public NHOrderTrackingViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateOrderTrackingView(OrderTrackingView entity)
        {
            Create(entity);
        }

        public virtual IList<OrderTrackingView> GetAllOrderTrackingView()
        {
            return FindAll<OrderTrackingView>();
        }

        public virtual OrderTrackingView LoadOrderTrackingView(Int32 id)
        {
            return FindById<OrderTrackingView>(id);
        }

        public virtual void UpdateOrderTrackingView(OrderTrackingView entity)
        {
            Update(entity);
        }

        public virtual void DeleteOrderTrackingView(Int32 id)
        {
            string hql = @"from OrderTrackingView entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteOrderTrackingView(OrderTrackingView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteOrderTrackingView(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from OrderTrackingView entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteOrderTrackingView(IList<OrderTrackingView> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (OrderTrackingView entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteOrderTrackingView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
