<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="DefaultNew.aspx.cs" Inherits="RentalInRome.DefaultNew" %>

<%@ Register Src="ucMain/ucZone.ascx" TagName="ucZone" TagPrefix="uc1" %>
<%@ Register Src="~/ucMain/ucSearch.ascx" TagName="uc_Search" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />

    <style>
        /* FIX FOR DELAY OF CSS LOADING OF BIG GALLERY */
        .revslider {
            height: 500px;
        }

            .revslider > ul {
                list-style: none;
                display: none;
            }

        .newsletterName, .newsletterSurname, .newsletterEmail {
            margin-bottom: 10px!important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_spDiscountCheckHomePage" runat="server" Value="1" />

    <!-- BEGIN HOME SLIDER SECTION -->
    <div class="revslider-container">
        <div class="revslider">
            <asp:ListView ID="LVHomeGallery" runat="server">
                <ItemTemplate>
                    <li data-transition="fade" data-slotamount="5">
                        <img src="/<%# Eval("img_banner") %>" alt="" />
                        <div class="caption sfr slider-title" data-x="70" data-y="120" data-speed="800" data-start="1300" data-easing="easeOutBack" data-end="9600" data-endspeed="700" data-endeasing="easeInSine"><%# contUtils.getLabel("lblAptInCity").Replace("#aptname#",Eval("title")+"").Replace("#cityname#", CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(), App.LangID,""))  %></div>
                        <div class="caption sfl slider-subtitle" data-x="75" data-y="190" data-speed="800" data-start="1500" data-easing="easeOutBack" data-end="9700" data-endspeed="500" data-endeasing="easeInSine"><%# contUtils.getLabel("lblPricePerNight").Replace("#price#", rntUtils.rntEstate_minPrice(Eval("id").objToInt32()).objToDecimal().ToString("N2")) %></div>
                        <a href="<%#contUtils.getLabel("lblNew") +"/"+ Eval("page_path") %>" class="caption sfb btn btn-default btn-lg" data-x="75" data-y="260" data-speed="800" data-easing="easeOutBack" data-start="1600" data-end="9800" data-endspeed="500" data-endeasing="easeInSine"><%# contUtils.getLabel("reqBookNow") %></a>
                    </li>
                </ItemTemplate>
                <LayoutTemplate>
                    <ul>
                        <li id="itemPlaceholder" runat="server"></li>
                    </ul>
                </LayoutTemplate>
            </asp:ListView>
        </div>
    </div>
    <!-- END HOME SLIDER SECTION -->

    <!-- BEGIN HOME ADVANCED SEARCH -->
    <div id="home-advanced-search" class="open">
        <div id="opensearch"></div>
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <uc1:uc_Search runat="server" ID="ucSearch" />
                </div>
            </div>
        </div>
    </div>
    <!-- END HOME ADVANCED SEARCH -->

    <!-- BEGIN PROPERTIES SLIDER WRAPPER-->
    <asp:PlaceHolder ID="phSpecialOffer" runat="server">
        <div class="parallax pattern-bg" data-stellar-background-ratio="0.5">
            <div class="container">
                <div class="row">
                    <div class="col-sm-12">
                        <h1 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="color: #fff; text-shadow: 0 0 5px #000;"><%= contUtils.getLabel("lblSpecialOffers1") %></h1>

                        <div id="featured-properties-slider" class="owl-carousel fullwidthsingle" data-animation-direction="from-bottom" data-animation-delay="250">
                            <asp:ListView ID="LVSpecialOffers" runat="server">
                                <ItemTemplate>
                                    <div class="item">
                                        <div class="image">
                                            <a href="/<%# Eval("page_path") %>"></a>
                                            <img src="/<%# Eval("img_banner") %>" alt="" />
                                        </div>
                                        <div class="price">
                                            <i class="fa fa-money"></i><%# contUtils.getLabel("lblDailyPriceFrom") %>:
								            <span>€<%# Eval("spDiscountedAmount").objToDecimal().ToString("N2") %></span>
                                        </div>
                                        <div class="info">
                                            <h3><a href="/<%# Eval("page_path") %>"><%# Eval("title") %></a></h3>
                                            <p><%# Eval("summary") %></p>
                                            <a href="<%#contUtils.getLabel("lblNew") +"/"+ Eval("page_path") %>" class="btn btn-default"><%# contUtils.getLabel("lblReadMore") %></a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <!-- END PROPERTIES SLIDER WRAPPER -->

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content" style="background-image: url(/images/bg-content-home.jpg); background-position:center center; background-color:#333366;">
        <div class="container">
            <div class="row">

                <!-- BEGIN ZONES SECTION -->
                <div class="main col-sm-8">
                    <h1 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50"><%= contUtils.getLabel("lblZonesOfRome") %></h1>
                    <div class="grid-style1 clearfix" id="zones-home">
                        <uc1:ucZone ID="ucZone" runat="server" currType="zoneHome" />
                    </div>

                    <p class="center">
                        <a href="#" class="btn btn-default-color" data-grid-id="zones-home" data-load-amount="2"><%= contUtils.getLabel("Load more") + " " + contUtils.getLabel("lblZonesOfRome") %></a>
                    </p>

                </div>
                <!-- END ZONES SECTION -->

                <!-- BEGIN SIDEBAR -->
                <div class="sidebar col-sm-4">

                    <!-- BEGIN SIDEBAR ABOUT -->
                    <div class="col-sm-12">
                        <h2 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50"><%=currStp.title %></h2>
                        <p class="left" data-animation-direction="from-bottom" data-animation-delay="200">
                            <%=currStp.description %>
                            <br />
                            <br />
                            <a href="<%=CurrentSource.getPagePath("5","stp",App.LangID+"") %>"><%= contUtils.getLabel("lblReadMore") %></a>
                        </p>
                    </div>
                    <!-- END SIDEBAR ABOUT -->

                    <!-- BEGIN NEWSLETTER -->
                    <div class="col-sm-12 newsletter-cont" data-animation-direction="from-bottom" data-animation-delay="200">
                        <div id="newsletter" class="col-sm-12">
                            <h2 class="section-title">
                                <%= contUtils.getLabel("lblSubscribeNewsletter") %>
                            </h2>
                            <%--<p><%= contUtils.getLabel("lblWriteYourEmail") %>:</p>--%>

                            <div class="input-group">
                                <input type="text" placeholder="<%= contUtils.getLabel("lblName") %>" name="newsletter_name" id="newsletter_name" class="form-control newsletterName" />

                                <div id="val_newsletterName" class="errorTooltip nameError" style="left: 0pt; display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%>
                                        <!--Errore: indirizzo email già registrato.-->
                                    </div>
                                </div>

                                <input type="text" placeholder="<%= contUtils.getLabel("lblSurname") %>" name="newsletter_surname" id="newsletter_surname" class="form-control newsletterSurname" />

                                <div id="val_newsletterLastName" class="errorTooltip surnameError" style="left: 0pt; display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%>
                                        <!--Errore: indirizzo email già registrato.-->
                                    </div>
                                </div>

                                <input type="text" placeholder="<%= contUtils.getLabel("pdf_Indirizzo_email") %>" name="newsletter_email" id="newsletter_email" class="form-control newsletterEmail" />

                                <div id="val_newsletterEmail" class="errorTooltip emailError" style="left: 0pt; display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%>
                                        <!--Errore: indirizzo email già registrato.-->
                                    </div>
                                </div>
                                <div id="val_newsletterEmail_invalid" class="errorTooltip emailError" style="display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("reqInvalidEmailFormat")%>
                                        <!--Errore: indirizzo email non valido.-->
                                    </div>
                                </div>
                                <div id="val_newsletterEmail_exist" class="errorTooltip emailError" style="display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("lblErrorMailReg")%>
                                        <!--Errore: indirizzo email non valido.-->
                                    </div>
                                </div>

                                <span class="input-group-btn" id="spSubscribe">
                                    <button class="btn btn-default" type="button" onclick="newsletterSend(); return false;"><%= contUtils.getLabel("lblSubscribe") %></button>
                                </span>
                                <div id="divNewsletterSuccess" class="newslettersuccess" style="display: none;">
                                    <div>
                                        <%=CurrentSource.getSysLangValue("lblRegCompl")%>
                                        <!--Errore: indirizzo email già registrato.-->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END NEWSLETTER -->

                </div>
                <!-- END SIDEBAR -->

                <div class="nulla">
                </div>


                <!-- START FOCUS ON -->

                <div class="row focusOnHomeCont">

                    <hr />

                    <h1 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50">Focus on...</h1>

                    <div id="latest-news-slider" class="focusOnHome owl-carousel latest-news-slider" data-animation-direction="from-bottom" data-animation-delay="250">
                        <div class="item">
                            <div class="image">
                                <a href="<%= CurrentSource.getPagePath("13","stp",CurrentLang.ID.ToString()) %>"><span class="btn btn-default"><i class="fa fa-file-o"></i><%= contUtils.getLabel("lblReadMore") %> </span></a>
                                <img src="/images/focuson.jpg" alt="" />
                            </div>

                            <div class="info">
                                <h3><a href="<%= CurrentSource.getPagePath("13","stp",CurrentLang.ID.ToString()) %>"><%= CurrentSource.getPageName("13","stp",CurrentLang.ID.ToString()) %> </a></h3>
                                <p><%= AppUtils.getStpPageSumary(13,App.LangID) %> </p>
                                <a href="<%= CurrentSource.getPagePath("13","stp",CurrentLang.ID.ToString()) %>" class="btn btn-default"><%= contUtils.getLabel("lblReadMore") %> </a>
                            </div>
                        </div>

                        <div class="item">
                            <div class="image">
                                <a href="<%= CurrentSource.getPagePath("23","stp",CurrentLang.ID.ToString()) %>"><span class="btn btn-default"><i class="fa fa-file-o"></i><%= contUtils.getLabel("lblReadMore") %>  </span></a>
                                <img src="/images/refund.jpg" alt="" />
                            </div>

                            <div class="info">
                                <h3><%= CurrentSource.getSysLangValue("lblfullrefund")%> </h3>
                                <p><%= AppUtils.getStpPageSumary(23,App.LangID) %> </p>
                                <%--All our apartments are rigorously checked by our staff in order to ensure our guests a pleasant stay. However if the unit you reserved should not correspond with the contents of our website rentalinrome.com we would like to offer you our special guarantee, giving you two options with one simple telephone call to our call-center, while in Rome.--%>

                                <a href="<%= CurrentSource.getPagePath("23","stp",CurrentLang.ID.ToString()) %>" class="btn btn-default"><%= contUtils.getLabel("lblReadMore") %> </a>
                            </div>
                        </div>
                        <div class="item">
                            <div class="image">
                                <a href="<%= CurrentSource.getPagePath("10","stp",CurrentLang.ID.ToString()) %>"><span class="btn btn-default"><i class="fa fa-file-o"></i><%= contUtils.getLabel("lblReadMore") %> </span></a>
                                <img src="/images/focuson.jpg" alt="" />
                            </div>

                            <div class="info">
                                <h3><a href="<%= CurrentSource.getPagePath("10","stp",CurrentLang.ID.ToString()) %>"><%= CurrentSource.getPageName("10","stp",CurrentLang.ID.ToString()) %> </a></h3>
                                <p><%= AppUtils.getStpPageSumary(10,App.LangID) %> </p>
                                <a href="<%= CurrentSource.getPagePath("10","stp",CurrentLang.ID.ToString()) %>" class="btn btn-default"><%= contUtils.getLabel("lblReadMore") %> </a>
                            </div>
                        </div>

                        <div class="item">
                            <div class="image">
                                <a href="<%= CurrentSource.getPagePath("40","stp",CurrentLang.ID.ToString()) %>"><span class="btn btn-default"><i class="fa fa-file-o"></i><%= contUtils.getLabel("lblReadMore") %> </span></a>
                                <img src="/images/focuson.jpg" alt="" />
                            </div>

                            <div class="info">
                                <h3><a href="<%= CurrentSource.getPagePath("40","stp",CurrentLang.ID.ToString()) %>"><%= CurrentSource.getPageName("40","stp",CurrentLang.ID.ToString()) %> </a></h3>
                                <p><%= AppUtils.getStpPageSumary(40,App.LangID) %> </p>
                                <a href="<%= CurrentSource.getPagePath("40","stp",CurrentLang.ID.ToString()) %>" class="btn btn-default"><%= contUtils.getLabel("lblReadMore") %> </a>
                            </div>
                        </div>

                    </div>

                </div>

                <!-- END FOCUS ON -->

            </div>
        </div>
    </div>
    <!-- END CONTENT WRAPPER -->

    <!-- BEGIN TESTIMONIALS -->
    <div class="parallax dark-bg" style="background-image: url(images/testimonials-home.jpg);" data-stellar-background-ratio="0.5">
        <div class="container">
            <div class="row">
                <div class="col-sm-12" data-animation-direction="from-top" data-animation-delay="50">
                    <h2 class="section-title">Testimonials</h2>

                    <div id="testimonials-slider" class="owl-carousel testimonials">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END TESTIMONIALS -->


    <script type="text/javascript">
        function newsletterShow(mode) {
            if (mode == "ok") {
                $("#divNewsletterSuccess").css("display", "");
                $("#newsletter_email").val("");
                $("#newsletter_name").val("");
                $("#newsletter_surname").val("");
            }
            else if (mode == "exist") {
                $("#val_newsletterEmail_exist").css("display", "");
            }
            else if (mode == "invalid") {
                $("#val_newsletterEmail_invalid").css("display", "");
            }
            $("#spSubscribe").css("display", "");
            return;
            alert(mode);
        }

        function newsletterCheck() {
            $("#divNewsletterSuccess").css("display", "none");
            $("#val_newsletterName").css("display", "none");
            $("#val_newsletterLastName").css("display", "none");
            $("#val_newsletterEmail").css("display", "none");
            $("#val_newsletterEmail_invalid").css("display", "none");
            $("#val_newsletterEmail_exist").css("display", "none");

            if ($.trim($("#newsletter_name").val()) == "") {
                $("#val_newsletterName").css("display", "block");
                return false;
            }

            if ($.trim($("#newsletter_surname").val()) == "") {
                $("#val_newsletterLastName").css("display", "block");
                return false;
            }

            if ($.trim($("#newsletter_email").val()) == "") {
                $("#val_newsletterEmail").css("display", "block");
                return false;
            }
            if (!FORM_validateEmail($("#newsletter_email").val())) {
                $("#val_newsletterEmail_invalid").css("display", "block");
                return false;
            }
            return true;
        }

        function newsletterSend() {
            if (!newsletterCheck()) return;
            // $("#spSubscribe").css("display", "none");
            //newsletterShow("ok");
            //return;

            var _url = "http://www.rentalinrome.com/newsletter/forms/optIn_ajax.asp";
            _url += "?idList=177";
            _url += "&email=" + $("#newsletter_email").val();
            _url += "&name=" + $("#newsletter_name").val();
            _url += "&lastname=" + $("#newsletter_surname").val();
            var _xml = $.ajax({
                type: "GET",
                url: "/common/proxy.aspx?u=" + encodeURIComponent(_url),
                dataType: "html",
                success: function (html) {
                    newsletterShow(html);
                }
            });
        }
        function RNT_commentList() {
            var _url = "/webservice/rnt_estate_comment.aspx";
            _url += "?SESSION_ID=<%= CURRENT_SESSION_ID %>";
            _url += "&lang=<%= CurrentLang.ID %>";
            _url += "&numPerPage=6";
            _url += "&currPage=1";
            _url += "&inhomepage=1";
            var _xml = $.ajax({
                type: "GET",
                url: _url,
                dataType: "html",
                success: function (html) {
                    $("#testimonials-slider").html(html);
                    if ($("#testimonials-slider").length) {
                        $("#testimonials-slider").owlCarousel({
                            singleItem: true,
                            autoHeight: true,
                            mouseDrag: false,
                            transitionStyle: "fade"
                        });
                    }
                }
            });
        }



        $(window).load(function () {

            RNT_commentList();
        });
    </script>
</asp:Content>
