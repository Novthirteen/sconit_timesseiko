using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;

namespace com.Sconit.Service.Batch.Job
{
    public class OrderCompleteJob : IJob
    {
         public IOrderMgrE orderMgrE { get; set; }

       

         [Transaction(TransactionMode.Unspecified)]
         public void Execute(JobRunContext context)
         {
             string flowCode = context.JobDataMap.GetStringValue("FlowCode");
             if (flowCode != null && flowCode != string.Empty)
             {
                 string[] flowCodeArray = flowCode.Split(',');
                 orderMgrE.TryCompleteOrder(flowCodeArray);
             }
         }
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.Batch.Job
{
    public partial class OrderCompleteJob : com.Sconit.Service.Batch.Job.OrderCompleteJob
    {
        public OrderCompleteJob()
        {
        }
    }
}

#endregion
