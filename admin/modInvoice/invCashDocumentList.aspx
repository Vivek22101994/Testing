<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="invCashDocumentList.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invCashDocumentList" %>

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
                url = "invCashDocumentDett.aspx?id=" + id;
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
                <h1>Gestione Documenti </h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0')" title="Crea Nuovo">
                        <span>+ Nuovo</span>
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
                                                    <label>Codice:</label>
                                                    <asp:TextBox ID="txt_flt_code" runat="server" Width="50" CssClass="inp"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label>Attivo/Passivo:</label>
                                                    <asp:DropDownList ID="drp_flt_cashInOut" runat="server" CssClass="inp">
                                                        <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                        <asp:ListItem Value="1">Attivo (Entrata)</asp:ListItem>
                                                        <asp:ListItem Value="0">Passivo (Uscita)</asp:ListItem>
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
                                                    <label>Numero Doc:</label>
                                                    <asp:TextBox ID="txt_flt_docNum" runat="server" Width="120" CssClass="inp"></asp:TextBox>
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
                    <div class="b">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both">
                <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModInvoice.DCmodInvoice" TableName="dbInvCashDocumentTBLs" OrderBy="docIssueDate desc" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanged="LV_PagePropertiesChanged">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <a class="ttpR" ttpc="div_curr_<%# Eval("id") %>" style="color: #FE6634; cursor: pointer; font-family: monospace; font-size: 19px; font-weight: bold; line-height: 12px;">?</a>
                                <div id="div_curr_<%# Eval("id") %>" style="display: none;">
                                    <div class="box_dett_day">
                                        <table>
                                            <tr>
                                                <td style="width: 80px;">
                                                </td>
                                                <td style="width: 220px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Registrato da :</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("createdUserNameFull")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>in data :</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("createdDate")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" style="border-top: 1px dotted #000;">
                                                    <strong>Descrizione:</strong>
                                                </td>
                                                <td style="border-top: 1px dotted #000;">
                                                    <%# Eval("docBody")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashInOut") + "" == "1" ? "Attivo (Entrata)" : "Passivo (Uscita)"%></span>
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
                            <td>
                                <span>
                                    <%# Eval("docCaseCode")%></span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# Eval("cashAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td>
                                <a class="ttpR" ttpC="div_pay_<%# Eval("id") %>" style="color: #<%# Eval("cashPayed").objToDecimal() >= Eval("cashAmount").objToDecimal() ? "00FF00" : "FE6634"%>; cursor: pointer; font-weight: bold;">
                                    <%# Eval("cashPayed").objToDecimal() > 0 ? "SI" : "NO"%>
                                </a>
                                <div id="div_pay_<%# Eval("id") %>" style="display: none;">
                                    <div class="box_dett_day">
                                        <table>
                                            <tr>
                                                <td style="width: 80px;">
                                                </td>
                                                <td style="width: 220px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Pagato:</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("cashPayed").objToDecimal().ToString("N2")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>in Data:</strong>
                                                </td>
                                                <td>
                                                    <%# getFirstCashBook(Eval("id").objToInt64())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Da pagare:</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("cashUnpayed").objToDecimal().ToString("N2")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
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
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <%# "" + Eval("docPath") == "" ? "" : "<a href=\"/" + Eval("docPath") + "\" target=\"_blank\" title=\"Scarica Documento\" style=\"text-decoration: none; border: 0 none; margin: 5px;\">Doc " + System.IO.Path.GetExtension("" + Eval("docPath")).Replace(".", "") + " </a>"%>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <a class="ttpR" ttpc="div_curr_<%# Eval("id") %>" style="color: #FE6634; cursor: pointer; font-family: monospace; font-size: 19px; font-weight: bold; line-height: 12px;">?</a>
                                <div id="div_curr_<%# Eval("id") %>" style="display: none;">
                                    <div class="box_dett_day">
                                        <table>
                                            <tr>
                                                <td style="width: 80px;">
                                                </td>
                                                <td style="width: 220px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Registrato da :</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("createdUserNameFull")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>in data :</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("createdDate")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" style="border-top: 1px dotted #000;">
                                                    <strong>Descrizione:</strong>
                                                </td>
                                                <td style="border-top: 1px dotted #000;">
                                                    <%# Eval("docBody")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("code") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("cashInOut") + "" == "1" ? "Attivo (Entrata)" : "Passivo (Uscita)"%></span>
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
                            <td>
                                <span>
                                    <%# Eval("docCaseCode")%></span>
                            </td>
                            <td style="text-align: right;">
                                <span>
                                    <%# Eval("cashAmount").objToDecimal().ToString("N2")%>
                                </span>
                            </td>
                            <td>
                                <a class="ttpR" ttpc="div_pay_<%# Eval("id") %>" style="color: #<%# Eval("cashPayed").objToDecimal() >= Eval("cashAmount").objToDecimal() ? "00FF00" : "FE6634"%>; cursor: pointer; font-weight: bold;">
                                    <%# Eval("cashPayed").objToDecimal() > 0 ? "SI" : "NO"%>
                                </a>
                                <div id="div_pay_<%# Eval("id") %>" style="display: none;">
                                    <div class="box_dett_day">
                                        <table>
                                            <tr>
                                                <td style="width: 80px;">
                                                </td>
                                                <td style="width: 220px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Pagato:</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("cashPayed").objToDecimal().ToString("N2")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>in Data:</strong>
                                                </td>
                                                <td>
                                                    <%# getFirstCashBook(Eval("id").objToInt64())%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Da pagare:</strong>
                                                </td>
                                                <td>
                                                    <%# Eval("cashUnpayed").objToDecimal().ToString("N2")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
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
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <%# "" + Eval("docPath") == "" ? "" : "<a href=\"/" + Eval("docPath") + "\" target=\"_blank\" title=\"Scarica Documento\" style=\"text-decoration: none; border: 0 none; margin: 5px;\">Doc " + System.IO.Path.GetExtension("" + Eval("docPath")).Replace(".", "") + " </a>"%>
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
                                    <th></th>
                                    <th style="width: 50px">Codice </th>
                                    <th style="width: 120px">Attivo/Passivo </th>
                                    <th style="width: 150px">Numero Doc </th>
                                    <th style="width: 80px">Data Doc </th>
                                    <th style="width: 150px">Causale</th>
                                    <th style="width: 100px; text-align: center;">Importo </th>
                                    <th style="width: 40px;">Pagato? </th>
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
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="20" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
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
