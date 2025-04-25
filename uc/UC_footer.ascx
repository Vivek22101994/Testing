<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_footer.ascx.cs" Inherits="RentalInRome.uc.UC_footer" %>
<%@ Register src="UC_static_block.ascx" tagname="UC_static_block" tagprefix="uc1" %>
<asp:HiddenField ID="HF_pid_lang" runat="server" Value="1" />
		<uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="0" />

<table class="indirizzi">
	<tr>
		<td align="left" valign="middle" style="width:256px;">
			<strong>Roma</strong><br />
			<strong style="color: #FFF">
                <%= CurrentSource.getSysLangValue("lblufficiooperativo")%>:</strong> 
			 Via Marianna Dionigi, 57 - 00193 Roma
		</td>
		<td align="left" valign="middle" style="width:256px;">
			<strong>Roma</strong><br />
			<strong style="color: #FFF">
                <%= CurrentSource.getSysLangValue("lblufficioamm")%>:</strong> 
			Via Appia Nuova, 677 - 00179 Roma
		</td>
		<!--<td align="left" valign="middle">
			<strong>Roma - Cerenova</strong><br />
			<strong style="color: #FFF">
                <%= CurrentSource.getSysLangValue("lblufficiooperativo")%></strong><br />
			Via Mantova, 2<br />
			00050 Cerenova (Rm)
		</td>
		<td align="left" valign="middle">
			<strong>New York City</strong><br />
			<strong style="color: #FFF">
                <%= CurrentSource.getSysLangValue("lblofficerappr")%></strong><br />
			401 East 34th Street (N8P)<br />
			New York, NY 10016
		</td>-->
	</tr>
</table>
<div class="nulla">
</div>

<div class="links societa" style="color: #FFF;">
	<strong>Rental in Rome S.r.l.</strong> - P. IVA: 07824541002 - 2003 - 2015 © all rights reserved
</div>
<div id="social">
	<a class="social" id="facebook" href="http://www.facebook.com/rentalinrome" target="_blank"></a>
	<a class="social" id="twitter" href="http://www.twitter.com/rentalinrome" target="_blank"></a>
</div>

<div class="nulla">
</div>

<div class="links" style="text-transform:lowercase;">
    <a href="<%=CurrentSource.getPagePath("5", "stp", CurrentLang.ID.ToString()) %>">
        <%= CurrentSource.getSysLangValue("lblAboutUs")%></a>
	|
	<a href="<%=CurrentSource.getPagePath("16", "stp", CurrentLang.ID.ToString()) %>">
    FAQs</a>
	|
    <a href="<%=CurrentSource.getPagePath("9", "stp", CurrentLang.ID.ToString()) %>">
        <%= CurrentSource.getSysLangValue("menu_Press")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("21", "stp", CurrentLang.ID.ToString()) %>">
        <%=CurrentSource.getSysLangValue("reqServices")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("10", "stp", CurrentLang.ID.ToString()) %>">
        <%=CurrentSource.getSysLangValue("reqTransport1")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("11", "stp", CurrentLang.ID.ToString()) %>">
        <%=CurrentSource.getSysLangValue("lblGuestbook")%></a>
	|
    <a href="http://www.yourweddinginrome.com/" target="_blank">
        <%=CurrentSource.getSysLangValue("lblmatrimonioaroma")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("4", "stp", CurrentLang.ID.ToString()) %>">
        <%= CurrentSource.getSysLangValue("lblContacts")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("8", "stp", CurrentLang.ID.ToString()) %>">
        <%= CurrentSource.getSysLangValue("lblYouArePropertyOwner")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("19", "stp", CurrentLang.ID.ToString()) %>">
        <%= CurrentSource.getSysLangValue("reqTermsOfService")%></a>
	|
    <a href="<%=CurrentSource.getPagePath("18", "stp", CurrentLang.ID.ToString()) %>">
    LINKS</a>
</div>
<div class="links" style="font-size: 11px; margin-top: 5px;">
	<strong style="font-size: 12px;">
        <%= CurrentSource.getSysLangValue("lblpartner1")%></strong>
	<br />
	<a href="http://www.rentalsinmauritius.com/" target="_blank">Rentals in Mauritius</a>
	|
	<a href="http://www.magarental.com" target="_blank" title="magarental">magarental</a>
</div>
<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
    <a href="http://www.villabismarck.com/" target="_blank" title="capri villa for rent">Capri Villa</a>
    |
    <a href="http://www.ota-berlin.de/english/" target="_blank">Berlin Apartments</a>
    |
    <a href="http://www.parisattitude.com/" target="_blank">Paris Apartments</a>
    |
</asp:PlaceHolder>
<table class="pay" style="float:left; position:relative; z-index:1;">
	<tr>
		<td valign="middle">
			<img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/master_p.gif" width="45" height="28">
		</td>
		<td valign="middle">
			<img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/css/visa_p.gif" width="43" height="28">
		</td>
        <td valign="middle">
            <uc1:UC_static_block ID="UC_static_block2" runat="server" BlockID="9" />

            <script>window.onload = function () { sendData('18778'); };</script>
        </td>
        <td valign="middle" align="right" style="width:781px; position:relative; z-index:1;">


            <uc1:UC_static_block ID="UC_static_block3" runat="server" BlockID="12" />
          
        </td>
	</tr>
</table>

<div class="nulla">
</div>
<table class="members">
	<tr>
		<td valign="middle" align="center">
			<%= CurrentSource.getSysLangValue("lblpartner")%>
		</td>
		<td>
		</td>
		<td valign="middle" align="center">
            <%= CurrentSource.getSysLangValue("lblsupport")%> Warchild.org
		</td>
	</tr>
	<tr>
		<td valign="middle" align="center">
			<a href="http://www.enit.it/it/" rel="nofollow" target="_blank">
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/enitlogo.gif" width="71" height="40">
			</a>
		</td>
		
		<td align="center" valign="middle" style="width: 590px;" id="shinystatContent">
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
        </td>
		<td>
			<a target="_blank" href="http://www.warchild.org/" rel="nofollow">
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/warchild.jpg" width="50" height="48" />
			</a>
		</td>
	</tr>
</table>



