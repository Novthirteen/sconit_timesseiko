using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;

using NPOI.HSSF.UserModel;
using System.Collections;
using com.Sconit.Utility;
using System.IO;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using NPOI.SS.UserModel;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ImportMgr : IImportMgr
    {
        #region 变量
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IShiftMgrE shiftMgrE { get; set; }
        public IFlowDetailMgrE flowDetailMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        #endregion

        

        #region IImportMgr接口实现
        [Transaction(TransactionMode.Unspecified)]
        public IList<ShiftPlanSchedule> ReadPSModelFromXls(Stream inputStream, User user, string regionCode, string flowCode, DateTime date, string shiftCode)
        {
            IList<ShiftPlanSchedule> spsList = new List<ShiftPlanSchedule>();
            Shift shift = shiftMgrE.LoadShift(shiftCode);

            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            if (shift == null)
                throw new BusinessErrorException("Import.PSModel.ShiftNotExist");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 4);
            int colIndex = this.GetPlanColumnIndexToRead((Row)rows.Current, shift.ShiftName, date);

            if (colIndex < 0)
                throw new BusinessErrorException("Import.PSModel.Shift.Not.Exist", shift.ShiftName);

            ImportHelper.JumpRows(rows, 2);

            while (rows.MoveNext())
            {
                Row row = (Row)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 4))
                {
                    break;//边界
                }

                //string regCode=row.GetCell(
                string fCode = string.Empty;
                string itemCode = string.Empty;
                int seq = 0;
                decimal planQty = 0;
                Cell cell = null;

                #region 读取生产线
                fCode = row.GetCell(1).StringCellValue;
                if (fCode.Trim() == string.Empty)
                    throw new BusinessErrorException("Import.PSModel.Empty.Error.Flow", (row.RowNum + 1).ToString());

                if (flowCode != null && flowCode.Trim() != string.Empty)
                {
                    if (fCode.Trim().ToUpper() != flowCode.Trim().ToUpper())
                        continue;//生产线过滤
                }
                #endregion

                #region 读取序号
                try
                {
                    string seqStr = row.GetCell(2).StringCellValue;
                    seq = row.GetCell(2).StringCellValue.Trim() != string.Empty ? int.Parse(row.GetCell(2).StringCellValue) : 0;
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.Seq", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取成品代码
                try
                {
                    itemCode = row.GetCell(3).StringCellValue;
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取计划量
                try
                {
                    cell = row.GetCell(colIndex);
                    if (cell == null || cell.CellType == NPOI.SS.UserModel.CellType.BLANK)
                        continue;

                    planQty = Convert.ToDecimal(row.GetCell(colIndex).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                }
                #endregion

                FlowDetail flowDetail = flowDetailMgrE.LoadFlowDetail(fCode, itemCode, seq);
                if (flowDetail == null)
                    throw new BusinessErrorException("Import.PSModel.FlowDetail.Not.Exist", (row.RowNum + 1).ToString());

                //区域权限过滤
                if (regionCode != null && regionCode.Trim() != string.Empty)
                {
                    if (regionCode.Trim().ToUpper() != flowDetail.Flow.PartyTo.Code.ToUpper())
                        continue;
                }
                if (!user.HasPermission(flowDetail.Flow.PartyTo.Code))
                    continue;

                ShiftPlanSchedule sps = new ShiftPlanSchedule();
                sps.FlowDetail = flowDetail;
                sps.ReqDate = date;
                sps.Shift = shift;
                sps.PlanQty = planQty;
                sps.LastModifyUser = user;
                sps.LastModifyDate = DateTime.Now;
                spsList.Add(sps);
            }

            if (spsList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return spsList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowPlan> ReadShipScheduleYFKFromXls(Stream inputStream, User user, string planType, string partyCode, string timePeriodType, DateTime date)
        {
            IList<FlowPlan> flowPlanList = new List<FlowPlan>();
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 8);
            int colIndex = this.GetColumnIndexToRead_ShipScheduleYFK((Row)rows.Current, date);

            if (colIndex < 0)
                throw new BusinessErrorException("Import.MRP.DateNotExist", date.ToShortDateString());

            #region 列定义
            int colFlow = 1;//Flow
            int colUC = 6;//单包装
            #endregion

            while (rows.MoveNext())
            {
                Row row = (Row)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 6))
                {
                    break;//边界
                }

                //string regCode=row.GetCell(
                string flowCode = string.Empty;
                string itemCode = string.Empty;
                decimal UC = 1;
                decimal planQty = 0;

                #region 读取客户代码
                //try
                //{
                //    pCode = row.GetCell(1).StringCellValue;
                //    if (pCode.Trim() == string.Empty)
                //        throw new BusinessErrorException("Import.MRP.Empty.Error.Customer", (row.RowNum + 1).ToString());

                //    if (partyCode != null && partyCode.Trim() != string.Empty)
                //    {
                //        if (pCode.Trim().ToUpper() != partyCode.Trim().ToUpper())
                //            continue;//客户过滤
                //    }
                //}
                //catch
                //{
                //    throw new BusinessErrorException("Import.MRP.Read.Error.Customer", (row.RowNum + 1).ToString());
                //}
                #endregion

                #region 读取Flow
                try
                {
                    flowCode = row.GetCell(colFlow).StringCellValue;
                    if (flowCode.Trim() == string.Empty)
                        continue;
                }
                catch
                {
                    this.ThrowCommonError(row, colIndex);
                }
                #endregion

                #region 读取成品代码
                try
                {
                    itemCode = row.GetCell(4).StringCellValue;
                    if (itemCode == string.Empty)
                        throw new BusinessErrorException("Import.PSModel.Empty.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.ItemCode", (row.RowNum + 1).ToString());
                }
                #endregion

                #region 读取单包装
                try
                {
                    UC = Convert.ToDecimal(row.GetCell(colUC).NumericCellValue);
                }
                catch
                {
                    this.ThrowCommonError(row.RowNum, colUC, row.GetCell(colUC));
                }
                #endregion

                #region 读取计划量
                try
                {
                    planQty = Convert.ToDecimal(row.GetCell(colIndex).NumericCellValue);
                }
                catch
                {
                    throw new BusinessErrorException("Import.PSModel.Read.Error.PlanQty", (row.RowNum + 1).ToString());
                }
                #endregion

                FlowDetail flowDetail = this.LoadFlowDetailByFlow(flowCode, itemCode, UC);
                if (flowDetail == null)
                    throw new BusinessErrorException("Import.MRP.Distribution.FlowDetail.Not.Exist", (row.RowNum + 1).ToString());

                if (partyCode != null && partyCode.Trim() != string.Empty)
                {
                    if (!StringHelper.Eq(flowCode, partyCode))
                        continue;//客户过滤
                }

                //区域过滤
                if (partyCode != null && partyCode.Trim() != string.Empty)
                {
                    if (!StringHelper.Eq(partyCode, flowDetail.Flow.PartyTo.Code))
                        continue;//客户过滤
                }
                //区域权限过滤
                if (!user.HasPermission(flowDetail.Flow.PartyFrom.Code) && !user.HasPermission(flowDetail.Flow.PartyTo.Code))
                    continue;

                FlowPlan flowPlan = new FlowPlan();
                flowPlan.FlowDetail = flowDetail;
                flowPlan.TimePeriodType = timePeriodType;
                flowPlan.ReqDate = date;
                flowPlan.PlanQty = planQty;
                flowPlanList.Add(flowPlan);
            }

            if (flowPlanList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return flowPlanList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<CycleCountDetail> ReadCycleCountFromXls(Stream inputStream, User user, CycleCount cycleCount)
        {
            if (inputStream.Length == 0)
                throw new BusinessErrorException("Import.Stream.Empty");

            //区域权限过滤
            if (!user.HasPermission(cycleCount.Location.Region.Code))
            {
                throw new BusinessErrorException("Common.Business.Error.NoPartyPermission", cycleCount.Location.Region.Code);
            }

            HSSFWorkbook workbook = new HSSFWorkbook(inputStream);

            Sheet sheet = workbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            ImportHelper.JumpRows(rows, 11);

            #region 列定义
            int colItem = 1;//物料代码
            int colUom = 3;//单位
            int colQty = 4;//数量
            int colHu = 5;//条码
            int colBin = 6;//库格
            #endregion

            IList<CycleCountDetail> cycleCountDetailList = new List<CycleCountDetail>();
            while (rows.MoveNext())
            {
                Row row = (Row)rows.Current;
                if (!this.CheckValidDataRow(row, 1, 7))
                {
                    break;//边界
                }

                if (row.GetCell(colHu) == null || row.GetCell(colHu).ToString() == string.Empty)
                {
                    string itemCode = string.Empty;
                    decimal qty = 0;
                    string uomCode = string.Empty;

                    #region 读取数据
                    #region 读取物料代码
                    itemCode = row.GetCell(colItem) != null ? row.GetCell(colItem).StringCellValue : string.Empty;
                    if (itemCode == null || itemCode.Trim() == string.Empty)
                        this.ThrowCommonError(row.RowNum, colItem, row.GetCell(colItem));

                    var i = (
                        from c in cycleCountDetailList
                        where c.HuId == null && c.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper()
                        select c).Count();

                    if (i > 0)
                        throw new BusinessErrorException("Import.Business.Error.Duplicate", itemCode, (row.RowNum + 1).ToString(), (colItem + 1).ToString());
                    #endregion

                    #region 读取数量
                    try
                    {
                        qty = Convert.ToDecimal(row.GetCell(colQty).NumericCellValue);
                    }
                    catch
                    {
                        this.ThrowCommonError(row.RowNum, colQty, row.GetCell(colQty));
                    }
                    #endregion

                    #region 读取单位
                    uomCode = row.GetCell(colUom) != null ? row.GetCell(colUom).StringCellValue : string.Empty;
                    if (uomCode == null || uomCode.Trim() == string.Empty)
                        throw new BusinessErrorException("Import.Read.Error.Empty", (row.RowNum + 1).ToString(), colUom.ToString());
                    #endregion
                    #endregion

                    #region 填充数据
                    Item item = itemMgrE.CheckAndLoadItem(itemCode);
                    Uom uom = uomMgrE.CheckAndLoadUom(uomCode);
                    //单位换算
                    if (item.Uom.Code.Trim().ToUpper() != uom.Code.Trim().ToUpper())
                    {
                        qty = uomConversionMgrE.ConvertUomQty(item, uom, qty, item.Uom);
                    }

                    CycleCountDetail cycleCountDetail = new CycleCountDetail();
                    cycleCountDetail.CycleCount = cycleCount;
                    cycleCountDetail.Item = item;
                    cycleCountDetail.Qty = qty; cycleCountDetailList.Add(cycleCountDetail);
                    #endregion
                }
                else
                {
                    string huId = string.Empty;
                    string binCode = string.Empty;

                    #region 读取数据
                    #region 读取条码
                    huId = row.GetCell(colHu) != null ? row.GetCell(colHu).StringCellValue : string.Empty;
                    if (huId == null || huId.Trim() == string.Empty)
                        throw new BusinessErrorException("Import.Read.Error.Empty", (row.RowNum + 1).ToString(), colHu.ToString());

                    var i = (
                        from c in cycleCountDetailList
                        where c.HuId != null && c.HuId.Trim().ToUpper() == huId.Trim().ToUpper()
                        select c).Count();

                    if (i > 0)
                        throw new BusinessErrorException("Import.Business.Error.Duplicate", huId, (row.RowNum + 1).ToString(), colHu.ToString());
                    #endregion

                    #region 读取库格
                    binCode = row.GetCell(colBin) != null ? row.GetCell(colBin).StringCellValue : null;
                    #endregion
                    #endregion

                    #region 填充数据
                    Hu hu = huMgrE.CheckAndLoadHu(huId);
                    StorageBin bin = null;
                    if (binCode != null && binCode.Trim() != string.Empty)
                    {
                        bin = storageBinMgrE.CheckAndLoadStorageBin(binCode);
                    }

                    CycleCountDetail cycleCountDetail = new CycleCountDetail();
                    cycleCountDetail.CycleCount = cycleCount;
                    cycleCountDetail.Item = hu.Item;
                    cycleCountDetail.Qty = hu.Qty * hu.UnitQty;
                    cycleCountDetail.HuId = hu.HuId;
                    cycleCountDetail.LotNo = hu.LotNo;
                    cycleCountDetail.StorageBin = bin.Code;
                    cycleCountDetailList.Add(cycleCountDetail);
                    #endregion
                }
            }

            if (cycleCountDetailList.Count == 0)
                throw new BusinessErrorException("Import.Result.Error.ImportNothing");

            return cycleCountDetailList;
        }

        #endregion

        #region Private Method
        private int GetPlanColumnIndexToRead(Row row, string shiftName, DateTime date)
        {
            int colIndex = -1;
            int startColIndex = 5; //从第5列开始

            int dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0)
                dayOfWeek = 7;

            startColIndex = startColIndex + (dayOfWeek - 1) * 6;
            for (int i = startColIndex; i < row.LastCellNum; i = i + 2)
            {
                Cell cell = row.GetCell(i);
                string cellValue = cell.StringCellValue;
                if (cellValue == shiftName)
                {
                    colIndex = i;
                    break;
                }
            }

            return colIndex;
        }

        private int GetColumnIndexToRead_ShipScheduleYFK(Row row, DateTime date)
        {
            int colIndex = -1;
            int startColIndex = 7; //从第7列开始

            for (int i = startColIndex; i < row.LastCellNum; i++)
            {
                Cell cell = row.GetCell(i);
                DateTime cellValue = cell.DateCellValue;
                if (DateTime.Compare(cellValue, date) == 0)
                {
                    colIndex = i;
                    break;
                }
            }

            return colIndex;
        }

        private bool CheckValidDataRow(Row row, int startColIndex, int endColIndex)
        {
            for (int i = startColIndex; i < endColIndex; i++)
            {
                Cell cell = row.GetCell(i);
                if (cell != null && cell.CellType !=  NPOI.SS.UserModel.CellType.BLANK)
                {
                    return true;
                }
            }

            return false;
        }

        private void ThrowCommonError(Row row, int colIndex)
        {
            this.ThrowCommonError(row.RowNum, colIndex, row.GetCell(colIndex));
        }
        private void ThrowCommonError(int rowIndex, int colIndex, Cell cell)
        {
            string errorValue = string.Empty;
            if (cell != null)
            {
                if (cell.CellType == NPOI.SS.UserModel.CellType.STRING)
                {
                    errorValue = cell.StringCellValue;
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
                {
                    errorValue = cell.NumericCellValue.ToString("0.########");
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.BOOLEAN)
                {
                    errorValue = cell.NumericCellValue.ToString();
                }
                else if (cell.CellType == NPOI.SS.UserModel.CellType.BLANK)
                {
                    errorValue = "Null";
                }
                else
                {
                    errorValue = "Unknow value";
                }
            }
            throw new BusinessErrorException("Import.Read.CommonError", (rowIndex + 1).ToString(), (colIndex + 1).ToString(), errorValue);
        }

        [Transaction(TransactionMode.Unspecified)]
        private FlowDetail LoadFlowDetailByFlow(string flowCode, string itemCode, decimal UC)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
            criteria.CreateAlias("FlowDetail", "fd");
            criteria.Add(Expression.Eq("Flow.Code", flowCode));
            criteria.Add(Expression.Eq("fd.Item.Code", itemCode));
            IList<FlowView> flowViewList = criteriaMgrE.FindAll<FlowView>(criteria);

            FlowDetail flowDetail = null;
            if (flowViewList != null && flowViewList.Count > 0)
            {
                var q1 = flowViewList.Where(f => f.FlowDetail.UnitCount == UC).Select(f => f.FlowDetail);
                if (q1.Count() > 0)
                {
                    flowDetail = q1.First();
                }
                else
                {
                    flowDetail = flowViewList[0].FlowDetail;
                }
            }

            return flowDetail;
        }

        #endregion
    }
}



#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    public partial class ImportMgrE : com.Sconit.Service.MasterData.Impl.ImportMgr, IImportMgrE
    {
       
    }
}

#endregion

#region Extend Interface







namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ImportMgrE : com.Sconit.Service.MasterData.Impl.ImportMgr, IImportMgrE
    {

    }
}

#endregion
