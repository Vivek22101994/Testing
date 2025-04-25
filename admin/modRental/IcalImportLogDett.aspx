<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="IcalImportLogDett.aspx.cs" Inherits="ModRental.admin.modRental.IcalImportLogDett" %>

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
                                <asp:Literal ID="ltr_errorCount" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltr_iCalType" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltr_iCalUrl" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>

                    <asp:ListView ID="LV" runat="server">
                        <ItemTemplate>
                            <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <td>
                                    <span>
                                        <%# ((DateTime?)Eval("iCalDtStart")).formatITA(false)%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# ((DateTime?)Eval("iCalDtEnd")).formatITA(false)%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("iCalComment")%></span>
                                </td>
                                <td>
                                    <a href="/admin/rnt_reservation_details.aspx?id=<%# Eval("reservationId")%>" target="_blank" style="margin: 5px;"><%# "Bloccato da pren #"+Eval("reservationId")+" ("+Eval("reservationStateName")+")"%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <td>
                                    <span>
                                        <%# ((DateTime?)Eval("iCalDtStart")).formatITA(false)%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# ((DateTime?)Eval("iCalDtEnd")).formatITA(false)%></span>
                                </td>
                                <td>
                                    <span>
                                        <%# Eval("iCalComment")%></span>
                                </td>
                                <td>
                                    <a href="/admin/rnt_reservation_details.aspx?id=<%# Eval("reservationId")%>" target="_blank" style="margin: 5px;"><%# "Bloccato da pren #"+Eval("reservationId")+" ("+Eval("reservationStateName")+")"%></a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <span class="titoloboxmodulo">Calendario importato con successo</span>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <span class="titoloboxmodulo">Calendario importato con alcuni errori</span>
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                    <td style="width: 100px;">Check-In</td>
                                    <td style="width: 100px;">Check-Out</td>
                                    <td style="width: 200px;">Comment</td>
                                    <td style="width: 300px;">Motivo</td>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
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
