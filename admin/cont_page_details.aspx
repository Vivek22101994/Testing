<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="cont_page_details.aspx.cs" Inherits="RentalInRome.admin.cont_page_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
		var _editors = ['<%=txt_description.ClientID %>'];
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
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <h1 class="titolo_main">Scheda </h1>
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">1. Dati pagina</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Nome pagina:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_page_name" Enabled="false" Width="230px" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td class="td_title">
                                        Page rewrite:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_page_rewrite" Width="230px" />
                                    </td>
                                </tr>
                            </table>
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
                                                    <span class="titoloboxmodulo">Visualizzazione</span>
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
                                                    <asp:TextBox runat="server" ID="txt_sub_title" Width="300px" />
                                                </td>
                                            </tr>
                                            <asp:PlaceHolder ID="PH_url" runat="server">
                                                <tr>
                                                    <td class="td_title">
                                                        Url:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txt_url" Width="300px" />
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                            <tr>
                                                <td colspan="2">
                                                    Sommario:<br />
                                                    <asp:TextBox runat="server" ID="txt_summary" Width="400px" TextMode="MultiLine" Height="115px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                    Descrizione:<br />
                                                    <asp:TextBox runat="server" ID="txt_description" Width="400px" TextMode="MultiLine" Height="250px" />
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
                                                <td colspan="2">
                                                    Meta Title:<br />
                                                    <asp:TextBox runat="server" ID="txt_meta_title" Width="350px" MaxLength="100" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Meta KeyWords:<br />
                                                    <asp:TextBox runat="server" ID="txt_meta_keywords" TextMode="MultiLine" MaxLength="500" Height="100px" Width="350px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Meta Description:<br />
                                                    <asp:TextBox runat="server" ID="txt_meta_description" TextMode="MultiLine" MaxLength="500" Height="150px" Width="350px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Immagine Grande</span>
                        <div class="boxmodulo">
                            <uc1:UC_get_image ID="UC_get_img_banner" runat="server" ShowCrop="true" ImgMaxWidth="405" ImgCropAspectRatio="true" ImgCropHeight="387" ImgCropWidth="405" ImgCropMaxHeight="387" ImgCropMaxWidth="405" />
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click" OnClientClick="removeTinyEditor();"><span>Modifica</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
                <asp:TextBox runat="server" ID="txt_css" Width="230px"></asp:TextBox>
                Banner Laterali:
                <asp:DropDownList ID="drp_static_collection" runat="server">
                </asp:DropDownList>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
