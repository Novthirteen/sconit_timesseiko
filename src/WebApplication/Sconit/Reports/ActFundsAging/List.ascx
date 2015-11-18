<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_ActFundsAging_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="BillAddress.Party.Code"
            OnRowDataBound="GV_List_RowDataBound" DefaultSortDirection="Ascending">
            <Columns>
                <asp:TemplateField SortExpression="BillAddress.Party.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("BillAddress.Party.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Currency.Code" HeaderText="${Reports.ActFundsAging.Currency}">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount1}" SortExpression="NoBackwashAmount1">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount1", "{0:###,##0.00}")%>' OnClick="lbtnDetail1_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount2}" SortExpression="NoBackwashAmount2">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount2" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount2", "{0:###,##0.00}")%>' OnClick="lbtnDetail2_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount3}" SortExpression="NoBackwashAmount3">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount3" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount3", "{0:###,##0.00}")%>' OnClick="lbtnDetail3_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount4}" SortExpression="NoBackwashAmount4">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount4" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount4", "{0:###,##0.00}")%>' OnClick="lbtnDetail4_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount5}" SortExpression="NoBackwashAmount5">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount5" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount5", "{0:###,##0.00}")%>' OnClick="lbtnDetail5_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount6}" SortExpression="NoBackwashAmount6">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount6" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount6", "{0:###,##0.00}")%>' OnClick="lbtnDetail6_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount7}" SortExpression="NoBackwashAmount7">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount7" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount7", "{0:###,##0.00}")%>' OnClick="lbtnDetail7_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount8}" SortExpression="NoBackwashAmount8">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount8" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount8", "{0:###,##0.00}")%>' OnClick="lbtnDetail8_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActFundsAging.NoBackwashAmount9}" SortExpression="NoBackwashAmount9">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnNoBackwashAmount9" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("NoBackwashAmount9", "{0:###,##0.00}")%>' OnClick="lbtnDetail9_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
