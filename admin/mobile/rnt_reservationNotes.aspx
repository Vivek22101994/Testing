<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminMobile.Master" AutoEventWireup="true" CodeBehind="rnt_reservationNotes.aspx.cs" Inherits="RentalInRome.admin.mobile.rnt_reservationNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Value="0" />
    <asp:HiddenField ID="HF_code" runat="server" />
    <div class="sezione">
        <span>
            Note della Pren. #<%=HF_code.Value %>
        </span>
    </div>
    <div class="contenuto" id="pnl_main" runat="server">
        <div class="button_segnala">
            <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" Width="300px" Height="200px"></asp:TextBox>
        </div>
        <div class="button_segnala">
            <asp:HyperLink ID="HL_back" runat="server"><span>&lt;&lt;&nbsp;Torna</span></asp:HyperLink>
        </div>
        <div class="nulla">
        </div>
    </div>
</asp:Content>
