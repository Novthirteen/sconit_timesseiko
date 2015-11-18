using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using com.Sconit.Entity;

namespace com.Sconit.Service.Report.Impl
{
    /*
     * 采购单
     *       适用于天熙
     */
    [Transactional]
    public class RepPurchaseMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }

        private static int ITEM_DESC1_ROW_LEN = 9;

        private static int ITEM_SPEC_ROW_LEN = 13;

        private static int MANUFACTURER_ROW_LEN = 6;

        public RepPurchaseMgr()
        {
            //明细部分的行数
            //this.pageDetailRowCount = 12;
            //列数   1起始
            this.columnCount = 8;
            //报表头的行数  1起始
            this.headRowCount = 18;
            //报表尾的行数  1起始
            //this.bottomRowCount = 22;

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


            if (list == null || list.Count < 2) throw new Exception();

            OrderHead orderHead = (OrderHead)(list[0]);
            IList<OrderDetail> orderDetails = (IList<OrderDetail>)(list[1]);

            if (orderHead == null
                || orderDetails == null || orderDetails.Count == 0)
            {
                throw new Exception();
            }

            //decimal taxRate = decimal.Parse(entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_TAX_RATE).Value);

            if (orderHead.IsIncludeTax.HasValue && orderHead.IsIncludeTax == true)
            {
                this.SetRowCell(0, 17, 4, "人民币单价（含税）");
                this.SetRowCell(0, 17, 6, "人民币总价（含税）");
            }
            
            this.FillHead(orderHead);

            //自动换行
            //style.WrapText = true;

            int pageIndex = 1;
            int rowIndex = 0;
            int seq = 1;


            foreach (OrderDetail orderDetail in orderDetails)
            {
                //序号	
                this.SetRowCell(pageIndex, rowIndex, 0, seq++);
                //品名
                this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Item.Desc1);

                //规格及描述	
                this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Spec);

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

                //不含税单价(人民币)	
                this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.UnitPriceAfterDiscount == null ? 0 : Double.Parse(orderDetail.UnitPriceAfterDiscount.Value.ToString("F2")));

                //数量	
                this.SetRowCell(pageIndex, rowIndex, 5, Double.Parse(orderDetail.OrderedQty.ToString("0.########")));

                //不含税总价(人民币)
                this.SetRowCell(pageIndex, rowIndex, 6, orderDetail.UnitPriceAfterDiscount == null ? 0 : Double.Parse((orderDetail.OrderedQty * orderDetail.UnitPriceAfterDiscount).Value.ToString("F2")));

                //交货期
                //this.SetRowCell(pageIndex, rowIndex, 7, string.Empty);

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
                    cellStyle.WrapText = true;
                    cellTo1.CellStyle = workbook.CreateCellStyle();
                    cellTo1.CellStyle.CloneStyleFrom(cellStyle);
                }

                rowIndex++;
            }

            //decimal totalPrice = orderDetails.Where(od => od.UnitPriceAfterDiscount != null).Sum(od => od.UnitPriceAfterDiscount.Value * od.OrderedQty);

            Cell cellBlank = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 7));
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

            Cell cellFrom = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, 6));
            Cell cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 6));
            CellStyle cellStyleTo = workbook.CreateCellStyle();
            if (cellFrom.CellStyle != null)
            {
                cellStyleTo.CloneStyleFrom(cellFrom.CellStyle);
                cellStyleTo.BorderBottom = NPOI.SS.UserModel.CellBorderType.NONE;
                cellStyleTo.BorderTop = NPOI.SS.UserModel.CellBorderType.NONE;
                cellStyleTo.BorderRight = NPOI.SS.UserModel.CellBorderType.NONE;
                cellStyleTo.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            }
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleTo);


            CellStyle cellStyleT = workbook.CreateCellStyle();
            cellStyleT.CloneStyleFrom(cellStyleTo);
            cellStyleT.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyleT.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleT.BorderBottom = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyleT.BorderTop = NPOI.SS.UserModel.CellBorderType.NONE;


            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
            CellStyle cellStyle3 = workbook.CreateCellStyle();
            if (cellTo.CellStyle != null)
            {
                cellStyle3.CloneStyleFrom(cellTo.CellStyle);
            }
            cellStyle3.Alignment = HorizontalAlignment.RIGHT;
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyle3);

            //含税了

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleBlank);
            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 7));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleT);

            
            this.SetRowCell(pageIndex, rowIndex, 5, "不含税 金额 人民币：");
            this.SetRowCell(pageIndex, rowIndex++, 6, orderHead.OrderExcludeTaxPrice.ToString("F2"));

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyle3);
            this.SetRowCell(pageIndex, rowIndex, 5, "含17%增值税 总金额 人民币：");

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 6));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleTo);

            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 7));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleT);
            cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleBlank);

            this.SetRowCell(pageIndex, rowIndex++, 6, orderHead.OrderIncludeTaxPrice.ToString("F2"));



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

            this.CopyCellStyle(pageIndex, rowIndex, new int[] { 1, 2, 3, 4, 5, 6 }, cellStyle1);

            cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            this.CopyCellStyle(pageIndex, rowIndex, new int[] { 7 }, cellStyle1);


            //this.SetRowCell(pageIndex, rowIndex++, 0, "总金额(含" + taxRate + "%税)：");

            String totalAmountOfTax = "G" + this.GetRowIndexAbsolute(pageIndex, rowIndex);

            this.SetRowCellFormula(this.GetRowIndexAbsolute(pageIndex, rowIndex++), 0, "\"含" + Decimal.Parse(orderHead.TaxCode).ToString("0.########") + "%增值税 人民币：\"&IF(" + totalAmountOfTax + "<0,\"负\",\"\")&TEXT(TRUNC(ABS(ROUND(" + totalAmountOfTax + ",2))),\"[DBNum2]\")&\"元\"&IF(ISERR(FIND(\".\",ROUND(" + totalAmountOfTax + ",2))),\"\",TEXT(RIGHT(TRUNC(ROUND(" + totalAmountOfTax + ",2)*10)),\"[DBNum2]\"))&IF(ISERR(FIND(\".0\",TEXT(" + totalAmountOfTax + ",\"0.00\"))),\"角\",\"\")&IF(LEFT(RIGHT(ROUND(" + totalAmountOfTax + ",2),3))=\".\",TEXT(RIGHT(ROUND(" + totalAmountOfTax + ",2)),\"[DBNum2]\")&\"分\",\"整\")");


            rowIndex++;

            //输出条款


            if (orderHead.Settlement != null && orderHead.Settlement.Trim().Length > 0)
            {

                string[] settlement = Regex.Split(orderHead.Settlement, "\r\n", RegexOptions.IgnoreCase);

                for (int i = 0; i < settlement.Length; i++)
                {

                    if (settlement[i] != null && settlement[i].Trim().Length > 0)
                    {
                        string[] settlementPart = Regex.Split(settlement[i], "\t", RegexOptions.IgnoreCase);
                        int pos = 0;
                        for (int j = 0; j < settlementPart.Length; j++)
                        {
                            if (settlementPart[j] != null && settlementPart[j].Trim().Length > 0 && settlementPart[j].Trim() != "\n")
                            {
                                this.SetRowCell(pageIndex, rowIndex, pos++, settlementPart[j].Trim());
                            }
                            else if (settlementPart[j] != null && settlementPart[j].Trim().Length == 0)
                            {
                                pos++;
                            }
                        }
                        rowIndex++;

                    }
                }
            }

            rowIndex++;

            //买方

            Cell cell2 = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
            CellStyle cellStyle2 = workbook.CreateCellStyle();
            if (cell2.CellStyle != null)
            {
                cellStyle2.CloneStyleFrom(cell2.CellStyle);
            }
            Font font2 = workbook.CreateFont();
            font2.FontHeightInPoints = (short)12;
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            cellStyle2.SetFont(font2);
            //cellStyle1.Alignment = HorizontalAlignment.LEFT;
            cell2.CellStyle = workbook.CreateCellStyle();
            cell2.CellStyle.CloneStyleFrom(cellStyle2);

            this.SetRowCell(pageIndex, rowIndex, 0, "买方：" + orderHead.BillTo.Address);
            this.AddSeal(orderHead.ApprovedUser, this.GetRowIndexAbsolute(pageIndex, rowIndex + 1), 1);
            //this.SetRowCell(pageIndex, rowIndex, 1, orderHead.BillFrom.Address);

            //卖方
            Cell cell3 = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 3));
            cell3.CellStyle = workbook.CreateCellStyle();
            cell3.CellStyle.CloneStyleFrom(cellStyle2);

            this.SetRowCell(pageIndex, rowIndex, 3, "卖方：" + orderHead.BillFrom.Address);
            //this.SetRowCell(pageIndex, rowIndex++, 5, orderHead.BillTo.Address);



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
         * Param orderHead 要货单头对象
         */
        protected void FillHead(OrderHead orderHead)
        {
            //买方的
            this.SetRowCell(0, 0, orderHead.BillTo.Address);
            //todo 英文名字
            this.SetRowCell(1, 0, orderHead.BillTo.Address2);
            //地址:
            this.SetRowCell(2, 1, orderHead.ShipTo.Address);
            //电话:
            this.SetRowCell(3, 1, orderHead.ShipTo.TelephoneNumber);
            //邮政编码:	
            this.SetRowCell(3, 5, orderHead.ShipTo.PostalCode);
            //传真:
            this.SetRowCell(4, 1, orderHead.ShipTo.Fax);
            //电子邮件：
            this.SetRowCell(4, 5, orderHead.ShipTo.Email);
            //卖方的
            this.SetRowCell(6, 1, orderHead.BillFrom.Address);
            //电话:
            this.SetRowCell(6, 5, orderHead.ShipFrom.TelephoneNumber);
            //收件人:	
            this.SetRowCell(7, 2, orderHead.ShipFrom.ContactPersonName);
            //传真: 
            this.SetRowCell(7, 5, orderHead.ShipFrom.Fax);
            //贵司报价号:	
            this.SetRowCell(8, 2, orderHead.ExternalOrderNo);
            //最终用户：
            this.SetRowCell(8, 5, orderHead.Customer);
            //我司询价号:
            this.SetRowCell(9, 2, orderHead.ReferenceOrderNo);
            //日期：
            this.SetRowCell(9, 5, orderHead.ReleaseDate == null ? string.Empty : orderHead.ReleaseDate.Value.ToString("yyyy-MM-dd"));
            //采购订单号：SDW-TSP10007
            this.SetRowCell(11, 0, "采购订单号：" + orderHead.OrderNo);

            this.SetRowCell(12, 0, "尊敬的" + orderHead.ShipFrom.ContactPersonName + ":");
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {



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

    public partial class RepPurchaseMgrE : com.Sconit.Service.Report.Impl.RepPurchaseMgr, IReportBaseMgrE
    {


    }
}

#endregion
