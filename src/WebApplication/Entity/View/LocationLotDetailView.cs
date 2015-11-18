using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public class LocationLotDetailView : LocationLotDetailViewBase
    {
        #region Non O/R Mapping Properties

        public Currency Currency;

        public decimal Amount;

        #endregion
    }
}