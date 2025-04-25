<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="loc_point_details.aspx.cs" Inherits="RentalInRome.admin.loc_point_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var _editors = []; //['<%=txt_description.ClientID %>'];
		function removeTinyEditor() {
			//removeTinyEditors(_editors);
		}
		function setTinyEditor(IsReadOnly) {
			//setTinyEditors(_editors, IsReadOnly);
		}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_isNew" Value="1" runat="server" />
            <asp:HiddenField ID="HF_gmaps_coords" runat="server" />
            <h1 class="titolo_main">Scheda </h1>
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
                    <a href="loc_point_gmap.aspx?id=<%=HF_id.Value %>">
                        <span>Configura GoogleMaps</span></a>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span></a>
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
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">1. Dati pagina</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td>
                                        Attivo:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_is_active" runat="server" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td>
                                        Tipologia:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_point_type" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td>
                                        Citta:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_city" Width="120px" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td>
                                        HA Place Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_placeType" Width="120px">
                                            <asp:ListItem Text="-seleziona-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="AIRPORT" Value="AIRPORT"></asp:ListItem>
                                            <asp:ListItem Text="BEACH" Value="BEACH"></asp:ListItem>
                                            <asp:ListItem Text="GOLF" Value="GOLF"></asp:ListItem>
                                            <asp:ListItem Text="RESTAURANT" Value="RESTAURANT"></asp:ListItem>
                                            <asp:ListItem Text="TRAIN" Value="TRAIN"></asp:ListItem>
                                            <asp:ListItem Text="BAR" Value="BAR"></asp:ListItem>
                                            <asp:ListItem Text="FERRY" Value="FERRY"></asp:ListItem>
                                            <asp:ListItem Text="HIGHWAY" Value="HIGHWAY"></asp:ListItem>
                                            <asp:ListItem Text="SKI" Value="SKI"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                                    <table>
                                        <tr>
                                            <td class="td_title">
                                                Nome:
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txt_title" Width="300px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="0" cellspacing="10" runat="server" visible="false">
                                        <tr>
                                            <td style="border-right: medium ridge;">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click">copia</asp:LinkButton>
                                                            &nbsp; &nbsp; &nbsp;
                                                            <asp:LinkButton ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click">incolla</asp:LinkButton>
                                                            <asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Visualizzazione</span>
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
                                                            <span class="error_text" id='count_txt_meta_title' style="display: none">
                                                                <span class="char"></span>
                                                                &nbsp;caratteri e&nbsp;
                                                                <span class="word"></span>
                                                                &nbsp;parole </span>
                                                            <br />
                                                            <asp:TextBox runat="server" ID="txt_meta_title" Width="350px" MaxLength="100" onkeyup="CountWords(this,'count_txt_meta_title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Meta KeyWords:<br />
                                                            <span class="error_text" id='count_txt_meta_keywords' style="display: none">
                                                                <span class="char"></span>
                                                                &nbsp;caratteri e&nbsp;
                                                                <span class="word"></span>
                                                                &nbsp;parole </span>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="txtConclusionValidator1" ControlToValidate="txt_meta_keywords" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                            <asp:TextBox runat="server" ID="txt_meta_keywords" TextMode="MultiLine" MaxLength="500" Height="100px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_keywords')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            Meta Description:<br />
                                                            <span class="error_text" id='count_txt_meta_description' style="display: none">
                                                                <span class="char"></span>
                                                                &nbsp;caratteri e&nbsp;
                                                                <span class="word"></span>
                                                                &nbsp;parole </span>
                                                            <br />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txt_meta_description" Display="Dynamic" Text="// 500 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,500}$" ValidationGroup="dati" runat="server" />
                                                            <asp:TextBox runat="server" ID="txt_meta_description" TextMode="MultiLine" MaxLength="500" Height="150px" Width="350px" onkeyup="CountWords(this,'count_txt_meta_description')" />
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
                            <uc1:UC_get_image ID="UC_get_img_preview" runat="server" ShowCrop="true" ImgMaxWidth="405" ImgCropAspectRatio="true" ImgCropHeight="387" ImgCropWidth="405" ImgCropMaxHeight="387" ImgCropMaxWidth="405" ImgRoot="images/travelpoints" />
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
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
            
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
