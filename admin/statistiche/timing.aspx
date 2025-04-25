<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="timing.aspx.cs" Inherits="RentalInRome.admin.statistiche.timing" %>
<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=7.2.14.127, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="1550px" Width="100%">
        <typereportsource typename="TimeAccountRent.Report1, TimeAccountRent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"></typereportsource>
    </telerik:ReportViewer>
</asp:Content>
