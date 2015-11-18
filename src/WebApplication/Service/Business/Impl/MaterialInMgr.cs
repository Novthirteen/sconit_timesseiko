using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Production;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    public class MaterialInMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }
        public IProductLineInProcessLocationDetailMgrE productLineInProcessLocationDetailMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IRoutingDetailMgrE routingDetailMgrE { get; set; }

        

        protected override void SetBaseInfo(Resolver resolver)
        {
            setBaseMgrE.FillResolverByFlow(resolver);
            if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                throw new BusinessErrorException("Flow.ShipReturn.Error.FlowTypeIsNotDistribution", resolver.OrderType);
            }
        }

        /// <summary>
        /// Todo:需要删除重复项?
        /// </summary>
        /// <param name="resolver"></param>
        protected override void GetDetail(Resolver resolver)
        {
            setDetailMgrE.SetMateria(resolver);
        }

        /// <summary>
        /// 仅校验投料的物料号,库位是否一致,不校验单位单包装等信息
        /// todo:不允许投入的又有数量又有Hu //可以前台控制
        /// </summary>
        /// <param name="resolver"></param>
        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanProductLineFirst");
            }
            LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(resolver.Input, resolver.UserCode);
            TransformerDetail transformerDetail = TransformerHelper.ConvertLocationLotDetailToTransformerDetail(locationLotDetail, false);
            var query = resolver.Transformers.Where
                    (t => (t.ItemCode == transformerDetail.ItemCode && t.LocationCode == transformerDetail.LocationCode));
            if (query.Count() < 1)
            {
                throw new BusinessErrorException("Warehouse.HuMatch.NotMatch", transformerDetail.HuId);
            }
            resolver.AddTransformerDetail(transformerDetail);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            IList<MaterialIn> materialInList = executeMgrE.ConvertTransformersToMaterialIns(resolver.Transformers);
            productLineInProcessLocationDetailMgrE.RawMaterialIn(resolver.Code, materialInList, userMgrE.CheckAndLoadUser(resolver.UserCode));
            resolver.Transformers = null;
            resolver.Result = languageMgrE.TranslateMessage("MasterData.MaterialIn.Successfully", resolver.UserCode, resolver.Code);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelRepackOperation(resolver);
        }
        
        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
        }
    }
}




﻿
#region Extend Class


namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class MaterialInMgrE : com.Sconit.Service.Business.Impl.MaterialInMgr, IBusinessMgrE
    {
        
    }
}

#endregion
