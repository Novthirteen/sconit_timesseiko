using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public class OrderDetTracerView : OrderDetTracerViewBase
    {
        #region Non O/R Mapping Properties

        public decimal ExcludeTaxPrice
        {
            get
            {
                decimal? unitPrice = decimal.Zero;
                if (this.UnitPriceAfterDiscount.HasValue)
                {
                    unitPrice = this.UnitPriceAfterDiscount;
                }
                else
                {
                    unitPrice = this.UnitPrice;
                }

                if (this.TaxCode != null)
                {
                    if (!this.IsIncludeTax)
                    {
                        return unitPrice.Value;
                    }
                    else
                    {
                        return (unitPrice / (decimal.Parse(this.TaxCode) / 100 + 1)).Value;
                    }
                }
                else
                {
                    return unitPrice.HasValue ? unitPrice.Value : decimal.Zero;
                }
            }
        }

        public decimal IncludeTaxPrice
        {
            get
            {
                decimal? unitPrice = decimal.Zero;
                if (this.UnitPriceAfterDiscount.HasValue)
                {
                    unitPrice = this.UnitPriceAfterDiscount;
                }
                else
                {
                    if (this.UnitPrice.HasValue)
                    {
                        unitPrice = this.UnitPrice;
                    }
                    else
                    {

                        return decimal.Zero;
                    }
                }

                if (this.TaxCode != null && !this.IsIncludeTax)
                {
                    return (unitPrice * (decimal.Parse(this.TaxCode) / 100 + 1)).Value;
                }
                else
                {
                    return unitPrice.HasValue ? unitPrice.Value : decimal.Zero;
                }
            }
        }
        public decimal IncludeTaxTotalPrice
        {
            get
            {
                if (this.OrderedQty != 0)
                {
                    return this.IncludeTaxPrice * this.OrderedQty;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }
        public decimal ExcludeTaxTotalPrice
        {
            get
            {
                if (this.OrderedQty != 0)
                {
                    return this.ExcludeTaxPrice * this.OrderedQty;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }

        #endregion
    }
}