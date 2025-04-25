<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCgetImg.ascx.cs" Inherits="ModContent.admin.modContent.UCgetImg" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
.ruCustom .RadUpload_Default .ruFakeInput {
    display: none;
}
.ruCustom .RadUpload_Default .ruBrowse {
    background-position: 0 -46px;
    width: 115px;
}
.ruCustom .RadUpload_Default .ruStyled .ruFileInput {
    cursor: pointer;
}
</style>
<telerik:RadAjaxPanel ID="rapMain" runat="server" OnAjaxRequest="rapMain_AjaxRequest">
    <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <asp:HiddenField ID="HF_show_img" Value="1" runat="server" />
    <asp:HiddenField ID="HF_show_path" Value="1" runat="server" />
    <asp:HiddenField ID="HF_imgPath" Value="" runat="server" />
    <asp:HiddenField ID="HF_imgPathDef" Value="" runat="server" />
    <asp:HiddenField ID="HF_imgName" Value="" runat="server" />
    <asp:HiddenField ID="HF_imgExtension" Value="" runat="server" />
    <asp:HiddenField ID="HF_root" Value="images" runat="server" />
    <table>
        <asp:PlaceHolder ID="PH_show_upload" runat="server" Visible="false">
            <tr>
                <td class="ruCustom">
                    <span class="invalid"></span>
                    <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" MaxFileInputsCount="1" OnFileUploaded="AsyncUpload1_FileUploaded" AllowedFileExtensions="jpeg,jpg,gif,png,bmp">
                        <Localization Select="Carica immagine" />
                    </telerik:RadAsyncUpload>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_show_dialog" runat="server">
            <tr>
                <td>
                    <telerik:DialogOpener runat="server" ID="DialogOpener1"></telerik:DialogOpener>
                    <telerik:RadButton ID="btnDialogOpen" runat="server" Text="Apri ImageManager">
                    </telerik:RadButton>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_show_path" runat="server" Visible="false">
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txt_img" Width="230px" ReadOnly="true" />
                    <asp:LinkButton ID="btn_clearImg" runat="server" OnClick="btn_clearImg_Click" OnClientClick="return confirm('Vuoi eliminare immagine?');"><img src="/images/ico/ico_del.gif" /></asp:LinkButton>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PH_show_img" runat="server" Visible="false">
            <tr>
                <td>
                    <asp:Image ID="img" runat="server" AlternateText="Immagine assente" />
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
    <div class="nulla">
    </div>
</telerik:RadAjaxPanel>
<div class="nulla">
</div>
