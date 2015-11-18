using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface ISalesOrderTrackingViewBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateSalesOrderTrackingView(SalesOrderTrackingView entity);

        SalesOrderTrackingView LoadSalesOrderTrackingView(Int32 id);

        IList<SalesOrderTrackingView> GetAllSalesOrderTrackingView();
    
        void UpdateSalesOrderTrackingView(SalesOrderTrackingView entity);

        void DeleteSalesOrderTrackingView(Int32 id);
    
        void DeleteSalesOrderTrackingView(SalesOrderTrackingView entity);
    
        void DeleteSalesOrderTrackingView(IList<Int32> pkList);
    
        void DeleteSalesOrderTrackingView(IList<SalesOrderTrackingView> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
