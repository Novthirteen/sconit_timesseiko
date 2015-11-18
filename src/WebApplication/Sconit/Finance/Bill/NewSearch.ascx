﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewSearch.ascx.cs" Inherits="Finance_Bill_NewSearch" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="NewList.ascx" TagName="NewList" TagPrefix="uc2" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPartyCode" runat="server" Text="${MasterData.ActingBill.Supplier}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyCode" runat="server" DescField="Name" ValueField="Code" ServicePath="SupplierMgr.service"
                    ServiceMethod="GetAllSupplier" Width="250" />
                <asp:Literal ID="ltlParty" runat="server" Visible="false"/>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlReceiver" runat="server" Text="${MasterData.ActingBill.OrderNo.Procurement}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbReceiver" runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.ActingBill.EffectiveDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${MasterData.ActingBill.EffectiveDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${Common.Business.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" DescField="DescriptionAndSpec"
                    ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCurrency" runat="server" Text="${MasterData.ActingBill.Currency}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCurrency" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="IsRelease" runat="server" Font-Size="9pt" Text="${MasterData.ActingBill.IsRelease}" Visible="false" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    Width="59px" CssClass="button2" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    Width="59px" CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:NewList ID="ucNewList" runat="server" />
<asp:Button ID="btnAddDetail"  runat="server" Text="${Common.Button.AddDetail}" Width="69px" OnClick="btnAddDetail_Click" CssClass="button2" Visible="false"/>
<asp:Button ID="btnClose" runat="server" OnClick="btnBack_Click"  Width="59px" CssClass="button2" Visible="false"/>