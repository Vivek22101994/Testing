<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="commentList.aspx.cs" Inherits="ModBlog.admin.modBlog.commentList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "commentDett.aspx?id=" + id;
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
                    Gestione Commenti per Articoli</h1>
                <div class="bottom_agg">
                    <a href="#" onclick="return setUrl('dett', '0')" title="Crea Nuovo"><span>+ Nuovo</span> </a>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModBlog.DCmodBlog" TableName="dbBlogCommentTBLs" OrderBy="commentDate desc" EntityTypeName="">
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
                                    <%# Eval("commentDate")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("typeCode")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("email")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# blogUtils.getArticle_title(Eval("pidArticle").objToInt64(), 1, "-non abbinato-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("commentSubject")%></span>
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
                                    <%# Eval("commentDate")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("typeCode")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("nameFull")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("email")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# blogUtils.getArticle_title(Eval("pidArticle").objToInt64(), 1, "-non abbinato-")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("commentSubject")%></span>
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
                                    <th style="width: 120px">
                                        Data Commento
                                    </th>
                                    <th style="width: 30px">
                                        Sesso
                                    </th>
                                    <th style="width: 200px">
                                        Nome Cognome
                                    </th>
                                    <th style="width: 150px">
                                        Email
                                    </th>
                                    <th style="width: 300px">
                                        Articolo
                                    </th>
                                    <th style="width: 300px">
                                        Sommario
                                    </th>
                                    <th style="width: 50px">
                                        Approvato?
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
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
</asp:Content>
