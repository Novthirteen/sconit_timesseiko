<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_BillAging_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
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
                <asp:TemplateField HeaderText="${Reports.BillAging.ItemCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.ItemDesc1}" SortExpression="Item.Desc1">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.ItemSpec}" SortExpression="Item.Spec">
                    <ItemTemplate>
                        <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.ItemBrand}" SortExpression="Item.Brand">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Uom}" SortExpression="Uom.Name">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount1}" SortExpression="Amount1">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount1", "{0:###,##0.00}")%>' OnClick="lbtnDetail1_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount2}" SortExpression="Amount2">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount2" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount2", "{0:###,##0.00}")%>' OnClick="lbtnDetail2_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount3}" SortExpression="Amount3">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount3" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount3", "{0:###,##0.00}")%>' OnClick="lbtnDetail3_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount4}" SortExpression="Amount4">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount4" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount4", "{0:###,##0.00}")%>' OnClick="lbtnDetail4_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount5}" SortExpression="Amount5">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount5" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount5", "{0:###,##0.00}")%>' OnClick="lbtnDetail5_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount6}" SortExpression="Amount6">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount6" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount6", "{0:###,##0.00}")%>' OnClick="lbtnDetail6_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount7}" SortExpression="Amount7">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount7" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount7", "{0:###,##0.00}")%>' OnClick="lbtnDetail7_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount8}" SortExpression="Amount8">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount8" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount8", "{0:###,##0.00}")%>' OnClick="lbtnDetail8_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.BillAging.Amount9}" SortExpression="Amount9">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAmount9" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("Amount9", "{0:###,##0.00}")%>' OnClick="lbtnDetail9_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
