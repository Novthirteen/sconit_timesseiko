using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IInspectOrderMgr : IInspectOrderBaseMgr
    {
        #region Customized Methods

        InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo);

        InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo, bool includeDetail);

        InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, User user);

        InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, User user,string ipNo,string receiptNo,bool isSeperated);

        InspectOrder CreateInspectOrder(string locationCode, IDictionary<string,decimal> inspectItemList, User user);

        InspectOrder CreateInspectOrder(Location location, IDictionary<string, decimal> inspectItemList, User user);

        InspectOrder CreateFgInspectOrder(string locationCode, IDictionary<string, decimal> itemFgQtyDic, User user);

        void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, User user);

        void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, string userCode);

        InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail);
        
        InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail, bool isSort);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IInspectOrderMgrE : com.Sconit.Service.MasterData.IInspectOrderMgr
    {
        
    }
}

#endregion

#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IInspectOrderMgrE : com.Sconit.Service.MasterData.IInspectOrderMgr
    {
        
    }
}

#endregion
