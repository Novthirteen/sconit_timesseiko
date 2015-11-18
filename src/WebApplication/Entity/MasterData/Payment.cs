using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Payment : PaymentBase
    {
        #region Non O/R Mapping Properties

        public decimal NoBackwashAmount
        {
            get
            {
                decimal noBackwashAmount = 0;
                if (this.BackwashAmount.HasValue)
                {
                    noBackwashAmount = this.Amount - this.BackwashAmount.Value;
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


        #endregion
    }
}