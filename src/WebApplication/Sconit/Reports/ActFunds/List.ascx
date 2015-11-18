<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_ActFunds_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="BillNo"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="BillAddress.Party.Code"
            DefaultSortDirection="Ascending" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField SortExpression="BillAddress.Party.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("BillAddress.Party.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFunds.BillNo}" SortExpression="BillNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnBillNo" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillNo") %>'
                            Text='<%# Eval("BillNo")%>' OnClick="lbtnBillDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFunds.ExternalBillNo}" SortExpression="ExternalBillNo">
                    <ItemTemplate>
                        <asp:Label ID="lblExternalBillNo" runat="server" Text='<%# Eval("ExternalBillNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <%--
                <asp:TemplateField HeaderText="${Reports.ActFunds.SubmitUser}" SortExpression="SubmitUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SubmitUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="SubmitDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="${Reports.ActFunds.SubmitDate}"
                    SortExpression="SubmitDate" />
                    --%>
                <asp:TemplateField HeaderText="${Reports.ActFunds.EffectiveDate}" SortExpression="EffectiveDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "EffectiveDate", "{0:yyyy-MM-dd HH:mm}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:TextBox ID="tbUser" runat="server" Width="100" onfocus="this.blur();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="Currency.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFunds.Amount}" SortExpression="AmountAfterDiscount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("AmountAfterDiscount", "{0:###,##0.00}")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- 
                <asp:TemplateField HeaderText="${Reports.ActFunds.BackwashAmount}" SortExpression="BackwashAmount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("BackwashAmount", "{0:###,##0.00}")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFunds.NoBackwashAmount}" >
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("NoBackwashAmount", "{0:###,##0.00}")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
