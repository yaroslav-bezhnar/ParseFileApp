<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ParseFileApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Parse File Application</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css"/>
</head>
<body>
<form id="form1" runat="server">
    <div style="text-align: center">
        <asp:Label ID="lblHeader" Font-Size="Large" Font-Bold="True" CssClass="header" Text="Parse File Application" runat="server"></asp:Label>
    </div>
    <br/>
    <div>
        <asp:FileUpload ID="fileUpload" Accept=".txt" AllowMultiple="False" runat="server"/>
    </div>
    <div>
        <asp:Label ID="lblEnterText" Text="Enter word to search:" runat="server"></asp:Label>
        <asp:TextBox ID="txtEnteredText" AutoCompleteType="Disabled" Width="163px" runat="server"></asp:TextBox>
        <asp:Button ID="btnStart" Text="Start" Width="100px" OnClick="btnStart_Click" runat="server"/>
    </div>
    <div>
        <asp:Label ID="lblSavedSentences" Text="Saved sentences in database:" runat="server"></asp:Label>
        <br/>
        <asp:ListBox ID="listBox" SelectionMode="Single" style="overflow-x: auto;" Width="500px" Height="300px" runat="server"/>
    </div>
</form>
</body>
</html>