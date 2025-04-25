<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlAirbnb_main.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlAirbnb_main" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlAirbnbTab.ascx" TagName="ucNav" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <style type="text/css">
                .main div.mainbox {
                    width: 100%;
                }
            </style>

            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <%--<%# rntUtils.getAgent_logoForDetailsPage(ChnlAirbnbProps.IdAdMedia) %>--%>
            <h1 class="titolo_main">
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1" class="airbnblist">                
                <uc1:ucNav ID="ucNav" runat="server" />                
            </div>
            <div class="nulla">
            </div>
            <%--<div class="mainline" id="div_status" runat="server">
                <table>
                    <tr>
                        <td>Status :
                            <asp:DropDownList ID="drp_status" runat="server">
                                <asp:ListItem Text="new" Value="new"></asp:ListItem>
                                <asp:ListItem Text="ready for review" Value="ready for review"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnk_update_details" runat="server" OnClick="lnk_update_details_Click" CssClass="btn">Update Details</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
