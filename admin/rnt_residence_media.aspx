<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_residence_media.aspx.cs" Inherits="RentalInRome.admin.rnt_residence_media" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="~/admin/uc/UC_rnt_residence_navlinks.ascx" TagName="UC_rnt_residence_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_main_folder" Value="romeapartmentsphoto" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdGalleryItem" Value="0" runat="server" />
            <asp:HiddenField ID="HF_IdResidence" Value="0" runat="server" />
            <h1 class="titolo_main">Multimedia della residenza:
            <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_residence_navlinks ID="UC_rnt_residence_navlinks1" runat="server" />
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlFolder" runat="server" Width="100%">
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
                                            <asp:TextBox ID="txt_media_folder" runat="server" Width="300" MaxLength="500"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFolderError" runat="server" Text="Error" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_saveFolder" runat="server" OnClick="lnk_saveFolder_Click"><span>Salva Cartella</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_cancelFolder" runat="server" OnClick="lnk_cancelFolder_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_changeFolder" runat="server" OnClick="lnk_changeFolder_Click"><span>Cambia Cartella</span></asp:LinkButton>
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
                                        <asp:LinkButton ID="lnk_newGalleryItem" runat="server" OnClick="lnk_newGalleryItem_Click"><span>Aggiungi Foto</span></asp:LinkButton>
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
                                            <a href="/<%# Eval("img_banner") %>" rel="shadowbox;" title="<%# Eval("code") %>" style="cursor: pointer; float: left;">
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <img alt="" width="60px" src="/<%# Eval("img_thumb") %>" />
                                            </a>
                                            <asp:Label ID="Label1" Visible="false" runat="server" Text='<%# Eval("id") %>' /><br />
                                            <asp:LinkButton ID="LinkButton2" CommandName="elimina" runat="server" OnClientClick="return confirm('vuoi eliminare la Photo?')">elimina</asp:LinkButton><br />
                                            <asp:LinkButton ID="LinkButton1" CommandName="seleziona" runat="server">modifica</asp:LinkButton>
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
                                <asp:LinqDataSource ID="LDS_gallery" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_RESIDENCE_MEDIAs" Where="pid_residence == @pid_residence &amp;&amp; type == @type">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="HF_IdResidence" Name="pid_residence" PropertyName="Value" Type="Int32" />
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
                            <span class="titoloboxmodulo">Video</span>
                            <div class="boxmodulo">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="salvataggio">
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_change_video" runat="server" OnClick="lnk_change_video_Click"><span>Cambia</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_save_video" runat="server" OnClick="lnk_save_video_Click"><span>Salva</span></asp:LinkButton>
                                                </div>
                                                <div class="bottom_salva">
                                                    <asp:LinkButton ID="lnk_cancel_video" runat="server" OnClick="lnk_cancel_video_Click"><span>Annulla</span></asp:LinkButton>
                                                </div>
                                                <div class="nulla">
                                                </div>
                                            </div>
                                            <div class="nulla">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_video_embed" runat="server" TextMode="MultiLine" Width="500" Height="300"></asp:TextBox>
                                            <asp:Literal ID="ltr_video_embed" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                                <div class="nulla">
                                </div>
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
                    <div id="Div1" class="mainbox" runat="server" visible="false">
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
