<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_payment.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_payment" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:HiddenField ID="HF_id" runat="server" Value="0" />
<asp:HiddenField ID="HF_reload" runat="server" Value="0" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
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
                            <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%>
                        </span>
                    </td>
                    <td>
                        <span>
                            <%# "&euro;&nbsp;" + (Eval("pr_total").objToDecimal() - Eval("pr_noInvoice").objToDecimal()).ToString("N2")%></span>
                    </td>
                    <td>
                        <span>
                            <%# "&euro;&nbsp;" + Eval("chargeFee").objToDecimal().ToString("N2")%></span>
                    </td>
                    <td>
                        <span>
                            <%# Eval("chargeFeeInvoice") + "" == "1" ? "SI" : "NO"%></span>
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
                        <asp:LinkButton ID="lnk_edit" runat="server" CommandName="select">completa</asp:LinkButton>
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
                            <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2") %></span>
                    </td>
                    <td>
                        <span>
                            <%# "&euro;&nbsp;" + (Eval("pr_total").objToDecimal() - Eval("pr_noInvoice").objToDecimal()).ToString("N2")%></span>
                    </td>
                    <td>
                        <span>
                            <%# "&euro;&nbsp;" + Eval("chargeFee").objToDecimal().ToString("N2")%></span>
                    </td>
                    <td>
                        <span>
                            <%# Eval("chargeFeeInvoice") + "" == "1" ? "SI" : "NO"%></span>
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
                        <th id="Th1" runat="server" style="width: 120px;">Data Ora Creazione
                        </th>
                        <th id="Th8" runat="server" style="width: 100px;">Causale
                        </th>
                        <th id="Th11" runat="server" style="width: 100px;">Importo
                        </th>
                        <th id="Th6" runat="server" style="width: 100px;">Da Fatturare
                        </th>
                        <th id="Th7" runat="server" style="width: 100px;">Charge fee
                        </th>
                        <th id="Th9" runat="server" style="width: 100px;">Fee in Fattura?
                        </th>
                        <th id="Th2" runat="server">Pagato?
                        </th>
                        <th id="Th4" runat="server" style="width: 120px;">Data Ora Pagamento
                        </th>
                        <th id="Th3" runat="server" style="width: 100px;">Mod. Pagamento
                        </th>
                        <th id="Th5" runat="server" style="width: 130px;">Utente
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
                            <%# rntUtils.rntReservation_paymentPartTitle("" + Eval("rnt_reservation_part"))%></span>
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
                        <telerik:RadNumericTextBox ID="ntxt_chargeFeePerc" runat="server" Width="50" Type="Percent" Style="text-align: right;">
                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <span>
                            <asp:DropDownList ID="drp_chargeFeeInvoice" runat="server" Style="font-size: 10px;">
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td>
                        <span>
                            <asp:DropDownList ID="drp_is_complete" runat="server" Style="font-size: 10px;">
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td>
                        <span>
                            <%# (Eval("is_complete") + "" == "1" && ((DateTime?)Eval("pay_date")) != null) ? Eval("pay_date") : DateTime.Now%></span>
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
                        <asp:Label runat="server" ID="lbl_pr_noInvoice" Text='<%# Eval("pr_noInvoice") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_pr_total" Text='<%# Eval("pr_total") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_chargeFeePerc" Text='<%# Eval("chargeFeePerc") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lbl_chargeFeeInvoice" Text='<%# Eval("chargeFeeInvoice") %>' Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnk_save" runat="server" CommandName="salva">salva</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_cancel" runat="server" CommandName="annulla">annulla</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="del" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');">elimina</asp:LinkButton>
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:LinqDataSource ID="LDS_payment" runat="server" ContextTypeName="RentalInRome.data.magaInvoice_DataContext" TableName="INV_TBL_PAYMENT" Where="rnt_pid_reservation == @rnt_pid_reservation && direction == 1" OrderBy="dtCreation desc">
            <WhereParameters>
                <asp:ControlParameter ControlID="HF_id" Name="rnt_pid_reservation" PropertyName="Value" Type="Int64" />
            </WhereParameters>
        </asp:LinqDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
