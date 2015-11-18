<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_OrderTracking_List" %>
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
                <asp:TemplateField HeaderText="${Reports.OrderTracking.PartyFrom}" SortExpression="OrderHead.PartyFrom.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyFrom" runat="server" Text='<%# Eval("OrderHead.PartyFrom.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.PartyTo}" SortExpression="OrderHead.PartyTo.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyTo" runat="server" Text='<%# Eval("OrderHead.PartyTo.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.BillTo}" SortExpression="OrderHead.BillTo.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBillTo" runat="server" Text='<%# Eval("OrderHead.BillTo.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.OrderNo}" SortExpression="OrderHead.OrderNo"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderHead.OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.ItemCode}" SortExpression="Item.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.ItemDesc1}" SortExpression="Item.Desc1"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDesc1" runat="server" Text='<%# Eval("Item.Desc1")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.ItemSpec}" SortExpression="Item.Spec"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSpec" runat="server" Text='<%# Eval("Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.ItemBrand}" SortExpression="Item.Brand"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("Item.Brand")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.CreateDate}" SortExpression="OrderHead.CreateDate"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" Text='<%# Bind("OrderHead.CreateDate","{0:yyyy-MM-dd HH:mm}") %>'
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.CreateUser}" SortExpression="OrderHead.CreateUser.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateUser" runat="server" Text='<%# Eval("OrderHead.CreateUser.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="WindowTime" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="${Reports.OrderTracking.WindowTime}"  HeaderStyle-Wrap="false"
                    SortExpression="WindowTime" />
                <asp:BoundField DataField="MinReceiptDate" DataFormatString="{0:yyyy-MM-dd HH:mm}"
                    HeaderText="${Reports.OrderTracking.MinReceiptDate}" SortExpression="MinReceiptDate"  HeaderStyle-Wrap="false"/>
                <%-- <asp:BoundField DataField="MaxCreateDate" DataFormatString="{0:yyyy-MM-dd HH:mm}"
                    HeaderText="${Reports.OrderTracking.MaxCreateDate}" SortExpression="MaxCreateDate" />--%>
                <asp:TemplateField HeaderText="${MasterData.Flow.Currency}" SortExpression="OrderHead.Currency.Code"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("OrderHead.Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.OrderQty}" SortExpression="OrderQty"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval("OrderQty","{0:#.##}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.RecQty}" SortExpression="RecQty"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblRecQty" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("RecQty", "{0:#.##}")%>' OnClick="lbtnRecQty_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.NoRecQty}" SortExpression="NoRecQty"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblNoRecQty" runat="server" Text='<%# Eval("NoRecQty","{0:#.##}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- 
                <asp:TemplateField HeaderText="${Reports.OrderTracking.ExternalBillNo}" SortExpression="ExternalBillNo">
                    <ItemTemplate>
                        <asp:Label ID="lblExternalBillNo" runat="server" Text='<%# Eval("ExternalBillNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
                <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:yyyy-MM-dd HH:mm}"  HeaderStyle-Wrap="false"
                    HeaderText="${Reports.OrderTracking.EffectiveDate}" SortExpression="EffectiveDate" />
                <asp:TemplateField HeaderText="${Reports.OrderTracking.BilledAmount}" SortExpression="BilledAmount  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnBilledAmount" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")+","+DataBinder.Eval(Container.DataItem, "Item.Code") %>'
                            Text='<%# Eval("BilledAmount", "{0:###,##0.00}")%>' OnClick="lbtnBilledAmount_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.OrderTracking.NoBilledAmount}" SortExpression="NoBilledAmount"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lbtnNoBilledAmount" runat="server" Text='<%# Eval("NoBilledAmount","{0:###,##0.00}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
