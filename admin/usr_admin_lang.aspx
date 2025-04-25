<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_admin_lang.aspx.cs" Inherits="RentalInRome.admin.usr_admin_lang" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HF_main_folder" Value="images/staff/staff_lang" runat="server" />
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
            <asp:HiddenField ID="HF_IdAdmin" Value="0" runat="server" />
            <h1 class="titolo_main">
                Gestione lingue del account:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <a href="usr_admin_role.aspx">
                        <span>Torna nel elenco</span></a>
                </div>
                <div class="nulla">
                </div>
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
                                            La cartella è abbinata a un'altro account
                                            <a href="usr_admin_lang.aspx?id=<%=HF_assignedEstateId.Value %>" target="_blank">"<%=HF_assignedEstateCode.Value %>"<a />!
                                                <br />
                                            per poter abbinare a questo account si prega di toglere prima l'abbinamento esistente, e riprovare di nuovo
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
                                <asp:Literal ID="ltrl_titolo" runat="server">Lingue</asp:Literal></span>
                            <div class="boxmodulo">
                                <div class="salvataggio" style="margin-bottom: 0px;">
                                    <div class="bottom_salva">
                                        <asp:LinkButton ID="lnk_newGalleryItem" runat="server" OnClick="lnk_newGalleryItem_Click"><span>Aggiungi Lingua</span></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <asp:Panel ID="pnl_editGalleryItem" runat="server" Visible="false" CssClass="boxmodulo">
                                <table>
                                    <tr>
                                        <td style="width: 100px; text-align: left; vertical-align: top;">
                                            Lingua:
                                        </td>
                                        <td style="width: 220px;">
                                            <asp:DropDownList runat="server" ID="drp_lang" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
                                            </asp:DropDownList>
                                            <asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
                                            </asp:LinqDataSource>
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
                            <span class="titoloboxmodulo" style="margin-top: 20px;">Elenco Lingue</span>
                            <div class="boxmodulo">
                                <asp:ListView ID="LV_gallery" runat="server" DataSourceID="LDS_gallery" OnItemCommand="LV_gallery_ItemCommand">
                                    <ItemTemplate>
                                        <div style="height: 110px; width: 60px; border: solid 1px; margin: 2px; float: left;">
                                            <%# contUtils.getLang_title(Eval("pid_lang").objToInt32())%>
                                            <img alt="" width="60px" src="/<%# Eval("img_thumb") %>" />
                                            <asp:Label ID="lbl_pid_lang" Visible="false" runat="server" Text='<%# Eval("pid_lang") %>' />
                                            <asp:LinkButton ID="LinkButton2" CommandName="elimina" runat="server" OnClientClick="return confirm('vuoi eliminare la Lingua?')">elimina</asp:LinkButton><br />
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
                                <asp:LinqDataSource ID="LDS_gallery" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_RL_ADMIN_LANG" Where="pid_admin == @pid_admin" OrderBy="sequence">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="HF_IdAdmin" Name="pid_admin" PropertyName="Value" Type="Int32" />
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
                <div class="salvataggio" runat="server" visible="false">
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
