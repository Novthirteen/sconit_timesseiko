<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewMain.ascx.cs" Inherits="Warehouse_InProcessLocation_ViewMain" %>
<%@ Register Src="~/Warehouse/InProcessLocationDetail/List.ascx" TagName="DetailList"
    TagPrefix="uc2" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<div id="floatdiv">
    <fieldset>
        <legend>${MasterData.Order.OrderHead}</legend>
        <uc2:Edit ID="ucEdit" runat="server" Visible="true" />
        <div class="tablefooter">
            <asp:Button ID="btnUpdate" runat="server" Text="${Common.Button.Save}" Visible="true"
                CssClass="button2" OnClick="btnUpdate_Click" />
            <cc1:Button ID="btnApprove" runat="server" Text="${MasterData.Order.Button.Approve}"
                CssClass="button2" OnClick="btnApprove_Click" FunctionId="ApproveOrder" />
            <cc1:Button ID="btnReject" runat="server" Text="${MasterData.Order.Button.Reject}"
                CssClass="button2" OnClick="btnReject_Click" FunctionId="RejectOrder" />
            <cc1:Button ID="btnAdjustLocFrom" runat="server" Text="${Common.Button.AdjustLocFrom}"
                CssClass="button2" OnClick="btnAdjustLocFrom_Click" FunctionId="AdjustAsn" OnClientClick="return confirm('${Common.ASN.Confirm.AdjustLocFrom}')" />
            <cc1:Button ID="btnAdjustLocTo" runat="server" Text="${Common.Button.AdjustLocTo}"
                CssClass="button2" OnClick="btnAdjustLocTo_Click" FunctionId="AdjustAsn" OnClientClick="return confirm('${Common.ASN.Confirm.AdjustLocTo}')" />
            <cc1:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" ValidationGroup="vgClose" FunctionId="AdjustAsn" />            
            <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" OnClick="btnPrint_Click"
                CssClass="button2" />
            <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnExport_Click"
                CssClass="button2" />
        </div>
    </fieldset>
    <uc2:DetailList ID="ucDetailList" runat="server" Visible="true" />
</div>
