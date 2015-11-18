using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public abstract class ActFundsAgingViewBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _id;
		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
        private string _transactionType;
        public string TransactionType
		{
			get
			{
                return _transactionType;
			}
			set
			{
                _transactionType = value;
			}
		}
        private com.Sconit.Entity.MasterData.BillAddress _billAddress;
        public com.Sconit.Entity.MasterData.BillAddress BillAddress
		{
			get
			{
                return _billAddress;
			}
			set
			{
                _billAddress = value;
			}
		}
		private com.Sconit.Entity.MasterData.Currency _currency;
		public com.Sconit.Entity.MasterData.Currency Currency
		{
			get
			{
				return _currency;
			}
			set
			{
				_currency = value;
			}
		}
		private string _status;
		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}
		private Decimal? _noBackwashAmount1;
		public Decimal? NoBackwashAmount1
		{
			get
			{
				return _noBackwashAmount1;
			}
			set
			{
				_noBackwashAmount1 = value;
			}
		}
		private Decimal? _totalBillDetailAmount1;
		public Decimal? TotalBillDetailAmount1
		{
			get
			{
				return _totalBillDetailAmount1;
			}
			set
			{
				_totalBillDetailAmount1 = value;
			}
		}
		private Decimal? _noBackwashAmount2;
		public Decimal? NoBackwashAmount2
		{
			get
			{
				return _noBackwashAmount2;
			}
			set
			{
				_noBackwashAmount2 = value;
			}
		}
		private Decimal? _totalBillDetailAmount2;
		public Decimal? TotalBillDetailAmount2
		{
			get
			{
				return _totalBillDetailAmount2;
			}
			set
			{
				_totalBillDetailAmount2 = value;
			}
		}
		private Decimal? _noBackwashAmount3;
		public Decimal? NoBackwashAmount3
		{
			get
			{
				return _noBackwashAmount3;
			}
			set
			{
				_noBackwashAmount3 = value;
			}
		}
		private Decimal? _totalBillDetailAmount3;
		public Decimal? TotalBillDetailAmount3
		{
			get
			{
				return _totalBillDetailAmount3;
			}
			set
			{
				_totalBillDetailAmount3 = value;
			}
		}
		private Decimal? _noBackwashAmount4;
		public Decimal? NoBackwashAmount4
		{
			get
			{
				return _noBackwashAmount4;
			}
			set
			{
				_noBackwashAmount4 = value;
			}
		}
		private Decimal? _totalBillDetailAmount4;
		public Decimal? TotalBillDetailAmount4
		{
			get
			{
				return _totalBillDetailAmount4;
			}
			set
			{
				_totalBillDetailAmount4 = value;
			}
		}
		private Decimal? _noBackwashAmount5;
		public Decimal? NoBackwashAmount5
		{
			get
			{
				return _noBackwashAmount5;
			}
			set
			{
				_noBackwashAmount5 = value;
			}
		}
		private Decimal? _totalBillDetailAmount5;
		public Decimal? TotalBillDetailAmount5
		{
			get
			{
				return _totalBillDetailAmount5;
			}
			set
			{
				_totalBillDetailAmount5 = value;
			}
		}
		private Decimal? _noBackwashAmount6;
		public Decimal? NoBackwashAmount6
		{
			get
			{
				return _noBackwashAmount6;
			}
			set
			{
				_noBackwashAmount6 = value;
			}
		}
		private Decimal? _totalBillDetailAmount6;
		public Decimal? TotalBillDetailAmount6
		{
			get
			{
				return _totalBillDetailAmount6;
			}
			set
			{
				_totalBillDetailAmount6 = value;
			}
		}
		private Decimal? _noBackwashAmount7;
		public Decimal? NoBackwashAmount7
		{
			get
			{
				return _noBackwashAmount7;
			}
			set
			{
				_noBackwashAmount7 = value;
			}
		}
		private Decimal? _totalBillDetailAmount7;
		public Decimal? TotalBillDetailAmount7
		{
			get
			{
				return _totalBillDetailAmount7;
			}
			set
			{
				_totalBillDetailAmount7 = value;
			}
		}
		private Decimal? _noBackwashAmount8;
		public Decimal? NoBackwashAmount8
		{
			get
			{
				return _noBackwashAmount8;
			}
			set
			{
				_noBackwashAmount8 = value;
			}
		}
		private Decimal? _totalBillDetailAmount8;
		public Decimal? TotalBillDetailAmount8
		{
			get
			{
				return _totalBillDetailAmount8;
			}
			set
			{
				_totalBillDetailAmount8 = value;
			}
		}
		private Decimal? _noBackwashAmount9;
		public Decimal? NoBackwashAmount9
		{
			get
			{
				return _noBackwashAmount9;
			}
			set
			{
				_noBackwashAmount9 = value;
			}
		}
		private Decimal? _totalBillDetailAmount9;
		public Decimal? TotalBillDetailAmount9
		{
			get
			{
				return _totalBillDetailAmount9;
			}
			set
			{
				_totalBillDetailAmount9 = value;
			}
		}
        
        #endregion

		public override int GetHashCode()
        {
			if (Id != null)
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            ActFundsAgingViewBase another = obj as ActFundsAgingViewBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.Id == another.Id);
            }
        } 
    }
	
}
