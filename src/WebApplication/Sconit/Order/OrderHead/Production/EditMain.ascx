<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMain.ascx.cs" Inherits="Order_OrderHead_EditMain" %>
<%@ Register Src="~/Order/OrderHead/TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc2" %>


<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
    <div class="ajax__tab_body">
        <uc2:Edit ID="ucEdit" runat="server" Visible="true" />
       
    </div>
</div>