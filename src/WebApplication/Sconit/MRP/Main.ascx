<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MRP_Main" %>
<%@ Register Src="TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="./PlanSchedule/Main.ascx" TagName="PlanSchedule" TagPrefix="uc2" %>
<%@ Register Src="./PlanImport/Main.ascx" TagName="PlanImport" TagPrefix="uc2" %>

<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
<div class="ajax__tab_body">
    <uc2:PlanSchedule ID="ucPlanSchedule" runat="server" Visible="true" />
    <uc2:PlanImport ID="ucPlanImport" runat="server" Visible="false" />
</div>
</div>