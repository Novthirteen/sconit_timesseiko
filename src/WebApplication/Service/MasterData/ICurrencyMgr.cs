using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICurrencyMgr : ICurrencyBaseMgr
    {
        #region Customized Methods

        IList GetCurrency(string code, string name);
        Currency CheckAndLoadCurrency(string currencyCode);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICurrencyMgrE : com.Sconit.Service.MasterData.ICurrencyMgr
    {
        
    }
}

#endregion
