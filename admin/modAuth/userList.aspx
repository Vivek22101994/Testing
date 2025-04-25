<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="userList.aspx.cs" Inherits="ModAuth.admin.modAuth.userList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "stpDett.aspx?id=" + id;
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                //rwdUrl.set_minWidth(700);
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
                <h1>
                    Gestione ruoli
                </h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModAuth.DCmodAuth" TableName="dbAuthUserTBLs" OrderBy="nameFull" Where="id>2" EntityTypeName="">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# authUtils.getRole_title(Eval("pidRole").objToInt32(), "-non abbinato-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');" Style="text-decoration: none; border: 0 none; margin: 5px;">Elimina</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# authUtils.getRole_title(Eval("pidRole").objToInt32(), "-non abbinato-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
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
                                    <th style="width: 30px">
                                        ID
                                    </th>
                                    <th style="width: 300px">
                                        Nome Completo
                                    </th>
                                    <th style="width: 150px">
                                        Ruolo
                                    </th>
                                    <th style="width: 50px">
                                        Attivo?
                                    </th>
                                    <th>
                                    </th>
                                    <th>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false" VisibleTitlebar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <h1 class="titolo_main">
                            Scheda
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
                                                    Attivo?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                                        <asp:ListItem Value="0" Text="NO"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="SI"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Nome Completo:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_nameFull" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Email:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_email" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Telefono:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_phone" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Cellulare:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_phoneMobile" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Ruolo:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_pidRole" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Login:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_login" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    Password:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_password" Width="230px" />
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
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
                                <div class="bottom_salva">
                                    <a onclick="return rwdDettClose();"><span>Chiudi</span></a>
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
