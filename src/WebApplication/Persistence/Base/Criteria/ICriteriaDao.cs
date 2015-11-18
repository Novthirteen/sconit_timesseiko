using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using NHibernate.Type;

namespace com.Sconit.Persistence.Criteria
{
    public interface ICriteriaDao
    {
        IList FindAll(DetachedCriteria criteria);

        IList FindAll(DetachedCriteria criteria, int firstRow, int maxRows);

        IList<T> FindAll<T>(DetachedCriteria criteria);

        IList<T> FindAll<T>(DetachedCriteria criteria, int firstRow, int maxRows);

        IList<T> FindAllWithHql<T>(string hql);
        IList<T> FindAllWithHql<T>(string hql, object value);
        IList<T> FindAllWithHql<T>(string hql, object[] values);
        IList<T> FindAllWithHql<T>(string hql, int firstRow, int maxRows);
        IList<T> FindAllWithHql<T>(string hql, object value, int firstRow, int maxRows);
        IList<T> FindAllWithHql<T>(string hql, object[] values, int firstRow, int maxRows);
        void DeleteWithHql(string hql);
        void DeleteWithHql(string hql, object value, IType type);
        void DeleteWithHql(string hql, object[] values, IType[] types);
    }
}
