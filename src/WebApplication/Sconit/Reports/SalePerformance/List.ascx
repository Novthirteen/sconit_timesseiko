<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_SalePerformance_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            ShowHeader="true" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-Width="30" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Literal ID="ltlSeq" runat="server" Text='<%# (Container as GridViewRow).RowIndex+1 %> ' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Region}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyFrom" runat="server" Text='<%# Bind("PartyFrom.Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.BillFrom}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBillFrom" runat="server" Text='<%# Bind("BillFrom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.BillTo}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBillTo" runat="server" Text='<%# Bind("BillTo.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.Salesman}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateUser" runat="server" Text='<%# Bind("CreateUser.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Customer}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyTo" runat="server" Text='<%# Bind("PartyTo.Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.ItemCategory}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCategory" runat="server" Text='<%# Bind("ItemCategory.Desc1") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Code}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.ItemDesc1}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.ItemSpec}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.ItemBrand}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnOrderedQty" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("OrderedQty", "{0:#.########}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.Currency}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalePerformance.Amount}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnIncludeTaxTotalPrice" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text='<%# Eval("IncludeTaxTotalPrice", "{0:###,##0.00}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
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
                    <asp:Literal ID="lblTotalAmount" runat="server" Text="${Reports.SalePerformance.TotalAmount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalAmount" runat="server" Visible="true" onfocus="this.blur();"
                        Width="150px" />
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
