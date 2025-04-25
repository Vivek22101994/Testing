<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="util_MailErrorLogDett.aspx.cs" Inherits="RentalInRome.admin.util_MailErrorLogDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Rental In Rome - Dettagli Errore</title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
        @import url(/css/common.css);
    </style>
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CloseRadWindow(arg) {
            rw = GetRadWindow();
            if (rw)
                rw.close(arg);
            else
                window.location = "/admin/";
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
    <asp:UpdateProgress ID="loading_cont" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <%="<div id=\"loading_big_cont\"></div><div id=\"overlay_site\"></div>" %>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="main">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div class="salvataggio">
                <div class="bottom_salva">
                    <a onclick="return CloseRadWindow('reload');">
                        <span>Chiudi</span></a>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_resend" runat="server" OnClick="lnk_resend_Click"><span>Reinvia</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox">
                    <div class="top">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="ltr_isResent" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Data:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_Date" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        IP:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_IP" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        tipo mail:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_mailDescrtiption" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Subject:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_Subject" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Mittente:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_From" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Destinatario:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_TOs" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="ltr_Body" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox runat="server" ID="txt_Value" Width="800px" Height="300px" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </div>
    </form>
</body>
</html>
