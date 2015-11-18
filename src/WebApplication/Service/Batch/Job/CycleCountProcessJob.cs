using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;

namespace com.Sconit.Service.Batch.Job
{
    [Transactional]
    public class CycleCountProcessJob : IJob
    {
        public ICycleCountMgrE cycleCountMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }

        

        public void Execute(JobRunContext context)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CycleCount));
            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
            criteria.Add(Expression.Not(Expression.Eq("CreateUser", "su")));
            IList<CycleCount> cycleCountList = criteriaMgrE.FindAll<CycleCount>(criteria);

            if (cycleCountList.Count > 0)
            {
                int count = 0;
                foreach (CycleCount cycleCount in cycleCountList)
                {
                    if (count < 10)
                    {
                        cycleCountMgrE.ProcessCycleCountResult(cycleCount.Code, userMgrE.GetMonitorUser());
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void Process(CycleCount c)
        {
            cycleCountMgrE.ProcessCycleCountResult(c.Code, userMgrE.GetMonitorUser());
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Batch.Job
{
    [Transactional]
    public partial class CycleCountProcessJob : com.Sconit.Service.Batch.Job.CycleCountProcessJob
    {

    }
}

#endregion
