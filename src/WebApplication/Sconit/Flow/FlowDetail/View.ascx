<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="MasterData_FlowDetail_View" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<div id="floatdiv">
    <asp:FormView ID="FV_FlowDetail" runat="server" DataSourceID="ODS_FlowDetail" DefaultMode="ReadOnly"
        OnDataBound="FV_FlowDetail_DataBound" DataKeyNames="Id">
        <ItemTemplate>
            <fieldset>
                <legend>${MasterData.Flow.FlowDetail.Basic.Info}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSeq" runat="server" Text="${MasterData.Flow.FlowDetail.Sequence}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbSeq" runat="server" Text='<%# Bind("Sequence") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblItemCode" runat="server" Text="${MasterData.Flow.FlowDetail.ItemCode}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbItemCode" runat="server" CodeField="Item.Code" DescField="Item.Description" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblUom" runat="server" Text="${MasterData.Flow.FlowDetail.Uom}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbUom" runat="server" CodeField="Uom.Code" DescField="Uom.Code" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblUC" runat="server" Text="${MasterData.Flow.FlowDetail.UnitCount}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbUc" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHuLotSize" runat="server" Text="${MasterData.Flow.FlowDetail.HuLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbHuLotSize" runat="server" Text='<%# Bind("HuLotSize") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblBrand" runat="server" Text="${MasterData.Flow.FlowDetail.Brand}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbBrand" runat="server" Text='<%# Bind("Brand") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblManufacturer" runat="server" Text="${MasterData.Flow.FlowDetail.Manufacturer}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbManufacturer" runat="server" Text='<%# Bind("Manufacturer") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblStartDate" runat="server" Text="${MasterData.Flow.FlowDetail.StartDate}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbStartDate" runat="server" Text='<%# Bind("StartDate") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblEndDate" runat="server" Text="${MasterData.Flow.FlowDetail.EndDate}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbEndDate" runat="server" Text='<%# Bind("EndDate") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSafeStock" runat="server" Text="${MasterData.Flow.FlowDetail.SafeStock}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbSafeStock" runat="server" Text='<%# Bind("SafeStock","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblMaxStock" runat="server" Text="${MasterData.Flow.FlowDetail.MaxStock}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbMaxStock" runat="server" Text='<%# Bind("MaxStock","{0:0.########}") %>' />
                        </tdLiteral
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblMinLotSize" runat="server" Text="${MasterData.Flow.FlowDetail.MinLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbMinLotSize" runat="server" Text='<%# Bind("MinLotSize","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblOrderLotSize" runat="server" Text="${MasterData.Flow.FlowDetail.OrderLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbOrderLotSize" runat="server" Text='<%# Bind("OrderLotSize","{0:0.########}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblOrderGoodsReceiptLotSize" runat="server" Text="${MasterData.Flow.FlowDetail.GoodsReceiptLotSize}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbOrderGoodsReceiptLotSize" runat="server" Text='<%# Bind("GoodsReceiptLotSize","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblRefItemCode" runat="server" Text="${MasterData.Flow.FlowDetail.RefItemCode}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbRefItemCode" runat="server" Text='<%# Bind("ReferenceItemCode") %>' />
                        </td>
                    </tr>
                    <tr id="trBom" runat="server" visible="false">
                        <td class="td01">
                            <asp:Literal ID="lblBom" runat="server" Text="${MasterData.Flow.FlowDetail.Bom}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbBom" runat="server" CodeField="Bom.Code" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblBatchSize" runat="server" Text="${MasterData.Flow.FlowDetail.BatchSize}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbBatchSize" runat="server" Text='<%# Bind("BatchSize","{0:0.########}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPackageVol" runat="server" Text="${MasterData.Flow.FlowDetail.PackageVolumn}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbPackageVol" runat="server" Text='<%# Bind("PackageVolumn","{0:0.########}") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblTimeUnit" runat="server" Text="${MasterData.Flow.FlowDetail.TimeUnit}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbTimeUnit" runat="server" Text='<%# Bind("TimeUnit") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblRoundUpOpt" runat="server" Text="${MasterData.Flow.FlowDetail.RoundUpOpt}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbRoundUpOpt" runat="server" Text='<%# Bind("RoundUpOption") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblProjectDescription" runat="server" Text="${MasterData.Flow.FlowDetail.ProjectDescription}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbProjectDescription" runat="server" Text='<%# Bind("ProjectDescription") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIdMark" runat="server" Text="${MasterData.Flow.FlowDetail.IdMark.Procurement}:"
                                Visible="false" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbIdMark" runat="server" Text='<%# Bind("IdMark") %>' Visible="false" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblBarCodeType" runat="server" Text="${MasterData.Flow.FlowDetail.BarTypeCode}:"
                                Visible="false" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="ddlRMBarCodeType" runat="server" Visible="false" Code="RMBarCodeType"
                                Text='<%# Bind("BarCodeType") %>' />
                            <sc1:CodeMstrLabel ID="ddlFGBarCodeType" runat="server" Visible="false" Code="FGBarCodeType"
                                Text='<%# Bind("BarCodeType") %>' />
                        </td>
                    </tr>
                    <tr id="trCustomer" runat="server" visible="false">
                        <td class="td01">
                            <asp:Literal ID="lblCustomer" runat="server" Text="${MasterData.Flow.FlowDetail.Customer}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCustomer" runat="server" CodeField="Customer.Code" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCustomerItemCode" runat="server" Text="${MasterData.Flow.FlowDetail.CustomerItemCode}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCustomerItemCode" runat="server" Text='<%# Bind("CustomerItemCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblRemark" runat="server" Text="${MasterData.Flow.FlowDetail.Remark}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <asp:Literal ID="tbRemark" runat="server" Text='<%# Bind("Remark") %>' />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--采购--%>
            <fieldset id="fdProcurement" runat="server" visible="false">
                <legend>${MasterData.Flow.FlowDetail.Default.Value}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblProcurementLocTo" runat="server" Text="${MasterData.Flow.FlowDetail.LocTo}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbProcurementLocTo" runat="server" CodeField="LocationTo.Code"
                                DescField="LocationTo.Name" />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--销售--%>
            <fieldset id="fdDistribution" runat="server" visible="false">
                <legend>${MasterData.Flow.FlowDetail.Default.Value}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblDistributionLocFrom" runat="server" Text="${MasterData.Flow.FlowDetail.LocFrom}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbDistributionLocFrom" runat="server" CodeField="LocationFrom.Code"
                                DescField="LocationFrom.Name" />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--生产--%>
            <fieldset id="fdProduction" runat="server" visible="false">
                <legend>${MasterData.Flow.FlowDetail.Default.Value}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblProductionLocFrom" runat="server" Text="${MasterData.Flow.FlowDetail.LocFrom.Production}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbProductionLocFrom" runat="server" CodeField="LocationFrom.Code"
                                DescField="LocationFrom.Name" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblProductionLocTo" runat="server" Text="${MasterData.Flow.FlowDetail.LocTo.Production}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbProductionLocTo" runat="server" CodeField="LocationTo.Code"
                                DescField="LocationTo.Name" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%--移库--%>
            <fieldset id="fdTransfer" runat="server" visible="false">
                <legend>${MasterData.Flow.FlowDetail.Default.Value}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblTransferLocFrom" runat="server" Text="${MasterData.Flow.FlowDetail.LocFrom}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbTransferLocFrom" runat="server" CodeField="LocationFrom.Code"
                                DescField="LocationFrom.Name" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblTransferLocTo" runat="server" Text="${MasterData.Flow.FlowDetail.LocTo}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbTransferLocTo" runat="server" CodeField="LocationTo.Code"
                                DescField="LocationTo.Name" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fdOption" runat="server">
                <legend>${MasterData.Flow.FlowDetail.Control.Option}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsAutoCreate" runat="server" Text="${MasterData.Flow.FlowDetail.AutoCreate}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsAutoCreate" runat="server" Checked='<%# Bind("IsAutoCreate") %>'
                                Enabled="false" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblNeedInspect" runat="server" Text="${MasterData.Flow.FlowDetail.NeedInspect}:"
                                Visible="false" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbNeedInspect" runat="server" Checked='<%# Bind("NeedInspection") %>'
                                Visible="false" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblBillSettleTerm" runat="server" Text="${MasterData.Flow.FlowDetail.BillSettleTerm}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="ddlBillSettleTerm" Code="BillSettleTerm" runat="server" Value='<%# Bind("BillSettleTerm") %>' />
                        </td>
                        <td class="ttd01">
                        </td>
                        <td class="ttd02">
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblOddShipOption" runat="server" Text="${MasterData.Flow.FlowDetail.OddShipOption}:"
                                Visible="false" />
                        </td>
                        <td class="ttd02">
                            <sc1:CodeMstrLabel ID="ddlShipOption" Code="ShipOption" runat="server" Value='<%# Bind("OddShipOption") %>'
                                Visible="false" />
                        </td>
                        <td class="ttd01">
                        </td>
                        <td class="ttd02">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="tablefooter">
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </div>
        </ItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_FlowDetail" runat="server" TypeName="com.Sconit.Web.FlowDetailMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.FlowDetail" SelectMethod="FindFlowDetail">
    <SelectParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
