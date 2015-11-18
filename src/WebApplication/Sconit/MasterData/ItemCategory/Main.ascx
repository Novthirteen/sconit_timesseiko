<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_ItemCategory_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset id="fldSearch" runat="server">
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.ItemCategory.Code}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbCode" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.ItemCategory.Desc1}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbDesc1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td colspan="3">
                    </td>
        <td class="ttd02">
            <asp:Button ID="SearchBtn" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                CssClass="button2" />
            <asp:Button ID="btnInsert" runat="server" Text="${Common.Button.New}" OnClick="btnInsert_Click"
                CssClass="button2" />
        </td>
        
        </tr>
    </table>
</fieldset>
<fieldset id="fldInsert" runat="server" visible="false">
    <legend>${MasterData.ItemCategory.NewItemCategory}</legend>
    <asp:FormView ID="FV_ItemCategory" runat="server" DataSourceID="ODS_FV_ItemCategory" DefaultMode="Insert">
        <InsertItemTemplate>
            <table class="mtable">
                <tr>
                    <td class="ttd01">
                        <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.ItemCategory.Code}:" />
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>'></asp:TextBox>
                    </td>
                    <td class="ttd01">
                        <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.ItemCategory.Desc1}:" />
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbDesc1" runat="server" Text='<%# Bind("Desc1") %>'></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3">
                    </td>
                    <td>
                        <asp:Button ID="btnNew" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                            CssClass="button2" ValidationGroup="vgSave" />
                        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                            CssClass="button2" />
                    </td>
                </tr>
            </table>
        </InsertItemTemplate>
    </asp:FormView>
</fieldset>
<fieldset id="fldGV" runat="server" visible="false">
    <asp:GridView ID="GV_ItemCategory" runat="server" AutoGenerateColumns="False" DataSourceID="ODS_GV_ItemCategory"
        OnDataBound="GV_ItemCategory_OnDataBind" DataKeyNames="Code" AllowPaging="True" PageSize="10">
        <Columns>
            <asp:BoundField ReadOnly="true" DataField="Code" HeaderText="${MasterData.ItemCategory.Code}"
                ItemStyle-Width="20%" />
            <asp:BoundField DataField="Desc1" HeaderText="${MasterData.ItemCategory.Desc1}" ItemStyle-Width="30%" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" ItemStyle-Width="20%"
                HeaderText="${Common.GridView.Action}" DeleteText="&lt;span onclick=&quot;JavaScript:return confirm('${Common.Delete.Confirm}?')&quot;&gt;${Common.Button.Delete}&lt;/span&gt;">
            </asp:CommandField>
        </Columns>
    </asp:GridView>
    <asp:Label runat="server" ID="lblResult" Text="${Common.GridView.NoRecordFound}"
        Visible="false" />
</fieldset>
<asp:ObjectDataSource ID="ODS_GV_ItemCategory" runat="server" TypeName="com.Sconit.Web.ItemCategoryMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemCategory" SelectMethod="LoadItemCategory"
    UpdateMethod="UpdateItemCategory" OnUpdating="ODS_GV_ItemCategory_OnUpdating" DeleteMethod="DeleteItemCategory"
    OnDeleting="ODS_GV_ItemCategory_OnDeleting" OnDeleted="ODS_GV_ItemCategory_OnDeleted">
    <SelectParameters>
        <asp:ControlParameter ControlID="tbCode" Name="Code" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="tbDesc1" Name="Desc1" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_FV_ItemCategory" runat="server" TypeName="com.Sconit.Web.ItemCategoryMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemCategory" InsertMethod="CreateItemCategory"
    OnInserted="ODS_ItemCategory_Inserted" OnInserting="ODS_ItemCategory_Inserting"></asp:ObjectDataSource>
