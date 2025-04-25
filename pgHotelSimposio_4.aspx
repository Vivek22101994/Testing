<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgHotelSimposio_4.aspx.cs" Inherits="RentalInRome.pgHotelSimposio_4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/css/style-simposio2015.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/style_new.css" rel="stylesheet" type="text/css" />
    <script src="/jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="hotelSimposioContainer">

            <h1 class="titHotelSimposio">Hotel Santa Prisca</h1>
            <strong class="hotelAddress">Largo Manlio Gelsomini 25 00153 Roma </strong>

            <div class="galleryHotelCont">
                <a class="browse prev left"></a>
                <div class="scrollable" id="galleryHotel">
                    <div class="items">
                        <img src="/images/hotel-simposio/santaprisca-1.jpg" alt="Hotel Santa Prisca" />
                        <img src="/images/hotel-simposio/santaprisca-2.jpg" alt="Hotel Santa Prisca" />
                        <img src="/images/hotel-simposio/santaprisca-3.jpg" alt="Hotel Santa Prisca" />
                        <img src="/images/hotel-simposio/santaprisca-4.jpg" alt="Hotel Santa Prisca" />
                        <img src="/images/hotel-simposio/santaprisca-5.jpg" alt="Hotel Santa Prisca" />
                    </div>
                </div>
                <a class="browse next right"></a>
            </div>

            <div class="nulla">
            </div>

            <div class="detailsHotelSimposio">

                <div class="txtHotelSimposio">
                   The Hotel Santa Prisca is located in Rome just a short distance from the FAO office behind the Coliseum and less than a ten minute walk from the Vatican City.
The hotel consists of an elegant three-story building surrounded by a beautiful park and is on a street that leads to the characteristic and picturesque market of Porta Portese, across from the old Testaccio neighbourhood with famous restaurants and bars.
The hotel has a spacious terrace that faces the foot of the Aventino hills, transportation service to and from the airports, and guided tours of the city with interpreters. Guests also have access to laundry service, baby sitting service, nightly porter service, restaurant reservation service, hair stylists, museums, galleries, and the chance to take tours of other cities.
<br /><br />
                    <h3 class="titBarra" style="margin-bottom:10px;">Services included in the price</h3>
                    <ul style="padding-left:15px;">
                        <li>24 Hour Reception Desk</li>
                        <li>Air-conditioning in public areas</li>
                        <li>Children’s holiday facilities / Children’s holidays</li>
                        <li>Excursions</li>
                        <li>Internal parking in private garage</li>
                        <li>International Cuisine</li>
                        <li>Left-luggage Facilities</li>
                        <li>Lift</li>
                        <li>Multilingual staff</li>
                        <li>Porter</li>
                        <li>Reading lounge</li>
                        <li>Tour Desk</li>
                        <li>Tourist Information Offices</li>
                        <li>Typical local cuisine</li>
                    </ul>
                    <br />
                    <h3 class="titBarra" style="margin-bottom:10px;">Restaurant and bar</h3>

                    The restaurant serves typical regional dishes prepared with simplicity and authenticity, as well as some international influences. <br />
Artisan desserts, fresh seasonal fruit and the wine cellar with classic Italian labels contribute to a complete meal ideal for business lunches or dinners, as well as romantic meals.
                    <br /><br />
                    <h3 class="titBarra" style="margin-bottom:10px;">How to reach the hotel</h3>
                    <strong>By Car</strong><br />
                    From the motorways, take the Grande Raccordo Anulare to Exit 28 on Via Ostiense, Piazza Porta San Paolo, then take Via Marmorata Largo Manlio Gelsomini.
By train

                    <br /><br />
                    <strong>By Train</strong><br />
                    The local railway station is Rome Termini Station.
                    <br /><br />
                    <strong>By Plane</strong><br />
                    The local airport is Rome Fiumicino International Airport.
                    <br /><br />
                    To reach <strong>Via del Commercio 13</strong> with pubblic trasport:<br />
                    you will take the bus 23 near the hotel and arrive at you destination in 10 minutes.<br />
                    Walking distance 1,3 km.
                </div>

                <div class="box1Col tariffeHotelSimposio">
                    <h2 class="titBarHome titTariffeHotelSimposio"><strong>Hotel's rates</strong></h2>
                    <strong class="subTitTariffeHotelSimposio">June 7 + 8 2015:</strong>
                    <ul class="listRatesHotelSimposio">
                        <li>
                            <span>Single Room</span>
                            <em>€95,00</em>
                        </li>
                        <li>
                            <span>Double Room</span>
                            <em>€115,00</em>
                        </li>
                        <li>
                            <span>Triple Room</span>
                            <em>€145,00</em>
                        </li> 
                </div>
            </div>

            <!--<a class="btnRequestInfoHotelSimposio" href="mailto:info@rentalinrome.com">Request informations about this hotel</a>

            <div class="infoCodeHotelSimposio">
                Or visit <a href="#">the official website of this Hotel</a>, and at the moment of booking insert the code:
                <span class="codeHotelSimposio">simposio2015</span>
            </div>-->

            <div class="infoCodiceHotelSimposio">
               Visit <a href="http://www.hotelsantaprisca.it/" target="_blank">the official website of this Hotel</a>, and at the moment of booking insert the code:
            </div>

            <span class="codiceHotelSimposio">A M ISPSO 2015</span>

        <div class="nulla">
        </div>
        </div>

    </form>

        <script type="text/javascript">
            $(document).ready(function () {

                var root = $("#galleryHotel").scrollable({ circular: true }).autoscroll({ autoplay: true });

            });

        </script>
</body>
</html>

