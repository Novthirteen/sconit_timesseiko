<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Reports_SalesOrderTracking_Detail" %>
<%@ Register Src="OrderTracerList.ascx" TagName="OrderTracerList" TagPrefix="uc2" %>
<%@ Register Src="IpDetList.ascx" TagName="IpDetList" TagPrefix="uc2" %>
<%@ Register Src="~\Reports\OrderTracking\BillList.ascx" TagName="BillList" TagPrefix="uc2" %>
<%@ Register Src="OrderDetailList.ascx" TagName="OrderDetailList" TagPrefix="uc2" %>
<div id="floatdiv" class="GridView">
    <fieldset>
        <uc2:BillList ID="ucBillList" runat="server"  />
        <uc2:OrderTracerList ID="ucOrderTracerList" runat="server"  />
        <uc2:IpDetList ID="ucIpDetList" runat="server"  />
        <uc2:OrderDetailList ID="ucOrderDetailList" runat="server"  />
        <div class="tablefooter">
            <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" />
        </div>
    </fieldset>
</div>
