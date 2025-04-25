<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChnlAirbnbConnectExisting.aspx.cs" Inherits="MagaRentalCE.admin.modRental.ChnlAirbnbConnectExisting" MasterPageFile="~/admin/common/MP_admin.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <%-- put $ at the end of sheet name  --%>
        <asp:HiddenField ID="HF_SheetName" runat="server" Value="Sheet1$" />
    <asp:HiddenField ID="HF_main_folder" Value="originalphotos" runat="server" />
        <div style="padding: 10px;">
            Select .xls (Miicrosoft Office Excel File to Import data )
        </div>
        <div style="width: 30%; margin-top: 15px; padding: 10px;">
            <asp:FileUpload ID="ctrl_upload" runat="server" />
            <asp:Button ID="btnImportData" runat="server" OnClick="btnImportData_Click" CssClass="btn_save" Text="Import Images" />
        </div>
        <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
