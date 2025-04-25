<%@ Page Title="" Language="C#" MasterPageFile="~/WLRental/common/MP_WLRental.Master" AutoEventWireup="true" CodeBehind="stp_estateListSearch.aspx.cs" Inherits="RentalInRome.WLRental.stp_estateListSearch" %>

<%--<%@ Register Src="uc/UC_staffInLang.ascx" TagName="UC_staffInLang" TagPrefix="uc1" %>--%>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_search.ascx" TagName="UC_search" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/javascript">
        function set_switchThumb() {

            $("a.switch_thumb").toggle(function () {
                $(this).addClass("swap");
                $("ul.display").fadeOut("fast", function () {
                    $(this).fadeIn("fast").removeClass("dett_view");
                    $(this).fadeIn("fast").addClass("thumb_view");
                });
            }, function () {
                $(this).removeClass("swap");
                $("ul.display").fadeOut("fast", function () {
                    $(this).fadeIn("fast").removeClass("thumb_view");
                    $(this).fadeIn("fast").addClass("dett_view");
                });
            });

        }
    </script>
    <script type="text/javascript">
        function RNT_changePage(page) {
            $("#hf_currPage").val("" + page);
            RNT_fillList("list");
        }
        function RNT_fillList(action) {
            if (action == "list") SITE_showLoader();
            var _url = "/webservice/rnt_estateListSearch.aspx";
            _url += "?lang=<%= CurrentLang.ID %>";
            _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&agentID=<%= RentalInRome.affiliatesarea.agentAuth.CurrentID %>";
            _url += "&dtS=" + $("#<%= HF_dtStart.ClientID %>").val(); 
            _url += "&dtE=" + $("#<%= HF_dtEnd.ClientID %>").val();
            _url += "&numPers_adult=" + $("#<%= drp_adult.ClientID %>").val();
            _url += "&numPers_childOver=" + $("#<%= drp_child_over.ClientID %>").val();
            _url += "&numPers_childMin=" + $("#<%= drp_child_min.ClientID %>").val();
            _url += "&title=" + $("#<%= HF_searchTitle.ClientID %>").val();
            _url += "&currZoneList=" + $("#<%= HF_currZoneList.ClientID %>").val();
            _url += "&currConfigList=" + $("#<%= HF_currConfigList.ClientID %>").val();
            _url += "&prRange=" + $("#hf_prRange").val();
            _url += "&voteRange=" + $("#hf_voteRange").val();

            _url += "&currPage=" + $("#hf_currPage").val();
            _url += "&numPerPage=" + $("#<%= HF_numPerPage.ClientID %>").val();
            _url += "&orderBy=" + $("#<%= HF_orderBy.ClientID %>").val();
            _url += "&orderHow=" + $("#<%= HF_orderHow.ClientID %>").val();
            _url += "&isWL=1";
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#estateListCont").html(html);
                    if (action == "list") {
                        $.scrollTo($("#estateListCont"), 500);
                        SITE_hideLoader();
                    }
                    RNT_priceRangeSlider_set();
                    RNT_voteRangeSlider_set();
                    set_switchThumb(); 
                    setToolTip()
                }
            });
        }
        $(document).ready(function () {
            RNT_fillList("first");
            if ($("#<%= HF_currZoneList.ClientID %>").val() == "0")
                $("#chk_zone_all").attr("checked", "checked");
            else
                $("#chk_zone_all").removeAttr("checked");
        });
        function RNT_currZone_onChange(sender) {
            if (sender != null && sender == "all") {
                if ($("#chk_zone_all").is(':checked')) {
                    $(".chk_zone").removeAttr("checked");
                    $("#<%= HF_currZoneList.ClientID %>").val("0");
                    RNT_fillList("list");
                }
                return;
            }
            var _array = new Array();
            var _arrStr = $("#<%= HF_activeZones.ClientID %>").val();
            var _arrSep = "|";
            if (_arrStr.indexOf(_arrSep) != -1)
                _array = _arrStr.split(_arrSep);
            else
                _array = [_arrStr];
            _arrStr = "";
            _arrSep = "";
            for (var i = 0; i < _array.length; i++) {
                if ($("#chk_zone_" + _array[i]).is(':checked')) {
                    _arrStr += _arrSep + "" + _array[i];
                    _arrSep = "|";
                }
            }
            if (_arrStr == "") {
                $(".chk_zone_all").attr("checked", "checked");
                $("#<%= HF_currZoneList.ClientID %>").val("0");
            }
            else {
                $("#chk_zone_all").removeAttr("checked");
                $("#<%= HF_currZoneList.ClientID %>").val(_arrStr);
            }
            RNT_fillList("list");
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
        var RNT_priceRangeSlider;
        function RNT_priceRangeSlider_set() {
            if (RNT_priceRangeSlider != null) $("#priceRange-slider").slider("destroy");
            var RangeMin = parseInt($("#hf_prMin").val());
            var RangeMax = parseInt($("#hf_prMax").val());
            var selFrom = parseInt($("#hf_prRange").val().split('|')[0]);
            var selTo = parseInt($("#hf_prRange").val().split('|')[1]);
            if (selFrom < RangeMin) selFrom = RangeMin;
            if (selTo > RangeMax) selTo = RangeMax;
            RNT_prRangeSlider = $("#priceRange-slider").slider({
                min: RangeMin,
                max: RangeMax,
                range: true,
                step: 1,
                values: [selFrom, selTo],
                slide: function (event, ui) {
                    var from = ui.values[0];
                    var to = ui.values[1];
                    $("#lbl_prFrom").html("" + from);
                    $("#lbl_prTo").html("" + to);
                    $("#hf_prRange").val(from + "|" + to);
                },
                change: function () {
                    var _oldVal = "" + $("#hf_prTemp").val();
                    var _newVal = "" + $("#hf_prRange").val();
                    $("#hf_prTemp").val(_newVal);
                    if (_oldVal != _newVal)
                        RNT_fillList("list");
                }
            });
            $("#lbl_prFrom").html("" + selFrom);
            $("#lbl_prTo").html("" + selTo);
        }
    </script>
    <script type="text/javascript">
        var RNT_voteRangeSlider;
        function RNT_voteRangeSlider_set() {
            if (RNT_voteRangeSlider != null) $("#voteRange-slider").slider("destroy");
            var RangeMin = parseInt($("#hf_voteMin").val());
            var RangeMax = parseInt($("#hf_voteMax").val());
            var selFrom = parseInt($("#hf_voteRange").val().split('|')[0]);
            var selTo = parseInt($("#hf_voteRange").val().split('|')[1]);
            if (selFrom < RangeMin) selFrom = RangeMin;
            if (selTo > RangeMax) selTo = RangeMax;
            RNT_voteRangeSlider = $("#voteRange-slider").slider({
                min: RangeMin,
                max: RangeMax,
                range: true,
                step: 1,
                values: [selFrom, selTo],
                slide: function (event, ui) {
                    var from = ui.values[0];
                    var to = ui.values[1];
                    $("#lbl_voteFrom").html("" + from);
                    $("#lbl_voteTo").html("" + to);
                    $("#hf_voteRange").val(from + "|" + to);
                },
                change: function () {
                    var _oldVal = "" + $("#hf_voteTemp").val();
                    var _newVal = "" + $("#hf_voteRange").val();
                    $("#hf_voteTemp").val(_newVal);
                    if (_oldVal != _newVal)
                        RNT_fillList("list");
                }
            });
            $("#lbl_voteFrom").html("" + selFrom);
            $("#lbl_voteTo").html("" + selTo);
        }
        function calculateNumPersons(sender) {
            var _selNum_adult = parseInt($("#<%= drp_adult.ClientID %>").val(), 10);
            var _selNum_child_over = parseInt($("#<%= drp_child_over.ClientID %>").val(), 10);
            if(sender=="childMin") { RNT_fillList("list"); return;}
            $("#<%= HF_numPersCount.ClientID %>").val(""+(_selNum_adult+_selNum_child_over)); RNT_fillList("list");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPersCount" runat="server" Value="0" />
    <asp:HiddenField ID="HF_searchTitle" runat="server" Value="" />
    <asp:HiddenField ID="HF_currPage" runat="server" Value="1" />
    <asp:HiddenField ID="HF_numPerPage" runat="server" Value="15" />
    <asp:HiddenField ID="HF_activeZones" runat="server" Value="" />
    <asp:HiddenField ID="HF_currZoneList" runat="server" Value="" />
    <asp:HiddenField ID="HF_currConfigList" runat="server" Value="" />
    <asp:HiddenField ID="HF_prRangeTmp" runat="server" Value="0|0" />
    <asp:HiddenField ID="HF_voteRangeTmp" runat="server" Value="0|10" />
    <asp:HiddenField ID="HF_activeConfigs" runat="server" Value="1|2" />
    <asp:Literal ID="ltr_activeConfigsControl" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="ltr_activeZonesControl" runat="server" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />

    <div id="lista_search">
        <div class="sx">
            <div id="search_int" style="background-color: #d5d5e1;">
                <div class="tabTitle" style="float: left;">
                    <%=CurrentSource.getSysLangValue("lblSearch")%>
                </div>
                <div class="cont_form">
                    <div class="preinput preinputdue">
                        <label id="lb_QuickSearch" style="position: absolute; display: block; width: 190px;">
                            <%=CurrentSource.getSysLangValue("lblQuickSearch")%>
                        </label>
                        <asp:TextBox ID="txt_title" runat="server" CssClass="search_aptComplete floatnone" Width="195"></asp:TextBox>
                        <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                        <asp:HiddenField ID="HF_estatePath" runat="server" Value="" />
                    </div>
                    <div class="preinput">
                        <label>
                            <%-- <%=CurrentSource.getSysLangValue("reqCheckInDate")%>--%>
                            Check-In
                        </label>
                        <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                        <a class="ico_cal" id="startCalTrigger_<%= Unique %>"></a>
                        <asp:HiddenField ID="HF_dtStartTmp" runat="server" Value="0" />
                    </div>
                    <div class="preinput">
                        <label>
                            <%-- <%=CurrentSource.getSysLangValue("reqCheckOutDate")%>--%>
                            Check-Out
                        </label>
                        <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                        <a class="ico_cal" id="endCalTrigger_<%= Unique %>"></a>
                        <asp:HiddenField ID="HF_dtEndTmp" runat="server" Value="0" />
                    </div>
                    <div class="preinput">
                        <label>
                            <%=CurrentSource.getSysLangValue("lblNights")%>
                        </label>
                        <asp:TextBox ID="txt_dtCount" runat="server"></asp:TextBox>
                    </div>
                    <a href="#" onclick="return submitForm()" class="search_but">
                        <span><%=CurrentSource.getSysLangValue("lblBeginSearch")%></span>
                    </a>
                </div>
                <script type="text/javascript">
    function submitForm()
    {
        $("#<%= HF_dtStart.ClientID %>").val($("#<%= HF_dtStartTmp.ClientID %>").val());
        $("#<%= HF_dtEnd.ClientID %>").val($("#<%= HF_dtEndTmp.ClientID %>").val());
        $("#<%= HF_searchTitle.ClientID %>").val($("#<%= txt_title.ClientID%>" ).val());
        RNT_fillList("list");
        return false;
    }
    var _JSCal_Range;
    function setCal() {
        _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStartTmp.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEndTmp.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }
    <%= ltr_checkCalDates.Text %>
	RNT = {};
	RNT.EstateOptions = {
		id: 0,
		path: "",
		label: "",
		pid_zone: 0
	};
	var _estateList_<%= Unique %> = new Array();
	function getEstateXml_<%= Unique %>(){
		var _xml = $.ajax({
			type: "GET",
			url: "/webservice/rnt_estate_list_xml.aspx?lang=<%= CurrentLang.ID %>&SESSION_ID=<%= CURRENT_SESSION_ID %>",
			dataType: "xml",
			success: function(xml) {
				$(xml).find('item').each(function() {
		            var _estOpt = {
			            id: parseInt($(this).find('id').text(), 10),
			            path: $(this).find('path').text(),
			            label: $(this).find('title').text(),
			            pid_zone: parseInt($(this).find('pid_zone').text(), 10)
		            };
		            _estateList_<%= Unique %>.push(_estOpt);
				});
				setAutocomplete_<%= Unique %>();
			}
		});
	}
	function setAutocomplete_<%= Unique %>(){
		$( ".search_aptComplete" ).autocomplete({
			source: _estateList_<%= Unique %>,
			search: function( event, ui ) {
				$( "#<%= HF_estateId.ClientID%>" ).val( '0' );
				$( "#<%= HF_estatePath.ClientID%>" ).val( '' );
			},
			select: function( event, ui ) {
				$( "#<%= HF_estateId.ClientID%>" ).val( ui.item.id );
				$( "#<%= HF_estatePath.ClientID%>" ).val( ui.item.path );
				if(ui.item.id!=0 && ui.item.path!='')
				$( "#zoneCont_<%= Unique %>" ).hide();
				else
				$( "#zoneCont_<%= Unique %>" ).show();
			}
		});
		//alert(_estateList_<%= Unique %>);
	}
	getEstateXml_<%= Unique %>();
	SITE_addDinamicLabel('lb_QuickSearch', '<%= txt_title.ClientID %>');
                </script>
                <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
            </div>
            <div id="more_search">
                <div class="box_more_search" style="margin-bottom: 20px;">
                    <div class="line">
                        <span class="titline">
                            <%=CurrentSource.getSysLangValue("lblPersons")%></span>
                    </div>
                    <div style="float: left; margin: 8px 8px 10px 8px; width: 235px;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" align="left" style="width: 50px;">
                                    <asp:DropDownList ID="drp_adult" runat="server" Style="width: 40px;" onchange="calculateNumPersons('adult')">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("reqAdults")%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_over" runat="server" Style="width: 40px;" onchange="calculateNumPersons('childOver')">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_min" runat="server" Style="width: 40px;" onchange="calculateNumPersons('childMin')">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <span class="nulla"></span>
                </div>
                <span class="tit">
                    <%=CurrentSource.getSysLangValue("lblAdvancedSearch")%></span>
                <div class="box_more_search">
                    <div class="line">
                        <span class="titline">
                            <%=CurrentSource.getSysLangValue("lblPrices")%></span>
                    </div>
                    <div style="float: left; margin: 8px 8px 10px 8px; width: 235px;">
                        <div class="slider-range-cont">
                            <div class="range_label">
                                <div class="minmax">
                                    <div class="min">
                                        min. &euro; <strong>
                                            <span id="lbl_prFrom"></span>
                                        </strong>
                                    </div>
                                    <div class="max">
                                        max &euro; <strong>
                                            <span id="lbl_prTo"></span>
                                        </strong>
                                    </div>
                                </div>
                            </div>
                            <div style="float: left; width: 100%;">
                                <div id="priceRange-slider">
                                </div>
                            </div>
                        </div>
                    </div>
                    <span class="nulla"></span>
                </div>
                <div class="box_more_search">
                    <div class="line">
                        <span class="titline">
                            <%=CurrentSource.getSysLangValue("lblRating")%></span>
                    </div>
                    <div style="float: left; margin: 8px 8px 10px 8px; width: 235px;">
                        <div class="slider-range-cont">
                            <div class="range_label">
                                <div class="minmax">
                                    <div class="min">
                                        min&nbsp; <strong>
                                            <span id="lbl_voteFrom"></span>
                                        </strong>
                                    </div>
                                    <div class="max">
                                        max&nbsp; <strong>
                                            <span id="lbl_voteTo"></span>
                                        </strong>
                                    </div>
                                </div>
                            </div>
                            <div style="float: left; width: 100%;">
                                <div id="voteRange-slider">
                                </div>
                            </div>
                        </div>
                    </div>
                    <span class="nulla"></span>
                </div>
                <div class="box_more_search">
                    <div class="line">
                        <span class="titline">
                            <%=CurrentSource.getSysLangValue("lblZone")%></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="checkbox" id="chk_zone_all" checked="checked" onclick="RNT_currZone_onChange('all')" />
                        <label class="titline" for="chk_zone_all">
                            All</label>
                    </div>
                    <div style="float: left; margin: 8px 8px 10px;">
                        <%= ltr_activeZonesControl.Text %>
                    </div>
                    <span class="nulla"></span>
                </div>
                <div class="box_more_search">
                    <div class="line">
                        <span class="titline">
                            <%=CurrentSource.getSysLangValue("lblAmenities")%></span>
                    </div>
                    <div style="float: left; margin: 8px 8px 10px;">
                        <%= ltr_activeConfigsControl.Text %>
                    </div>
                    <span class="nulla"></span>
                </div>
            </div>
            <%--<uc1:UC_staffInLang ID="UC_staffInLang" runat="server" />--%>
            <div class="nulla">
            </div>
        </div>
        <div class="dx">
            <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
            <div id="estateListCont">
                <div class="lista lista_dettagli">
                    <input type="hidden" name="hf_currPage" id="hf_currPage" value="<%=HF_currPage.Value %>" />
                    <input type="hidden" name="hf_voteMin" id="hf_voteMin" value="0" />
                    <input type="hidden" name="hf_voteMax" id="hf_voteMax" value="10" />
                    <input type="hidden" name="hf_voteRange" id="hf_voteRange" value="<%=HF_voteRangeTmp.Value %>" />
                    <input type="hidden" name="hf_voteTemp" id="hf_voteTemp" value="<%=HF_voteRangeTmp.Value %>" />
                    <input type="hidden" name="hf_prMin" id="hf_prMin" value="0" />
                    <input type="hidden" name="hf_prMax" id="hf_prMax" value="0" />
                    <input type="hidden" name="hf_prRange" id="hf_prRange" value="<%=HF_prRangeTmp.Value %>" />
                    <input type="hidden" name="hf_prTemp" id="hf_prTemp" value="<%=HF_prRangeTmp.Value %>" />
                    <div class="loadingSrc">
                        <span>
                            <%=CurrentSource.getSysLangValue("lblLoadingData")%>
                        </span>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
</asp:Content>
