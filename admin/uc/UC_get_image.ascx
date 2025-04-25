<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_get_image.ascx.cs" Inherits="RentalInRome.admin.uc.UC_get_image" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="HF_unique" Value="" runat="server" />
        <asp:HiddenField ID="HF_show_img" Value="1" runat="server" />
        <asp:HiddenField ID="HF_show_path" Value="1" runat="server" />
        <asp:HiddenField ID="HF_show_crop" Value="1" runat="server" />
        <asp:HiddenField ID="HF_img" Value="" runat="server" />
        <asp:HiddenField ID="HF_root" Value="images" runat="server" />
        <asp:HiddenField ID="HF_imgWidth" Value="" runat="server" />
        <asp:HiddenField ID="HF_imgHeight" Value="" runat="server" />
        <asp:HiddenField ID="HF_imgCropWidth" Value="" runat="server" />
        <asp:HiddenField ID="HF_imgCropHeight" Value="" runat="server" />
        <asp:HiddenField ID="HF_imgCropMaxWidth" Value="null" runat="server" />
        <asp:HiddenField ID="HF_imgCropMaxHeight" Value="null" runat="server" />
        <asp:HiddenField ID="HF_imgCropAspectRatio" Value="0" runat="server" />
        <asp:HiddenField ID="HF_imgCropContHeight" Value="300" runat="server" />
        <asp:HiddenField ID="HF_imgCropContWidth" Value="400" runat="server" />
        <asp:HiddenField ID="HF_imgCropIsVertical" Value="false" runat="server" />
        <table>
            <asp:PlaceHolder ID="PH_show_path" runat="server" Visible="false">
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txt_img" Width="230px" ReadOnly="true" />
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
            <tr>
                <td>
                    <asp:HyperLink ID="HL_select" runat="server">seleziona</asp:HyperLink>
                    <br />
                    <asp:HyperLink ID="HL_crop" runat="server">crop</asp:HyperLink>
            </tr>
        </table>
        <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" OnClick="btn_page_update_Click" />
    </ContentTemplate>
</asp:UpdatePanel>
