<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="    .aspx.cs" Inherits="RentalInRome.stp_streetList" %>

<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">

    <script src="/jquery/plugin/jquerytools/scrollable.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal> 
	<asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    

<div id="blogMain">

    <div class="blogPost">
        <h3 class="blogPostTit"><a href="#">Via del Corso</a></h3>
        
        <div class="postDett">
            <div style="float:left;">
                <span class="postAuthor">pubblicato da: <strong>Federico</strong></span>
                <span class="postDate">il <em>15/05/2012</em></span>
            </div> 
            <span class="commentsNum">10</span>
        </div>

        <div class="nulla">
        </div>
        <div class="blogPostImg">
            <img src="/images/via-del-corso.jpg" alt="via del corso" />
        </div>
        <div class="blogPostTxt">
        Oggi inzia una nuova  rubrica per i nostri  “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città.

Evitaremo riferimenti storici, facilmente rintracciabili un po' ovunque nella rete, per svelare piuttosto qualche piccolo segreto delle vie che andremo a descrivere.

La via più indicata per iniziare questa avventura e’ sicuramente Via del Corso, arteria principale della vita del Centro Storico e punto di raccordo per tante attrazioni della Citta’ Eterna. Ogni romano che si rispetti ha percorso questa via  almeno una volta se non altro dopo aver marinato la scuola, grazie soprattutto all’alta densità di negozi di marche alla moda che attragono tanto i ragazzi. 
Luogo privilegiato per i giornalisti che molto spesso vi realizzano le loro interviste all'"uomo della strada", questa lunghissima via romana si caratterizza anche per l’adiacenza con Montecitorio (il parlamento italiano), con il conseguente pellegrinaggio di tantissimi curiosi
        </div>
        <div class="social">

            <div class="fb">
                <a name="fb_share" type="button_count" href="http://www.facebook.com/sharer.php">
                Condividi</a><script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share"
                type="text/javascript"></script>
            </div>

            <div class="twit">
                <a href="https://twitter.com/share" class="twitter-share-button" data-lang="it">Tweet</a>
                <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>
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
        <a class="btn" href="#">
            <span>
                Leggi tutto
            </span>
        </a>
        <div class="nulla">
        </div>
    </div>

     <div class="blogPost">
        <h3 class="blogPostTit"><a href="#">Via del Corso</a></h3>
        
        <div class="postDett">
            <div style="float:left;">
                <span class="postAuthor">pubblicato da: <strong>Federico</strong></span>
                <span class="postDate">il <em>15/05/2012</em></span>
            </div> 
            <span class="commentsNum">10</span>
        </div>

        <div class="nulla">
        </div>
        <div class="blogPostImg">
            <img src="/images/via-del-corso.jpg" alt="via del corso" />
        </div>
        <div class="blogPostTxt">
        Oggi inzia una nuova  rubrica per i nostri  “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città.

Evitaremo riferimenti storici, facilmente rintracciabili un po' ovunque nella rete, per svelare piuttosto qualche piccolo segreto delle vie che andremo a descrivere.

La via più indicata per iniziare questa avventura e’ sicuramente Via del Corso, arteria principale della vita del Centro Storico e punto di raccordo per tante attrazioni della Citta’ Eterna. Ogni romano che si rispetti ha percorso questa via  almeno una volta se non altro dopo aver marinato la scuola, grazie soprattutto all’alta densità di negozi di marche alla moda che attragono tanto i ragazzi. 
Luogo privilegiato per i giornalisti che molto spesso vi realizzano le loro interviste all'"uomo della strada", questa lunghissima via romana si caratterizza anche per l’adiacenza con Montecitorio (il parlamento italiano), con il conseguente pellegrinaggio di tantissimi curiosi
        </div>
        <div class="social">
            <div class="fb">
                <a name="fb_share" type="button_count" href="http://www.facebook.com/sharer.php">Condividi</a>
                <script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share" type="text/javascript"></script>
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
        <a class="btn" href="#">
            <span>
                Leggi tutto
            </span>
        </a>
        <div class="nulla">
        </div>
    </div>

     <div class="blogPost">
        <h3 class="blogPostTit"><a href="#">Via del Corso</a></h3>
        
        <div class="postDett">
            <div style="float:left;">
                <span class="postAuthor">pubblicato da: <strong>Federico</strong></span>
                <span class="postDate">il <em>15/05/2012</em></span>
            </div> 
            <span class="commentsNum">10</span>
        </div>

        <div class="nulla">
        </div>
        <div class="blogPostImg">
            <img src="/images/via-del-corso.jpg" alt="via del corso" />
        </div>
        <div class="blogPostTxt">
        Oggi inzia una nuova  rubrica per i nostri  “followers", fatta di foto e piccole descrizioni attraverso cui cercheremo di farvi sentire più vicina la nostra città.

Evitaremo riferimenti storici, facilmente rintracciabili un po' ovunque nella rete, per svelare piuttosto qualche piccolo segreto delle vie che andremo a descrivere.

La via più indicata per iniziare questa avventura e’ sicuramente Via del Corso, arteria principale della vita del Centro Storico e punto di raccordo per tante attrazioni della Citta’ Eterna. Ogni romano che si rispetti ha percorso questa via  almeno una volta se non altro dopo aver marinato la scuola, grazie soprattutto all’alta densità di negozi di marche alla moda che attragono tanto i ragazzi. 
Luogo privilegiato per i giornalisti che molto spesso vi realizzano le loro interviste all'"uomo della strada", questa lunghissima via romana si caratterizza anche per l’adiacenza con Montecitorio (il parlamento italiano), con il conseguente pellegrinaggio di tantissimi curiosi
        </div>
        <div class="social">

            <div class="fb">
                <a name="fb_share" type="button_count" href="http://www.facebook.com/sharer.php">
                Condividi</a><script src="http://static.ak.fbcdn.net/connect.php/js/FB.Share"
                type="text/javascript"></script>
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
        <a class="btn" href="#">
            <span>
                Leggi tutto
            </span>
        </a>
        <div class="nulla">
        </div>
    </div>
</div>

<div id="blogColDx">
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

     <div class="boxblogColDx" style="position:relative;">

         <h4 class="titblogColDx">Da vedere a Roma</h4>

         <div id="daVedereCont">

             <div id="scrollDaVedere_navigator">
             </div>

             <div id="scrollDaVedere">
        <div class="items" id="items_daVedere">   
            <div class="scrollDaVedere_item">
                <a href="#">
                <span class="imgDaVedereCont">
                    <img src="/images/altare-patria.jpg" alt="altare della patria" />
                </span>
                <span class="nomeDaVedere">Altare della Patria</span>
                <span class="doveDaVedere">vicino a <strong>Piazza Venezia</strong></span>
                </a>
            </div>
            <div class="scrollDaVedere_item">
                <a href="#"><span class="imgDaVedereCont">
                    <img src="/images/altare-patria.jpg" alt="altare della patria" />
                </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">
                    vicino a <strong>Piazza Venezia</strong></span> </a>
            </div>
            <div class="scrollDaVedere_item">
                <a href="#"><span class="imgDaVedereCont">
                    <img src="/images/altare-patria.jpg" alt="altare della patria" />
                </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">
                    vicino a <strong>Piazza Venezia</strong></span> </a>
            </div>
            <div class="scrollDaVedere_item">
                <a href="#"><span class="imgDaVedereCont">
                    <img src="/images/altare-patria.jpg" alt="altare della patria" />
                </span><span class="nomeDaVedere">Altare della Patria</span> <span class="doveDaVedere">
                    vicino a <strong>Piazza Venezia</strong></span> </a>
            </div>
        </div>
        </div>

             <script type="text/javascript">
                 $(document).ready(function () {
                     $("#scrollDaVedere").scrollable({ size: 1, clickable: false, circular: true, keyboard: 'static' }).navigator("#scrollDaVedere_navigator").autoscroll({ circular: true, autoplay: true });
                 });
             </script>

         </div>

        <a class="btn" href="#" id="altroDaVedere">
            <span>
                Altro
            </span>
        </a>

     </div>

</div>

</asp:Content>
