using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class CurrencyMgr : CurrencyBaseMgr, ICurrencyMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Currency CheckAndLoadCurrency(string currencyCode)
        {
            Currency currency = this.LoadCurrency(currencyCode);
            if (currency == null)
            {
                throw new BusinessErrorException("Currency.Error.CurrencyCodeNotExist", currencyCode);
            }

            return currency;
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList GetCurrency(string code, string name)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Currency));
            if (code != string.Empty && code != null)
                criteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            if (name != string.Empty && name != null)
                criteria.Add(Expression.Like("Name", name, MatchMode.Anywhere));
            return criteriaMgrE.FindAll(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class CurrencyMgrE : com.Sconit.Service.MasterData.Impl.CurrencyMgr, ICurrencyMgrE
    {
        
    }
}
#endregion
