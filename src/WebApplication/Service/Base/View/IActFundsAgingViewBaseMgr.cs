using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IActFundsAgingViewBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateActFundsAgingView(ActFundsAgingView entity);

        ActFundsAgingView LoadActFundsAgingView(String id);

        IList<ActFundsAgingView> GetAllActFundsAgingView();
    
        void UpdateActFundsAgingView(ActFundsAgingView entity);

        void DeleteActFundsAgingView(String id);
    
        void DeleteActFundsAgingView(ActFundsAgingView entity);
    
        void DeleteActFundsAgingView(IList<String> pkList);
    
        void DeleteActFundsAgingView(IList<ActFundsAgingView> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
