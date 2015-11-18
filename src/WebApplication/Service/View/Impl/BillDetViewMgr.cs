using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class BillDetViewMgr : BillDetViewBaseMgr, IBillDetViewMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class BillDetViewMgrE : com.Sconit.Service.View.Impl.BillDetViewMgr, IBillDetViewMgrE
    {
    }
}

#endregion Extend Class