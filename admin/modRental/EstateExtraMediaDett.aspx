<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstateExtraMediaDett.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraMediaDett" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
            @import url(/admin/css/adminStyle.css);
            @import url(/css/common.css);
            @import url(/jquery/css/ui-core.css);
            @import url(/jquery/css/ui-autocomplete.css);
            @import url(/jquery/css/ui-datepicker.css);
            body 
            {
               background:#FFF;
            } 
        </style>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script src="/jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
        <script src="/jquery/js/ui-all.min.js" type="text/javascript"></script>
        <script src="/jquery/datepicker-lang/jquery.ui.datepicker-it.js" type="text/javascript"></script>
        <script src="/jquery/plugin/scrollTo/scrollTo.min.js" type="text/javascript"></script>
        <script src="/jquery/plugin/jquery.tooltip.js" type="text/javascript"></script>
        <script src="/jquery/plugin/utils.js" type="text/javascript"></script>
        <script type="text/javascript">
            function setToolTip() {
                $('._tooltip').tooltip({ track: true, delay: 0, positionLeft: true, top: 0, left: 0 });
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
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
            $(function () {
                $("#imgMain").css({ "max-width": $(window).width() + "px", "max-height": ($(window).height() - $("#pnlControls").height() - 10) + "px", "display": "block", "margin-top": "10px" });
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
        <ProgressTemplate>
            <%="<div id=\"loading_big_cont\"></div><div id=\"overlay_site\"></div>" %>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:HiddenField ID="HF_imgPath" runat="server" Visible="false" />
    <asp:HiddenField ID="HF_id" Value="0" runat="server" />
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <div id="pnlControls" class="controls">
            <asp:TextBox ID="txt_code" Width="210px" MaxLength="90" runat="server"></asp:TextBox>
            <asp:LinkButton ID="lnk_saveGalleryItem" runat="server" CssClass="inlinebtn" OnClick="lnk_saveGalleryItem_Click">
                        Salva
            </asp:LinkButton>
            <asp:LinkButton ID="lnk_deleteGalleryItem" runat="server" CssClass="inlinebtn" OnClick="lnk_deleteGalleryItem_Click" OnClientClick="return confirm('Stai per eliminare la foto definitivamente?');">
                        Elimina
            </asp:LinkButton>
            <a href="#" onclick="parent.closeDett();return false;" class="inlinebtn">Chiudi</a>
        </div>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <img src="/<%= HF_imgPath.Value %>" alt="Immagine assente!" id="imgMain" style="display: none;" />
        </telerik:RadCodeBlock>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
