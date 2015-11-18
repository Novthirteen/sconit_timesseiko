using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;

public partial class MasterData_Item_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
        if (actionParameter.ContainsKey("Desc"))
        {
            this.tbDesc.Text = actionParameter["Desc"];
        }
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text.Trim();
        string desc = this.tbDesc.Text.Trim();
        string brand = this.tbBrand.Text.Trim();
        string spec = this.tbSpec.Text.Trim();
        bool isActive = this.cbIsActive.Checked;
        bool showImage = this.cbShowImage.Checked;

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Item));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Item)).SetProjection(Projections.Count("Code"));
            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            }

            if (desc != string.Empty)
            {
                selectCriteria.Add(
                    Expression.Or(
                    Expression.Or(
                    Expression.Like("Desc1", desc, MatchMode.Anywhere), Expression.Like("Desc2", desc, MatchMode.Anywhere)
                   ), Expression.Like("TextField1", desc, MatchMode.Anywhere))
                   );

                selectCountCriteria.Add(
                    Expression.Or(
                    Expression.Or(
                    Expression.Like("Desc1", desc, MatchMode.Anywhere), Expression.Like("Desc2", desc, MatchMode.Anywhere)
                   ), Expression.Like("TextField1", desc, MatchMode.Anywhere))
                   );
            }

            if (brand != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Brand", brand, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Brand", brand, MatchMode.Anywhere));
            }

            if (spec != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Spec", spec, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Spec", spec, MatchMode.Anywhere));
            }

            selectCriteria.Add(Expression.Eq("IsActive", isActive));
            selectCountCriteria.Add(Expression.Eq("IsActive", isActive));

            SearchEvent((new object[] { selectCriteria, selectCountCriteria, showImage }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }
}
