<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_SalesOrderTracking_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="OrderHead.OrderNo" DefaultSortDirection="Ascending"
            OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.PartyTo}" SortExpression="OrderHead.PartyTo.Code"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyTo" runat="server" Text='<%# Eval("OrderHead.PartyTo.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.BillFrom}" SortExpression="OrderHead.BillFrom.Code"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBillFrom" runat="server" Text='<%# Eval("OrderHead.BillFrom.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.OrderNo}" SortExpression="OrderHead.OrderNo"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderHead.OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.Sequence}" SortExpression="Sequence"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSequence" runat="server" Text='<%# Eval("Sequence")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.ItemCode}" SortExpression="Item.Code"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.ItemDesc1}" SortExpression="Item.Desc1"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.ItemSpec}" SortExpression="Item.Spec"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.ItemBrand}" SortExpression="Item.Brand"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.CreateDate}" SortExpression="OrderHead.CreateDate"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" Text='<%# Bind("OrderHead.CreateDate","{0:yyyy-MM-dd}") %>'
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.CreateUser}" SortExpression="OrderHead.CreateUser.Code"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateUser" runat="server" Text='<%# Eval("OrderHead.CreateUser.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="WindowTime" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${Reports.SalesOrderTracking.WindowTime}"
                    SortExpression="WindowTime" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="MinDeliverDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${Reports.SalesOrderTracking.MinDeliverDate}"
                    SortExpression="MinDeliverDate" HeaderStyle-Wrap="false" />
                <%-- <asp:BoundField DataField="MaxDeliverDate" DataFormatString="{0:yyyy-MM-dd}"
                    HeaderText="${Reports.SalesOrderTracking.MaxDeliverDate}" SortExpression="MaxDeliverDate" />--%>
                <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="OrderHead.Currency.Code"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("OrderHead.Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.OrderQty}" SortExpression="OrderQty"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblOrderQty" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("OrderQty", "{0:#.##}")%>' OnClick="lbtnOrderQty_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.DeliverQty}" SortExpression="DeliverQty"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblDeliverQty" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("DeliverQty", "{0:#.##}")%>' OnClick="lbtnDeliverQty_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.NoDeliverQty}" SortExpression="NoDeliverQty"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblNoDeliverQty" runat="server" Text='<%# Eval("NoDeliverQty","{0:#.##}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.Qty}" SortExpression="OrderedQty"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblOrderedQty" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("OrderedQty", "{0:#.##}")%>' OnClick="lbtnOrderedQty_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${Reports.SalesOrderTracking.EffectiveDate}"
                    SortExpression="EffectiveDate" HeaderStyle-Wrap="false" />
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.BilledAmount}" SortExpression="BilledAmount"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnBilledAmount" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("BilledAmount", "{0:###,##0.00}")%>' OnClick="lbtnBilledAmount_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.NoBilledAmount}" SortExpression="NoBilledAmount"
                    HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lbtnNoBilledAmount" runat="server" Text='<%# Eval("NoBilledAmount","{0:0.00}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
