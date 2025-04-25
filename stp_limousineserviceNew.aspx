<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_limousineserviceNew.aspx.cs" Inherits="RentalInRome.stp_limousineserviceNew" %>

<%@ Register Src="/uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="ucBreadcrumbs" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
      <title><%= currStp.meta_title %></title>
    <meta name="description" content="<%=currStp.meta_description %>" />
    <meta name="keywords" content="<%=currStp.meta_keywords %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer">
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar gray col-sm-4 servicesColSx pickupPricesDx">

                    <div class="servicePrices">
                        <h2 class="section-title"><%=CurrentSource.getSysLangValue("lblPickupInfoPrices")%>  </h2>



                        <%--   <table cellpadding="0" cellspacing="0" class="table-bordered tablePrices">
                            <tbody>
                                <tr>
                                    <th colspan="4" valign="middle" align="center">Arrivals/Departures from/to
                                        <br />
                                        <em>Fiumicino / Ciampino Airports</em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>1-3 pax</strong><br />
                                        €50,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>4-5 pax</strong><br />
                                        €65,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>6 pax</strong><br />
                                        €75,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>7-8 pax</strong><br />
                                        €85,00
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4" valign="middle" align="center">Arrivals/Departures from/to
                                        <br />
                                        <em>Railway stations</em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>1-3 pax</strong><br />
                                        €40,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>4-5 pax</strong><br />
                                        €48,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>6 pax</strong><br />
                                        €58,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>7-8 pax</strong><br />
                                        €68,00
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4" valign="middle" align="center">
                                        <em>Round Trip Transfer </em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>1-3 pax</strong><br />
                                        €95,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>4-5 pax</strong><br />
                                        €120,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>6 pax</strong><br />
                                        €140,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>7-8 pax</strong><br />
                                        €160,00
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table cellpadding="0" cellspacing="0" class="table-bordered tablePrices">
                            <tbody>
                                <tr>
                                    <th colspan="3" valign="middle" align="center">Arrivals/Departures from/to
                                        <br />
                                        <em>Civitavecchia Port</em></th>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <strong>1-2 pax</strong><br />
                                        €150,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>3-5 pax</strong><br />
                                        €170,00
                                    </td>
                                    <td valign="middle" align="center">
                                        <strong>6-8 pax</strong><br />
                                        €190,00
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>

                        <table class="table-bordered tablePrices" cellspacing="0" cellpadding="0">
                            <tr>
                                <th colspan="4" valign="middle" align="center"><%=CurrentSource.getSysLangValue("lblArrivalsDepartures")%>
                                    <br />
                                    <em><%=CurrentSource.getSysLangValue("lblAirports")%> </em></th>
                            </tr>
                            <tr>
                                <td valign="middle" align="center">
                                    <strong>1-3 <%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    €55,00
                                </td>
                                <td valign="middle" align="center">
                                    <strong>1-4 <%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    €65,00
                                </td>
                                <td valign="middle" align="center">
                                    <strong>1-6<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    €75,00
                                </td>
                                <td valign="middle" align="center">
                                    <strong>1-8 <%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    €85,00
                                </td>
                            </tr>

                            <tr>

                                <th colspan="4" valign="middle" align="center"><%=CurrentSource.getSysLangValue("lblArrivalsDepartures")%>
                                    <br />
                                    <em><%=CurrentSource.getSysLangValue("lblRailwayStation")%></em></th>
                            </tr>

                            <tr>
                                <td valign="middle" align="center">
                                    <strong>1-3 <%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    40,00€</td>
                                <td valign="middle" align="center"><strong>1-4<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    45,00€</td>
                                <td valign="middle" align="center"><strong>1-6<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    50,00€</td>
                                <td valign="middle" align="center"><strong>1-8 <%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    55,00€</td>
                            </tr>                            
                        </table>

                        <table class="table-bordered tablePrices" cellspacing="0" cellpadding="0">
                            <tr>
                                <th colspan="4" valign="middle" align="center">
                                    <%=CurrentSource.getSysLangValue("lblArrivalsDepartures")%>
                                    <br />
                                    <em><%=CurrentSource.getSysLangValue("lblCivitavecchia")%> </em>
                                </th>
                            </tr>
                            <tr>
                                <td valign="middle" align="center"><strong>1-2<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    150,00€
                                </td>
                                <td valign="middle" align="center"><strong>1-4<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    160,00€
                                </td>
                                <td valign="middle" align="center"><strong>1-6<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    170,00€
                                </td>
                                <td valign="middle" align="center"><strong>1-8<%=CurrentSource.getSysLangValue("lblPax")%></strong><br />
                                    180,00€
                                </td>
                            </tr>
                        </table>
                        <uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="6" />
                        <%--<ul class="pricesMoreInfoList">
                            <li>For Arrivals/Departures between 10.30pm and 6.00am an additional charge of 20,00€ is required.</li>
                            <li>Our rates include a 1h30 minute waiting time after the flight has officially landed and parking fees. Over 1h30 minutes of waiting we will charge 25,00 euros so please in any case contact us in order to inform the driver.</li>
                            <li>Cancellation of your reservation is free up to 24 hours prior to the hour of the service booked. 100% of the rate will be due in case of NO SHOW or late cancellation.</li>
                        </ul>

                        For more info call +39 393 9523056 or send a mail to <a href="mailto:pickupservice@rentalinrome.com" target="_blank">pickupservice@rentalinrome.com</a>
                        <br />
                        <br />--%>
                    </div>
                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc1:ucBreadcrumbs ID="ucBreadcrumbs" runat="server" />
                    <h1 class="section-title"><%= currStp.title %> </h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">

                        <p><%= currStp.description  %> </p>
                        <uc1:UC_static_block ID="UC_static_block2" runat="server" BlockID="5" />
                    </div>

                    <div class="col-md-12">
                        <iframe width="100%" height="323" src="https://www.youtube.com/embed/QuME84eQOBg" frameborder="0" allowfullscreen></iframe>
                    </div>

                    <div class="nulla">
                    </div>

                    <p class="center" style="margin-top: 15px;">
                        <a class="btn btn-fullcolor" style="margin: 10px;" href="<%=CurrentSource.getPagePath("32","stp",CurrentLang.ID+"") %>"><%=CurrentSource.getSysLangValue("lblPickupReservation")%> </a>


                        <a class="btn btn-default-color" href="<%=CurrentSource.getPagePath("15", "stp", CurrentLang.ID.ToString()) %>" style="margin: 10px;">
                            <%=CurrentSource.getSysLangValue("lblRentAnElectricCar")%> 
                        </a>

                        <a class="btn btn-default-color" href="<%=CurrentSource.getPagePath("17", "stp", CurrentLang.ID.ToString()) %>" style="margin: 10px;">
                            <%=CurrentSource.getSysLangValue("lblTermsAndConditions")%> 
                        </a>
                    </p>
                    <hr style="margin-top: 25px;" />

                    <div class="nulla">
                    </div>

                </div>


            </div>
            <!-- End BEGIN MAIN CONTENT -->
            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar gray col-sm-4 servicesColSx pickupPricesDx" style="margin-bottom: 25px;">

                    <div class="servicePrices">

                        <h2 class="section-title"><%=CurrentSource.getSysLangValue("lblPrice")%> *</h2>
                        <ul class="categories">
                            <li>1-3 <%=CurrentSource.getSysLangValue("lblPax")%> (4 <%=CurrentSource.getSysLangValue("lblHours")%>):	<span class="pull-right"><strong>€150,00</strong></span></li>
                            <li>4-6 <%=CurrentSource.getSysLangValue("lblPax")%> (4 <%=CurrentSource.getSysLangValue("lblHours")%>):	<span class="pull-right"><strong>€175,00</strong></span></li>
                        </ul>

                        <em>*<%=CurrentSource.getSysLangValue("lblCancellationOfYourReservation")%> </em>
                        <br />
                        <br />

                    </div>
                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">

                    <h2 class="section-title" style="margin-top: 0;"><%=CurrentSource.getSysLangValue("lblPrivateExcursionsAndCityTours")%> </h2>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">

                        <%-- <p>
                            With great pleasure we would like to propose you our selected special City Tour in order to enjoy an original and unforgettable time while discovering Rome’s charm and beauty.
                            <br />
                            This special tour has been planned to give you the opportunity to enjoy a global view of the city and easily visit the main attractions in a comfortable way.
                            <br />
                            Our driver will pick you up and drop you off at any location in Rome or directly at your accommodation.
                            <br />
                            The enormous advantage of this tour consists in the fact that you can move around in a quick and practical while sitting in a comfortable and air conditioned car that has free access to the restrict historical centre area.
                            <br />
                            You will have the chance to decide to make your own tour or be guided by our driver who will select the places according to your needs.
                            <br />
                            You will be free to stop, to get off or on the car right in front of the monuments so that you can visit them or have a close look.
                            <br />
                            Moreover, on request, our driver can recommend and bring you to traditional places where you can taste the real and original flavours of the Roman cuisine or suggest you the most characteristic places of the city where to have a pleasant break and enjoy an ice cream or a special coffee.
                            <br />
                            Here below we have selected the most interesting places for you so you can plan your own personalised tour: 
                            <br />
                            <br />
                        </p>

                        <ul class="zonesTours">
                            <li>Piazza Venezia</li>
                            <li>Campidoglio </li>
                            <li>Imperial Forums </li>
                            <li>Pantheon </li>
                            <li>Piazza Navona</li>
                            <li>Campo de’ Fiori</li>
                            <li>St. Peter’s Basilica</li>
                            <li>Colosseum</li>
                            <li>Spanish Steps</li>
                            <li>Villa Borghese</li>
                            <li>Piazza del Popolo</li>
                            <li>Trevi Fountain</li>
                        </ul>--%>

                        <uc1:UC_static_block ID="UC_static_block7" runat="server" BlockID="7" />

                        <div class="nulla">
                        </div>

                    </div>

                    <div class="nulla">
                    </div>

                    <hr />

                    <div class="nulla">
                    </div>

                    <div class="list-style" id="tours-listing">

                        <asp:ListView ID="LV_tour" runat="server" DataSourceID="LDS_tour">
                            <ItemTemplate>
                                <div class="item col-md-4">
                                    <a class="image" href="/<%# Eval("page_path") %>">
                                        <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("title") %>" />
                                    </a>
                                    <div class="info">
                                        <h3>
                                            <a href="/<%# Eval("page_path") %>" rel="nofollow"><%# Eval("title") %> </a>
                                        </h3>
                                        <p><%# Eval("summary") %></p>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <a id="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:HiddenField ID="HF_lang" runat="server" Value="0" />
                        <asp:LinqDataSource ID="LDS_tour" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_VIEW_TOURs" Where="is_acitve == 1 && pid_lang == @pid_lang">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="HF_lang" Name="pid_lang" PropertyName="Value" Type="Int32" />
                            </WhereParameters>
                        </asp:LinqDataSource>

                        <%--   <div class="item col-md-4">
                            <a class="image" href="tour-details.html">
                                <img src="images/ostia.jpg" alt="" />
                            </a>

                            <div class="info">
                                <h3>
                                    <a href="tour-details.html" rel="nofollow">Ostia Antica</a>

                                </h3>

                                <p>Ostia Antica’s archaeological area is about 30 km away from the centre of Rome...</p>

                            </div>
                        </div>

                        <div class="item col-md-4">
                            <a class="image" href="tour-details.html">
                                <img src="images/tivoli.jpg" alt="" />
                            </a>

                            <div class="info">
                                <h3>
                                    <a href="tour-details.html" rel="nofollow">Tivoli</a>

                                </h3>

                                <p>Our first stage will be the beautiful Villa Adriana.  You'll be dropped off at the entrance and the driver will wait for you outside.</p>

                            </div>
                        </div>

                        <div class="item col-md-4">
                            <a class="image" href="tour-details.html">
                                <img src="images/castelli.jpg" alt="" />
                            </a>

                            <div class="info">
                                <h3>
                                    <a href="tour-details.html" rel="nofollow">Castelli Romani</a>

                                </h3>

                                <p>The Castelli Romani area is located 35 km south from Rome and represents one of the favourite destinations of the Romans for out of town trips, because it’s ideally situated.</p>

                            </div>
                        </div>

                        <div class="item col-md-4">
                            <a class="image" href="tour-details.html">
                                <img src="images/cerveteri.jpg" alt="" />
                            </a>

                            <div class="info">
                                <h3>
                                    <a href="tour-details.html" rel="nofollow">Cerveteri - Bracciano</a>

                                </h3>

                                <p>Our first stage will be the ancient Cerveteri archeological site. You'll be dropped off at the entrance and the driver will wait for you outside.</p>

                            </div>
                        </div>

                        <div class="item col-md-4">
                            <a class="image" href="tour-details.html">
                                <img src="images/civita.jpg" alt="" />
                            </a>

                            <div class="info">
                                <h3>
                                    <a href="tour-details.html" rel="nofollow">Bolsena - Civita</a>

                                </h3>

                                <p>Bolsena is a town and comune of Italy, in the province of Viterbo in northern Lazio on the eastern shore of Lake Bolsena.</p>

                            </div>
                        </div>--%>
                    </div>

                </div>


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
</asp:Content>
