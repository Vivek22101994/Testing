<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResRefund.ascx.cs" Inherits="ModRental.admin.modRental.uc.ucResRefund" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    function refreshDates() {
    }
    function INV_openSelection(IdPayment) {
        var _url = "/admin/inv_invoice_form.aspx?id=" + IdPayment;
        OpenShadowbox(_url);
    }
</script>
<input type="hidden" id="rnt_refundHistory_state" />

<script type="text/javascript">
    function rnt_toggle_refundHistory() {
        if ($("#rnt_refundHistory_state").val() == "1") {
            //$("#rnt_refundHistory_toggler").removeClass("opened");
            //$("#rnt_refundHistory_toggler").addClass("closed");
            $("#rnt_refundHistory_toggler").html("Aggiungi Rimborsi");
            $("#rnt_refundHistory_cont").slideUp();
            $("#rnt_refundHistory_state").val("0");
        }
        else {
            //$("#rnt_refundHistory_toggler").removeClass("closed");
            //$("#rnt_refundHistory_toggler").addClass("opened");
            $("#rnt_refundHistory_toggler").html("Annulla / Chiudi");
            $("#rnt_refundHistory_cont").slideDown();
            $("#rnt_refundHistory_state").val("1");
        }
    }
    $("#rnt_refundHistory_state").val("0");
</script>
<a id="rnt_refundHistory_toggler" class="changeapt" href="javascript:rnt_toggle_refundHistory()">Aggiungi Rimborsi</a>
<div class="nulla">
</div>

<asp:HiddenField ID="HF_id" runat="server" Value="0" />
<asp:HiddenField ID="HF_reload" runat="server" Value="0" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table id="rnt_refundHistory_cont" style="display: none;">
            <tr>
                <td colspan="2">
                    <h3>Aggiungi rimborso</h3>
                </td>
            </tr>
            <tr>
                <td>Importo</td>
                <td>
                    <telerik:RadNumericTextBox ID="ntxt_prRefundTotal" runat="server" Width="100" Style="text-align: right;">
                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                    </telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>Data</td>
                <td>
                    <telerik:RadDateTimePicker ID="rdtp_prRefundDate" runat="server">
                        <DateInput DateFormat="dd/MM/yyyy HH:mm">
                        </DateInput>
                    </telerik:RadDateTimePicker>
                </td>
            </tr>
            <tr>
                <td>Mod. Pagamento</td>
                <td>
                    <asp:DropDownList ID="drp_prRefundPayMode" runat="server" Style="font-size: 10px;">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="lnk_saveRefund" runat="server" CssClass="changeapt" OnClick="lnk_saveRefund_Click">Aggiungi</asp:LinkButton>

                </td>
            </tr>
        </table>
        <div class="nulla">
        </div>
        <asp:ListView runat="server" ID="LV_payment" DataSourceID="LDS_payment" OnItemCommand="LV_payment_ItemCommand" OnSelectedIndexChanging="LV_payment_SelectedIndexChanging" OnItemDataBound="LV_payment_ItemDataBound">
            <ItemTemplate>
                <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
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
                            <%# "&euro;&nbsp;" + (Eval("pr_total").objToDecimal() - Eval("pr_noInvoice").objToDecimal()).ToString("N2")%></span>
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
                        <span>
                            <%# AdminUtilities.usr_adminName((int)Eval("state_pid_user"), "")%></span>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="deletepayment" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');">Elimina</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
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
                            <%# "&euro;&nbsp;" + (Eval("pr_total").objToDecimal() - Eval("pr_noInvoice").objToDecimal()).ToString("N2")%></span>
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
                        <span>
                            <%# AdminUtilities.usr_adminName((int)Eval("state_pid_user"), "")%></span>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="deletepayment" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');">Elimina</asp:LinkButton>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                <h3>Nessun Rimborso Registrato</h3>
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
                        <th id="Th6" runat="server" style="width: 100px;">
                            Da Fatturare
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
                        <th id="Th5" runat="server" style="width: 130px;">
                            Utente
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <SelectedItemTemplate>
                <tr class="current">
                    <td>
                        <span>
                            <%# Eval("dtCreation")%></span>
                    </td>
                    <td>
                        <span>
                            <asp:DropDownList ID="drp_rnt_reservation_part" runat="server" Style="font-size: 10px;">
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td>
                        <span>
                            <%# "&euro;&nbsp;"+Eval("pr_total").objToDecimal().ToString("N2")%></span>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="ntxt_pr_invoice" runat="server" Width="100" Style="text-align: right;">
                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <span>
                            <asp:DropDownList ID="drp_is_complete" runat="server" Style="font-size: 10px;">
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td>
                        <span>
                            <telerik:RadDateTimePicker ID="rdtp_pay_date" runat="server">
                                <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                </DateInput>
                            </telerik:RadDateTimePicker>
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:DropDownList ID="drp_pay_mode" runat="server" Style="font-size: 10px;">
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td>
                        <span>
                            <%# (Eval("is_complete") + "" == "1") ? AdminUtilities.usr_adminName((int)Eval("state_pid_user"), "") : UserAuthentication.CurrentUserName%></span>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_is_complete" Text='<%# Eval("is_complete") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_pay_mode" Text='<%# Eval("pay_mode") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_rnt_reservation_part" Text='<%# Eval("rnt_reservation_part") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_pr_noInvoice" Text='<%# Eval("pr_noInvoice") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_pr_total" Text='<%# Eval("pr_total") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_pay_date" Text='<%# (Eval("is_complete") + "" == "1" && ((DateTime?)Eval("pay_date")) != null) ? ((DateTime)Eval("pay_date")).JSCal_dateTimeToString() : ""%>' Visible="false"></asp:Label>

                        <asp:LinkButton ID="lnk_save" runat="server" CommandName="salva">salva</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="annulla">annulla</asp:LinkButton>
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:LinqDataSource ID="LDS_payment" runat="server" ContextTypeName="RentalInRome.data.magaInvoice_DataContext" TableName="INV_TBL_PAYMENT" Where="rnt_pid_reservation == @rnt_pid_reservation && pr_total > 0 && direction == 0" OrderBy="dtCreation desc">
            <WhereParameters>
                <asp:ControlParameter ControlID="HF_id" Name="rnt_pid_reservation" PropertyName="Value" Type="Int64" />
            </WhereParameters>
        </asp:LinqDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
