﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_Flow_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Flow" runat="server" DataSourceID="ODS_Flow" DefaultMode="Insert"
        DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Flow.Basic.Info}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Flow.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="${MasterData.Flow.Code.Required}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${MasterData.Flow.Code.Exists}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkFlowExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Address.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Bind("IsActive") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${MasterData.Flow.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                                CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="${MasterData.Flow.Description.Required}"
                                Display="Dynamic" ControlToValidate="tbDescription" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblRefFlow" runat="server" Text="${MasterData.Flow.ReferenceFlow}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbRefFlow" runat="server" Visible="true" DescField="Description"
                                ValueField="Code" ServicePath="FlowMgr.service" ServiceMethod="GetAllFlow" Width="250" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Flow.Party.From.Supplier}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" Width="250" DescField="Name"
                                ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetFromParty"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvPartyFrom" runat="server" ErrorMessage="${MasterData.Flow.Party.From.Required}"
                                Display="Dynamic" ControlToValidate="tbPartyFrom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Flow.Party.To.Region}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbPartyTo" runat="server" Visible="true" Width="250" DescField="Name"
                                ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetToParty" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvPartyTo" runat="server" ErrorMessage="${MasterData.Flow.Party.To.Required}"
                                Display="Dynamic" ControlToValidate="tbPartyTo" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblShipFrom" runat="server" Text="${MasterData.Flow.Ship.From}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbShipFrom" runat="server" Visible="true" DescField="Address" ValueField="Code"
                                Width="250" ServicePath="AddressMgr.service" ServiceMethod="GetShipAddress" ServiceParameter="string:#tbPartyFrom"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvShipFrom" runat="server" ErrorMessage="${MasterData.Flow.Ship.From.Required}"
                                Display="Dynamic" ControlToValidate="tbShipFrom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblShipTo" runat="server" Text="${MasterData.Flow.Ship.To}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbShipTo" runat="server" Visible="true" DescField="Address" ValueField="Code"
                                Width="250" ServicePath="AddressMgr.service" ServiceMethod="GetShipAddress" ServiceParameter="string:#tbPartyTo"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvShipTo" runat="server" ErrorMessage="${MasterData.Flow.Ship.To.Required}"
                                Display="Dynamic" ControlToValidate="tbShipTo" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lbCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbCurrency" runat="server" Visible="true" DescField="Name" ValueField="Code"
                                ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>${MasterData.Flow.Default.Value}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblLocTo" runat="server" Text="${MasterData.Flow.Location.To}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbLocTo" runat="server" Visible="true" DescField="Name" ValueField="Code"
                                CssClass="inputRequired" Width="250" ServicePath="LocationMgr.service" ServiceMethod="GetLocation"
                                ServiceParameter="string:#tbPartyTo" />
                            <asp:RequiredFieldValidator ID="rfvLocTo" runat="server" ErrorMessage="${MasterData.Flow.Location.To.Required}"
                                Display="Dynamic" ControlToValidate="tbLocTo" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblOrderTemplate" runat="server" Text="${MasterData.Flow.OrderTemplate}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlOrderTemplate" Code="OrderTemplate" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblBillFrom" runat="server" Text="${MasterData.Flow.Bill.From}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBillFrom" runat="server" Visible="true" DescField="Address" ValueField="Code"
                                ServiceParameter="string:#tbPartyFrom" Width="250" ServicePath="AddressMgr.service"
                                ServiceMethod="GetBillAddress" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvBillFrom" runat="server" ErrorMessage="${MasterData.Flow.BillFrom.Required}"
                                Display="Dynamic" ControlToValidate="tbBillFrom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblAsnTemplate" runat="server" Text="${MasterData.Flow.ASNTemplate}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlAsnTemplate" Code="AsnTemplate" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPriceListFrom" runat="server" Text="${MasterData.Flow.PriceList.From}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbPriceListFrom" runat="server" Visible="true" Width="250" DescField="Code"
                                ServiceParameter="string:#tbBillFrom" ValueField="Code" ServicePath="PurchasePriceListMgr.service"
                                ServiceMethod="GetAllPurchasePriceList" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvPriceListFrom" runat="server" ErrorMessage="${MasterData.Flow.PriceListFrom.Required}"
                                Display="Dynamic" ControlToValidate="tbPriceListFrom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblReceiptTemplate" runat="server" Text="${MasterData.Flow.ReceiptTemplate}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlReceiptTemplate" Code="ReceiptTemplate" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblBillTo" runat="server" Text="${MasterData.Flow.BillTo.Procurement}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBillTo" runat="server" Visible="true" DescField="Address" ValueField="Code"
                                ServiceParameter="string:#tbPartyTo" Width="250" ServicePath="AddressMgr.service"
                                ServiceMethod="GetBillAddress" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfv" runat="server" ErrorMessage="${MasterData.Flow.BillTo.Required}"
                                Display="Dynamic" ControlToValidate="tbBillTo" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblHuTemplate" runat="server" Text="${MasterData.Flow.HuTemplate}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlHuTemplate" Code="HuTemplate" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblSettlement" runat="server" Text="${MasterData.Flow.Settlement}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <asp:TextBox ID="tbSettlement" runat="server" Text='<%# Bind("Settlement") %>' TextMode="MultiLine"
                                Rows="8" Columns="80" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>${MasterData.Flow.Control.Option}</legend>
                <table class="mtable">
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoCreate" runat="server" Text="${MasterData.Flow.AutoCreate}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoCreate" runat="server" Checked='<%# Bind("IsAutoCreate") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoBill" runat="server" Text="${MasterData.Flow.AutoBill}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoBill" runat="server" Checked='<%# Bind("IsAutoBill") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoShip" runat="server" Text="${MasterData.Flow.AutoShip}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoShip" runat="server" Checked='<%# Bind("IsAutoShip") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoReceive" runat="server" Text="${MasterData.Flow.AutoReceive}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoReceive" runat="server" Checked='<%# Bind("IsAutoReceive") %>'>
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintASN" runat="server" Text="${MasterData.Flow.PrintASN}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintASN" runat="server" Checked='<%# Bind("NeedPrintASN") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintReceipt" runat="server" Text="${MasterData.Flow.PrintReceipt}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintReceipt" runat="server" Checked='<%# Bind("NeedPrintReceipt") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintOrder" runat="server" Text="${MasterData.Flow.PrintOrder}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintOrder" runat="server" Checked='<%# Bind("NeedPrintOrder") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblAllowExceed" runat="server" Text="${MasterData.Flow.AllowExceed}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbAllowExceed" runat="server" Checked='<%# Bind("AllowExceed") %>'>
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lbAllowCreateDetail" runat="server" Text="${MasterData.Flow.AllowCreateDetail}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbAllowCreateDetail" runat="server" Checked='<%# Bind("AllowCreateDetail") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsListDetail" runat="server" Text="${MasterData.Flow.IsListDetail}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsListDetail" runat="server" Checked='<%# Bind("IsListDetail") %>' />
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsShipScanHu" runat="server" Text="${MasterData.Flow.IsShipScanHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsShipScanHu" runat="server" Checked='<%# Bind("IsShipScanHu") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsReceiptScanHu" runat="server" Text="${MasterData.Flow.IsReceiptScanHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsReceiptScanHu" runat="server" Checked='<%# Bind("IsReceiptScanHu") %>'>
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lbFulfillUC" runat="server" Text="${MasterData.Flow.FulfillUC}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbFulfillUC" runat="server" Checked='<%# Bind("FulfillUnitCount") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblCreateHuOption" runat="server" Text="${MasterData.Flow.CreateHuOption}:" />
                        </td>
                        <td class="ttd02">
                            <cc1:CodeMstrDropDownList ID="ddlCreateHuOption" Code="CreateHuOption" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblAutoPrintHu" runat="server" Text="${MasterData.Flow.AutoPrintHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbAutoPrintHu" runat="server" Checked='<%# Bind("AutoPrintHu") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsGoodsReceiveFIFO" runat="server" Text="${MasterData.Flow.IsGoodsReceiveFIFO}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsGoodsReceiveFIFO" runat="server" Checked='<%# Bind("IsGoodsReceiveFIFO") %>'>
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAsnUniqueReceipt" runat="server" Text="${MasterData.Flow.IsAsnUniqueReceipt}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAsnUniqueReceipt" runat="server" Checked='<%# Bind("IsAsnUniqueReceipt") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsShowPrice" runat="server" Text="${MasterData.Flow.IsShowPrice}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsShowPrice" runat="server" Checked='<%# Bind("IsShowPrice") %>'>
                            </asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblAntiResolveHu" runat="server" Text="${MasterData.Flow.AntiResolveHu}:" />
                        </td>
                        <td class="ttd02">
                            <cc1:CodeMstrDropDownList ID="ddlAntiResolveHu" Code="AntiResolveHu" runat="server">
                            </cc1:CodeMstrDropDownList>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="tablefooter">
                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                    CssClass="button2" ValidationGroup="vgSave" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </div>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Flow" runat="server" TypeName="com.Sconit.Web.FlowMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Flow" InsertMethod="CreateFlow"
    OnInserted="ODS_Flow_Inserted" OnInserting="ODS_Flow_Inserting" SelectMethod="LoadFlow">
    <SelectParameters>
        <asp:Parameter Type="String" Name="code" />
    </SelectParameters>
</asp:ObjectDataSource>
