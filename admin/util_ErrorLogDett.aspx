<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="util_ErrorLogDett.aspx.cs" Inherits="RentalInRome.admin.util_ErrorLogDett" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Rental In Rome - Dettagli Errore</title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
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
    <div id="main">
        <div class="salvataggio">
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');">
                    <span>Chiudi</span></a>
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
                            <td>
                                <asp:Literal ID="ltrDate" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrUrl" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrIP" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txt_body" Width="800px" Height="300px" TextMode="MultiLine" />
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
    </div>
    </form>
</body>
</html>
