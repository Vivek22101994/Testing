<%@ Page Title="" Language="C#" MasterPageFile="~/areariservataprop/common/MP.Master" AutoEventWireup="true" CodeBehind="inv_invoice_list.aspx.cs" Inherits="RentalInRome.areariservataprop.inv_invoice_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <asp:LinkButton ID="lnkFilter" runat="server" CssClass="ricercaris" onclick="lnkFilter_Click"> <span>Filtra Risultati</span> </asp:LinkButton>
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
                                            <table>
                                                <tr>
                                                    <td style="height: 40px;">
                                                        <label>Num.</label>
                                                        <asp:TextBox ID="txt_count" runat="server" CssClass="inp" Style="width: 40px;"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 40px;">
                                                        <label>Totale Importo &euro;</label>
                                                        <asp:TextBox ID="txt_prTotal" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 40px;">
                                                        <label>Media Importo &euro;</label>
                                                        <asp:TextBox ID="txt_prTotalMedia" runat="server" CssClass="inp" Style="width: 100px;"></asp:TextBox>
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
                                <asp:ListView ID="LV" runat="server" onpagepropertieschanged="LV_PagePropertiesChanged">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
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
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a target="_blank" href="<%# CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + Eval("uid")%>" style="margin-top: 6px; margin-right: 15px;">Anteprima</a>
                                            </td>
                                            <td>
                                                <a target="_blank" href="<%# CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + Eval("uid")).urlEncode() + "&filename=" + ("RiR-reservation_invoice-code_" + Eval("code") + ".pdf").urlEncode()%>" style="margin-top: 6px; margin-right: 5px;">Scarica il PDF</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
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
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_is_exported_1" Visible="false" runat="server" Text='<%# Eval("is_exported_1") %>' />
                                                <a target="_blank" href="<%# CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + Eval("uid")%>" style="margin-top: 6px; margin-right: 15px;">Anteprima</a>
                                            </td>
                                            <td>
                                                <a target="_blank" href="<%# CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + (CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + Eval("uid")).urlEncode() + "&filename=" + ("RiR-reservation_invoice-code_" + Eval("code") + ".pdf").urlEncode()%>" style="margin-top: 6px; margin-right: 5px;">Scarica il PDF</a>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>
                                                    No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th style="width: 80px;">Codice Pren </th>
                                                    <th style="width: 80px;">
                                                        Numero Fattura
                                                    </th>
                                                    <th style="width: 120px;">
                                                        Data Fattura
                                                    </th>
                                                    <th style="width: 80px; text-align: center; ">
                                                        Importo
                                                    </th>
                                                    <th>
                                                    </th>
                                                    <th></th>
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
    </asp:UpdatePanel>

    <script type="text/javascript">
        var cal_inv_dtInvoice_from;
        var cal_inv_dtInvoice_to;
        function setCal() {
            cal_inv_dtInvoice_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_from.ClientID %>", View: "#txt_inv_dtInvoice_from", Cleaner: "#del_inv_dtInvoice_from", changeMonth: true, changeYear: true });
            cal_inv_dtInvoice_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_inv_dtInvoice_to.ClientID %>", View: "#txt_inv_dtInvoice_to", Cleaner: "#del_inv_dtInvoice_to", changeMonth: true, changeYear: true });
        }
    </script>
</asp:Content>
