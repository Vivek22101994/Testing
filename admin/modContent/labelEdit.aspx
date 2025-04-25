<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="labelEdit.aspx.cs" Inherits="ModContent.admin.modContent.labelEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="" runat="server" />
            <div style="clear: both">
                <h1>Gestione Variabili di lingua</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>
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
                                                        <label>Tipo:</label>
                                                        <asp:DropDownList ID="drp_flt_type" runat="server" CssClass="inp">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Nome:</label>
                                                        <asp:TextBox ID="txt_flt_id" runat="server" Width="150" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Valore in Italiano:</label>
                                                        <asp:TextBox ID="txt_flt_mTitle" runat="server" Width="200" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Lingua:</label>
                                                        <asp:DropDownList ID="drp_flt_pidLang" runat="server" CssClass="inp">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Valore in lingua:</label>
                                                        <asp:TextBox ID="txt_flt_title" runat="server" Width="150" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Senza Valore:</label>
                                                        <asp:DropDownList ID="drp_flt_withNoValue" runat="server" CssClass="inp">
                                                            <asp:ListItem Text="-non filtrare-" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="in Italiano" Value="main"></asp:ListItem>
                                                            <asp:ListItem Text="in Lingua" Value="other"></asp:ListItem>
                                                            <asp:ListItem Text="1 dei 2" Value="all"></asp:ListItem>
                                                        </asp:DropDownList>
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
                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("type")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td<%# ("" + Eval("mTitle")).Trim() == "" ? " style=\"background-color: #FF0000;\"" : ""%>>
                                <span>
                                    <%# Eval("mtitle")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# contUtils.getLang_title(""+Eval("pidLang"))%></span>
                            </td>
                            <td<%# ("" + Eval("title")).Trim() == "" ? " style=\"background-color: #FF0000;\"" : ""%>>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare la variabile in tutt le lingue senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("type")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td<%# ("" + Eval("mtitle")).Trim() == "" ? " style=\"background-color: #FF0000;\"" : ""%>>
                                <span>
                                    <%# Eval("mtitle")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# contUtils.getLang_title(""+Eval("pidLang"))%></span>
                            </td>
                            <td<%# ("" + Eval("title")).Trim() == "" ? " style=\"background-color: #FF0000;\"" : ""%>>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare la variabile in tutt le lingue senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
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
                                    <th style="width: 50px">Tipo </th>
                                    <th style="width: 200px">Nome </th>
                                    <th style="width: 200px">Italiano </th>
                                    <th style="width: 80px">Lingua </th>
                                    <th style="width: 200px">Traduzione </th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <h1 class="titolo_main" style="color: #E01E15; text-transform: none; font-size: 15px; margin-left: 30px;">
                            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
                        </h1>
                        <!-- INIZIO MAIN LINE -->
                        <div class="mainline">
                            <div class="mainbox">
                                <div class="top">
                                    <div class="sx">
                                    </div>
                                    <div class="dx">
                                    </div>
                                </div>
                                <div class="center">
                                    <div class="boxmodulo">
                                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                            <tr>
                                                <td class="td_title">
                                                    Nome Variabile:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_id" Width="230px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_id" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Tipo:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_type" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:HiddenField ID="HfLang" Value="0" runat="server" />
                                                    <asp:ListView ID="LvLangs" runat="server" OnItemCommand="LvLangs_ItemCommand">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkLang" CommandName="change_lang" runat="server" CssClass='<%# HfLang.Value == "" + Eval("id") ? "tab_item_current" : "tab_item"%>'>
                                                                <span>
                                                                    <%# Eval("title") %></span>
                                                            </asp:LinkButton>
                                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                        </EmptyDataTemplate>
                                                        <LayoutTemplate>
                                                            <div class="menu2">
                                                                <a id="itemPlaceholder" runat="server" />
                                                                <div class="nulla">
                                                                </div>
                                                            </div>
                                                        </LayoutTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Traduzione:<br />
                                                    <asp:TextBox runat="server" ID="txt_title" Width="400px" TextMode="MultiLine" Height="115px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="bottom">
                                    <div class="sx">
                                    </div>
                                    <div class="dx">
                                    </div>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <div class="salvataggio">
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" CausesValidation="true" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <a onclick="return rwdDettClose();">
                                        <span>Chiudi</span></a>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                    </telerik:RadAjaxPanel>
                </ContentTemplate>
            </telerik:RadWindow>
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
