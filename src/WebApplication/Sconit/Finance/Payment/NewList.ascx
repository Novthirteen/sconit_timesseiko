<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewList.ascx.cs" Inherits="Finance_Payment_NewList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<script language="javascript" type="text/javascript" src="Js/calcamount.js"></script>
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
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowPaging="False" DataKeyNames="BillNo" AllowSorting="False"
            AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" runat="server" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfBillNo" runat="server" Value='<%# Bind("BillNo") %>' />
                        <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.ActingBill.Supplier}" SortExpression="BillAddress.Party.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillAddress.Party.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BillNo" HeaderText="${MasterData.Bill.BillNo}"
                    SortExpression="BillNo" />
                <asp:BoundField DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="${MasterData.Bill.CreateDate}"
                    SortExpression="CreateDate" />
                <asp:TemplateField HeaderText="${MasterData.Bill.CreateUser}" SortExpression="CreateUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.Status}" SortExpression="Status">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
               <asp:TemplateField HeaderText="${MasterData.Bill.ExternalBillNo}" SortExpression="ExternalBillNo">
                    <ItemTemplate>
                         <%# DataBinder.Eval(Container.DataItem, "ExternalBillNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="${MasterData.Payment.Bill.Amount}">
                    <ItemTemplate>
                         <%# DataBinder.Eval(Container.DataItem, "AmountAfterDiscount", "{0:0.00}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="${MasterData.Bill.BackwashAmount}">
                    <ItemTemplate>
                         <%# DataBinder.Eval(Container.DataItem, "BackwashAmount", "{0:0.00}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- 
                <asp:TemplateField HeaderText="${MasterData.Payment.Bill.NoBackwashAmount}">
                    <ItemTemplate>
                         <%# DataBinder.Eval(Container.DataItem, "NoBackwashAmount", "{0:0.00}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                --%>
                <asp:TemplateField HeaderText="${MasterData.Payment.ThisReturnBackwashAmount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbThisBackwashAmount" runat="server" onmouseup="if(!readOnly)select();" Width="80px"  Text='<%# Bind("ThisBackwashAmount","{0:0.00}") %>'
                            onchange=""></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revThisBackwashAmount" runat="server" ControlToValidate="tbThisBackwashAmount"
                                ErrorMessage="${MasterData.Payment.Amount.MustNumber}" Display="Dynamic" ValidationGroup="vgAddBill"
                                ValidationExpression="^[+]?(0\.\d+)|([1-9][0-9]*(.[0-9]{2})?)$" />
                    </ItemTemplate>
                </asp:TemplateField>
             
            </Columns>
        </asp:GridView>
        <asp:Literal ID="lblNoRecordFound" runat="server" Text="${Common.GridView.NoRecordFound}"
            Visible="false" />
        <table id="tblTotalDetailAmount"  runat="server" class="mtable" Visible="false" >
            <tr>
                <td class="td02">
                   
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblTotalDetailAmount" runat="server" Text="${MasterData.Bill.TotalDetailAmount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalDetailAmount" runat="server" onfocus="this.blur();" Width="150px" />
                </td>
            </tr>
            
        </table>
    </div>
</fieldset>
