<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BillList.ascx.cs" Inherits="Reports_OrderTracking_BillList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<legend>${Reports.OrderTracking.BillList}</legend>
<div class="GridView">
    <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
        AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
        ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
        CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
        SelectCountMethod="FindCount" DefaultSortExpression="Id" DefaultSortDirection="Ascending">
        <Columns>
            <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.BillNo}" SortExpression="Bill.BillNo">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Bill.BillNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.OrderTracking.ExternalBillNo}" SortExpression="Bill.ExternalBillNo">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Bill.ExternalBillNo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item}" SortExpression="Item.Code"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Code")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Spec}" SortExpression="Item.Spec"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Spec")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Item.Brand}" SortExpression="Item.Brand"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Brand")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="Item.Uom.Name"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.Uom.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.Business.UnitCount}" SortExpression="Item.UnitCount"
                HeaderStyle-Wrap="false">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Item.UnitCount", "{0:0.########}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.BilledQty}" SortExpression="BilledQty">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "BilledQty", "{0:0.########}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.UnitPrice}" SortExpression="UnitPrice">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "UnitPrice", "{0:###,##0.00}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.OrderAmount}" SortExpression="OrderAmount">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "OrderAmount", "{0:###,##0.00}")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
    </cc1:GridPager>
</div>
