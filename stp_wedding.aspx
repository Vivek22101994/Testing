<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_wedding.aspx.cs" Inherits="RentalInRome.stp_wedding" %>

<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + "</h3>" : ""%>
    <div class="nulla">
    </div>
    <div id="imgWedding">
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-1.jpg" alt="wedding in rome" />
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-2.jpg" alt="wedding in rome" />
        <!--<img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-3.jpg" alt="wedding in rome" />-->
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-4.jpg" alt="wedding in rome" />
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-5.jpg" alt="wedding in rome" />
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-6.jpg" alt="wedding in rome" />
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-7.jpg" alt="wedding in rome" />
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/wedding-in-rome-8.jpg" alt="wedding in rome" />
    </div>
    <div class="txtWedding">
        <%=ltr_description.Text %>
    </div>
    <div class="nulla">
    </div>
    <a href="http://www.yourweddinginrome.com/" target="_blank"><img style="padding-left: 22px;" src="/images/img_banner_wedding.jpg" alt="wedding in rome"></a>
</asp:Content>
