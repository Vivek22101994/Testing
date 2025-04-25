<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_media.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_media" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <script type="text/javascript">
        var rwdUrl = null;
        function setMediaSequence() {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
            url = "/admin/modRental/MediaSequence.aspx?id=<%=IdEstate %>";
            rwdUrl.set_autoSize(false);
            rwdUrl.set_visibleTitlebar(true);
            rwdUrl.set_minWidth(700);
            rwdUrl.setUrl(url);
            rwdUrl.show();
            rwdUrl.maximize();
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
            window.location = "/admin/rnt_estate_media.aspx?id=<%=IdEstate %>";
        }
    </script>
    <asp:HiddenField ID="HF_main_folder" Value="romeapartmentsphoto" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
	            var items = [
		            <%=ltr_folderItems.Text %>
	            ];
		
        		
	            function setAutocomplete(){
		            $( ".autoComplete" ).autocomplete({
			            source: items
		            });
	            }
            </script>
            <asp:Literal ID="ltr_folderItems" runat="server" Visible="false"></asp:Literal>
            <asp:HiddenField ID="HF_IdGalleryItem" Value="0" runat="server" />
            <asp:HiddenField ID="HF_IdVideoItem" Value="0" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <h1 class="titolo_main">Multimedia della struttura:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlFolder" runat="server" Width="100%">
                <asp:HiddenField ID="HF_assignedEstateId" Value="" runat="server" Visible="false" />
                <asp:HiddenField ID="HF_assignedEstateCode" Value="" runat="server" Visible="false" />
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
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Cartella di multimedia</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_media_folder" CssClass="autoComplete" runat="server" Width="300" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFolderError" runat="server" Text="Error" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="pnl_folderAssignedToAnotherEstate" runat="server" visible="false">
                                        <td>
                                            La cartella è abbinata alla struttura
                                            <a href="rnt_estate_media.aspx?id=<%=HF_assignedEstateId.Value %>" target="_blank">"<%=HF_assignedEstateCode.Value %>"<a />!
                                                <br />
                                            per poter abbinare a questa strutture si prega di toglere prima l'abbinamento esistente, e riprovare di nuovo
                                        </td>
                                    </tr>
                                    <tr id="pnl_folderNotExist" runat="server" visible="false">
                                        <td>
                                            La cartella "<%=txt_media_folder.Text %>" non esiste, vuole crearla?
                                            <br />
                                            <asp:LinkButton ID="lnk_createFolderOK" runat="server" OnClick="lnk_createFolderOK_Click">SI</asp:LinkButton>
                                            <br />
                                            <asp:LinkButton ID="lnk_createFolderNO" runat="server" OnClick="lnk_createFolderNO_Click">NO</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr id="pnl_folderSave" runat="server">
                                        <td>
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_saveFolder" runat="server" OnClick="lnk_saveFolder_Click"><span>Salva Cartella</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_cancelFolder" runat="server" OnClick="lnk_cancelFolder_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_changeFolder" runat="server" OnClick="lnk_changeFolder_Click"><span>Togli abbinamento delle foto e la cartella</span></asp:LinkButton>
                                                </div>
                                            </div>
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
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlContent" runat="server" Width="100%">
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo">
                                <asp:Literal ID="ltrl_titolo" runat="server">Gallery</asp:Literal></span>
                            <div class="boxmodulo">
                                <div class="salvataggio" style="margin-bottom: 0px;">
                                    <div class="bottom_salva">
                                        <a href="#" onclick="setMediaSequence(); return false;">
                                            <span>Cambia ordine delle foto</span>
                                        </a>
                                    </div>
                                    <div class="bottom_salva">
                                        <asp:LinkButton ID="lnk_newGalleryItem" runat="server" OnClick="lnk_newGalleryItem_Click"><span>Aggiungi Foto</span></asp:LinkButton>
                                    </div>
                                    <div class="bottom_salva">
                                        <asp:LinkButton ID="lnk_bindGalleryItems" runat="server" OnClick="lnk_bindGalleryItems_Click"><span>Abbina le foto dalla cartella</span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <asp:Panel ID="pnl_editGalleryItem" runat="server" Visible="false" CssClass="boxmodulo">
                                <table>
                                    <tr>
                                        <td style="width: 100px; text-align: left; vertical-align: top;">
                                            Titolo:
                                        </td>
                                        <td style="width: 220px;">
                                            <asp:TextBox ID="txt_codeGalleryItem" Width="210px" MaxLength="50" runat="server"></asp:TextBox>
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_codeGalleryItem" ValidationGroup="GalleryItem" runat="server" ErrorMessage="// inserire titolo"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 20px;" rowspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Anteprima:
                                        </td>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_thumbGalleryItem" runat="server" ShowCrop="false" ImgCropWidth="177" ImgCropHeight="94" ImgCropMaxWidth="177" ImgCropMaxHeight="94" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Grande:
                                        </td>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_bannerGalleryItem" runat="server" ShowCrop="false" ImgCropWidth="700" ImgCropHeight="372" ImgCropMaxWidth="700" ImgCropMaxHeight="372" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div class="salvataggio" style="margin: 0px;">
                                                <div class="bottom_salva" style="padding-left: 0;">
                                                    <asp:LinkButton ID="lnk_saveGalleryItem" runat="server" CausesValidation="true" ValidationGroup="GalleryItem" OnClick="lnk_saveGalleryItem_Click"><span>Salva</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_cancelGalleryItem" runat="server" OnClick="lnk_cancelGalleryItem_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-top: 20px;">Elenco Foto</span>
                            <div class="boxmodulo">
                                <asp:ListView ID="LV_gallery" runat="server" DataSourceID="LDS_gallery" OnItemCommand="LV_gallery_ItemCommand">
                                    <ItemTemplate>
                                        <div style="height: 90px; width: 60px; border: solid 1px; margin: 2px; float: left;">
                                            <a href="javascript:openShadowboxImg('/<%# Eval("img_banner") %>')" title="<%# Eval("code") %>" style="cursor: pointer; float: left;">
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <img alt="" width="60px" src="/<%# Eval("img_thumb") %>" />
                                            </a>
                                            <asp:Label ID="Label1" Visible="false" runat="server" Text='<%# Eval("id") %>' /><br />
                                            <asp:LinkButton ID="LinkButton2" CommandName="elimina" runat="server" OnClientClick="return confirm('vuoi eliminare la Photo?')">elimina</asp:LinkButton><br />
                                            <asp:LinkButton ID="LinkButton1" CommandName="seleziona" runat="server">modifica</asp:LinkButton>
                                            <div>
                                                <asp:LinkButton ID="LinkButton3" CommandName="move_up" runat="server"><<</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="LinkButton4" CommandName="move_down" runat="server">>></asp:LinkButton>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <div id="itemPlaceholderContainer" runat="server" style="float: left;">
                                            <a id="itemPlaceholder" runat="server" />
                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:LinqDataSource ID="LDS_gallery" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_ESTATE_MEDIAs" Where="pid_estate == @pid_estate &amp;&amp; type == @type" OrderBy="sequence">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="HF_IdEstate" Name="pid_estate" PropertyName="Value" Type="Int32" />
                                        <asp:Parameter DefaultValue="gallery" Name="type" Type="String" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
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
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo">
                                <asp:Literal ID="Literal1" runat="server">Video</asp:Literal></span>
                            <div class="boxmodulo">
                                <div class="salvataggio" style="margin-bottom: 0px;">
                                    <div class="bottom_salva">
                                        <asp:LinkButton ID="lnk_newVideoItem" runat="server" OnClick="lnk_newVideoItem_Click"><span>Aggiungi Video</span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <asp:Panel ID="pnl_editVideoItem" runat="server" Visible="false" CssClass="boxmodulo">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_video_embed" runat="server" TextMode="MultiLine" Width="500" Height="300"></asp:TextBox>
                                            <asp:Literal ID="ltr_video_embed" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="salvataggio" style="margin: 0px;">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_changeVideoItem" runat="server" OnClick="lnk_changeVideoItem_Click"><span>Cambia</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_saveVideoItem" runat="server" CausesValidation="true" ValidationGroup="VideoItem" OnClick="lnk_saveVideoItem_Click"><span>Salva</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_cancelVideoItem" runat="server" OnClick="lnk_cancelVideoItem_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_closeVideoItem" runat="server" OnClick="lnk_closeVideoItem_Click"><span>Chiudi</span></asp:LinkButton>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <div class="center">
                            <span class="titoloboxmodulo" style="margin-top: 20px;">Elenco Video</span>
                            <div class="boxmodulo">
                                <asp:ListView ID="LV_video" runat="server" DataSourceID="LDS_video" OnItemCommand="LV_video_ItemCommand">
                                    <ItemTemplate>
                                        <div style="border: 1px solid; float: left; height: 50px; margin: 2px; padding: 5px; width: 50px;">
                                            Video
                                            <%# Container.DataItemIndex+1 %>
                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' /><br />
                                            <asp:LinkButton ID="LinkButton2" CommandName="elimina" runat="server" OnClientClick="return confirm('vuoi eliminare la Video?')">elimina</asp:LinkButton><br />
                                            <asp:LinkButton ID="LinkButton1" CommandName="seleziona" runat="server" >modifica</asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <div id="itemPlaceholderContainer" runat="server" style="float: left;">
                                            <a id="itemPlaceholder" runat="server" />
                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:LinqDataSource ID="LDS_video" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_ESTATE_MEDIAs" Where="pid_estate == @pid_estate &amp;&amp; type == @type">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="HF_IdEstate" Name="pid_estate" PropertyName="Value" Type="Int32" />
                                        <asp:Parameter DefaultValue="video" Name="type" Type="String" />
                                    </WhereParameters>
                                </asp:LinqDataSource>
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
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Anteprima</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_preview_1" runat="server" ShowCrop="false" />
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
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Anteprima</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_preview_2" runat="server" ShowCrop="false" />
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
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Anteprima</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_preview_3" runat="server" ShowCrop="false" />
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
                </div>
                <div class="nulla">
                </div>
                <div class="mainline">
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Banner</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_banner" runat="server" ShowCrop="false" />
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
                    <div class="nulla">
                    </div>
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Percorso video per mobile (.mp4)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            es: http://www.rentalinrome.com/video/rome-parioli_archimede-penthouse.mp4
                                            <br/>
                                            <asp:TextBox ID="txt_mobileVideoFilePath" runat="server" MaxLength="250" Width="600"></asp:TextBox>
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
                    <div class="mainbox" runat="server" visible="false">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span class="titoloboxmodulo">Anteprima</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:UC_get_image ID="UC_img_thumb" runat="server" />
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
                </div>
                <div class="nulla">
                </div>
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click" CausesValidation="true" TabIndex="28" ValidationGroup="price"><span>Salva Modifiche</span></asp:LinkButton>
                    </div>
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <div class="nulla" style="height: 30px;">
                </div>
            </asp:Panel>

            <script type="text/javascript">
                
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
