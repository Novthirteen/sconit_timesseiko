<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_Item_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Code"
            ShowSeqNo="true" AllowSorting="true" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.Item.Image}" SortExpression="ImageUrl" ItemStyle-Width="150px">
                    <ItemTemplate>
                       <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ImageUrl")%>' runat="server" ID="imgImageUrl"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Code" HeaderText="${Common.Business.Code}" SortExpression="Code" />
                <asp:TemplateField HeaderText="${Common.Business.Description} 1" SortExpression="Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Desc1")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Description} 2" SortExpression="Desc2">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Desc2")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Description} 3" SortExpression="TextField1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TextField1")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Type}" SortExpression="Type">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblType" runat="server" Code="ItemType" Value='<%# DataBinder.Eval(Container.DataItem, "Type")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="${MasterData.Item.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Uom.Code") %> [<%# DataBinder.Eval(Container.DataItem, "Uom.Description")%>]
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UnitCount" DataFormatString="{0:0.########}" HeaderText="${MasterData.Item.Uc}"
                    SortExpression="UnitCount" />
                <asp:TemplateField HeaderText="${MasterData.Item.Location}" SortExpression="Location" Visible="false">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Location.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Bom}" SortExpression="Bom" Visible="false">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Item.Routing}" SortExpression="Routing" Visible="false">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Routing.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Memo" HeaderText="${MasterData.Item.Memo}" SortExpression="Memo" Visible="false"/>
                <asp:TemplateField HeaderText="${MasterData.Item.ItemCategory}" SortExpression="ItemCategory.Desc1">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ItemCategory.Desc1")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Brand" HeaderText="${MasterData.Item.Brand}" SortExpression="Brand" />
                <%-- 
                <asp:TemplateField HeaderText="${MasterData.Item.Spec}" SortExpression="Spec" >
                    <ItemTemplate >
                        <asp:TextBox ID="tbSpec" runat="server" Text='<%# Bind("Spec") %>'
                                  TextMode="MultiLine" onfocus="this.blur();"
                                Width="100%" ReadOnly="true"  BorderWidth="0px" style="overflow-y:visible" Wrap="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
                <asp:BoundField DataField="Spec" HeaderText="${MasterData.Item.Spec}" SortExpression="Spec" ItemStyle-Wrap="true" />
                <asp:BoundField   DataField="Manufacturer" HeaderText="${MasterData.Item.Manufacturer}" SortExpression="Manufacturer" /> 
                
                <asp:CheckBoxField DataField="IsActive" HeaderText="${MasterData.Item.IsActive}"
                    SortExpression="IsActive" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
