<%@ Page Title="" Language="C#" MasterPageFile="~/common/Main.Master" AutoEventWireup="true" CodeBehind="stp_aboutusNew.aspx.cs" Inherits="RentalInRome.stp_aboutusNew" %>
<%@ Register Src="~/ucMain/ucBreadcrumbs.ascx" TagName="breadCrumbs" TagPrefix="uc1" %>
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
    <asp:Literal runat="server" ID="ltr_summary" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <!-- BEGIN CONTENT WRAPPER -->
    <div class="content">
        <div class="container bookingFormContainer stpContainer aboutUsContainer">
            <div class="row">
                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar col-sm-4 bookingFormDetCont">

                    <div class="gray recommendedByBox">

                        <h3 class="section-title"><%= CurrentSource.getSysLangValue("lblOurWebsiteIsRecommendedBy")%> </h3>

                        <div class="nulla">
                        </div>

                        <table cellspacing="0" cellpadding="0" class="recommendedLogos">
                            <tbody>
                                <tr>
                                    <td valign="middle" align="center" style="padding: 10px 0;">
                                        <img alt="businessweek" src="/images/logos/businessweek.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="the independent" src="/images/logos/independent_new2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center" style="padding: 20px 0;">
                                        <img alt="the philadelphia enquirer" src="/images/logos/thephiladelphia.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="the Times" src="/images/logos/thetimes2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="la presse" src="/images/logos/lapresse2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="new york post" src="/images/logos/nypost2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="the daily herald" src="/images/logos/dailyherald2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center" style="padding: 10px 0;">
                                        <img alt="il sole 24 ore" src="/images/logos/sole24ore_new2.jpg">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="center">
                                        <img alt="Condé nast traveler" src="/images/logos/conde_new2.jpg">
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <div class="nulla">
                        </div>

                    </div>

                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <uc1:breadCrumbs ID="breadCrumbs" runat="server" />
                    <h1 class="section-title"><%=ltr_title.Text %></h1>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">

                        <%=ltr_description.Text %>

                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6" style="text-align: center">
                            <a target="chat1220380" style="margin: 20px; display: block;" href="http://server.iad.liveperson.net/hc/1220380/?cmd=file&amp;file=visitorWantsToChat&amp;site=1220380&amp;byhref=1&amp;imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a" onclick="lpButtonCTTUrl = 'http://server.iad.liveperson.net/hc/1220380/?cmd=file&amp;file=visitorWantsToChat&amp;site=1220380&amp;imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a&amp;referrer='+escape(document.location); lpButtonCTTUrl = (typeof(lpAppendVisitorCookies) != 'undefined' ? lpAppendVisitorCookies(lpButtonCTTUrl) : lpButtonCTTUrl); lpButtonCTTUrl = ((typeof(lpMTag)!='undefined' &amp;&amp; typeof(lpMTag.addFirstPartyCookies)!='undefined')?lpMTag.addFirstPartyCookies(lpButtonCTTUrl):lpButtonCTTUrl);window.open(lpButtonCTTUrl,'chat1220380','width=472,height=320,resizable=yes');return false;" id="_lpChatBtn">
                                <img border="0" alt="" src="http://server.iad.liveperson.net/hc/1220380/?cmd=repstate&amp;site=1220380&amp;channel=web&amp;&amp;ver=1&amp;imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a"></a>

                            <div class="nulla">
                            </div>
                            <p class="center" style="margin-top: 25px;">
                                <a class="btn btn-fullcolor" href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>">
                                    <i class="fa fa-file"></i>Rental in Rome in <strong><%= CurrentSource.getSysLangValue("menu_Press")%></strong>
                                </a>
                            </p>
                        </div>

                        <div class="nulla">
                        </div>
                    </div>

                    <div class="nulla">
                    </div>
                </div>
                <!-- End BEGIN MAIN CONTENT -->
            </div>

            <div class="nulla">
            </div>

            <hr />

            <div class="nulla">
            </div>

            <div class="row">

                <!-- BEGIN BOOKING FORM -->
                <div class="sidebar col-sm-4 bookingFormDetCont travelShowCol">

                    <div class="gray recommendedByBox">

                        <h3 class="section-title"><%= CurrentSource.getSysLangValue("lblOurExpositions")%></h3>

                        <div class="nulla">
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/1a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/1b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Bit 2007 (International Tourism Exchange)<br />
                                    <small>Fieramilano from February 22 to 25, 2007</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/2a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/2b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Globe 2007 (Travel Exhibition in Rome) 
                                    <br />
                                    <small>Fiera di Roma from March 22 to 24, 2007</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/3a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/3b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>TTI 2007 (Travel Trade Italia)  
                                    <br />
                                    <small>Fiera di Rimini from October 12 to 14, 2007</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/4a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/4b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Globe 2008 (Travel Exhibition in Rome)
                                    <br />
                                    <small>Fiera di Roma from March 13 to 15, 2008</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/5a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/5b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>TTI 2008 (Travel Trade Italia) 
                                    <br />
                                    <small>Fiera di Rimini from October 24 to 25, 2008</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/6a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/6b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>New York Times Travel Show 
                                    <br />
                                    <small>New York from February 6 to 8, 2009 </small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/7a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/7b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Globe 2009 (Travel Exhibition in Rome)  
                                    <br />
                                    <small>Fiera di Roma from March 26 to 28, 2009</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/8a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/8b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Italië het evenement 
                                    <br />
                                    <small>Utrecht from May 15 to 17, 2009</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/9a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/9b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>Bit 2011 (International Tourism Exchange) 
                                    <br />
                                    <small>Fieramilano from February 17 to 20, 2011</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/rental-in-rome-mitt-2012-1.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/rental-in-rome-mitt-2012-2.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>MITT 2012  
                                    <br />
                                    <small>Moscow, from March 21 to 24 2012</small>
                                </h4>
                            </div>
                        </div>

                        <div class="travelShowItem">
                            <div class="owl-carousel owl-theme listItemGallery">
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/10a.jpg" alt="" />
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="imgTravel">
                                        <img src="/images/expo/10b.jpg" alt="" />
                                    </div>
                                </div>
                            </div>

                            <div class="info">
                                <h4>TTI 2014 (Travel Trade Italia) 
                                    <br />
                                    <small>Rimini, from October 8 to 11 2014</small>
                                </h4>
                            </div>
                        </div>

                        <div class="nulla">
                        </div>

                    </div>
                    <div class="nulla">
                    </div>
                </div>
                <!-- END BOOKING FORM -->

                <!-- BEGIN MAIN CONTENT -->
                <div class="main col-sm-8">
                    <%=ltr_summary.Text %>

                    <%--<h2 class="section-title">Why choose Us</h2>

                    <div class="nulla">
                    </div>

                    <div class="col-md-12 mainStp">


                        <h3>EXPERIENCE </h3>
                        <br />
                        <p>
                            We were one of the first agencies to start up this business and we have been doing this for 10 years, very few competitor agencies have our experience in the Rome apartment rental field. 
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>PRICES </h3>
                        <br />
                        <p>
                            The rates of our apartment rentals in Rome are the most convenient you can find. One of the main targets of our marketing area during the selection and affiliation of apartment owners is to be able to offer the best solution for the best rate, for our guests only.
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>PASSION </h3>
                        <br />
                        <p>
                            Our customers’ satisfaction is of utmost importance to us, this is why we pay close attention to our guests’ needs.
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>UNDERSTANDING </h3>
                        <br />
                        <p>
                            We listen to our guests to understand and take good care of them. Your suggestions help us do a better job.
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>CARE </h3>
                        <br />
                        <p>
                            We take care of all our apartments and check them thoroughly in order to make your stay in Rome unforgettable.
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>COMMUNICATION </h3>
                        <br />
                        <p>
                            We are not just an online booking website: our aim is to communicate with our guests and help them have their perfect vacation, step by step. 
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>AWARENESS </h3>
                        <br />
                        <p>
                            We are aware of the fact that our guests’ loyalty is really important. A happy guest is our main target. 
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>EXPERTISE </h3>
                        <br />
                        <p>
                            Our multi-lingual and professional staff has full knowledge of our apartments and the surrounding area, and will give you all necessary information to make the most of your stay in Rome. 
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>SAFETY </h3>
                        <br />
                        <p>
                            All our apartments have the Rome City Council authorization to work as holiday homes.  
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>GUARANTEE </h3>
                        <br />
                        <p>
                            We guarantee the quality of our apartments and we are ready to change the apartment you are renting in case it doesn’t correspond to the quality criteria mentioned in the website, at no extra cost.  
                        </p>

                        <hr style="margin: 25px 0;" />

                        <h3>INNOVATION </h3>
                        <br />
                        <p>
                            Our interest and curiosity for the novelty keep us up-to-date on the latest solutions, technology is one of our passions. We have been awarded as best mobile application in 2013 in the tourism field in Italy. 
                            <br />
                            <br />
                            We are happy to meet you in our office located in Via Marianna Dionigi 57 ( Cavour square ) to give you a warm welcome in Rome with a good italian espresso coffee.   
                        </p>

                        <div class="nulla">
                        </div>

                        <hr />

                        <div class="nulla">
                        </div>
                    </div>--%>

                    <h2 class="section-title"><%= CurrentSource.getSysLangValue("lblOurWebsiteIsRecommendedBy")%></h2>

                    <div class="nulla">
                    </div>

                    <div id="agents" class="col-md-12 mainStp">
                        <div class="center imgStp" style="margin-bottom: 40px;">
                            <img src="/images/staff/gruppo.jpg" />
                        </div>

                        <ul class="agency-detail-agents clearfix">
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/jacopo-calabro.jpg" alt="" />
                                <div class="info">
                                    <h3>Jacopo Calabrò (Jack)</h3>
                                    <span class="location">Chairman</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:jacopo.calabro@rentalinrome.com">jacopo.calabro@rentalinrome.com</a>
                                </div>
                            </li>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="/images/staff/claudio-triconnet.jpg" alt="" />
                                <div class="info">
                                    <h3>Claudio Triconnet</h3>
                                    <span class="location">Sales Account</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-fra.gif" />
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:claude.triconnet@rentalinrome.com">claude.triconnet@rentalinrome.com</a>
                                </div>
                            </li>--%>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/barbara-giovannelli.jpg" alt="" />
                                <div class="info">
                                    <h3>Barbara Giovannelli</h3>
                                    <span class="location">Sales Account </span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                        <img alt="" src="/images/css/flag-spa.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:barbara.giovannelli@rentalinrome.com">barbara.giovannelli@rentalinrome.com</a>
                                </div>
                            </li>--%>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="/images/staff/francesca-sangiorgio.jpg" alt="" />
                                <div class="info">
                                    <h3>Francesca Sangiorgio</h3>
                                    <span class="location">Administration & Maintenance Coordinator</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:francesca.sangiorgio@rentalinrome.com">francesca.sangiorgio@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/federicomariani.jpg" alt="" />
                                <div class="info">
                                    <h3>Federico Mariani</h3>
                                    <span class="location">Photo / Video Operator</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:federico.mariani@rentalinrome.com">federico.mariani@rentalinrome.com</a>
                                </div>
                            </li>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/ivan-migoni.jpg" alt="" />
                                <div class="info">
                                    <h3>Ivan Migoni</h3>
                                    <span class="location">Webmaster</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:ivan.migoni@rentalinrome.com">ivan.migoni@rentalinrome.com</a>
                                </div>
                            </li>--%>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/isaac-van-aggelen.jpg" alt="" />
                                <div class="info">
                                    <h3>Isaac van Aggelen</h3>
                                    <span class="location">Sales Account</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-dut.gif" />
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                        <img alt="" src="/images/css/flag-ger.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:isaac.vanaggelen@rentalinrome.com">isaac.vanaggelen@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/francesca-tarantola.jpg" alt="" />
                                <div class="info">
                                    <h3>Francesca Tarantola</h3>
                                    <span class="location">Account Manager</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-ger.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:francesca.tarantola@rentalinrome.com">francesca.tarantola@rentalinrome.com</a>
                                </div>
                            </li>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/davide-pitzalis.jpg" alt="" />
                                <div class="info">
                                    <h3>Davide Pitzalis</h3>
                                    <span class="location">Customer Services Manager</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:davide.pitzalis@rentalinrome.com">davide.pitzalis@rentalinrome.com</a>
                                </div>
                            </li>--%>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/matteo-calabro.jpg" alt="" />
                                <div class="info">
                                    <h3>Matteo Calabrò</h3>
                                    <span class="location">Account Manager</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:matteo.calabro@rentalinrome.com">matteo.calabro@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/paolo-calabro.jpg" alt="" />
                                <div class="info">
                                    <h3>Paolo Calabrò</h3>
                                    <span class="location">Marketing Manager</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                        <img alt="" src="/images/css/flag-fra.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:paolo.calabro@rentalinrome.com">paolo.calabro@rentalinrome.com</a>
                                </div>
                            </li>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/alessandro-d-arcadia.jpg" alt="" />
                                <div class="info">
                                    <h3>Alessandro D'Arcadia</h3>
                                    <span class="location">Welcoming Staff Manager</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-bra.gif" />
                                        <img alt="" src="/images/css/flag-usa.gif" />
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:alessandro.darcadia@rentalinrome.com">alessandro.darcadia@rentalinrome.com</a>
                                </div>
                            </li>--%>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/alessandro-pancaldi.jpg" alt="" />
                                <div class="info">
                                    <h3>Alessandro Pancaldi</h3>
                                    <span class="location">Apartment Representative</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-usa.gif" />
                                        <img alt="" src="/images/css/flag-eng.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:alessandro.pancaldi@rentalinrome.com">alessandro.pancaldi@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/fabio-cortese.jpg" alt="" />
                                <div class="info">
                                    <h3>Fabio Cortese</h3>
                                    <span class="location">Organization & Development</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-usa.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:fabio.cortese@rentalinrome.com">fabio.cortese@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/victoria-shapovalova.jpg" alt="" />
                                <div class="info">
                                    <h3>Victoria Shapovalova</h3>
                                    <span class="location">Sales Account</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-rus.gif" />
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:victoria.shapovalova@rentalinrome.com">victoria.shapovalova@rentalinrome.com</a>
                                </div>
                            </li>
                            <li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/giulia-calabro.jpg" alt="" />
                                <div class="info">
                                    <h3>Giulia Calabrò</h3>
                                    <span class="location">Sales Account</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                        <img alt="" src="/images/css/flag-usa.gif" />
                                        <img alt="" src="/images/css/flag-spa.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:giulia.calabro@rentalinrome.com">giulia.calabro@rentalinrome.com</a>
                                </div>
                            </li>
                            <%--<li class="col-lg-6" data-animation-direction="from-left" data-animation-delay="250">
                                <img src="images/staff/antonio-dia.jpg" alt="" />
                                <div class="info">
                                    <h3>Antonio Dia</h3>
                                    <span class="location">Maintenance</span>
                                    <p>
                                        <img alt="" src="/images/css/flag-ita.gif" />
                                    </p>
                                    <div class="nulla">
                                    </div>
                                    <a href="mailto:manutentori@rentalinrome.com">manutentori@rentalinrome.com</a>
                                </div>
                            </li>--%>
                        </ul>

                    </div>

                    <div class="nulla">
                    </div>
                </div>
                <!-- End BEGIN MAIN CONTENT -->

                <div class="nulla">
                </div>

            </div>

            <div class="nulla">
            </div>
        </div>
    </div>
    <!-- End BEGIN CONTENT WRAPPER -->
    <script>


        $(document).ready(function () {

            $(".listItemGallery").owlCarousel({ navigation: true, slideSpeed: 300, paginationSpeed: 400, singleItem: true });

        });


    </script>

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
