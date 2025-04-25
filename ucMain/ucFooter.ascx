<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFooter.ascx.cs" Inherits="RentalInRome.ucMain.ucFooter" %>
<!-- BEGIN FOOTER -->
<footer id="footer">
    <div id="footer-top" class="container">
        <div class="row">
            <div class="block col-sm-3">
                <a href="<%= CurrentSource.getPagePath("1","stp",App.LangID.ToString())  %>">
                    <img src="/images/logo.png" alt="Rental in Rome" /></a>

            </div>
            <div class="block col-sm-3">
                <h3>Contacts </h3>
                <ul class="footer-contacts">
                    <li style="padding-top: 1px; line-height: 15px;">
                        <i class="fa fa-map-marker"></i>
                        <strong><%= CurrentSource.getSysLangValue("lblufficiooperativo")%>:</strong> Via Marianna Dionigi, 57 - 00193 Roma
                    </li>
                    <li style="padding-top: 1px; line-height: 15px;">
                        <i class="fa fa-map-marker"></i>
                        <strong><%= CurrentSource.getSysLangValue("lblufficioamm")%>:</strong> Via Appia Nuova, 677 - 00179 Roma 
                    </li>
                    <li><i class="fa fa-phone"></i>+39 06 3220068</li>
                    <li><i class="fa fa-envelope"></i><a href="mailto:info@rentalinrome.com">info@rentalinrome.com</a></li>
                </ul>
            </div>
            <div class="block col-sm-3">
                <h3>Info</h3>
                <ul class="footer-links">
                    <li><a href="<%=CurrentSource.getPagePath("5", "stp", CurrentLang.ID.ToString()) %>" rel="nofollow"><%= CurrentSource.getSysLangValue("lblAboutUs")%></a>
                    </li>
                    <li><a href="<%=CurrentSource.getPagePath("16", "stp", CurrentLang.ID.ToString()) %>">FAQ</a>
                    </li>
                    <li><a href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("menu_Press")%></a>
                    </li>
                    <li><a href="<%=CurrentSource.getPagePath("21", "stp", CurrentLang.ID.ToString()) %>"><%=CurrentSource.getSysLangValue("reqServices")%></a>
                    </li>
                    <%-- <li><a href="#">Guestbook</a>
                    </li>--%>
                    <li><a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>" rel="nofollow"><%= CurrentSource.getSysLangValue("lblContacts")%></a>
                    </li>
                    <li><a href="<%=CurrentSource.getPagePath("19", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("reqTermsOfService")%></a>
                    </li>
                    <li style="display: none;"><a href="<%=CurrentSource.getPagePath("18", "stp", CurrentLang.ID.ToString()) %>">Links</a>
                    </li>
                </ul>
            </div>
            <div class="block col-sm-3">
                <h3><%= CurrentSource.getSysLangValue("lblNetwork")%></h3>
                <ul class="footer-links">
                    <li><a href="<%=CurrentSource.getPagePath("8", "stp", CurrentLang.ID.ToString()) %>"><%= CurrentSource.getSysLangValue("lblYouArePropertyOwner")%></a>
                    </li>
                    <li>
                        <a target="_blank" href="http://www.rentalcastles.com/"><%=CurrentSource.getSysLangValue("menuRentCastle")%></a>
                    </li>
                    <li>
                        <a target="_blank" href="http://www.millennium-beb.com/"><%=CurrentSource.getSysLangValue("menuBeB")%></a>
                    </li>
                   <%-- <li><a target="_blank" href="http://www.yourweddinginrome.com/">Wedding in Rome</a>
                    </li>--%>
                    <li><a target="_blank" href="http://www.rentalinflorence.com/">Rental in Florence</a>
                    </li>
                    <li><a target="_blank" href="http://www.rentalinvenice.com/">Rental in Venice</a>
                    </li>
                    <li><a target="_blank" href="http://www.rentalincortina.com/">Rental in Cortina d'Ampezzo</a>
                    </li>
                    <li><a target="_blank" href="http://www.rentalinkenya.com/">Rental in Kenya</a>
                    </li>
                    <li><a target="_blank" href="http://www.liontravel.it/">Lion Travel</a>
                    </li>
                    <li><a target="_blank" href="http://www.greenmalindi.com/">Green Malindi</a>
                    </li>
                </ul>
            </div>
        </div>
    </div>


    <!-- BEGIN COPYRIGHT -->
    <div id="copyright">
        <div class="container">
            <div class="row">
                <div class="col-sm-10">
                    Rental in Rome S.r.l. - 2003 - 2016 © all rights reserved
                    <br />
                    <em style="font-size: 11px; line-height: 1.5; opacity: 0.7; display: inline-block; width: 70%;">P. IVA: 07824541002 | Tel:  +39 06 3220068 | Fax: +39 06 23328717 | <a href="mailto:info@rentalinrome.com">info@rentalinrome.com</a> | <a href="http://www.magarental.com" target="_blank">magarental</a>  </em>



                </div>

                <div class="col-sm-2" id="shinystatContent">

                    <asp:PlaceHolder ID="PH_shinystat_tmp" runat="server" Visible="false">
                        <script type="text/javascript">
                            var shinystatContent = ''
  			                    //+ '<' + 'script type="text/javascript" language="JavaScript" src="http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1"><' + '/' + 'script' + '>'
                                + '<' + 'noscript>'
                                + '    <a href="http://www.shinystat.it/cgi-bin/shinystatv.cgi?USER=rentalinromejack" target="_top">'
                                + '        <img src="http://www.shinystat.it/cgi-bin/shinystat.cgi?USER=rentalinromejack&NC=1" border="0" /></a>'
                                + '</' + 'noscript' + '>';

                            $(document).ready(function () {
                                $(window).load(function () {
                                    $('#shinystatContent').html(shinystatContent);
                                    //var script = document.createElement('script');
                                    //script.type = 'text/javascript';
                                    //script.src = "http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1";
                                    //$("#shinystatContent").append(script);
                                    var th = document.getElementById("shinystatContent");
                                    var s = document.createElement('script');
                                    s.setAttribute('type', 'text/javascript');
                                    s.setAttribute('src', "http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1");
                                    th.appendChild(s);

                                });
                            });
                        </script>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PH_shinystat" runat="server" Visible="false">
                        <script type="text/javascript" language="JavaScript" src="http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1"></script>
                        <noscript>
                            <a href="http://www.shinystat.it/cgi-bin/shinystatv.cgi?USER=rentalinromejack" target="_top">
                                <img src="http://www.shinystat.it/cgi-bin/shinystat.cgi?USER=rentalinromejack&NC=1" border="0" /></a>
                        </noscript>
                    </asp:PlaceHolder>

                </div>
            </div>
        </div>
    </div>
    <!-- END COPYRIGHT -->

</footer>
<!-- END FOOTER -->
