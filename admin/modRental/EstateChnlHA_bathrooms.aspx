<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHA_bathrooms.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHA_bathrooms" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHAEstateDetailsTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="/jquery/plugin/jquerytools/all.min.js" type="text/javascript"></script>--%>
    <script src="/jquery/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/jquery/plugin/jquerytools/tabs.js" type="text/javascript"></script>
    
    <img src="/images/css/homeaway-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with HomeAway" />

    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <h1 class="titolo_main">Bedrooms of Property for HA:<asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
    </telerik:RadCodeBlock>
    <div id="fascia1">
        <div class="tabsTop" id="tabsHomeaway">
            <uc1:ucNav ID="ucNav" runat="server" />
        </div>
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1">
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <asp:HiddenField ID="HF_uid" Value="" runat="server" />
            <div style="clear: both">
                <asp:ListView ID="Lv" runat="server" OnItemCommand="LvItemCommand">
                    <ItemTemplate>
                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                        <div class="mainbox homeAwayBox" style="margin: 10px;">
                            <div class="top">
                                <div class="sx">
                                </div>
                                <div class="dx">
                                </div>
                            </div>
                            <span class="titoloboxmodulo"><%=roomType %> # <asp:Literal ID="ltrNumber" runat="server"></asp:Literal></span>
                            <div class="boxmodulo">
                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                    <tr>
                                        <td class="td_title">type:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpType" runat="server" Style="height: 25px;" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LvFeatures" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_code" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                            <tr>
                                                <td class="td_title"><%# Eval("title") %>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_featuresCount" runat="server" Width="50px">
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="chk_featuresSelected" runat="server" Text='' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <tr>
                                        <td colspan="2" class="">
                                            <div class="boxBooking" style="width: 100%;">
                                                <ul class="tabs">
                                                    <asp:ListView ID="LvLangTab" runat="server">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                            <li><a href="#"><%# Eval("title") %></a></li>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </ul>
                                                <div class="panes">
                                                    <asp:ListView ID="LvLangPane" runat="server">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                            <div class="paneCliente">
                                                                <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                                                    <tr>
                                                                        <td class="td_title">name:
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txt_title" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">note:<br />
                                                                            <asp:TextBox runat="server" ID="txt_description" TextMode="MultiLine" Height="100" Width="400" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </div>

                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CssClass="inlinebtn" CommandName="mod">Modifica</asp:LinkButton>
                                            <asp:LinkButton ID="lnkSave" runat="server" CssClass="inlinebtn" CommandName="sav">Salva</asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CssClass="inlinebtn" CommandName="canc">Annulla</asp:LinkButton>
                                            <asp:LinkButton ID="lnkDel" runat="server" CssClass="inlinebtn" CommandName="del" OnClientClick="return confirm('Vuoi eliminare?')">Elimina</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                </asp:ListView>
                <div class="mainbox homeAwayBox" style="margin: 10px;">
                    <div class="top">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <span class="titoloboxmodulo">Aggiungi</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td class="td_title">type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpType" runat="server" Style="height: 25px;" Width="100px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:ListView ID="LvFeatures" runat="server">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_code" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                    <tr>
                                        <td class="td_title"><%# Eval("title") %>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drp_featuresCount" runat="server" Width="50px">
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chk_featuresSelected" runat="server" Text='' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                            <tr>
                                <td colspan="2">
                                    <div class="boxBooking" style="width: 100%;">
                                        <ul class="tabs">
                                            <asp:ListView ID="LvLangTab" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                    <li><a href="#"><%# Eval("title") %></a></li>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </ul>
                                        <div class="panes">
                                            <asp:ListView ID="LvLangPane" runat="server">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                                    <div class="paneCliente">
                                                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                                            <tr>
                                                                <td class="td_title">name:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txt_title" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">note:<br />
                                                                    <asp:TextBox runat="server" ID="txt_description" TextMode="MultiLine" Height="100" Width="400" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>

                                        <div class="nulla">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="inlinebtn" OnClick="lnkSave_Click">Salva</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="inlinebtn" OnClick="lnkCancel_Click">Annulla</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
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
  
        <script type="text/javascript">
            $j = jQuery.noConflict();
            function setTabs() {
                $j("ul.tabs").tabs("div.panes > div");
            }
            $j(document).ready(function () {
                setTabs();
            });
        </script>
    </telerik:RadCodeBlock>
</asp:Content>