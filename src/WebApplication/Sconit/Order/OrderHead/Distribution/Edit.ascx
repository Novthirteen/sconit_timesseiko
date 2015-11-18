<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Order_OrderHead_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="~/Order/OrderDetail/List.ascx" TagName="Detail" TagPrefix="uc2" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<fieldset>
    <legend>${MasterData.Order.OrderHead}</legend>
    <asp:FormView ID="FV_Order" runat="server" DataSourceID="ODS_Order" DefaultMode="Edit"
        DataKeyNames="OrderNo" OnDataBound="FV_Order_DataBound">
        <EditItemTemplate>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblOrderNo" runat="server" Text="${MasterData.Order.OrderHead.OrderNo.Distribution}:" />
                    </td>
                    <td class="td02">
                        <asp:Label ID="tbOrderNo" runat="server" Text='<%#Bind("OrderNo") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblRelatedOrderNo" runat="server" Text="${MasterData.Order.OrderHead.RelatedOrderNo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbRelatedOrderNo" runat="server" Text='<%# Bind("RelatedOrderNo") %>' />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="ltlSubType" runat="server" Text="${MasterData.Order.OrderHead.SubType}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbSubType" ReadOnly="true" runat="server" Text='<%# Bind("SubType") %>'></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlCurrency" runat="server" Text="${MasterData.Order.OrderHead.Currency}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbCurrency" ReadOnly="true" runat="server" Text='<%# Bind("Currency.Name") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPriority" runat="server" Text="${MasterData.Order.OrderHead.Priority}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="ddlPriority" Code="OrderPriority" runat="server" Value='<%#Bind("Priority") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblWindowTime" runat="server" Text="${MasterData.Order.OrderHead.WindowTime.Distribution}:" />
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbWinTime" runat="server" Text='<%# Bind("WindowTime","{0:yyyy-MM-dd}") %>'
                            CssClass="inputRequired" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom.Region}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPartyFrom" runat="server" CodeField="PartyFrom.Code" DescField="PartyFrom.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPartyTo" runat="server" CodeField="PartyTo.Code" DescField="PartyTo.Name" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblShipFrom" runat="server" Text="${MasterData.Order.OrderHead.ShipFrom}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbShipFrom" runat="server" CodeField="ShipFrom.Code" DescField="ShipFrom.Address" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblShipTo" runat="server" Text="${MasterData.Order.OrderHead.ShipTo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbShipTo" runat="server" CodeField="ShipTo.Code" DescField="ShipTo.Address" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblBillFrom" runat="server" Text="${MasterData.Order.OrderHead.BillFrom.Distribution}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbBillFrom" runat="server" CodeField="BillFrom.Code" DescField="BillFrom.Address" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblBillTo" runat="server" Text="${MasterData.Order.OrderHead.BillTo.Distribution}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbBillTo" runat="server" CodeField="BillTo.Code" DescField="BillTo.Address" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblSettlement" runat="server" Text="${MasterData.Order.OrderHead.Settlement}:" />
                    </td>
                    <td class="td02" colspan="3">
                        <asp:TextBox ID="tbSettlement" runat="server" Text='<%# Bind("Settlement") %>' TextMode="MultiLine"
                            Rows="8" Columns="80" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Order.OrderHead.Status}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="cmlStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblApprovalStatus" runat="server" Text="${MasterData.Order.OrderHead.ApprovalStatus}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="cmlApprovalStatus" runat="server" Code="ApprovalStatus" Value='<%# Bind("ApprovalStatus") %>' />
                    </td>
                </tr>
                <tr>
                 <td class="td01">
                        <asp:Literal ID="lblHasPrepayment" runat="server" Text="${MasterData.Order.OrderHead.HasPrepayment}:" />
                    </td>
                    <td class="td02">
                        <asp:CheckBox ID="cbHasPrepayment" runat="server" Checked='<%# Bind("BoolField1") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblHasPaymentBeforeDelivery" runat="server" Text="${MasterData.Order.OrderHead.HasPaymentBeforeDelivery}:" />
                    </td>
                    <td class="td02">
                        <asp:CheckBox ID="cbHasPaymentBeforeDelivery" runat="server" Checked='<%# Bind("BoolField2") %>' />
                    </td>
                </tr>
            </table>
            <div id="divMore" style="display: none">
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCreateDate" runat="server" Text="${MasterData.Order.OrderHead.CreateDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCreateDate" runat="server" CodeField="CreateDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Order.OrderHead.Salesman}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCreateUser" runat="server" CodeField="CreateUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCancelDate" runat="server" Text="${MasterData.Order.OrderHead.CancelDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCancelDate" runat="server" CodeField="CancelDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCancelUser" runat="server" Text="${MasterData.Order.OrderHead.CancelUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCancelUser" runat="server" CodeField="CancelUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCancelReason" runat="server" Text="${MasterData.Order.OrderHead.CancelReason}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <sc1:ReadonlyTextBox ID="tbCancelReason" runat="server" CodeField="CancelReason" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCompleteDate" runat="server" Text="${MasterData.Order.OrderHead.CompleteDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCompleteDate" runat="server" CodeField="CompleteDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCompleteUser" runat="server" Text="${MasterData.Order.OrderHead.CompleteUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCloseUser" runat="server" CodeField="CompleteUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblLastModifyDate" runat="server" Text="${MasterData.Order.OrderHead.LastModifyDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbLastModifyDate" runat="server" CodeField="LastModifyDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblLastModifyUser" runat="server" Text="${MasterData.Order.OrderHead.LastModifyUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbLastModifyUser" runat="server" CodeField="LastModifyUser.Name" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <a type="text/html" onclick="More()" href="#" visible="true" id="more">More... </a>
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <div class="tablefooter">
        <asp:CheckBox ID="cbShowDiscount" runat="server" Text="${MasterData.Order.OrderHead.ShowDiscount}"
            AutoPostBack="true" OnCheckedChanged="cbShowDiscount_CheckChanged" />
        <sc1:Button ID="btnEdit" runat="server" Text="${Common.Button.Save}" CssClass="button2"
            OnClick="btnEdit_Click" FunctionId="EditOrder" />
        <sc1:Button ID="btnRecalculate" runat="server" Text="${Common.Button.Recalculate}" CssClass="button2"
            OnClick="btnRecalculate_Click" FunctionId="RecalculateOrder" />
        <sc1:Button ID="btnApprove" runat="server" Text="${MasterData.Order.Button.Approve}"
            CssClass="button2" OnClick="btnApprove_Click" FunctionId="ApproveOrder" />
        <sc1:Button ID="btnReject" runat="server" Text="${MasterData.Order.Button.Reject}"
            CssClass="button2" OnClick="btnReject_Click" FunctionId="RejectOrder" />
        <sc1:Button ID="btnSubmit" runat="server" Text="${MasterData.Order.Button.Submit}"
            CssClass="button2" OnClick="btnSubmit_Click" FunctionId="SubmitOrder" Visible="false" />
        <sc1:Button ID="btnStart" runat="server" Text="${MasterData.Order.Button.Start}"
            CssClass="button2" OnClick="btnStart_Click" FunctionId="StartOrder" />
        <sc1:Button ID="btnShip" runat="server" Text="${MasterData.Order.Button.Ship}" CssClass="button2"
            OnClick="btnShip_Click" FunctionId="ShipOrder" />
        <sc1:Button ID="btnComplete" runat="server" Text="${MasterData.Order.Button.Complete}"
            CssClass="button2" OnClick="btnComplete_Click" FunctionId="CompleteOrder" />
        <sc1:Button ID="BtnVoid" runat="server" Text="${MasterData.Order.Button.Void}" CssClass="button2"
            OnClick="btnVoid_Click" FunctionId="VoidOrder" OnClientClick="return confirm('${MasterData.Order.Confirm.Void}')" />
        <sc1:Button ID="btnDelete" runat="server" Text="${Common.Button.Delete}" CssClass="button2"
            OnClick="btnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
            FunctionId="DeleteOrder" />
        <sc1:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" CssClass="button2"
            OnClick="btnPrint_Click" FunctionId="PrintOrder" />
        <sc1:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
            OnClick="btnExport_Click" FunctionId="ExportOrder" />
        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
            CssClass="button2" />
    </div>
</fieldset>
<asp:ObjectDataSource ID="ODS_Order" runat="server" TypeName="com.Sconit.Web.OrderMgrProxy"
    DataObjectTypeName="com.Sconit.Web.CustomizedOrderHead" SelectMethod="LoadOrderHead">
    <SelectParameters>
        <asp:Parameter Name="orderNo" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<uc2:Detail ID="ucDetail" runat="server" />
