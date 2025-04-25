<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_reservation.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select
        {
            font-family: verdana;
            font-size: 10px;
        }
    </style>

    <script type="text/javascript">
        var rwdUrl = null;
        function rwdUrl_open(url) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            rwdUrl.set_autoSize(false);
            rwdUrl.set_visibleTitlebar(true);
            //rwdUrl.set_minWidth(700);
            rwdUrl.setUrl(url);
            rwdUrl.show();
            rwdUrl.maximize();
            return false;
        } 
        function rwdUrl_close() {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
            rwdUrl.close();
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <asp:Literal ID="ltr_tooltip_template" runat="server" Visible="false">
	<strong>ttp_cl_name_full, [Code. ttp_code]</strong><br/>
	Persone: ttp_persons, Prezzo:&euro; ttp_pr_total<br/>
	Scadenza: ttp_block_expire
    </asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="1" runat="server" />
            <asp:HiddenField ID="HF_activeConfigs" runat="server" Value="1|2" />
            <asp:HiddenField ID="HF_currConfig" runat="server" Value="" />
            <asp:Literal ID="ltr_activeConfigsControl" runat="server" Visible="false"></asp:Literal>
            <h1 class="titolo_main">
                Gestione Prenotazioni e Disponibilità delle Strutture</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <asp:Literal ID="ltr_empty" runat="server" Visible="false">IG</asp:Literal>
                        <asp:Literal ID="ltr_filter_estates" runat="server" Visible="false"></asp:Literal>
                        <asp:HiddenField runat="server" ID="HF_pid_estate" Value="0" />
                        <asp:HiddenField ID="HF_unique" Value="" runat="server" />
                        <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 20px;">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Periodo:
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="inp">
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                dal:</label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtStart_<%= Unique %>" style="width: 120px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                al:</label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtEnd_<%= Unique %>" style="width: 120px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="HF_dtStart" runat="server" />
                                                <asp:HiddenField ID="HF_dtEnd" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                con stato:&nbsp;
                                                <asp:DropDownList ID="drp_flt_state" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="HF_dateMode" runat="server" Value="1" />
                                    <script type="text/javascript">
                                        function RNT_fillList() {
                                            $("#resList_body").html($("#loaderBody").html());
                                            var _zoneList = $("#<%=lbx_flt_zone.ClientID%>").val();
                                            if (!_zoneList) _zoneList = new Array();
                                            var _zoneListStr = _zoneList.length == 0 ? "0" : ("" + _zoneList).replace(/\,/g, "|");

                                            var _estateList = $("#<%=lbx_flt_estate.ClientID%>").val();
                                            if (!_estateList) _estateList = new Array();
                                            var _estateListStr = _estateList.length == 0 ? "0" : ("" + _estateList).replace(/\,/g, "|");

                                            var _url = "webservice/rnt_estateReservation.aspx";
                                            _url += "?CURRENT_ID=xxxxxx";
                                            _url += "&dtS=" + $("#<%= HF_dtStart.ClientID %>").val();
                                            _url += "&dtE=" + $("#<%= HF_dtEnd.ClientID %>").val();
                                            _url += "&state=" + $("#<%= drp_flt_state.ClientID %>").val();
                                            _url += "&currCity=" + $("#<%= drp_flt_city.ClientID %>").val();
                                            _url += "&currZone=" + _zoneListStr;
                                            _url += "&currEstate=" + _estateListStr;
                                            _url += "&minPers=" + $("#<%= drp_min_num_persons_max.ClientID %>").val();
                                            _url += "&minRooms=" + $("#<%= drp_min_num_rooms_bed.ClientID %>").val();
                                            _url += "&importance_stars=" + $("#<%= drp_min_importance_stars.ClientID %>").val();
                                            _url += "&is_exclusive=" + $("#<%= drp_is_exclusive.ClientID %>").val();
                                            _url += "&is_online_booking=" + $("#<%= drp_is_online_booking.ClientID %>").val();
                                            _url += "&currConfig=" + $("#<%= HF_currConfig.ClientID %>").val();
                                            var _xml = $.ajax({
                                                type: "GET",
                                                url: _url,
                                                dataType: "html",
                                                success: function (html) {
                                                    $("#resList_body").html(html);
                                                    setToolTip();
                                                }
                                            });
                                        }
                                        function refreshDates() {
                                            RNT_fillList();
                                            Shadowbox.close();
                                            rwdUrl_close();
                                        }
                                        function closeFromForm() {
                                            refreshDates();
                                        }
                                        function RNT_openSelection(elm) {
                                            var dtDate = parseInt($(elm).attr("dtDate"));
                                            var dtPart = parseInt($(elm).attr("dtPart"));
                                            var IdEstate = parseInt($(elm).attr("IdEstate"));
                                            var IdRes = parseInt($(elm).attr("IdRes"));
                                            var dtStart = $("#<%=HF_dtStart.ClientID %>").val();
                                            var dtEnd = $("#<%=HF_dtEnd.ClientID %>").val();
                                            var _url = "rnt_reservation_form.aspx?nomenu=true&dtStart=" + dtStart + "&dtEnd=" + dtEnd + "&dtDate=" + dtDate + "&dtPart=" + dtPart + "&IdEstate=" + IdEstate + "&IdRes=" + IdRes;
                                            rwdUrl_open(_url);
                                            //OpenShadowbox(_url, 800, 0);
                                        }
                                        function RNT_currConfig_onChange() {
                                            var _array = new Array();
                                            var _arrStr = $("#<%= HF_activeConfigs.ClientID %>").val();
                                            var _arrSep = "|";
                                            if (_arrStr.indexOf(_arrSep) != -1)
                                                _array = _arrStr.split(_arrSep);
                                            else
                                                _array = [_arrStr];
                                            _arrStr = "";
                                            _arrSep = "";
                                            for (var i = 0; i < _array.length; i++) {
                                                if ($("#chk_config_" + _array[i]).is(':checked')) {
                                                    _arrStr += _arrSep + "" + _array[i];
                                                    _arrSep = "|";
                                                }
                                            }
                                            $("#<%= HF_currConfig.ClientID %>").val(_arrStr);
                                        }
                                    </script>
                                </td>
                                <td id="pnl_flt_estate_zone" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Zone:</label>
                                                        <asp:DropDownList runat="server" ID="drp_flt_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_flt_city_SelectedIndexChanged" CssClass="inp" Style="margin-bottom: 10px;" />
                                                        <br />
                                                        <asp:ListBox ID="lbx_flt_zone" runat="server" SelectionMode="Multiple" Width="150px" Rows="9" CssClass="inp" AutoPostBack="true" OnSelectedIndexChanged="lbx_flt_zone_SelectedIndexChanged"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Strutture:</label>
                                                        <asp:ListBox ID="lbx_flt_estate" runat="server" SelectionMode="Multiple" Width="220px" Rows="11" CssClass="inp"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td id="Td2" runat="server">
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Accessori:
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= ltr_activeConfigsControl.Text %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="pnl_flt_prop" runat="server">
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
                                        <tr>
                                            <td colspan="2">
                                                <label class="barraTop">
                                                    Filtro caratteristiche:</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Posti Letto
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_num_persons_max" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Camere
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_num_rooms_bed" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Min. #.Stelle
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_importance_stars" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                In Esclusiva
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_exclusive" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Instant Booking
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_online_booking" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                Attico*
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_loft" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <a href="javascript:refreshDates();">
                                                            <span>Filtra</span></a>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="Td1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Ricerca per Codice:</label>
                                                        <asp:TextBox ID="txt_code" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="salvataggio">
                                                            <div class="bottom_salva">
                                                                <asp:LinkButton ID="lnk_searchCode" runat="server" OnClick="lnk_searchCode_Click"><span>Cerca</span></asp:LinkButton>
                                                            </div>
                                                            <div class="nulla">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_codeError" runat="server" Text="Il codice fornito inesistente." Visible="false" Style="color: red; font-size: 15px;"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div class="salvataggio" style="margin-bottom:0; margin-left:0;">
                                        <div class="bottom_salva" style="margin-left:5px;">
                                            <a href="javascript:refreshDates();">
                                                <span>Filtra</span></a>
                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:LinqDataSource ID="LDS_availability" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_LK_RESERVATION_STATEs" OrderBy="title" Where="title != @title && id!=3 && type==1">
                                        <WhereParameters>
                                            <asp:Parameter Name="title" DefaultValue="" Type="String" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_availability" DataSourceID="LDS_availability" runat="server" EnableViewState="false">
                                        <ItemTemplate>
                                            <td>
                                                <span class="<%# (""+Eval("css_class")).ToLower() %>" style="width: 30px;"></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("abbr") + "&nbsp;-&nbsp;" + Eval("title")%></span>
                                            </td>
                                            <td style="width: 10px;">
                                            </td>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            empty
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="res_free" style="width: 30px;"></span>
                                                    </td>
                                                    <td>
                                                        <span>Disponibile</span>
                                                    </td>
                                                    <td style="width: 10px;">
                                                    </td>
                                                    <td id="itemPlaceholder" runat="server" />
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <div id="resList_cont">
                                        <table cellspacing="0" cellpadding="0" border="1" style="font-size: 10px;">
                                            <tbody id="resList_body">
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="nulla">
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
            <asp:Literal ID="ltr_tooltip_cont" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellspacing="0" cellpadding="0" border="1" style="font-size: 10px; display: none;">
        <tbody id="loaderBody">
            <tr>
                <td>
                    <uc2:UC_loader_list ID="UC_loader_list1" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
    <script type="text/javascript">
     var _JSCal_Range_;
    function setCal_<%= Unique %>() {
        _JSCal_Range_= new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", changeMonth: true, changeYear: true });
    }
       function ReloadItems(id) {
            Shadowbox.close();
            rwdUrl_close();
            __doPostBack('lnk_new', id);
        }
    </script>
</asp:Content>
