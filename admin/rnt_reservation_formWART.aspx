<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_reservation_formWART.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_formWART" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_rnt_reservation_state.ascx" TagName="UC_rnt_reservation_state" TagPrefix="uc5" %>
<%@ Register Src="uc/UC_rnt_reservation_notes.ascx" TagName="UC_rnt_reservation_notes" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_rnt_reservation_dt_pers.ascx" TagName="UC_rnt_reservation_dt_pers" TagPrefix="uc4" %>
<%@ Register Src="uc/UC_rnt_reservation_inout.ascx" TagName="UC_rnt_reservation_inout" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
            <asp:HiddenField ID="HF_IdEstate" runat="server" />
            <asp:HiddenField ID="HF_dtStart" runat="server" />
            <asp:HiddenField ID="HF_dtEnd" runat="server" />
            <div id="main">
                <span class="titlight">Gestione Prenotazioni e Disponibilità di WART</span>
                <div class="mainline">
                    <div class="prices">
                        <uc5:UC_rnt_reservation_state ID="UC_state" runat="server" />
                        <uc4:UC_rnt_reservation_dt_pers ID="UC_dt_pers" runat="server" />
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>
                                Struttura: <%= CurrentSource.rntEstate_code(HF_IdEstate.Value.ToInt32(),"-errore-") %></h3>
                            <div class="price_div">
                                <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 110px;">
                                            Commissione
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txt_pr_part_commission_tf" runat="server"></asp:TextBox>&nbsp;&euro;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_pr_part_commission_tf" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt_pr_part_commission_tf" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Agency Fee
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txt_pr_part_agency_fee" runat="server"></asp:TextBox>&nbsp;&euro;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_pr_part_agency_fee" ErrorMessage="<br/>//obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_pr_part_agency_fee" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="price" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Totale Acconto
                                        </td>
                                        <td align="right">
                                            <%=(txt_pr_part_commission_tf.Text.ToDecimal() + txt_pr_part_agency_fee.Text.ToDecimal()) + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Saldo all'Arrivo
                                        </td>
                                        <td align="right">
                                            <%=(txt_pr_total.Text.ToDecimal() - txt_pr_part_agency_fee.Text.ToDecimal() - txt_pr_part_commission_tf.Text.ToDecimal()) + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Totale Prenotazione
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txt_pr_total" runat="server"></asp:TextBox>&nbsp;&euro;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_pr_total" ErrorMessage="<br/>//obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_pr_total" ErrorMessage="<br/>//non valido" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="price" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="btnric" style="float: left; margin: 50px;" id="pnl_btnSave" runat="server">
                                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" ValidationGroup="price"><span>Salva Prezzi</span></asp:LinkButton>
                                            </div>
                                            <div class="btnric" style="float: left; margin: 50px;">
                                                <a href="javascript:parent.refreshDates();">
                                                    <span>chiudi</span></a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <a id="rnt_paymentHistory_toggler" class="changeapt" href="javascript:rnt_toggle_paymentHistory()">Visualizza Storico Pagamenti</a>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="price_div" id="rnt_paymentHistory_cont" style="display: none;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:ListView runat="server" ID="LV_payment" DataSourceID="LDS_payment" OnItemCommand="LV_payment_ItemCommand" OnSelectedIndexChanging="LV_payment_SelectedIndexChanging" OnItemDataBound="LV_payment_ItemDataBound">
                                        <ItemTemplate>
                                            <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                <td>
                                                    <span>
                                                        <%# Eval("dtCreation")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%#  rntUtils.rntReservation_paymentPartTitle("" + Eval("rnt_reservation_part"))%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# (Eval("is_complete") + "" == "1") ? "SI" : "NO"%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# (Eval("is_complete") + "" == "1" && ((DateTime?)Eval("pay_date")) != null) ? Eval("pay_date") : "- - -"%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%#  invUtils.invPayment_modeTitle("" + Eval("pay_mode"), "- - -")%></span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select">completa</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" id="tr_normal" runat="server" style="">
                                                <td>
                                                    <span>
                                                        <%# Eval("dtCreation")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%#  rntUtils.rntReservation_paymentPartTitle("" + Eval("rnt_reservation_part"))%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# (Eval("is_complete") + "" == "1") ? "SI" : "NO"%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# (Eval("is_complete") + "" == "1" && ((DateTime?)Eval("pay_date")) != null) ? Eval("pay_date") : "- - -"%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%#  invUtils.invPayment_modeTitle("" + Eval("pay_mode"), "- - -")%></span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select">completa</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            Nessun Pagamento Registrato
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                                <tr id="Tr5" runat="server" style="">
                                                    <th id="Th1" runat="server" style="width: 120px;">
                                                        Data Ora Creazione
                                                    </th>
                                                    <th id="Th8" runat="server" style="width: 100px;">
                                                        Causale
                                                    </th>
                                                    <th id="Th11" runat="server" style="width: 100px;">
                                                        Importo
                                                    </th>
                                                    <th id="Th2" runat="server">
                                                        Pagato?
                                                    </th>
                                                    <th id="Th4" runat="server" style="width: 120px;">
                                                        Data Ora Pagamento
                                                    </th>
                                                    <th id="Th3" runat="server" style="width: 100px;">
                                                        Mod. Pagamento
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <SelectedItemTemplate>
                                            <tr class="current" id="tr_selected" runat="server" style="cursor: pointer;">
                                                <td>
                                                    <span>
                                                        <%# Eval("dtCreation")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%#  rntUtils.rntReservation_paymentPartTitle("" + Eval("rnt_reservation_part"))%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# "&euro;&nbsp;"+Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <asp:DropDownList ID="drp_is_complete" runat="server" Style="font-size: 10px;">
                                                        </asp:DropDownList>
                                                    </span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <%# DateTime.Now %>
                                                    </span>
                                                </td>
                                                <td>
                                                    <span>
                                                        <asp:DropDownList ID="drp_pay_mode" runat="server" Style="font-size: 10px;">
                                                        </asp:DropDownList>
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                                                    <asp:LinkButton ID="lnk_save" runat="server" CommandName="salva">salva</asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="annulla">annulla</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </SelectedItemTemplate>
                                    </asp:ListView>
                                    <asp:LinqDataSource ID="LDS_payment" runat="server" ContextTypeName="RentalInRome.data.magaInvoice_DataContext" TableName="INV_TBL_PAYMENT" Where="rnt_pid_reservation == @rnt_pid_reservation" OrderBy="dtCreation desc">
                                        <WhereParameters>
                                            <asp:ControlParameter ControlID="HF_id" Name="rnt_pid_reservation" PropertyName="Value" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="nulla">
                        </div>
                        <uc2:UC_rnt_reservation_notes ID="UC_notes" runat="server" />
                        <uc2:UC_rnt_reservation_inout ID="UC_inout" runat="server" />
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="rnt_paymentHistory_state" />
    <script type="text/javascript">
        function rnt_toggle_paymentHistory() {
            if ($("#rnt_paymentHistory_state").val() == "1") {
                //$("#rnt_paymentHistory_toggler").removeClass("opened");
                //$("#rnt_paymentHistory_toggler").addClass("closed");
                $("#rnt_paymentHistory_toggler").html("Visualizza Storico Pagamenti");
                $("#rnt_paymentHistory_cont").slideUp();
                $("#rnt_paymentHistory_state").val("0");
            }
            else {
                //$("#rnt_paymentHistory_toggler").removeClass("closed");
                //$("#rnt_paymentHistory_toggler").addClass("opened");
                $("#rnt_paymentHistory_toggler").html("Nascondi Storico Pagamenti");
                $("#rnt_paymentHistory_cont").slideDown();
                $("#rnt_paymentHistory_state").val("1");
            }
        }
        $("#rnt_paymentHistory_state").val("0");
    </script>
    </form>
</body>
</html>
