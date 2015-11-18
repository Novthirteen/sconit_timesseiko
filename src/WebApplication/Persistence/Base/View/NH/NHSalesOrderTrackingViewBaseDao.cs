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
    public class NHSalesOrderTrackingViewBaseDao : NHDaoBase, ISalesOrderTrackingViewBaseDao
    {
        public NHSalesOrderTrackingViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            Create(entity);
        }

        public virtual IList<SalesOrderTrackingView> GetAllSalesOrderTrackingView()
        {
            return FindAll<SalesOrderTrackingView>();
        }

        public virtual SalesOrderTrackingView LoadSalesOrderTrackingView(Int32 id)
        {
            return FindById<SalesOrderTrackingView>(id);
        }

        public virtual void UpdateSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            Update(entity);
        }

        public virtual void DeleteSalesOrderTrackingView(Int32 id)
        {
            string hql = @"from SalesOrderTrackingView entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteSalesOrderTrackingView(SalesOrderTrackingView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteSalesOrderTrackingView(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from SalesOrderTrackingView entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteSalesOrderTrackingView(IList<SalesOrderTrackingView> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (SalesOrderTrackingView entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteSalesOrderTrackingView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
