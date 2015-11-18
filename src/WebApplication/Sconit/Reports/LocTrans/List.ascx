<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_LocTrans_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="~/Visualization/GoodsTraceability/Traceability/View.ascx" TagName="View"
    TagPrefix="uc" %>
<fieldset>
    <div class="GridView">
        <sc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" seqtext="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.EffDate}" SortExpression="EffDate" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEffDate" runat="server" Text='<%# Eval("EffDate","{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Visualization.GoodsTraceability.TransType}" SortExpression="TransType" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblTransType" runat="server" Text='<%# Eval("TransType")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.OrderNo}" SortExpression="OrderNo" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="${Common.Business.IpNo}" SortExpression="IpNo" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblIpNo" runat="server" Text='<%# Eval("IpNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="${Common.Business.ReceiptNo}" SortExpression="RecNo" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecNo" runat="server" Text='<%# Eval("RecNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Supplier}" SortExpression="PartyFromName" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("PartyFromName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Customer}" SortExpression="PartyToName" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("PartyToName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Location}" SortExpression="Loc" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Loc")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Location.Name}" SortExpression="LocName" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemCode}" SortExpression="Item" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="${Common.Business.Item.Category}" SortExpression="Item.Description" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("ItemDescription")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemSpec}"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemSpec" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemBrand}"  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemBrand" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Uom" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Qty}" SortExpression="Qty" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty","{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.CreateUser}" SortExpression="CreateUser" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateUser" runat="server" Text='<%# Eval("CreateUser.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </sc1:GridView>
        <sc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </sc1:GridPager>
    </div>
</fieldset>
