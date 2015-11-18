﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Order_OrderHead_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Assembly="ASTreeView" Namespace="Geekees.Common.Controls" TagPrefix="ct" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblOrderNo" runat="server" Text="${MasterData.Order.OrderHead.OrderNo.Distribution}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderNo" runat="server" Visible="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPriority" runat="server" Text="${MasterData.Order.OrderHead.Priority}:" />
            </td>
            <td class="td02">
                <cc1:CodeMstrDropDownList ID="ddlPriority" Code="OrderPriority" runat="server" IncludeBlankOption="true"
                    DefaultSelectedValue="" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Distribution}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" MustMatch="true" Width="250" ServiceMethod="GetFlowList" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCreateUser" runat="server" Text="${Reports.SalePerformance.Salesman}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCreateUser" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom.Region}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyFrom" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="PartyMgr.service" ServiceMethod="GetOrderFromParty" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo.Customer}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbPartyTo" runat="server" Visible="true" Width="250" DescField="Name"
                    ValueField="Code" MustMatch="true" ServicePath="PartyMgr.service" ServiceMethod="GetOrderToParty" />
            </td>
        </tr>
        <tr runat="server" visible="false" id="trLoc">
            <td class="td01">
                <asp:Literal ID="lblLocFrom" runat="server" Text="${MasterData.Order.OrderHead.LocFrom}:"
                    Visible="false" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocFrom" runat="server" Visible="false" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="LocationMgr.service" ServiceMethod="GetLocation"
                    ServiceParameter="string:#tbPartyFrom" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblLocTo" runat="server" Text="${MasterData.Order.OrderHead.LocTo}:"
                    Visible="false" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocTo" runat="server" Visible="false" Width="250" DescField="Name"
                    ValueField="Code" ServicePath="LocationMgr.service" ServiceMethod="GetLocation"
                    ServiceParameter="string:#tbPartyTo" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Order.OrderHead.Status}:" />
            </td>
            <td class="td02">
                <ct:ASDropDownTreeView ID="astvMyTree" runat="server" BasePath="~/Js/astreeview/"
                    DataTableRootNodeValue="0" EnableRoot="false" EnableNodeSelection="false" EnableCheckbox="true"
                    EnableDragDrop="false" EnableTreeLines="false" EnableNodeIcon="false" EnableCustomizedNodeIcon="false"
                    EnableDebugMode="false" EnableRequiredValidator="false" InitialDropdownText=""
                    Width="170" EnableCloseOnOutsideClick="true" EnableHalfCheckedAsChecked="true"
                    DropdownIconDown="~/Js/astreeview/images/windropdown.gif" EnableContextMenuAdd="false"
                    MaxDropdownHeight="300" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSubType" runat="server" Text="${MasterData.Order.OrderHead.SubType}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="Description" DataValueField="Value">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.PlannedBill.CreateDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${MasterData.PlannedBill.CreateDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
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
                <asp:Literal ID="ltlApprovalStatus" runat="server" Text="${MasterData.Order.OrderHead.ApprovalStatus}:" />
            </td>
            <td class="td02">
                <cc1:CodeMstrDropDownList ID="ddlApprovalStatus" runat="server" Code="ApprovalStatus"
                    IncludeBlankOption="true" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
               
                <asp:Literal ID="ltlListFormat" runat="server" Text="${Common.ListFormat}:" />
            </td>
            <td>

                <asp:RadioButtonList ID="rblListFormat" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="${Common.ListFormat.Group}" Value="Group" Selected="True" />
                    <asp:ListItem Text="${Common.ListFormat.Detail}" Value="Detail" />
                </asp:RadioButtonList>
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <cc1:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                    CssClass="button2" FunctionId="EditOrder" />
                <cc1:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                    CssClass="button2" FunctionId="EditOrder" Visible="false"/>
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnSearch_Click" Visible="false" />
            </td>
        </tr>
    </table>
</fieldset>