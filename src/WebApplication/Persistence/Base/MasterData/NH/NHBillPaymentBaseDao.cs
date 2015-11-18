using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHBillPaymentBaseDao : NHDaoBase, IBillPaymentBaseDao
    {
        public NHBillPaymentBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreateBillPayment(BillPayment entity)
        {
            Create(entity);
        }

        public virtual IList<BillPayment> GetAllBillPayment()
        {
            return FindAll<BillPayment>();
        }

        public virtual BillPayment LoadBillPayment(Int32 id)
        {
            return FindById<BillPayment>(id);
        }

        public virtual void UpdateBillPayment(BillPayment entity)
        {
            Update(entity);
        }

        public virtual void DeleteBillPayment(Int32 id)
        {
            string hql = @"from BillPayment entity where entity.Id = ?";
            Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.Int32 });
        }

        public virtual void DeleteBillPayment(BillPayment entity)
        {
            Delete(entity);
        }

        public virtual void DeleteBillPayment(IList<Int32> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from BillPayment entity where entity.Id in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeleteBillPayment(IList<BillPayment> entityList)
        {
            IList<Int32> pkList = new List<Int32>();
            foreach (BillPayment entity in entityList)
            {
                pkList.Add(entity.Id);
            }

            DeleteBillPayment(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
