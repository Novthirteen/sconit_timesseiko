using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

namespace com.Sconit.Service.Procurement
{
    public interface ILeanEngineMgr
    {
        #region 2G
        //void GenerateOrder();

        //IList<OrderHead> GenerateOrder(OrderHead orderTemplate);

        //IList<OrderHead> GenerateOrder(OrderHead orderTemplate, bool isUrgent);

        //void GetOrderDetailReqQty(OrderDetail orderDetail, bool isUrgent);

        //void UpdateAbnormalNextOrderTime();
        #endregion
        
        //3G
        void OrderGenerate();

        List<LeanEngine.Entity.Orders> GenerateLeanEngineOrder(string flowCode, string customerCode, string orderNo, DateTime? startDate, DateTime? endDate);

        //OrderHead PreviewGenOrder(string flowCode);

        OrderHead PreviewGenOrder(string flowCode, string strategy, DateTime? windowTime);

        void CreateOrder(OrderHead order, string userCode);
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.Procurement
{
    public partial interface ILeanEngineMgrE : com.Sconit.Service.Procurement.ILeanEngineMgr
    {
      
    }
}

#endregion
