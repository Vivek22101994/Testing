<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/common/mpMobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RentalInRome.mobile.Default" %>
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
            <span class="priceFrom"><span class="fromTo"><%= contUtils.getLabel("lblDateFrom") %></span> <span class="data">${dateStartFormatted}</span><br/><span class="fromTo"><%= contUtils.getLabel("lblDateTo") %></span> <span class="data">${dateEndFormatted}</span></span>
            <span class="price SpOffDiscout" >
                <strong>-${priceFormatted}</strong><em>%</em>
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
            $("#listViewNoData").hide();
            $("#listViewLoding").show();
            var url = "/webservice/rntEstateOffersList.aspx";
            var jsondata = {};
            jsondata.json = "true";
            jsondata.lang = "<%= App.LangID %>";
            jsondata.SESSION_ID = "<%= CURRENT_SESSION_ID %>";
            jsondata.currZone = $("#<%= drp_zone.ClientID %>").val();
            jsondata.currPage = AjaxList_currPage;
            jsondata.numPerPage = 5;
            jsondata.orderByHow = "";
            jsondata.orderBy = "";
            jsondata.orderHow = "";
            AjaxList_currRequest = $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                data: jsondata,
                success: function (data) {
                    $("#custom-listview").show();
                    if (data.list.length == 0) {
                        $("#listViewNoData").show();
                    }
                    if (action == "first") {
                        $("#custom-listview").data("kendoMobileListView").setDataSource(new kendo.data.DataSource({ data: data.list }));
                    }
                    else {
                        $("#custom-listview").data("kendoMobileListView").append(data.list);
                    }
                    $("#listViewLoding").hide();
                    if (data.pageSize > AjaxList_currPage)
                        $("#listViewMoreButton").show();
                    else
                        $("#listViewMoreButton").hide();
                }
            });
        }
        function AjaxList_fillNext() {
            AjaxList_currPage++;
            AjaxList_fillList("list");
        } 
        function AjaxList_filter() {
            AjaxList_fillList("first");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main_top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_main" runat="server">

        <div id="mainHomeMobile" class="mainMobile">
            <asp:DropDownList ID="drp_zone" runat="server" CssClass="selectZone" onchange="AjaxList_filter()" Style="border: medium none; border-radius: 0px;">
            </asp:DropDownList>
            <h1 class="titPag"><%= contUtils.getLabel("lblSpecialOffersAndLastMinute") %></h1>
            <div class="filters">
                <!-- insert filters here -->
            </div>

            <div class="listCont" id="OffersHomeMobile specialOfferItems">
                <ul id="custom-listview" class="listview" data-role="listview" data-template="customListViewTemplate"></ul>

                <div class="nulla">
                </div>

                <div class="list-loading" id="listViewLoding"></div>
                <div class="list-nodata" id="listViewNoData" style="display: none;"><%= contUtils.getLabel("lblApartmentSearchError") %></div>
                <a class="LoadMore" id="listViewMoreButton" data-role="button" data-click="AjaxList_fillNext" style="display: none;"><%= contUtils.getLabel("ShowMoreOffers") %></a>
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
