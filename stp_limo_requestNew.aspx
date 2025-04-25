<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_limo_requestNew.aspx.cs" Inherits="RentalInRome.stp_limo_requestNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
      <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    <%--    <link href="/jquery/css/ui-timepicker-custom.css" rel="stylesheet" type="text/css" rel="stylesheet" />
    <script src="/jquery/js/ui-all.min.js"></script>
    <script type="text/javascript" src="/jquery/js/ui-timepicker-custom.js"></script>--%>
    <link href="/jquery/plugin/TimePicker/jquery.timepicker.css" rel="stylesheet" />
    <script src="/jquery/plugin/TimePicker/jquery.timepicker.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <asp:HiddenField ID="HF_num_persons_max" runat="server" Value="0" />
    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer">
            <div class="row">

                <!-- BEGIN LEFT CONTACTS -->
                <div class="sidebar col-sm-4 bookingFormDetCont contactsSx">

                    <div class="gray bookingFormDet">

                        <h2 class="section-title"><%= currStp.title %></h2>
                        <%= currStp.description %>
                        <div class="nulla">
                        </div>
                    </div>
                    <div style="float: left; padding-top: 10px;">
                        <a class="freeCall" onclick="c2c=window.open('http://gol.green-online.it/c2c.php?user=29&amp;cmp=1&amp;id=1','GreenOnLine','width=350,height=525,left=15,top=15,resizable=no,toolbar=no,location=no,titlebar=no,menubar=no,dependent=yes');c2c.focus();" href="javascript:void(0);">
                            <img border="0" align="left" alt="GreenOnLine" src="http://gol.green-online.it/links.php?user=29&amp;link=1" />
                            <span>
                                <%= CurrentSource.getSysLangValue("lblCallForFree")%></span>
                        </a>
                        <a id="_lpChatBtn" href='http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&byhref=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' target='chat1220380' onclick="lpButtonCTTUrl = 'http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a&referrer='+escape(document.location); lpButtonCTTUrl = (typeof(lpAppendVisitorCookies) != 'undefined' ? lpAppendVisitorCookies(lpButtonCTTUrl) : lpButtonCTTUrl); lpButtonCTTUrl = ((typeof(lpMTag)!='undefined' && typeof(lpMTag.addFirstPartyCookies)!='undefined')?lpMTag.addFirstPartyCookies(lpButtonCTTUrl):lpButtonCTTUrl);window.open(lpButtonCTTUrl,'chat1220380','width=472,height=320,resizable=yes');return false;">
                            <img src='http://server.iad.liveperson.net/hc/1220380/?cmd=repstate&site=1220380&channel=web&&ver=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' name='hcIcon' border="0" />
                        </a>
                    </div>
                </div>
                <!-- END LEFT CONTACTS -->

                <!-- BEGIN MAIN CONTENT -->
                <asp:UpdatePanel ID="pnlmain" class="main col-sm-8" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div id="pnl_request_sent" runat="server" visible="false">
                            <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                            <asp:HiddenField ID="HF_IdRequest" runat="server" />
                        </div>


                        <div id="pnl_request_cont" runat="server">

                            <h1 class="section-title"><%= currStp.title %></h1>

                            <div id="errorLi" class="well col-md-8 alert alert-danger" style="display: none;">
                                <h3 id="errorMsgLbl">
                                    <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                                <p id="errorMsg">
                                    <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                                </p>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:DropDownList ID="drp_honorific" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txt_name_full" runat="server" Style="width: 350px;" CssClass="form-control"></asp:TextBox>
                                    <span id="txt_name_full_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txt_email" runat="server" class="form-control"></asp:TextBox>
                                    <span id="txt_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txt_email_conf" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span id="txt_email_conf_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></span>
                                </div>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txt_phone_mobile" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span id="txt_phone_mobile_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>


                            <div id="drp_trip_mode_cont" class="form-group col-md-12">
                                <div class="col-md-6">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblArrivalAndDeparture")%>*
                                    </label>
                                    <asp:DropDownList ID="drp_trip_mode" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="1" Text="Arrival to Rome"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Departure from Rome"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Arrival to Rome + Departure from Rome"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group mode_in  place">

                                <div class="col-md-12 mode_in place">
                                    <h4><%=CurrentSource.getSysLangValue("lbl_ArrivalToRome")%> </h4>
                                </div>

                                <div class="col-md-6 mode_in place" id="drp_in_place_cont">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblArrivalplace")%>
                                    </label>

                                    <asp:DropDownList ID="drp_in_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                                        <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                                        <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                                        <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_in air hide" id="drp_in_air_place_cont">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_AirportName")%>
                                    </label>
                                    <asp:DropDownList ID="drp_in_air_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
                                        <asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_in air dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_Airline")%> / <%=CurrentSource.getSysLangValue("lbl_FlightNum")%>:
                                    </label>
                                    <asp:TextBox ID="txt_in_air_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_in sea hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_SeaportName")%>
                                    </label>
                                    <asp:DropDownList ID="drp_in_sea_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
                                        <asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
                                        <asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_in sea dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblShippingCompany")%>
                                    </label>
                                    <asp:TextBox ID="txt_in_sea_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_in train hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_RailwayStationName")%>
                                    </label>
                                    <asp:DropDownList ID="drp_in_train_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_in train dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_TrainNum")%>
                                    </label>
                                    <asp:TextBox ID="txt_in_train_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_in other hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblOtherPlace")%>
                                    </label>
                                    <asp:TextBox ID="txt_in_other_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-12">
                                    <div class="col-md-6 mode_in date" id="txt_dtBirth_cont">
                                        <label>
                                            <%=CurrentSource.getSysLangValue("lblArrivalDate")%>
                                        </label>
                                        <input type="text" id="txt_in_date" class="form-control calendar-input" />
                                        <asp:HiddenField ID="HF_in_date" runat="server" />

                                    </div>
                                    <div class="col-md-6 mode_in date">

                                        <label>
                                            <%=CurrentSource.getSysLangValue("lblArrivaltime")%>
                                        </label>

                                        <input id="in_time_view1" runat="server" title="click to select the time" style="cursor: pointer;" />
                                        <%-- <img id="in_time_toggler" src="/images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />--%>
                                        <asp:HiddenField ID="HF_in_time_cont" runat="server" />
                                        <script type="text/javascript">
                                            $("#<%= in_time_view1.ClientID%>").timepicker();
                                        </script>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="col-md-6 mode_in pick" id="req_myList">

                                        <label>
                                            <%=CurrentSource.getSysLangValue("lbl_DestinationApartment")%>
                                        </label>

                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField3" runat="server" Value="0" />
                                        <asp:TextBox ID="txt_dest_apt" runat="server" CssClass="form-control" Width="195"></asp:TextBox>

                                    </div>
                                    <div class="col-md-6 mode_in pick">
                                        <label>
                                            <%=CurrentSource.getSysLangValue("lbl_OrOtherAddress")%>
                                        </label>

                                        <asp:TextBox ID="txt_dest_other" runat="server" CssClass="form-control" Width="195"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>

                            <hr style="margin: 10px;" />

                            <div class="form-group mode_out  place">
                                <div class="col-md-12 mode_out place">
                                    <h4><%=CurrentSource.getSysLangValue("lbl_DepartureFromRome")%> </h4>
                                </div>
                                <div class="col-md-6 mode_out place" id="drp_out_place_cont">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblDeparturePlace")%>
                                    </label>
                                    <asp:DropDownList ID="drp_out_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                                        <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                                        <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                                        <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_out air hide" id="drp_out_air_place_cont">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_AirportName")%>
                                    </label>
                                    <asp:DropDownList ID="drp_out_air_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
                                        <asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_out air dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_Airline")%> / <%=CurrentSource.getSysLangValue("lbl_FlightNum")%>
                                    </label>
                                    <asp:TextBox ID="txt_out_air_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_out sea hide">

                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_SeaportName")%>
                                    </label>

                                    <asp:DropDownList ID="drp_out_sea_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
                                        <asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
                                        <asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_out sea dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblShippingCompany")%>
                                    </label>
                                    <asp:TextBox ID="txt_out_sea_dett" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_out train hide">

                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_RailwayStationName")%>
                                    </label>

                                    <asp:DropDownList ID="drp_out_train_place" runat="server" onchange="checkArrivalDeparture()">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
                                        <asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mode_out train dett hide">
                                    <label>
                                        <%=CurrentSource.getSysLangValue("lbl_TrainNum")%>
                                    </label>
                                    <asp:TextBox ID="txt_out_train_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mode_out other hide">

                                    <label>
                                        <%=CurrentSource.getSysLangValue("lblOtherPlace")%>
                                    </label>

                                    <asp:TextBox ID="txt_out_other_dett" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-12">
                                    <div class="col-md-6 mode_out date">
                                        <label>
                                            <%=CurrentSource.getSysLangValue("lblDepartureDate")%>
                                        </label>
                                        <input type="text" id="txt_out_date" class="form-control calendar-input" />
                                        <asp:HiddenField ID="HF_out_date" runat="server" />
                                    </div>
                                    <div class="col-md-6 mode_out date">
                                        <label>
                                            <%= CurrentSource.getSysLangValue("lblDepartureTimeAirPortTrain")%>
                                        </label>

                                        <input id="out_time_view1" runat="server" title="click to select the time" style="cursor: pointer;" />
                                        <%--    <img id="out_time_toggler" src="/images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />--%>
                                        <asp:HiddenField ID="HF_out_time_cont" runat="server" />

                                        <script type="text/javascript">
                                            $("#<%= out_time_view1.ClientID%>").timepicker();
                                        </script>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="col-md-6 mode_out pick" id="Div18">
                                        <label>
                                            <%=CurrentSource.getSysLangValue("lblPickupApt")%>
                                        </label>

                                        <asp:HiddenField ID="HiddenField5" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField6" runat="server" Value="0" />
                                        <asp:TextBox ID="txt_pick_apt" runat="server" CssClass="form-control" Width="195"></asp:TextBox>

                                    </div>
                                    <div class="col-md-6 mode_out pick">
                                        <label>
                                            <%=CurrentSource.getSysLangValue("lbl_OrOtherAddress")%>
                                        </label>
                                        <asp:TextBox ID="txt_pick_other" CssClass="form-control" runat="server" Width="195"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <hr style="margin-bottom: 10px;" />

                            <div class="nulla">
                            </div>

                            <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>

                            <div class="col-sm-12">
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_adult" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_child_over" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_child_min" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_num_case_l" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_num_case_m" runat="server" Width="50">
                                    </asp:DropDownList>

                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="drp_num_case_s" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>

                            </div>




                            <!-- SPECIAL REQUEST SECTION -->

                            <h3 class="section-title"><%=CurrentSource.getSysLangValue("reqSpecialRequest")%> </h3>

                            <div class="form-group contacts-special-request">
                                <div class="col-sm-12">
                                    <textarea class="form-control" id="txt_note" runat="server"></textarea>
                                </div>
                            </div>
                            <!-- END OF SPECIAL REQUEST SECTION -->

                            <!-- end -->
                            <div class="col-sm-12 login complete-booking">

                                <div class="form-style">
                                    <div class="checkbox" id="chk_privacyAgree_cont">
                                        <label>
                                            <input type="checkbox" id="chk_privacyAgree" />
                                            <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>  <a href="<%= CurrentSource.getPagePath("2","stp",CurrentLang.ID+"") %>" target="_blank">" <%= CurrentSource.getPageName("2","stp",CurrentLang.ID+"") %> "</a> <%--and authorize the use of my personal data in compliance with Legislative Decree 196/03*--%>
                                        </label>
                                    </div>
                                    <span id="chk_privacyAgree_check" class="alertErrorSmall" style="display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>

                                    <div class="checkbox" id="chk_termsOfService_cont">
                                        <label>
                                            <input type="checkbox" id="chk_termsOfService" />
                                            <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%> <a href="<%= CurrentSource.getPagePath("19","stp",CurrentLang.ID+"") %>" target="_blank">"  <%= CurrentSource.getPageName("19","stp",CurrentLang.ID+"") %> "</a>
                                        </label>
                                    </div>
                                    <span id="chk_termsOfService_check" class="alertErrorSmall" style="display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></span>
                                </div>

                                <asp:LinkButton ID="lnk_send" CssClass="btn btn-fullcolor btn-lg" runat="server" OnClick="lnk_send_Click" OnClientClick="return RNT_validateRequestForm()"><span>Submit</span></asp:LinkButton>
                            </div>
                            <!-- end end -->
                        </div>
                        <!-- End BEGIN MAIN CONTENT -->
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    <!-- End BEGIN CONTENT WRAPPER -->



    <script type="text/javascript">
        // "FIXED BOOKING FORM"
        var topHeight = $("#header").height() - $("#top-info").height();

        $(window).scroll(function () {
            if ($(this).scrollTop() > (topHeight)) {
                $('.bookingFormDet').addClass("fixedBox");
                $(".bookingFormDet").width($(".bookingFormDetCont").width() - 30);
                if ($(this).width() > 768) {
                    $('.bookingFormDet').css({ "top": topHeight });
                }
            } else {
                $('.bookingFormDet').removeClass("fixedBox");
                $(".bookingFormDet").removeAttr('style');
            }
        });
        $(window).resize(function () {
            topHeight = $("#header").height();
            if ($(this).scrollTop() > (topHeight)) {
                $('.bookingFormDet').addClass("fixedBox");
                $(".bookingFormDet").width($(".bookingFormDetCont").width() - 30);
                if ($(this).width() > 768) {
                    $('.bookingFormDet').css({ "top": topHeight });
                }
            } else {
                $('.bookingFormDet').removeClass("fixedBox");
                $(".bookingFormDet").removeAttr('style');
            }
        });

    </script>


    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            $("#errorLi").hide();
            $("#txt_email_check").hide();
            $("#<%= txt_email.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_email_conf_check").hide();
            $("#<%= txt_email_conf.ClientID%>").removeClass(FORM_errorClass);
            $("#txt_name_full_check").hide();
            $("#<%= txt_name_full.ClientID%>").removeClass(FORM_errorClass);

            $("#txt_phone_mobile_check").hide();
            $("#<%= txt_phone_mobile.ClientID%>").removeClass(FORM_errorClass);


            $("#chk_privacyAgree_check").hide();
            $("#chk_privacyAgree_cont").removeClass(FORM_errorClass);
            $("#chk_termsOfService_check").hide();
            $("#chk_termsOfService_cont").removeClass(FORM_errorClass);

            if ($.trim($("#<%= txt_email.ClientID%>").val()) == "") {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_email_check").html('This field is required. Please enter a value.');
                $("#txt_email_check").css("display", "block");
                _validate = false;
            }
            else if (!FORM_validateEmail($("#<%= txt_email.ClientID%>").val())) {
                $("#<%= txt_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_email_check").html('Please enter a valid email address.');
                $("#txt_email_check").css("display", "block");
                _validate = false;
            }
        if ($.trim($("#<%= txt_email_conf.ClientID%>").val()) != $.trim($("#<%= txt_email.ClientID%> ").val())) {
                $("#<%= txt_email_conf.ClientID%>").addClass(FORM_errorClass);
            $("#txt_email_conf_check").css("display", "block");
            _validate = false;
        }
        if ($.trim($("#<%= txt_name_full.ClientID%>").val()) == "") {
                $("#<%= txt_name_full.ClientID%>").addClass(FORM_errorClass);
            $("#txt_name_full_check").css("display", "block");
            _validate = false;
        }
        if ($.trim($("#<%= txt_phone_mobile.ClientID%>").val()) == "") {
                $("#<%= txt_phone_mobile.ClientID%>").addClass(FORM_errorClass);
            $("#txt_phone_mobile_check").css("display", "block");
            _validate = false;
        }

        if (!$("#chk_privacyAgree").is(':checked')) {
            $("#chk_privacyAgree_cont").addClass(FORM_errorClass);
            $("#chk_privacyAgree_check").css("display", "block");
            _validate = false;
        }
        if (!$("#chk_termsOfService").is(':checked')) {
            $("#chk_termsOfService_cont").addClass(FORM_errorClass);
            $("#chk_termsOfService_check").css("display", "block");
            _validate = false;
        }
        if (!_validate) {
            $("#errorLi").css("display", "block");
            $.scrollTo($("#errorLi"), 500);
        }
        return _validate;
    }
    $(function () {
        $("#<% =txt_email.ClientID %>,#<% =txt_email_conf.ClientID %>").bind("cut copy paste", function (event) {
            event.preventDefault();
        });
    });


    </script>
    <script type="text/javascript">
        function checkArrivalDeparture() {
            var _modeInt = $("#<%=drp_trip_mode.ClientID %>").val();
            $(".mode_out").addClass("hide");
            $(".mode_in").addClass("hide");
            if (_modeInt == "1") {
                setMode("mode_in");
            }
            else if (_modeInt == "2") {
                setMode("mode_out");
            }
            else {
                setMode("mode_in");
                setMode("mode_out");
            }

            function setMode(_mode) {
                if (_mode == "") return;
                var _place = "";
                var _place_select = "";

                $("." + _mode + ".pick").removeClass("hide");
                $("." + _mode + ".dest").removeClass("hide");
                $("." + _mode + ".date").removeClass("hide");
                $("." + _mode + ".time").removeClass("hide");
                $("." + _mode + ".place").removeClass("hide");

                _place = _mode == "mode_in" ? $("#<%=drp_in_place.ClientID %>").val() : $("#<%=drp_out_place.ClientID %>").val();
                console.log("_place" + _place);
                if (_place != "")
                    $("." + _mode + "." + _place).removeClass("hide");
                if (_mode == "mode_in") {
                    if (_place == "air")
                        _place_select = "#<%=drp_in_air_place.ClientID %>";
                    if (_place == "sea")
                        _place_select = "#<%=drp_in_sea_place.ClientID %>";
                    if (_place == "train")
                        _place_select = "#<%=drp_in_train_place.ClientID %>";
                }
                else {
                    if (_place == "air")
                        _place_select = "#<%=drp_out_air_place.ClientID %>";
                    if (_place == "sea")
                        _place_select = "#<%=drp_out_sea_place.ClientID %>";
                    if (_place == "train")
                        _place_select = "#<%=drp_out_train_place.ClientID %>";
                }
                console.log(" place_select -- > " + $(_place_select).val());

                if (_place != "") {
                    if ($(_place_select).val() == "" || $(_place_select).val() == 'undefined') {
                        console.log("hide");
                        $("." + _mode + "." + _place + ".dett").addClass("hide");
                    }
                    else {
                        console.log("show")
                        $("." + _mode + "." + _place + ".dett").removeClass("hide");
                    }
                }
                //if ($(_place_select).val() != "" && _place != "")
                //    $("." + _mode + "." + _place + ".dett").removeClass("hide");
            }
        }



        checkArrivalDeparture();
    </script>

</asp:Content>
