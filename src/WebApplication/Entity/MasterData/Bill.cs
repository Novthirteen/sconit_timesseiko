using System;
using System.Collections.Generic;
using System.Linq;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Bill : BillBase
    {
        #region Non O/R Mapping Properties
        public void AddBillDetail(BillDetail billDetail)
        {
            if (this.BillDetails == null)
            {
                this.BillDetails = new List<BillDetail>();
            }
            this.BillDetails.Add(billDetail);
        }

        public void UpdateAmount()
        {
            decimal billDetailAmount = 0;
            if (this.BillDetails != null)
            {
                foreach (BillDetail billDetailT in this.BillDetails)
                {
                    billDetailAmount += billDetailT.Amount;
                }

                //包含税，不包含折扣的总金额
                this.Amount = this.BillDetails.Sum(bd => bd.ListPrice * bd.BilledQty);
                //包含税和折扣的总金额
                this.AmountAfterDiscount = this.BillDetails.Sum(bd => bd.OrderAmount);
                
                if (!this.AmountAfterDiscount.HasValue || AmountAfterDiscount == 0)
                {
                    this.TotalBillDiscountRate = 0;
                }
                else
                {
                    this.TotalBillDiscountRate = (this.Discount.HasValue ? this.Discount.Value : 0) / AmountAfterDiscount * 100;
                }
            }
        }

        public void RemoveBillDetail(BillDetail billDetail)
        {
            if (this.BillDetails != null)
            {
                this.BillDetails.Remove(billDetail);
            }
        }

        public decimal NoBackwashAmount
        {
            get
            {
                decimal noBackwashAmount = 0;
                if (this.AmountAfterDiscount.HasValue)
                {
                    if (this.BackwashAmount.HasValue)
                    {
                        noBackwashAmount = this.AmountAfterDiscount.Value - this.BackwashAmount.Value;
                    }
                    else
                    {
                        noBackwashAmount = this.AmountAfterDiscount.Value;
                    }
                }
                return noBackwashAmount;
            }
        }

        private decimal _thisBackwashAmount;
        public decimal ThisBackwashAmount
        {
            get
            {
                return _thisBackwashAmount;
            }
            set
            {
                _thisBackwashAmount = value;
            }
        }

        /*
        public decimal Amount
        {
            get
            {
                decimal billDetailAmount = 0;
                if (this.BillDetails != null)
                {
                    foreach (BillDetail billDetail in this.BillDetails)
                    {
                        billDetailAmount += billDetail.Amount;
                    }
                }
                return billDetailAmount;
            }
        }

        public decimal AmountAfterDiscount
        {
            get
            {
                return this.Amount - (this.Discount.HasValue ? this.Discount.Value : 0);
            }
        }

        public decimal TotalBillDiscountRate
        {
            get
            {
                if (Amount == 0)
                {
                    return 0;
                }
                return (this.Discount.HasValue ? this.Discount.Value : 0) / Amount * 100;
            }
        }
         * */
        #endregion
    }
}