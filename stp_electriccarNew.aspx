<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_electriccarNew.aspx.cs" Inherits="RentalInRome.stp_electriccarNew" %>

<%@ Register Src="ucMain/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>

    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar gray col-sm-4 servicesColSx">

                    <div class="servicePrices">
                        <h2 class="section-title"><%=CurrentSource.getSysLangValue("lblPrices")%></h2>
                        <ul class="categories">
                            <li>1  <%=CurrentSource.getSysLangValue("lblHour")%>:	<span class="pull-right"><strong>€25,00</strong></span></li>
                            <li>1 <%=CurrentSource.getSysLangValue("lblDay")%>:	<span class="pull-right"><strong>€120,00</strong></span></li>
                        </ul>
                    </div>

                    <div class="gray videoService">
                        <iframe width="100%" height="315" src="https://www.youtube.com/embed/ZmlKIM1Rnh4" frameborder="0" allowfullscreen></iframe>
                    </div>
                    <p class="center">
                        <a class="btn btn-default-color btn-request-info" href="contacts.html">Questions? Request informations!</a>
                    </p>
                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">

                    <h1 class="section-title"><%= ltr_title.Text %></h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">
                        <!-- BEGIN PROPERTY DETAIL LARGE IMAGE SLIDER -->
                        <div id="property-detail-large" class="owl-carousel">
                            <div class="item">
                                <img src="/images/electric-car.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto1.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto2.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto3.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto4.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto5.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto6.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto7.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto8.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto9.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto10.jpg" alt="" />
                            </div>
                        </div>
                        <!-- END PROPERTY DETAIL LARGE IMAGE SLIDER -->

                        <!-- BEGIN PROPERTY DETAIL THUMBNAILS SLIDER -->
                        <div id="property-detail-thumbs" class="owl-carousel">
                            <div class="item">
                                <img src="/images/electric-car.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto1.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto2.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto3.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto4.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto5.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto6.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto7.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto8.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto9.jpg" alt="" />
                            </div>
                            <div class="item">
                                <img src="http://www.rentalinrome.com/images/auto-elettriche/foto10.jpg" alt="" />
                            </div>
                        </div>
                        <!-- END PROPERTY DETAIL THUMBNAILS SLIDER -->

                        <div class="nulla">
                        </div>

                        <p>
                            <%=ltr_description.Text %>
                            <%-- We at Rental in Rome know very well that exploring the Eternal City is a fascinating journey through the culture and poetry which permeate Italian history. We also know that it can be difficult to move around in a big metropolis: crowded public transportation, timetables that aren’t respected, and restricted traffic zones are just some of the hindrances.
                                <br />
                            <br />
                            That is why we decided to offer our guests an exclusive, innovative and environmentally friendly service: a comfortable and elegant electric car for rent, to be delivered and picked up directly at your apartment!
                                <br />
                            <br />
                            Why rent one of our electric cars? For two simple reasons: it’s convenient and economical.
                                <br />
                            <br />
                            It’s convenient because you can drive your electric car into all the central areas which are restricted to other vehicles: Via del Corso, Via Condotti, Trastevere, Piazza Navona, Piazza del Popolo and the Spanish Steps…
                                <br />
                            <br />
                            It’s economical because of the advantageous prices: 25 Euros per hour or 120 Euros per day.
                                <br />
                            <br />
                            Moreover, you will not need to pick up or return your electric car at the car rental office, because we will deliver and pick up your car directly at your apartment or wherever you like!
                                <br />
                            <br />
                            What are you waiting for? Getting around Rome has never been this easy!
                                <br />
                            <br />
                            The Dolce Vita is waiting for you! --%>
                        </p>

                        <div class="nulla">
                        </div>

                        <hr />

                        <div class="nulla">
                        </div>

                        <!-- BEGIN ZONES SECTION -->
                        <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" currType="sxZone" />
                        <%--<div class="main col-sm-12">
                            <h3 class="section-title" data-animation-direction="from-bottom" data-animation-delay="50" style="text-align: center;">Apartments in Rome</h3>

                            <div class="grid-style1 clearfix" id="zones-home">
                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="100">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Trevi fountain</strong></span>
                                        </a>
                                        <img src="images/zone-1.jpg" alt="Trevi fountain" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Trevi fountain</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>47 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="200">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Piazza Navona</strong></span>
                                        </a>
                                        <img src="images/zone-2.jpg" alt=" Piazza Navona" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Piazza Navona</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>80 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="300">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Spanish Steps</strong></span>
                                        </a>
                                        <img src="images/zone-3.jpg" alt="Spanish Steps" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Spanish Steps</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>75 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="400">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Saint Peter</strong></span>
                                        </a>
                                        <img src="images/zone-4.jpg" alt="Saint Peter" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Saint Peter</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>104 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="500">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Colosseum</strong></span>
                                        </a>
                                        <img src="images/zone-5.jpg" alt="Colosseum" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Colosseum</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>64 Apartments</li>
                                    </ul>
                                </div>

                                <div class="item col-md-4" data-animation-direction="from-bottom" data-animation-delay="600">
                                    <div class="image">
                                        <a href="#">
                                            <span class="location">See all apartments in
                                                <br />
                                                <strong>Trastevere</strong></span>
                                        </a>
                                        <img src="images/zone-6.jpg" alt="Trastevere" />
                                    </div>
                                    <div class="price zoneNameHome">
                                        <span>Trastevere</span>
                                    </div>
                                    <ul class="amenities numAptsZoneHome">
                                        <li><i class="icon-house"></i>91 Apartments</li>
                                    </ul>
                                </div>

                            </div>

                        </div>--%>
                        <!-- END ZONES SECTION -->

                    </div>


                </div>
                <!-- End BEGIN MAIN CONTENT -->


            </div>
        </div>
    </div>
    <!-- End BEGIN CONTENT WRAPPER -->
</asp:Content>
