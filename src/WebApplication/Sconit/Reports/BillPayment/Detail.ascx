<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Reports_BillPayment_Detail" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv" class="GridView">
    <fieldset>
        <legend>${Reports.BillPayment.BillPaymentList}</legend>
        <fieldset>
            <div class="GridView">
                <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                    AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
                    ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
                    CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
                    SelectCountMethod="FindCount" DefaultSortExpression="Id" DefaultSortDirection="Ascending">
                    <Columns>
                        <asp:TemplateField HeaderText="${Reports.BillPayment.BillNo}" SortExpression="Bill.BillNo">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Bill.BillNo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="${Reports.BillPayment.PaymentNo}" SortExpression="Payment.PaymentNo">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Payment.PaymentNo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="${Reports.BillPayment.VoucherNo}" SortExpression="Payment.VoucherNo">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Payment.VoucherNo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="${Reports.BillPayment.PaymentDate}" SortExpression="Payment.PaymentDate">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "Payment.PaymentDate", "{0:yyyy-MM-dd HH:mm}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="${Reports.BillPayment.BackwashAmount}" SortExpression="BackwashAmount">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "BackwashAmount", "{0:###,##0.00}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc1:GridView>
                <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
                </cc1:GridPager>
            </div>
        </fieldset>
        <div class="tablefooter">
            <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" />
        </div>
    </fieldset>
</div>
