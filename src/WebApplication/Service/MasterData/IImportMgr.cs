using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.MasterData
{
    public interface IImportMgr
    {
        IList<ShiftPlanSchedule> ReadPSModelFromXls(Stream inputStream, User user, string regionCode, string flowCode, DateTime date, string shiftCode);

        IList<FlowPlan> ReadShipScheduleYFKFromXls(Stream inputStream, User user, string planType, string partyCode, string timePeriodType, DateTime date);

        IList<CycleCountDetail> ReadCycleCountFromXls(Stream inputStream, User user, CycleCount cycleCount);
    }
}





#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IImportMgrE : com.Sconit.Service.MasterData.IImportMgr
    {
       
    }
}

#endregion
