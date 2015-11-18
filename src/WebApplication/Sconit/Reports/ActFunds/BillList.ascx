<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BillList.ascx.cs" Inherits="Reports_ActFunds_BillList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv" class="GridView">
    <fieldset>
        <legend>${MasterData.Order.OrderDetail}</legend>
        <div class="GridView">
            <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                ShowHeader="true" OnRowDataBound="GV_List_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="No." ItemStyle-Width="30">
                        <ItemTemplate>
                            <asp:Literal ID="ltlSeq" runat="server" Text='<%# (Container as GridViewRow).RowIndex+1 %> ' />
                        </ItemTemplate>
                    </asp:TemplateField>
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
                    <asp:TemplateField HeaderText="${Common.Business.Item}" SortExpression="ActingBill.Item.Code"
                        HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Item.Spec}" SortExpression="ActingBill.Item.Spec"
                        HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Spec")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Item.Brand}" SortExpression="ActingBill.Item.Brand"
                        HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Brand")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.Uom}" SortExpression="ActingBill.Item.Uom.Name"
                        HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Uom.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.Business.UnitCount}" SortExpression="ActingBill.Item.UnitCount"
                        HeaderStyle-Wrap="false">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.UnitCount", "{0:0.########}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Bill.FixtureDate", "{0:yyyy-MM-dd}")%>
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
                    <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.Discount}" Visible="false" SortExpression="Discount">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Discount", "{0:0.00}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Reports.OrderTracking.BillList.OrderAmount}" SortExpression="OrderAmount">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "OrderAmount", "{0:###,##0.00}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="tablefooter">
            <asp:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" CssClass="button2"
                OnClick="btnClose_Click" />
        </div>
    </fieldset>
</div>
