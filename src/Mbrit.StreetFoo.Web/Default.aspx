<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Mbrit.StreetFoo.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>StreetFoo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>StreetFoo Web Service</h1>
        See <a href="http://https://github.com/mbrit/dotnet-streetfoo">https://github.com/mbrit/dotnet-streetfoo</a>.
        <br /><br />
        New API key: <asp:TextBox runat="server" ID="textGuid" Width="300px"></asp:TextBox> <asp:Button runat="server" ID="buttonRefresh" Text="Another Code" />
    </div>
    </form>
</body>
</html>
