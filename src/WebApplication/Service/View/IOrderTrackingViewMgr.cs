using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IOrderTrackingViewMgr : IOrderTrackingViewBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.View
{
    public partial interface IOrderTrackingViewMgrE : com.Sconit.Service.View.IOrderTrackingViewMgr
    {
    }
}

#endregion Extend Interface