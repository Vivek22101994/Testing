<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RentalInRome.areariservataprop.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../admin/css/adminStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="HF_referer" Value="default.aspx" runat="server" />
    <div class="mainline" style="width: 100%; text-align: center;">
        <!-- BOX 1 -->
        <div class="mainbox" style="width: 310px; float: none;">
            <div class="top">
                <div style="float: left;">
                    <img src="../admin/images/mainbox1.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="../admin/images/mainbox2.gif" width="10" height="10" alt="" /></div>
            </div>
            <div class="center">
                <img src="images/logo_rentalinrome_areaprop.gif" width="280" alt="" />
                <div class="boxmodulo">
                    <table width="366">
                        <tr>
                            <td colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td width="68" align="right">
                                &nbsp;
                            </td>
                            <td colspan="2" class="red">
                                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Text="* nome o password errato" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Login:
                            </td>
                            <td width="159">
                                <asp:TextBox ID="txtUser" runat="server" MaxLength="50" ValidationGroup="login" Width="200px"></asp:TextBox>
                            </td>
                            <td width="123" class="red">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUser" ErrorMessage="*inserisci nome" ValidationGroup="login"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Password:
                            </td>
                            <td width="159">
                                <asp:TextBox ID="txtPwd" runat="server" MaxLength="50" TextMode="Password" ValidationGroup="login" Width="200px"></asp:TextBox>
                            </td>
                            <td class="red">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPwd" ErrorMessage="*inserisci password" ValidationGroup="login"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSend" runat="server" Text="invia" ValidationGroup="login" OnClick="btnSend_Click" />
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="annulla" ToolTip="Ritorna al sito principale" OnClick="btnCancel_Click" />
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="bottom">
                <div style="float: left;">
                    <img src="../admin/images/mainbox3.gif" width="10" height="10" alt="" /></div>
                <div style="float: right;">
                    <img src="../admin/images/mainbox4.gif" width="10" height="10" alt="" /></div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
