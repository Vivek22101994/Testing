<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgHotelSimposio_3.aspx.cs" Inherits="RentalInRome.pgHotelSimposio_3" %>

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

            <h1 class="titHotelSimposio">Hotel Pulitzer</h1>
            <strong class="hotelAddress">Via Guglielmo Marconi, 905, 00146 Roma</strong>

            <div class="galleryHotelCont">
                <a class="browse prev left"></a>
                <div class="scrollable" id="galleryHotel">
                    <div class="items">
                        <img src="/images/hotel-simposio/pulitzer-1.jpg" alt="Hotel Pulitzer" />
                        <img src="/images/hotel-simposio/pulitzer-2.jpg" alt="Hotel Pulitzer" />
                        <img src="/images/hotel-simposio/pulitzer-3.jpg" alt="Hotel Pulitzer" />
                        <img src="/images/hotel-simposio/pulitzer-4.jpg" alt="Hotel Pulitzer" />
                        <img src="/images/hotel-simposio/pulitzer-5.jpg" alt="Hotel Pulitzer" />
                    </div>
                </div>
                <a class="browse next right"></a>
            </div>

            <div class="nulla">
            </div>

            <div class="detailsHotelSimposio">

                <div class="txtHotelSimposio">
                   Hotel Pulitzer, a <strong>4-star design hotel</strong>, is located in the heart of Rome's financial district, between the Ostiense and EUR districts.                    <br />
It has an <strong>elegant and sophisticated interior</strong>, in <strong>contemporary chic</strong> style, enhanced by decorative elements and <strong>works of art</strong> inspired by the seventies.
                <br />
                    Due to its <strong>strategic position</strong> close to the main underground <strong>railway lines</strong>, the Hotel is only a few minutes from all the most famous sights of Rome.
                    <br />
                    Hotel Pulitzer has <strong>83 rooms</strong>, distributed on 6 floors; the rooms, most of them with a balcony, are distinguished by their practical features, and comfort as well as the care and attention given to details to ensure that guests have a pleasant and unforgettable stay.
                    <br />
                    An ideal Hotel not only to <strong>stay during leisure and business trips</strong> to Rome, but also for holding talks and meetings, light lunches, business luncheons and dinners, in the two dining areas: the <strong>restaurant and lounge bar</strong> on the reception floor and the <strong>banquet hall and breakfast room</strong> on floor -1.
                    <br />
                    The hotel also has <strong>two meeting rooms</strong> equipped with the latest <strong>technologies</strong> and <strong>WI-FI connection at no extra charge</strong>.
                    <br />
                    For <strong>fitness</strong> enthusiasts there is a very well-equipped exercise room.
                    <br />
                    The <strong>Hotel Pulitzer is part of the <a href="http://www.planetariahotels.com" target="_blank">Planetaria Hotels Group</a></strong>, an Italian hotel chain that has luxury four or five-star establishments all over Italy located in the heart of some of the art cities and old charming small towns.
                    <br /><br />
                    Hotel Pulitzer is in Rome near the very modern <strong>EUR district</strong>, the Capital's financial centre, and an important area connecting the city centre with the new Rome Exhibition Centre and the airport in Fiumicino.
                    <br />
                    Opposite the hotel you will find a <strong>bus stop</strong> where you can catch a bus that in just a few minutes takes you to Palazzo della Civiltà Italiana (also known as the Square Colosseum) the symbol and beating heart of Rome's EUR district.
                    <br />
                    Just 70 metres from the hotel you will find the <strong>Marconi station of the underground railway</strong> which in just a few minutes takes you either to the centre of Rome or to EUR financial district.
                    <br />
                    The <strong>Roma Termini train station</strong> is only <strong>10 minutes</strong> away by underground railway (7 stops - out from the Metro, take left), while you can also get to the <strong>Colosseum in 5 minutes</strong> (5 stops - out from the Metro, take left). From there you can visit all the sights of the Italian capital, immersing yourselves in culture, history and art. 
                    <br />
                    Not to be underestimated is the vicinity of the entrance A90 to the <strong>A1 motorway</strong> which in just a few minutes takes you to Rome's Fiumicino Airport.
                    <br /><br />
                    To reach <strong>Via del Commercio 13</strong> with public transport:<br />
 you will take the blue subway (Line B) from the stop " Eur Marconi"  which is just 200 m. from the hotel and get off after about 15/20 minutes  at the stop Piramide, than walk for about  600 mt. (7 minutes) and you arrive to your destination.
                </div>

                <div class="box1Col tariffeHotelSimposio">
                    <h2 class="titBarHome titTariffeHotelSimposio"><strong>Hotel's rates</strong></h2>
                    <ul class="listRatesHotelSimposio">
                        <li>
                            <span>Min. Price:</span>
                            <em>€150,00 per room/night</em>
                        </li>
                        <li>
                            <span>Max. Price:</span>
                            <em>€300,00 per room/night</em>
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
               Visit <a href="http://www.hotelpulitzer.it/" target="_blank">the official website of this Hotel</a>, and at the moment of booking insert the code:
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

