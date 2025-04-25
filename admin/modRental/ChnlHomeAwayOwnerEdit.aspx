<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlHomeAwayOwnerEdit.aspx.cs" Inherits="ModRental.admin.modRental.ChnlHomeAwayOwnerEdit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Gestione ChnlHomeAwayOwner</h1>
                <div class="bottom_agg">
                    <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click"><span>+ Nuovo</span></asp:LinkButton>
                </div>
            </div>
            <div style="clear: both">
                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCchnlHomeAway" TableName="dbRntChnlHomeAwayOwnerTBLs" OrderBy="id" EntityTypeName="">
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
                                    <%# Eval("username")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("advertiserAssignedId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ppb_advertiserAssignedId")%></span>
                            </td>
                            
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" CommandName="del" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;" OnClientClick="return confirm('sei sicuro?')">Elimina</asp:LinkButton>
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
                                    <%# Eval("username")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("advertiserAssignedId")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("ppb_advertiserAssignedId")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <asp:LinkButton ID="lnkDett" CommandName="dett" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;">Scheda</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" CommandName="del" runat="server" Style="text-decoration: none; border: 0 none; margin: 5px;" OnClientClick="return confirm('sei sicuro?')">Elimina</asp:LinkButton>
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
                                    <th style="width: 100px">Id </th>
                                    <th style="width: 150px">UserName</th>
                                    <th style="width: 150px">advertiserAssignedId </th>
                                    <th style="width: 150px">PPB advertiserAssignedId </th>
                                    <th></th>
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
            <telerik:RadWindow runat="server" ID="rwdDett" Modal="true" AutoSize="true" ShowContentDuringLoad="false" MinWidth="450" OnClientClose="rwdDett_OnClientClose" VisibleStatusbar="false">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
                        <h1 class="titolo_main">Scheda </h1>
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
                                                <td class="td_username">username:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_username" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_username">advertiserAssignedId:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_advertiserAssignedId" Width="230px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_username">PPB advertiserAssignedId:
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_ppb_advertiserAssignedId" Width="230px" />
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
                                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton>
                                </div>
                                <div class="bottom_salva">
                                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                                </div>
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
                //RadAjaxOwner_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
