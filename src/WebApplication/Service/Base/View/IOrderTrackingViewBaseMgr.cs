using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IOrderTrackingViewBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateOrderTrackingView(OrderTrackingView entity);

        OrderTrackingView LoadOrderTrackingView(Int32 id);

        IList<OrderTrackingView> GetAllOrderTrackingView();
    
        void UpdateOrderTrackingView(OrderTrackingView entity);

        void DeleteOrderTrackingView(Int32 id);
    
        void DeleteOrderTrackingView(OrderTrackingView entity);
    
        void DeleteOrderTrackingView(IList<Int32> pkList);
    
        void DeleteOrderTrackingView(IList<OrderTrackingView> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
