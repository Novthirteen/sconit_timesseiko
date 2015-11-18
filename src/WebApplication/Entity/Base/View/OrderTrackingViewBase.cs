using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public abstract class OrderTrackingViewBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private Int32 _id;
		public Int32 Id
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
        private com.Sconit.Entity.MasterData.OrderHead _orderHead;
        public com.Sconit.Entity.MasterData.OrderHead OrderHead
		{
			get
			{
                return _orderHead;
			}
			set
			{
                _orderHead = value;
			}
		}
		private com.Sconit.Entity.MasterData.Item _item;
		public com.Sconit.Entity.MasterData.Item Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}
        private Int32 _sequence;
        public Int32 Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }
		private DateTime? _windowTime;
		public DateTime? WindowTime
		{
			get
			{
				return _windowTime;
			}
			set
			{
				_windowTime = value;
			}
		}
        private DateTime? _minReceiptDate;
        public DateTime? MinReceiptDate
		{
			get
			{
                return _minReceiptDate;
			}
			set
			{
                _minReceiptDate = value;
			}
		}
        private DateTime? _maxReceiptDate;
        public DateTime? MaxReceiptDate
		{
			get
			{
                return _maxReceiptDate;
			}
			set
			{
                _maxReceiptDate = value;
			}
		}
        private DateTime? _effectiveDate;
        public DateTime? EffectiveDate
        {
            get
            {
                return _effectiveDate;
            }
            set
            {
                _effectiveDate = value;
            }
        }
		private Decimal? _orderQty;
		public Decimal? OrderQty
		{
			get
			{
				return _orderQty;
			}
			set
			{
				_orderQty = value;
			}
		}
		private Decimal? _recQty;
		public Decimal? RecQty
		{
			get
			{
				return _recQty;
			}
			set
			{
				_recQty = value;
			}
		}
		private Decimal? _noRecQty;
		public Decimal? NoRecQty
		{
			get
			{
				return _noRecQty;
			}
			set
			{
				_noRecQty = value;
			}
		}
		private Decimal? _billAmount;
		public Decimal? BillAmount
		{
			get
			{
				return _billAmount;
			}
			set
			{
				_billAmount = value;
			}
		}
        private Decimal? _billedAmount;
        public Decimal? BilledAmount
        {
            get
            {
                return _billedAmount;
            }
            set
            {
                _billedAmount = value;
            }
        }
		private Decimal? _noBilledAmount;
		public Decimal? NoBilledAmount
		{
			get
			{
				return _noBilledAmount;
			}
			set
			{
				_noBilledAmount = value;
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
            OrderTrackingViewBase another = obj as OrderTrackingViewBase;

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
