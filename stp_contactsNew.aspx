<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_contactsNew.aspx.cs" Inherits="RentalInRome.stp_contactsNew" %>

<%@ Register Src="/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
     <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
        <script>
            fbq('track', 'Lead');
</script>
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

                        <h2 class="section-title"><%= ltr_title.Text %></h2>
                        <%= ltr_description.Text %>
                        <%--  <div class="col-sm-12 contactsSxLine">
                            <h4>Booking office:</h4>
                            <em>Mon-Fri from 09:30 a.m. to 7:00 p.m. Sat-Sun from 10:00 a.m. to 6:00 pm (GMT + 1.00 h)</em>
                            <br />
                            <br />
                            <strong>Phone:</strong> +39 06 3220068
                            <br />
                            <strong>Email:</strong> <a href="mailto:info@rentalinrome.com">info@rentalinrome.com</a>
                            <br />
                            <strong>Fax:</strong> +39 06 23328717
                            <br />
                        </div>

                        <div class="col-sm-12 contactsSxLine">
                            <h4>For guests currently in our apartments:</h4>
                            <em>Mon-Fri from 09:30 a.m. to 7:00 p.m. Sat-Sun from 10:00 a.m. to 6:00 pm (GMT + 1.00 h)</em>
                            <br />
                            <br />
                            <strong>Phone:</strong> +39 06 3220068 
                        </div>

                        <div class="col-sm-12 contactsSxLine" style="border: none; margin-bottom: 0;">
                            <h4>Pick-Up/Excursions Service:</h4>
                            <br />
                            <strong>Phone:</strong> +39 393 9523056 
                            <br />
                            <strong>Email:</strong> <a href="mailto:pickupservice@rentalinrome.com">pickupservice@rentalinrome.com</a>
                        </div>--%>
                        <div class="nulla">
                        </div>
                    </div>

                </div>
                <!-- END LEFT CONTACTS -->

                <!-- BEGIN MAIN CONTENT -->
                <asp:UpdatePanel ID="pnlmain" class="main col-sm-8" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:breadCrumbs ID="breadcrumbs" runat="server" />
                        <div id="pnl_request_sent" runat="server" visible="false">
                            <%=CurrentSource.getSysLangValue("reqRequestSent")%>
                            <asp:HiddenField ID="HF_IdRequest" runat="server" />
                            <iframe width="0" height="0" border="0" style="width: 0; height: 0; border: none; display: none;" src="/webservice/conversionScriptsRntRequest.aspx?id=<%= HF_IdRequest.Value %>"></iframe>
                        </div>
                        <!-- ADROLL  Conversion Page -->
                        <div id="pnl_adRollScript" runat="server" visible="false">
                            <script type="text/javascript">
                                adroll_adv_id = "W5NVD6QFLRBLTMY4G444SZ";
                                adroll_pix_id = "MRHU7YJK65H5NKQVYJKNHQ";
                               
                                (function () {
                                    var oldonload = window.onload;
                                    window.onload = function () {
                                        __adroll_loaded = true;
                                        var scr = document.createElement("script");
                                        var host = (("https:" == document.location.protocol) ? "https://s.adroll.com" : "http://a.adroll.com");
                                        scr.setAttribute('async', 'true');
                                        scr.type = "text/javascript";
                                        scr.src = host + "/j/roundtrip.js";
                                        ((document.getElementsByTagName('head') || [null])[0] ||
                                         document.getElementsByTagName('script')[0].parentNode).appendChild(scr);
                                        if (oldonload) {
                                            oldonload();
                                        }
                                    };
                                }());


                            </script>
                            <script type="text/javascript">
                            
                                try {
                                    console.log('sending conversation');
                                    __adroll.record_user({ "adroll_segments": "c42f565e" })
                                } catch (err) { }
                            
                            </script>

                        </div>
                        <div id="pnl_request_cont" runat="server">

                            <h1 class="section-title"><%= contUtils.getLabel("lblFormRichiesta",App.LangID,"lblFormRichiesta") %> </h1>

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

                                <div class="col-md-6">
                                    <asp:DropDownList runat="server" ID="drp_country" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id" TabIndex="5">
                                    </asp:DropDownList>
                                    <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <span id="drp_country_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                </div>
                            </div>

                            <div class="nulla">
                            </div>

                            <!-- SELECT APARTMENTS SECTION -->

                            <h3 class="section-title"><%=CurrentSource.getSysLangValue("reqApartments")%> </h3>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <%=CurrentSource.getSysLangValue("reqAddApartmentsDesc")%>
                                </div>
                                <div class="col-sm-12">
                                    <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                                    <asp:HiddenField ID="HF_deleteId" runat="server" Value="0" />
                                    <asp:TextBox ID="txt_title" runat="server" CssClass="form-control req_aptComplete" Width="195"></asp:TextBox>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <script type="text/javascript">
                                function req_refreshMyList() {
                                    SITE_cursorWait_show();
                                    var _url = "/webservice/rnt_myPreferedEstateList.aspx";
                                    _url += "?currEstate=" + $( "#<%= HF_estateId.ClientID%>" ).val();
                                    _url += "&SESSION_ID=<%= CURRENT_SESSION_ID %>";
                                    _url += "&lang=<%= CurrentLang.ID %>";
                                    _url += "&deleteEstate=" + $( "#<%= HF_deleteId.ClientID%>" ).val();
                                    _url += "&mode=req_new";
                                    var _xml = $.ajax({
                                        type: "GET",
                                        url: _url,
                                        dataType: "html",
                                        success: function(html) {
                                            if (html != "") {
                                                $("#req_myAptList_list").html(html);
                                                $(".listItemGallery").owlCarousel({ navigation: true, slideSpeed: 300, paginationSpeed: 400, singleItem: true });
                                            }
                                            else {
                                                $("#req_myAptList_list").html(html);
                                            }
                                            SITE_cursorWait_hide();
                                        }
                                    });
                                    $( "#<%= HF_estateId.ClientID%>" ).val('0');
                                    $( "#<%= HF_deleteId.ClientID%>" ).val('0');
                                }
                                function req_deleteFromMyList(id) {
                                    $( "#<%= HF_deleteId.ClientID%>" ).val(''+id);
                                    req_refreshMyList();
                                }
                                req_refreshMyList();
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

                                function setAutocomplete_<%= Unique %>() {
                                    $(".req_aptComplete").typeahead({
                                        source: _estateList_<%= Unique %>,
                                        items: 50,
                                        displayText: function (item) {
                                            return item.label;
                                        },
                                        afterSelect: function (item) {
                                            $("#<%= HF_estateId.ClientID%>").val( item.id );
                                            $(".req_aptComplete").val('');
                                            req_refreshMyList();
                                            return false;
                                        }
                                    });

                                        $(".req_aptComplete").attr('placeholder', '<%= contUtils.getLabel("lblPropertyName", App.LangID,"") %>');
                                }

                                  
                                getEstateXml_<%= Unique %>();
                            </script>
                            <div class="list-style contacts-apt-list" id="req_myAptList_list">
                            </div>

                            <!-- END OF SELECT APARTMENTS SECTION -->

                            <div class="nulla">
                            </div>

                            <hr style="margin-bottom: 0;" />

                            <div class="nulla">
                            </div>

                            <!-- SELECT ZONES SECTION -->

                            <h3 class="section-title"><%=CurrentSource.getSysLangValue("reqOrAndArea")%> </h3>

                            <div class="col-md-12 check-list contacts-select-zone">

                                <asp:ListView ID="LV_Area" runat="server">
                                    <ItemTemplate>
                                        <div class="checkbox col-md-4">
                                            <label>
                                                <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"> </asp:Label>
                                                <asp:CheckBox ID="chkArea" runat="server" />
                                                <asp:Label ID="lbl_title" runat="server" Text='<%# Eval("title") %>' Visible="false"> </asp:Label>
                                                <%# Eval("title") %>
                                            </label>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>


                            </div>

                            <!-- END OF SELECT ZONES SECTION -->

                            <!-- SELECT PRICE RANGE SECTION -->

                            <h3 class="section-title"><%=CurrentSource.getSysLangValue("reqOrAndPriceRange")%> </h3>

                            <div class="col-md-12 check-list contacts-select-price-range">
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkPrice_1" />
                                        <asp:Literal ID="ltr_Price1" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkPrice_2" />
                                        <asp:Literal ID="ltr_Price2" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkPrice_3" />
                                        <asp:Literal ID="ltr_Price3" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkPrice_4" />
                                        <asp:Literal ID="ltr_Price4" runat="server">
                                        </asp:Literal>
                                    </label>
                                </div>
                            </div>

                            <!-- END OF SELECT PRICE RANGE SECTION -->

                            <!-- SELECT PERIOD AND GUESTS SECTION -->

                            <h3 class="section-title"><%= contUtils.getLabel("lblPeriod") %></h3>

                            <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" placeholder="Check-in" class="form-control calendar-input" />
                                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" class="form-control calendar-input" placeholder="Check-out" />
                                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                                </div>

                                <div class="checkbox traveldates-flexible">
                                    <label>
                                        <input id="chk_date_is_flexible" runat="server" tabindex="19" type="checkbox" />
                                        <%=CurrentSource.getSysLangValue("reqTravelDatesFlexible")%>
                                    </label>
                                </div>

                            </div>
                            <script type="text/javascript">
                                var _JSCal_Range_<%= Unique %>;
                                function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
                                    _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", 
                                        countCont: "#<%= txt_dtCount.ClientID %>", 
                                        countLabel:"<%= contUtils.getLabel("lblNights") %>",changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
                                }
                                <%= ltr_checkCalDates.Text %>
                            </script>
                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_dtCount" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="drp_adult" runat="server" Width="50" onchange="pgEstate_calculateNumPersons()">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-sm-3">
                                    <asp:DropDownList ID="drp_child_over" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="drp_child_min" runat="server" Width="50">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <script type="text/javascript">
                                function pgEstate_calculateNumPersons() {
                                    var _num_persons_max = parseInt($("#<%= HF_num_persons_max.ClientID %>").val(), 10);
                                    var _selNum_adult = parseInt($("#<%= drp_adult.ClientID %>").val(), 10);
                                    var _selNum_child_over = parseInt($("#<%= drp_child_over.ClientID %>").val(), 10);
                                    $("#<%= drp_child_over.ClientID %> option").remove();
                                    $("#<%= drp_child_over.ClientID %>").append("<option value='0'>   <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%></option>");
                                    for (var i = 1; i <= (_num_persons_max - _selNum_adult); i++) {
                                        $("#<%= drp_child_over.ClientID %>").append("<option value='" + i + "'>" + i + "</option>");
                                    }
                                    if (_selNum_child_over > (_num_persons_max - _selNum_adult)) _selNum_child_over = (_num_persons_max - _selNum_adult);
                                    $("#<%= drp_child_over.ClientID %>").val("" + _selNum_child_over);
                                }
                            </script>
                            <!-- END OF SELECT PERIOD AND GUESTS SECTION -->

                            <!-- SELECT EXTRAS SECTION -->

                            <h3 class="section-title"><%=CurrentSource.getSysLangValue("reqMeansOfTransport")%></h3>

                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:DropDownList ID="drp_transport" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <h3 class="section-title">Services</h3>

                            <div class="col-md-12 check-list contacts-select-services">
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkService_1" />
                                        <asp:Literal ID="ltr_service_1" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkService_2" />
                                        <asp:Literal ID="ltr_service_2" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkService_3" />
                                        <asp:Literal ID="ltr_service_3" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                                <div class="checkbox col-md-6">
                                    <label>
                                        <asp:CheckBox runat="server" ID="chkService_4" />
                                        <asp:Literal ID="ltr_service_4" runat="server">
                                        </asp:Literal>

                                    </label>
                                </div>
                            </div>

                            <!-- END OF SELECT EXTRAS SECTION -->

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

                                <!--<div>
                                <label>
                                    <input type="radio" name="privacyAccept" />
                                        I have read and accept the <a href="#">"Terms and Conditions"</a></label>
                                <br /><br />

                            </div>-->
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

            $("#drp_country_check").hide();
            $("#<%= drp_country.ClientID%>").removeClass(FORM_errorClass);
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
        
        if ($.trim($("#<%= drp_country.ClientID%>").val()) == "") {
                $("#<%= drp_country.ClientID%>").addClass(FORM_errorClass);
            $("#drp_country_check").css("display", "block");
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
        if(!_validate){
            $("#errorLi").css("display", "block");
            $.scrollTo($("#errorLi"), 500);
        }
        return _validate;
    }
    $(function() {
        $("#<% =txt_email.ClientID %>,#<% =txt_email_conf.ClientID %>").bind("cut copy paste", function(event) {
            event.preventDefault();
        });
    });

            
    </script>

</asp:Content>
