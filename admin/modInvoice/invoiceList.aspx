<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="invoiceList.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invoiceList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showFilter() {
            document.getElementById("lnk_showfilt").style.display = "none";
            document.getElementById("lnk_hidefilt").style.display = "";
            document.getElementById("tbl_filter").style.display = "";
        }
        function hideFilter() {
            document.getElementById("lnk_showfilt").style.display = "";
            document.getElementById("lnk_hidefilt").style.display = "none";
            document.getElementById("tbl_filter").style.display = "none";
        }
    </script>
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "invoiceDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            } if (type == "new") {
                url = "invoiceDett.aspx" ;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(700);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                rwdUrl.maximize();
            }
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione Nota di credito </h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('new', '0')" title="Crea Nuova Fattura" runat="server" visible="false">
                        <span>+ Nuova Fattura</span>
                    </a>
                </div>
                <div class="nulla">
                </div>
                <div class="filt">
                    <div class="t">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="c">
                        <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                        <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                        <div class="filtro_cont">
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <telerik:RadAjaxPanel ID="rapFilter" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label>
                                                        Numero Fattura:</label>
                                                    <asp:TextBox ID="txt_flt_docNum" runat="server" Width="120" CssClass="inp"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label>Attivo/Passivo:</label>
                                                    <asp:DropDownList ID="drp_flt_cashInOut" runat="server" CssClass="inp">
                                                        <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                        <asp:ListItem Value="1">Attivo (Entrata)</asp:ListItem>
                                                        <asp:ListItem Value="0">Passivo (Nota di credito)</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>Data da:</label>
                                                    <telerik:RadDatePicker ID="rdp_flt_cashDateFrom" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <label>Data a:</label>
                                                    <telerik:RadDatePicker ID="rdp_flt_cashDateTo" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <label>Tipo anag.:</label>
                                                    <asp:DropDownList ID="drp_flt_ownerType" runat="server" CssClass="inp">
                                                        <asp:ListItem Value="">-tutti-</asp:ListItem>
                                                        <asp:ListItem Value="Fornitori">Fornitori</asp:ListItem>
                                                        <asp:ListItem Value="Gestori">Gestori</asp:ListItem>
                                                        <asp:ListItem Value="Proprietari">Proprietari</asp:ListItem>
                                                        <asp:ListItem Value="Risorse">Risorse Umane</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>Al nome di:</label>
                                                    <asp:TextBox ID="txt_flt_ownerNameFull" runat="server" Width="250" CssClass="inp"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        </telerik:RadAjaxPanel>
                                    </td>
                                    <td valign="bottom">
                                        <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra Risultati</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="c">
                        <a class="filto_bottone2" style="background-image: none; font-size: 11px; font-weight: bold; margin-top: 20px; display: block; padding: 0pt;">TOTALI</a>
                        <div class="filtro_cont">
                            <table border="0" cellpadding="0" cellspacing="0" id="Table1">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <label style="font-weight: bold; font-size: 13px;">Attivi</label>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Totale Importo</label>
                                                    <asp:TextBox ID="txt_cashTotalAmount_1" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imponibile</label>
                                                    <asp:TextBox ID="txt_cashTaxFree_1" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imposta</label>
                                                    <asp:TextBox ID="txt_cashTaxAmount_1" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Registrati</label>
                                                    <asp:TextBox ID="txt_count_1" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();" Style="width: 50px;"></asp:TextBox>
                                                </td>
                                                <td style="border-left: 3px dotted rgb(0, 0, 0); padding-left: 10px;">
                                                    <label style="font-weight: bold; font-size: 13px;">Passivi</label>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Totale Importo</label>
                                                    <asp:TextBox ID="txt_cashTotalAmount_0" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imponibile</label>
                                                    <asp:TextBox ID="txt_cashTaxFree_0" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imposta</label>
                                                    <asp:TextBox ID="txt_cashTaxAmount_0" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Registrati</label>
                                                    <asp:TextBox ID="txt_count_0" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();" Style="width: 50px;"></asp:TextBox>
                                                </td>
                                                <td style="border-left: 3px dotted rgb(0, 0, 0); padding-left: 10px;">
                                                    <label style="font-weight: bold; font-size: 13px;">Situazione (Differenza)</label>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Totale Importo</label>
                                                    <asp:TextBox ID="txt_cashTotalAmount_diff" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imponibile</label>
                                                    <asp:TextBox ID="txt_cashTaxFree_diff" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Imposta</label>
                                                    <asp:TextBox ID="txt_cashTaxAmount_diff" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();"></asp:TextBox>
                                                    <div class="nulla">
                                                    </div>
                                                    <label>Registrati</label>
                                                    <asp:TextBox ID="txt_count_diff" runat="server" CssClass="inp" ReadOnly="true" onfocus="this.select();" Style="width: 50px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Table seconda riga -->
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>  
                    <div class="b">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanged="LV_PagePropertiesChanged">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("cashInOut") + "" == "1" ? "Attivo (Entrata)" : "Passivo (Nota di credito)"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("docNum")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("docIssueDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTaxFree").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTaxAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTotalAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ownerType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ownerNameFull")%></span>
                            </td>
                            <td>
                                <a href="<%# App.RP + "admin/modPdf/createPdf.aspx?url=" + (App.HOST + App.RP + "admin/modInvoice/invoicePdf.aspx?uid=" + Eval("uid")).urlEncode() + "&filename=" + (Eval("docNum") + ".pdf").urlEncode()%>" title="Scarica PDF" target="_blank" style="text-decoration: none; border: 0 none; margin: 5px;">PDF </a>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("cashInOut") + "" == "1" ? "Attivo (Entrata)" : "Passivo (Nota di credito)"%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("docNum")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("docIssueDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTaxFree").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTaxAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# "&euro;&nbsp;" + Eval("cashTotalAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ownerType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ownerNameFull")%></span>
                            </td>
                            <td>
                                <a href="<%# App.RP + "admin/modPdf/createPdf.aspx?url=" + (App.HOST + App.RP + "admin/modInvoice/invoicePdf.aspx?uid=" + Eval("uid")).urlEncode() + "&filename=" + (Eval("docNum") + ".pdf").urlEncode()%>" title="Scarica PDF" target="_blank" style="text-decoration: none; border: 0 none; margin: 5px;">PDF </a>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
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
                                <tr style="text-align: left">
                                    <th style="width: 120px">Attivo/Passivo </th>
                                    <th style="width: 80px">
                                        Num. Fattura
                                    </th>
                                    <th style="width: 80px">Data Doc </th>
                                    <th style="width: 100px; text-align: right;">Imponibile&nbsp;&nbsp;&nbsp; </th>
                                    <th style="width: 100px; text-align: right;">Imposta&nbsp;&nbsp;&nbsp; </th>
                                    <th style="width: 100px; text-align: right;">Importo Totale&nbsp;&nbsp;&nbsp; </th>
                                    <th style="width: 70px">Tipo anag. </th>
                                    <th style="width: 200px">Al nome di </th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
                <div class="page">
                    <asp:DataPager ID="LV_DataPager" runat="server" PagedControlID="LV" PageSize="50" style="border-right: medium none;">
                        <Fields>
                            <asp:NumericPagerField ButtonCount="20" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </div>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            } 
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
