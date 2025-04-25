<%@ Page Title="" Language="C#" MasterPageFile="~/mobile/common/mpMobile.Master" AutoEventWireup="true" CodeBehind="pg_rntEstateDett.aspx.cs" Inherits="RentalInRome.mobile.pg_rntEstateDett" %>

<%@ Register Src="uc/ucBooking.ascx" TagName="ucBooking" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <script type="text/x-kendo-template" id="customListViewTemplate"> 
        <span class="commentMobile commento list">
            <span class="commCont">
                <span class="userName">${nameFull}</span>
                <span class="dataComm">${dtCommentFormatted}</span>
                <span class="nulla">
                </span>
                <span class="commentoTxt">#= commentBody #</span>
            </span>
        </span>
    </script>
    <script type="text/javascript">
        function scrollviewResize() {
            //var containerWidth = $("#container").width();
            //var colSxWidth = 60 + 263;
            //var marginLeftRight = 20 * 2;
            //var paddingLeftRight = 25 * 2;
            //var mainBoxWidth = containerWidth - colSxWidth - marginLeftRight - paddingLeftRight;
            //$("#main").width(mainBoxWidth + "px");
            var width = $(window).width();
            var height = width * 480 / 640;
            $("#scrollview-container .galleryDett img").css("max-height", height + "px");
        }
    </script>
    <script>
        mainViewInit = function () {
            AjaxList_fillList("first");
        }
        applicationInit = function () {
            $(window).resize(function () { scrollviewResize(); });
            scrollviewResize();
        }
    </script>
    <script>
        AjaxList_currPage = 1;
        var AjaxList_currRequest;
        function AjaxList_fillList(action) {
            if (AjaxList_currRequest)
                AjaxList_currRequest.abort();
            if (action == "first") {
                AjaxList_currPage = 1;
                $("#guestBook").hide();
            }
            //$("#listViewNoData").hide();
            $("#listViewLoding").show();
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?action=" + action;
            _url += "&json=true";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&currEstate=" + $("#<%= HF_id.ClientID%>").val();
            _url += "&voteRange=";
            _url += "&currPage=" + $("#hf_estComment_currPage").val();
            _url += "&numPerPage=5";
            _url += "&fullView=0";
            AjaxList_currRequest = $.ajax({
                type: "GET",
                url: _url,
                dataType: "json",
                success: function (data) {
                    $("#guestBook").show();
                    if (data.list.length == 0) {
                        //$("#listViewNoData").show();
                    }
                    if (action == "first") {
                        $("#guestBook").data("kendoMobileListView").setDataSource(new kendo.data.DataSource({ data: data.list }));
                    }
                    else if (action == "list") {
                        $("#guestBook").data("kendoMobileListView").append(data.list);
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main_top" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField Value="" ID="HF_unique" runat="server" />
    <asp:HiddenField Value="" ID="HF_id" runat="server" />

    <div id="mainHomeMobile" class="mainMobile" data-stretch="true">

        <a class="btnBack" data-role="button" data-rel="external" href="<%= "/m"+CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()) %>"><%= contUtils.getLabel("lblBackToList") %></a>

        <h1 class="titBg">
            <strong class="aptName"><%= currEstateLN.title %></strong>
            <span class="zoneName"><%= CurrentSource.locZone_title(currEstateTB.pid_zone.objToInt32(), App.LangID, "") %></span>
        </h1>

        <div class="nulla">
        </div>
        <asp:ListView ID="LV_gallery" runat="server" DataSourceID="LDS_gallery">
            <ItemTemplate>
                <div class="photo" data-role="page">
                    <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("code") %>" />
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <div id="scrollview-container">
                    <div class="galleryDett" id="galleryDett" data-role="scrollview">
                        <div id="itemPlaceholder" runat="server" />
                    </div>
                </div>
            </LayoutTemplate>
        </asp:ListView>
        <asp:LinqDataSource ID="LDS_gallery" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_RL_ESTATE_MEDIAs" Where="pid_estate == @pid_estate &amp;&amp; type == @type" OrderBy="sequence">
            <WhereParameters>
                <asp:ControlParameter ControlID="HF_id" Name="pid_estate" PropertyName="Value" Type="Int32" />
                <asp:Parameter DefaultValue="gallery" Name="type" Type="String" />
            </WhereParameters>
        </asp:LinqDataSource>
        <div class="nulla">
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucBooking ID="ucBooking" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="nulla">
        </div>


        <div id="tabsDett" class="k-content">

            <div id="tabstrip">
                <ul id="tabs-view" data-index="0">
                    <li><%= contUtils.getLabel("lblDetails") %>
                    </li>
                    <li><%= contUtils.getLabel("Description") %>
                    </li>
                    <%if (!string.IsNullOrEmpty(currEstateTB.google_maps) && currEstateTB.is_google_maps == 1)
                      { %>
                    <li><%= contUtils.getLabel("lblMap") %>
                    </li>
                    <%} %>
                    <li><%= contUtils.getLabel("lblPrices") %>
                    </li>
                    <% if (LV_special_offer.Items.Count > 0)
                       { %>
                    <li><%= contUtils.getLabel("Offers") %>
                    </li>
                    <%} %>
                </ul>
                <div class="tabDett">
                    <table cellpadding="0" cellspacing="0" id="aptDetails" style="width: auto; float: left;">
                        <tr>
                            <th colspan="2" valign="middle" align="left">
                                <%= CurrentSource.getSysLangValue("lblCharacteristics")%></th>
                        </tr>
                        <% if (currEstateTB.num_persons_optional.objToInt32() != 0)
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail">
                                <%= CurrentSource.getSysLangValue("lblUpTo") + " <strong>" + currEstateTB.num_persons_max + "</strong> " + CurrentSource.getSysLangValue("lblPersons") + " (" + currEstateTB.num_persons_adult + "+" + currEstateTB.num_persons_optional + ")"%>
                            </td>
                        </tr>
                        <%}
                           else
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail">
                                <%= CurrentSource.getSysLangValue("lblSleeps") + ": " + "<strong>" + currEstateTB.num_persons_max + "</strong> "%>
                            </td>
                        </tr>
                        <%} %>

                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail">
                                <%=(currEstateTB.num_rooms_bed > 1) ? CurrentSource.getSysLangValue("lblBedRooms") + ": " + "<strong>" + currEstateTB.num_rooms_bed + "</strong> " : CurrentSource.getSysLangValue("lblBedRoom") + ": " + "<strong>" + currEstateTB.num_rooms_bed + "</strong> "%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail">
                                <%=(currEstateTB.num_rooms_bath > 1) ? CurrentSource.getSysLangValue("lblBathRooms") + ": " + "<strong>" + currEstateTB.num_rooms_bath + "</strong> " : CurrentSource.getSysLangValue("lblBathRoom") + ": " + "<strong>" + currEstateTB.num_rooms_bath + "</strong> "%>
                            </td>
                        </tr>



                        <% if (currEstateTB.num_bed_single.objToInt32() != 0)
                           { %>

                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail">
                                <%= CurrentSource.getSysLangValue("lblBedding")%>:
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail" style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_single.objToInt32() > 1) ? currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsSingle") : currEstateTB.num_bed_single.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedSingle")%>
                            </td>
                        </tr>
                        <%}%>
                        <% if (currEstateTB.num_bed_double.objToInt32() != 0)
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail" style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_double.objToInt32() > 1) ? currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDouble") : currEstateTB.num_bed_double.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDouble")%>
                            </td>
                        </tr>
                        <%}%>
                        <% if (currEstateTB.num_bed_double_divisible.objToInt32() != 0)
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail" style="background: none;">&nbsp;-&nbsp;&nbsp;<%=(currEstateTB.num_bed_double_divisible.objToInt32() > 1) ? currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedsDoubleDivisible") : currEstateTB.num_bed_double_divisible.objToInt32() + " " + CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>
                            </td>
                        </tr>
                        <%}%>
                        <% if ((currEstateTB.num_sofa_double.objToInt32()) != 0)
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail" style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstateTB.num_sofa_double.objToInt32()) > 1) ? (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsDouble") : (currEstateTB.num_sofa_double.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedDouble")%>
                            </td>
                        </tr>
                        <%}%>
                        <% if ((currEstateTB.num_sofa_single.objToInt32()) != 0)
                           { %>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="labelDetail" style="background: none;">&nbsp;-&nbsp;&nbsp;<%=((currEstateTB.num_sofa_single.objToInt32()) > 1) ? (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedsSingle") : (currEstateTB.num_sofa_single.objToInt32()) + " " + CurrentSource.getSysLangValue("lblSofaBedSingle")%>
                            </td>
                        </tr>
                        <%}%>
                        <tr>
                            <th colspan="2" valign="middle" align="left">
                                <%= CurrentSource.getSysLangValue("lblAdditionalServices")%></th>
                        </tr>
                        <tr>
                            <td colspan="2" valign="middle" align="left" class="amenities">
                                <asp:ListView ID="LV_configs" runat="server">
                                    <ItemTemplate>
                                        <img src="/<%# Eval("img_thumb") %>" title="<%# CurrentSource.rnt_configTitle(Eval("id").objToInt32(),CurrentLang.ID,"") %>" class="ico_tooltip_right" />
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div id="serviceIcons">
                                            <img id="itemPlaceholder" runat="server" alt="" />
                                        </div>
                                        <div class="nulla">
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="tabDett" style="display: none;">
                    <div class="txtTabs" style="word-spacing: normal;">
                        <%= currEstateLN.description %>
                    </div>
                </div>
                <%if (!string.IsNullOrEmpty(currEstateTB.google_maps) && currEstateTB.is_google_maps == 1)
                  { %>
                <div class="tabDett" style="display: none;">
                    <img src="http://maps.googleapis.com/maps/api/staticmap?center=<%= currEstateTB.google_maps.Replace(",",".").Replace("|",",")%>&zoom=14&size=640x400&sensor=false&markers=icon:<%=App.HOST %>/images/google_maps/google_icon_logo.png%7C<%= currEstateTB.google_maps.Replace(",",".").Replace("|",",")%>" alt="" style="width: 100%;" />
                </div>
                <%} %>
                <div class="tabDett" style="display: none;">
                    <asp:PlaceHolder ID="PH_priceOnRequestCont" runat="server" Visible="false">
                        <%= CurrentSource.getSysLangValue("lblOnRequest")%>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_priceListCont" runat="server">
                        <asp:PlaceHolder ID="PH_priceListCont_v1" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%" style="float: left; width: 100%;" class="priceTable">
                                <tr>
                                    <th colspan="7" style="border: none; background: none;">
                                        <span class="subTitleStp aptPrice">
                                            <%= CurrentSource.getSysLangValue("lblPrices")%>
                                        </span>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="border-right: none;"></th>
                                    <th colspan="2"><strong>
                                        <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong>
                                    </th>
                                    <th colspan="2" runat="server" ><strong>
                                        <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
                                    </th>
                                    <th colspan="2"><strong>
                                        <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong>
                                    </th>
                                    <th colspan="2" style="border-radius: 0 10px 0 0; -moz-border-radius: 0 10px 0 0; -webkit-border-radius: 0 10px 0 0;"><strong>
                                        <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong>
                                    </th>
                                </tr>
                                <tr>
                                    <td class="prima">
                                        <%= CurrentSource.getSysLangValue("lblPersons")%>
                                    </td>
                                    <td class="prima" colspan="2">
                                        <%= CurrentSource.getSysLangValue("lblDaily")%>
                                    </td>
                                    <td id="Td1" class="prima" runat="server" visible="false">
                                        <%= CurrentSource.getSysLangValue("lblWeekly")%>
                                    </td>
                                    <td class="prima" colspan="2">
                                        <%= CurrentSource.getSysLangValue("lblDaily")%>:
                                    </td>
                                    <td class="prima" id="Td4" runat="server" visible="false">
                                        <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                                    </td>
                                    <td class="prima" colspan="2">
                                        <%= CurrentSource.getSysLangValue("lblDaily")%>
                                    </td>
                                    <td id="Td2" class="prima" runat="server" visible="false">
                                        <%= CurrentSource.getSysLangValue("lblWeekly")%>
                                    </td>
                                    <td class="prima" colspan="2">
                                        <%= CurrentSource.getSysLangValue("lblDaily")%>
                                    </td>
                                    <td id="Td3" class="prima" runat="server" visible="false">
                                        <%= CurrentSource.getSysLangValue("lblWeekly")%>
                                    </td>
                                </tr>
                                <%= ltr_priceDetails.Text%>
                                <tr>
                                    <td style="border-left: none; border-bottom: none;" colspan="7"></td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <strong><%= ltr_price_longStayDiscount.Text%></strong>
                                    </td>
                                </tr>
                                <% if (currEstateTB.longTermRent == 1 && currEstateTB.longTermPrMonthly.objToDecimal() > 0)
                                   { %>
                                <tr>
                                    <td style="border-left: none; border-bottom: none;" colspan="7"></td>
                                </tr>
                                <tr>
                                    <th style="border-left: none; border-top: 2px solid #333366;" colspan="2"><strong>
                                        <%=CurrentSource.getSysLangValue("lblbusinesshousing")%>
                                    </strong></th>
                                    <th colspan="5" style="border-top: 2px solid #333366; background: none; text-align: left; color: #394548;">
                                        <%=CurrentSource.getSysLangValue("lblFrom") + "&nbsp;&euro;&nbsp;" + currEstateTB.longTermPrMonthly.objToDecimal().ToString("N2") + "&nbsp;" + CurrentSource.getSysLangValue("lblMonthly")%>
                                    </th>
                                </tr>
                                <% } %>
                            </table>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PH_priceListCont_v2" runat="server"></asp:PlaceHolder>
                    </asp:PlaceHolder>
                    <asp:Literal ID="ltr_priceDetails" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_price_longStayDiscount" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltr_priceTemplate_old" runat="server" Visible="false">
				            <tr>
					            <td style="border-left: none;">
						            #num_pers#
					            </td>
					            <td>
						            #pr_1#
					            </td>
					            <td>
						            #pr_1_w#
					            </td>
					            <td>
						            #pr_2#
					            </td>
					            <td>
						            #pr_2_w#
					            </td>
					            <td>
						            #pr_3#
					            </td>
					            <td>
						            #pr_3_w#
					            </td>
				            </tr>
                    </asp:Literal>
                    <asp:Literal ID="ltr_priceTemplate" runat="server" Visible="false">
				            <tr>
					            <td style="border-left: none;">
						            #num_pers#
					            </td>
					            <td colspan="2">
						            #pr_1#
					            </td>
                                 <td colspan="2">
                                     #pr_4#
                                 </td>
					            <td colspan="2">
						            #pr_2#
					            </td>
					            <td colspan="2">
						            #pr_3#
					            </td>
				            </tr>
                    </asp:Literal>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left; width: 100%;">
                        <tr>
                            <th colspan="3" style="border: none; background: none;">
                                <span class="subTitleStp aptSeasons"><%= CurrentSource.getSysLangValue("mobileSeasonReference")%> </span>
                            </th>
                        </tr>
                        <tr>
                            <th><strong>
                                <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                            </th>
                            <th><strong>
                                <%= CurrentSource.rntPeriod_title(4, CurrentLang.ID, "Medium Season")%></strong><br />
                            </th>
                            <th><strong>
                                <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                            </th>
                            <th style="border-radius: 0 10px 0 0; -moz-border-radius: 0 10px 0 0; -webkit-border-radius: 0 10px 0 0;"><strong>
                                <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                            </th>
                        </tr>
                        <tr>
                            <td style="padding: 0pt; vertical-align: top; border-bottom: medium none; border-left: medium none;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="prima">
                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                        </td>
                                        <td class="prima">
                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LVseasonDates_1" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                            <tr>
                                                <td>
                                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                                <td>
                                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </td>
                            <td style="padding: 0pt; vertical-align: top; border-bottom: medium none; border-left: medium none;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="prima">
                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                        </td>
                                        <td class="prima" style="border-right: 1px dotted #576063">
                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LVseasonDates_4" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                            <tr>
                                                <td>
                                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                                <td style="border-right: 1px dotted #576063">
                                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </td>
                            <td style="padding: 0pt; vertical-align: top; border-bottom: medium none; border-left: medium none;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="prima">
                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                        </td>
                                        <td class="prima" style="border-right: 1px dotted #576063">
                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LVseasonDates_2" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                            <tr>
                                                <td>
                                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                                <td style="border-right: 1px dotted #576063">
                                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </td>
                            <td style="padding: 0pt; vertical-align: top; border-bottom: medium none; border-left: medium none;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="prima" style="border-left: none;">
                                            <%= CurrentSource.getSysLangValue("lblDateFrom")%>
                                        </td>
                                        <td class="prima" style="border-right: 1px dotted #576063">
                                            <%= CurrentSource.getSysLangValue("lblDateTo")%>
                                        </td>
                                    </tr>
                                    <asp:ListView ID="LVseasonDates_3" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_pid_period" runat="server" Text='<%#Eval("pidPeriod") %>' Visible="false"></asp:Label>
                                            <tr>
                                                <td style="border-left: none;">
                                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                                <td style="border-right: 1px dotted #576063">
                                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #M# #yy#", CurrentLang.ID, "")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:ListView ID="LV_special_offer" runat="server">
                    <ItemTemplate>
                        <a class="commento <%# Eval("class_type") %>">
                            <span class="commCont">
                                <span class="userName">
                                    <%# Eval("title") %>&nbsp;<%# Eval("sub_title") %></span>
                                <span class="nulla"></span>
                                <span class="commentoTxt">
                                    <%= CurrentSource.getSysLangValue("lblDateFrom")%>:&nbsp;<%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "")%>&nbsp;&nbsp;-&nbsp;&nbsp;<%= CurrentSource.getSysLangValue("lblDateTo")%>:&nbsp;<%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", CurrentLang.ID,"") %>
                                </span>
                            </span>
                            <span class="alt_estate_euro">-<%# Eval("pr_discount").objToDecimal().ToString("N2") %>&nbsp;% </span>
                        </a>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="tabDett" style="display: none;">
                            <div id="specOffert">
                                <h3 class="titBar">
                                    <asp:Literal ID="ltr_title" runat="server"></asp:Literal>
                                </h3>
                                <div id="ListspecOffert">
                                    <a id="itemPlaceholder" runat="server" />
                                </div>
                            </div>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>

        </div>

        <div class="nulla">
        </div>

        <h1 class="titPag" style="margin: 2% 0 2% 4%;"><%= contUtils.getLabel("lblGuestbook") %></h1>
        <div class="nulla">
        </div>

        <ul class="mobileBox" id="guestBook" data-role="listview" data-template="customListViewTemplate" style="list-style: none outside none;">
        </ul>
        <div class="list-loading" id="listViewLoding"></div>
        <a class="LoadMore" id="listViewMoreButton" data-role="button" data-click="AjaxList_fillNext" style="display: none;"><%= contUtils.getLabel("Load more") %></a>

    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_main_bottom" runat="server">
    <script>
        try {
            $("#tabs-view").kendoMobileButtonGroup({
                select: function () {
                    $("#tabstrip .tabDett").hide()
                             .eq(this.selectedIndex)
                             .show();
                },
                index: 0
            });
        } catch (ex) { alert(ex); }
    </script>
</asp:Content>
