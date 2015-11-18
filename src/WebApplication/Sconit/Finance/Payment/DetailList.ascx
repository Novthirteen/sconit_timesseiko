<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetailList.ascx.cs" Inherits="Finance_DetailList_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="NewSearch.ascx" TagName="NewSearch" TagPrefix="uc" %>

<script type="text/javascript" language="javascript">
    function GVCheckClick() {
        if ($(".GVHeader input:checkbox").attr("checked") == true) {
            $(".GVRow input:checkbox").attr("checked", true);
            $(".GVAlternatingRow input:checkbox").attr("checked", true);
        }
        else {
            $(".GVRow input:checkbox").attr("checked", false);
            $(".GVAlternatingRow input:checkbox").attr("checked", false);
        }
    }
</script>

<fieldset>
    <legend runat="server" id="lgdTitle">${MasterData.Bill.POBill}</legend>
    <div class="GridView">
        <asp:GridView ID="Gv_List" runat="server" AllowPaging="False" DataKeyNames="Id" AllowSorting="False"
            AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" runat="server" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfBillNo" runat="server" Value='<%# Bind("Bill.BillNo") %>' />
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                        <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.ExternalBillNo}" SortExpression="Bill.ExternalBillNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bill.ExternalBillNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Bill.BillNo}" SortExpression="Bill.BillNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bill.BillNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Bill.Supplier}" SortExpression="Bill.BillAddress.Party.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bill.BillAddress.Party.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Bill.CreateDate}" SortExpression="Bill.CreateDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bill.CreateDate", "{0:yyyy-MM-dd HH:mm}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.CreateUser}" SortExpression="Bill.CreateUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Bill.CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.Status}" SortExpression="Bill.Status">
                    <ItemTemplate>
                        <sc1:CodeMstrLabel ID="lblStatus" runat="server" Code="Status" Value='<%# Bind("Bill.Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.BillAmount}" SortExpression="Bill.AmountAfterDiscount">
                    <ItemTemplate>
                        <asp:TextBox ID="tbTotalDetailAmount" runat="server" Width="80px" onfocus="this.blur();"
                            Text='<%# Bind("Bill.AmountAfterDiscount","{0:0.00}") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.BackwashAmount}" SortExpression="Bill.BackwashAmount">
                    <ItemTemplate>
                        <asp:TextBox ID="tbBillBackwashAmount" runat="server" Width="80px" onfocus="this.blur();"
                            Text='<%# Bind("Bill.BackwashAmount","{0:0.00}") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.NoBackwashAmount}" SortExpression="Bill.NoBackwashAmount"
                    Visible="false">
                    <ItemTemplate>
                        <asp:TextBox ID="tbBillNoBackwashAmount" runat="server" Width="80px" onfocus="this.blur();"
                            Text='<%# Bind("Bill.NoBackwashAmount","{0:0.00}") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Payment.ThisReturnBackwashAmount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbThisBackwashAmount" runat="server" onmouseup="if(!readOnly)select();"
                            Width="80px" Text='<%# Bind("BackwashAmount","{0:0.00}") %>' onchange=""></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revThisBackwashAmount" runat="server" ControlToValidate="tbThisBackwashAmount"
                            ErrorMessage="${MasterData.Payment.Amount.MustNumber}" Display="Dynamic" ValidationGroup="vgSave"
                            ValidationExpression="^[+]?(0\.\d+)|([1-9][0-9]*(.[0-9]{2})?)$" />
                        <asp:RequiredFieldValidator ID="rfvThisBackwashAmount" runat="server" ErrorMessage="${MasterData.Payment.Amount.MustNumber}"
                                Display="Dynamic" ControlToValidate="tbThisBackwashAmount" ValidationGroup="vgSave" />
                       <%-- 
                        <asp:CompareValidator ID="cvThisBackwashAmount" runat="server" ControlToValidate="tbThisBackwashAmount" ValueToCompare="0"
                            Type="Currency" Operator="Equal" Display="Dynamic" ValidationGroup="vgSage" ErrorMessage="${MasterData.Payment.Amount.MustNumber1111}" />
                        
                        
                        <asp:CompareValidator runat="server" Display="Dynamic" ValidationGroup="vgSave" 
                                ErrorMessage="${MasterData.Payment.Return.Blunt.Amount.Must.Be.Less.Than.Or.Equal.To.Bill.Amount}" 
                                ControlToValidate="tbThisBackwashAmount" ControlToCompare="tbBillNoBackwashAmount" 
                                Operator="LessThan" Type="Currency"/>
                                --%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table class="mtable">
            <tr>
                <td class="td02">
                    <asp:Button ID="btnAddBill" runat="server" Text="${Common.Button.New}" OnClick="btnAddBill_Click" />
                    <asp:Button ID="btnDeleteBill" runat="server" Text="${Common.Button.Remove}" OnClick="btnDeleteBill_Click" />
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<div id="floatdiv">
    <uc:NewSearch ID="ucNewSearch" runat="server" Visible="false" />
</div>
