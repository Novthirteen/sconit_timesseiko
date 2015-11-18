<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Reports_OrderTracking_Detail" %>
<%@ Register Src="BillList.ascx" TagName="BillList" TagPrefix="uc2" %>
<%@ Register Src="ReceiptDetailList.ascx" TagName="ReceiptDetailList" TagPrefix="uc2" %>

<div id="floatdiv" class="GridView">
    <fieldset>
        <uc2:BillList ID="ucBillList" runat="server"  />
        <uc2:ReceiptDetailList ID="ucReceiptDetailList" runat="server" />
        <div class="tablefooter">
            <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" />
        </div>
    </fieldset>
</div>
