<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/mobile/masterPage.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="RentalInRome.reservationarea.mobile.payment" %>

<%@ Register Src="ucHeader.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_code" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_tf" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_commission_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_agency_fee" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_payment_total" runat="server" Value="0" />
    <asp:HiddenField ID="HF_pr_part_owner" runat="server" Value="0" />
    <asp:HiddenField ID="HF_inv_pay_id" runat="server" Visible="false" Value="0" />
    <asp:HiddenField ID="HF_pr_part_is_payed" runat="server" Visible="false" Value="0" />
    <uc1:ucHeader ID="ucHeader" runat="server" />
    <div data-role="content" class="paymentdata">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
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
                                            <a data-ajax="false" href="<%= CurrentAppSettings.HOST_SSL %>/util_paypal_redirect.aspx?mobile=true&type=payment&code=<%# Eval("code") %>&lang=<%= CurrentLang.ID%>" class="costo pagared">
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
                                                    <span class="pagaora_ris">
                                                        <span>
                                                            <%=CurrentSource.getSysLangValue("lblPayNow").htmlNoBreakSpace()%>
                                                        </span>
                                                    </span>
                                                    <span class="box_ris_pagaqui">
                                                        <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                                                    </span>
                                                    <span class="nulla"></span>
                                                </span>
                                                <span class="nulla"></span>
                                            </a>
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
                                            <a data-ajax="false" href="<%= CurrentAppSettings.HOST_SSL %>/util_paypal_redirect.aspx?mobile=true&type=payment&code=<%# Eval("code") %>&lang=<%= CurrentLang.ID%>" class="costo pagared">
                                                <span class="t_pagam">Main Payment</span>
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
                                                    <span class="pagaora_ris">
                                                        <span>
                                                            <%=CurrentSource.getSysLangValue("lblPayNow").htmlNoBreakSpace()%>
                                                        </span>
                                                    </span>
                                                    <span class="box_ris_pagaqui">
                                                        <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                                                    </span>
                                                    <span class="nulla"></span>
                                                </span>
                                                <span class="nulla"></span>
                                            </a>
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
                    <a data-ajax="false" href="<%= CurrentAppSettings.HOST_SSL %>/util_paypal_redirect.aspx?mobile=true&type=payment&code=<%= HF_fullPaymentCode.Value %>&lang=<%= CurrentLang.ID%>" class="costo pagared" style="width: 280px;">
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
                            <span class="pagaora_ris">
                                <span>
                                    <%=CurrentSource.getSysLangValue("lblPayNow").htmlNoBreakSpace()%>
                                </span>
                            </span>
                            <span class="box_ris_pagaqui">
                                <%=CurrentSource.getSysLangValue("lblClickHereToCompletePayment")%>
                            </span>
                            <span class="nulla"></span>
                        </span>
                        <span class="nulla"></span>
                    </a>
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
</asp:Content>
