using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    public class InspectionMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }

        

        protected override void SetBaseInfo(Resolver resolver)
        {
        }

        protected override void GetDetail(Resolver resolver)
        {
        }

        protected override void SetDetail(Resolver resolver)
        {
            LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(resolver.Input, resolver.UserCode);
            TransformerDetail transformerDetail = TransformerHelper.ConvertLocationLotDetailToTransformerDetail(locationLotDetail, false);
            resolver.AddTransformerDetail(transformerDetail);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            this.CreateInspectOrder(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
        }

        /// <summary>
        /// 报验单
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        [Transaction(TransactionMode.Unspecified)]
        public void CreateInspectOrder(Resolver resolver)
        {
            IList<LocationLotDetail> locationLotDetailList = executeMgrE.ConvertTransformersToLocationLotDetails(resolver.Transformers, false);
            if (locationLotDetailList.Count == 0)
            {
                throw new BusinessErrorException("MasterData.Inventory.Repack.Error.RepackDetailEmpty");
            }
            User user = userMgrE.LoadUser(resolver.UserCode, false, true);
            InspectOrder inspectOrder = inspectOrderMgrE.CreateInspectOrder(locationLotDetailList, user);
            resolver.Result = languageMgrE.TranslateMessage("MasterData.InspectOrder.Create.Successfully", resolver.UserCode, inspectOrder.InspectNo);
            resolver.Transformers = null;
            resolver.Code = inspectOrder.InspectNo;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
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
#region Extend Interface

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class InspectionMgrE : com.Sconit.Service.Business.Impl.InspectionMgr, IBusinessMgrE
    {
        
    }
}

#endregion
