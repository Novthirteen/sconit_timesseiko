<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Finance_Payment_New" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>

<script language="javascript" type="text/javascript" src="Js/DatePicker/WdatePicker.js"></script>

<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Payment" runat="server" DataSourceID="ODS_Payment" DefaultMode="Insert"
        DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Payment.AddPayment}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlPartyCode" runat="server" Text="${MasterData.Payment.Supplier}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbPartyCode" CssClass="inputRequired" runat="server" DescField="Name"
                                ValueField="Code" ServicePath="SupplierMgr.service" ServiceMethod="GetAllSupplier"
                                Width="250" />
                            <asp:RequiredFieldValidator ID="rfvParty" runat="server" ErrorMessage="${MasterData.Payment.Party.Empty}"
                                Display="Dynamic" ControlToValidate="tbPartyCode" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlPaymentDate" runat="server" Text="${MasterData.Payment.PaymentDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbPaymentDate" CssClass="inputRequired" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                            <asp:RequiredFieldValidator ID="rfvPaymentDate" runat="server" ErrorMessage="${MasterData.Payment.PaymentDate.Empty}"
                                Display="Dynamic" ControlToValidate="tbPaymentDate" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lbCurrency" runat="server" Text="${MasterData.Flow.Currency}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbCurrency" CssClass="inputRequired" runat="server" Visible="true"
                                DescField="Name" ValueField="Code" ServicePath="CurrencyMgr.service" ServiceMethod="GetAllCurrency" />
                            <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ErrorMessage="${MasterData.Payment.Currency.Empty}"
                                Display="Dynamic" ControlToValidate="tbCurrency" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblAmount" runat="server" Text="${MasterData.Payment.Amount}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbAmount" CssClass="inputRequired" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="${MasterData.Payment.Amount.Empty}"
                                Display="Dynamic" ControlToValidate="tbAmount" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="tbAmount"
                                ErrorMessage="${MasterData.Payment.Amount.MustNumber}" Display="Dynamic" ValidationGroup="vgSave"
                                ValidationExpression="^[+]?(0\.\d+)|([1-9][0-9]*(.[0-9]{2})?)$" />
                        </td>
                    </tr>
                    <%--
                        <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlExtPaymentNo" runat="server" Text="${MasterData.Payment.ExtPaymentNo}:" Visible="false"/>
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbExtPaymentNo" runat="server" Visible="false" Text='<%# Bind("ExtPaymentNo") %>' 
                                Width="250" />
                        </td>
                        <td class="td01">
                            <!--<asp:Literal ID="lblInvoiceNo" runat="server" Text="${MasterData.Payment.InvoiceNo}:" />-->
                        </td>
                        <td class="td02">
                             <!--<asp:TextBox ID="tbInvoiceNo" runat="server" Text='<%# Bind("InvoiceNo") %>' Width="250"></asp:TextBox>-->
                        </td>
                    </tr>
                     --%>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPayType" runat="server" Text="${MasterData.Payment.PayType}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrDropDownList ID="ddlPayType" Code="Paytype" runat="server">
                            </sc1:CodeMstrDropDownList>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlVoucherNo" runat="server" Text="${MasterData.Payment.VoucherNo}:"   />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbVoucherNo" runat="server" Text='<%# Bind("VoucherNo") %>' CssClass="inputRequired"/>
                             <asp:RequiredFieldValidator ID="rfvVoucherNo" runat="server" ErrorMessage="${MasterData.Payment.VoucherNo.Empty}"
                                Display="Dynamic" ControlToValidate="tbVoucherNo" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbContinuousCreate" runat="server" Text="${MasterData.Order.OrderHead.ContinuousCreate}" />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
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
                            <div class="buttons">
                                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Payment" runat="server" TypeName="com.Sconit.Web.PaymentMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Payment" InsertMethod="CreatePayment">
</asp:ObjectDataSource>
