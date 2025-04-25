<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="invCashBookNew.aspx.cs" Inherits="ModInvoice.admin.modInvoice.invCashBookNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Elenco Documenti non pagati</h1>
                <div class="bottom_agg">
                    <a onclick="return CloseRadWindow('reload');">
                        <span>Chiudi</span>
                    </a>
                </div>
                <div class="bottom_agg" style="margin-right: 20px;">
                    <a href="invCashDocumentDett.aspx?id=0" title="Crea Nuovo">
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
                                                            <asp:ListItem Value="1">Attivo (Entrata)</asp:ListItem>
                                                            <asp:ListItem Value="0">Passivo (Uscita)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Numero Doc:</label>
                                                        <asp:TextBox ID="txt_flt_docNum" runat="server" Width="120" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Data Doc:</label>
                                                        <asp:TextBox ID="TextBox1" runat="server" Width="120" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Importo:</label>
                                                        <asp:TextBox ID="TextBox2" runat="server" Width="120" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Tipo anag.:</label>
                                                        <asp:DropDownList ID="drp_flt_ownerType" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="Fornitori">Fornitori</asp:ListItem>
                                                            <asp:ListItem Value="Gestori">Gestori</asp:ListItem>
                                                            <asp:ListItem Value="Proprietari">Proprietari</asp:ListItem>
                                                            <asp:ListItem Value="Risorse">Risorse Umane</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Al nome di:</label>
                                                        <asp:TextBox ID="TextBox3" runat="server" Width="250" CssClass="inp"></asp:TextBox>
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
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModInvoice.DCmodInvoice" TableName="dbInvCashDocumentTBLs" OrderBy="docIssueDate desc" EntityTypeName="" Where="cashPayed != cashAmount">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
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
                            <td style="text-align: right;">
                                <span>
                                    <%# Eval("cashUnpayed").objToDecimal().ToString("N2")%>
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
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="invCashDocumentDett.aspx?id=<%# Eval("id") %>&add=true" title="Completa Pagamento">Completa </a>
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
                            <td style="text-align: right;">
                                <span>
                                    <%# Eval("cashUnpayed").objToDecimal().ToString("N2")%>
                                </span>
                            </td>                            <td>
                                <span>
                                    <%# Eval("ownerType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ownerNameFull")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="invCashDocumentDett.aspx?id=<%# Eval("id") %>&add=true" title="Completa Pagamento">Completa </a>
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
                                    <th style="width: 50px">Codice </th>
                                    <th style="width: 120px">Attivo/Passivo </th>
                                    <th style="width: 150px">Numero Doc </th>
                                    <th style="width: 80px">Data Doc </th>
                                    <th style="width: 150px">Causale</th>
                                    <th style="width: 100px; text-align: center;">Importo </th>
                                    <th style="width: 100px; text-align: center;">Da pagare </th>
                                    <th style="width: 70px">Tipo anag. </th>
                                    <th style="width: 200px">Al nome di </th>
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
