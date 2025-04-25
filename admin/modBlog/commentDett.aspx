<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="commentDett.aspx.cs" Inherits="ModBlog.admin.modBlog.commentDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:TextBox runat="server" ID="txt_pageRewrite" Visible="false" Text="pg_blogCategoryDett.aspx?" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal></h1>
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
                                    Approvato?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server">
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                        <asp:ListItem Value="1">SI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Sesso
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_typeCode" runat="server">
                                        <asp:ListItem Value="m">Maschio</asp:ListItem>
                                        <asp:ListItem Value="f">Femmina</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Nome Cognome:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_nameFull" Width="300px" MaxLength="200" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    E-mail:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_email" Width="300px" MaxLength="200" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Data Commento:
                                </td>
                                <td>
                                    <telerik:RadDateTimePicker ID="rdtp_commentDate" runat="server" ZIndex="99999">
                                        <DateInput DateFormat="dd/MM/yyyy HH:mm">
                                        </DateInput>
                                    </telerik:RadDateTimePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rdtp_commentDate" CssClass="errore_form" ValidationGroup="dati" Display="Dynamic" ErrorMessage="<br/>// obbligatorio"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Articolo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_pidArticle" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Somamrio:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_commentSubject" Width="300px" MaxLength="200" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Commento:<br />
                                    <telerik:RadEditor runat="server" ID="re_commentBody" SkinID="DefaultSetOfTools" Height="200" Width="400" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                                        <CssFiles>
                                            <telerik:EditorCssFile Value="" />
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
            <div class="nulla">
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
