<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_text.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_text" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _editors = [];
        function removeTinyEditor() {
            removeTinyEditors(_editors);
        }
        function setTinyEditor(IsReadOnly) {
            setTinyEditors(_editors, IsReadOnly);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_updater_newId" Value="0" runat="server" />
            <asp:HiddenField ID="HF_updater_args" Value="0" runat="server" />
            <h1 class="titolo_main">Scheda Struttura</h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio hiddenbeforload" style="display: none;">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva e torna nella lista</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>"><span>Torna nella lista senza salvare</span> </a>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                    <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" OnClientClick="removeTinyEditor()" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <span>No data was returned.</span>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <table cellpadding="0" cellspacing="10">
                                        <tr>
                                            <td style="border-right: medium ridge;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click">copia</asp:LinkButton>&nbsp; &nbsp; &nbsp;
                                                            <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click">incolla</asp:LinkButton><asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Visualizzazione</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">Nome in Lingua:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_title" MaxLength="100" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">Caratteristica:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txt_sub_title" MaxLength="200" Width="300px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Sommario nel elenco:<br />
                                                            <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            Descrizione:<br />
                                                            <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="250" Width="400" ToolbarMode="ShowOnFocus" StripFormattingOptions="AllExceptNewLines" ToolsFile="/admin/common/ToolsFileLimited.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0px;">Testi per HomeAway</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">HeadLine (Titolone):<br />
                                                            <asp:TextBox runat="server" ID="txt_headLine" Width="400px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Description (Contenuto):<br />
                                                            <telerik:RadEditor runat="server" ID="re_haDescription" SkinID="DefaultSetOfTools" Height="250" Width="400" ToolbarMode="ShowOnFocus" StripFormattingOptions="AllExceptNewLines">
                                                                <Tools>
                                                                    <telerik:EditorToolGroup>
                                                                    </telerik:EditorToolGroup>
                                                                </Tools>
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Other Activities:<br />
                                                            <telerik:RadEditor runat="server" ID="re_haOtherActivities" SkinID="DefaultSetOfTools" Height="100" Width="400" ToolbarMode="ShowOnFocus" StripFormattingOptions="AllExceptNewLines">
                                                                <Tools>
                                                                    <telerik:EditorToolGroup>
                                                                    </telerik:EditorToolGroup>
                                                                </Tools>
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Note per i Prezzi:<br />
                                                            <telerik:RadEditor runat="server" ID="re_rateNotes" SkinID="DefaultSetOfTools" Height="100" Width="400" ToolbarMode="ShowOnFocus" StripFormattingOptions="AllExceptNewLines">
                                                                <Tools>
                                                                    <telerik:EditorToolGroup>
                                                                    </telerik:EditorToolGroup>
                                                                </Tools>
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="border-left: medium ridge;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Motori Ricerca</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Meta Title:<br />
                                                            <span class="error_text" id='count_txt_meta_title' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_meta_title" Width="350px" MaxLength="100" onkeyup="CountWords(this,'count_txt_meta_title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Meta KeyWords:<br />
                                                            <span class="error_text" id='count_txt_meta_keywords' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="txtConclusionValidator1" ControlToValidate="txt_meta_keywords" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                            <asp:TextBox runat="server" ID="txt_meta_keywords" TextMode="MultiLine" MaxLength="500" Height="100px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_keywords')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Meta Description:<br />
                                                            <span class="error_text" id='count_txt_meta_description' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txt_meta_description" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                            <asp:TextBox runat="server" ID="txt_meta_description" TextMode="MultiLine" MaxLength="500" Height="150px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_description')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo" style="margin-top: 20px; margin-bottom: 0;">Descrizione per mobile</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Attenzione! Non inserire i tag html<br />
                                                            <asp:TextBox runat="server" ID="txt_mobileDescription" TextMode="MultiLine" Height="250px" Width="350px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
