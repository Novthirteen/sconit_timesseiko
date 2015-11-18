using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public abstract class SalesOrderTrackingViewBase : EntityBase
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
		private Int32? _sequence;
		public Int32? Sequence
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
        private DateTime? _minDeliverDate;
        public DateTime? MinDeliverDate
		{
			get
			{
                return _minDeliverDate;
			}
			set
			{
                _minDeliverDate = value;
			}
		}
        private DateTime? _maxDeliverDate;
        public DateTime? MaxDeliverDate
		{
			get
			{
				return _maxDeliverDate;
			}
			set
			{
				_maxDeliverDate = value;
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
        private Decimal? _deliverQty;
		public Decimal? DeliverQty
		{
			get
			{
                return _deliverQty;
			}
			set
			{
                _deliverQty = value;
			}
		}
        private Decimal? _noDeliverQty;
        public Decimal? NoDeliverQty
		{
			get
			{
                return _noDeliverQty;
			}
			set
			{
                _noDeliverQty = value;
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
        private Decimal? _orderedQty;
        public Decimal? OrderedQty
		{
			get
			{
                return _orderedQty;
			}
			set
			{
                _orderedQty = value;
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
            SalesOrderTrackingViewBase another = obj as SalesOrderTrackingViewBase;

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
