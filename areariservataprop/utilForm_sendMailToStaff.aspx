<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="utilForm_sendMailToStaff.aspx.cs" Inherits="RentalInRome.areariservataprop.utilForm_sendMailToStaff" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <uc1:uc_loader id="UC_loader1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1 class="titolo_main">
                Invia messaggio allo Staff di RentalInRome</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="salvataggio" style="width: auto;">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_send" runat="server" ValidationGroup="dati" OnClick="lnk_send_Click"><span>Invia</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="#" onclick="parent.Shadowbox.close()"><span>Annulla</span></a>
                </div>
                <div class="nulla">
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
                            <table>
                                <tr>
                                    <td style="width:80px;">
                                        Da:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_from" runat="server" Width="300" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Oggetto:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_subject" runat="server" Width="300" Text="Richiesta modifica alla struttura: "></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Messaggio:<br />
                                        <asp:TextBox ID="txt_body" runat="server" Width="384" Height="200" TextMode="MultiLine"></asp:TextBox>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
