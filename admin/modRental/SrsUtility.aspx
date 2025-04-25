<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="SrsUtility.aspx.cs" Inherits="RentalInRome.admin.modRental.SrsUtility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LinkButton ID="lnk_update" runat="server" OnClick="lnk_update_Click">Send Cancalled Reservations to Srs</asp:LinkButton>
</asp:Content>
