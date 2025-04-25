<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_reservation_form.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_form" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_rnt_reservation_client.ascx" TagName="UC_rnt_reservation_client" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_estate.ascx" TagName="UC_rnt_reservation_estate" TagPrefix="uc3" %>
<%@ Register Src="uc/UC_rnt_reservation_dt_pers.ascx" TagName="UC_rnt_reservation_dt_pers" TagPrefix="uc4" %>
<%@ Register src="uc/UC_rnt_reservation_state.ascx" tagname="UC_rnt_reservation_state" tagprefix="uc5" %>
<%@ Register Src="uc/UC_rnt_reservation_notes.ascx" TagName="UC_rnt_reservation_notes" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_agent.ascx" TagName="UC_rnt_reservation_agent" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Creazione della nuova prenotazione</title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-datepicker.css);
        html, body
        {
            background-color: #FFF;
        }
    </style>

    <script src="../js/tiny_mce/tiny_mce.js" type="text/javascript"></script>

    <script src="../js/tiny_mce/init.js" type="text/javascript"></script>

    <script src="../jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>

    <script src="../jquery/js/ui--core.min.js" type="text/javascript"></script>

    <script src="../jquery/js/ui-effects.min.js" type="text/javascript"></script>

    <script src="../jquery/js/ui-datepicker.min.js" type="text/javascript"></script>

    <script src="../jquery/datepicker-lang/jquery.ui.datepicker-it.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <uc1:UC_loader ID="UC_loader1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <asp:HiddenField ID="HF_IdRequest" runat="server" />
            <asp:HiddenField ID="HF_dtDate" runat="server" />
            <asp:HiddenField ID="HF_dtPart" runat="server" />
            <asp:HiddenField ID="HF_dtStart" runat="server" />
            <asp:HiddenField ID="HF_dtEnd" runat="server" />
            <asp:HiddenField ID="HF_dtCount" runat="server" Value="0" />
            <asp:HiddenField ID="HF_saveOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_priceOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_periodOK" runat="server" Value="0" />
            <asp:HiddenField ID="HF_avvOK" runat="server" Value="0" />
            
            <div id="main">
                <span class="titlight">Gestione Prenotazioni e Disponibilità delle Strutture</span>
                <div class="mainline">
                    <div class="prices">
                        <uc3:UC_rnt_reservation_estate ID="UC_estate" runat="server" />
                        <uc5:UC_rnt_reservation_state ID="UC_state" runat="server" />
                        <uc4:UC_rnt_reservation_dt_pers ID="UC_dt_pers" runat="server" />
                        <uc2:UC_rnt_reservation_notes ID="UC_notes" runat="server" />
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>
                                Prezzi</h3>
                            <div class="price_div">
                                <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 110px;">
                                            Commissione
                                        </td>
                                        <td align="right">
                                            <%=currOutPrice.pr_part_commission_total + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Agency Fee
                                        </td>
                                        <td align="right">
                                            <%=currOutPrice.pr_part_agency_fee + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Totale Acconto
                                        </td>
                                        <td align="right">
                                            <%=currOutPrice.pr_part_payment_total + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Saldo all'Arrivo
                                        </td>
                                        <td align="right">
                                            <%=currOutPrice.pr_part_owner + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <%if (currOutPrice.srsPrice > 0)
                                          {%>
                                    <tr>
                                        <td valign="middle" align="right">
                                            Welcome Service
                                        </td>
                                        <td valign="middle" align="right">
                                            <%=currOutPrice.srsPrice + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <%} %>
                                    <%if (currOutPrice.ecoPrice > 0)
                                          {%>
                                    <tr>
                                        <td valign="middle" align="right">
                                            Cleaning Service X<%= currOutPrice.ecoCount%>
                                        </td>
                                        <td valign="middle" align="right">
                                            <%=currOutPrice.ecoPrice + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <%} %>
                                    <tr>
                                        <td>
                                            Totale Prenotazione
                                        </td>
                                        <td align="right">
                                            <%=currOutPrice.prTotal + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                </table>
                                <div style="float: right; width: 210px;">
                                    <asp:LinkButton ID="lnk_calculatePrice" runat="server" CssClass="btnCalcola" OnClick="lnk_calculatePrice_Click">
					                    <span>
						                    Ricalcola Prezzi
					                    </span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    <div class="nulla">
                    </div>
                        <uc2:UC_rnt_reservation_agent ID="ucAgent" runat="server" />
                        <uc2:UC_rnt_reservation_client ID="UC_client" runat="server" />
                    <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                        <h3>Salvataggio Dati</h3>
                        <div class="price_div" id="pnl_saveNO" runat="server" >
                            <table class="selPeriod" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_stateError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Selezionare lo stato!"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_clientError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Compilare i dati del cliente"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_estateError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Selezionare la struttura"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_priceError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Calcolare i prezzi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_periodError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Selezionare il periodo e ospiti"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_avvError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Appartamento non disponibile nel periodo!"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_priceError1" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Attenzione! MinStay in Altissima stagione è di 5 notti"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="pnl_notifyNew" runat="server" visible="false">
                                    <td>
                                        Invia Mail di richiesta del pagamento al cliente
                                        <br/>per confermare la Prenotazione
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_sendMailCreated" runat="server">
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="btnric" style="float: left; margin: 50px;" id="pnl_btnSave" runat="server">
                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                           </div>
                        </div>
                        <div class="price_div" id="pnl_saveOK" runat="server" visible="false">
                            <table class="selPeriod" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label1" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Tutti i dati sono stati salvati"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="btnric" style="float: left; margin: 50px;" id="Div2" runat="server">
                                 <a href="javascript:parent.refreshDates();"><span>chiudi</span></a>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
