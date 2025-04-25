<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="inv_payment_list.aspx.cs" Inherits="RentalInRome.admin.inv_payment_list" %>

<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function refreshDates() {
            //window.location = "rnt_request_details.aspx?id=<%=HF_id.Value %>";
        }
        function INV_openSelection(IdPayment) {
            var _url = "inv_payment_form.aspx?id=" + IdPayment;
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
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
                        <h1>Elenco pagamenti (Prima nota)</h1>
                        <div class="bottom_agg">
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
                                                                Codice Pagamento:</label>
                                                            <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Codice Prenotazione:</label>
                                                            <asp:TextBox ID="txt_rnt_reservation_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
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
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data Pagamento:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_pay_date_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_pay_date_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_pay_date_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_pay_date_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_pay_date_from" runat="server" />
                                                            <asp:HiddenField ID="HF_pay_date_to" runat="server" />
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
                    <div style="clear: both">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                                <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaInvoice_DataContext" TableName="INV_TBL_PAYMENT" OrderBy="dtCreation desc">
                                </asp:LinqDataSource>
                                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnDataBound="LV_DataBound">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("dtCreation")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  invUtils.invPayment_causeTitle("" + Eval("pay_cause"), "- - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
                                            </td>
                                            <td style="text-align: right;">
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
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("dtCreation")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%#  invUtils.invPayment_causeTitle("" + Eval("pay_cause"), "- - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("rnt_reservation_code")%></span>
                                            </td>
                                            <td style="text-align: right;">
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
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a onclick="INV_openSelection('<%# Eval("id") %>')" style="margin-top: 6px; margin-right: 5px;">dettagli</a>
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
                                        <div class="page">
                                            <asp:DataPager ID="DataPager2" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th style="width: 80px;">
                                                        Codice
                                                    </th>
                                                    <th style="width: 120px;">
                                                        Data Creazione
                                                    </th>
                                                    <th style="width: 200px;">
                                                        Causale
                                                    </th>
                                                    <th style="width: 80px;">
                                                        Codice Pren
                                                    </th>
                                                    <th style="width: 80px; text-align: center; ">
                                                        Importo
                                                    </th>
                                                    <th style="width: 50px;">
                                                        Pagato?
                                                    </th>
                                                    <th style="width: 120px;">
                                                        Data Ora Pagamento
                                                    </th>
                                                    <th style="width: 120px;">
                                                        Mod. Pagamento
                                                    </th>
                                                    <th>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                            </table>
                                        </div>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
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
		var cal_dtCreation_from;
		var cal_dtCreation_to;

		var cal_pay_date_from;
		var cal_pay_date_to;
		function setCal() {
		    cal_dtCreation_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_from.ClientID %>", View: "#txt_dtCreation_from", Cleaner: "#del_dtCreation_from", changeMonth: true, changeYear: true });
		    cal_dtCreation_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_to.ClientID %>", View: "#txt_dtCreation_to", Cleaner: "#del_dtCreation_to", changeMonth: true, changeYear: true });

		    cal_pay_date_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_pay_date_from.ClientID %>", View: "#txt_pay_date_from", Cleaner: "#del_pay_date_from", changeMonth: true, changeYear: true });
		    cal_pay_date_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_pay_date_to.ClientID %>", View: "#txt_pay_date_to", Cleaner: "#del_pay_date_to", changeMonth: true, changeYear: true });
		}
    </script>

</asp:Content>
