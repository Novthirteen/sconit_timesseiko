using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepRequisitionOrderMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }

        //private int[] numCell = new int[] { 6, 7 };
        //private int[] stringCell = new int[] { 0, 1, 2, 3, 4, 5 };

        private static int ITEM_DESC1_ROW_LEN = 11;

        private static int ITEM_SPEC_ROW_LEN = 20;

        private static int ITEM_BRAND_ROW_LEN = 9;

        public RepRequisitionOrderMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 20;
            //列数   1起始
            this.columnCount = 8;
            //报表头的行数  1起始
            this.headRowCount = 15;
            //报表尾的行数  1起始
            this.bottomRowCount = 10;
        }

        public override bool FillValues(String templateFileName, IList<object> list)
        {
            try
            {
                if (list == null || list.Count < 1 || templateFileName == null || templateFileName.Length < 0) return false;
                this.rowCount = pageDetailRowCount + bottomRowCount;
                OrderHead orderHead = (OrderHead)(list[0]);
                IList<OrderDetail> orderDetails = (IList<OrderDetail>)(list[1]);
                if (orderHead == null
                        || orderDetails == null || orderDetails.Count == 0)
                {
                    return false;
                }

                decimal? discount = orderDetails.Where(od => od.UnitPriceAfterDiscount.HasValue && od.UnitPrice.HasValue).Sum(od => od.OrderedQty * (od.UnitPrice - od.UnitPriceAfterDiscount));

                if (discount.HasValue && discount.Value != 0)
                {
                    list.Add(true);
                    list.Add(discount);
                    this.columnCount = 9;
                    ITEM_SPEC_ROW_LEN = 20;
                    templateFileName = templateFileName.Substring(0, templateFileName.IndexOf(".xls")) + "2" + templateFileName.Substring(templateFileName.IndexOf(".xls"));
                }
                else
                {
                    this.columnCount = 8;
                    ITEM_SPEC_ROW_LEN = 35;
                    list.Add(false);
                }

                this.init(templateFileName);

                this.FillValuesImpl(templateFileName, list);
                return true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                this.Dispose();
                return false;
            }

        }


        /**
         * 填充报表
         * 
         * Param list [0]OrderHead
         * Param list [0]IList<OrderDetail>           
         */
        [Transaction(TransactionMode.Requires)]
        protected override void FillValuesImpl(String templateFileName, IList<object> list)
        {


            OrderHead orderHead = (OrderHead)(list[0]);
            IList<OrderDetail> orderDetails = (IList<OrderDetail>)(list[1]);

            bool hasDiscount = (bool)(list[2]);
            decimal totalDiscount = 0;
            if (hasDiscount)
            {
                totalDiscount = (decimal)(list[3]);
            }

            //decimal taxRate = decimal.Parse(entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_TAX_RATE).Value);

            if (orderHead.IsIncludeTax.HasValue && orderHead.IsIncludeTax.Value == true)
            {
                this.SetRowCell(0, 14, hasDiscount ? 4 : 5, "含税单价        (人民币)");
                this.SetRowCell(0, 14, hasDiscount ? 8 : 7, "含税总价        (人民币)");
            }

            this.FillHead(orderHead);

            //检查用哪种输出
            //1页显示
            //2附录清单显示

            int rowSpec = 0;
            int rowItemDesc1 = 0;
            //int rowBrand = 0;
            int rowMax = 0;
            foreach (OrderDetail orderDetail in orderDetails)
            {
                //品名
                List<string> itemDesc1List = StringHelper.SplitStringByLength(orderDetail.Item.Desc1, ITEM_DESC1_ROW_LEN);
                rowItemDesc1 += itemDesc1List.Count + 1;
                //规格及描述	
                List<string> specList = StringHelper.SplitStringByLength(orderDetail.Item.Spec, ITEM_SPEC_ROW_LEN);
                rowSpec += specList.Count + 1;
                //品牌
                //List<string> brandList = StringHelper.SplitStringByLength(orderDetail.Item.Brand, ITEM_BRAND_ROW_LEN);
                //rowBrand += brandList.Count + 1;
            }

            rowMax = Math.Max(rowItemDesc1, rowSpec);

            int pageIndex = 1;
            int rowIndex = 0;
            int seq = 1;

            //decimal? totalAmount = orderDetails.Sum(od => od.TotalAmountTo);

            if (rowMax <= pageDetailRowCount)
            {

                foreach (OrderDetail orderDetail in orderDetails)
                {
                    //序号	
                    this.SetRowCell(pageIndex, rowIndex, 0, seq++);
                    //品名	
                    int itemDesc1RowCount = 0;
                    List<string> itemDesc1List = StringHelper.SplitStringByLength(orderDetail.Item.Desc1, ITEM_DESC1_ROW_LEN);
                    for (int i = 0; i < itemDesc1List.Count; i++)
                    {
                        this.SetRowCell(pageIndex, rowIndex + i, 1, itemDesc1List[i]);
                    }
                    itemDesc1RowCount = itemDesc1List.Count;

                    //规格及描述	
                    List<string> specList = StringHelper.SplitStringByLength(orderDetail.Item.Spec, ITEM_SPEC_ROW_LEN);
                    for (int i = 0; i < specList.Count; i++)
                    {
                        this.SetRowCell(pageIndex, rowIndex + i, 2, specList[i]);
                    }
                    int specRowCount = specList.Count;


                    /*//品牌	
                    List<string> brandList = StringHelper.SplitStringByLength(orderDetail.Item.Brand, ITEM_BRAND_ROW_LEN);
                    for (int i = 0; i < brandList.Count; i++)
                    {
                        this.SetRowCell(pageIndex, rowIndex + i, 3, brandList[i]);
                    }
                    int brandRowCount = brandList.Count;
                     */
                    //制造商	
                    string manufacturer = string.Empty;
                    if (orderDetail.Manufacturer == null || orderDetail.Manufacturer.Trim().Length == 0)
                    {
                        if (orderDetail.Item.Manufacturer != null && orderDetail.Item.Manufacturer.Trim().Length > 0)
                        {
                            manufacturer = orderDetail.Item.Manufacturer;
                        }
                    }
                    else
                    {
                        manufacturer = orderDetail.Manufacturer;
                    }
                    this.SetRowCell(pageIndex, rowIndex, 3, manufacturer);

                    if (!hasDiscount)
                    {
                        //单位	
                        this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.Uom.Name);
                    }

                    //不含税单价(人民币)	
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 4 : 5, hasDiscount ? (orderDetail.UnitPrice.HasValue ? double.Parse(orderDetail.UnitPrice.Value.ToString("F2")) : 0) : (orderDetail.UnitPriceAfterDiscount == null ? 0 : double.Parse(orderDetail.UnitPriceAfterDiscount.Value.ToString("F2"))));

                    if (hasDiscount)
                    {
                        //折扣率
                        this.SetRowCell(pageIndex, rowIndex, 5, orderDetail.UnitPriceAfterDiscount.HasValue && orderDetail.UnitPrice.HasValue ? Double.Parse((1 - orderDetail.UnitPriceAfterDiscount / orderDetail.UnitPrice).Value.ToString("F2")) : 0);

                        //折扣
                        this.SetRowCell(pageIndex, rowIndex, 6, Double.Parse(orderDetail.UnitPriceAfterDiscount.Value.ToString("F2")));
                    }

                    //数量	
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, double.Parse(orderDetail.OrderedQty.ToString("0.########")));

                    //不含税总价(人民币)
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 8 : 7, (orderDetail.UnitPriceAfterDiscount * orderDetail.OrderedQty).Value.ToString("F2"));


                    int maxRowTemp = rowIndex + Math.Max(itemDesc1RowCount, specRowCount) - 1;
                    if (!(rowIndex == 0 && maxRowTemp == 0))
                    {
                        for (int pos = rowIndex; pos <= maxRowTemp; pos++)
                        {
                            //this.CopyRowStyle(this.GetRowIndexAbsolute(1, 0), this.GetRowIndexAbsolute(pageIndex, pos), this.GetColumnIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(pageIndex, columnCount));
                            Row rowTo = this.GetRow(this.GetRowIndexAbsolute(1, 0));
                            Row rowFrom = this.GetRow(this.GetRowIndexAbsolute(pageIndex, pos));
                            rowTo.Height = rowFrom.Height;
                            for (int i = 0; i < this.columnCount; i++)
                            {
                                if (pageIndex == 1 && pos == 0) continue;
                                Cell cell = this.GetCell(this.GetRowIndexAbsolute(pageIndex, pos), i);
                                cell.CellStyle = workbook.CreateCellStyle();
                                cell.CellStyle.CloneStyleFrom(this.GetCellStyle(this.GetRowIndexAbsolute(1, 0), i));

                                CellStyle cellStyle = this.GetCellStyle(this.GetRowIndexAbsolute(pageIndex, pos), i);
                                cellStyle.BorderTop = NPOI.SS.UserModel.CellBorderType.NONE;
                                this.GetCell(this.GetRowIndexAbsolute(pageIndex, pos), i).CellStyle.CloneStyleFrom(cellStyle);
                            }
                        }
                        rowIndex = maxRowTemp;
                    }


                    //每条记录 最后一行加分割线
                    for (int i = 0; i < this.columnCount; i++)
                    {
                        CellStyle cellStyle = this.GetCellStyle(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, i));
                        cellStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
                        this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, i)).CellStyle.CloneStyleFrom(cellStyle);
                    }

                    //this.CopyCellStyle(pageIndex, rowIndex, this.stringCell, cellStrBottomStyle);
                    //this.CopyCellStyle(pageIndex, rowIndex, this.numCell, cellNumBottomStyle);

                    rowIndex++;

                }
            }
            else
            {

                //IList<Item> itemList = (from orderDetail in orderDetails
                //                        let item = orderDetail.Item
                //                        group orderDetail by item into item
                //                        select item.Key).ToList<Item>();

                //foreach (Item item in itemList)
                //{
                //序号	
                this.SetRowCell(pageIndex, rowIndex, 0, seq++);
                //品名
                this.SetRowCell(pageIndex, rowIndex, 1, "刀具一批");

                this.SetRowCell(pageIndex, rowIndex, 2, "见清单");
                this.SetRowCell(pageIndex, rowIndex, 3, "见清单");

                if (!hasDiscount)
                {
                    //单位
                    this.SetRowCell(pageIndex, rowIndex, 4, "批");
                }

                //decimal itemUnitPrice = orderDetails.Where(od => od.PriceListDetailTo != null).Where(od => od.Item == item).Sum(od => od.PriceListDetailTo.UnitPrice);
                //decimal itemTotalPrice = orderDetails.Where(od => od.PriceListDetailTo != null).Where(od => od.Item == item).Sum(od => od.PriceListDetailTo.UnitPrice * od.OrderedQty);

                //不含税单价(人民币)
                this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 4 : 5, double.Parse(orderHead.OrderIncludeTaxPrice.ToString("F2")));
                //折扣
                if (hasDiscount)
                {
                    decimal? orderDiscountRate = orderDetails.Where(od => od.UnitPriceAfterDiscount.HasValue && od.UnitPrice.HasValue).Average(od => Decimal.One - od.UnitPriceAfterDiscount / od.UnitPrice);
                    //orderHead.OrderDiscountRate
                    this.SetRowCell(pageIndex, rowIndex, 5, double.Parse(orderDiscountRate.Value.ToString("F2")));
                    this.SetRowCell(pageIndex, rowIndex, 7, double.Parse(totalDiscount.ToString("F2")));
                }

                //数量
                this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, "1");

                //不含税总价(人民币)
                this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 8 : 7, double.Parse(orderHead.OrderIncludeTaxPrice.ToString("F2")));

                for (int i = 0; i < this.columnCount; i++)
                {
                    if (rowIndex != 0)
                    {
                        Cell cell = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), i);
                        cell.CellStyle = workbook.CreateCellStyle();
                        cell.CellStyle.CloneStyleFrom(this.GetCellStyle(this.GetRowIndexAbsolute(1, 0), i));
                    }
                    CellStyle cellStyle = this.GetCellStyle(this.GetRowIndexAbsolute(pageIndex, rowIndex), i);
                    cellStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
                    this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), i).CellStyle.CloneStyleFrom(cellStyle);
                }

                rowIndex++;
            }

            Cell cellBlank = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 8 : 7));
            CellStyle cellStyleBlank = workbook.CreateCellStyle();
            if (cellBlank.CellStyle != null)
            {
                cellStyleBlank.CloneStyleFrom(cellBlank.CellStyle);
            }
            cellStyleBlank.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            cellBlank.CellStyle = workbook.CreateCellStyle();
            cellBlank.CellStyle.CloneStyleFrom(cellStyleBlank);


            cellBlank = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellStyleBlank = workbook.CreateCellStyle();
            if (cellBlank.CellStyle != null)
            {
                cellStyleBlank.CloneStyleFrom(cellBlank.CellStyle);
            }
            cellStyleBlank.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            cellBlank.CellStyle = workbook.CreateCellStyle();
            cellBlank.CellStyle.CloneStyleFrom(cellStyleBlank);

            cellStyleBlank = workbook.CreateCellStyle();
            cellStyleBlank.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyleBlank.BorderRight = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleBlank.BorderTop = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleBlank.BorderBottom = NPOI.SS.UserModel.CellBorderType.NONE;

            rowIndex++;

            Cell cellFrom = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, hasDiscount ? 8 : 7));
            Cell cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 8 : 7));
            CellStyle cellStyleTo = workbook.CreateCellStyle();
            if (cellFrom.CellStyle != null)
            {
                cellStyleTo.CloneStyleFrom(cellFrom.CellStyle);
            }
            cellStyleTo.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleTo);

            CellStyle cellStyleT = workbook.CreateCellStyle();
            cellStyleT.CloneStyleFrom(cellStyleTo);
            cellStyleT.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyleT.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleT.BorderBottom = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleT.BorderTop = NPOI.SS.UserModel.CellBorderType.NONE;

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 7 : 6));
            CellStyle cellStyle3 = workbook.CreateCellStyle();
            if (cellTo.CellStyle != null)
            {
                cellStyle3.CloneStyleFrom(cellTo.CellStyle);
            }
            cellStyle3.Alignment = HorizontalAlignment.RIGHT;
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyle3);


            //含税了
            this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, "不含税总金额(人民币)：");

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleBlank);
            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 8 : 7));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleT);

            this.SetRowCell(pageIndex, rowIndex++, hasDiscount ? 8 : 7, double.Parse(orderHead.OrderExcludeTaxPrice.ToString("F2")));

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 7 : 6));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyle3);
            this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, Decimal.Parse(orderHead.TaxCode).ToString("0.########") + "%增值税：");

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 8 : 7));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleT);
            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleBlank);

            this.SetRowCell(pageIndex, rowIndex++, hasDiscount ? 8 : 7, double.Parse(orderHead.OrderTax.ToString("F2")));

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 7 : 6));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyle3);
            this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, "含税总金额(人民币)：");

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, hasDiscount ? 8 : 7));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleT);
            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleBlank);


            this.SetRowCell(pageIndex, rowIndex++, hasDiscount ? 8 : 7, double.Parse(orderHead.OrderIncludeTaxPrice.ToString("F2")));


            Cell cell1 = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            CellStyle cellStyle1 = workbook.CreateCellStyle();
            if (cell1.CellStyle != null)
            {
                cellStyle1.CloneStyleFrom(cell1.CellStyle);
            }
            Font font1 = workbook.CreateFont();
            font1.FontHeightInPoints = (short)12;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            cellStyle1.SetFont(font1);

            cellStyle1.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderRight = NPOI.SS.UserModel.CellBorderType.NONE;
            //cellStyle1.Alignment = HorizontalAlignment.LEFT;
            cell1.CellStyle = workbook.CreateCellStyle();
            cell1.CellStyle.CloneStyleFrom(cellStyle1);

            cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderRight = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;

            this.CopyCellStyle(pageIndex, rowIndex, hasDiscount ? new int[] { 1, 2, 3, 4, 5, 6, 7 } : new int[] { 1, 2, 3, 4, 5, 6 }, cellStyle1);

            cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            this.CopyCellStyle(pageIndex, rowIndex, new int[] { hasDiscount ? 8 : 7 }, cellStyle1);

            //this.SetRowCell(pageIndex, rowIndex++, 0, "总金额(含" + taxRate + "%税)：");

            String totalAmountOfTax = (hasDiscount ? "I" : "H") + this.GetRowIndexAbsolute(pageIndex, rowIndex);

            this.SetRowCellFormula(this.GetRowIndexAbsolute(pageIndex, rowIndex++), 0, "\"货物总金额 (包含" + Decimal.Parse(orderHead.TaxCode).ToString("0.########") + "%增值税)：人民币\"&IF(" + totalAmountOfTax + "<0,\"负\",\"\")&TEXT(TRUNC(ABS(ROUND(" + totalAmountOfTax + ",2))),\"[DBNum2]\")&\"元\"&IF(ISERR(FIND(\".\",ROUND(" + totalAmountOfTax + ",2))),\"\",TEXT(RIGHT(TRUNC(ROUND(" + totalAmountOfTax + ",2)*10)),\"[DBNum2]\"))&IF(ISERR(FIND(\".0\",TEXT(" + totalAmountOfTax + ",\"0.00\"))),\"角\",\"\")&IF(LEFT(RIGHT(ROUND(" + totalAmountOfTax + ",2),3))=\".\",TEXT(RIGHT(ROUND(" + totalAmountOfTax + ",2)),\"[DBNum2]\")&\"分\",\"整\")");

            rowIndex++;

            //卖方
            this.SetRowCell(pageIndex, rowIndex, 1, "业务员签署:");
            this.SetRowCell(pageIndex, rowIndex, 3, "日期:");

            rowIndex++;
            rowIndex++;
            //买方
            this.SetRowCell(pageIndex, rowIndex, 1, "负责人签署:");

            this.AddSeal(orderHead.ApprovedUser, this.GetRowIndexAbsolute(pageIndex, rowIndex), 2);

            this.SetRowCell(pageIndex, rowIndex, 3, "日期:");


            //在第二页 输出明细
            if (rowMax > pageDetailRowCount)
            {
                //this.sheet.SetRowBreak(this.GetRowIndexAbsolute(pageIndex, rowIndex ));

                pageIndex++;
                rowIndex = 0;

                this.SetMergedRegion(pageIndex, rowIndex, 0, rowIndex, this.columnCount - 1);
                this.SetRowCell(pageIndex, rowIndex, 0, "清单");

                this.CopyRowStyle(this.GetRowIndexAbsolute(0, 0), this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(0, 0), this.GetColumnIndexAbsolute(0, 1));

                rowIndex++;

                this.CopyRowStyle(this.GetRowIndexAbsolute(0, this.headRowCount - 1), this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(0, 0), this.GetColumnIndexAbsolute(0, this.columnCount));

                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 0, "A" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 1, "B" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 2, "C" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 3, "D" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 4, "E" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 5, "F" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 6, "G" + this.headRowCount);
                this.CopyCell(pageIndex, rowIndex + this.headRowCount, 7, "H" + this.headRowCount);
                if (hasDiscount)
                {
                    this.CopyCell(pageIndex, rowIndex + this.headRowCount, 8, "I" + this.headRowCount);
                }

                rowIndex++;
                seq = 1;
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    //序号	
                    this.SetRowCell(pageIndex, rowIndex, 0, seq++);
                    //品名
                    this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Item.Desc1);

                    //规格及描述	
                    this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Spec);

                    //品牌	
                    //this.SetRowCell(pageIndex, rowIndex, 3, orderDetail.Item.Brand);

                    //制造商	
                    string manufacturer = string.Empty;
                    if (orderDetail.Manufacturer == null || orderDetail.Manufacturer.Trim().Length == 0)
                    {
                        if (orderDetail.Item.Manufacturer != null && orderDetail.Item.Manufacturer.Trim().Length > 0)
                        {
                            manufacturer = orderDetail.Item.Manufacturer;
                        }
                    }
                    else
                    {
                        manufacturer = orderDetail.Manufacturer;
                    }
                    this.SetRowCell(pageIndex, rowIndex, 3, manufacturer);

                    if (!hasDiscount)
                    {
                        //单位	
                        this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.Uom.Name);
                    }

                    //不含税单价(人民币)	
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 4 : 5, hasDiscount ? (orderDetail.UnitPrice.HasValue ?  double.Parse(orderDetail.UnitPrice.Value.ToString("F2")):0) : (orderDetail.UnitPriceAfterDiscount.HasValue ? double.Parse(orderDetail.UnitPriceAfterDiscount.Value.ToString("F2")):0));

                    if (hasDiscount)
                    {
                        //折扣率
                        this.SetRowCell(pageIndex, rowIndex, 5, orderDetail.UnitPriceAfterDiscount.HasValue && orderDetail.UnitPrice.HasValue ? double.Parse((1 - orderDetail.UnitPriceAfterDiscount / orderDetail.UnitPrice).Value.ToString("F2")) : 0);

                        //折扣价
                        this.SetRowCell(pageIndex, rowIndex, 6, double.Parse((orderDetail.UnitPriceAfterDiscount).Value.ToString("F2")));
                    }

                    //数量	
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 7 : 6, double.Parse(orderDetail.OrderedQty.ToString("0.########")));

                    //不含税总价(人民币)
                    this.SetRowCell(pageIndex, rowIndex, hasDiscount ? 8 : 7, double.Parse((orderDetail.UnitPriceAfterDiscount * orderDetail.OrderedQty).Value.ToString("F2")));

                    for (int i = 0; i < this.columnCount; i++)
                    {
                        Cell cellFrom1 = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, i));
                        Cell cellTo1 = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, i));
                        CellStyle cellStyle = workbook.CreateCellStyle();
                        if (cellFrom1.CellStyle != null)
                        {
                            cellStyle.CloneStyleFrom(cellFrom1.CellStyle);
                        }
                        cellStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;

                        if (i == 1 || i == 2 || i == 3)
                        {
                            IList<string> temp = null;
                            switch (i)
                            {
                                case 1:
                                    {
                                        temp = StringHelper.SplitStringByLength(orderDetail.Item.Desc1, ITEM_DESC1_ROW_LEN);
                                        break;
                                    }
                                case 2:
                                    {
                                        temp = StringHelper.SplitStringByLength(orderDetail.Item.Spec, ITEM_SPEC_ROW_LEN);
                                        break;
                                    }
                                case 3:
                                    {
                                        temp = StringHelper.SplitStringByLength(orderDetail.Item.Brand, ITEM_BRAND_ROW_LEN);
                                        break;
                                    }
                            }
                            if (temp == null || temp.Count() == 1)
                            {
                                cellStyle.ShrinkToFit = true;
                            }
                            else
                            {
                                cellStyle.WrapText = true;
                            }
                        }
                        else
                        {
                            cellStyle.WrapText = true;
                        }

                        //cellTo1.CellStyle = workbook.CreateCellStyle();
                        //cellTo1.CellStyle.CloneStyleFrom(cellStyle);
                        cellTo1.CellStyle = cellStyle;
                    }

                    rowIndex++;
                }


                //首先输出 汇总信息
                //IList<string> itemCategoryCode = orderDetails.GroupBy(orderDetail => orderDetail.Item.ItemCategory.Code).Select(orderDetail);

            }



            this.sheet.DisplayGridlines = false;
            this.sheet.IsPrintGridlines = false;

            if (orderHead.IsPrinted == null || orderHead.IsPrinted == false)
            {
                orderHead.IsPrinted = true;
                orderHeadMgrE.UpdateOrderHead(orderHead);
            }


        }


        /*
         * 填充报表头
         * 
         * Param repack 要货单头对象
         */
        private void FillHead(OrderHead orderHead)
        {

            this.SetRowCell(0, 0, orderHead.BillFrom != null ? orderHead.BillFrom.Address : string.Empty);

            //要货单编号：
            this.SetRowCell(3, 2, orderHead.OrderNo);

            //日期：
            this.SetRowCell(3, 5, orderHead.CreateDate.ToString("yyyy-MM-dd"));
            //要货单位：
            this.SetRowCell(5, 2, orderHead.BillTo != null ? orderHead.BillTo.Address : string.Empty);
            //地址：
            this.SetRowCell(6, 2, orderHead.ShipTo != null ? orderHead.ShipTo.Address : string.Empty);
            //电话：
            this.SetRowCell(7, 2, orderHead.ShipTo != null ? orderHead.ShipTo.TelephoneNumber : string.Empty);
            //传真：
            this.SetRowCell(7, 5, orderHead.ShipTo != null ? orderHead.ShipTo.Fax : string.Empty);
            //联系人：
            this.SetRowCell(8, 2, orderHead.ShipTo != null ? orderHead.ShipTo.ContactPersonName : string.Empty);
            //邮政编码：
            this.SetRowCell(8, 5, orderHead.ShipTo != null ? orderHead.ShipTo.PostalCode : string.Empty);
            //发货方式:
            //this.SetRowCell(9, 2, );//todo
            //发货地址:
            this.SetRowCell(10, 2, orderHead.ShipFrom.Address);

            //17%增值税:
            //this.SetRowCell(41, 6, taxRate + "%增值税:");
            //合同总金额 (包含17%增值税)：人民币
            // this.SetRowCell(43, 0, "合同总金额 (包含" + taxRate + "%增值税)：人民币");
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //CIF总金额   (人民币):
            this.CopyCell(pageIndex, 40, 6, "G41");

            //17%增值税:
            this.CopyCell(pageIndex, 41, 6, "G42");

            //含税总金额   (人民币):
            this.CopyCell(pageIndex, 42, 6, "G43");

            //合同总金额 (包含17%增值税)：人民币
            this.CopyCell(pageIndex, 43, 0, "A44");

            //业务员签署:
            this.CopyCell(pageIndex, 45, 1, "B46");
            //日期:
            this.CopyCell(pageIndex, 45, 3, "D46");
            //负责人签署:
            this.CopyCell(pageIndex, 47, 1, "B48");
            //日期:
            this.CopyCell(pageIndex, 47, 3, "D48");


        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(code, true);
            if (orderHead != null)
            {
                list.Add(orderHead);
                list.Add(orderHead.OrderDetails);
            }
            return list;
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepRequisitionOrderMgrE : com.Sconit.Service.Report.Impl.RepRequisitionOrderMgr, IReportBaseMgrE
    {


    }
}

#endregion
