<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderTracerList.ascx.cs"
    Inherits="Reports_SalesOrderTracking_OrderTracerList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<legend>${Reports.SalesOrderTracking.OrderTracerList}</legend>
<div class="GridView">
    <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
        AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
        ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
        CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
        SelectCountMethod="FindCount" DefaultSortExpression="Id" DefaultSortDirection="Ascending">
        <Columns>
            <asp:TemplateField SortExpression="OrderHead.OrderNo" HeaderText="${MasterData.Order.OrderHead.OrderNo.Procurement}">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderHead.OrderNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Seq}" SortExpression="Sequence">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Sequence")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.CreateDate}" SortExpression="OrderHead.CreateDate">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderHead.CreateDate", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.SalePerformance.Buyer}" SortExpression="OrderHead.CreateUser.Code">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderHead.CreateUser.Code")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Item.Manufacturer}" SortExpression="Manufacturer">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Manufacturer")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item}" SortExpression="Item.Code">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Code")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Spec}">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Spec")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Brand}">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Brand")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="OrderDetail.Uom.Name">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Uom.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.UnitCount}" SortExpression="OrderDetail.UnitCount">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "UnitCount", "{0:0.########}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- 
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.OrderTracerList.ReqTime}"
                    SortExpression="ReqTime">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ReqTime", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.OrderTracerList.RequiredQty}"
                    SortExpression="Qty">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RequiredQty", "{0:###,##0.00}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
            <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.OrderTracerList.OrderedQty}"
                SortExpression="OrderedQty">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderedQty", "{0:0.########}")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
    </cc1:GridPager>
</div>
