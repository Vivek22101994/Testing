<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true"
    CodeBehind="pg_streetDett.aspx.cs" Inherits="RentalInRome.pg_streetDett" %>

<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:content>
<asp:content id="Content2" contentplaceholderid="CPH_head_bottom" runat="server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    

<div id="blogMain">

    <div class="blogPost" id="blogPostDettaglio">
        <h3 class="blogPostTit">Via del Corso</h3>
        <div class="social" id="socialPostDett">
            <div class="fb">
                <a name="fb_share" type="button_count" href="http://www.facebook.com/sharer.php">Condividi</a><script
                    src="http://static.ak.fbcdn.net/connect.php/js/FB.Share" type="text/javascript"></script>
            </div>
            <div class="twit">
                <a href="https://twitter.com/share" class="twitter-share-button" data-lang="it">Tweet</a>
                <script>                    !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>
            </div>
            <div class="gplus">
                <g:plusone size="medium" annotation="none"></g:plusone>
                <script type="text/javascript">
                    window.___gcfg = { lang: 'it' };

                    (function () {
                        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                        po.src = 'https://apis.google.com/js/plusone.js';
                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                    })();
                </script>
            </div>
        </div>
        
        <div class="postDett" id="dettPostDett">

            
                <span class="postAuthor" id="authorPostDett">pubblicato da: <strong>Federico</strong></span>
                <span class="postDate" id="datePostDett">il <em>15/05/2012</em></span>
            
        </div>

        <div class="nulla">
        </div>

        <!-- INIZIO GALLERY DA SISTEMARE -->
        <div class="blogPostImg">
            <img src="/images/via-del-corso.jpg" alt="via del corso" />
        </div>

        <div id="galleryBlogNav">

            <a class="prev" href="#"></a>

            <div id="galleryBlogThumbsCont">
                <div class="items" id="items_galleryBlogNav">
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                    <a href="#" class="galleryBlogThumb">
                        <img src="/images/via-del-corso-thumb-img.jpg" alt="" />
                    </a>
                </div>
            </div>

            <a class="next" href="#"></a>

        </div>

        <!-- FINE GALLERY DA SITEMARE -->

        <div class="blogPostTxt" id="txtPostDett">
        Oggi inzia una nuova  rubrica per i nostri  “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città.

Evitaremo riferimenti storici, facilmente rintracciabili un po' ovunque nella rete, per svelare piuttosto qualche piccolo segreto delle vie che andremo a descrivere.

La via più indicata per iniziare questa avventura e’ sicuramente Via del Corso, arteria principale della vita del Centro Storico e punto di raccordo per tante attrazioni della Citta’ Eterna. Ogni romano che si rispetti ha percorso questa via  almeno una volta se non altro dopo aver marinato la scuola, grazie soprattutto all’alta densità di negozi di marche alla moda che attragono tanto i ragazzi. 
Luogo privilegiato per i giornalisti che molto spesso vi realizzano le loro interviste all'"uomo della strada", questa lunghissima via romana si caratterizza anche per l’adiacenza con Montecitorio (il parlamento italiano), con il conseguente pellegrinaggio di tantissimi curiosi
        </div>

        <div id="daVedereDett">

            <h3 class="daVedereDettTit">Da vedere nelle vicinanze</h3>
            <div id="daVedereDettScroll_navigator">
            </div>
            <div id="daVedereDettScroll">
                <div class="items" id="daVedereDettItems">
                    <div class="daVedereDett_item">
                        <a href="#">
                            <span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                            </span>
                            <span class="nomeDaVedereDett">Barcaccia</span>
                            <span class="txtDaVedereDett">This luxury apartment has a stunning view of the Trevi
                                Fountain. It is located in Piazza dei Crociferi. a square located about 20 meters
                                from the well-known fountain in Rome.
                                <br /><br />
                                The apartment is situated on the 4th floor
                                of a historic building without elevator, in which some ruins of the Roman age can
                                be found.
                            </span>
                            <span class="readMoreDaVedere">Leggi tutto...</span>
                        </a>
                    </div>
                    <div class="daVedereDett_item">
                        <a href="#"><span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                        </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">
                            This luxury apartment has a stunning view of the Trevi Fountain. It is located in
                            Piazza dei Crociferi. a square located about 20 meters from the well-known fountain
                            in Rome.
                            <br />
                            <br />
                            The apartment is situated on the 4th floor of a historic building without elevator,
                            in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">
                                Leggi tutto...</span> </a>
                    </div>
                    <div class="daVedereDett_item">
                        <a href="#"><span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                        </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">
                            This luxury apartment has a stunning view of the Trevi Fountain. It is located in
                            Piazza dei Crociferi. a square located about 20 meters from the well-known fountain
                            in Rome.
                            <br />
                            <br />
                            The apartment is situated on the 4th floor of a historic building without elevator,
                            in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">
                                Leggi tutto...</span> </a>
                    </div>
                    <div class="daVedereDett_item">
                        <a href="#"><span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                        </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">
                            This luxury apartment has a stunning view of the Trevi Fountain. It is located in
                            Piazza dei Crociferi. a square located about 20 meters from the well-known fountain
                            in Rome.
                            <br />
                            <br />
                            The apartment is situated on the 4th floor of a historic building without elevator,
                            in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">
                                Leggi tutto...</span> </a>
                    </div>
                    <div class="daVedereDett_item">
                        <a href="#"><span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                        </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">
                            This luxury apartment has a stunning view of the Trevi Fountain. It is located in
                            Piazza dei Crociferi. a square located about 20 meters from the well-known fountain
                            in Rome.
                            <br />
                            <br />
                            The apartment is situated on the 4th floor of a historic building without elevator,
                            in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">
                                Leggi tutto...</span> </a>
                    </div>
                    <div class="daVedereDett_item">
                        <a href="#"><span class="overflowImg">
                            <img src="/images/da-vedere-thumb-1.jpg" alt="" />
                        </span><span class="nomeDaVedereDett">Barcaccia</span> <span class="txtDaVedereDett">
                            This luxury apartment has a stunning view of the Trevi Fountain. It is located in
                            Piazza dei Crociferi. a square located about 20 meters from the well-known fountain
                            in Rome.
                            <br />
                            <br />
                            The apartment is situated on the 4th floor of a historic building without elevator,
                            in which some ruins of the Roman age can be found. </span><span class="readMoreDaVedere">
                                Leggi tutto...</span> </a>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                $(document).ready(function () {
                    $("#daVedereDettScroll").scrollable({ size: 1, clickable: false, circular: true, keyboard: 'static' }).navigator("#daVedereDettScroll_navigator").autoscroll({ circular: true, autoplay: true });
                });
            </script>

        </div>
        
        <div id="commentListCont">
            <h3 class="daVedereDettTit">
                Commenti</h3>
            <div class="commento list">
                <div class="commCont">
                    <div style="float: left; width: 582px;">
                        <span class="userName">Stephen A. Fritch </span><span class="postDate">15/05/2012</span>
                        <div class="nulla">
                        </div>
                        <span class="aptCommName">Relativo alla via:&nbsp;<a href="#">Via del Corso</a></span>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="commentoTxt">
                        We were so very pleased with our Ginnasi rental that we have recommended your company
                        to many of our friends. We will remember the rooftop views and drinking Italian
                        wine on our private patio. The location was perfect and we felt we did not just
                        visit Rome, we lived in Rome. Grazie mille.
                    </div>
                </div>
            </div>
            <div class="commento list">
                <div class="commCont">
                    <div style="float: left; width: 582px;">
                        <span class="userName">Stephen A. Fritch </span><span class="postDate">15/05/2012</span>
                        <div class="nulla">
                        </div>
                        <span class="aptCommName">Relativo alla via:&nbsp;<a href="#">Via del Corso</a></span>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="commentoTxt">
                        We were so very pleased with our Ginnasi rental that we have recommended your company
                        to many of our friends. We will remember the rooftop views and drinking Italian
                        wine on our private patio. The location was perfect and we felt we did not just
                        visit Rome, we lived in Rome. Grazie mille.
                    </div>
                </div>
            </div>
            <div class="commento list">
                <div class="commCont">
                    <div style="float: left; width: 582px;">
                        <span class="userName">Stephen A. Fritch </span>
                        <span class="postDate">15/05/2012</span>
                        <div class="nulla">
                        </div>
                        <span class="aptCommName">Relativo alla via:&nbsp;<a href="#">Via del Corso</a></span>
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="commentoTxt">
                        We were so very pleased with our Ginnasi rental that we have recommended your company
                        to many of our friends. We will remember the rooftop views and drinking Italian
                        wine on our private patio. The location was perfect and we felt we did not just
                        visit Rome, we lived in Rome. Grazie mille.
                    </div>
                </div>
            </div>

            <div class="nulla">
            </div>
            <h3 class="daVedereDettTit">
                Lascia il tuo commento</h3>
            <div class="nulla">
            </div>
            <!-- INIZIO LASCIA IL TUO COMMENTO -->
            <div class="editComment" id="ctl00_CPH_main_pnl_cont">
                <div style="margin: 15px 0px 15px 0;" class="nomeComm">
                    <strong>Nome:</strong>
                    <input type="text" style="width: 210px;" id="ctl00_CPH_main_txt_name_full" name="ctl00$CPH_main$txt_name_full">
                    <span style="width: 300px; float: none; display: none;" class="alertErrorSmall" id="txt_name_full_check">
                        Questo campo è obbligatorio: inserisci un valore.</span>
                </div>
                <div style="margin: 15px 0px 15px 0;" class="nomeComm">
                    <strong>Email:</strong>
                    <input type="text" style="width: 210px;" id="ctl00_CPH_main_txt_email" name="ctl00$CPH_main$txt_email">
                    <span style="width: 300px; float: none; display: none;" class="alertErrorSmall" id="txt_email_check">
                    </span>
                </div>
                <div class="userType">
                    <span>
                        <input type="radio" value="rbt_pers_m" name="ctl00$CPH_main$pers" id="ctl00_CPH_main_rbt_pers_m">
                        <a class="ico_tooltip" ttp="div_man">
                            <img alt="Uomo" src="/images/css/user-m.gif"></a>
                        <div style="display: none;" id="tooltip_div_man">
                            Uomo
                        </div>
                    </span><span>
                        <input type="radio" value="rbt_pers_f" name="ctl00$CPH_main$pers" id="ctl00_CPH_main_rbt_pers_f">
                        <a class="ico_tooltip" ttp="div_woman">
                            <img alt="Donna:" src="/images/css/user-f.gif"></a>
                        <div style="display: none;" id="tooltip_div_woman">
                            Donna
                        </div>
                    </span><span>
                        <input type="radio" value="rbt_pers_co" name="ctl00$CPH_main$pers" id="ctl00_CPH_main_rbt_pers_co">
                        <a class="ico_tooltip" ttp="div_couple">
                            <img alt="Coppie" src="/images/css/user-co.gif"></a>
                        <div style="display: none;" id="tooltip_div_couple">
                            Coppie
                        </div>
                    </span><span>
                        <input type="radio" value="rbt_pers_fam" name="ctl00$CPH_main$pers" id="ctl00_CPH_main_rbt_pers_fam">
                        <a class="ico_tooltip" ttp="div_family">
                            <img alt="Famiglia" src="/images/css/user-fam.gif"></a>
                        <div style="display: none;" id="tooltip_div_family">
                            Famiglia
                        </div>
                    </span><span>
                        <input type="radio" value="rbt_pers_gr" name="ctl00$CPH_main$pers" id="ctl00_CPH_main_rbt_pers_gr">
                        <a class="ico_tooltip" ttp="div_groups">
                            <img alt="Gruppi" src="/images/css/user-gr.gif"></a>
                        <div style="display: none;" id="tooltip_div_groups">
                            Gruppi
                        </div>
                    </span>
                </div>
                
                <div class="nomeComm">
                    <span>Nazione </span>
                    <select id="ctl00_CPH_main_drp_country" name="ctl00$CPH_main$drp_country">
                        <option value="">- - -</option>
                        <option value="Afghanistan">Afghanistan</option>
                        <option value="Aland Islands">Aland Islands</option>
                        <option value="Albania">Albania</option>
                        <option value="Algeria">Algeria</option>
                        <option value="American Samoa">American Samoa</option>
                        <option value="Andorra">Andorra</option>
                        <option value="Angola">Angola</option>
                        <option value="Anguilla">Anguilla</option>
                        <option value="Antarctica">Antarctica</option>
                        <option value="Antigua And Barbuda">Antigua And Barbuda</option>
                        <option value="Argentina">Argentina</option>
                        <option value="Armenia">Armenia</option>
                        <option value="Aruba">Aruba</option>
                        <option value="Australia">Australia</option>
                        <option value="Austria">Austria</option>
                        <option value="Azerbaijan">Azerbaijan</option>
                        <option value="Bahamas">Bahamas</option>
                        <option value="Bahrain">Bahrain</option>
                        <option value="Bangladesh">Bangladesh</option>
                        <option value="Barbados">Barbados</option>
                        <option value="Belarus">Belarus</option>
                        <option value="Belgium">Belgium</option>
                        <option value="Belize">Belize</option>
                        <option value="Benin">Benin</option>
                        <option value="Bermuda">Bermuda</option>
                        <option value="Bhutan">Bhutan</option>
                        <option value="Bolivia">Bolivia</option>
                        <option value="Bosnia And Herzegovina">Bosnia And Herzegovina</option>
                        <option value="Botswana">Botswana</option>
                        <option value="Bouvet Island">Bouvet Island</option>
                        <option value="Brazil">Brazil</option>
                        <option value="British Indian Ocean Territory">British Indian Ocean Territory</option>
                        <option value="Brunei Darussalam">Brunei Darussalam</option>
                        <option value="Bulgaria">Bulgaria</option>
                        <option value="Burkina Faso">Burkina Faso</option>
                        <option value="Burundi">Burundi</option>
                        <option value="Cambodia">Cambodia</option>
                        <option value="Cameroon">Cameroon</option>
                        <option value="Canada">Canada</option>
                        <option value="Cape Verde">Cape Verde</option>
                        <option value="Cayman Islands">Cayman Islands</option>
                        <option value="Central African Republic">Central African Republic</option>
                        <option value="Chad">Chad</option>
                        <option value="Chile">Chile</option>
                        <option value="China">China</option>
                        <option value="Christmas Island">Christmas Island</option>
                        <option value="Cocos (Keeling) Islands">Cocos (Keeling) Islands</option>
                        <option value="Colombia">Colombia</option>
                        <option value="Comoros">Comoros</option>
                        <option value="Congo">Congo</option>
                        <option value="Congo, The Democratic Republic Of The">Congo, The Democratic Republic
                            Of The</option>
                        <option value="Cook Islands">Cook Islands</option>
                        <option value="Costa Rica">Costa Rica</option>
                        <option value="Cote D'Ivoire">Cote D'Ivoire</option>
                        <option value="Croatia">Croatia</option>
                        <option value="Cuba">Cuba</option>
                        <option value="Cyprus">Cyprus</option>
                        <option value="Czech Republic">Czech Republic</option>
                        <option value="Denmark">Denmark</option>
                        <option value="Djibouti">Djibouti</option>
                        <option value="Dominica">Dominica</option>
                        <option value="Dominican Republic">Dominican Republic</option>
                        <option value="Ecuador">Ecuador</option>
                        <option value="Egypt">Egypt</option>
                        <option value="El Salvador">El Salvador</option>
                        <option value="Equatorial Guinea">Equatorial Guinea</option>
                        <option value="Eritrea">Eritrea</option>
                        <option value="Estonia">Estonia</option>
                        <option value="Ethiopia">Ethiopia</option>
                        <option value="Falkland Islands (Malvinas)">Falkland Islands (Malvinas)</option>
                        <option value="Faroe Islands">Faroe Islands</option>
                        <option value="Fiji">Fiji</option>
                        <option value="Finland">Finland</option>
                        <option value="France">France</option>
                        <option value="French Guiana">French Guiana</option>
                        <option value="French Polynesia">French Polynesia</option>
                        <option value="French Southern Territories">French Southern Territories</option>
                        <option value="Gabon">Gabon</option>
                        <option value="Gambia">Gambia</option>
                        <option value="Georgia">Georgia</option>
                        <option value="Germany">Germany</option>
                        <option value="Ghana">Ghana</option>
                        <option value="Gibraltar">Gibraltar</option>
                        <option value="Greece">Greece</option>
                        <option value="Greenland">Greenland</option>
                        <option value="Grenada">Grenada</option>
                        <option value="Guadeloupe">Guadeloupe</option>
                        <option value="Guam">Guam</option>
                        <option value="Guatemala">Guatemala</option>
                        <option value="Guernsey">Guernsey</option>
                        <option value="Guinea">Guinea</option>
                        <option value="Guinea-Bissau">Guinea-Bissau</option>
                        <option value="Guyana">Guyana</option>
                        <option value="Haiti">Haiti</option>
                        <option value="Heard Island And Mcdonald Islands">Heard Island And Mcdonald Islands</option>
                        <option value="Holy See (Vatican City State)">Holy See (Vatican City State)</option>
                        <option value="Honduras">Honduras</option>
                        <option value="Hong Kong">Hong Kong</option>
                        <option value="Hungary">Hungary</option>
                        <option value="Iceland">Iceland</option>
                        <option value="India">India</option>
                        <option value="Indonesia">Indonesia</option>
                        <option value="Iran, Islamic Republic Of">Iran, Islamic Republic Of</option>
                        <option value="Iraq">Iraq</option>
                        <option value="Ireland">Ireland</option>
                        <option value="Isle Of Man">Isle Of Man</option>
                        <option value="Israel">Israel</option>
                        <option value="Italy">Italy</option>
                        <option value="Jamaica">Jamaica</option>
                        <option value="Japan">Japan</option>
                        <option value="Jersey">Jersey</option>
                        <option value="Jordan">Jordan</option>
                        <option value="Kazakhstan">Kazakhstan</option>
                        <option value="Kenya">Kenya</option>
                        <option value="Kiribati">Kiribati</option>
                        <option value="Korea, Democratic People'S Republic Of">Korea, Democratic People'S Republic
                            Of</option>
                        <option value="Korea, Republic Of">Korea, Republic Of</option>
                        <option value="Kuwait">Kuwait</option>
                        <option value="Kyrgyzstan">Kyrgyzstan</option>
                        <option value="Lao People'S Democratic Republic">Lao People'S Democratic Republic</option>
                        <option value="Latvia">Latvia</option>
                        <option value="Lebanon">Lebanon</option>
                        <option value="Lesotho">Lesotho</option>
                        <option value="Liberia">Liberia</option>
                        <option value="Libyan Arab Jamahiriya">Libyan Arab Jamahiriya</option>
                        <option value="Liechtenstein">Liechtenstein</option>
                        <option value="Lithuania">Lithuania</option>
                        <option value="Luxembourg">Luxembourg</option>
                        <option value="Macao">Macao</option>
                        <option value="Macedonia, The Former Yugoslav Republic Of">Macedonia, The Former Yugoslav
                            Republic Of</option>
                        <option value="Madagascar">Madagascar</option>
                        <option value="Malawi">Malawi</option>
                        <option value="Malaysia">Malaysia</option>
                        <option value="Maldives">Maldives</option>
                        <option value="Mali">Mali</option>
                        <option value="Malta">Malta</option>
                        <option value="Marshall Islands">Marshall Islands</option>
                        <option value="Martinique">Martinique</option>
                        <option value="Mauritania">Mauritania</option>
                        <option value="Mauritius">Mauritius</option>
                        <option value="Mayotte">Mayotte</option>
                        <option value="Mexico">Mexico</option>
                        <option value="Micronesia, Federated States Of">Micronesia, Federated States Of</option>
                        <option value="Moldova, Republic Of">Moldova, Republic Of</option>
                        <option value="Monaco">Monaco</option>
                        <option value="Mongolia">Mongolia</option>
                        <option value="Montserrat">Montserrat</option>
                        <option value="Morocco">Morocco</option>
                        <option value="Mozambique">Mozambique</option>
                        <option value="Myanmar">Myanmar</option>
                        <option value="Namibia">Namibia</option>
                        <option value="Nauru">Nauru</option>
                        <option value="Nepal">Nepal</option>
                        <option value="Netherlands">Netherlands</option>
                        <option value="Netherlands Antilles">Netherlands Antilles</option>
                        <option value="New Caledonia">New Caledonia</option>
                        <option value="New Zealand">New Zealand</option>
                        <option value="Nicaragua">Nicaragua</option>
                        <option value="Niger">Niger</option>
                        <option value="Nigeria">Nigeria</option>
                        <option value="Niue">Niue</option>
                        <option value="Norfolk Island">Norfolk Island</option>
                        <option value="Northern Mariana Islands">Northern Mariana Islands</option>
                        <option value="Norway">Norway</option>
                        <option value="Oman">Oman</option>
                        <option value="Pakistan">Pakistan</option>
                        <option value="Palau">Palau</option>
                        <option value="Palestinian Territory, Occupied">Palestinian Territory, Occupied</option>
                        <option value="Panama">Panama</option>
                        <option value="Papua New Guinea">Papua New Guinea</option>
                        <option value="Paraguay">Paraguay</option>
                        <option value="Peru">Peru</option>
                        <option value="Philippines">Philippines</option>
                        <option value="Pitcairn">Pitcairn</option>
                        <option value="Poland">Poland</option>
                        <option value="Portugal">Portugal</option>
                        <option value="Puerto Rico">Puerto Rico</option>
                        <option value="Qatar">Qatar</option>
                        <option value="Reunion">Reunion</option>
                        <option value="Romania">Romania</option>
                        <option value="Russian Federation">Russian Federation</option>
                        <option value="Rwanda">Rwanda</option>
                        <option value="Saint Helena">Saint Helena</option>
                        <option value="Saint Kitts And Nevis">Saint Kitts And Nevis</option>
                        <option value="Saint Lucia">Saint Lucia</option>
                        <option value="Saint Pierre And Miquelon">Saint Pierre And Miquelon</option>
                        <option value="Saint Vincent And The Grenadines">Saint Vincent And The Grenadines</option>
                        <option value="Samoa">Samoa</option>
                        <option value="San Marino">San Marino</option>
                        <option value="Sao Tome And Principe">Sao Tome And Principe</option>
                        <option value="Saudi Arabia">Saudi Arabia</option>
                        <option value="Senegal">Senegal</option>
                        <option value="Serbia And Montenegro">Serbia And Montenegro</option>
                        <option value="Seychelles">Seychelles</option>
                        <option value="Sierra Leone">Sierra Leone</option>
                        <option value="Singapore">Singapore</option>
                        <option value="Slovakia">Slovakia</option>
                        <option value="Slovenia">Slovenia</option>
                        <option value="Solomon Islands">Solomon Islands</option>
                        <option value="Somalia">Somalia</option>
                        <option value="South Africa">South Africa</option>
                        <option value="South Georgia And The South Sandwich Islands">South Georgia And The South
                            Sandwich Islands</option>
                        <option value="Spain">Spain</option>
                        <option value="Sri Lanka">Sri Lanka</option>
                        <option value="Sudan">Sudan</option>
                        <option value="Suriname">Suriname</option>
                        <option value="Svalbard And Jan Mayen">Svalbard And Jan Mayen</option>
                        <option value="Swaziland">Swaziland</option>
                        <option value="Sweden">Sweden</option>
                        <option value="Switzerland">Switzerland</option>
                        <option value="Syrian Arab Republic">Syrian Arab Republic</option>
                        <option value="Taiwan, Province Of China">Taiwan, Province Of China</option>
                        <option value="Tajikistan">Tajikistan</option>
                        <option value="Tanzania, United Republic Of">Tanzania, United Republic Of</option>
                        <option value="Thailand">Thailand</option>
                        <option value="Timor-Leste">Timor-Leste</option>
                        <option value="Togo">Togo</option>
                        <option value="Tokelau">Tokelau</option>
                        <option value="Tonga">Tonga</option>
                        <option value="Trinidad And Tobago">Trinidad And Tobago</option>
                        <option value="Tunisia">Tunisia</option>
                        <option value="Turkey">Turkey</option>
                        <option value="Turkmenistan">Turkmenistan</option>
                        <option value="Turks And Caicos Islands">Turks And Caicos Islands</option>
                        <option value="Tuvalu">Tuvalu</option>
                        <option value="Uganda">Uganda</option>
                        <option value="Ukraine">Ukraine</option>
                        <option value="United Arab Emirates">United Arab Emirates</option>
                        <option value="United Kingdom">United Kingdom</option>
                        <option value="United States">United States</option>
                        <option value="United States Minor Outlying Islands">United States Minor Outlying Islands</option>
                        <option value="Uruguay">Uruguay</option>
                        <option value="Uzbekistan">Uzbekistan</option>
                        <option value="Vanuatu">Vanuatu</option>
                        <option value="Venezuela">Venezuela</option>
                        <option value="Viet Nam">Viet Nam</option>
                        <option value="Virgin Islands, British">Virgin Islands, British</option>
                        <option value="Virgin Islands, U.S.">Virgin Islands, U.S.</option>
                        <option value="Wallis And Futuna">Wallis And Futuna</option>
                        <option value="Western Sahara">Western Sahara</option>
                        <option value="Yemen">Yemen</option>
                        <option value="Zambia">Zambia</option>
                        <option value="Zimbabwe">Zimbabwe</option>
                    </select>
                    <span style="width: 300px; float: none; display: none;" class="alertErrorSmall" id="drp_country_check">
                        Seleziona un paese.</span>
                </div>
                <div class="nulla">
                </div>
                <textarea id="ctl00_CPH_main_txt_body" cols="20" rows="2" name="ctl00$CPH_main$txt_body"></textarea>
                <span style="width: 300px; float: none; display: none;" class="alertErrorSmall" id="txt_body_check">
                    Links are not allowed here.</span>
                <div class="nulla">
                </div>
                <div id="Captcha">
                    <div class="RadCaptcha RadCaptcha_Default" id="ctl00_CPH_main_RadCaptcha1">
                        <!-- 2011.3.1305.40 -->
                        <span style="color: Red; visibility: hidden;" id="ctl00_CPH_main_RadCaptcha1_ctl00">
                        </span>
                        <div id="ctl00_CPH_main_RadCaptcha1_SpamProtectorPanel">
                            <div id="ctl00_CPH_main_RadCaptcha1_ctl01">
                                <img style="height: 50px; width: 180px; border-width: 0px; display: block;" src="Telerik.Web.UI.WebResource.axd?type=rca&amp;guid=d1dfd20a-c218-40a3-a2f7-5729247c1374"
                                    alt="" class="imageClass" id="ctl00_CPH_main_RadCaptcha1_CaptchaImageUP"><a style="display: block;"
                                        href="javascript:__doPostBack('ctl00$CPH_main$RadCaptcha1$CaptchaLinkButton','')"
                                        title="Generate New Image" class="rcRefreshImage" id="ctl00_CPH_main_RadCaptcha1_CaptchaLinkButton">Generate
                                        New Image</a>
                            </div>
                        </div>
                        <input type="hidden" name="ctl00_CPH_main_RadCaptcha1_ClientState" id="ctl00_CPH_main_RadCaptcha1_ClientState"
                            autocomplete="off">
                    </div>
                    <div class="RadCaptchaText">
                        <input type="text" id="ctl00_CPH_main_txtCaptcha" maxlength="5" name="ctl00$CPH_main$txtCaptcha">
                        <span>Insert the text you see in the image.</span>
                    </div>
                </div>
                <div class="nulla">
                </div>
                <a style="float: right; margin-right: 0;" href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;ctl00$CPH_main$lnk_save&quot;, &quot;&quot;, true, &quot;gbook&quot;, &quot;&quot;, false, true))"
                    class="btn" id="ctl00_CPH_main_lnk_save" onclick="return RNT_validateRequestForm();">
                    <span>Invia il tuo feedback </span></a>
                <div class="nulla">
                </div>
            </div>
            <!-- FINE LASCIA IL TUO COMMENTO -->

        </div>
        <div class="nulla">
        </div>
    </div>

</div>

<div id="blogColDx">
    <div class="boxblogColDx" id="aptsVicini">
        
        
        <h4 class="titblogColDx">
           Appartamenti vicini
        </h4>

        <div id="aptViciniScroll">
        <a class="prev" href="#"></a>
        <a class="next" href="#"></a>
        <div id="scrollDaVedere">
                <div class="scrollDaVedere_item">
                    <a href="#"><span class="imgDaVedereCont">
                        <img src="/images/altare-patria.jpg" alt="altare della patria" />
                    </span>
                    <span style="float:left; display:block;">
                        <span class="nomeDaVedere">Appartamento</span>
                        <span class="doveDaVedere"> vicino a <strong>Piazza Venezia</strong></span> 
                    </span>
                    <span class="przAptBlog">
                        <span>da<br />a notte</span>
                        <strong>150€</strong>
                    </span>
                    <span class="nulla" style="display:block;">
                    </span> 
                    <span class="btn-prenota-rental-blog">
                    Prenota ora su
                    </span> 
                    </a>
                </div>
            </div>
        </div>
    </div>

    <div class="boxblogColDx">
        <h4 class="titblogColDx">Zone di Roma</h4>
        <ul class="menublogColDx">
            <li><a href="#">Fontana di trevi</a></li>
            <li><a href="#">Piazza Navona</a></li>
            <li><a href="#">Piazza di Spagna</a></li>
            <li><a href="#">San Pietro</a></li>
            <li><a href="#">Colosseo</a></li>
            <li><a href="#">Trastevere</a></li>
            <li><a href="#">Parioli</a></li>
            <li><a href="#">Termini</a></li>
            <li><a href="#">Campo de fiori </a></li>    
            <li><a href="#">Altre zone</a></li>
        </ul>
    </div>
</div>

</asp:content>
