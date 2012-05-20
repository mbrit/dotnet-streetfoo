<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleData.aspx.cs" Inherits="Mbrit.StreetFoo.Web.SampleData" MasterPageFile="~/Masters/Master.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="placeholderContent">

    <h1>Ensure Sample Data</h1>
    This utility will check to make sure that some test reports are available for the given user.

    <fieldset>
        <label for="textApiKey">API key</label>
        <asp:TextBox runat="server" ID="textApiKey"></asp:TextBox>
        <label for="textUsername">Username</label>
        <asp:TextBox runat="server" ID="textUsername"></asp:TextBox>
        <asp:Button runat="server" ID="buttonEnsureSampleData" text="Ensure Sample Data" />
    </fieldset>
    <asp:Label runat="server" ID="labelMessage"></asp:Label>

</asp:Content>
