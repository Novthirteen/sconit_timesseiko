using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IPartyMgr : IPartyBaseMgr
    {
        #region Customized Methods

        IList<Party> GetFromParty(string orderType, string userCode);

        IList<Party> GetFromParty(string orderType, string userCode, bool includeInactive);

        IList<Party> GetToParty(string orderType, string userCode);

        IList<Party> GetToParty(string orderType, string userCode, bool includeInactive);

        IList<Party> GetAllParty(string type);

        IList<Party> GetAllParty(string userCode, string type);

        IList<Party> GetOrderFromParty(string orderType, string userCode);

        IList<Party> GetOrderFromParty(string orderType, string userCode, bool includeInactive);

        IList<Party> GetOrderToParty(string orderType, string userCode);

        IList<Party> GetOrderToParty(string orderType, string userCode, bool includeInactive);

        Party CheckAndLoadParty(string partyCode);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPartyMgrE : com.Sconit.Service.MasterData.IPartyMgr
    {
        
    }
}

#endregion
