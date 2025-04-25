<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHA_adContent.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHA_adContent" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHAEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tabsTop.tabsChannelsTop table td a[title="Ad content"], #tabsHomeaway.tabsTop table td a[title="Ad content"]  {
	        background:#848484;
	        border-color:#606060;
	        color:#FFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/atraveo-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with Atraveo" />

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_TestiStrutturaLingue")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>

            <div class="copiaIncolla">
                <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click" CssClass="btnCopia"><%= contUtils.getLabel("lbl_Copy")%></asp:LinkButton>
                <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click" CssClass="btnCopia"><%= contUtils.getLabel("lblPaste")%></asp:LinkButton><asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
            </div>

            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span><%= contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span><%= contUtils.getLabel("lblCancelChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span><%= contUtils.getLabel("lblTornaElenco")%></span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline mainChannels mainHomeAway mainAdContent">
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
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("abbr") %>' />
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
                                            <td valign="top">
                                                <table cellpadding="3" cellspacing="0">

                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo"><%= contUtils.getLabel("lbl_Visualizzazione")%></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Accommodation Summary:<span class="error_text" id='count_txt_summary' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            Max 80 chars
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" onkeyup="CountWords(this,'count_txt_summary')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Description:<br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            Headline:<span class="error_text" id='count_txt_title' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                            <br />
                                                            Min 20 chars
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_title" MaxLength="100" Style="width: 99%; margin-left: 0; float: left;" onkeyup="CountWords(this,'count_txt_title')" />
                                                        </td>
                                                    </tr>

                                                </table>
                                                    <table cellpadding="3" cellspacing="0">
                                                        <tr>
                                                            <td colspan="2">
                                                                <span class="titoloboxmodulo">Addtional Data VRBO</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Listing Story:<br />
                                                                <telerik:RadEditor runat="server" ID="re_listing_story" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                    <CssFiles>
                                                                        <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                    </CssFiles>
                                                                </telerik:RadEditor>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Unique Benifits:<br />
                                                                <telerik:RadEditor runat="server" ID="re_unique_benifits" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                    <CssFiles>
                                                                        <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                    </CssFiles>
                                                                </telerik:RadEditor>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Why Purchased?:<br />
                                                                <telerik:RadEditor runat="server" ID="re_why_purchased" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                    <CssFiles>
                                                                        <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                    </CssFiles>
                                                                </telerik:RadEditor>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td colspan="2">Rate Notes:<br />
                                                                <telerik:RadEditor runat="server" ID="re_rate_notes" SkinID="DefaultSetOfTools" Height="400" Width="800" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                    <CssFiles>
                                                                        <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                    </CssFiles>
                                                                </telerik:RadEditor>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Year purchased</td>
                                                            <td>
                                                                <asp:TextBox ID="txt_year" runat="server"></asp:TextBox>
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

