using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IActFundsAgingViewMgr : IActFundsAgingViewBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.View
{
    public partial interface IActFundsAgingViewMgrE : com.Sconit.Service.View.IActFundsAgingViewMgr
    {
    }
}

#endregion Extend Interface