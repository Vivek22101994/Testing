<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_search.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_search" %>
<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label.barraTop{min-width:300px;}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function RNT_changePage(page) {
            $("#hf_currPage").val("" + page);
            RNT_fillList("list");
        }
        function RNT_changeNumPerPage(page) {
            $("#<%= HF_numPerPage.ClientID %>").val("" + page);
            RNT_fillList("list");
        }
        function RNT_fillList(action) {
            $("#estateListCont").html($("#loaderBody").html());
            var _url = "/webservice/rnt_estateListSearch.aspx";
            _url += "?lang=" + $("#<%= drp_pidLang.ClientID %>").val();
            _url += "&SESSION_ID=<%= Unique %>";
            _url += "&id_op=<%= UserAuthentication.CurrentUserID %>";
            _url += "&foradmin=true";
            _url += "&formail=" + $("#<%= drp_formail.ClientID %>").val();
            _url += "&showonrequest=" + $("#<%= drp_showonrequest.ClientID %>").val();
            _url += "&agentID=" + $("#<%= drp_pidAgent.ClientID %>").val();
            _url += "&dtS=" + $("#<%= HF_dtStart.ClientID %>").val();
            _url += "&dtE=" + $("#<%= HF_dtEnd.ClientID %>").val();
            _url += "&numPers_adult=" + $("#<%= HF_numPers_adult.ClientID %>").val();
            _url += "&numPers_childOver=0";
            _url += "&numPers_childMin=0";
            _url += "&title=" + $("#<%= HF_searchTitle.ClientID %>").val();
            _url += "&currCity=" + $("#<%= drp_city.ClientID %>").val();
            _url += "&currZoneList=" + $("#<%= HF_currZoneList.ClientID %>").val();
            _url += "&currConfigList=" + $("#<%= HF_currConfigList.ClientID %>").val();
            _url += "&prRange=" + $("#hf_prRange").val();
            _url += "&voteRange=0|10"; //  + $("#hf_voteRange").val();

            _url += "&currPage=" + $("#hf_currPage").val();
            _url += "&numPerPage=" + $("#<%= HF_numPerPage.ClientID %>").val();
            _url += "&orderBy=title";
            _url += "&orderHow=asc";

            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#estateListCont").html(html);
                    if (action == "list") {
                        $.scrollTo($("#estateListCont").offset().top + 110, 500);
                        SITE_hideLoader();
                    }
                }
            });
        }
        $(document).ready(function () {
            //RNT_fillList("first");
            if ($("#<%= HF_currZoneList.ClientID %>").val() == "0")
                $("#chk_zone_all").attr("checked", "checked");
            else
                $("#chk_zone_all").removeAttr("checked");
        });
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
            $("#<%= HF_currConfigList.ClientID %>").val(_arrStr);
            RNT_fillList("list");
        }
        function RNT_orderBy(order) {
            var _orderByCont = $("#<%= HF_orderBy.ClientID %>");
            var _orderHowCont = $("#<%= HF_orderHow.ClientID %>");
            var _oldOrderBy = _orderByCont.val();
            var _oldOrderHow = _orderHowCont.val();
            var _newOrderBy = order;
            var _newOrderHow = "";
            if (_newOrderBy == _oldOrderBy)
                _newOrderHow = _oldOrderHow == "asc" ? "desc" : "asc";
            else if (_newOrderBy == "price")
                _newOrderHow = "asc";
            else if (_newOrderBy == "vote")
                _newOrderHow = "desc";
            else if (_newOrderBy == "title")
                _newOrderHow = "asc";
            _orderByCont.val(_newOrderBy);
            _orderHowCont.val(_newOrderHow);
            RNT_fillList("list");
            $("#hl_orderBy_price").attr("class", (_newOrderBy == "price") ? _newOrderHow : "");
            $("#hl_orderBy_vote").attr("class", (_newOrderBy == "vote") ? _newOrderHow : "");
            $("#hl_orderBy_title").attr("class", (_newOrderBy == "title") ? _newOrderHow : "");
        }
    </script>

    <script type="text/javascript">
        function submitForm() {
            $("#<%= HF_numPers_adult.ClientID %>").val($("#<%= drp_numPers_adult.ClientID %>").val());
            $("#<%= HF_numPers_childOver.ClientID %>").val($("#<%= drp_numPers_childOver.ClientID %>").val());
            $("#<%= HF_numPers_childMin.ClientID %>").val($("#<%= drp_numPers_childMin.ClientID %>").val());
            $("#<%= HF_searchTitle.ClientID %>").val($("#<%= txt_searchTitle.ClientID %>").val());
            RNT_get_chkList_zone();
            RNT_fillList("list");
            return false;
        }
        function RNT_get_chkList_zone() {
            _arrStr = "";
            _arrSep = "";
            $(".chkList_zone_item").each(function (index) {
                if ($(this).is(':checked')) {
                    _arrStr += _arrSep + "" + $(this).val();
                    _arrSep = "|";
                }
            });
            if (_arrStr == "") {
                //$(".chk_zone_all").attr("checked", "checked");
                $("#<%= HF_currZoneList.ClientID %>").val("0");
            }
            else {
                //$("#chk_zone_all").removeAttr("checked");
                $("#<%= HF_currZoneList.ClientID %>").val(_arrStr);
            }
        }
        function RNT_openSelection(IdEstate, dtStart, dtEnd, numPers, agentID) {
            var _url = "/admin/modRental/newResSteps/step1.aspx?pr_isManual=0&IdRequest=0&IdEstate=" + IdEstate + "&dtStart=" + dtStart + "&dtEnd=" + dtEnd + "&numPers=" + numPers + "&agentID=" + agentID;
            //alert(_url);
            OpenShadowbox(_url);
        }
        function RNT_openFreePriceReservation(IdEstate, dtStart, dtEnd, numPers, agentID) {
            var _url = "/admin/modRental/newResSteps/step1.aspx?pr_isManual=1&IdRequest=0&IdEstate=" + IdEstate + "&dtStart=" + dtStart + "&dtEnd=" + dtEnd + "&numPers=" + numPers + "&agentID=" + agentID;
            //alert(_url);
            OpenShadowbox(_url);
        }
    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_searchTitle" runat="server" Value="" />
            <asp:HiddenField ID="HF_currPage" runat="server" Value="0" />
            <asp:HiddenField ID="HF_numPerPage" runat="server" Value="15" />
            <asp:HiddenField ID="HF_activeZones" runat="server" Value="" />
            <asp:HiddenField ID="HF_currZoneList" runat="server" Value="" />
            <asp:HiddenField ID="HF_currConfigList" runat="server" Value="" />
            <asp:HiddenField ID="HF_prRangeTmp" runat="server" Value="0|0" />
            <asp:HiddenField ID="HF_voteRangeTmp" runat="server" Value="0|10" />
            <asp:HiddenField ID="HF_activeConfigs" runat="server" Value="1|2" />
            <asp:HiddenField ID="HF_numPers_adult" runat="server" Value="0" />
            <asp:HiddenField ID="HF_numPers_childOver" runat="server" Value="0" />
            <asp:HiddenField ID="HF_numPers_childMin" runat="server" Value="0" />
            <asp:Literal ID="ltr_activeConfigsControl" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_activeZonesControl" runat="server" Visible="false"></asp:Literal>
            <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
            <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />
            <h1 class="titolo_main">Ricerca appartamenti disponibili per prenotazioni</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox mainRicDisp">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center reservationcont estateSearch">
                        <asp:HiddenField ID="HF_unique" Value="" runat="server" />
                        <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 20px; float: left;">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Nome appartamento:
                                                </label>
                                                <asp:TextBox ID="txt_searchTitle" runat="server" Style="width: 96%;"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
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
                                                                Check-in:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtStart_<%= Unique %>" style="width: 120px" />
                                                            <asp:HiddenField ID="HF_dtStart" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Check-out:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txt_dtEnd_<%= Unique %>" style="width: 120px" />
                                                            <asp:HiddenField ID="HF_dtEnd" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Num. pers.:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_numPers_adult" runat="server" Style="width: 45px; float: left;">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">Min. #.Camere
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_min_num_rooms_bed" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_title">Min. # Bagni
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_min_num_rooms_bath" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1" runat="server" visible="false">
                                                        <td>
                                                            <label>
                                                                Bambini:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_numPers_childOver" runat="server" Style="width: 45px; float: left;">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr2" runat="server" visible="false">
                                                        <td>
                                                            <label>
                                                                Neonati:
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drp_numPers_childMin" runat="server" Style="width: 45px; float: left;">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="pnl_flt_estate_zone" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                                <tr>
                                                    <td>
                                                        <label class="barraTop">
                                                            Citta/Zone:
                                                        </label>
                                                        <asp:DropDownList ID="drp_city" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_city_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                        <%= ltr_activeZonesControl.Text %>
                                                        <asp:CheckBoxList ID="chkList_zone" runat="server" CssClass="chk" RepeatColumns="2" Visible="false">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
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
                            </tr>
                            <tr id="Tr3" runat="server" visible="false">
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
                                        <tr>
                                            <td colspan="2">
                                                <label class="barraTop">
                                                    Filtro caratteristiche:
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">Min. #.Posti Letto
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_num_persons_max" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">Min. #.Stelle
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_min_importance_stars" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">In Esclusiva
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
                                            <td class="td_title">Instant Booking
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
                                            <td class="td_title">Attico*
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drp_is_loft" runat="server">
                                                    <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Agenzia:
                                                </label>
                                                <asp:DropDownList ID="drp_pidAgent" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">
                                                    Stagione/Prezzi:
                                                </label>
                                                <asp:DropDownList ID="drp_showonrequest" runat="server">
                                                    <asp:ListItem Value="false" Text="Solo strutture con prezzi"></asp:ListItem>
                                                    <asp:ListItem Value="true" Text="Anche strutture senza prezzo nel periodo selezionato"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
                                        <tr>
                                            <td>
                                                <label class="barraTop">Filtra per:</label>
                                                <asp:DropDownList ID="drp_pidLang" runat="server">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:DropDownList ID="drp_formail" runat="server">
                                                    <asp:ListItem Value="false" Text="Risultato per prenotare subito"></asp:ListItem>
                                                    <asp:ListItem Value="true" Text="Risultato per inviare nella mail, senza pulsante"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <div class="salvataggio">
                                                    <div class="bottom_salva">
                                                        <a href="#" onclick="return submitForm()">
                                                            <span>Filtra</span>
                                                        </a>
                                                    </div>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                        <div id="estateListCont" style="float: right;">
                            <input type="hidden" name="hf_currPage" id="Hidden1" value="<%=HF_currPage.Value %>" />
                            <input type="hidden" name="hf_voteMin" id="hf_voteMin" value="0" />
                            <input type="hidden" name="hf_voteMax" id="hf_voteMax" value="10" />
                            <input type="hidden" name="hf_voteRange" id="hf_voteRange" value="<%=HF_voteRangeTmp.Value %>" />
                            <input type="hidden" name="hf_voteTemp" id="hf_voteTemp" value="<%=HF_voteRangeTmp.Value %>" />
                            <input type="hidden" name="hf_prMin" id="hf_prMin" value="0" />
                            <input type="hidden" name="hf_prMax" id="hf_prMax" value="0" />
                            <input type="hidden" name="hf_prRange" id="hf_prRange" value="<%=HF_prRangeTmp.Value %>" />
                            <input type="hidden" name="hf_prTemp" id="hf_prTemp" value="<%=HF_prRangeTmp.Value %>" />
                        </div>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Literal ID="ltr_tooltip_cont" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="loaderBody" style="display: none;">
        <div class="loadingSrc">
            <span>
                <%=CurrentSource.getSysLangValue("lblLoadingData")%>
            </span>
        </div>
        <div class="nulla">
        </div>
    </div>
    <script type="text/javascript">
        var _JSCal_Range_;
        function setCal_<%= Unique %>() {
         _JSCal_Range_ = new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", changeMonth: true, changeYear: true });
    }
    function ReloadItems(id) {
        Shadowbox.close();
        __doPostBack('lnk_new', id);
    }
    function refreshDates() {
        Shadowbox.close();
    }
    </script>
</asp:Content>
