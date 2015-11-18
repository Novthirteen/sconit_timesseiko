<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Finance_Payment_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="PaymentNo"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="true" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PaymentNo" HeaderText="${MasterData.Payment.PaymentNo}"
                    SortExpression="PaymentNo" />
                <asp:TemplateField HeaderText=" ${MasterData.Payment.Supplier}" SortExpression="Party.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Party.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.Amount}" SortExpression="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount","{0:0.00}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.BackwashAmount}" SortExpression="BackwashAmount">
                    <ItemTemplate>
                        <asp:Label ID="lblBackwashAmount" runat="server" Text='<%# Bind("BackwashAmount","{0:0.00}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.PayType}" SortExpression="PayType">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblPayType" runat="server" Code="PayType" Value='<%# Bind("PayType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Payment.VoucherNo}" SortExpression="VoucherNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "VoucherNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PaymentDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${MasterData.Payment.PaymentDate}"
                    SortExpression="PaymentDate" />
                <asp:TemplateField HeaderText="${MasterData.Payment.CreateUser}" SortExpression="CreateUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.Status}" SortExpression="Status">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PaymentNo") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PaymentNo") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
