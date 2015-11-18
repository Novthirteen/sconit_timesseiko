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
    public class NHPaymentBaseDao : NHDaoBase, IPaymentBaseDao
    {
        public NHPaymentBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Method Created By CodeSmith

        public virtual void CreatePayment(Payment entity)
        {
            Create(entity);
        }

        public virtual IList<Payment> GetAllPayment()
        {
            return FindAll<Payment>();
        }

        public virtual Payment LoadPayment(String paymentNo)
        {
            return FindById<Payment>(paymentNo);
        }

        public virtual void UpdatePayment(Payment entity)
        {
            Update(entity);
        }

        public virtual void DeletePayment(String paymentNo)
        {
            string hql = @"from Payment entity where entity.PaymentNo = ?";
            Delete(hql, new object[] { paymentNo }, new IType[] { NHibernateUtil.String });
        }

        public virtual void DeletePayment(Payment entity)
        {
            Delete(entity);
        }

        public virtual void DeletePayment(IList<String> pkList)
        {
            StringBuilder hql = new StringBuilder();
            hql.Append("from Payment entity where entity.PaymentNo in (");
            hql.Append(pkList[0]);
            for (int i = 1; i < pkList.Count; i++)
            {
                hql.Append(",");
                hql.Append(pkList[i]);
            }
            hql.Append(")");

            Delete(hql.ToString());
        }

        public virtual void DeletePayment(IList<Payment> entityList)
        {
            IList<String> pkList = new List<String>();
            foreach (Payment entity in entityList)
            {
                pkList.Add(entity.PaymentNo);
            }

            DeletePayment(pkList);
        }


        #endregion Method Created By CodeSmith
    }
}
