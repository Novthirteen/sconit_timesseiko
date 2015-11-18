using com.Sconit.Service.Ext.MasterData;


using System;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Persistence;
using System.Data;
using System.Data.SqlClient;


//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class NumberControlMgr : NumberControlBaseMgr, INumberControlMgr
    {
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public ISqlHelperDao sqlHelperDao { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public string GenerateNumber(string code)
        {

            string numberPrefix  = code + DateTime.Now.Year.ToString().Substring(2);
            string numberSuffix = GetNextSequence(numberPrefix);

            EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ORDER_LENGTH);
            int orderLength = int.Parse(entityPreference.Value);
            numberSuffix = numberSuffix.PadLeft(orderLength - numberPrefix.Length, '0');


            return (numberPrefix + numberSuffix);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateNumber(string code, int numberSuffixLength)
        {
            string numberPrefix = code + DateTime.Now.Year.ToString().Substring(2);
            string numberSuffix = GetNextSequence(numberPrefix);

            if (numberSuffix.Length > numberSuffixLength)
            {
                throw new TechnicalException("numberSuffix.length > numberSuffixLength");
            }

            numberSuffix = numberSuffix.PadLeft(numberSuffixLength, '0');

            return (numberPrefix + numberSuffix);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(FlowDetail flowDetail, string lotNo, decimal qty)
        {
            return GenerateRMHuId(flowDetail, lotNo, qty, null);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(FlowDetail flowDetail, string lotNo, decimal qty, string idMark)
        {
            #region 物料条码
            //if (flowDetail.BarCodeType == BusinessConstants.CODE_MASTER_RAW_MATERIAL_BAR_CODE_TYPE_VALUE_DEFAULT)
            //{

            //}
            //else
            //{

            //}
            //ItemCode＋供应商标识（1位）＋年 月 日4位 + 数量（4位，不足4位以0补齐）＋生产序号（3位，不足3位以0补齐）
            string itemCode = flowDetail.Item.Code;
            string supIdMark = idMark != null && idMark.Trim() != string.Empty ? idMark :
                (flowDetail.IdMark != null && flowDetail.IdMark.Trim() != string.Empty
                ? flowDetail.IdMark.Trim() : BusinessConstants.DEFAULT_SUPPLIER_ID_MARK);
            if (lotNo != null && lotNo.Trim().Length != 0)
            {
                LotNoHelper.validateLotNo(lotNo);
            }
            else
            {
                lotNo = LotNoHelper.GenerateLotNo();
            }
            if (flowDetail.Item.Uom.Code != flowDetail.Uom.Code)
            {
                qty = qty * this.uomConversionMgrE.ConvertUomQty(flowDetail.Item, flowDetail.Uom, 1, flowDetail.Item.Uom);   //单位用量
            }
            string qtyStr = qty.ToString().PadLeft(4, '0');

            string barCodePrefix = itemCode + supIdMark + lotNo + qtyStr;
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateFGHuId(FlowDetail flowDetail, string shiftCode, decimal qty)
        {
            return GenerateFGHuId(flowDetail, shiftCode, qty, null);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateFGHuId(FlowDetail flowDetail, string shiftCode, decimal qty, string idMark)
        {
            #region 成品条码
            //if (flowDetail.BarCodeType == BusinessConstants.CODE_MASTER_FINISH_GOODS_BAR_CODE_TYPE_VALUE_DEFAULT)
            //{

            //}
            //else
            //{

            //}
            //生产线识别码2位＋产品识别码(1位) ＋生产日期4位 (规则同5.5.1.2中的供货日期编码) ＋班别号1位 (以大写字母表示) ＋3位顺序号
            string pl = flowDetail.Flow.Code;
            if (pl.Length > 2)
            {
                //如果生产线代码长度大于2，取后两位
                pl = pl.Substring(pl.Length - 2);
            }

            string pIdMark = idMark != null && idMark.Trim() != string.Empty ? idMark :
                (flowDetail.IdMark != null && flowDetail.IdMark.Trim() != string.Empty
                ? flowDetail.IdMark.Trim() : BusinessConstants.DEFAULT_FINISHI_GOODS_ID_MARK);
            string lotNo = flowDetail.HuLotNo;
            if (lotNo != null && lotNo.Trim().Length != 0)
            {
                LotNoHelper.validateLotNo(lotNo);
            }
            else
            {
                lotNo = LotNoHelper.GenerateLotNo();
            }
            string shift = shiftCode;

            string barCodePrefix = pl + pIdMark + lotNo + shift;
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(OrderDetail orderDetail, string lotNo, decimal qty)
        {
            return GenerateRMHuId(orderDetail, lotNo, qty, null);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(OrderDetail orderDetail, string lotNo, decimal qty, string idMark)
        {
            #region 物料条码
            //if (orderDetail.BarCodeType == BusinessConstants.CODE_MASTER_RAW_MATERIAL_BAR_CODE_TYPE_VALUE_DEFAULT)
            //{

            //}
            //else
            //{

            //}
            //ItemCode＋供应商标识（1位）＋年 月 日4位 + 数量（4位，不足4位以0补齐）＋生产序号（3位，不足3位以0补齐）
            string itemCode = orderDetail.Item.Code;
            string supIdMark = idMark != null && idMark.Trim() != string.Empty ? idMark :
                (orderDetail.IdMark != null && orderDetail.IdMark.Trim() != string.Empty
                ? orderDetail.IdMark.Trim() : BusinessConstants.DEFAULT_SUPPLIER_ID_MARK);
            if (lotNo != null && lotNo.Trim().Length != 0)
            {
                LotNoHelper.validateLotNo(lotNo);
            }
            else
            {
                lotNo = LotNoHelper.GenerateLotNo();
            }

            //单位换算，转换为基本单位
            //理论上应该qty * inOrderLocationTransaction.UnitQty，现在暂时直接用单位换算
            if (orderDetail.Item.Uom.Code != orderDetail.Uom.Code)
            {
                qty = qty * uomConversionMgrE.ConvertUomQty(orderDetail.Item, orderDetail.Uom, qty, orderDetail.Item.Uom);
            }

            string qtyStr = qty.ToString("0.########").PadLeft(4, '0');

            string barCodePrefix = itemCode + supIdMark + lotNo + qtyStr;
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateFGHuId(OrderDetail orderDetail, decimal qty)
        {
            return GenerateFGHuId(orderDetail, qty, null);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateFGHuId(OrderDetail orderDetail, decimal qty, string idMark)
        {
            #region 成品条码
            //if (orderDetail.BarCodeType == BusinessConstants.CODE_MASTER_FINISH_GOODS_BAR_CODE_TYPE_VALUE_DEFAULT)
            //{

            //}
            //else
            //{

            //}
            //生产线识别码2位＋产品识别码(1位) ＋生产日期4位 (规则同5.5.1.2中的供货日期编码) ＋班别号1位 (以大写字母表示) ＋3位顺序号
            string pl = orderDetail.OrderHead.Flow;
            if (pl.Length > 2)
            {
                //如果生产线代码长度大于2，取后两位
                pl = pl.Substring(pl.Length - 2);
            }

            string pIdMark = idMark != null && idMark.Trim() != string.Empty ? idMark :
                (orderDetail.IdMark != null && orderDetail.IdMark.Trim() != string.Empty
                ? orderDetail.IdMark.Trim() : BusinessConstants.DEFAULT_FINISHI_GOODS_ID_MARK);
            string lotNo = LotNoHelper.GenerateLotNo();
            string shift = orderDetail.OrderHead.Shift.Code;

            string barCodePrefix = pl + pIdMark + lotNo + shift;
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(InProcessLocationDetail inProcessLocationDetail, string lotNo, decimal qty)
        {
            return GenerateRMHuId(inProcessLocationDetail.OrderLocationTransaction.OrderDetail, lotNo, qty);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateRMHuId(ReceiptDetail receiptDetail, string lotNo, decimal qty)
        {
            return GenerateRMHuId(receiptDetail.OrderLocationTransaction.OrderDetail, lotNo, qty);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateFGHuId(ReceiptDetail receiptDetail, decimal qty)
        {
            return GenerateFGHuId(receiptDetail.OrderLocationTransaction.OrderDetail, qty);
        }

        [Transaction(TransactionMode.Requires)]
        public string CloneRMHuId(string huId, decimal qty)
        {
            #region 物料条码
            string prefix = huId.Substring(0, huId.Length - 7);
            string qtyStr = qty.ToString("0.########").PadLeft(4, '0');

            string barCodePrefix = prefix + qtyStr;
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public string CloneFGHuId(string huId)
        {
            #region 成品条码
            string barCodePrefix = huId.Substring(0, huId.Length - 3);
            return GenerateNumber(barCodePrefix, 3);
            #endregion
        }

        [Transaction(TransactionMode.Requires)]
        public void ReverseUpdateHuId(string huId)
        {
            string barCodePrefix = BarcodeHelper.GetBarcodePrefix(huId);
            int seq = BarcodeHelper.GetBarcodeSeq(huId);
            ReverseUpdateSequence(BarcodeHelper.GetBarcodePrefix(huId), seq);
        }

        #endregion Customized Methods

        #region Private Methods
        private String GetNextSequence(string code)
        {
            SqlParameter[] parm = new SqlParameter[2];

            parm[0] = new SqlParameter("@CodePrefix", SqlDbType.VarChar, 50);
            parm[0].Value = code;

            parm[1] = new SqlParameter("@NextSequence", SqlDbType.Int, 50);
            parm[1].Direction = ParameterDirection.InputOutput;

            sqlHelperDao.ExecuteStoredProcedure("GetNextSequence", parm);

            return parm[1].Value.ToString();
        }

        private void ReverseUpdateSequence(string code, int seq)
        {
            SqlParameter[] parm = new SqlParameter[2];

            parm[0] = new SqlParameter("@CodePrefix", SqlDbType.VarChar, 50);
            parm[0].Value = code;

            parm[1] = new SqlParameter("@NextSequence", SqlDbType.Int, 50);
            parm[1].Value = seq;

            sqlHelperDao.ExecuteStoredProcedure("GetNextSequence", parm);
        }
        #endregion
    }
}


#region Extend Class




namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class NumberControlMgrE : com.Sconit.Service.MasterData.Impl.NumberControlMgr, INumberControlMgrE
    {
        
    }
}
#endregion
