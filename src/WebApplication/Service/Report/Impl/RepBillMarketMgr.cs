using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using NPOI.SS.UserModel;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepBillMarketMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IBillMgrE billMgrE { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }


        public RepBillMarketMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 27;
            //列数   1起始
            this.columnCount = 10;
            //报表头的行数  1起始
            this.headRowCount = 5;
            //报表尾的行数  1起始
            this.bottomRowCount = 1;

        }

        /**
         * 填充报表
         * 
         * Param list [0]bill
         * Param list [0]IList<BillDetail>           
         */
        protected override void FillValuesImpl(String templateFileName, IList<object> list)
        {

            if (list == null || list.Count < 2) throw new Exception();

                Bill bill = (Bill)(list[0]);
                IList<BillDetail> billDetails = (IList<BillDetail>)(list[1]);


                if (bill == null
                    || billDetails == null || billDetails.Count == 0)
                {
                    throw new Exception();
                }

                //this.CopyPage(billDetails.Count);

                this.FillHead(bill);

                int pageIndex = 1;
                int rowIndex = 0;


                decimal totalAmount = billDetails.Sum(bd => bd.OrderAmount);

                foreach (BillDetail billDetail in billDetails)
                {
                    //客户回单号	
                    this.SetRowCell(pageIndex, rowIndex, 0, billDetail.ActingBill.ExternalReceiptNo);
                    //销售单号	
                    this.SetRowCell(pageIndex, rowIndex, 1, billDetail.ActingBill.OrderHead.OrderNo);
                    if (billDetail.ActingBill.ReceiptNo != null && billDetail.ActingBill.ReceiptNo.Length > 0)
                    {
                        Receipt receipt = receiptMgrE.LoadReceipt(billDetail.ActingBill.ReceiptNo);
                        //发货单号
                        this.SetRowCell(pageIndex, rowIndex, 2, receipt.ReferenceIpNo);
                    }

                    //零件号
                    this.SetRowCell(pageIndex, rowIndex, 3, billDetail.ActingBill.Item.Code);

                    //品名
                    this.SetRowCell(pageIndex, rowIndex, 4, billDetail.ActingBill.Item.Desc1);

                    //型号/规格
                    this.SetRowCell(pageIndex, rowIndex, 5, billDetail.ActingBill.Item.Spec);

                    //品牌
                    this.SetRowCell(pageIndex, rowIndex, 6, billDetail.ActingBill.Item.Brand);
                    //单价
                    this.SetRowCell(pageIndex, rowIndex, 7, Double.Parse(billDetail.UnitPrice.ToString("0.00")));

                    //数量	
                    this.SetRowCell(pageIndex, rowIndex, 8, Double.Parse(billDetail.BilledQty.ToString("0.########")));
                    //总价
                    this.SetRowCell(pageIndex, rowIndex, 9, Double.Parse(billDetail.Amount.ToString("0.00")));

                    for (int i = 0; i < this.columnCount; i++)
                    {
                        Cell cellFrom = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, i));
                        Cell cellTo = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, i));
                        CellStyle cellStyle = workbook.CreateCellStyle();
                        if (cellFrom.CellStyle != null)
                        {
                            cellStyle.CloneStyleFrom(cellFrom.CellStyle);
                        }
                        cellStyle.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
                        cellStyle.WrapText = true;
                        cellTo.CellStyle = workbook.CreateCellStyle();
                        cellTo.CellStyle.CloneStyleFrom(cellStyle);
                    }

                    rowIndex++;
                }


                Cell cellFrom1 = this.GetCell(this.GetRowIndexAbsolute(1, 0), this.GetColumnIndexAbsolute(1, 9));
                Cell cellTo1 = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 9));
                CellStyle cellStyle1 = workbook.CreateCellStyle();
                if (cellFrom1.CellStyle != null)
                {
                    cellStyle1.CloneStyleFrom(cellFrom1.CellStyle);
                }
                cellTo1.CellStyle = workbook.CreateCellStyle();
                cellTo1.CellStyle.CloneStyleFrom(cellStyle1);
                this.SetRowCell(pageIndex, rowIndex, 9, Double.Parse(totalAmount.ToString("0.00")));

                rowIndex++;

                //实际到货时间:
                this.SetRowCell(pageIndex, rowIndex, 1, "对账员：");
                //收货人签字:
                this.SetRowCell(pageIndex, rowIndex, 8, "主管：");


                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;
            
        }


        /*
         * 填充报表头
         * 
         * Param repack 报验单头对象
         */
        private void FillHead(Bill bill)
        {
            if (bill.BillDetails != null && bill.BillDetails.Count > 0
                && bill.BillDetails[0].ActingBill != null
                && bill.BillDetails[0].ActingBill.OrderHead != null
                && bill.BillDetails[0].ActingBill.OrderHead.BillFrom != null)
            {
                this.SetRowCell(0, 0, bill.BillDetails[0].ActingBill.OrderHead.BillFrom.Address);
            }
            this.SetRowCell(3, 2, bill.BillAddress.Party.Name);
            this.SetRowCell(3, 8, bill.BillNo);
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //对账员：
            //this.CopyCell(pageIndex, 37, 1, "B38");
            //主管：
            //this.CopyCell(pageIndex, 37, 7, "H38");

        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            Bill bill = billMgrE.LoadBill(code, true);
            if (bill != null)
            {
                list.Add(bill);
                list.Add(bill.BillDetails);
            }
            return list;
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBillMarketMgrE : com.Sconit.Service.Report.Impl.RepBillMarketMgr, IReportBaseMgrE
    {

    }
}

#endregion
