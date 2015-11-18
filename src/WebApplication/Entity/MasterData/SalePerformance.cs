using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class SalePerformance : EntityBase
    {
        #region Non O/R Mapping Properties

        public Int32? Id { get; set; }
        public Decimal OrderedQty { get; set; }
        public BillAddress BillFrom { get; set; }
        public BillAddress BillTo { get; set; }
        public Item Item { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public Decimal? UnitPrice { get; set; }
        public Decimal? UnitPriceAfterDiscount { get; set; }
        public Decimal IncludeTaxPrice { get; set; }
        public Decimal IncludeTaxTotalPrice { get; set; }
        public Currency Currency { get; set; }
        public Party PartyFrom { get; set; }
        public Party PartyTo { get; set; }
        public User CreateUser { get; set; }
        #endregion
    }
}