<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="inv_invoice_list.aspx.cs" Inherits="RentalInRome.admin.inv_invoice_list" %>

<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function refreshDates() {
            //window.location = "rnt_request_details.aspx?id=<%=HF_id.Value %>";
        }
        function INV_openSelection(IdPayment) {
            var _url = "inv_invoice_form.aspx?id=" + IdPayment;
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
        }
        function INV_openNew() {
            var _url = "inv_invoice_formNew.aspx";
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
            return false;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_is_filtered" runat="server" />
            <asp:HiddenField ID="HF_url_filter" runat="server" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco Fatture</h1>
                        <div class="bottom_agg">
                            <a href="#" onclick="return INV_openNew()">
                                <span>+ Nuova fattura</span></a>
                        </div>

                        <div class="bottom_agg">
                            <a href="/admin/inv_digital_invoice_error.aspx" target="_blank">
                                <span>Invoice Errors</span></a>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                                <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 180px;">
                                                            <label>
                                                                Numero Fattura:</label>
                                                            <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla">
                                                            </div>
                                                            <label>Codice Prenotazione:</label>
                                                            <asp:TextBox ID="txt_rnt_reservation_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla">
                                                            </div>
                                                            <label>l'intenstario della fattura:</label>
                                                            <asp:TextBox ID="txt_cl_name_full" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data Creazione:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtCreation_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtCreation_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtCreation_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtCreation_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtCreation_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtCreation_to" runat="server" />
                                                            <div class="nulla">
                                                            </div>
                                                            <label>
                                                                Pagamenti con fatturazione cambiata:<br />
                                                                ( ATTENZIONE: max differenza di date 2 mesi)</label>
                                                            <asp:DropDownList ID="drp_has_noInvoice" runat="server">
                                                                <asp:ListItem Value="-1" Text="- tutti -"></asp:ListItem>
                                                                <asp:ListItem Value="0" Text="NON cambiati"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="cambiati"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data Fattura:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_inv_dtInvoice_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_inv_dtInvoice_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_inv_dtInvoice_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_inv_dtInvoice_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                            <asp:HiddenField ID="HF_inv_dtInvoice_from" runat="server" />
                                                            <asp:HiddenField ID="HF_inv_dtInvoice_to" runat="server" />
                                                            <div class="nulla">
                                                            </div>
                                                            <label>Tipo di Pagamento:  </label>
                                                            <asp:DropDownList ID="drp_payment_type" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="lnkFilter" runat="server" CssClass="ricercaris" OnClick="lnkFilter_Click"> <span>Filtra Risultati</span> </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>

                                    <asp:LinkButton ID="lnk_import_excel" runat="server" OnClick="lnk_import_excel_Click" Style="margin-left: 669px;"><img src="images/excel_icon1.png" height="24px"/></asp:LinkButton>
                                </div>
                            </div>
                            <div class="b">
                                <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="filt" id="pnl_stats" runat="server" visible="false">
                        <div class="t">
                            <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                        </div>
                        <div class="c">
                            <div class="filtro_cont">
                                <table border="0" cellpadding="0" cellspacing="0" id="tbl_new">
                                    <tr>
                                        <td>
                                            <span style="color: #FFFFFF; font-size: 14px;">Statistiche</span>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td style="height: 40px;">
                                                                    <label>
                                                                        Registrati</label>
                                                                    <asp:TextBox ID="txt_count" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 40px;">
                                                                    <label>
                                                                        Totale Importo &euro;</label>
                                                                    <asp:TextBox ID="txt_prTotal" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 40px;">
                                                                    <label>
                                                                        Media Importo &euro;</label>
                                                                    <asp:TextBox ID="txt_prTotalMedia" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td style="height: 40px;">
                                                                    <label>
                                                                        Non esportati per Comercialista (Iamotti)</label>
                                                                    <asp:TextBox ID="txt_countNotExported_1" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="pnl_newExport_1" runat="server">
                                                                <td style="height: 40px;">
                                                                    <table style="background-color: #FFF;">
                                                                        <tr>
                                                                            <td>
                                                                                <strong>Esporta Fatture Non esportati</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Nome Esportazione<br />
                                                                                <asp:TextBox ID="txt_newExportName_1" runat="server" Style="width: 200px;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnk_newExport_1" runat="server" OnClick="lnk_newExport_1_Click">Esporta</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td style="height: 40px;">
                                                                    <label>
                                                                        Non esportati per Co.Ge. (Nuovo tracciato)</label>
                                                                    <asp:TextBox ID="txt_countNotExported_2" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="pnl_newExport_2" runat="server">
                                                                <td style="height: 40px;">
                                                                    <table style="background-color: #FFF;">
                                                                        <tr>
                                                                            <td>
                                                                                <strong>Esporta Fatture Non esportati</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Nome Esportazione<br />
                                                                                <asp:TextBox ID="txt_newExportName_2" runat="server" Style="width: 200px;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnk_newExport_2" runat="server" OnClick="lnk_newExport_2_Click">Esporta</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="b">
                            <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                            <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <asp:CheckBox ID="chk_send_invoices_all" class="send_all" runat="server" Text="Check All - send invoice" onclick="checkAllSend();" />
                    <div class="bottom_agg">
                        <asp:LinkButton ID="lnk_send_invoices" runat="server" OnClick="lnk_send_invoices_Click">
                            <span><%=contUtils.getLabel("lbl_send_invoice_batch") %></span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                    <asp:CheckBox ID="chk_import_xml_all" CssClass="xml_all" runat="server" Text="Check All - import XML" onclick="checkAllXml();" />
                    <div class="bottom_agg">
                        <asp:LinkButton ID="lnk_import_xml" runat="server" OnClick="lnk_import_xml_Click">
                            <span><%=contUtils.getLabel("lbl_get_xml_invoice_batch") %></span></asp:LinkButton>
                    </div>
                    <div style="clear: both">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                                <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                                <div class="page">
                                    <asp:DataPager ID="DataPager2" runat="server" PagedControlID="LV" PageSize="50" style="border-right: medium none;">
                                        <Fields>
                                            <asp:NumericPagerField ButtonCount="20" />
                                        </Fields>
                                    </asp:DataPager>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <asp:ListView ID="LV" runat="server" OnPagePropertiesChanged="LV_PagePropertiesChanged" OnItemCommand="LV_ItemCommand" OnItemDataBound="LV_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>

                                                <asp:CheckBox ID="chk_sent_invoice" runat="server" class="send" />
                                            </td>
                                            <td>

                                                <asp:CheckBox ID="chk_import_xml" runat="server" CssClass="xml" />
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("is_exported_1") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("inv_dtInvoice")).formatCustom("#dd# #MM# #yy#",1,"")%></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("is_payed") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>
                                            <td>
                                                <span><a id="lnk_res_detail" runat="server" target="_blank">
                                                    <%# Eval("rnt_reservation_code")%></a></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#getPaymentType(Eval("inv_pid_payment").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:LinkButton ID="lnk_send_invoice" runat="server" CommandName="sendInvoice">Send Invoice</asp:LinkButton>
                                                    <asp:Label ID="lbl_num_send_invoice" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td style="display: none">
                                                <span>
                                                    <asp:LinkButton ID="lnk_send_credit_note" runat="server" CommandName="sendCreditNote">Send Credit Note</asp:LinkButton>
                                                    <asp:Label ID="lbl_num_send_credit_note" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <%# invUtils.getInvoiceNotificationsType(Convert.ToString(Eval("responseUniqueId"))) %>
                                            </td>
                                            <td>
                                                <%# invUtils.getInvoiceNotificationsMessage(Convert.ToString(Eval("responseUniqueId"))) %>
                                            </td>
                                            <%--<td>
                                                <span>
                                                    <asp:LinkButton ID="lnk_get_xml" runat="server" CommandName="getInvoice">Get Invoice Xml</asp:LinkButton>
                                                </span>
                                            </td>--%>
                                            <td><span><a id="lbl_res_area" runat="server" target="_blank">scheda</a></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <asp:CheckBox ID="chk_sent_invoice" runat="server" class="send" />
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:CheckBox ID="chk_import_xml" runat="server" class="xml" /></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("is_exported_1") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("inv_dtInvoice")).formatCustom("#dd# #MM# #yy#",1,"")%></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("is_payed") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>
                                            <td>
                                                <span><a id="lnk_res_detail" runat="server" target="_blank">
                                                    <%# Eval("rnt_reservation_code")%> </a></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#getPaymentType(Eval("inv_pid_payment").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:LinkButton ID="lnk_send_invoice" runat="server" CommandName="sendInvoice">Send Invoice</asp:LinkButton>
                                                    <asp:Label ID="lbl_num_send_invoice" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td style="display: none">
                                                <span>
                                                    <asp:LinkButton ID="lnk_send_credit_note" runat="server" CommandName="sendCreditNote">Send Credit Note</asp:LinkButton>
                                                    <asp:Label ID="lbl_num_send_credit_note" runat="server"></asp:Label>
                                                </span>
                                            </td>
                                            <td>
                                                <%# invUtils.getInvoiceNotificationsType(Convert.ToString(Eval("responseUniqueId"))) %>
                                            </td>
                                            <td>
                                                <%# invUtils.getInvoiceNotificationsMessage(Convert.ToString(Eval("responseUniqueId"))) %>
                                            </td>
                                            <%--<td>
                                                <span>
                                                    <asp:LinkButton ID="lnk_get_xml" runat="server" CommandName="getInvoice">Send Invoice</asp:LinkButton>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                </span>
                                            </td>--%>
                                            <td><span><a id="lbl_res_area" runat="server" target="_blank">scheda</a></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <th>Send</th>
                                                    <th>Import</th>
                                                    <th style="width: 40px;">Exp.?
                                                    </th>
                                                    <th style="width: 80px;">Numero Fattura
                                                    </th>
                                                    <th style="width: 120px;">Data Fattura
                                                    </th>
                                                    <th style="width: 80px; text-align: center;">Importo
                                                    </th>
                                                    <th style="width: 50px;">Saldata?
                                                    </th>
                                                    <th style="width: 80px;">Codice Pren
                                                    </th>
                                                    <th style="width: 120px;">Fatturato a
                                                    </th>
                                                    <th style="width: 120px;">Tipo Pagamento
                                                    </th>
                                                    <th>Invoice</th>
                                                    <th style="display: none">credit note</th>
                                                    <th>Inv Noti Type</th>
                                                    <th>Inv Noti Mess</th>
                                                    <th>Res.Area</th>
                                                    <th></th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                            </table>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>

                                <asp:ListView ID="LV_Excel" runat="server" OnPagePropertiesChanged="LV_Excel_PagePropertiesChanged" Style="display: none;">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <%-- <td>
                                                <span>
                                                    <%# (Eval("is_exported_1") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>--%>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("inv_dtInvoice")).formatCustom("#dd# #MM# #yy#",1,"")%></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                            </td>
                                            <%-- <td>
                                                <span>
                                                    <%# (Eval("is_payed") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>--%>
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_loc_country")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#getPaymentType(Eval("inv_pid_payment").objToInt32())%></span>
                                            </td>
                                            <%-- <td>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>--%>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <%-- <td>
                                                <span>
                                                    <%# (Eval("is_exported_1") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>--%>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("inv_dtInvoice")).formatCustom("#dd# #MM# #yy#",1,"")%></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2")%></span>
                                            </td>
                                            <%-- <td>
                                                <span>
                                                    <%# (Eval("is_payed") + "" == "1") ? "SI" : "NO"%></span>
                                            </td>--%>
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  Eval("cl_loc_country")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#getPaymentType(Eval("inv_pid_payment").objToInt32())%></span>
                                            </td>
                                            <%-- <td>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>--%>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="display: none;">
                                                <tr>
                                                    <%-- <th style="width: 40px;">Exp.?
                                                    </th>--%>

                                                    <th style="width: 80px;">Numero Fattura
                                                    </th>
                                                    <th style="width: 120px;">Data Fattura
                                                    </th>
                                                    <th style="width: 80px; text-align: center;">Importo
                                                    </th>
                                                    <%-- <th style="width: 50px;">Saldata?
                                                    </th>--%>
                                                    <th style="width: 80px;">Codice Pren
                                                    </th>
                                                    <th style="width: 120px;">Fatturato a
                                                    </th>
                                                     <th style="width: 120px;">Nationality
                                                    </th>
                                                    <th style="width: 120px;">Tipo Pagamento
                                                    </th>
                                                    <%-- <th></th>--%>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                            </table>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnk_import_excel" />
            <asp:PostBackTrigger ControlID="lnk_import_xml" />
        </Triggers>

    </asp:UpdatePanel>

    <script type="text/javascript">
        var cal_dtCreation_from;
        var cal_dtCreation_to;

        var cal_inv_dtInvoice_from;
        var cal_inv_dtInvoice_to;
        function setCal() {
            cal_dtCreation_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_from.ClientID %>", View: "#txt_dtCreation_from", Cleaner: "#del_dtCreation_from", changeMonth: true, changeYear: true });
            cal_dtCreation_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_to.ClientID %>", View: "#txt_dtCreation_to", Cleaner: "#del_dtCreation_to", changeMonth: true, changeYear: true });

            cal_inv_dtInvoice_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_from.ClientID %>", View: "#txt_inv_dtInvoice_from", Cleaner: "#del_inv_dtInvoice_from", changeMonth: true, changeYear: true });
            cal_inv_dtInvoice_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_to.ClientID %>", View: "#txt_inv_dtInvoice_to", Cleaner: "#del_inv_dtInvoice_to", changeMonth: true, changeYear: true });
        }
    </script>

    <script type="text/javascript">
        function checkAllSend() {
            $("span.send input[type='checkbox']").attr('checked', $("#<%=chk_send_invoices_all.ClientID%>").attr('checked'));
        }

        function checkAllXml() {
            $("span.xml input[type='checkbox']").attr('checked', $("#<%=chk_import_xml_all.ClientID%>").attr('checked'));
        }
    </script>
</asp:Content>
