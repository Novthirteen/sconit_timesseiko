using com.Sconit.Service.Ext.MasterData;


using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class BillAddressMgr : BillAddressBaseMgr, IBillAddressMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class





namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class BillAddressMgrE : com.Sconit.Service.MasterData.Impl.BillAddressMgr, IBillAddressMgrE
    {
        
    }
}
#endregion
