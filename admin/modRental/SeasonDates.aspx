<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="SeasonDates.aspx.cs" Inherits="ModRental.admin.modRental.SeasonDates" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var rwdUrl = null;
            function selectDate(id, dt) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "SeasonDatesEdit.aspx?id=" + id + "&dt=" + dt + "&pidSeasonGroup=<%= SeasonGroup %>";
                var width = 700;
                var height = $(window).height();
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(width);
                rwdUrl.set_minHeight(height);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
                return false;
            }
            function rwdUrl_OnClientClose(sender, eventArgs) {
                $find('<%= pnlFascia.ClientID %>').ajaxRequest('rwdUrl_Closing');
            }
            function openSeasonGroupEdit(id) {
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                url = "SeasonGroupEdit.aspx?id=" + id + "&callback=onSeasonGroupEdit";
                var width = 700;
                var height = $(window).height();
                rwdUrl.set_autoSize(false);
                rwdUrl.set_visibleTitlebar(true);
                rwdUrl.set_minWidth(width);
                rwdUrl.set_minHeight(height);
                rwdUrl.setUrl(url);
                rwdUrl.show();
                //rwdUrl.maximize();
                return false;
            }
            function onSeasonGroupEdit(id) {
                document.location = "SeasonDates.aspx?pidSeasonGroup=" + id;
            }
        </script>
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            }
        </script>
        <script type="text/javascript">
            function setCal() {
                $("#avvCalendarCont").datepicker({ numberOfMonths: [3, 4], stepMonths: 4, selectOtherMonths: false, showOtherMonths: false, showButtonPanel: false, beforeShowDay: checkCalDates, onSelect: avvCalendarOnSelect });
            }
        </script>
        <script type="text/javascript">
            function toggleAptList() {
                if ($('#overflowlist_AptList').hasClass('expand'))
                    $("#overflowlist_AptList").removeClass("expand");
                else
                    $("#overflowlist_AptList").addClass("expand");
            } function toggleOtherGroups() {
                if ($('#overflowlist_OtherGroups').hasClass('expand'))
                    $("#overflowlist_OtherGroups").removeClass("expand");
                else
                    $("#overflowlist_OtherGroups").addClass("expand");
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .overflowlist {
            float: left;
            overflow-y: scroll;
            min-width: 300px;
            height: 80px;
        }

            .overflowlist.expand {
                height: auto !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <div id="fascia1">
        <asp:Literal ID="ltr_groupTitle" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_groupDesc" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_AptList" runat="server" Visible="false"></asp:Literal>
        <asp:Literal ID="ltr_OtherGroups" runat="server" Visible="false"></asp:Literal>
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HF_SeasonGroup" Value="0" runat="server" />
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <h2 style="margin-bottom: 2px;">Calendario della stagionalità: <%=ltr_groupTitle.Text %></h2>
                <p style="border-bottom: 1px dotted #000; margin-top: 2px;">
                    <%=ltr_groupDesc.Text %>
                    <%if (HF_SeasonGroup.Value != "0")
                      { %>
                    <a href="#" onclick="openSeasonGroupEdit(<%=HF_SeasonGroup.Value %>);  return false;" class="inlinebtn">Modifica nome e descrizione</a>
                    <%} %>
                </p>
            </telerik:RadCodeBlock>
            <div class="nulla">
            </div>
            <table style="float: left;">
                <tr>
                    <td class="rntCal sel_f" style="position: relative;"></td>
                    <td>- Bassa
                    </td>
                </tr>
                <tr>
                    <td class="rntCal mv_f" style="position: relative;"></td>
                    <td>- Media
                    </td>
                </tr>
                <tr>
                    <td class="rntCal opz_f" style="position: relative;"></td>
                    <td>- Alta
                    </td>
                </tr>

                <tr>
                    <td class="rntCal prt_f" style="position: relative;"></td>
                    <td>- Altissima
                    </td>
                </tr>
                <tr>
                    <td class="rntCal nd_f" style="position: relative;"></td>
                    <td>- Nessuna Stagione: date non prenotabili
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Clicca sulle date per modificare o inserire nuove date.
                    </td>
                </tr>
            </table>
            <div style="float: left; margin-left: 20px;">
                <strong onclick="toggleAptList()" style="cursor: pointer;">Appartamenti abbinati</strong>
                <div class="nulla">
                </div>
                <div id="overflowlist_AptList" class="overflowlist">
                    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                        <%=ltr_AptList.Text%>
                    </telerik:RadCodeBlock>
                </div>
            </div>
            <div style="float: left; margin-left: 20px;">
                <strong onclick="toggleOtherGroups()" style="cursor: pointer;">Altre stagionalità</strong>
                <div class="nulla">
                </div>
                <div id="overflowlist_OtherGroups" class="overflowlist">
                    <a href="#" onclick="openSeasonGroupEdit(-1);  return false;" class="inlinebtn">Crea Nuova stagionalità</a>
                    <br />
                    <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
                        <%=ltr_OtherGroups.Text %>
                    </telerik:RadCodeBlock>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div id="avvCalendarCont" style="margin-left: 5px; margin-top: 20px;">
            </div>
        </telerik:RadAjaxPanel>
    </div>
    <div class="nulla">
    </div>
</asp:Content>
