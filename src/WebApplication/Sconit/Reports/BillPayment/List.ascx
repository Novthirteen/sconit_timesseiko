<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_BillPayment_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            ShowHeader="true" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-Width="30">
                    <ItemTemplate>
                        <asp:Literal ID="ltlSeq" runat="server" Text='<%# (Container as GridViewRow).RowIndex+1 %> ' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="BillAddress.Party.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyCode" runat="server" Text='<%# Eval("BillAddress.Party.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillPayment.BillNo}" SortExpression="BillNo">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("BillNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillPayment.ExternalBillNo}" SortExpression="ExternalBillNo">
                    <ItemTemplate>
                        <asp:Label ID="lblExternalBillNo" runat="server" Text='<%# Eval("ExternalBillNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <%--
                <asp:TemplateField HeaderText="${Reports.BillPayment.SubmitUser}" SortExpression="SubmitUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SubmitUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="SubmitDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="${Reports.BillPayment.SubmitDate}"
                    SortExpression="SubmitDate" />
                    --%>
                <asp:TemplateField HeaderText="${Reports.BillPayment.EffectiveDate}" SortExpression="EffectiveDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "EffectiveDate", "{0:yyyy-MM-dd HH:mm}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="FixtureDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FixtureDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.CodeMaster.Status}" SortExpression="Status">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="Currency.Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("Currency.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillPayment.BillAmount}" SortExpression="AmountAfterDiscount">
                    <ItemTemplate>
                        <asp:Label ID="lblBillAmount" runat="server" Text='<%# Eval("AmountAfterDiscount", "{0:###,##0.00}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  SortExpression="BackwashAmount">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblBackwashAmount" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillNo") %>'
                            Text='<%# Eval("BackwashAmount", "{0:###,##0.00}")%>' OnClick="lbtnBackwashAmount_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:Label ID="lblNoBackwashAmount" runat="server" Text='<%# Eval("NoBackwashAmount", "{0:###,##0.00}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
    </div>
    <table class="mtable" id="tabTotalAmount" runat="server" visible="false">
        <tbody>
            <tr>
                <td class="td02">
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblTotalBillAmount" runat="server"  Text="${Reports.BillPayment.TotalBillAmount}:"/>
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalBillAmount" runat="server" Visible="true" onfocus="this.blur();"
                        Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="td02">
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblBackwashAmount" runat="server" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbBackwashAmount" runat="server" Visible="true" onfocus="this.blur();"
                        Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="td02">
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblNoBackwashAmount" runat="server" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbNoBackwashAmount" runat="server" Visible="true" onfocus="this.blur();"
                        Width="150px" />
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
