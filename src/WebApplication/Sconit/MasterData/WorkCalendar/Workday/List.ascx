﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_WorkCalendar_Workday_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.Region}" SortExpression="Region.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Region.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.WorkCenter}" SortExpression="WorkCenter.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "WorkCenter.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DayOfWeek" HeaderText="${MasterData.WorkCalendar.Workday.Week}"
                    SortExpression="DayOfWeek" />
                <asp:BoundField DataField="Type" HeaderText="${MasterData.WorkCalendar.SpecialTime.Type}"
                    SortExpression="Type" />
                <asp:BoundField DataField="Description" HeaderText="${MasterData.WorkCalendar.Description}"
                    SortExpression="Description" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
