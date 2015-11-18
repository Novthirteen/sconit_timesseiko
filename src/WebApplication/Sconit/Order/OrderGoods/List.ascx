<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Order_OrderGoods_List" %>
<fieldset>
    <asp:ListView ID="LV_List" runat="server" OnItemDataBound="LV_List_OnItemDataBound">
        <LayoutTemplate>
            <table rules="all" cellspacing="0" cellpadding="4" border="1" style="width: 100%;
                border-collapse: collapse;" id="ctl01_ucList_GV_List" class="GV">
                <tr class="GVHeader">
                    <th>
                        路线
                    </th>
                    <th>
                        物料
                    </th>
                    <th>
                        名称
                    </th>
                    <th>
                        规格型号
                    </th>
                    <th>
                        销售单号
                    </th>
                    <th>
                        客户
                    </th>
                    <th>
                        预付款已付
                    </th>
                    <th>
                        总数
                    </th>
                    <th>
                        已发数
                    </th>
                    <th>
                        已订数
                    </th>
                    <th>
                        本次订货数
                    </th>
                    <th>
                        交货日期
                    </th>
                    <th>
                        库存
                    </th>
                    <th>
                        安全库存
                    </th>
                    <th>
                        库存已订数
                    </th>
                    <th>
                        本次订货数
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
                <tbody id="itemPlaceholder" runat="server">
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <%
                if (flowQueue != null)
                {
                    this.Count++;
                    PFlow flow = flowQueue.Dequeue();
                    if (flow.ItemList == null || flow.ItemList.Count == 0)
                    {
                        return;
                    }
            %>
            <tr class="GVRow">
                <td style="display:none">
                    <asp:HiddenField ID="hf_Flow" runat="server" />
                    <asp:HiddenField ID="hf_OrderNo" runat="server" />
                    <asp:HiddenField ID="hf_OrderItem" runat="server" />
                    <asp:HiddenField ID="hf_Item" runat="server" />
                    <asp:TextBox ID="tb_InventoryQty" runat="server" />
                </td>
                <td rowspan="<%= flow.RowCount %>">
                    <%= flow.FlowCode %>
                    <input type="hidden" name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$hf_Flow" id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_hf_Flow"
                        value="<%= flow.FlowCode %>" />
                </td>
                <% 
                    for (int i = 0; i < flow.ItemList.Count; i++)
                    {
                        PItem item = flow.ItemList[i];
                        if (i > 0)
                        {
                %>
                <tr class="GVRow">
                    <% 
                        }
                    %>
                    <td rowspan="<%= item.RowCount %>">
                        <%=item.Code%>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <%=item.Desc%>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <%=item.Spec%>
                    </td>
                    <%
                        if (item.OrderList != null && item.OrderList.Count > 0)
                        {
                            for (int j = 0; j < item.OrderList.Count; j++)
                            {
                                POrder order = item.OrderList[j];
                                if (j > 0)
                                {                            
                    %>
                    <tr class="GVRow">
                        <%
                            }
                        %>
                        <td>
                            <%= order.OrderNo%>
                        </td>
                        <td>
                            <%= order.CustomerCode%>
                        </td>
                        <td align="center">
                            <% 
                                if (order.NeedPrepayment)
                                {
                            %>
                            <input type="checkbox" <%= order.HasPrepayed ? "checked" : "" %> disabled />
                            <%
                                }  
                            %>
                        </td>
                        <td>
                            <%= string.Format("{0:N}", order.RequiredQty)%>
                        </td>
                        <td>
                            <%= string.Format("{0:N}", order.ShippedQty)%>
                        </td>
                        <td>
                            <%= string.Format("{0:N}", order.OrderedQty)%>
                        </td>
                        <td>
                            <input type="hidden" name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$hf_OrderNo"
                                id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_hf_OrderNo" value="<%= order.OrderNo %>" />
                            <input type="hidden" name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$hf_OrderItem"
                                id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_hf_OrderItem" value="<%= item.Code %>" />
                            <asp:TextBox ID="tb_CurrQty" runat="server" Width="70" />
                        </td>
                        <td>
                            <%= string.Format("{0:yyyy-MM-dd}", order.DeliverDate)%>
                        </td>
                        <% 
                            if (j == 0)
                            {
                        %>
                        <td rowspan="<%= item.RowCount %>">
                            <%= string.Format("{0:N}", item.Inventory)%>
                        </td>
                        <td rowspan="<%= item.RowCount %>">
                            <%= string.Format("{0:N}", item.SaveStock)%>
                        </td>
                        <td rowspan="<%= item.RowCount %>">
                            <%= string.Format("{0:N}", item.OrderedQty)%>
                        </td>
                        <td rowspan="<%= item.RowCount %>">
                            <input type="hidden" name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$hf_Item" id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_hf_Item" value="<%= item.Code %>" />   
                            <input name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$tb_InventoryQty" type="text" id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_tb_InventoryQty" style="width:70px"/>
                        </td>
                        <% 
                            if (i == 0)
                            {
                        %>
                        <td rowspan="<%= flow.RowCount %>">
                            <asp:LinkButton ID="lbPur" Text="订货" runat="server" OnClick="lbPur_Click" />
                        </td>
                        <%
                            }
                            }
                        %>
                        <%
                            if (j > 0)
                            {
                        %>
                    </tr>
                    <%
                        }
                            }
                        }
                        else
                        {
                    %>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <%= string.Format("{0:N}", item.Inventory)%>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <%= string.Format("{0:N}", item.SaveStock)%>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <%= string.Format("{0:N}", item.OrderedQty)%>
                    </td>
                    <td rowspan="<%= item.RowCount %>">
                        <input name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$tb_InventoryQty" type="text" id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_tb_InventoryQty" style="width:70px"/>
                        <input type="hidden" name="ctl01$ucList$LV_List$ctrl<%= this.Count %>$hf_Item" id="ctl01_ucList_LV_List_ctrl<%= this.Count %>_hf_Item" value="<%= item.Code %>" />
                    </td>
                    <% 
                        if (i == 0)
                        {
                    %>
                    <td rowspan="<%= flow.RowCount %>">
                        <asp:LinkButton ID="lbPur2" Text="订货" runat="server" OnClick="lbPur_Click" />
                    </td>
                    <%
                        }
                        }
                    %>
                    <% 
                        if (i > 0)
                        {
                    %>
                </tr>
                <%          }
                    }
                %>
            </tr>
            <%
                }
            %>
        </ItemTemplate>
    </asp:ListView>
</fieldset>
