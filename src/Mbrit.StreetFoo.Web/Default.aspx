<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Mbrit.StreetFoo.Web._Default" MasterPageFile="~/Masters/Master.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="placeholderContent">

    <h1>StreetFoo Web Service</h1>
    See <a href="http://https://github.com/mbrit/dotnet-streetfoo">https://github.com/mbrit/dotnet-streetfoo</a>.
    <br /><br />
    New API key: <asp:TextBox runat="server" ID="textGuid" Width="300px"></asp:TextBox> <asp:Button runat="server" ID="buttonRefresh" Text="Another Code" />
    <br /><br />
    <h2>Sample data</h2>
    Need sample data for an account?
    <asp:Button runat="server" ID="buttonCreateSampleData" Text="Create Sample Data" />

</asp:Content>
