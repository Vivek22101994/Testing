<%@ Page Title="" Language="C#" MasterPageFile="/admin/common/mp_AdminNoMenu.Master" AutoEventWireup="true" CodeBehind="EstateInternsSubTypeDett.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateInternsSubTypeDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <title>
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <h1 class="titolo_main"><%# contUtils.getLabel("lblDett", App.LangID,"Scheda") %> </h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span><%# contUtils.getLabel("lblSaveChanges", App.LangID,"Salva Modifiche") %></span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span><%# contUtils.getLabel("lblCancelChanges", App.LangID,"Annulla Modifiche") %></span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span><%# contUtils.getLabel("lblCalClose", App.LangID,"Chiudi") %></span></a>
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
                                <td class="td_title"><%# contUtils.getLabel("lblCode", App.LangID,"Codice") %>:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_code" Width="230px" />
                                </td>
                                <td style="width: 20px"></td>
                                <td class="td_title" style="width: 40px"><%# contUtils.getLabel("lblActive", App.LangID,"Attivo") %>?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_isActive" runat="server" DataSource='<%# CommonUtilities.GetIsActiveList() %>'
                                        DataTextField="Value" DataValueField="Key">
                                    </asp:DropDownList>
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
                                            <td style="vertical-align: top;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo"> <%# contUtils.getLabel("lbl_Visualizzazione", App.LangID,"Visualizzazione") %></span>
                                                        </td>
                                                    </tr>       
                                                    <tr>
                                                        <td class="td_title">
                                                            <%# contUtils.getLabel("Lbltitle", App.LangID,"Titolo") %>:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_title" Width="300px" />
                                                        </td>
                                                    </tr>                                                                                              
                                                </table>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2"><%# contUtils.getLabel("lblContent", App.LangID,"Contenuto") %>:<br />
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
        <div class="nulla">
        </div>       
    </telerik:RadAjaxPanel>
</asp:Content>
