<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="EstateLogDett.aspx.cs" Inherits="ModRental.admin.modRental.EstateLogDett" %>

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
<body style="background-image: none;">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Visible="true">
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
                                    <td class="td_title">
                                        Data:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_Date" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Struttura:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_estateCode" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Utente:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_userName" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Campo:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_changeField" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Prima:
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_valueBefore" Width="500px" Height="100px" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Dopo:
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_valueAfter" Width="500px" Height="100px" TextMode="MultiLine" />
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
