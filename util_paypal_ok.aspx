<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="util_paypal_ok.aspx.cs" Inherits="RentalInRome.util_paypal_ok" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Payment Complete</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <div id="schedaDett">
    <%= ltr_title.Text%>
    <br />
        <asp:HyperLink ID="HL_goToArea" runat="server" Visible="false">Go to the reservation area</asp:HyperLink>
    </div>
</asp:Content>
