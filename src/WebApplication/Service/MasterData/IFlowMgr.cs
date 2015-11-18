using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IFlowMgr : IFlowBaseMgr
    {
        #region Customized Methods

        Flow LoadFlow(string code, bool includeFlowDetail);

        Flow LoadFlow(string flowCode, string userCode);

        Flow LoadFlow(string code, bool includeFlowDetail, bool includeRefDetail);

        Flow CheckAndLoadFlow(string code);

        Flow CheckAndLoadFlow(string code, bool includeFlowDetail);

        Flow CheckAndLoadFlow(string code, bool includeFlowDetail, bool includeRefDetail);

        IList<Flow> GetProcurementFlow(string userCode);

        IList<Flow> GetProcurementFlow(string userCode, string partyAuthrizeOpt);

        IList<Flow> GetDistributionFlow(string userCode);

        IList<Flow> GetProductionFlow(string userCode);

        IList<Flow> GetTransferFlow(string userCode, string partyAuthrizeOpt);

        IList<Flow> GetTransferFlow(string userCode);

        IList<Flow> GetCustomerGoodsFlow(string userCode);

        IList<Flow> GetSubconctractingFlow(string userCode);

        IList<string> FindWinTime(Flow flow, DateTime date);

        IList<string> FindWinTime(string flowCode, DateTime date);

        IList<Flow> GetAllFlow(string userCode);

        IList<Flow> GetFlowList(string userCode, bool includeProcurement, bool includeDistribution, bool includeTransfer, bool includeProduction, bool includeCustomerGoods, bool includeSubconctracting, string orderAuthrizeOpt);

        IList<Flow> GetAllFlow(DateTime lastModifyDate, int firstRow, int maxRows);

        IList<BomDetail> GetBatchFeedBomDetail(Flow flow);

        IList<BomDetail> GetBatchFeedBomDetail(string flowCode);

        //Flow CheckAndLoadTransferFlow(Location locationFrom, Location locationTo, Hu hu, string userCode);

        FlowView CheckAndLoadFlowView(string flowCode, string userCode, string locationFromCode, string locationToCode, Hu hu, List<string> flowTypes);

        //FlowView CheckAndLoadFlowView(string flowCode, string userCode, Location locationFrom, Location locationTo, Hu hu, params string[] flowTypes);


        #endregion Customized Methods
    }
}



#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IFlowMgrE : com.Sconit.Service.MasterData.IFlowMgr
    {
       
    }
}

#endregion

#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IFlowMgrE : com.Sconit.Service.MasterData.IFlowMgr
    {
       
    }
}

#endregion
