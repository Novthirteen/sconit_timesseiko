<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderDetailList.ascx.cs"
    Inherits="Reports_SalesOrderTracking_OrderDetailList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<legend>${MasterData.Order.OrderDetail}</legend>
<div class="GridView">
    <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
        AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
        ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
        CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
        SelectCountMethod="FindCount" DefaultSortExpression="OrderHead.OrderNo" DefaultSortDirection="Ascending"
        OnRowDataBound="GV_List_RowDataBound">
        <Columns>
            <asp:TemplateField SortExpression="OrderHead.OrderNo" HeaderText="${MasterData.Order.OrderHead.OrderNo.Distribution}">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Seq}" SortExpression="Sequence">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Sequence")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.ItemCode}" SortExpression="Item.Code">
                <ItemTemplate>
                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.SalePerformance.ItemDesc1}" SortExpression="Item.Desc1">
                <ItemTemplate>
                    <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.ItemSpec}" SortExpression="Item.Spec">
                <ItemTemplate>
                    <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.ItemBrand}" SortExpression="Item.Brand">
                <ItemTemplate>
                    <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Uom.Name">
                <ItemTemplate>
                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom.Name")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="OrderHead.Currency.Code">
                <ItemTemplate>
                    <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("OrderHead.Currency.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.UnitPrice}">
                <ItemTemplate>
                    <asp:Label ID="lblIncludeTaxPrice" runat="server" Text='<%# Eval("IncludeTaxPrice", "{0:###,##0.00}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}" SortExpression="OrderedQty">
                <ItemTemplate>
                    <asp:Label ID="lblOrderedQty" runat="server" Text='<%# Eval("OrderedQty", "{0:0.###}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Amount}" SortExpression="IncludeTaxTotalPrice">
                <ItemTemplate>
                    <asp:Label ID="lblIncludeTaxTotalPrice" runat="server" Text='<%# Eval("IncludeTaxTotalPrice", "{0:###,##0.00}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
    </cc1:GridPager>
</div>
