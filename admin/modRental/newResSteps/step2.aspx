<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="step2.aspx.cs" Inherits="ModRental.admin.modRental.newResSteps.step2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register src="../uc/ucReservationTmpPriceChange.ascx" tagname="ucReservationTmpPriceChange" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <h1 class="titolo_main">Nuova prenotazione</h1>
            <asp:Panel ID="pnlContent" runat="server" Width="50%">
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Struttura</span>
                            <div class="boxmodulo">
                                <%= CurrentSource.rntEstate_code(currTBL.pidEstate.objToInt32(), "- - -") %>
                            </div>
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Periodo e Ospiti</span>
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            Check In:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.dtStart.formatCustom("#dd# #MM# #yy#", 1, "- - -") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Check Out:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.dtEnd.formatCustom("#dd# #MM# #yy#", 1, "- - -") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Adulti:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.numPers_adult %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Bambini over 3:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.numPers_childOver %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Neonati (gratis):
                                        </td>
                                        <td align="right">
                                            <%= currTBL.numPers_childMin %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                    </div>
                    <div class="mainbox">
                        <div class="top">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Dati anagrafici</span>
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">
                                            E-mail:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.cl_email %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Nome completo:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.cl_name_honorific + " " + currTBL.cl_name_full%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Lingua:
                                        </td>
                                        <td align="right">
                                            <%= contUtils.getLang_title(currTBL.cl_pid_lang.objToInt32()) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Cellulare:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.cl_contact_phone_mobile %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Paese:
                                        </td>
                                        <td align="right">
                                            <%= currTBL.cl_loc_country %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="mainbox">
                        <div class="top">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-bottom: 0;">Costi</span>
                            <div class="boxmodulo">
                                <uc2:ucReservationTmpPriceChange ID="ucPrice" runat="server" />
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="sx">
                            </div>
                            <div class="dx">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_save" runat="server" ValidationGroup="dati" OnClick="lnk_save_Click"><span>Salva i costi</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <a href="#" onclick="CloseRadWindow('');return false;"><span>Annulla / Chiudi</span></a>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
