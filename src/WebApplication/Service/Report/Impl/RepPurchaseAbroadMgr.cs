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
     * 采购单(海外)
     *       适用于天熙
     */
    [Transactional]
    public class RepPurchaseAbroadMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }

        private static int ITEM_DESC2_ROW_LEN = 11;

        private static int ITEM_SPEC_ROW_LEN = 20;

        public RepPurchaseAbroadMgr()
        {
            //明细部分的行数
            //this.pageDetailRowCount = 10;
            //列数   1起始
            this.columnCount = 6;
            //报表头的行数  1起始
            this.headRowCount = 16;
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

            this.FillHead(orderHead);

            //自动换行
            //style.WrapText = true;

            int pageIndex = 1;
            int rowIndex = 0;
            int seq = 1;

            this.SetRowCell(0, headRowCount - 1, 3, "Unit Price(" + orderHead.Currency.Code + ")");
            this.SetRowCell(0, headRowCount - 1, 5, "Amount(" + orderHead.Currency.Code + ")");

            foreach (OrderDetail orderDetail in orderDetails)
            {
                //序号	
                this.SetRowCell(pageIndex, rowIndex, 0, seq++);
                //品名
                this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Item.Desc2);

                //规格及描述	
                this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Spec);

                //不含税单价(人民币)	
                this.SetRowCell(pageIndex, rowIndex, 3, orderDetail.UnitPriceAfterDiscount == null ? 0 : Double.Parse(orderDetail.UnitPriceAfterDiscount.Value.ToString("F2")));

                //数量	
                this.SetRowCell(pageIndex, rowIndex, 4, double.Parse(orderDetail.OrderedQty.ToString("0.########")));

                //不含税总价(人民币)
                this.SetRowCell(pageIndex, rowIndex, 5, orderDetail.UnitPriceAfterDiscount == null ? 0 : Double.Parse((orderDetail.OrderedQty * orderDetail.UnitPriceAfterDiscount.Value).ToString("F2")));

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

            Cell cellBlank = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
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


            this.SetRowCell(pageIndex, rowIndex, 4, "Sub total(" + orderHead.Currency.Code + "):");

            Cell cellFrom = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, 5));
            Cell cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
            CellStyle cellStyleTo = workbook.CreateCellStyle();
            if (cellFrom.CellStyle != null)
            {
                cellStyleTo.CloneStyleFrom(cellFrom.CellStyle);
                cellStyleTo.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
                cellStyleTo.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
                cellStyleTo.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
                cellStyleTo.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
            }
            cellTo.CellStyle = workbook.CreateCellStyle();
            cellTo.CellStyle.CloneStyleFrom(cellStyleTo);


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

            cell1.CellStyle = workbook.CreateCellStyle();
            cell1.CellStyle.CloneStyleFrom(cellStyle1);

            cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.Alignment = HorizontalAlignment.RIGHT;
            cellStyle1.BorderTop = NPOI.SS.UserModel.CellBorderType.THIN;
            cellStyle1.BorderLeft = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderRight = NPOI.SS.UserModel.CellBorderType.NONE;
            cellStyle1.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;

            this.CopyCellStyle(pageIndex, rowIndex, new int[] { 1, 2, 3, 4 }, cellStyle1);

            this.SetRowCell(pageIndex, rowIndex, 5, orderHead.OrderAmountAfterDiscount.ToString("F2"));

            rowIndex++;
            //rowIndex++;
            //this.SetRowCell(pageIndex, rowIndex, 1, "Remark:");

            //rowIndex++;
            //this.SetRowCell(pageIndex, rowIndex, 0, "1");
            //this.SetRowCell(pageIndex, rowIndex, 1, "Please delivery the inspection reports for every tool with your shipment.");

            //rowIndex++;

            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "When processing this order, please proceed with following instructions :");
            rowIndex++;
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
            this.SetRowCell(pageIndex, rowIndex, 0, "Please confirm this Purchase Order and delivery time by return fax.");

            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "Sincerely yours,");

            rowIndex++;

            rowIndex++;

            //买方
            this.SetRowCell(pageIndex, rowIndex, 0, "Buyer:");

            //卖方
            this.SetRowCell(pageIndex, rowIndex, 3, "Saler:");

            rowIndex++;

            {
                this.SetMergedRegion(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0), this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 2));
                Cell cell = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
                CellStyle style = workbook.CreateCellStyle();
                if (cell.CellStyle != null)
                {
                    style.CloneStyleFrom(cell.CellStyle);
                }
                style.ShrinkToFit = true;
                cell.CellStyle = workbook.CreateCellStyle();
                cell.CellStyle.CloneStyleFrom(style);
                this.SetRowCell(pageIndex, rowIndex, 0, orderHead.BillTo.Address2);

                this.SetMergedRegion(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 3), this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
                cell = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 3));
                cell.CellStyle = workbook.CreateCellStyle();
                cell.CellStyle.CloneStyleFrom(style);
                this.SetRowCell(pageIndex, rowIndex, 3, orderHead.BillFrom.Address2);

            }
            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "Signiture:");
            this.AddSeal("default2.png", this.GetRowIndexAbsolute(pageIndex, rowIndex), 1);
            this.SetRowCell(pageIndex, rowIndex, 3, "Signiture:");


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
            this.SetRowCell(2, 1, orderHead.ShipTo.Address2);
            //电话:
            this.SetRowCell(3, 1, orderHead.ShipTo.TelephoneNumber);
            //E-mail:
            this.SetRowCell(3, 4, orderHead.ShipTo.Email);

            //传真:
            this.SetRowCell(4, 1, orderHead.ShipTo.Fax);

            //卖方的
            this.SetRowCell(6, 1, orderHead.PartyFrom.Name);
            //电话:
            this.SetRowCell(6, 4, orderHead.ShipFrom.TelephoneNumber);
            //收件人:	
            this.SetRowCell(7, 1, orderHead.ShipFrom.ContactPersonName);
            //传真: 
            this.SetRowCell(7, 4, orderHead.ShipFrom.Fax);
            //E-mail:	
            this.SetRowCell(8, 1, orderHead.ShipFrom.Email);
            //Date:
            this.SetRowCell(8, 4, orderHead.ReleaseDate == null ? string.Empty : orderHead.ReleaseDate.Value.ToString("yyyy-MM-dd"));
            //Address:
            this.SetRowCell(9, 1, orderHead.ShipFrom.Address);

            //采购订单号:SDW-TSP10007
            this.SetRowCell(11, 0, "Purchase Order No.:" + orderHead.OrderNo);

            this.SetRowCell(12, 0, "Dear " + orderHead.ShipFrom.ContactPersonName + ":");
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

    public partial class RepPurchaseAbroadMgrE : com.Sconit.Service.Report.Impl.RepPurchaseAbroadMgr, IReportBaseMgrE
    {


    }
}

#endregion
