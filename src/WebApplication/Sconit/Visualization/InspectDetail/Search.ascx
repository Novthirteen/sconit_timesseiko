﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Visualization_InspectDetail_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblInspectNo" runat="server" Text="${MasterData.Inventory.InspectOrder.InspectNo}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbInspectNo" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Common.CreateUser}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCreateUser" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${MasterData.Common.CreateDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblEndDate" runat="server" Text="${MasterData.Common.CreateDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblLocation" runat="server" Text="${Common.Business.Location}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbLocation" runat="server" Visible="true" DescField="Name" Width="280"
                    ValueField="Code" ServicePath="LocationMgr.service" ServiceMethod="GetLocationByUserCode" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItemCode" runat="server" Text="${MasterData.Flow.FlowDetail.ItemCode}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" MustMatch="true"
                    DescField="DescriptionAndSpec" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStatus" runat="server" Text="${Common.CodeMaster.Status}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Description" DataValueField="Value" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblDisposition" runat="server" Text="${MasterData.Common.Disposition}:" />
            </td>
            <td class="td02">
                <cc1:codemstrdropdownlist id="ddlDisposition" runat="server" code="Disposition" includeblankoption="true" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                    OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</fieldset>
