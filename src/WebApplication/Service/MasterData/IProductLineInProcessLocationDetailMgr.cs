using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.Production;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IProductLineInProcessLocationDetailMgr : IProductLineInProcessLocationDetailBaseMgr
    {
        #region Customized Methods

        IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetail(string prodLineCode, string status);

        void RawMaterialIn(string prodLineCode, IList<MaterialIn> materialInList, User user);

        void RawMaterialIn(Flow prodLine, IList<MaterialIn> materialInList, User user);

        void RawMaterialBackflush(string prodLineCode, User user);

        void RawMaterialBackflush(Flow prodLine, User user);

        void RawMaterialBackflush(string prodLineCode, IDictionary<string, decimal> itemQtydic, User user);

        void RawMaterialBackflush(Flow prodLine, IDictionary<string, decimal> itemQtydic, User user);

        IList<ProductLineInProcessLocationDetail> GetProductLineInProcessLocationDetailGroupByItem(string prodLineCode, string status);

        #endregion Customized Methods
    }
}





#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IProductLineInProcessLocationDetailMgrE : com.Sconit.Service.MasterData.IProductLineInProcessLocationDetailMgr
    {
        
    }
}

#endregion
