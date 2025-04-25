<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="RiKenyaStpDett.aspx.cs" Inherits="ModContent.admin.modContent.RiKenyaStpDett" %>

<%@ Register Src="UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <h1 class="titolo_main">Scheda </h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
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
                                    Nome pagina:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_pageName" Width="230px" />
                                </td>
                                <td style="width: 20px">
                                </td>
                                <td class="td_title">
                                    Page rewrite:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_pageRewrite" Width="230px" />
                                </td>
                                <td style="width: 20px">
                                </td>
                                <td class="td_title" style="width:40px">
                                    Attivo?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                            <tr>
                                <td>
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
                                <td>
                                    <table cellpadding="0" cellspacing="10">
                                        <tr>
                                            <td style="border-right: medium ridge; vertical-align: top;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Visualizzazione</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">
                                                            Titolo Voce Menu:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_menuTitle" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">
                                                            Titolo:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_title" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">
                                                            Sottotitolo:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_subTitle" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">
                                                            Url:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_url" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Sommario:<br />
                                                            <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="border-left: medium ridge; vertical-align: top;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Motori Ricerca</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Meta Title:<br />
                                                            <span class="error_text" id='count_txt_metaTitle' style="display: none">
                                                                <span class="char"></span>
                                                                &nbsp;caratteri e&nbsp;
                                                                <span class="word"></span>
                                                                &nbsp;parole </span>
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_metaTitle" Width="350px" MaxLength="100" onkeyup="CountWords(this,'count_txt_metaTitle')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Meta Description:<br />
                                                            <span class="error_text" id='count_txt_metaDescription' style="display: none">
                                                                <span class="char"></span>
                                                                &nbsp;caratteri e&nbsp;
                                                                <span class="word"></span>
                                                                &nbsp;parole </span>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txt_metaDescription" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                            <asp:TextBox runat="server" ID="txt_metaDescription" TextMode="MultiLine" MaxLength="500" Height="70px" Width="350px" onkeyup="CountWords(this,'count_txt_metaDescription')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title"><br />
                                                            Title immagine Banner:<br />
                                                            <asp:TextBox runat="server" ID="txt_imgBannerTitle" Width="300px" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Contenuto:<br />
                                                <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="600" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                    <CssFiles>
                                                        <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                    </CssFiles>
                                                </telerik:RadEditor>
                                            </td>
                                        </tr>
                                    </table>
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
        </div>
        <div class="mainline">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                        <tr>
                            <td colspan="">
                                Fine head:<br />
                                <asp:TextBox runat="server" ID="txt_endHead" Width="350px" TextMode="MultiLine" Height="100px" />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="">
                                Fine body:<br />
                                <asp:TextBox runat="server" ID="txt_endBody" Width="350px" TextMode="MultiLine" Height="100px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
        </div>
        <div class="nulla">
        </div>
        <div class="mainline" id="pnlImg" runat="server" visible="true">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Immagine anteprima</span>
                    <div class="boxmodulo">
                        <getimg:ucgetimg id="imgPreview" runat="server" />
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Immagine banner</span>
                    <div class="boxmodulo">
                        <getimg:ucgetimg id="imgBanner" runat="server" />
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
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
