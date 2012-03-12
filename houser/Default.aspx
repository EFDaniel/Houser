<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="houser._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Houser App</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" visible="true">
    <div>
        <span>press this button to populate the sales data</span>
        <asp:Button ID="btnPopulateData" Text="Get Data" runat="server" Visible="true" 
            onclick="btnPopulateData_Click"/>
        <asp:DropDownList ID="ddlSaleDate" runat="server" 
            onselectedindexchanged="ddlSaleDate_SelectedIndexChanged" />
    </div>
    <div id="displayData">
        <asp:Panel ID="displayPanel" runat="server">
        </asp:Panel>
    </div>
    </form>
    
</body>
</html>
