<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IpDetList.ascx.cs" Inherits="Reports_SalesOrderTracking_IpDetList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<legend>${Reports.SalesOrderTracking.DeliverQtyDetail}</legend>
<div class="GridView">
    <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
        AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
        ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
        CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
        SelectCountMethod="FindCount" DefaultSortExpression="Id" DefaultSortDirection="Ascending"
        OnRowDataBound="GV_List_RowDataBound">
        <Columns>
            <asp:TemplateField SortExpression="OrderLocationTransaction.OrderDetail.OrderHead.OrderNo"
                HeaderText="${MasterData.Order.OrderHead.OrderNo.Distribution}" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.OrderDetail.OrderHead.OrderNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.IpNo}" SortExpression="InProcessLocation.IpNo"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "InProcessLocation.IpNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.DeliverDate}" SortExpression="InProcessLocation.CreateDate"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "InProcessLocation.CreateDate", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.SalesOrderTracking.DeliverUser}" SortExpression="InProcessLocation.CreateUser.Code"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "InProcessLocation.CreateUser.Code", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item}" SortExpression="Item.Code"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.Item.Code")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Spec}" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.Item.Spec")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Brand}" HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.Item.Brand")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="OrderLocationTransaction.Item.Uom.Name"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.Item.Uom.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.UnitCount}" SortExpression="OrderLocationTransaction.Item.UnitCount"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderLocationTransaction.Item.UnitCount", "{0:0.########}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="Qty" HeaderStyle-Wrap="false" HeaderText="${Reports.SalesOrderTracking.DeliverQty}">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Qty", "{0:#.##}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- 
                <asp:TemplateField  HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblNoRecQty" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
        </Columns>
    </cc1:GridView>
    <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
    </cc1:GridPager>
</div>
