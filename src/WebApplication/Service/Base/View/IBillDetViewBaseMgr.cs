using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IBillDetViewBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateBillDetView(BillDetView entity);

        BillDetView LoadBillDetView(Int32 id);

        IList<BillDetView> GetAllBillDetView();
    
        void UpdateBillDetView(BillDetView entity);

        void DeleteBillDetView(Int32 id);
    
        void DeleteBillDetView(BillDetView entity);
    
        void DeleteBillDetView(IList<Int32> pkList);
    
        void DeleteBillDetView(IList<BillDetView> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}
