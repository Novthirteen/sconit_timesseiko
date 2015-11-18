<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Reports_BillAging_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblParty" runat="server" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbParty" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code"  />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${Reports.ActBill.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="DescriptionAndSpec" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>           
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lbCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCurrency" CssClass="inputRequired" runat="server" Visible="true"
                    DescField="Name" ValueField="Code" ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
                <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ErrorMessage="${Reports.ActBill.Currency.Empty}"
                    Display="Dynamic" ControlToValidate="tbCurrency" ValidationGroup="vgSearch" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="t02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="button2"
                    OnClick="btnSearch_Click" ValidationGroup="vgSearch" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnExport_Click" ValidationGroup="vgSearch" />
            </td>
        </tr>
    </table>
</fieldset>