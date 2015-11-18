<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Reports_IntransitDetail_Detail" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv" class="GridView">
    <fieldset>
        <legend>${Reports.IntransitDetail.Detail}</legend>
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="OrderDetail.OrderHead.WindowTime"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Asn}" SortExpression="InProcessLocation.IpNo" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblIpNo" runat="server" Text='<%# Eval("InProcessLocation.IpNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Region}" SortExpression="InProcessLocation.PartyFrom.Name" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("InProcessLocation.PartyFrom.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Customer}" SortExpression="InProcessLocation.PartyTo.Code" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("InProcessLocation.PartyTo.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ItemCode}" SortExpression="OrderDetail.Item.Code" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("OrderDetail.Item.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="${Common.Business.Item.Category}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCategory" runat="server" Text='<%# Eval("OrderDetail.Item.Desc1") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Item.Spec}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemSpec" runat="server" Text='<%# Eval("OrderDetail.Item.Spec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Item.Brand}" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblItemBrand" runat="server" Text='<%# Eval("OrderDetail.Item.Brand") %>' />
                    </ItemTemplate>
                </asp:TemplateField>      
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Uom}" SortExpression="OrderDetail.Uom.Name" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("OrderDetail.Uom.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
             
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.LocationFrom}" SortExpression="OrderDetail.LocationFrom" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationFrom" runat="server" Text='<%# Eval("OrderDetail.DefaultLocationFrom.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:BoundField DataField="Qty" HeaderText="${Reports.IntransitDetail.Qty}" DataFormatString="{0:0.###}" HeaderStyle-Wrap="false"/>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.CreateDate}" SortExpression="InProcessLocation.CreateDate" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWindowTime" runat="server" Text='<%# Eval("InProcessLocation.CreateDate", "{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.DeliverDate}" SortExpression="InProcessLocation.DeliverDate" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDeliverDate" runat="server" Text='<%# Eval("InProcessLocation.DeliverDate", "{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.WindowTime}" SortExpression="OrderDetail.OrderHead.WindowTime" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWindowTime" runat="server" Text='<%# Eval("OrderDetail.OrderHead.WindowTime", "{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
        <div class="tablefooter">
            <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" />
        </div>
    </fieldset>
</div>
