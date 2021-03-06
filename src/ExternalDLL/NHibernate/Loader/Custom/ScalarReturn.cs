using System;
using NHibernate.Type;

namespace NHibernate.Loader.Custom
{
	public class ScalarReturn : IReturn
	{
		private readonly IType type;
		private readonly string columnAlias;

		public ScalarReturn(IType type, string columnAlias)
		{
			this.type = type;
			this.columnAlias = columnAlias;
		}

		public IType Type
		{
			get { return type; }
		}

		public string ColumnAlias
		{
			get { return columnAlias; }
		}
	}
}