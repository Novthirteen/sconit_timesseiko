<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_Item_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/FileUpload.ascx" TagName="FileUpload" TagPrefix="uc3" %>
<div id="divFV">
    <asp:FormView ID="FV_Item" runat="server" DataSourceID="ODS_Item" DefaultMode="Insert"
        Width="100%" DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Item.NewItem}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Item.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${MasterData.Item.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${MasterData.Item.CodeExist1}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlItemImage" runat="server" Text="${MasterData.Item.Attachment}:" />
                        </td>
                        <td class="td02">
                            <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlItemCategory" runat="server" Text="${MasterData.Item.ItemCategory}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbItemCategory" runat="server" Visible="true" DescField="Desc1"
                                ValueField="Code" ServicePath="ItemCategoryMgr.service" ServiceMethod="GetAllItemCategory"
                                MustMatch="true" CssClass="inputRequired" Width="200" />
                            <asp:RequiredFieldValidator ID="rfvItemCategory" runat="server" ErrorMessage="${MasterData.Item.ItemCategory.Empty}"
                                Display="Dynamic" ControlToValidate="tbItemCategory" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlBrand" runat="server" Text="${MasterData.Item.Brand}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbBrand" runat="server" Text='<%# Bind("Brand") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlManufacturer" runat="server" Text="${MasterData.Item.Manufacturer}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbManufacturer" runat="server" Text='<%# Bind("Manufacturer") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlBom" runat="server" Text="${MasterData.Item.Bom}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="BomMgr.service" ServiceMethod="GetAllBom" MustMatch="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.Item.Description}1:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc1" runat="server" Text='<%# Bind("Desc1") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDesc1" runat="server" ErrorMessage="${MasterData.Item.Desc1.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc1" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc2" runat="server" Text="${MasterData.Item.Description}2:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc2" runat="server" Text='<%# Bind("Desc2") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc3" runat="server" Text="${MasterData.Item.Description}3:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc3" runat="server" Text='<%# Bind("TextField1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlUom" runat="server" Text="${MasterData.Item.Uom}:" />
                        </td>
                        <td class="td02">
                            <%--<asp:DropDownList ID="ddlUom" runat="server" DataTextField="Description" DataValueField="Code"
                                Text='<%# Bind("Uom") %>' DataSourceID="ODS_ddlUom">
                            </asp:DropDownList>--%>
                            <uc3:textbox ID="tbUom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="UomMgr.service" ServiceMethod="GetAllUom" MustMatch="true" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvUom" runat="server" ErrorMessage="${MasterData.Item.Uom.Empty}"
                                Display="Dynamic" ControlToValidate="tbUom" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlType" runat="server" Text="${MasterData.Item.Type}:" />
                        </td>
                        <td class="td02">
                            <asp:DropDownList ID="ddlType" runat="server" DataTextField="Description" DataValueField="Value"
                                Text='<%# Bind("Type") %>' DataSourceID="ODS_ddlType">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlCount" runat="server" Text="${MasterData.Item.Uc}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCount" runat="server" Text='<%# Bind("UnitCount") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUC" runat="server" ErrorMessage="${MasterData.Item.UC.Empty}"
                                Display="Dynamic" ControlToValidate="tbCount" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revCount" ControlToValidate="tbCount" runat="server"
                                ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlLocation" runat="server" Text="${MasterData.Item.Location}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbLocation" runat="server" Visible="true" DescField="Name" ValueField="Code"
                                ServicePath="LocationMgr.service" ServiceMethod="GetLocation" MustMatch="true"
                                Width="250" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlSpec" runat="server" Text="${MasterData.Item.Spec}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <asp:TextBox ID="tbSpec" runat="server" Text='<%# Bind("Spec") %>'  Width="85%"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Item.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="tbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                            <div class="buttons">
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Item" runat="server" TypeName="com.Sconit.Web.ItemMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Item" InsertMethod="CreateItem"
    OnInserted="ODS_Item_Inserted" OnInserting="ODS_Item_Inserting"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlType" runat="server" TypeName="com.Sconit.Web.CodeMasterMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.CodeMaster" SelectMethod="GetCachedCodeMaster">
    <SelectParameters>
        <asp:QueryStringParameter Name="code" DefaultValue="ItemType" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_ddlUom" runat="server" TypeName="com.Sconit.Web.UomMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Uom" SelectMethod="GetAllUom">
</asp:ObjectDataSource>
