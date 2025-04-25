﻿<%@ Page Title="" Language="C#" MasterPageFile="/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateInternsFeatureList.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateInternsFeatureList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var rwdUrl = null;
        function setUrl(type, id) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            var url;
            if (type == "dett") {
                url = "EstateInternsFeatureDett.aspx?id=" + id;
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
                <h1><%# contUtils.getLabel("lblInternsFeatureList", App.LangID,"Gestione Caratteristiche") %></h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ <%# contUtils.getLabel("lblNew", App.LangID,"Nuovo") %></span></asp:LinkButton>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntEstateInternsFeatureTBs" OrderBy="id" EntityTypeName="">
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
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("pidInternsType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? contUtils.getLabel("lblYes", App.LangID,"SI") : contUtils.getLabel("lblNo", App.LangID,"NO")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="<%# contUtils.getLabel("lblDettTooltip", App.LangID,"Scheda") %>" style="text-decoration: none; border: 0 none; margin: 5px;">
                                    <%# contUtils.getLabel("lblDett", App.LangID,"Scheda") %>
                                </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick='<%# "return confirm(\""+contUtils.getLabel("lblDeleteAlert", App.LangID,"Stai per eliminare senza la possibilità di ripristinare?") +"\");" %>'
                                    ToolTip='<%# contUtils.getLabel("lblDelete", App.LangID,"Elimina") %>' Style="text-decoration: none; border: 0 none; margin: 5px;">
                                     <%# contUtils.getLabel("lblDelete", App.LangID,"Elimina") %>
                                </asp:LinkButton>
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
                                    <%# Eval("code")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("pidInternsType")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("isActive") + "" == "1" ? contUtils.getLabel("lblYes", App.LangID,"SI") : contUtils.getLabel("lblNo", App.LangID,"NO")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="#" onclick="return setUrl('dett', '<%# Eval("id") %>')" title="<%# contUtils.getLabel("lblDettTooltip", App.LangID,"Scheda") %>" style="text-decoration: none; border: 0 none; margin: 5px;">
                                    <%# contUtils.getLabel("lblDett", App.LangID,"Scheda") %>
                                </a>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkDel" CommandName="del" runat="server" OnClientClick='<%# "return confirm(\""+contUtils.getLabel("lblDeleteAlert", App.LangID,"Stai per eliminare senza la possibilità di ripristinare?") +"\");" %>'
                                    ToolTip='<%# contUtils.getLabel("lblDelete", App.LangID,"Elimina") %>' Style="text-decoration: none; border: 0 none; margin: 5px;">
                                     <%# contUtils.getLabel("lblDelete", App.LangID,"Elimina") %>
                                </asp:LinkButton>
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
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr style="text-align: left">
                                    <th style="width: 30px">ID </th>
                                    <th style="width: 300px"><%# contUtils.getLabel("lblCode", App.LangID,"Codice") %></th>
                                    <th style="width: 300px"><%# contUtils.getLabel("lblInternType", App.LangID,"Tipo") %></th>
                                    <th style="width: 50px"><%# contUtils.getLabel("lblActive", App.LangID,"Attivo") %>? </th>
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
                        <h1 class="titolo_main"><%# contUtils.getLabel("lblDett", App.LangID,"Scheda") %></h1>
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
                                                    <%# contUtils.getLabel("lblActive", App.LangID,"Attivo") %>?
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_isActive" runat="server" DataSource='<%# CommonUtilities.GetIsActiveList() %>'
                                                        DataTextField ="Value" DataValueField="Key">                                                        
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    <%# contUtils.getLabel("lblCode", App.LangID,"Codice") %>:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_code" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">
                                                    <%# contUtils.getLabel("lblInternType", App.LangID,"Tipo") %>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_pidInternsType" runat="server">
                                                    </asp:DropDownList>
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
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span><%# contUtils.getLabel("lblSaveChanges", App.LangID,"Salva Modifiche") %></span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span><%# contUtils.getLabel("lblCancelChanges", App.LangID,"Annulla Modifiche") %></span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <a onclick="return rwdDettClose();">
                                        <span><%# contUtils.getLabel("lblCalClose", App.LangID,"Chiudi") %></span></a>
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
