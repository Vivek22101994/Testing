<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_interns.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_interns" %>

<%@ Register Src="/admin/modRental/uc/EstateInternsMain.ascx" TagName="UC_Intern" TagPrefix="uc" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
    </div>
    <uc:UC_Intern runat="server" ID="UC_InternMain" />
    <script src="/jquery/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/jquery/plugin/jquerytools/all.min.js" type="text/javascript"></script>
</asp:Content>
