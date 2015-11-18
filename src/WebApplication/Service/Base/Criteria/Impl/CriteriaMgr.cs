using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using com.Sconit.Persistence.Criteria;
using NHibernate.Expression;
using NHibernate.Type;

namespace com.Sconit.Service.Criteria.Impl
{
    public class CriteriaMgr : SessionBase, ICriteriaMgr
    {
        public ICriteriaDao _criteriaDao {get;set; }

        public IList FindAll(DetachedCriteria criteria)
        {
            return _criteriaDao.FindAll(criteria);
        }

        public IList FindAll(DetachedCriteria criteria, int firstRow, int maxRows)
        {
            return _criteriaDao.FindAll(criteria, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(DetachedCriteria criteria)
        {
            return _criteriaDao.FindAll<T>(criteria);
        }

        public IList<T> FindAll<T>(DetachedCriteria criteria, int firstRow, int maxRows)
        {
            return _criteriaDao.FindAll<T>(criteria, firstRow, maxRows);
        }

        public IList<T> FindAllWithHql<T>(string hql)
        {
            return _criteriaDao.FindAllWithHql<T>(hql);
        }

        public IList<T> FindAllWithHql<T>(string hql, object value)
        {
            return _criteriaDao.FindAllWithHql<T>(hql, value);
        }

        public IList<T> FindAllWithHql<T>(string hql, object[] values)
        {
            return _criteriaDao.FindAllWithHql<T>(hql, values);
        }

        public IList<T> FindAllWithHql<T>(string hql, int firstRow, int maxRows)
        {
            return _criteriaDao.FindAllWithHql<T>(hql, firstRow, maxRows);
        }

        public IList<T> FindAllWithHql<T>(string hql, object value, int firstRow, int maxRows)
        {
            return _criteriaDao.FindAllWithHql<T>(hql, value, firstRow, maxRows);
        }

        public IList<T> FindAllWithHql<T>(string hql, object[] values, int firstRow, int maxRows)
        {
            return _criteriaDao.FindAllWithHql<T>(hql, values, firstRow, maxRows);
        }

        public void DeleteWithHql(string hql)
        {
            _criteriaDao.DeleteWithHql(hql);
        }

        public void DeleteWithHql(string hql, object[] values, IType[] types)
        {
            _criteriaDao.DeleteWithHql(hql, values, types);
        }

        public void DeleteWithHql(string hql, object value, IType type)
        {
            _criteriaDao.DeleteWithHql(hql, value, type);
        }

    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Criteria.Impl
{
    public partial class CriteriaMgrE : com.Sconit.Service.Criteria.Impl.CriteriaMgr, ICriteriaMgrE
    {
    }
}

#endregion
