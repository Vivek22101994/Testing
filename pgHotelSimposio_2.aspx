<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgHotelSimposio_2.aspx.cs" Inherits="RentalInRome.pgHotelSimposio_2" %>

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

            <h1 class="titHotelSimposio">Hotel Abitart</h1>
            <strong class="hotelAddress">Via Pellegrino Matteucci 10/20 00154 Roma</strong>

            <div class="galleryHotelCont">
                <a class="browse prev left"></a>
                <div class="scrollable" id="galleryHotel">
                    <div class="items">
                        <img src="/images/hotel-simposio/abitart-1.jpg" alt="Hotel Abitart Roma" />
                        <img src="/images/hotel-simposio/abitart-2.jpg" alt="Hotel Abitart Roma" />
                        <img src="/images/hotel-simposio/abitart-3.jpg" alt="Hotel Abitart Roma" />
                        <img src="/images/hotel-simposio/abitart-4.jpg" alt="Hotel Abitart Roma" />
                        <img src="/images/hotel-simposio/abitart-5.jpg" alt="Hotel Abitart Roma" />
                    </div>
                </div>
                <a class="browse next right"></a>
            </div>

            <div class="nulla">
            </div>

            <div class="detailsHotelSimposio">

                <div class="txtHotelSimposio">
                    The <strong>Abitart Hotel </strong>in Rome is the <strong>art boutique hotel par excellence of the capital</strong>. A fine tribute to the energy of contemporary art and at the same time habitable house for <strong>businessmen</strong> tired of ordinariness,<strong> travelers</strong> who look at theworld through different eyes and <strong>artists</strong> searching for inspiration.                    <br /><br />With its irreverent industrial profile framed by greenery, <strong>Hotel Abitart</strong> could only be born here, between the districts <strong>Testaccio and Ostiense</strong>, 5 minutes by <strong>Eataly, the Macro and Casa Italo</strong>. This is the real heart of old Rome, this is theforge in turmoil all the latest creative trends.                    <br /><br />You don't expect to find such a hotel in Rome. Ours is not a museum of Art but the <strong>Art of living; welcome and familiar</strong>. Art amongst which you can read your newspaper, make your business calls, sip an aperitif or sit down to dinner with close friends as if you were at home.                    <br /><br />From Fiumicino's Leonardo Da Vinci airport, take the train from Fiumicino – metropolitan rail FM1 towards Roma-Fara Sabina and get off at Ostiense station. Follow directions for Via Pellegrino Matteucci. Hotel Abitart is 200 meters away.
                    <br /><br />
                    Walking distance 600 mt. (7 minutes) from hotel to Isa in <strong>Via del Commercio 13</strong>.
                </div>

                <div class="box1Col tariffeHotelSimposio">
                    <h2 class="titBarHome titTariffeHotelSimposio"><strong>Hotel's rates</strong></h2>
                    <strong class="subTitTariffeHotelSimposio">June 7 + 8 2015:</strong>
                    <ul class="listRatesHotelSimposio">
                        <li>
                            <span>Double room, Single use</span>
                            <em>€165,00 per room/night</em>
                        </li>
                        <li>
                            <span>Double room</span>
                            <em>€175,00 per room/night</em>
                        </li>
                        <li>
                            <span>Junior Suite (single use)</span>
                            <em>€199,00 per room/night</em>
                        </li>
                        <li>
                            <span>Junior Suite (double use)</span>
                            <em>€200,00 per room/night</em>
                        </li>
                    </ul>
                    <div class="separator">
                    </div>
                    <strong class="subTitTariffeHotelSimposio">June 9 to 14 2015:</strong>
                    <ul class="listRatesHotelSimposio">
                        <li>
                            <span>Double room, Single use</span>
                            <em>€215,00 per room/night</em>
                        </li>
                        <li>
                            <span>Double room</span>
                            <em>€235,00 per room/night</em>
                        </li>
                        <li>
                            <span>Junior Suite (single use)</span>
                            <em>€235,00 per room/night</em>
                        </li>
                        <li>
                            <span>Junior Suite (double use)</span>
                            <em>€250,00 per room/night</em>
                        </li>
                    </ul>
                </div>
            </div>

            <!--<a class="btnRequestInfoHotelSimposio" href="mailto:info@rentalinrome.com">Request informations about this hotel</a>

            <div class="infoCodeHotelSimposio">
                Or visit <a href="#">the official website of this Hotel</a>, and at the moment of booking insert the code:
                <span class="codeHotelSimposio">simposio2015</span>
            </div>-->

            <div class="infoCodiceHotelSimposio">
               Visit <a href="http://www.abitarthotel.com/" target="_blank">the official website of this Hotel</a>, and at the moment of booking insert the code:
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

