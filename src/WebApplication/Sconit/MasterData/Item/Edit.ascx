<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_Item_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Item" runat="server" DataSourceID="ODS_Item" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_Item_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.Item.UpdateItem}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td02" rowspan="6" style="width: 150px">
                            <asp:Image ID="imgUpload" ImageUrl='<%# Eval("ImageUrl") %>' runat="server" Width="150px" />
                        </td>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Item.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlItemImage" runat="server" Text="${MasterData.Item.Attachment}:" />
                        </td>
                        <td class="td02">
                            <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
                            &nbsp;
                            <asp:HyperLink ID="hlFile" runat="server" Text="${Common.Business.ClickToDownload}"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
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
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlBom" runat="server" Text="${MasterData.Item.Bom}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBom" runat="server" Visible="true" DescField="Description" ValueField="Code"
                                ServicePath="BomMgr.service" ServiceMethod="GetAllBom" MustMatch="true" Width="250" />
                        </td>
                        <td class="td01" >
                            <asp:Literal ID="ltlManufacturer" runat="server" Text="${MasterData.Item.Manufacturer}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbManufacturer" runat="server" Text='<%# Bind("Manufacturer") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlDesc1" runat="server" Text="${MasterData.Item.Description}1:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc1" runat="server" Text='<%# Bind("Desc1") %>' CssClass="inputRequired"
                                Width="200" />
                            <asp:RequiredFieldValidator ID="rfvDesc1" runat="server" ErrorMessage="${MasterData.Item.Desc1.Empty}"
                                Display="Dynamic" ControlToValidate="tbDesc1" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDesc2" runat="server" Text="${MasterData.Item.Description}2:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc2" runat="server" Text='<%# Bind("Desc2") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlDesc3" runat="server" Text="${MasterData.Item.Description}3:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbDesc3" runat="server" Text='<%# Bind("TextField1") %>' Width="200"></asp:TextBox>
                        </td>
                    </tr>                 
                    <tr>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlUom" runat="server" Text="${MasterData.Item.Uom}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbUom" runat="server" ReadOnly="true" />
                            <%--                            <uc3:textbox ID="tbUom" runat="server" Visible="true" DescField="Description" CssClass="inputRequired"
                                ValueField="Code" ServicePath="UomMgr.service" ServiceMethod="GetAllUom" MustMatch="true" />
                            <asp:RequiredFieldValidator ID="rfvtbUom" runat="server" ErrorMessage="${MasterData.Item.Uom.Empty}"
                                Display="Dynamic" ControlToValidate="tbUom" ValidationGroup="vgSave" />--%>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlType" runat="server" Text="${MasterData.Item.Type}:" />
                        </td>
                        <td class="td02">
                            <cc1:CodeMstrDropDownList ID="ddlType" Code="ItemType" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 150px">
                        </td>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlCount" runat="server" Text="${MasterData.Item.Uc}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>'
                                CssClass="inputRequired" />
                            <asp:RegularExpressionValidator ID="revCount" ControlToValidate="tbCount" runat="server"
                                ValidationGroup="vgSave" ErrorMessage="${MasterData.Item.UC.Format}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="rfvUC" runat="server" ErrorMessage="${MasterData.Item.UC.Empty}"
                                Display="Dynamic" ControlToValidate="tbCount" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlLocation" runat="server" Text="${MasterData.Item.Location}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbLocation" runat="server" Visible="true" DescField="Name" Width="250"
                                ValueField="Code" ServicePath="LocationMgr.service" ServiceMethod="GetAllLocation"
                                MustMatch="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 150px">
                        </td>
                        <td class="td01" style="width: 100px">
                            <asp:Literal ID="ltlSpec" runat="server" Text="${MasterData.Item.Spec}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <asp:TextBox ID="tbSpec" runat="server" Text='<%# Bind("Spec") %>'  Width="85%"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01" style="width: 150px; text-align: center">
                            <asp:Literal ID="ltlDeleteImage" runat="server" Text="${MasterData.Item.DeleteImage}:" />
                            <asp:CheckBox ID="cbDeleteImage" runat="server" />
                        </td>
                        <td class="td01" style="width: 100px">
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
                        <td class="td01" style="width: 150px">
                            &nbsp;
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                            <div class="buttons">
                                <asp:Button ID="btnSave" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                                    CssClass="delete" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Item" runat="server" TypeName="com.Sconit.Web.ItemMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Item" UpdateMethod="UpdateItem"
    OnUpdated="ODS_Item_Updated" SelectMethod="LoadItem" OnUpdating="ODS_Item_Updating"
    DeleteMethod="DeleteItem" OnDeleted="ODS_Item_Deleted" OnDeleting="ODS_Item_Deleting">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
