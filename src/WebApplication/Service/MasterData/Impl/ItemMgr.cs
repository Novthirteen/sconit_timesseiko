using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using System.Data;
using com.Sconit.Persistence;
using System.Text;
using com.Sconit.Utility;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemMgr : ItemBaseMgr, IItemMgr
    {
        private static IList<Item> cachedAllItem;
        private static DateTime cacheDateTime;
        private static string cachedAllItemString;
        private static long cachedAllItemCount;
        private static long cachedAllItemId;

        public IItemKitMgrE itemKitMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ISqlHelperDao sqlHelperDao { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetCacheAllItem()
        {
            if (cachedAllItem == null)
            {
                cachedAllItem = GetAllItem();
                cacheDateTime = DateTime.Now;
            }
            else
            {
                //检查Item大小是否发生变化
                //DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
                //criteria.Add(Expression.Eq("IsActive", true));
                //criteria.SetProjection(Projections.ProjectionList().Add(Projections.Count("Code")));
                //IList<int> count = this.criteriaMgrE.FindAll<int>(criteria);

                if (cacheDateTime < DateTime.Now.AddMinutes(-10))
                {
                    cachedAllItem = GetAllItem();
                    cacheDateTime = DateTime.Now;
                }
            }

            return cachedAllItem;
        }

        public Item GetCatchItem(string itemCode)
        {
            return GetCacheAllItem().FirstOrDefault(p => string.Equals(itemCode.Trim(), p.Code.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public string GetCacheAllItemString()
        {
            if (cachedAllItemString == null)
            {
                DoGetAllCacheItemString();
            }
            else
            {
                //检查Item大小是否发生变化
                DataSet ds = sqlHelperDao.GetDatasetBySql("select COUNT(1) as c, SUM(Id) as s from Item where IsActive = 1", null);
                long count = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                long sumId = long.Parse(ds.Tables[0].Rows[0][1].ToString());

                if (count != cachedAllItemCount || sumId != cachedAllItemId)
                {
                    DoGetAllCacheItemString();
                }
            }

            return cachedAllItemString;
        }

        private static object GetAllItemStringLock = new object();
        private void DoGetAllCacheItemString()
        {
            lock (GetAllItemStringLock)
            {
                //检查Item大小是否发生变化
                DataSet ds = sqlHelperDao.GetDatasetBySql("select COUNT(1) as c, SUM(Id) as s from Item where IsActive = 1", null);
                long count = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                long sumId = long.Parse(ds.Tables[0].Rows[0][1].ToString());

                if (count != cachedAllItemCount || sumId != cachedAllItemId)
                {
                    IList<Item> thisAllItem = GetAllItem();
                    StringBuilder data = new StringBuilder("[");
                    for (int i = 0; i < thisAllItem.Count; i++)
                    {
                        Item item = thisAllItem[i];
                        string desc = item.DescriptionAndSpec;
                        desc = desc.Replace("'", "");
                        data.Append(TextBoxHelper.GenSingleData(desc, item.Code) + (i < (thisAllItem.Count - 1) ? "," : string.Empty));
                    }
                    data.Append("]");

                    cachedAllItemString = data.ToString();
                    cachedAllItemCount = count;
                    cachedAllItemId = sumId;
                }
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetPMItem()
        {
            IList<Item> listItem = GetCacheAllItem();
            IList<Item> listPMItem = new List<Item>();
            foreach (Item item in listItem)
            {
                if (item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_M || item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_P)
                {
                    listPMItem.Add(item);
                }
            }

            return listPMItem;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetItem(DateTime lastModifyDate, int firstRow, int maxRows)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            criteria.AddOrder(Order.Asc("LastModifyDate"));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria, firstRow, maxRows);
            if (itemList.Count > 0)
            {
                return itemList;
            }
            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetItem(IList<string> itemCodeList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            if (itemCodeList.Count == 1)
            {
                criteria.Add(Expression.Eq("Code", itemCodeList[0]));
            }
            else
            {
                criteria.Add(Expression.InG<string>("Code", itemCodeList));
            }
            return criteriaMgrE.FindAll<Item>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public int GetItemCount(DateTime lastModifyDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria);
            return itemList.Count;
        }

        [Transaction(TransactionMode.Unspecified)]
        public int GetItemCount(string itemCategoryCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Item));
            criteria.Add(Expression.Eq("ItemCategory.Code", itemCategoryCode));
            IList<Item> itemList = criteriaMgrE.FindAll<Item>(criteria);
            return itemList.Count;
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(string code)
        {
            IList<ItemKit> itemKitList = itemKitMgrE.GetChildItemKit(code, true);
            itemKitMgrE.DeleteItemKit(itemKitList);

            base.DeleteItem(code);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(Item entity)
        {
            IList<ItemKit> itemKitList = itemKitMgrE.GetChildItemKit(entity, true);
            itemKitMgrE.DeleteItemKit(itemKitList);

            base.DeleteItem(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(IList<Item> entityList)
        {
            IList<ItemKit> itemKitList = new List<ItemKit>();
            foreach (Item item in entityList)
            {
                itemKitList = itemKitMgrE.GetChildItemKit(item, true);
                itemKitMgrE.DeleteItemKit(itemKitList);
            }

            base.DeleteItem(entityList);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItem(IList<string> pkList)
        {
            IList<ItemKit> itemKitList = new List<ItemKit>();
            foreach (string item in pkList)
            {
                itemKitList = itemKitMgrE.GetChildItemKit(item, true);
                itemKitMgrE.DeleteItemKit(itemKitList);
            }

            base.DeleteItem(pkList);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Item CheckAndLoadItem(string itemCode)
        {
            Item item = this.LoadItem(itemCode);
            if (item == null)
            {
                throw new BusinessErrorException("Item.Error.ItemCodeNotExist", itemCode);
            }

            return item;
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateItem(Item item, User user)
        {
            item.LastModifyDate = DateTime.Now;
            item.LastModifyUser = user;

            this.UpdateItem(item);
        }
        #endregion Customized Methods
    }
}


#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemMgrE : com.Sconit.Service.MasterData.Impl.ItemMgr, IItemMgrE
    {
        
    }
}
#endregion
