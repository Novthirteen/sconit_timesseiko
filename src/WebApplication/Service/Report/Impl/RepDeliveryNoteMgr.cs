
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Service;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Service.Distribution;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Report;
using NPOI.SS.UserModel;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepDeliveryNoteMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }

        public RepDeliveryNoteMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 30;
            //列数   1起始
            this.columnCount = 10;
            //报表头的行数  1起始
            this.headRowCount = 13;
            //报表尾的行数  1起始
            this.bottomRowCount = 10;

        }


        public override bool FillValues(String templateFileName, IList<object> list)
        {
            try
            {
                if (list == null || list.Count < 2) return false;
                this.rowCount = pageDetailRowCount + bottomRowCount;


                InProcessLocation inProcessLocation = (InProcessLocation)list[0];
                IList<InProcessLocationDetail> inProcessLocationDetailList = (IList<InProcessLocationDetail>)list[1];

                if (inProcessLocation == null
                    || inProcessLocationDetailList == null || inProcessLocationDetailList.Count == 0)
                {
                    return false;
                }

                if (inProcessLocation.BillFrom != null
                    && inProcessLocation.BillFrom.Code != null
                    && !inProcessLocation.BillFrom.Code.ToUpper().Equals("TS"))
                {
                    templateFileName = templateFileName.Substring(0, templateFileName.IndexOf(".xls")) + "2" + templateFileName.Substring(templateFileName.IndexOf(".xls"));
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
         * Param list [0]InProcessLocation
         *            [1]inProcessLocationDetailList
         */
        [Transaction(TransactionMode.Requires)]
        protected override void FillValuesImpl(String templateFileName, IList<object> list)
        {



            InProcessLocation inProcessLocation = (InProcessLocation)list[0];
            IList<InProcessLocationDetail> inProcessLocationDetailList = (IList<InProcessLocationDetail>)list[1];

            if (inProcessLocation == null
                || inProcessLocationDetailList == null || inProcessLocationDetailList.Count == 0)
            {
                throw new Exception();
            }

            //this.SetRowCellBarCode(0, 1, 6);
            this.barCodeFontName = this.GetBarcodeFontName(1, 6);

            //this.CopyPage(inProcessLocationDetailList.Count);

            this.FillHead(inProcessLocation);

            if (inProcessLocation.BillFrom != null
                 && inProcessLocation.BillFrom.Code != null
                 && !inProcessLocation.BillFrom.Code.ToUpper().Equals("TS"))
            {
                //抬头
                this.SetRowCell(0, 0, inProcessLocation.BillFrom.Address);
            }


            int pageIndex = 1;
            int rowIndex = 0;
            int rowTotal = 0;
            //ASN号:

            CellStyle style = null;
            foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
            {
                OrderDetail orderDetail = inProcessLocationDetail.OrderLocationTransaction.OrderDetail;

                //订单号(销售发运显示客户订单号,其它为订单号)
                //if (orderDetail.OrderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
                //    this.SetRowCell(pageIndex, rowIndex, 0, orderDetail.OrderHead.ExternalOrderNo);
                //else
                this.SetRowCell(pageIndex, rowIndex, 0, orderDetail.OrderHead.OrderNo);

                //序号.	
                this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Sequence.ToString());

                //"零件号Item Code"	
                //this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Code);

                //品名
                this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Desc1);

                //型号/规格
                this.SetRowCell(pageIndex, rowIndex, 3, orderDetail.Item.Spec);

                //品牌
                this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.Item.Brand);

                //定货数量	
                this.SetRowCell(pageIndex, rowIndex, 5, double.Parse(inProcessLocationDetail.OrderLocationTransaction.OrderedQty.ToString("0.########")));
                //已交数量
                if (inProcessLocationDetail.OrderLocationTransaction.AccumulateQty.HasValue)
                {
                    this.SetRowCell(pageIndex, rowIndex, 6, double.Parse((inProcessLocationDetail.OrderLocationTransaction.AccumulateQty - inProcessLocationDetail.Qty).Value.ToString("0.########")));
                }
                //发货数
                this.SetRowCell(pageIndex, rowIndex, 7, double.Parse(inProcessLocationDetail.Qty.ToString("0.########")));
                //剩余数量
                this.SetRowCell(pageIndex, rowIndex, 8, double.Parse(inProcessLocationDetail.OrderLocationTransaction.RemainQty.ToString("0.########")));

                //实收数

                Cell cell = this.GetCell(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 0));
                if (style == null)
                {
                    style = workbook.CreateCellStyle();
                    style.CloneStyleFrom(cell.CellStyle);
                    style.BorderBottom = NPOI.SS.UserModel.CellBorderType.THIN;
                    style.BorderLeft = NPOI.SS.UserModel.CellBorderType.THIN;
                    style.BorderRight = NPOI.SS.UserModel.CellBorderType.THIN;
                    style.VerticalAlignment = VerticalAlignment.CENTER;
                    style.WrapText = true;
                }
                //this.SetMergedRegion(this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 3), this.GetRowIndexAbsolute(pageIndex, rowIndex), this.GetColumnIndexAbsolute(pageIndex, 5));
                this.CopyCellStyle(pageIndex, rowIndex, this.columnCount, style);

                if (this.isPageBottom(rowIndex, rowTotal))//页的最后一行
                {
                    //实际到货时间:
                    //this.SetRowCell(pageIndex, rowIndex, , "");

                    pageIndex++;
                    rowIndex = 0;
                }
                else
                {
                    rowIndex++;
                }
                rowTotal++;
            }

            rowIndex++;

            //发货人签字:
            this.SetRowCell(pageIndex, rowIndex, 0, "发货人签字：");
            this.AddSeal(inProcessLocation.ApprovedUser, this.GetRowIndexAbsolute(pageIndex, rowIndex), 2);
            
            //收货人签字:
            this.SetRowCell(pageIndex, rowIndex, 4, "收货人签字：");

            rowIndex++;
            //发货日期
            this.SetRowCell(pageIndex, rowIndex, 0, "发货日期：");
            //收货日期:
            this.SetRowCell(pageIndex, rowIndex, 4, "收货日期：");

            rowIndex++;
            rowIndex++;
            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "备注:");
            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "a. 内附货物发票，发票号为: No._____________.");
            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "b. 收到货物，货物完好无损，确认后请回传021-38870739。");
            rowIndex++;
            this.SetRowCell(pageIndex, rowIndex, 0, "c. 货物如有不符，请于七日内通知，逾期视同验收无误。");

            this.sheet.DisplayGridlines = false;
            this.sheet.IsPrintGridlines = false;

            if (inProcessLocation.IsPrinted == null || inProcessLocation.IsPrinted == false)
            {
                inProcessLocation.IsPrinted = true;
                inProcessLocationMgrE.UpdateInProcessLocation(inProcessLocation);
            }

        }


        /*
         * 填充报表头
         * 
         * Param InProcessLocation 头对象
         */
        protected void FillHead(InProcessLocation inProcessLocation)
        {

            string asnCode = Utility.BarcodeHelper.GetBarcodeStr(inProcessLocation.IpNo, this.barCodeFontName);
            //ASN号:
            this.SetRowCell(1, 6, asnCode);
            //this.AddPicture
            //ASN No.:
            this.SetRowCell(2, 6, inProcessLocation.IpNo);

            //窗口时间 Window Time:
            //this.SetRowCell(4, 6, inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead.WindowTime.ToString("yyyy-MM-dd HH:mm"));

            //供应商代码 Supplier Code:
            this.SetRowCell(4, 2, inProcessLocation.PartyFrom == null ? string.Empty : inProcessLocation.PartyFrom.Code);
            //供应商名称 Supplier Name:		
            this.SetRowCell(5, 2, inProcessLocation.PartyFrom == null ? string.Empty : inProcessLocation.PartyFrom.Name);
            //供应商地址 Supplier Address:		
            this.SetRowCell(6, 2, inProcessLocation.ShipFrom == null ? string.Empty : inProcessLocation.ShipFrom.Address);
            //供应商联系人 Contact:	
            if (inProcessLocation.InProcessLocationDetails.Count > 0
                && inProcessLocation.InProcessLocationDetails[0] != null
                && inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction != null
                && inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail != null
                && inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead != null
                && inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead.CreateUser != null)
            {
                this.SetRowCell(7, 2, inProcessLocation.InProcessLocationDetails[0].OrderLocationTransaction.OrderDetail.OrderHead.CreateUser.Name);
            }
            //供应商联系人 Telephone:		
            this.SetRowCell(8, 2, inProcessLocation.ShipFrom == null ? string.Empty : inProcessLocation.ShipFrom.TelephoneNumber);
            //供应商传真：		:		
            this.SetRowCell(9, 2, inProcessLocation.ShipFrom == null ? string.Empty : inProcessLocation.ShipFrom.Fax);

            //货运公司：				
            this.SetRowCell(10, 2, inProcessLocation.TransportCompany);


            //客户代码 Customer Code:
            this.SetRowCell(4, 6, inProcessLocation.PartyTo == null ? string.Empty : inProcessLocation.PartyTo.Code);
            //客户名称 Customer Name:		
            this.SetRowCell(5, 6, inProcessLocation.PartyTo == null ? string.Empty : inProcessLocation.PartyTo.Name);
            //客户地址：	
            this.SetRowCell(6, 6, inProcessLocation.ShipTo == null ? string.Empty : inProcessLocation.ShipTo.Address);
            //客户联系人：
            this.SetRowCell(7, 6, inProcessLocation.ShipTo == null ? string.Empty : inProcessLocation.ShipTo.ContactPersonName);
            //客户电话 Telephone:		
            this.SetRowCell(8, 6, inProcessLocation.ShipTo == null ? string.Empty : inProcessLocation.ShipTo.TelephoneNumber);
            //传真
            this.SetRowCell(9, 6, inProcessLocation.ShipTo == null ? string.Empty : inProcessLocation.ShipTo.Fax);
            //货运单号：
            this.SetRowCell(10, 6, inProcessLocation.ExpressNo);
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
            InProcessLocation inProcessLocation = inProcessLocationMgrE.LoadInProcessLocation(code, true);
            if (inProcessLocation != null)
            {
                list.Add(inProcessLocation);
            }
            return list;
        }

    }
}




#region Extend Class




namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepDeliveryNoteMgrE : com.Sconit.Service.Report.Impl.RepDeliveryNoteMgr, IReportBaseMgrE
    {

    }


}



#endregion
