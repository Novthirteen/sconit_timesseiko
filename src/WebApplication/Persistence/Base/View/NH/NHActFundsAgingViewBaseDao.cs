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
    public class NHActFundsAgingViewBaseDao : NHDaoBase, IActFundsAgingViewBaseDao
    {
        public NHActFundsAgingViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateActFundsAgingView(ActFundsAgingView entity)
        {
            Create(entity);
        }

        public virtual IList<ActFundsAgingView> GetAllActFundsAgingView()
        {
            return FindAll<ActFundsAgingView>();
        }

        public virtual ActFundsAgingView LoadActFundsAgingView(String id)
        {
            return FindById<ActFundsAgingView>(id);
        }

        public virtual void UpdateActFundsAgingView(ActFundsAgingView entity)
        {
            Update(entity);
        }

        public virtual void DeleteActFundsAgingView(String id)
        {
            string hql = @"from ActFundsAgingView entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeleteActFundsAgingView(ActFundsAgingView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteActFundsAgingView(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from ActFundsAgingView entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteActFundsAgingView(IList<ActFundsAgingView> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (ActFundsAgingView entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteActFundsAgingView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
