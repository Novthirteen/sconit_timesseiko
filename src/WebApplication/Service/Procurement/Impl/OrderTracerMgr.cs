using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Procurement;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using LeanEngine.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Procurement.Impl
{
    [Transactional]
    public class OrderTracerMgr : OrderTracerBaseMgr, IOrderTracerMgr
    {

        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods


        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderTracerByOrderLocationTransaction(IList<int> olt)
        {
            if ((olt == null) || (olt.Count == 0))
            {
                return;
            }

            StringBuilder hql = new StringBuilder("From com.Sconit.Entity.Procurement.OrderTracer where RefOrderLocTransId in ( ");

            for (int i = 0; i < olt.Count;i++ )
            {
                if (i != 0)
                    hql.Append(",");
                hql.Append(olt[i]);

            }
            hql.Append(" )");

            criteriaMgrE.DeleteWithHql(hql.ToString());
                      
        }   

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Procurement.Impl
{
    [Transactional]
    public partial class OrderTracerMgrE : com.Sconit.Service.Procurement.Impl.OrderTracerMgr, IOrderTracerMgrE
    {
    }
}

#endregion Extend Class