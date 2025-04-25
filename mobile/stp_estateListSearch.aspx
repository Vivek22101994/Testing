<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/common/mpMobile.Master" AutoEventWireup="true" CodeBehind="stp_estateListSearch.aspx.cs" Inherits="RentalInRome.mobile.stp_estateListSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/x-kendo-template" id="customListViewTemplate">
    <a class="listItemMobile" href="/m${pagePath}" data-role="button" data-rel="external">
        <span class="imgCont"><img class="item-photo" src="${imgPreview}" /></span>
            <span class="listItemMobileDesc">
                <h3 class="item-title">${title}</h3>
                <span class="item-info">${locZoneFull}</span>
                <span class="nulla">
                </span> 
                <span class="listAmenities">#= extrasListFormatted #</span>
            </span>
            <span class="priceFrom"></span>
            <span class="price SpOffDiscout" >
                <strong>${priceFormatted}</strong>
                <em></em>
            </span>
        </a>
    </script>
    <script>
        mainViewInit = function() {
            AjaxList_fillList("first");
        }
        AjaxList_currPage = 1;
        var AjaxList_currRequest;
        function AjaxList_fillList(action) {
            if (AjaxList_currRequest)
                AjaxList_currRequest.abort();
            if (action == "first") {
                AjaxList_currPage = 1;
                $("#custom-listview").hide();
            }
            if (action == "flt") SITE_showLoader();
            $("#listViewNoData").hide();
            $("#listViewLoding").show();
            var _url = "/webservice/rntEstateSearch.aspx";
            _url += "?lang=<%= App.LangID %>";
            _url += "&json=true";
            _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&dtS=" + $("#<%= HF_dtStart.ClientID %>").val();
            _url += "&dtE=" + $("#<%= HF_dtEnd.ClientID %>").val();
            _url += "&numPers_adult=" + $("#<%= HF_numPers_adult.ClientID %>").val();
            _url += "&numPers_childOver=" + $("#<%= HF_numPers_childOver.ClientID %>").val();
            _url += "&numPers_childMin=" + $("#<%= HF_numPers_childMin.ClientID %>").val();
            _url += "&title=" + $("#<%= HF_searchTitle.ClientID %>").val();
            _url += "&currCity=" + $("#<%= HF_currCity.ClientID %>").val();
            _url += "&currZoneList=" + $("#<%= HF_currZoneList.ClientID %>").val();
            _url += "&currConfigList=" + $("#<%= HF_currConfigList.ClientID %>").val();
            _url += "&prRange=" + $("#<%= HF_prRange.ClientID %>").val();
            _url += "&voteRange=0|10";//  + $("#hf_voteRange").val();

            _url += "&currPage=" + AjaxList_currPage;
            _url += "&numPerPage=" + $("#<%= HF_numPerPage.ClientID %>").val();
            _url += "&orderBy=" + $("#<%= HF_orderBy.ClientID %>").val();
            _url += "&orderHow=" + $("#<%= HF_orderHow.ClientID %>").val();
            AjaxList_currRequest = $.ajax({
                type: "GET",
                url: _url,
                dataType: "json",
                success: function (data) {
                    $("#custom-listview").show();
                    if (data.list.length == 0) {
                        $("#listViewNoData").show();
                    }
                    if (action == "first") {
                        $("#custom-listview").data("kendoMobileListView").setDataSource(new kendo.data.DataSource({ data: data.list }));
                    }
                    else if (action == "list") {
                        $("#custom-listview").data("kendoMobileListView").append(data.list);
                    }
                    $("#listViewLoding").hide();
                    if (data.pageSize > AjaxList_currPage)
                        $("#listViewMoreButton").show();
                    else
                        $("#listViewMoreButton").hide();
                    fltPriceRange_fillValues(data.priceRange);
                    AjaxList_fillFltList("dropdown", "fltCityList", data.fltCityList);
                    AjaxList_fillFltList("checkbox", "fltZoneList", data.fltZoneList);
                    AjaxList_fillFltList("checkbox", "fltExtrasList", data.fltExtrasList);
                    SITE_hideLoader();
                    if (action == "first")
                        $('.km-content:visible').data('kendoMobileScroller').reset();
                }
            });
        }
        function AjaxList_fillNext() {
            AjaxList_currPage++;
            AjaxList_fillList("list");
        }
        function AjaxList_showFilters() {
            $("#lnkShowFilters").hide();
            $("#pnlListCont").hide();
            window.setTimeout(function () { $("#lnkShowList").show(); $("#pnlFiltersCont").show(); }, 100);
        }
        function AjaxList_showList() {
            $("#lnkShowList").hide();
            $("#pnlFiltersCont").hide();
            window.setTimeout(function () { AjaxList_fillList("first"); $("#lnkShowFilters").show(); $("#pnlListCont").show(); }, 100);
        }
        function fltZoneList_setValues() {
            var _arrStr = "";
            var _arrSep = "";
            $(".fltZoneList").each(function () {
                if ($(this).is(':checked')) {
                    _arrStr += _arrSep + "" + $(this).val();
                    _arrSep = "|";
                }
            });
            $("#<%= HF_currZoneList.ClientID %>").val(_arrStr);
        }
        function fltExtrasList_setValues() {
            var _arrStr = "";
            var _arrSep = "";
            $(".fltExtrasList").each(function () {
                if ($(this).is(':checked')) {
                    _arrStr += _arrSep + "" + $(this).val();
                    _arrSep = "|";
                }
            });
            $("#<%= HF_currConfigList.ClientID %>").val(_arrStr);
        }
        function setFltValues() {
            $("#<%= HF_currCity.ClientID %>").val($("#fltCityList").val());
            fltZoneList_setValues();
            fltExtrasList_setValues();
            AjaxList_fillList("flt");
        }
        function AjaxList_fillFltList(type, container, data) {
            if (type == "checkbox") {
                var html = "";
                var index = 0;
                $.each(data, function (key, value) {
                    index++;
                    html += '\
                        <div class="lineFilters lineBooking">\
                            <input type="checkbox" id="' + container + '_' + index + '" class="' + container + '" onclick="setFltValues()" value="' + value.value + '" ' + (value.selected ? ' checked="checked"' : '') + '/>\
                            <label for="' + container + '_' + index + '">' + value.text + '</label>\
                        </div>\
                       ';
                });
                if (html == "") { $("#" + container + "Title").hide(); $("#" + container + "Cont").hide(); }
                else { $("#" + container + "Title").show(); $("#" + container + "Cont").show(); }
                $("#" + container + "Cont").html(html);
            }
            else if (type == "dropdown") {
                var html = "";
                html += '<option value=""><%= "- " + contUtils.getLabel("lblAll") + " -" %></option>';
                var index = 0;
                $.each(data, function (key, value) {
                    index++;
                    html += '<option value="' + value.value + '" ' + (value.selected ? ' selected="selected"' : '') + '>' + value.text + '</option>';
                });
                if (html == "") { $("#" + container + "Title").hide(); $("#" + container + "Cont").hide(); }
                else { $("#" + container + "Title").show(); $("#" + container + "Cont").show(); }
                $("#" + container + "Cont").html("<div class='lineFilters lineBooking'><select id='" + container + "' name='" + container + "' onchange='setFltValues()'>" + html + "</select></div>");
            }
    }
    var AjaxList_priceRangeSlider;
        function fltPriceRange_fillValues(data) {
            if (AjaxList_priceRangeSlider != null) $("#priceRange-slider").slider("destroy");
            if (data.start == 0) data.start = data.min;
            if (data.end == 0) data.end = data.max;
            AjaxList_prRangeSlider = $("#priceRange-slider").slider({
                min: data.min,
                max: data.max,
                range: true,
                step: 1,
                values: [data.start, data.end],
                slide: function (event, ui) {
                    var start = ui.values[0];
                    var end = ui.values[1];
                    $("#lbl_prFrom").html("" + start);
                    $("#lbl_prTo").html("" + end);
                },
                change: function (event, ui) {
                    var start = ui.values[0];
                    var end = ui.values[1];
                    var oldValue = $("#<%= HF_prRange.ClientID %>").val();
                    var newValue = start + "|" + end;
                    $("#<%= HF_prRange.ClientID %>").val(newValue);
                    if (oldValue != "" && oldValue != "0|0" && oldValue != newValue)
                        AjaxList_fillList("flt");
                }
            });
            $("#lbl_prFrom").html(data.start == 0 ? data.min : data.start);
            $("#lbl_prTo").html(data.end == 0 ? data.max : data.end);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main_top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" />
    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
    <asp:HiddenField ID="HF_searchTitle" runat="server" Value="" />
    <asp:HiddenField ID="HF_currCity" runat="server" Value="" />
    <asp:HiddenField ID="HF_currZoneList" runat="server" Value="" />
    <asp:HiddenField ID="HF_currConfigList" runat="server" Value="" />
    <asp:HiddenField ID="HF_prRange" runat="server" Value="0|0" />
    <asp:HiddenField ID="HF_voteRange" runat="server" Value="0|10" />
    <asp:HiddenField ID="HF_numPers_adult" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPers_childOver" runat="server" Value="0" />
    <asp:HiddenField ID="HF_numPers_childMin" runat="server" Value="0" />

    <asp:HiddenField ID="HF_numPerPage" runat="server" Value="15" />
    <asp:HiddenField ID="HF_orderBy" runat="server" Value="" />
    <asp:HiddenField ID="HF_orderHow" runat="server" Value="" />

    <div id="mainHomeMobile" class="mainMobile" data-stretch="true">
        <h1 class="titPag"><%= contUtils.getLabel("ApartmentsInRome") %></h1>
        <a id="lnkShowFilters" class="filterBtn" data-role="button" data-click="AjaxList_showFilters"><%= contUtils.getLabel("Filter") %> </a>
        <a id="lnkShowList" class="btnBack" data-role="button" data-click="AjaxList_showList" style="display: none;"><%= contUtils.getLabel("lblSaveSearchFilters") %></a>
        <div class="filters" id="pnlFiltersCont" style="display: none;">
            <h2 class="titPag" style="margin: 2% 0 2% 4%;"><%= contUtils.getLabel("lblPrices") %></h2>
            <div class="nulla">
            </div>
            <div class="mobileBox boxFilters" id="PricesBox">
                <div class="slider-range-cont">
                    <div class="range_label">
                        <div class="minmax">
                            <div class="min">
                                min. € <strong>
                                    <span id="lbl_prFrom"></span>
                                </strong>
                            </div>
                            <div class="max">
                                max € <strong>
                                    <span id="lbl_prTo"></span>
                                </strong>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; width: 100%;">
                        <div id="priceRange-slider"></div>
                    </div>
                </div>
            </div>

            <div class="nulla">
            </div>

            <h2 id="fltZoneListTitle" class="titPag" style="margin: 2% 0 2% 4%;"><%=contUtils.getLabel("lblZone")%></h2>

            <div class="nulla">
            </div>

            <div class="mobileBox boxFilters" id="fltZoneListCont">

            </div>

            <div class="nulla">
            </div>

            <h2 id="fltExtrasListTitle" class="titPag" style="margin: 2% 0 2% 4%;"><%= contUtils.getLabel("lblAmenities") %></h2>

            <div class="nulla">
            </div>


            <div class="mobileBox boxFilters" id="fltExtrasListCont"></div>

            <a class="btnBigMobile btnBookNowDett" data-role="button" data-click="AjaxList_showList"><%= contUtils.getLabel("lblSaveSearchFilters") %></a>
            <div class="nulla">
            </div>
        </div>

        <div class="listCont" id="pnlListCont">
            <ul id="custom-listview" class="listview" data-role="listview" data-template="customListViewTemplate"></ul>

            <div class="nulla">
            </div>

            <div class="list-loading" id="listViewLoding"></div>
            <div class="list-nodata" id="listViewNoData" style="display: none;"><%= contUtils.getLabel("lblApartmentSearchError") %></div>
            <a class="LoadMore" id="listViewMoreButton" data-role="button" data-click="AjaxList_fillNext" style="display: none;"><%= contUtils.getLabel("Load more") %></a>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_main_bottom" runat="server">

    <style scoped>

#listview-templates .head {
    display: block;
    margin: 0 auto;
    width: 100%;
    height: 100px;
    background: url(../../content/mobile/listview/food.jpg) no-repeat center top;
    -webkit-background-size: 100% auto;
    background-size: 100% auto;
}





#custom-listview {
    margin: 0;
}

#listview-templates .details-link {
    margin-top: -1em;
    position: absolute;
    right: 0.6em;
    top: 50%;
}

#listview-templates .km-listview .km-list {
    margin: 0;
}


#listview-templates .km-group-title h2 {
    margin: 0;
    padding-top: .2em;
    text-shadow: none;
}

#listview-templates .km-group-title h2 {
    color: #974d2e;
    font-weight: normal;
    font-size: 1.4em;
    background-image: -moz-linear-gradient(center top , rgba(255, 255, 255, 0.5), rgba(255, 255, 255, 0.45) 6%, rgba(255, 255, 255, 0.2) 50%, rgba(255, 255, 255, 0.15) 50%, rgba(100, 100, 100, 0)), url(../../content/shared/images/patterns/pattern4.png);
    background-image: -webkit-gradient(linear, 50% 0, 50% 100%, color-stop(0, rgba(255, 255, 255, 0.5)), color-stop(0.06, rgba(255, 255, 255, 0.45)), color-stop(0.5, rgba(255, 255, 255, 0.2)), color-stop(0.5, rgba(255, 255, 255, 0.15)), color-stop(1, rgba(100, 100, 100, 0))), url(../../content/shared/images/patterns/pattern4.png);
}


.km-tablet .km-ios #listview-templates .km-view-title
{
    color: #fff;
    text-shadow: 0 -1px rgba(0,0,0,.5);
}
.km-wp .km-group-title .km-text {
    padding: 0;
}
.km-wp .km-group-title .km-text h2 {
    padding: 0 0 .1em .7em;
}
.km-ios7 #listview-templates .km-navbar .km-button,
.km-ios7 #listview-templates .km-navbar .km-view-title
{
    background: transparent;
    color: #fff;
}
</style>
</asp:Content>
