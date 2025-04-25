<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="articleList.aspx.cs" Inherits="ModBlog.admin.modBlog.articleList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {

            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "articleDett.aspx?id=" + id;
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
                <h1>Gestione Articoli </h1>
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
                                                    <td runat="server" visible="false">
                                                        <label>
                                                            Lingua:</label>
                                                        <asp:DropDownList ID="drp_flt_pidLang" runat="server" CssClass="inp">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div class="nulla"></div>
                                                        <label>Stato:</label>
                                                        <asp:DropDownList ID="drp_flt_isActive" runat="server" CssClass="inp">
                                                            <asp:ListItem Value="-1">-tutti-</asp:ListItem>
                                                            <asp:ListItem Value="1">Attivi</asp:ListItem>
                                                            <asp:ListItem Value="0">NON Attivi</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <label>Categoria:</label>
                                                        <asp:DropDownList ID="drp_flt_pidCategory" runat="server" CssClass="inp">
                                                        </asp:DropDownList>
                                                        <div class="nulla">
                                                        </div>
                                                        <label>Titolo:</label>
                                                        <asp:TextBox ID="txt_flt_title" runat="server" Width="250" CssClass="inp"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <label>Publicazione:</label>
                                                        <table class="inp">
                                                            <tr>
                                                                <td>
                                                                    <label>da:</label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="rdp_flt_publicDateFrom" runat="server" Width="100px" CssClass="inp">
                                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                                        </DateInput>
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label>a:</label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="rdp_flt_publicDateTo" runat="server" Width="100px" CssClass="inp">
                                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                                        </DateInput>
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        
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
                <asp:HiddenField ID="HF_LDS_orderBy" runat="server" Value="publicDate desc" Visible="false" />
                <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModBlog.DCmodBlog" TableName="dbBlogArticleVIEWs">
                </asp:LinqDataSource>
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" OnPagePropertiesChanged="LV_PagePropertiesChanged">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("id")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# contUtils.getLang_title("" + Eval("pidLang"))%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("publicDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# blogUtils.getCategory_title(Eval("pidCategory").objToInt64(), 1, "-senza categoria-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <a href="ArticleMedia.aspx?id=<%# Eval("id") %>" title="Apri Gallery" style="text-decoration: none; border: 0 none; margin: 5px;">Gallery </a>
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
                                    <%# contUtils.getLang_title("" + Eval("pidLang"))%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime?)Eval("publicDate")).formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----")%>
                                </span>
                            </td>
                            <td>
                                <span>
                                    <%# blogUtils.getCategory_title(Eval("pidCategory").objToInt64(), 1, "-senza categoria-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? "SI" : "NO"%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="Apri Scheda" style="text-decoration: none; border: 0 none; margin: 5px;">Scheda </a>
                            </td>
                            <td>
                                <a href="ArticleMedia.aspx?id=<%# Eval("id") %>" title="Apri Gallery" style="text-decoration: none; border: 0 none; margin: 5px;">Gallery </a>
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
                                    <th style="width: 30px">ID </th>
                                    <th style="width: 60px">Lingua </th>
                                    <th style="width: 400px">
                                        <asp:LinkButton ID="lnk_orderBy_title" runat="server" CommandName="orderBy" CommandArgument="title">Titolo</asp:LinkButton>
                                    </th>
                                    <th style="width: 100px">
                                        <asp:LinkButton ID="lnk_orderBy_publicDate" runat="server" CommandName="orderBy" CommandArgument="publicDate">Publicazione</asp:LinkButton>
                                    </th>
                                    <th style="width: 200px">Categoria </th>
                                    <th style="width: 50px">Attivo? </th>
                                    <th colspan="7"></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                        <div class="page">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;">
                                <Fields>
                                    <asp:NumericPagerField />
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
