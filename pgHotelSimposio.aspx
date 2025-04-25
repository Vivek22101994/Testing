<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgHotelSimposio.aspx.cs" Inherits="RentalInRome.pgHotelSimposio_Abitart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

            <h1 class="titHotelSimposio">Hotel American Palace Eur</h1>
            <strong class="hotelAddress">Via Laurentina, 554 - 00143 Roma</strong>
            <div class="galleryHotelCont">
                <a class="browse prev left"></a>
                <div class="scrollable" id="galleryHotel">
                    <div class="items">
                        <img src="/images/hotel-simposio/american-1.jpg" alt="Hotel American Palace Eur" />
                        <img src="/images/hotel-simposio/american-2.jpg" alt="Hotel American Palace Eur" />
                        <img src="/images/hotel-simposio/american-3.jpg" alt="Hotel American Palace Eur" />
                        <img src="/images/hotel-simposio/american-4.jpg" alt="Hotel American Palace Eur" />
                        <img src="/images/hotel-simposio/american-5.jpg" alt="Hotel American Palace Eur" />
                    </div>
                </div>
                <a class="browse next right"></a>
            </div>

            <div class="nulla">
            </div>

            <div class="detailsHotelSimposio">

                <div class="txtHotelSimposio">
                   Discover “American Palace”, a 4 stars hotel, located in the heart of the business district in Rome: EUR. A historic 7 floors building which has been completely renovated, during 2010, in all the bathrooms of its double rooms. Well connected to the main centre of Rome, the Hotel is well located close to public transports and just 50 m away from the subway B (LAURENTINA).The right place to stay and find all the comforts, a warm atmosphere granted by a high quality and variety of services, and a professional and friendly staff. During winter its wide common spaces are the ideal place to spend your spare time and in summer time our elegant inner garden offers rest at our open bar.                    <br /><br />If you need accommodation in the EUR area, the charming and comfortable American Palace is the right choice. EUR is the business and commercial district in Rome, but it is a major fair and exhibition centre for congresses, conferences and big events too, close to public and private offices and branches of Italian and international big companies. Nevertheless you can also find points of cultural interest, such as the Museum of Roman Civilization, the Museum of Prehistoric Age and the Exhibition Building. In the same area you’ll have the chance to have a pleasant walk by the shores of the characteristic “Eur little lake”, an artificial one in the heart of the district. Not far the Palalottomatica, the sport building where great concerts are often held. This modern part of Rome offers you so much: from museums to functionalist architecture.
                <br /><br />
                    Hotel American Palace Eur is situated in via Laurentina 554 and it has got a central location in Rome Eur green district, near the subway station and 20 minutes away from the airport. This position is perfect for both leisure and business Guests. In order to reach the hotel from Fiumicino Airport Leonardo Da Vinci, take the train “Leonardo Express” to Termini Station, then take the blue subway (Line B) and get off at the final stop Laurentina which is just 100 m. from the hotel.
                <br /><br />
                    To reach <strong>Via del Commercio 13</strong> with public transport:<br />
 you will take the blue subway (Line B) from the stop " Laurentina"  which is just 100 m. from the hotel and get off after about 20/25 minutes  at the stop Piramide, than walk for about  600 mt. (7 minutes) and you arrive to your destination.
                </div>

                <div class="box1Col tariffeHotelSimposio">
                    <h2 class="titBarHome titTariffeHotelSimposio"><strong>Hotel's rates</strong></h2>
                    
                    <ul class="listRatesHotelSimposio">
                        <li>
                            <span>Single Room</span>
                            <em>€110,00</em>
                        </li>
                        <li>
                            <span>Double room</span>
                            <em>€155,00</em>
                        </li>
                        <li>
                            <span>Suite</span>
                            <em>€200,00</em>
                        </li>
                       
                </div>
            </div>

            <!--<a class="btnRequestInfoHotelSimposio" href="mailto:info@rentalinrome.com">Request informations about this hotel</a>

            <div class="infoCodeHotelSimposio">
                Or visit <a href="#">the official website of this Hotel</a>, and at the moment of booking insert the code:
                <span class="codeHotelSimposio">simposio2015</span>
            </div>-->

            <div class="infoCodiceHotelSimposio">
               Visit <a href="http://www.gruppoloan.it/ape/" target="_blank">the official website of this Hotel</a>, and at the moment of booking insert the code:
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
