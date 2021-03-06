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
    public class NHBillDetViewBaseDao : NHDaoBase, IBillDetViewBaseDao
    {
        public NHBillDetViewBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateBillDetView(BillDetView entity)
        {
            Create(entity);
        }

        public virtual IList<BillDetView> GetAllBillDetView()
        {
            return FindAll<BillDetView>();
        }

        public virtual BillDetView LoadBillDetView(Int32 id)
        {
            return FindById<BillDetView>(id);
        }

        public virtual void UpdateBillDetView(BillDetView entity)
        {
            Update(entity);
        }

        public virtual void DeleteBillDetView(Int32 id)
        {
            string hql = @"from BillDetView entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteBillDetView(BillDetView entity)
        {
            Delete(entity);
        }

        public virtual void DeleteBillDetView(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from BillDetView entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteBillDetView(IList<BillDetView> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (BillDetView entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteBillDetView(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
