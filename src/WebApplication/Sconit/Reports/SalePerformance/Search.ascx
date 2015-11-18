<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Reports_SalePerformance_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<script language="javascript" type="text/javascript">
function checkCheckbox(source, args)
{

    if( $('#<%= cbClassifiedParty.ClientID %>').attr("checked")==false 
        && $('#<%= cbClassifiedBillFrom.ClientID %>').attr("checked")==false 
        && $('#<%= cbClassifiedCreateUser.ClientID %>').attr("checked")==false 
        && $('#<%= cbClassifiedCustomer.ClientID %>').attr("checked")==false 
        && $('#<%= cbClassifiedItemCategory.ClientID %>').attr("checked")==false 
        && $('#<%= cbClassifiedItem.ClientID %>').attr("checked")==false  )
   {
        args.IsValid = false; 
   }
          
}
</script>

<fieldset>
    <table class="mtable">
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom.Supplier}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" Width="250" DescField="Name"
                        ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetFromParty" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo.Customer}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbPartyTo" runat="server" Visible="true" Width="250" DescField="Name"
                        ValueField="Code" MustMatch="true" ServicePath="PartyMgr.service" ServiceMethod="GetToParty" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="ltlBillFrom" runat="server" Text="${MasterData.Order.OrderHead.BillFrom.Distribution}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbBillFrom" runat="server" Visible="true" DescField="Address" ValueField="Code"
                        ServiceParameter="string:#tbPartyFrom" Width="250" ServicePath="AddressMgr.service"
                        ServiceMethod="GetBillAddress" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCreateUser" runat="server" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbCreateUser" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="ltlItemCategory" runat="server" Text="${MasterData.Item.ItemCategory}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbItemCategory" runat="server" Visible="true" DescField="Desc1"
                        ValueField="Code" ServicePath="ItemCategoryMgr.service" ServiceMethod="GetAllItemCategory"
                        MustMatch="true" Width="200" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblItem" runat="server" Text="${Common.Business.ItemCode}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbItem" runat="server" Visible="true" DescField="DescriptionAndSpec" ImageUrlField="ImageUrl"
                        Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="ltlClassifiedParam" runat="server" Text="${Reports.SummarizeParameters}:" />
                </td>
                <td class="td02">
                    <asp:CheckBox ID="cbClassifiedParty" runat="server" Text="${Common.Business.Region}" />
                    <asp:CheckBox ID="cbClassifiedBillFrom" runat="server" Text="${Reports.SalePerformance.BillFrom}" />
                    <asp:CheckBox ID="cbClassifiedCreateUser" runat="server" />
                    <asp:CheckBox ID="cbClassifiedCustomer" runat="server" />
                    <asp:CheckBox ID="cbClassifiedItemCategory" runat="server" Text="${MasterData.Item.ItemCategory}" />
                    <asp:CheckBox ID="cbClassifiedItem" runat="server" Text="${Common.Business.Item}" />
                    <br>
                    <asp:CustomValidator ID="cvChedked" runat="server" ErrorMessage="${Reports.SalePerformance.CollectPartsMustBeSelectedACheckbox}"
                        ValidationGroup="vgSearch" ClientValidationFunction="checkCheckbox" Display="Dynamic" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lbCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCurrency" runat="server" Visible="true" DescField="Name" ValueField="Code"
                        ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
                </td>
            </tr>
            <tr>
                <td colspan="3" />
                
                <td class="t02">
                    <div class="buttons">
                        <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="query"
                            OnClick="btnSearch_Click" ValidationGroup="vgSearch" />
                        <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                            OnClick="btnExport_Click" ValidationGroup="vgSearch" />
                    </div>
                </td>
            </tr>
        </table>
</fieldset>
