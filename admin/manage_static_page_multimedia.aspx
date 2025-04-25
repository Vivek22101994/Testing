<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="manage_static_page_multimedia.aspx.cs" Inherits="RentalInRome.admin.manage_static_page_multimedia" %>
<%@ Register src="uc/UC_static_page_multimedia.ascx" tagname="UC_static_page_multimedia" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<uc1:UC_static_page_multimedia ID="UC_static_page_multimedia1" runat="server" />
</asp:Content>
