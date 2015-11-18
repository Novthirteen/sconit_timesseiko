<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Order_OrderDetail_List" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="View.ascx" TagName="View" TagPrefix="uc2" %>

<script language="javascript" type="text/javascript" src="Js/calcamount.js"></script>

<script language="javascript" type="text/javascript">
    function discountChanged(obj, isBlank) {
       
        if('<%= trIncludeTaxPrice.Visible %>' == 'True')
        {
            CalCulateRowAmount(obj, "tbDiscount", "BaseOnDiscount", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>',  '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>', isBlank);
        }else
        {
            CalCulateRowAmount(obj, "tbDiscount", "BaseOnDiscount", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>',  isBlank);
        }
    }
    function discountRateChanged(obj, isBlank) {
        
        if('<%= trIncludeTaxPrice.Visible %>' == 'True')
        {
            CalCulateRowAmount(obj, "tbDiscountRate", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>', isBlank);
        }
        else
        {
            CalCulateRowAmount(obj, "tbDiscountRate", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', isBlank);
        }
    }
    function calcPrice(obj, isBlank) {
        
        if('<%= trIncludeTaxPrice.Visible %>' == 'True')
        {
            CalCulateRowAmount(obj, "tbOrderQty", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>',  '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>',isBlank);
        }else
        {
            CalCulateRowAmount(obj, "tbOrderQty", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>',null,null,null, isBlank);
        }
    }
    function GenerateFlowDetail(obj) {
        
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbItemCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GenerateFlowDetailProxy', false,
                { "flowCode": "<%=FlowCode%>", "itemCode": $(obj).val(), "partyFromCode": "<%=PartyFromCode%>", "partyToCode": "<%=PartyToCode%>",
                    "moduleType": "<%=ModuleType%>", "changeRef": true, "startTime": "<%=StartTime%>"
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbItemDescription').attr('value', result.ItemDescription);
                $('#' + parentId + 'tbRefItemCode_suggest').attr('value', result.ItemReferenceCode);
                $('#' + parentId + 'tbUom_suggest').attr('value', result.UomCode);
                $('#' + parentId + 'tbUnitCount').attr('value', result.UnitCount);
                //$('#' + parentId + 'tbHuLotSize').attr('value', result.HuLotSize);
                $('#' + parentId + 'tbUnitPrice').attr('value', result.UnitPrice);
                $('#' + parentId + 'hfPriceListCode').attr('value', result.PriceListCode);
                $('#' + parentId + 'hfPriceListDetailId').attr('value', result.PriceListDetailId);
                $('#' + parentId + 'hfLocationCode').attr('value', result.LocationCode);
                
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }

    function orderDiscountChanged(obj) {
        if('<%= trIncludeTaxPrice.Visible %>' == 'True')
        {
            CalCulateTotalAmount("BaseOnDiscount", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0,  '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>');
        }
        else
        {
            CalCulateTotalAmount("BaseOnDiscount", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0);
        }
    }

    function orderDiscountRateChanged(obj) {
        if('<%= trIncludeTaxPrice.Visible %>' == 'True')
        {
            CalCulateTotalAmount("BaseOnDiscountRate", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0,  '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>');
        }
        else
        {
            CalCulateTotalAmount("BaseOnDiscountRate", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0);
        }
    }

    function GetUnitPriceByUom(obj) {
    
        if ($(obj).val() != "") {
            var objId = $(obj).attr("id");
            var parentId = objId.substring(0, objId.length - "tbUom_suggest".length);
            var priceListCodeId = "#" + parentId + "hfPriceListCode";
            var itemCodeId = "#" + parentId + "tbItemCode_suggest";
            var orderQtyId = "#" + parentId + "tbOrderQty";
            var priceListCode;
            if ($(priceListCodeId).val() == undefined) {
                priceListCode = "";
            } else {
                priceListCode = $(priceListCodeId).val();
            }
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GetUnitPriceByUom', false,
                { "priceListCode": priceListCode, "itemCode": $(itemCodeId).val(), "startTime": "<%=StartTime%>", "currencyCode": "<%=currencyCode%>",
                    "uomCode": $(obj).val()
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbUnitPrice').attr('value', result);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
           if('<%= trIncludeTaxPrice.Visible %>' == 'True')
            {
                CalCulateRowAmount(obj, "tbUom_suggest", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>',  '#<%= hfTax.ClientID %>', '#<%= tbOrderTax.ClientID %>', '#<%= tbOrderIncludeTaxPrice.ClientID %>', true);
            }
            else
            {
                CalCulateRowAmount(obj, "tbUom_suggest", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', true);
            }
        }
    }
</script>

<fieldset runat="server" id="fdDetail">
    <legend>${MasterData.Order.OrderDetail}</legend>
    <div>
        <asp:CustomValidator ID="cvShipQty" runat="server" />
        <asp:CustomValidator ID="cvReceiveQty" runat="server" />
        <asp:HiddenField ID="hfIsChanged" runat="server" Value="N" />
        <asp:HiddenField ID="hfHuIdPageX" runat="server" />
        <asp:HiddenField ID="hfTax" runat="server" />
        <asp:HiddenField ID="hfHuIdPageY" runat="server" />
        <div class="GridView">
            <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                OnRowDataBound="GV_List_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Sequence}">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                            <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("Sequence") %>' onmouseup="if(!readOnly)select();" />
                            <asp:TextBox ID="tbSeq" runat="server" onmouseup="if(!readOnly)select();" Visible="false"
                                Width="30" Text='<%# Bind("Sequence") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Code}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' Width="100" />
                            <uc3:textbox ID="tbItemCode" runat="server" Visible="false" Width="250" DescField="DescriptionAndSpec"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" InputWidth="150" MustMatch="true" />
                            <asp:RequiredFieldValidator ID="rfvItemCode" runat="server" ControlToValidate="tbItemCode"
                                Display="Dynamic" ErrorMessage="${MasterData.Order.OrderDetail.ItemCode.Required}"
                                ValidationGroup="vgAdd" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Category}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCategory" runat="server" Text='<%# Bind("Item.Desc1") %>'
                                 />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Spec}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemSpec" runat="server" Text='<%# Bind("Item.Spec") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Brand}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemBrand" runat="server" Text='<%# Bind("Item.Brand") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Uom}">
                        <ItemTemplate>
                            <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom.Code") %>' />
                            <uc3:textbox ID="tbUom" runat="server" Visible="false" Width="200" DescField="Description"
                                ServiceParameter="string:#tbItemCode" ValueField="Code" ServicePath="UomMgr.service"
                                InputWidth="50" ServiceMethod="GetItemUom" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvUom" runat="server" ControlToValidate="tbUom"
                                Display="Dynamic" ErrorMessage="${MasterData.Order.OrderDetail.Uom.Required}"
                                ValidationGroup="vgAdd" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationFrom}">
                        <ItemTemplate>
                            <asp:Label ID="lblLocFrom" runat="server" Text='<%# Bind("DefaultLocationFrom.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationTo}">
                        <ItemTemplate>
                            <asp:Label ID="lblLocTo" runat="server" Text='<%# Bind("DefaultLocationTo.Name") %>' />
                            <asp:DropDownList ID="ddlLocTo" runat="server" Visible="false" DataValueField="Code"
                                DataTextField="Name" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Bom}">
                        <ItemTemplate>
                            <asp:Label ID="lblBom" runat="server" Text='<%# Bind("Bom.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("OrderedQty","{0:0.########}") %>'
                                Width="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revOrderQty" runat="server" Display="Dynamic"
                                ControlToValidate="tbOrderQty" Enabled="false"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ShippedQty}">
                        <ItemTemplate>
                            <asp:Label ID="lblShippedQty" runat="server" Text='<%# Bind("ShippedQty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentShipQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbShipQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentShipQty","{0:0.########}") %>'
                                Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ReceivedQty}">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedQty" runat="server" Text='<%# Bind("ReceivedQty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentReceiveQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbReceiveQty" runat="server" onmouseup="if(!readOnly)select();"
                                Text='<%# Bind("CurrentReceiveQty","{0:0.########}") %>' onchange="setChanged();"
                                Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentRejectQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbRejectQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentRejectQty","{0:0.########}") %>'
                                onchange="setChanged();" Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentScrapQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbScrapQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentScrapQty","{0:0.########}") %>'
                                onchange="setChanged();" Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.UnitPrice}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbUnitPrice" runat="server" Width="50" onfocus="this.blur();" Text='<%# Bind("UnitPrice","{0:0.00}") %>' />
                            <asp:HiddenField ID="hfPriceListCode" runat="server" />
                            <asp:HiddenField ID="hfPriceListDetailId" runat="server" />
                            <asp:HiddenField ID="hfLocationCode" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.DiscountRate}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbDiscountRate" runat="server" onmouseup="if(!readOnly)select();"
                                Width="50" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Discount}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbDiscount" runat="server" onmouseup="if(!readOnly)select();" Width="50" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Amount}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbPrice" runat="server" Width="50" onfocus="this.blur();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.GridView.Action}" Visible="false">
                        <ItemTemplate>
                            <cc1:LinkButton ID="lbtnAdd" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                Text="${Common.Button.New}" OnClick="lbtnAdd_Click" Visible="false" FunctionId="EditOrderDetail"
                                ValidationGroup="vgAdd">
                            </cc1:LinkButton>
                            <cc1:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                                FunctionId="DeleteOrderDetail">
                            </cc1:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <table class="mtable" id="tabPrice" runat="server" visible="false">
        <tr>
            <td class="td02">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderDetailPrice" runat="server" Text="${MasterData.Order.OrderDetail.TotalPrice}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderDetailPrice" runat="server" onfocus="this.blur();" Width="150px" />
            </td>
        </tr>
        <tr id="trdiscount" runat="server" >
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderDiscountRate" runat="server" Text="${MasterData.Order.OrderHead.Discount}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderDiscountRate" runat="server" onChange="orderDiscountRateChanged(this);"
                    onmouseup="if(!readOnly)select();" Width="65px" />%
                <asp:TextBox ID="tbOrderDiscount" runat="server" onChange="orderDiscountChanged(this);"
                    onmouseup="if(!readOnly)select();" Width="63px" />
            </td>
        </tr>
        <tr id="trOrderPrice" runat="server">
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderPrice" runat="server" Text="${MasterData.Order.OrderHead.Price}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderPrice" runat="server" Visible="true" onfocus="this.blur();"
                    Width="150px" />
            </td>
        </tr>
        <tr id="trTax" runat="server" visible="false">
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderTax" runat="server" Text="${MasterData.Order.OrderHead.Tax}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderTax" runat="server" Visible="true" onfocus="this.blur();"
                    Width="150px" />
            </td>
        </tr>
        <tr id="trIncludeTaxPrice" runat="server" visible="false">
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderIncludeTaxPrice" runat="server" Text="${MasterData.Order.OrderHead.IncludeTaxPrice}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderIncludeTaxPrice" runat="server" Visible="true" onfocus="this.blur();"
                    Width="150px" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:View ID="ucView" runat="server" Visible="false" />
