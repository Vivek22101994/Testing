<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_press.aspx.cs" Inherits="RentalInRome.stp_press" %>

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
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <h3 class="underlined">
		<%=ltr_title.Text %></h3>
	<div class="nulla">
	</div>
    <%= ltr_description.Text %>
	<table cellpadding="20" cellspacing="0" id="loghi" runat="server" visible="false">
		<tr>
			<td colspan="3" align="center" valign="middle">
				<a rel="nofollow" href="http://frugaltraveler.blogs.nytimes.com/2011/03/01/the-travel-industry-shares-money-saving-tips-and-freebies/" target="_blank">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/newyorktimesnew.gif" width="458" height="58" style="max-width: 458px; width: 458px;" /></a>
			</td>
		</tr>
		<tr>
			<td colspan="3" align="center" valign="middle">
				<a rel="nofollow" href="http://www.irishtimes.com/newspaper/travel/2011/0312/1224291935659.html" target="_blank">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/irishtimes.gif" width="320" height="31" style="max-width: 458px; width: 458px;" /></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<p align="center">
					<a rel="nofollow" href="http://www.frommers.com/articles/6066.html" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/frommers2.jpg"></a></p>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://images.businessweek.com/ss/06/07/shortterm_rentals/source/8.htm" target="_blank">
					<img border="0" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/businessweek.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<p>
					<a rel="nofollow" href="http://traveler.nationalgeographic.com/2010/01/feature/rome-text/1" target="_blank">
						<img height="76" width="191" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/logonational1.jpg"></a></p>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<p align="center">
					<a rel="nofollow" href="http://travel.independent.co.uk/europe/article2056536.ece" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/independent_new2.jpg"></a></p>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.nypost.com/p/go_home_Crev0ZHIsvg3VshSX2WRtI" target="_blank">
					<img height="43" border="0" width="273" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/nypost2.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.philly.com/philly/travel/destinations/europe/20080302_Senior_Traveler__Apartment_instead_of_hotel.html" target="_blank">
					<img height="43" border="0" width="273" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/thephiladelphia.jpg"></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.fodors.com/community/europe/four-friends-eight-days-easter-week-in-rome-a-trip-report.cfm" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/fodors.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.dailyherald.com/story/?id=155699" target="_blank">
					<img height="76" border="0" width="191" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/dailyherald2.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.concierge.com/cntraveler/articles/11139?pageNumber=7" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/concierge_new2.jpg"></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<p align="center">
					<a rel="nofollow" href="http://www.fodors.com/community/europe/four-friends-eight-days-easter-week-in-rome-a-trip-report.cfm" target="_blank"></a>
					<a rel="nofollow" href="http://technology.timesonline.co.uk/tol/news/tech_and_web/article475541.ece" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/thetimes2.jpg"></a></p>
			</td>
			<td align="center" valign="middle">
				<a rel="nofollow" href="http://www.losviajeros.com/index.php?name=Forums&amp;file=search&amp;mode=results" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/foros2.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://community.myfoxspringfield.com/blogs/Lisa_Breckenridge/2008/07/02/roman_holiday" target="_blank">
					<img height="74" border="0" width="150" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/foxlosangeles2.jpg"></a><a rel="nofollow" href="http://www.myfoxny.com/dpp/good_day_ny/tech_it_out/Tech_It_Out_Travel" target="_blank"></a><a
						rel="nofollow" href="http://romejapan.com/postDetail.jsf?post_id=122" target="_blank"></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<div align="center">
					<a rel="nofollow" href="http://www.ocholeguas.com/2009/06/18/europa/1245341051.html?pestana=3" target="_blank">
						<img height="43" border="0" width="273" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/elmundo.gif"></a>
			</td>
			<td valign="middle" align="center">
				<div align="center">
					<a rel="nofollow" href="http://politiken.dk/turengaartil/rejsenyt/storbyogkultur/ECE1121530/15-fede-ferielejligheder-i-fem-storbyer/" target="_blank">
						<img height="39" border="0" width="300" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/politiken.gif"></a></div>
			</td>
			<td valign="middle" align="center">
				<div align="center">
					<a rel="nofollow" href="http://album.turizm.ru/24453/mess_2" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/turizm.jpg"></a></div>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<a target="_blank" rel="nofollow" href="http://forum.gazeta.pl/forum/w,206,98401384,98401384,Kilka_porad_po_powrocie_z_Rzymu.html">
					<img height="55" width="283" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/gazetaturystykalogo.gif"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://keskustelu.plaza.fi/matkalaukku/keskustelu/t1515409" target="_blank">
					<img height="48" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/suomi24new.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.lametayel.co.il/%25D7%2590%25D7%2599%25D7%2598%25D7%259C%25D7%2599%25D7%2594%20%25D7%25A9%25D7%2591%25D7%2595%25D7%25A2%25D7%2599%25D7%2599%25D7%259D%20%25D7%259E%25D7%25A7%25D7%2599%25D7%25A4%25D7%2599%25D7%259D%20%25D7%2591%25D7%25A6%25D7%25A4"
					target="_blank">
					<img height="65" border="0" width="227" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/lametayel.jpg"></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<a target="_blank" rel="nofollow" href="http://www.gazeta.lv/story/14118.html">
					<img height="59" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/gazeta.gif"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.subbota.com/2010/03/31/ne021.html" target="_blank">
					<img height="85" border="0" width="232" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/subbota.gif"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.theledger.com/article/20100618/NEWS/6185007/1178?p=2&amp;tc=pg" target="_blank">
					<img height="50" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/theledger.gif"></a>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://www.concierge.com/cntraveler/articles/11139?pageNumber=7" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/conde_new2.jpg"></a>
			</td>
			<td valign="middle" align="center">
				<a rel="nofollow" href="http://goitaly.about.com/od/vacationrentalsinitaly/Vacation_Rentals_Italy_Italian_Weekly_Vacation_Rentals.htm" target="_blank">
					<img height="60" border="0" width="250" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/aboutcomitalytravel.gif"></a>
			</td>
			<td valign="middle" align="center">
				<a target="_blank" rel="nofollow" href="http://www.zoover.nl/italie/lazio-latium/rome/sinibaldi-palace-a/appartement/informatie">
					<img height="82" width="227" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/zoover.gif"></a>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<table cellpadding="10" cellspacing="0" style="margin: 0 20px; width: 860px;">
					<tr>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a target="_blank" rel="nofollow" href="http://books.google.it/books?id=zpCKVslIwigC&amp;pg=PA25&amp;dq=rentalinrome&amp;hl=it&amp;ei=hvMqTZn2JImr8QP15rm0Ag&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=3&amp;ved=0CEUQ6AEwAg#v=onepage&amp;q=rentalinrome&amp;f=false">
								<img height="307" border="0" align="left" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/paulinefrommers.gif"></a>
						</td>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=WKDSx5YDeaIC&amp;pg=PA29&amp;dq=rentalinrome&amp;hl=en&amp;ei=oS11TPLeIIiWswbu6tjjBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=4&amp;ved=0CEQQ6AEwAw#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/frommersirreverent.gif"></a>
						</td>
						<td valign="middle" align="center" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=KZbFXhbMbjgC&amp;pg=PA468&amp;dq=rentalinrome&amp;hl=en&amp;ei=oS11TPLeIIiWswbu6tjjBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=2&amp;ved=0CDcQ6AEwAQ#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/askarthurfrommer.gif"></a>
						</td>
						<td valign="middle" align="right" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=rMkF_Lq8fyoC&amp;pg=PA145&amp;dq=rentalinrome&amp;hl=en&amp;ei=oS11TPLeIIiWswbu6tjjBQ&amp;sa=X&amp;oi=book_result&amp;ct=book-thumbnail&amp;resnum=1&amp;ved=0CDMQ6wEwAA#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/lonelyplanet.gif"></a>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<table cellpadding="10" cellspacing="0" style="margin: 0 20px; width: 860px;">
					<tr>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=uWBdgmYsFO0C&amp;pg=PA130&amp;dq=rentalinrome&amp;hl=en&amp;ei=oS11TPLeIIiWswbu6tjjBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=7&amp;ved=0CFUQ6AEwBg#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/petitfute2.gif"></a>
						</td>
						<td valign="middle" align="center" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=ctQoiESdj8wC&amp;pg=PA291&amp;dq=rentalinrome&amp;hl=en&amp;ei=3i91TOb8F4PJswak7vTdBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=3&amp;ved=0CDUQ6AEwAjgK#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/petitfute3.gif"></a>
						</td>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=e7Fz7FwF714C&amp;pg=PA50&amp;dq=rentalinrome&amp;hl=en&amp;ei=3i91TOb8F4PJswak7vTdBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=4&amp;ved=0CD0Q6AEwAzgK#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/timeout.gif"></a>
						</td>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=aCZOvYuxH7YC&amp;pg=PA287&amp;dq=rentalinrome&amp;hl=en&amp;ei=oS11TPLeIIiWswbu6tjjBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=6&amp;ved=0CFAQ6AEwBQ#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" align="right" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/petifute.gif"></a>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<table cellpadding="10" cellspacing="0" style="margin: 0 20px; width: 860px;">
					<tr>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=BxV2_SgGidUC&amp;pg=PA292&amp;dq=rentalinrome&amp;hl=en&amp;ei=eDB1TKnjFojFswaK_uHcBQ&amp;sa=X&amp;oi=book_result&amp;ct=result&amp;resnum=3&amp;ved=0CDsQ6AEwAjgU#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" align="left" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/lonelyplanet2.gif"></a>
						</td>
						<td valign="middle" align="center" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=UesEFf-7gpAC&amp;pg=PA204&amp;dq=rentalinrome&amp;hl=en&amp;ei=osqVTLyWA8HJswacrP1a&amp;sa=X&amp;oi=book_result&amp;ct=book-thumbnail&amp;resnum=7&amp;ved=0CFIQ6wEwBg#v=onepage&amp;q=rentalinrome&amp;f=false"
								target="_blank">
								<img height="307" border="0" align="middle" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/petifute4.gif"></a>
						</td>
						<td align="center" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=OOOrKIcIWqEC&amp;pg=PA165&amp;dq=www.rentalinrome.com&amp;hl=en&amp;ei=Acl4TNDDCoWWswaU8cWyDQ&amp;sa=X&amp;oi=book_result&amp;ct=book-thumbnail&amp;resnum=5&amp;ved=0CEcQ6wEwBA#v=onepage&amp;q=www.rentalinrome.com&amp;f=false"
								target="_blank">
								<img height="307" border="0" width="207" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/frommers2.gif"></a>
						</td>
						<td height="24" align="center" width="224" valign="middle" style="padding-top: 0px;">
							<a rel="nofollow" href="http://books.google.com/books?id=RqeuSiAUqCUC&amp;pg=PA436&amp;dq=www.rentalinrome.com&amp;hl=en&amp;ei=rcl4TKWLPITBswakmMCyDQ&amp;sa=X&amp;oi=book_result&amp;ct=book-thumbnail&amp;resnum=3&amp;ved=0CDcQ6wEwAjgU#v=onepage&amp;q=www.rentalinrome.com&amp;f=false"
								target="_blank">
								<img height="307" border="0" align="right" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/sportticket.gif"></a>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td height="20" align="center" width="334">
				&nbsp;
			</td>
			<td height="20" align="center" width="423">
				&nbsp;
			</td>
			<td height="20" align="center" width="344">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td height="72" align="center">
				<a target="_blank" rel="nofollow" href="http://www.zoover.nl/italie/lazio-latium/rome/sinibaldi-palace-a/appartement/informatie"></a>
				<a rel="nofollow" href="http://www.capecodonline.com/apps/pbcs.dll/article?AID=/20100613/LIFE06/100609749/-1/NEWSMAP" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/capecodonline.gif"></a>
			</td>
			<td height="72" align="center">
				<a target="_blank" rel="nofollow" href="http://travel.blog.nl/budgettips/2010/05/04/tips-voor-een-goedkope-citytrip">
					<img height="59" width="370" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/travelblognl.gif"></a>
			</td>
			<td height="72" align="center">
				<a rel="nofollow" href="http://www.petitfute.com/adresses/etablissement/index/id/298763?nopopin=1"></a>
				<a target="_blank" rel="nofollow" href="http://www.petitfute.com/adresses/etablissement/index/id/298763?nopopin=1">
					<img height="79" width="203" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/petitfutenuovo.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="72" align="center">
				<a rel="nofollow" href="http://romejapan.com/postDetail.jsf?post_id=122" target="_blank">
					<img height="65" border="0" width="161" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/romejapan2.jpg"></a>
			</td>
			<td height="72" align="center">
				<a rel="nofollow" href="http://www.travelforum.pl/w-ochy-hotele-o-rodki-wypoczynkowe/6857-apartamenty-w-centrum-rzymu-wiarygodna-alternatywa.html" target="_blank">
					<img height="65" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/travelforum.jpg"></a>
			</td>
			<td height="72" align="center">
				<a rel="nofollow" href="http://www.lametayel.co.il/%25D7%2590%25D7%2599%25D7%2598%25D7%259C%25D7%2599%25D7%2594%20%25D7%25A9%25D7%2591%25D7%2595%25D7%25A2%25D7%2599%25D7%2599%25D7%259D%20%25D7%259E%25D7%25A7%25D7%2599%25D7%25A4%25D7%2599%25D7%259D%20%25D7%2591%25D7%25A6%25D7%25A4"
					target="_blank">
					<img height="65" border="0" width="227" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/lametayel.jpg"></a>
			</td>
		</tr>
		<tr>
			<td height="72" align="center">
				<a rel="nofollow" href="http://www.ciao.fr/rentalinrome_com__Avis_927559" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/ciao1.jpg"></a>
			</td>
			<td height="72" align="center">
				<a rel="nofollow" href="http://www.ilsole24ore.com/art/tecnologie/2011-01-06/vacanze-online-065054.shtml?uuid=AYpjiYxC" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/sole24ore_new2.jpg"></a>
			</td>
			<td height="72" align="center">
				<a rel="nofollow" href="http://expatforums.com/rome-and-florence-50129/" target="_blank">
					<img height="57" border="0" width="250" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/expatforum2.jpg"></a><a rel="nofollow" href="http://www.losviajeros.com/index.php?name=Forums&amp;file=search&amp;mode=results"
						target="_blank"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://googlebox.cyberpresse.ca/search?site=lesnouvellesCBP&amp;entqr=0&amp;output=xml_no_dtd&amp;sort=date:D:S:d1&amp;client=default_frontend&amp;ud=1&amp;oe=ISO-8859-1&amp;ie=ISO-8859-1&amp;proxystylesheet=cbp2_result&amp;q=rentalinrome"
					target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/lapresse2.jpg"></a><a target="_blank" href="http://www.gazeta.lv/story/14118.html"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.tipsdeviajero.com/2009/04/europa-hospedarse-en-un-depa.html" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/tipsdeviajero.jpg"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.matraqueando.com.br/italia-a-50-euros-por-dia-parte-1" target="_blank">
					<img height="79" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/matraqueando.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://forum.littleone.ru/" target="_blank">
					<img height="60" border="0" width="240" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/forumlittleone.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://italie.blogo.nl/2010/03/26/appartement-huren-in-rome/" target="_blank">
					<img height="60" border="0" width="267" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/italieblogo.gif"></a>
			</td>
			<td height="77" align="center">
				<a target="_blank" rel="nofollow" href="http://www.chas-daily.com/win/index.html">
					<img height="86" border="0" width="155" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/chasdaily.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://viajeaqui.abril.com.br/vt/materias/vt_materia_431638.shtml?page=2" target="_blank">
					<img height="55" border="0" width="240" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/viajeaqui.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.italia-ru.it/forums/2010/01/14/gostinitsa-rime" target="_blank">
					<img height="49" border="0" width="240" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/italiaru.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://foros.telva.com/viajes/6162-hoteles-buenos-roma.html" target="_blank">
					<img height="65" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/telva.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.gaytravel.com/plan/accommodation/rental-in-rome" target="_blank">
					<img height="45" border="0" width="240" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/gaytravel.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://italia-vera.ru/2008/12/24/otel_v_rime/" target="_blank">
					<img height="74" border="0" width="194" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/italiavera.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://anothertrip.wordpress.com/" target="_blank">
					<img height="67" border="0" width="224" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/anothertrip.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.reisforum.info/algemeen/4206-goedkoop-reizen-europa.html" target="_blank">
					<img height="81" border="0" width="190" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/reisforum.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.goldenline.pl/forum/1193100/20-2410-rzym-szukam-fajnego-apatamentu-w-rzymie-i-co-w-te-4-dni-zobaczyctylko-4" target="_blank">
					<img height="73" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/goldenline.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://tripster.ru/questions/5370/" target="_blank">
					<img height="60" border="0" width="269" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/tripster.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="83" align="center">
				<a rel="nofollow" href="http://www.projectwedding.com/ourwedding/laurenandkurt/accommodations" target="_blank">
					<img height="55" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/laurenandkurt.gif"></a>
			</td>
			<td height="83" align="center">
				<a rel="nofollow" href="http://travel.kosmix.com/topic/Villa_Roma_Rentals" target="_blank">
					<img height="67" border="0" width="220" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/kosmix.gif"></a>
			</td>
			<td height="83" align="center">
				<a rel="nofollow" href="http://www.anfiteatro.be/Reistips.htm" target="_blank">
					<img height="78" border="0" width="160" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/anfiteatro.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="80" align="center">
				<a rel="nofollow" href="http://voyageforum.com/v.f?do=resultats_recherche&amp;destination=&amp;search_forum=all&amp;search_string=www.rentalinrome.com&amp;search_type=AND&amp;search_fields=sb&amp;discussion=0&amp;pas_tout_inclus=0&amp;pas_petites_annonces=0&amp;photo=0&amp;search_user_username=&amp;search_time=&amp;sb=score"
					target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/voyage2.jpg"></a>
			</td>
			<td height="80" align="center">
				<a rel="nofollow" href="http://fattoinitalia1.blogspot.com/2008/11/apartamentos-para-alugar-na-itlia.html" target="_blank">
					<img height="63" border="0" width="240" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/fattoinitalia.gif"></a>
			</td>
			<td height="80" align="center">
				<a rel="nofollow" href="http://cafe.naver.com/firenze.cafe?iframe_url=/ArticleRead.nhn?articleid=708572" target="_blank">
					<img height="76" border="0" width="165" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/eurang.gif"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://britishexpats.com/forum/showthread.php?t=264530" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/britishexpats2.jpg"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.wereldwijzer.nl/showthread.php?t=88298" target="_blank">
					<img height="58" border="0" width="268" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/wereldwijzer.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.101cookbooks.com/archives/late-summer-favorites-list-recipe.html" target="_blank">
					<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/101cooksbooks1.jpg"></a>
			</td>
		</tr>
		<tr>
			<td height="77" align="center">
				<a rel="nofollow" href="http://www.familytravelforum.com/what/hotels/10896-Rome-Italy-Hotels-And-Resorts.html?p=3" target="_blank">
					<img height="70" border="0" width="150" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/familytravelforum.gif"></a>
			</td>
			<td height="77" align="center">
				<a rel="nofollow" href="http://mymelange.net/mymelange/2009/03/travel-tip-tuesday-finding-rome-accommodation.html" target="_blank">
					<img height="91" border="0" width="230" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/mymelange.gif"></a>
			</td>
			<td height="77" align="center">
				<a target="_blank" rel="nofollow" href="http://www.flipkey.com/frontdesk/view/4337">
					<img height="53" width="196" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/flipkeylogo.gif"></a><a rel="nofollow" href="http://keskustelu.plaza.fi/matkalaukku/keskustelu/t1515409" target="_blank"></a>
			</td>
		</tr>
		<tr>
			<td height="75">
				<p align="center">
					<a rel="nofollow" href="http://www.romealive.com/rome_blog/" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/romealive.gif"></a></p>
			</td>
			<td height="75">
				<p align="center">
					<a rel="nofollow" href="http://members.virtualtourist.com/m/ac3a3/23513/" target="_blank">
						<img height="66" border="0" width="141" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/virtualtourist2.jpg"></a></p>
			</td>
			<td height="75">
				<p align="center">
					<a rel="nofollow" href="http://www.familytravelforum.com/articles/article/10896" target="_blank">
						<img height="60" border="0" width="275" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/familytravel2.jpg"></a></p>
			</td>
		</tr>
		<tr>
			<td height="75">
				<div align="center">
					<a target="_blank" rel="nofollow" href="http://www.wegwijzer.be/reislinks/itali%C3%AB">
						<img height="84" width="143" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/wegwijzerlogo.gif"></a></div>
			</td>
			<td>
				<div align="center">
					<a target="_blank" rel="nofollow" href="http://www.in2life.gr/escape/infoguide/articles/177991/article.aspx">
						<img height="83" width="167" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/in2lifelogo.gif"></a></div>
			</td>
			<td height="75">
				<div align="center">
					<a target="_blank" rel="nofollow" href="http://cruisetalk.org/2009/11/jet-lag-and-airline-food-the-worst-part-of-a-trans-atlantic-cruise.html">
						<img height="54" width="224" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/cruisetalcklogo.png"></a></div>
			</td>
		</tr>
		<tr>
			<td height="75">
				<div align="center">
					<a rel="nofollow" href="http://forum.viva.nl/forum/list_messages/21072" target="_blank">
						<img height="66" border="0" width="134" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/vivalogo2.jpg"></a></div>
			</td>
			<td>
				<div align="center">
					<a rel="nofollow" href="http://www.doctorsreview.com/node/44" target="_blank">
						<img height="74" border="0" width="174" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/doctorreview2.jpg"></a></div>
			</td>
			<td height="75">
				<div align="center">
					<a target="_blank" rel="nofollow" href="http://www.projectwedding.com/ourwedding/laurenandkurt/accommodations">
						<img height="59" width="260" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/projectweddinglogo.gif"></a></div>
			</td>
		</tr>
		<tr>
			<td height="75">
				<p align="center">
					<a rel="nofollow" href="http://www.weddingsonline.ie/discussion/viewtopic.php?t=122728&amp;highlight=rentalinrome&amp;sid=8a940f57965ce13fc12fe160ade2a027" target="_blank">
						<img height="61" border="0" width="214" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/weddingsonline2.jpg"></a></p>
			</td>
			<td>
				<p align="center">
					<a rel="nofollow" href="http://wiki.apache.org/cocoon/GT2007Apartments" target="_blank">
						<img height="40" border="0" width="244" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/cocoon2.jpg"></a></p>
			</td>
			<td height="75">
				<p align="center">
					<a rel="nofollow" href="http://rome-apartments.center.us.com/?search=true" target="_blank">
						<img height="54" border="0" width="214" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/centerusa2.jpg"></a></p>
			</td>
		</tr>
		<tr>
			<td height="50">
				<p align="center">
					<a rel="nofollow" href="http://www.wired2theworld.com/ROMEplanning.html" target="_blank">
						<img height="54" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/wired2theworld2.jpg"></a></p>
			</td>
			<td rowspan="2">
				<p align="center">
					<a rel="nofollow" href="http://www.languageinitaly.com/EN/language_schools_in_italy.php#link006" target="_blank">
						<img height="143" border="0" width="148" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/logodante_new3.jpg"></a></p>
			</td>
			<td height="50" align="center">
				<p align="center">
					<a rel="nofollow" href="http://www.squidoo.com/spotlight-on-rome-italy" target="_blank">
						<img height="54" border="0" width="200" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/squidoo.gif"></a></p>
			</td>
		</tr>
		<tr>
			<td height="50">
				<p align="center">
					<a rel="nofollow" href="http://www.43places.com/people/progress/thihoa/1998688" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/43places2.jpg"></a></p>
			</td>
			<td height="50">
				<p align="center">
					<a rel="nofollow" href="http://frugaltraveler.blogs.nytimes.com/2011/03/01/the-travel-industry-shares-money-saving-tips-and-freebies/" target="_blank">
						<img height="60" border="0" width="251" src="<%=CurrentAppSettings.ROOT_PATH %>images/logos/frugaltraveler.gif"></a></p>
			</td>
		</tr>
	</table>
	<div class="nulla">
	</div>

</asp:Content>
