<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Finance_Payment_Edit" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="DetailList.ascx" TagName="Detail" TagPrefix="uc2" %>
<fieldset>
    <legend id="lgd" runat="server">${MasterData.Payment.POPayment}</legend>
    <asp:FormView ID="FV_Payment" runat="server" DataSourceID="ODS_Payment" DefaultMode="Edit"
        DataKeyNames="PaymentNo" OnDataBound="FV_Payment_DataBound">
        <EditItemTemplate>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPaymentNo" runat="server" Text="${MasterData.Payment.PaymentNo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPaymentNo" runat="server" CodeField="PaymentNo" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblExtPaymentNo" runat="server" Text="${MasterData.Payment.ExtPaymentNo}:"
                            Visible="false" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbExtPaymentNo" runat="server" Text='<%# Bind("ExtPaymentNo") %>'
                            Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblParty" runat="server" Text="${MasterData.Payment.Supplier}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbParty" runat="server" CodeField="Party.Code" DescField="Party.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCurrency" runat="server" CodeField="Currency.Code" DescField="Currency.Name" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPaymentDate" runat="server" Text="${MasterData.Payment.PaymentDate}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPaymentDate" runat="server" CodeField="PaymentDate" CodeFieldFormat="{0:yyyy-MM-dd}" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Payment.Status}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="tbStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblAmount" runat="server" Text="${MasterData.Payment.Amount}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbAmount" runat="server" CodeField="Amount" CodeFieldFormat="{0:0.00}" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblBackwashAmount" runat="server" Text="${MasterData.Payment.BackwashAmount}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbBackwashAmount" runat="server" CodeField="BackwashAmount"
                            CodeFieldFormat="{0:0.00}" />
                    </td>
                </tr>
                
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPayType" runat="server" Text="${MasterData.Payment.PayType}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="tbPayType" runat="server" Code="PayType" Value='<%# Bind("PayType") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlVoucherNo" runat="server" Text="${MasterData.Payment.VoucherNo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbVoucherNo" runat="server" CodeField="VoucherNo" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblCreateDate" runat="server" Text="${MasterData.Payment.CreateDate}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCreateDate" runat="server" CodeField="CreateDate" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Payment.CreateUser}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCreateUser" runat="server" CodeField="CreateUser.Code"
                            DescField="CreateUser.Name" />
                    </td>
                </tr>
            </table>
        </EditItemTemplate>
    </asp:FormView>
    <div class="tablefooter">
        <sc1:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" Width="59px"
            OnClick="btnSave_Click" FunctionId="EditPayment" ValidationGroup="vgSave" />
        <sc1:Button ID="btnDelete" runat="server" Text="${Common.Button.Delete}" Width="59px"
            OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" OnClick="btnDelete_Click"
            FunctionId="EditPayment" />
        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" Width="59px"
            OnClick="btnBack_Click" />
    </div>
</fieldset>
<asp:ObjectDataSource ID="ODS_Payment" runat="server" TypeName="com.Sconit.Web.PaymentMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Payment" SelectMethod="LoadPayment">
    <SelectParameters>
        <asp:Parameter Name="paymentNo" Type="String" />
        <asp:Parameter Name="includeBillPayment" Type="Boolean" DefaultValue="True" />
    </SelectParameters>
</asp:ObjectDataSource>
<uc2:Detail ID="ucDetail" runat="server" />
