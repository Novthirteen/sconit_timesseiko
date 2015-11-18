using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IOrderDetTracerViewBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateOrderDetTracerView(OrderDetTracerView entity);

        OrderDetTracerView LoadOrderDetTracerView(Int32 id);

        IList<OrderDetTracerView> GetAllOrderDetTracerView();
    
        void UpdateOrderDetTracerView(OrderDetTracerView entity);

        void DeleteOrderDetTracerView(Int32 id);
    
        void DeleteOrderDetTracerView(OrderDetTracerView entity);
    
        void DeleteOrderDetTracerView(IList<Int32> pkList);
    
        void DeleteOrderDetTracerView(IList<OrderDetTracerView> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
