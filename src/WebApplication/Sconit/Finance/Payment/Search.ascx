<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Finance_Payment_Search" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPaymentNo" runat="server" Text="${MasterData.Payment.PaymentNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPaymentNo" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlStatus" runat="server" Text="${MasterData.Payment.Status}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Description" DataValueField="Value" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPartyCode" runat="server" Text="${MasterData.Payment.Supplier}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyCode" runat="server" DescField="Name" ValueField="Code" ServicePath="SupplierMgr.service"
                    ServiceMethod="GetAllSupplier" Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="lbCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCurrency" runat="server" Visible="true" DescField="Name" ValueField="Code"
                    ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
            </td>
        </tr>
        <%-- 
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPaymentDate" runat="server" Text="${MasterData.Payment.PaymentDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPaymentDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlExtPaymentNo" runat="server" Text="${MasterData.Payment.ExtPaymentNo}:" Visible="false" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbExtPaymentNo" runat="server" Visible="false" />
            </td>
        </tr>--%>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPayType" runat="server" Text="${MasterData.Payment.PayType}:" />
            </td>
            <td class="td02">
                <cc1:CodeMstrDropDownList ID="ddlPayType" Code="Paytype" runat="server" IncludeBlankOption="true"
                    DefaultSelectedValue="">
                </cc1:CodeMstrDropDownList>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlVoucherNo" runat="server" Text="${MasterData.Payment.VoucherNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbVoucherNo" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td>
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="add" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
