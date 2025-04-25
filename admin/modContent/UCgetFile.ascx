<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCgetFile.ascx.cs" Inherits="ModContent.admin.modContent.UCgetFile" %>
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
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
    <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <asp:HiddenField ID="HF_filePath" Value="" runat="server" />
    <asp:HiddenField ID="HF_fileName" Value="" runat="server" />
    <asp:HiddenField ID="HF_fileExtension" Value="" runat="server" />
    <asp:HiddenField ID="HF_root" Value="images" runat="server" />
    <table>
        <tr>
            <td class="ruCustom">
                <span class="invalid"></span>
                <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" MaxFileInputsCount="1" OnFileUploaded="AsyncUpload1_FileUploaded" AllowedFileExtensions="jpeg,jpg,gif,png,bmp,pdf,ics">
                    <Localization Select="Carica File" />
                </telerik:RadAsyncUpload>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox runat="server" ID="txt_file" Width="230px" ReadOnly="true" onfocus="this.select()" />
                 <asp:HyperLink ID="HL" runat="server" Target="_blank"><img src="/images/css-common/icoFile_<%= FileExtension.Replace(".", "") %>.png" height="20" style=" border: none;" alt="<%= FileExtension.Replace(".", "") %>" /></asp:HyperLink>
           </td>
        </tr>
    </table>
    <div class="nulla">
    </div>
</telerik:RadAjaxPanel>
<div class="nulla">
</div>
