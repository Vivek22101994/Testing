<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="EstateExtraMacroCategoryDett.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraMacroCategoryDett" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
<script src="../js/jquery-1.4.3.min.js" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js" type="text/javascript"></script>
  <title>
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
  </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <h1 class="titolo_main">
                <%= ltrTitle.Text%>
            </h1>
        </telerik:RadCodeBlock>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');"><span>Chiudi</span></a>
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
                    <span class="titoloboxmodulo">Dati essenziali</span>
                    <div class="boxmodulo">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                            <td>
                                                Nome:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txt_code" runat="server" Width="300px"></asp:TextBox>
                                            </td>
                                        </tr>
                            <tr>
                                <td class="td_title">
                                    Attivo?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server" >
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
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
                        <getImg:UCgetImg ID="imgPreview" runat="server" />
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
                                <td colspan="2">
                                    <asp:HiddenField ID="HfLang" Value="1" runat="server" />
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
                                <td class="td_title">
                                    Nome in lingua:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_title" Width="230px" />
                                </td>
                            </tr>
                             <tr>
                                <td class="td_title">
                                    Sottotitolo:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_sub_title" Width="230px" />
                                </td>
                            </tr>

                            <tr>
                               <td class="td_title">
                                    Descrizione:
                                </td>
                                    <td>
                                    <telerik:RadEditor runat="server" StripFormattingOnPaste="AllExceptNewLines" ID="txt_description"
                                        SkinID="DefaultSetOfTools" Height="400" Width="500" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                        <CssFiles>
                                            <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                        </CssFiles>
                                    </telerik:RadEditor>
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
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>

