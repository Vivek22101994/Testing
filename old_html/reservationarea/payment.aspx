<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="RentalInRome.reservationarea.payment" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<%@ Register Src="UC_payment_loader.ascx" TagName="UC_payment_loader" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Payment summary</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script src="/jquery/plugin/jquery.creditCardValidator.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_code" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Visible="false" Value="1" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_adult" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_child_over" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_num_child_min" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_tf" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_agency_fee" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_payment_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_owner" runat="server" Value="0" />
    <asp:HiddenField ID="HF_inv_pay_id" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_pr_part_is_payed" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_visa_isRequested" runat="server" Visible="false" Value="0" />
    <asp:Literal runat="server" ID="ltr_unique_id" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_cl_name_full" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_cl_name_honorific" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_est_code" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_est_address" Visible="false"></asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx">
            <asp:PlaceHolder runat="server" ID="PH_PaymentDetails">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                            <%=CurrentSource.getSysLangValue("lblPaymentSummary")%>
                        </h3>
                        <div class="nulla">
                        </div>
                        <ul class="ordini">
                            <li>
                                <div>
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td rowspan="3">
                                                <div class="dati_ord_sx">
                                                    <span class="n_ord">
                                                        <%=CurrentSource.getSysLangValue("pdf_Codice_di_prenotazione")%>:
                                                    <%= HF_code.Value%>
                                                    </span>
                                                    <div class="nulla">
                                                    </div>
                                                    <h3>
                                                        <%= ltr_est_code.Text%></h3>
                                                    <%=CurrentSource.getSysLangValue("lblDateFrom")%>: <strong>
                                                        <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                                                    <br />
                                                    <%=CurrentSource.getSysLangValue("lblDateTo")%>: <strong>
                                                        <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                                                    <br />
                                                    <strong>
                                                        <%= HF_num_adult.Value + " Adults + " + HF_num_child_over.Value + " Children + " + HF_num_child_min.Value + " Infants"%>
                                                    </strong>
                                                    <br />
                                                    <br />
                                                    <a style="display: none;" href="#">dettagli e costi</a>
                                                </div>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ListView runat="server" ID="LV_partPayment">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                                                                    <asp:PlaceHolder ID="PH_payed" runat="server">
                                                                        <div class="costo pagato">
                                                                            <span class="t_pagam">
                                                                                <%=CurrentSource.getSysLangValue("lblAnticipo")%></span>
                                                                            <span class="euro"><strong>
                                                                                <%# Eval("pr_total").objToDecimal().ToString("N2")%></strong>&euro;</span>
                                                                            <div class="stato">
                                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <img src="/images/css/reservation_paga_ok.png" alt="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <%=CurrentSource.getSysLangValue("lblPayOK")%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div class="nulla">
                                                                            </div>
                                                                        </div>
                                                                        <div class="nulla">
                                                                        </div>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="PH_toPay" runat="server">
                                                                        <span class="costo pagared">
                                                                            <span class="t_pagam">
                                                                                <%=CurrentSource.getSysLangValue("lblAnticipo")%></span>
                                                                            <span class="euro"><strong>
                                                                                <%# Eval("pr_total").objToDecimal().ToString("N2")%></strong>&euro;</span>
                                                                            <span class="stato" style="float: none;">
                                                                                <span style="width: 130px;">
                                                                                    <table style="margin-left: auto; margin-right: auto;">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img src="/images/css/reservation_paga_arrivo.png" alt="" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <%=CurrentSource.getSysLangValue("lblPayNO")%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                                <span class="cartered" style="float: none;">
                                                                                    <img alt="" src="/images/css/carte_small.png" width="130" height="41" style="margin-left: -6px;" />
                                                                                    <span class="nulla"></span>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                                <asp:LinkButton ID="lnkPayWithPaypal" CssClass="pagaora_ris" runat="server" OnClick="lnkPayWithPaypal_Click" CommandArgument='<%# Eval("id") %>'>
                                                                                <span>
                                                                                    <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                                                </span>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton ID="lnkPayWithBancaSella" CssClass="pagaora_ris cc" runat="server" OnClick="lnkPayWithBancaSella_Click" CommandArgument='<%# Eval("id") %>'>
                                                                                <span>
                                                                                    <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                                                </span>
                                                                                </asp:LinkButton>
                                                                                <span class="pagaora_ris" style="height: 0px; overflow: hidden; border: none; margin-bottom: 0;"><span>
                                                                                    <span class="box_ris_pagaqui">
                                                                                        <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                                                                                    </span></span>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                            </span>
                                                                            <span class="nulla"></span>
                                                                        </span>
                                                                        <div class="nulla">
                                                                        </div>
                                                                    </asp:PlaceHolder>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <div class="costo pagagray" id="pnl_partPaymentView" runat="server">
                                                                <span class="t_pagam">
                                                                    <%=CurrentSource.getSysLangValue("lblAnticipo")%></span>
                                                                <span class="euro"><strong>
                                                                    <%= HF_pr_part_payment_total.Value.ToDecimal().ToString("N2")%></strong>€</span>
                                                                <div class="stato" id="pnl_partPaymentKO" runat="server">
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img src="/images/css/reservation_paga_arrivo.png" alt="" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <%=CurrentSource.getSysLangValue("lblPayNO")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="nulla">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:ListView runat="server" ID="LV_partOwner">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                                                                    <asp:PlaceHolder ID="PH_payed" runat="server">
                                                                        <div class="costo pagato">
                                                                            <span class="t_pagam">Main Payment</span>
                                                                            <span class="euro"><strong><%# Eval("pr_total").objToDecimal().ToString("N2")%></strong>&euro;</span>
                                                                            <div class="stato">
                                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <img src="/images/css/reservation_paga_ok.png" alt="" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <%=CurrentSource.getSysLangValue("lblPayOK")%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div class="nulla">
                                                                            </div>
                                                                        </div>
                                                                        <div class="nulla">
                                                                        </div>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="PH_toPay" runat="server">
                                                                        <span class="costo pagared">
                                                                            <span class="t_pagam">Main Payment</span>
                                                                            <span class="euro"><strong><%# Eval("pr_total").objToDecimal().ToString("N2")%></strong>&euro;</span>
                                                                            <span class="stato" style="float: none;">
                                                                                <span style="width: 130px;">
                                                                                    <table style="margin-left: auto; margin-right: auto;">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <img src="/images/css/reservation_paga_arrivo.png" alt="" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <%=CurrentSource.getSysLangValue("lblPayNO")%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                                <span class="cartered" style="float: none;">
                                                                                    <img alt="" src="/images/css/carte_small.png" width="130" height="41" style="margin-left: -6px;" />
                                                                                    <span class="nulla"></span>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                                <asp:LinkButton ID="lnkPayWithPaypal" CssClass="pagaora_ris" runat="server" OnClick="lnkPayWithPaypal_Click" CommandArgument='<%# Eval("id") %>'>
                                                                                <span>
                                                                                    <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                                                </span>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton ID="lnkPayWithBancaSella" CssClass="pagaora_ris cc" runat="server" OnClick="lnkPayWithBancaSella_Click" CommandArgument='<%# Eval("id") %>'>
                                                                                <span>
                                                                                    <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                                                </span>
                                                                                </asp:LinkButton>
                                                                                <span class="pagaora_ris" style="height: 0px; overflow: hidden;"><span>
                                                                                    <span class="box_ris_pagaqui">
                                                                                        <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                                                                                    </span></span>
                                                                                </span>
                                                                                <span class="nulla"></span>
                                                                            </span>
                                                                            <span class="nulla"></span>
                                                                        </span>
                                                                        <div class="nulla">
                                                                        </div>
                                                                    </asp:PlaceHolder>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <div class="costo pagagray" id="pnl_ownerPaymentView" runat="server">
                                                                <span class="t_pagam">
                                                                    <%=CurrentSource.getSysLangValue("lblSaldoArrivo")%></span>
                                                                <span class="euro"><strong>
                                                                    <%= HF_pr_part_owner.Value.ToDecimal().ToString("N2") %></strong>€</span>
                                                                <div class="stato" id="pnl_ownerPaymentKO" runat="server">
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <img src="/images/css/reservation_paga_arrivo.png" alt="" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <%=CurrentSource.getSysLangValue("lblPayNO")%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="nulla">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="pnl_fullPaymentKO" runat="server">
                                            <td>
                                                <asp:HiddenField ID="HF_fullPaymentCode" runat="server" Value="0" />
                                                <span class="costo pagared" style="width: 280px;">
                                                    <span class="t_pagam">
                                                        <%=CurrentSource.getSysLangValue("lblTotalPrice")%></span>
                                                    <span class="euro"><strong>
                                                        <%= HF_pr_total.Value.ToDecimal().ToString("N2") %></strong>&euro;</span>
                                                    <span class="stato" style="float: none;">
                                                        <span style="width: 130px;">
                                                            <table style="margin-left: auto; margin-right: auto;">
                                                                <tr>
                                                                    <td>
                                                                        <img src="/images/css/reservation_paga_arrivo.png" alt="" />
                                                                    </td>
                                                                    <td>
                                                                        <%=CurrentSource.getSysLangValue("lblPayNO")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </span>
                                                        <span class="nulla"></span>
                                                        <span class="cartered" style="float: none;">
                                                            <img alt="" src="/images/css/carte_small.png" width="130" height="41" style="margin-left: -6px;" />
                                                            <span class="nulla"></span>
                                                        </span>
                                                        <span class="nulla"></span>
                                                        <asp:LinkButton ID="lnkPayWithPaypalFull" CssClass="pagaora_ris" runat="server" OnClick="lnkPayWithPaypal_Click" CommandArgument='full'>
                                                        <span>
                                                            <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                        </span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkPayWithBancaSellaFull" CssClass="pagaora_ris cc" runat="server" OnClick="lnkPayWithBancaSella_Click" CommandArgument='full'>
                                                        <span>
                                                            <%=CurrentSource.getSysLangValue("reqPayWith").htmlNoBreakSpace()%>:
                                                        </span>
                                                        </asp:LinkButton>
                                                        <span class="pagaora_ris" style="height: 0px; overflow: hidden;"><span>
                                                            <span class="box_ris_pagaqui">
                                                                <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                                                            </span></span>
                                                        </span>

                                                        <span class="nulla"></span>
                                                    </span>
                                                    <span class="nulla"></span>
                                                </span>
                                                <div class="nulla">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="pnl_fullPaymentOK" runat="server">
                                            <td>
                                                <div class="costo pagato" style="width: 280px;">
                                                    <span class="t_pagam">
                                                        <%=CurrentSource.getSysLangValue("lblTotalPrice")%></span>
                                                    <span class="euro"><strong>
                                                        <%= HF_pr_total.Value.ToDecimal().ToString("N2")%></strong>&euro;</span>
                                                    <div class="stato">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <img src="/images/css/reservation_paga_ok.png" alt="" />
                                                                            </td>
                                                                            <td>
                                                                                <%=CurrentSource.getSysLangValue("lblPayOK")%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="pnl_fullPaymentNO" runat="server">
                                            <td>
                                                <span class="pagatotal"><strong>
                                                    <%= CurrentSource.getSysLangValue("lblTotalPrice")%></strong>
                                                    <%= HF_pr_total.Value.ToDecimal().ToString("N2") %><span style="font-size: 17px;">&euro;</span>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <% if (HF_pr_deposit.Value.ToDecimal() != 0)
                                                   {%>
                                                <span class="pagatotal"><strong>
                                                    <%=CurrentSource.getSysLangValue("lblDamageDeposit") %></strong>
                                                    <%= HF_pr_deposit.Value.ToDecimal().ToString("N2")%><span style="font-size: 17px;">&euro;</span>
                                                </span>
                                                <%} %>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                            <li id="Li1" runat="server" visible="false">
                                <div style="background-color: #e2e2e2;">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <div class="dati_ord_sx">
                                                    <span class="n_ord" style="background-color: #666666;">Ordine n°: 65856</span>
                                                    <div class="nulla">
                                                    </div>
                                                    <h4>Limo Service Andata</h4>
                                                    <strong>15/05/2011</strong> ore <strong>14:50</strong>
                                                    <br />
                                                    <br />
                                                    <h4>Limo Service Ritorno</h4>
                                                    <strong>15/05/2011</strong> ore <strong>18:00</strong>
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div class="costo pagagray" style="float: right;">
                                                    <span class="t_pagam">SALDO</span>
                                                    <span class="euro"><strong>350</strong>€</span>
                                                    <div class="stato">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/reservation_paga_arrivo.png" />
                                                                            </td>
                                                                            <td>Pagato
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                            <li id="Li2" runat="server" visible="false">
                                <div style="background-color: #e2e2e2;">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td rowspan="2">
                                                <div class="dati_ord_sx">
                                                    <span class="n_ord" style="background-color: #666666;">Ordine n°: 65856</span>
                                                    <div class="nulla">
                                                    </div>
                                                    <h4>Limo Service Andata</h4>
                                                    <strong>15/05/2011</strong> ore <strong>14:50</strong>
                                                    <br />
                                                    <br />
                                                    <h4>Limo Service Ritorno</h4>
                                                    <strong>15/05/2011</strong> ore <strong>18:00</strong>
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div class="costo pagato">
                                                    <span class="t_pagam">ANTICIPO</span>
                                                    <span class="euro"><strong>350</strong>€</span>
                                                    <div class="stato">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/reservation_paga_ok.png" />
                                                                            </td>
                                                                            <td>Pagato
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                                <div class="costo pagagray">
                                                    <span class="t_pagam">SALDO</span>
                                                    <span class="euro"><strong>350</strong>€</span>
                                                    <div class="stato">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/reservation_paga_arrivo.png" />
                                                                            </td>
                                                                            <td>lblPayOK
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="pagatotal"><strong>Totale</strong> 900<span style="font-size: 17px;">€</span>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                            <li id="Li3" runat="server" visible="false">
                                <div style="background-color: #e2e2e2;">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <div class="dati_ord_sx">
                                                    <span class="n_ord" style="background-color: #666666;">Ordine n°: 65856</span>
                                                    <div class="nulla">
                                                    </div>
                                                    <h4>Limo Service Andata</h4>
                                                    <strong>15/05/2011</strong> ore <strong>14:50</strong>
                                                    <br />
                                                    <br />
                                                    <h4>Limo Service Ritorno</h4>
                                                    <strong>15/05/2011</strong> ore <strong>18:00</strong>
                                                </div>
                                            </td>
                                            <td align="right">
                                                <div class="costo pagared" style="float: right;">
                                                    <span class="t_pagam">SALDO</span>
                                                    <span class="euro"><strong>350</strong>€</span>
                                                    <div class="stato">
                                                        <a href="#">Paga Ora<br />
                                                            <img src="images/css/paypal_small.gif" /></a>
                                                        <a class="ico_tooltip" title="div_infobonifico">Bonifico Bancario</a>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>
                        <div class="nulla">
                        </div>
                        <asp:UpdateProgress ID="loading_cont" runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <uc2:UC_payment_loader ID="UC_payment_loader1" Visible="True" runat="server" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div class="nulla">
                        </div>
                        <asp:HiddenField ID="HF_paymentId" runat="server" />
                        <asp:HiddenField ID="HF_prToPay" runat="server" />
                        <asp:HiddenField ID="HF_prFee" runat="server" />
                        <asp:HiddenField ID="HF_prTotal" runat="server" />
                        <div id="pnlSendPaypal" class="box_client_booking" runat="server" visible="false" style="width: auto;">
                            <div id="pnlSendPaypalFees" runat="server" visible="false" class="line" style="border: none;">
                                <div class="left fees">
                                    <label class="desc">
                                        To pay: <%= HF_prToPay.Value%>&nbsp;&euro;<br />
                                        Paypal fee: <%= HF_prFee.Value%>&nbsp;%<br />
                                        Total charge: <%= HF_prTotal.Value%>&nbsp;&euro;<br />
                                    </label>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <a href="<%= (App.WLAgentId > 0) ? ("http://"+ Request.Url.Host) : CurrentAppSettings.HOST_SSL %>/util_paypal_redirect.aspx?type=payment&code=<%= HF_paymentId.Value %>&lang=<%= CurrentLang.ID%>" class="btn bonifico">
                                <span>
                                    <%=CurrentSource.getSysLangValue("reqPayWithPayPal").htmlNoBreakSpace()%>:
                                </span>
                            </a>
                        </div>
                        <div id="pnlSendBancaSella" class="box_client_booking" runat="server" visible="false" style="width: auto;">
                            <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
                                <h3 id="errorMsgLbl">
                                    <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                                <p id="errorMsg">
                                    <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                                </p>
                                <div class="nulla">
                                </div>
                            </div>
                            <div id="pnlSendBancaSellaFees" runat="server" visible="false" class="line" style="border: none;">
                                <div class="left fees">
                                    <label class="desc">
                                        To pay: <%= HF_prToPay.Value%>&nbsp;&euro;<br />
                                        CC fee: <%= HF_prFee.Value%>&nbsp;%<br />
                                        Total charge: <%= HF_prTotal.Value%>&nbsp;&euro;<br />
                                    </label>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="line" style="border: none;">
                                <div class="left">
                                    <label class="desc">
                                        <%= contUtils.getLabel("lblHolder")%>
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txt_cc_titolareCarta" runat="server" MaxLength="300"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="left">
                                    <label class="desc">
                                        <%= contUtils.getLabel("lblCreditCardNumber")%>
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txt_cc_numeroCarta" runat="server" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="left">
                                    <label class="desc">
                                        <%= contUtils.getLabel("lblCardType")%>
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_cc_tipoCarta" runat="server">
                                            <asp:ListItem Value="" Text="- - -"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Visa"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="MasterCard"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="line" style="border: none;">
                                <div class="left">
                                    <label class="desc">
                                        <%= contUtils.getLabel("lblDueDate")%>
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_cc_meseScadenza" runat="server" Style="width: 85px;">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="drp_cc_annoScadenza" runat="server" Style="width: 85px;">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="left">
                                    <label class="desc">
                                        CVV
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txt_cc_cvv" runat="server" MaxLength="4" Style="width: 40px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <asp:LinkButton ID="lnk_payBancaSella" CssClass="btn bonifico" runat="server" OnClick="lnk_payBancaSella_Click" OnClientClick="return bookingForm_validateForm()"><span><%=CurrentSource.getSysLangValue("lblSubmit")%></span></asp:LinkButton>

                            <div class="line" id="pnlVerifiedByVisa" runat="server" visible="false" style="border: none;">
                                <asp:HiddenField ID="HF_uidVerifiedByVisa" runat="server" />
                                <div class="left" style="position: relative;">
                                    <img src="/images/css/verify-by-visa.png" alt="Verify By Visa" style="position: absolute; top: 0; left: -245px;" />
                                    <label class="desc verifiedByVisa">
                                        <%= contUtils.getLabel("lblVerifiedByVisaDesc")%>
                                    </label>
                                    <div class="nulla">
                                    </div>
                                    <iframe width="300px" height="100px" border="0" style="width: 300px; height: 100px; border: none;" src="/util_bancasella3d_redir.aspx?uid=<%= HF_uidVerifiedByVisa.Value%>"></iframe>
                                </div>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <a class="transferImg" href="/reservationarea/arrivaldeparture.aspx">
                            <span class="transferImgTxt">
                                <%= CurrentSource.getSysLangValue("lblTransferAreaRis")%>
                            </span>
                            <span class="forOnly">
                                <span><%= CurrentSource.getSysLangValue("lblForOnly")%></span>
                                <strong>50<span>€</span></strong>
                            </span>
                        </a>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <script type="text/javascript">

                    var bookingForm_validateForm_firstTime = true;
                    function bookingForm_validateForm() {
                        var callBack = null;
                        if (bookingForm_validateForm_firstTime) {
                            callBack = function () { return bookingForm_validateForm(); };
                            bookingForm_validateForm_firstTime = false;
                        }
                        var isValid = true;
                        FORM_hideErrorToolTip();

                        if (!FORM_validate_requiredField("<%= drp_cc_annoScadenza.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        if (!FORM_validate_requiredField("<%= drp_cc_meseScadenza.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        if ($('#<%= drp_cc_annoScadenza.ClientID%>').val() != "" && $('#<%= drp_cc_meseScadenza.ClientID%>').val() != "") {
                            $("#<%= drp_cc_meseScadenza.ClientID%>").removeClass("errorInput");
                            var ccExpYear = parseInt($('#<%= drp_cc_annoScadenza.ClientID%>').val(), 10);
                            var ccExpMonth = parseInt($('#<%= drp_cc_meseScadenza.ClientID%>').val(), 10);
                            var expDate = new Date(ccExpYear, ccExpMonth);
                            var today = new Date();
                            //alert(ccExpYear+"/"+ ccExpMonth);
                            if (expDate < today) {
                                FORM_showErrorToolTip("<%= contUtils.getLabel("lblCcExpired")%>", "<%= drp_cc_meseScadenza.ClientID%>");
                                $("#<%= drp_cc_meseScadenza.ClientID%>").addClass("errorInput");
                                isValid = false;
                            }
                        }
                        if (!FORM_validate_requiredField("<%= txt_cc_cvv.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        if (!FORM_validate_requiredField("<%= drp_cc_tipoCarta.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        if (!FORM_validate_requiredField("<%= txt_cc_numeroCarta.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        if ($('#<%= drp_cc_tipoCarta.ClientID%>').val() != "" && $('#<%= txt_cc_numeroCarta.ClientID%>').val() != "") {
                            $("#<%= txt_cc_numeroCarta.ClientID%>").removeClass("errorInput");
                            var length_valid;
                            var luhn_valid;
                            var accept_cc = "";
                            if ($('#<%= drp_cc_tipoCarta.ClientID%>').val() == "1") accept_cc = "visa";
                            if ($('#<%= drp_cc_tipoCarta.ClientID%>').val() == "2") accept_cc = "mastercard";
                            if ($('#<%= drp_cc_tipoCarta.ClientID%>').val() == "5") accept_cc = "amex";
                            $('#<%= txt_cc_numeroCarta.ClientID%>').validateCreditCard(function (result) { length_valid = result.length_valid; luhn_valid = result.luhn_valid; }, { accept: [accept_cc] })
                            if (!length_valid || !luhn_valid) {
                                FORM_showErrorToolTip("<%= contUtils.getLabel("lblCcNotValid")%>", "<%= txt_cc_numeroCarta.ClientID%>");
                                $("#<%= txt_cc_numeroCarta.ClientID%>").addClass("errorInput");
                                isValid = false;
                            }
                        }
                        if (!FORM_validate_requiredField("<%= txt_cc_titolareCarta.ClientID%>", "", "", "", callBack))
                            isValid = false;
                        return isValid;
                    }
                </script>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PH_AgnecyResNoPayment" runat="server" Visible="false">
                <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                    <%=CurrentSource.getSysLangValue("lblPaymentSummary")%>
                </h3>
                <div class="nulla">
                </div>
                <ul class="ordini">
                    <li>
                        <div>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td rowspan="3">
                                        <div class="dati_ord_sx">
                                            <span class="n_ord">
                                                <%=CurrentSource.getSysLangValue("pdf_Codice_di_prenotazione")%>:
                                                    <%= HF_code.Value%>
                                            </span>
                                            <div class="nulla">
                                            </div>
                                            <h3>
                                                <%= ltr_est_code.Text%></h3>
                                            <%=CurrentSource.getSysLangValue("lblDateFrom")%>: <strong>
                                                <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                                            <br />
                                            <%=CurrentSource.getSysLangValue("lblDateTo")%>: <strong>
                                                <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%></strong>
                                            <br />
                                            <strong>
                                                <%= HF_num_adult.Value + " Adults + " + HF_num_child_over.Value + " Children + " + HF_num_child_min.Value + " Infants"%>
                                            </strong>
                                            <br />
                                            <br />
                                            <a style="display: none;" href="#">dettagli e costi</a>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="pagatotal"><strong>
                                            <%= CurrentSource.getSysLangValue("lblTotalPrice")%></strong>
                                            <%= HF_pr_total.Value.ToDecimal().ToString("N2") %><span style="font-size: 17px;">&euro;</span>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <% if (HF_pr_deposit.Value.ToDecimal() != 0)
                                           {%>
                                        <span class="pagatotal"><strong>
                                            <%=CurrentSource.getSysLangValue("lblDamageDeposit") %></strong>
                                            <%= HF_pr_deposit.Value.ToDecimal().ToString("N2")%><span style="font-size: 17px;">&euro;</span>
                                        </span>
                                        <%} %>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
