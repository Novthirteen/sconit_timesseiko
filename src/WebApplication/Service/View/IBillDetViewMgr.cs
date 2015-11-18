using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IBillDetViewMgr : IBillDetViewBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.View
{
    public partial interface IBillDetViewMgrE : com.Sconit.Service.View.IBillDetViewMgr
    {
    }
}

#endregion Extend Interface