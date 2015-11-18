using System;
using com.Sconit.Entity.MasterData;
using System.Collections;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IItemCategoryMgr : IItemCategoryBaseMgr
    {
        #region Customized Methods

        ItemCategory CheckAndLoadItemCategory(string itemCategoryCode);
        IList GetItemCategory(string code, string desc1);
        IList GetItemCategory(string code, string desc1, string desc2);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IItemCategoryMgrE : com.Sconit.Service.MasterData.IItemCategoryMgr
    {
    }
}

#endregion Extend Interface