<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_ActBill_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-Width="30">
                    <ItemTemplate>
                        <asp:Literal ID="ltlSeq" runat="server" Text='<%# (Container as GridViewRow).RowIndex+1 %> ' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="BillAddress.Party.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblParty" runat="server" Text='<%# Eval("BillAddress.Party.Code")%>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.OrderNo}" SortExpression="OrderNo">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ReceiptNo}" SortExpression="ReceiptNo">
                    <ItemTemplate>
                        <asp:Label ID="lblReceiptNo" runat="server" Text='<%# Eval("ReceiptNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ExternalReceiptNo}" SortExpression="ExternalReceiptNo">
                    <ItemTemplate>
                        <asp:Label ID="lblExternalReceiptNo" runat="server" Text='<%# Eval("ExternalReceiptNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemDesc1}" SortExpression="Item.Desc1">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemSpec}" SortExpression="Item.Spec">
                    <ItemTemplate>
                        <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemBrand}" SortExpression="Item.Brand">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.Uom}" SortExpression="Uom.Name">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.Qty}" SortExpression="Qty">
                    <ItemTemplate>
                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty", "{0:0.###}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="Currency.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.Amount}" SortExpression="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount", "{0:###,##0.00}")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
    </div>
    <table class="mtable" id="tabTotalAmount" runat="server">
        <tr>
            <td class="td02">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblTotalPrice" runat="server" Text="${Reports.ActBill.TotalPrice}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbTotalPrice" runat="server" onfocus="this.blur();" Width="150px" />
            </td>
        </tr>
    </table>
</fieldset>
