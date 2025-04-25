<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHoliday_main.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHoliday_main" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHLEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <style type="text/css">
                .main div.mainbox
                {
                    width: 100%;
                }
            </style>

            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/hl-integrated.jpg" class="homeAwayLogo" alt="Integrated with Holiday Lettings" style="height: 65px; width: 130px;" />
            <h1 class="titolo_main">Dati generali del Holiday Lettings:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <span class="titoloboxmodulo">Abbina ad una struttura esistente su HL</span>
                        <div class="boxmodulo">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="middle">Home ID:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_homeID" />
                                    </td>
                                </tr>
                            </table>
                            <asp:LinkButton ID="lnkSaveExisting" runat="server" CssClass="inlinebtn" OnClick="lnkSaveExisting_Click">Salva Home ID</asp:LinkButton>
                            <asp:LinkButton ID="lnkGetFromHL" runat="server" CssClass="inlinebtn" OnClick="lnkGetFromHL_Click">Importa scheda da HL</asp:LinkButton>
                            <asp:LinkButton ID="lnkCreateNew" runat="server" CssClass="inlinebtn" OnClick="lnkCreateNew_Click">Crea nuovo Home ID</asp:LinkButton>
                            <asp:LinkButton ID="lnkGetFromRental" runat="server" CssClass="inlinebtn" OnClick="lnkGetFromRental_Click">Copia scheda MagaRental</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="mainbox iCalMainBox" id="pnlActiveState" runat="server">
                    <div class="center">
                        <span class="titoloboxmodulo">Active State</span>
                        <div class="boxmodulo">
                            <asp:LinkButton ID="lnkActiveStateOn" runat="server" CssClass="inlinebtn" OnClick="lnkActiveState_Click" CommandArgument="on">Attiva</asp:LinkButton>
                            <asp:LinkButton ID="lnkActiveStateOff" runat="server" CssClass="inlinebtn" OnClick="lnkActiveState_Click" CommandArgument="off">Disattiva</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
