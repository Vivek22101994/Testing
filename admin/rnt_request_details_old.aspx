<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_request_details_old.aspx.cs" Inherits="RentalInRome.admin.rnt_request_details_old" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_rnt_rl_request_state.ascx" TagName="UC_rnt_rl_request_state" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_request_operator.ascx" TagName="UC_rnt_request_operator" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <h1 class="titolo_main">Scheda Richiesta</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_modify" Visible="false" runat="server" OnClick="lnk_modify_Click" OnClientClick="removeTinyEditor();"><span>Modifica</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" Visible="false" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" Visible="false" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span></a>
                </div>
                <div class="bottom_salva">
                    <a href="rnt_request_details.aspx?id=<%= HF_id.Value %>">
                        <span>Nuova Scheda</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Cliente</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Data/Ora richiesta:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_date_request" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nome Cognome:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_name" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        E-mail:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_email" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Telefono:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_phone" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nazione/Location:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_country" Width="300px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Dati Prenotazione</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Check-in:
                                    </td>
                                    <td>
                                        <input type="text" id="cal_date_start" style="width: 120px" readonly="readonly" />
                                        <asp:HiddenField ID="HF_date_start" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Check-out:
                                    </td>
                                    <td>
                                        <input type="text" id="cal_date_end" style="width: 120px" readonly="readonly" />
                                        <asp:HiddenField ID="HF_date_end" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Date Flessibili?:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_date_is_flexible" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        num. Adulti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_adult_num" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        num. Bambini:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_child_num" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Trasporto:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_transport" Width="300px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Opzioni</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Price Range:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_price_range" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Servizi:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_services" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Richiesta speciale:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_notes" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">
                                    Preferenza-1:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_choice_1" Width="300px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Preferenza-2:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_choice_2" Width="300px" />
                                </td>
                            </tr>
                        </table>
                        <span class="titoloboxmodulo">Preferenze</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" width="100%">
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltr_choices" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Area / Zona</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltr_area" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Correlazione</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td colspan="2">
                                        <asp:HyperLink ID="HL_related_request" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ListView runat="server" ID="LV_relatedRequests">
                                            <ItemTemplate>
                                                <a href="rnt_request_details.aspx?id=<%# Eval("id")%>">rif.
                                                    <%# Eval("id")%>
                                                    -
                                                    <%# Eval("name_full")%>
                                                    - del
                                                    <%# Eval("request_date_created")%></a>
                                                <br />
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <a href="rnt_request_details.aspx?id=<%# Eval("id")%>">rif.
                                                    <%# Eval("id")%>
                                                    -
                                                    <%# Eval("name_full")%>
                                                    - del
                                                    <%# Eval("request_date_created")%></a>
                                                <br />
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                Richiesta Unica
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                Richieste Correlate:<br />
                                                <a id="itemPlaceholder" runat="server" />
                                            </LayoutTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr id="pnl_setRelatedRequest" runat="server" visible="false">
                                    <td colspan="2">
                                        Imposta come secondaria di:
                                        <asp:DropDownList ID="drp_relatedRequests" runat="server">
                                        </asp:DropDownList>
                                        <div class="nulla">
                                        </div>
                                        <span class="error_text" id="lbl_relatedRequestError" runat="server" visible="false">!Attenzione! Selezionare Una richiesta</span>
                                        <asp:LinkButton ID="lnk_setRelatedRequest" runat="server" OnClick="lnk_setRelatedRequest_Click">Salva</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="center">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc3:UC_rnt_request_operator ID="UC_rnt_request_operator1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <asp:UpdatePanel ID="UpdatePanel_UC_rnt_rl_request_state" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc2:UC_rnt_rl_request_state ID="UC_rnt_rl_request_state1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
                <asp:TextBox runat="server" ID="txt_css" Width="230px"></asp:TextBox>
                Banner Laterali:
                <asp:DropDownList ID="drp_static_collection" runat="server">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
		var cal_date_start_from = new JSCal_single('cal_date_start', '<%= HF_date_start.ClientID %>', null, '', '', null);
		var cal_date_end_from = new JSCal_single('cal_date_end', '<%= HF_date_end.ClientID %>', null, '', '', null);
    </script>

</asp:Content>
