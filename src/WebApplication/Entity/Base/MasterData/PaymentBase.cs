using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class PaymentBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _paymentNo;
		public string PaymentNo
		{
			get
			{
				return _paymentNo;
			}
			set
			{
				_paymentNo = value;
			}
		}
        private Party _party;
        [System.Xml.Serialization.XmlElement(Type = typeof(Supplier)), System.Xml.Serialization.XmlElement(Type = typeof(Region)), System.Xml.Serialization.XmlElement(Type = typeof(Customer))]
        public Party Party
        {
            get
            {
                return _party;
            }
            set
            {
                _party = value;
            }
        }
		private string _extPaymentNo;
		public string ExtPaymentNo
		{
			get
			{
				return _extPaymentNo;
			}
			set
			{
				_extPaymentNo = value;
			}
		}
		private string _refPaymentNo;
		public string RefPaymentNo
		{
			get
			{
				return _refPaymentNo;
			}
			set
			{
				_refPaymentNo = value;
			}
		}
		private string _invoiceNo;
		public string InvoiceNo
		{
			get
			{
				return _invoiceNo;
			}
			set
			{
				_invoiceNo = value;
			}
		}
        private string _voucherNo;
        public string VoucherNo
        {
            get
            {
                return _voucherNo;
            }
            set
            {
                _voucherNo = value;
            }
        }
		private Decimal _amount;
		public Decimal Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
			}
		}
		private Decimal? _backwashAmount;
		public Decimal? BackwashAmount
		{
			get
			{
				return _backwashAmount;
			}
			set
			{
				_backwashAmount = value;
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
		private string _transType;
		public string TransType
		{
			get
			{
				return _transType;
			}
			set
			{
				_transType = value;
			}
		}
		private string _payType;
		public string PayType
		{
			get
			{
				return _payType;
			}
			set
			{
				_payType = value;
			}
		}
		private DateTime _createDate;
		public DateTime CreateDate
		{
			get
			{
				return _createDate;
			}
			set
			{
				_createDate = value;
			}
		}
        private DateTime _paymentDate;
        public DateTime PaymentDate
        {
            get
            {
                return _paymentDate;
            }
            set
            {
                _paymentDate = value;
            }
        }
		private com.Sconit.Entity.MasterData.User _createUser;
		public com.Sconit.Entity.MasterData.User CreateUser
		{
			get
			{
				return _createUser;
			}
			set
			{
				_createUser = value;
			}
		}
		private DateTime _lastModifyDate;
		public DateTime LastModifyDate
		{
			get
			{
				return _lastModifyDate;
			}
			set
			{
				_lastModifyDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _lastModifyUser;
		public com.Sconit.Entity.MasterData.User LastModifyUser
		{
			get
			{
				return _lastModifyUser;
			}
			set
			{
				_lastModifyUser = value;
			}
		}

        private IList<BillPayment> _billPayments;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<BillPayment> BillPayments
        {
            get
            {
                return _billPayments;
            }
            set
            {
                _billPayments = value;
            }
        }
        
        #endregion

		public override int GetHashCode()
        {
			if (PaymentNo != null)
            {
                return PaymentNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            PaymentBase another = obj as PaymentBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.PaymentNo == another.PaymentNo);
            }
        } 
    }
	
}
