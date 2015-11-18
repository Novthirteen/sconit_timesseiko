<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_IntransitDetail_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeys="ItemCode, Uom, UnitCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.Region}">
                    <ItemTemplate>
                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Eval("PartyFrom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Customer}">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("PartyTo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.ItemCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="${Common.Business.Item.Category}" >
                    <ItemTemplate>
                        <asp:Label ID="lblItemCategory" runat="server" Text='<%# Eval("ItemCategory") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Item.Spec}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemSpec" runat="server" Text='<%# Eval("ItemSpec")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Item.Brand}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemBrand" runat="server" Text='<%# Eval("ItemBrand") %>' />
                    </ItemTemplate>
                </asp:TemplateField>      
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.IntransitDetail.DefaultActivity}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtDefaultActivity" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$0$" + Eval("FlowCode")%>'
                            Text='<%# Eval("DefaultActivity", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity1" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$1$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity1", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity2" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$2$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity2", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity3" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$3$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity3", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity4" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$4$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity4", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity5" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$5$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity5", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity6" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$6$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity6", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity7" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$7$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity7", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity8" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$8$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity8", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity9" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$9$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity9", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity10" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$10$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity10", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity11" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$11$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity11", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity12" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$12$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity12", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity13" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$13$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity13", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity14" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$14$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity14", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity15" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$15$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity15", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity16" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$16$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity16", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity17" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$17$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity17", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity18" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$18$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity18", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity19" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$19$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity19", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtActivity20" runat="server" CommandArgument='<%# Eval("ItemCode") + "$" + Eval("Uom") + "$" + Eval("UnitCount") + "$20$" + Eval("FlowCode")%>'
                            Text='<%# Eval("Activity20", "{0:0.###}")%>' OnClick="lbtnDetail_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
    </div>
</fieldset>
