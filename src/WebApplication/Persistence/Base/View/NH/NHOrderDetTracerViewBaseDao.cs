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
    public class NHOrderDetTracerViewBaseDao : NHDaoBase, IOrderDetTracerViewBaseDao
    {
        public NHOrderDetTracerViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateOrderDetTracerView(OrderDetTracerView entity)
        {
            Create(entity);
        }

        public virtual IList<OrderDetTracerView> GetAllOrderDetTracerView()
        {
            return FindAll<OrderDetTracerView>();
        }

        public virtual OrderDetTracerView LoadOrderDetTracerView(Int32 id)
        {
            return FindById<OrderDetTracerView>(id);
        }

        public virtual void UpdateOrderDetTracerView(OrderDetTracerView entity)
        {
            Update(entity);
        }

        public virtual void DeleteOrderDetTracerView(Int32 id)
        {
            string hql = @"from OrderDetTracerView entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteOrderDetTracerView(OrderDetTracerView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteOrderDetTracerView(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from OrderDetTracerView entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteOrderDetTracerView(IList<OrderDetTracerView> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (OrderDetTracerView entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteOrderDetTracerView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
