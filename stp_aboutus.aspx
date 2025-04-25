<%@ Page Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_aboutus.aspx.cs" Inherits="RentalInRome.stp_aboutus" %>


<%@ Register src="uc/UC_apt_in_rome_bottom.ascx" tagname="UC_apt_in_rome_bottom" tagprefix="uc1" %>


<%@ Register src="uc/UC_static_block.ascx" tagname="UC_static_block" tagprefix="uc2" %>

 
<%@ Register src="uc/UC_Rental_Recommended.ascx" tagname="UC_Rental_Recommended" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
<script type="text/javascript" src="/jquery/plugin/jquery.xslider.min.js"></script>
<script type="text/javascript">

$(document).ready(function () {
    $(window).load(function () {
       
        $('#scrollPress').xslider({ effect: 'fade', timeout: 3000, autoPlay: true, navigation: null });
       
    });
});
</script>
<style>
    #testoMain #scrollPress li {width: 310px; height:105px;}
    #testoMain #scrollPress li { padding-left:0; background:none;}
    #testoMain #press {margin-top:24px;}
</style>
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
    
    <div id="testoMain">
	<h3 class="underlined">
		<%= ltr_title.Text%></h3>
	<div class="mainContent">


		<%= ltr_description.Text%>

            <script type="text/javascript">
                GLOBAL_imgLoader.push(new Array("homePress_img_1", "/images/press.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_2", "/images/press-times.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_3", "/images/press-inquirer.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_4", "/images/press-independent.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_5", "/images/press-business.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_6", "/images/press-traveler.jpg"));
                GLOBAL_imgLoader.push(new Array("homePress_img_7", "/images/press-frommers.jpg"));
             </script>

        <div style="float:right;" >
                <div id="press">
                    <a href="press.htm"></a>
                    <span>Press</span>
                    <ul id="scrollPress">
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="new york post" id="homePress_img_1" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="the times" id="homePress_img_2" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="the philadelphia inquirer" id="homePress_img_3" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="the independent" id="homePress_img_4" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="business week" id="homePress_img_5" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="national geographic traveler" id="homePress_img_6" />
                        </li>
                        <li>
                            <img src="/images/css/blankimg.png" width="632" height="62" alt="frommer's" id="homePress_img_7" />
                        </li>
                    </ul>
                </div>
                <div class="nulla"></div>
                <a href="http://frugaltraveler.blogs.nytimes.com/2011/03/01/the-travel-industry-shares-money-saving-tips-and-freebies/" rel="nofollow" class="lastPress" target="_blank">
                    <span><%= CurrentSource.getSysLangValue("lblleggiarticolo").Replace("'","\\'")%></span>
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/nytimesmini.jpg" style="border: none; width: 100px;" align="right" />
                </a>
                <div class="nulla"></div>
            </div>

	</div>
</div>
<div id="colDx">
	<h3 class="underlined">
		<%= CurrentSource.getSysLangValue("lblOurWebsiteIsRecommendedBy")%>
	</h3>
	<uc3:UC_Rental_Recommended ID="UC_Rental_Recommended1" runat="server" />
</div>
<div class="nulla">
</div>

<div id="scegliereNoi" class="bigBox">

    <%= ltr_summary.Text%>

</div>

<div class="nulla">
</div>
<div class="bigBox" id="staffCont">
	<div>
		<h3>
			<%= CurrentSource.getSysLangValue("lblOurStaff")%>
		</h3>
		<div class="nulla">
		</div>
		<div id="staff">
			<div class="nulla">
			</div>
			<img src="/images/staff/gruppo.jpg" alt="rental in rome staff" style="margin: 0 0 15px 8px;" />
			<div class="nulla">
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/jacopo-calabro.jpg" alt="Jacopo Calabrò" />
				<span class="nomeStaff">Jacopo Calabrò (Jack)</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Chairman
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:jacopo.calabro@rentalinrome.com">jacopo.calabro@rentalinrome.com</a>
			</div>
			<!--<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/francesco-marchese.jpg" alt="Francesco Marchese" />
				<span class="nomeStaff">Francesco Marchese</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Accountant
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:francesco.marchese@rentalinrome.com">francesco.marchese@rentalinrome.com</a>
			</div>
			
            
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/michael-smith.jpg" alt="Michael Smith" />
				<span class="nomeStaff">Michael Smith</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Customer Services Manager
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:michael.smith@rentalinrome.com">michael.smith@rentalinrome.com</a>
			</div>
            
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ger.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/sonja-wolfram.jpg" alt="Sonja Wolfram" />
				<span class="nomeStaff">Sonja Wolfram</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:sonja.wolfram@rentalinrome.com">sonja.wolfram@rentalinrome.com</a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-fra.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/francesca-scaccini.jpg" alt="Francesca Scaccini" />
				<span class="nomeStaff">Francesca Scaccini</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Transport and Tours Services Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:pickupservice@rentalinrome.com">pickupservice@rentalinrome.com</a>
			</div>
            
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ger.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/marco-denzler.jpg" alt="Marco Denzler" />
				<span class="nomeStaff">Marco Denzler</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:marco.denzler@rentalinrome.com">marco.denzler@rentalinrome.com</a>
			</div>
			-->
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-fra.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/claudio-triconnet.jpg" alt="Claudio Triconnet" />
				<span class="nomeStaff">Claudio Triconnet</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:claude.triconnet@rentalinrome.com">claude.triconnet@rentalinrome.com</a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/barbara-giovannelli.jpg" alt="Barbara Giovannelli" />
				<span class="nomeStaff">Barbara Giovannelli</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:barbara.giovannelli@rentalinrome.com">barbara.giovannelli@rentalinrome.com</a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/francesca-sangiorgio.jpg" alt="Francesca Sangiorgio" />
				<span class="nomeStaff">Francesca Sangiorgio</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Administration & Maintenance Coordinator
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:francesca.sangiorgio@rentalinrome.com">francesca.sangiorgio@rentalinrome.com</a>
			</div>
			
			<!-- 
                <div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ger.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-fra.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/cynthia-wolfram.jpg" alt="Cynthia Wolfram" />
				<span class="nomeStaff">Cynthia Wolfram</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:cynthia.wolfram@rentalinrome.com">cynthia.wolfram@rentalinrome.com</a>
			</div>
                -->
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/federicomariani.jpg" alt="Federico Mariani" />
				<span class="nomeStaff">Federico Mariani</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Photo / Video Operator
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:federico.mariani@rentalinrome.com">federico.mariani@rentalinrome.com</a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/ivan-migoni.jpg" alt="Ivan Migoni" />
				<span class="nomeStaff">Ivan Migoni</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Webmaster
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:ivan.migoni@rentalinrome.com">ivan.migoni@rentalinrome.com</a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-dut.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ger.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/isaac-van-aggelen.jpg" alt="Isaac van Aggelen" />
				<span class="nomeStaff">Isaac van Aggelen</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Sales Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:isaac.vanaggelen@rentalinrome.com">isaac.vanaggelen@rentalinrome.com</a>
			</div>
            <!--
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/alessandro-moreschini.jpg" alt="Alessandro Moreschini" />
				<span class="nomeStaff">Alessandro Moreschini</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Photo / Video Operator
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:alex.moreschini@rentalinrome.com">alex.moreschini@rentalinrome.com</a>
			</div>
            -->
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ger.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/francesca-tarantola.jpg" alt="Francesca Tarantola" />
				<span class="nomeStaff">Francesca Tarantola</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Account Manager
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:francesca.tarantola@rentalinrome.com">francesca.tarantola@rentalinrome.com </a>
			</div>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/davide-pitzalis.jpg" alt="Davide Pitzalis" />
				<span class="nomeStaff">Davide Pitzalis</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Customer Services Manager
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:davide.pitzalis@rentalinrome.com">davide.pitzalis@rentalinrome.com</a>
			</div>
			
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/matteo-calabro.jpg" alt="Matteo Calabrò" />
				<span class="nomeStaff">Matteo Calabrò</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Account Manager
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:matteo.calabro@rentalinrome.com">matteo.calabro@rentalinrome.com</a>
			</div>
            <%--
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-fra.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/romina-venturini.jpg" alt="Romina Venturini" />
				<span class="nomeStaff">Romina Venturini</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Transport and Tours Services Account
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:pickupservice@rentalinrome.com">pickupservice@rentalinrome.com</a>
			</div>
    --%>
			<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-fra.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/paolo-calabro.jpg" alt="Paolo Calabrò" />
				<span class="nomeStaff">Paolo Calabrò</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							Marketing Manager
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:paolo.calabro@rentalinrome.com">paolo.calabro@rentalinrome.com</a>
			</div>
			<!--<div class="staff">
				<div class="langStaff">
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
					<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
				</div>
				<img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/vicki-ciampa.jpg" alt="Vicki Ciampa" />
				<span class="nomeStaff">Vicki Ciampa</span>
				<table class="jobStaff" cellpadding="0" cellspacing="0">
					<tr>
						<td align="center" valign="middle">
							New York Representative
						</td>
					</tr>
				</table>
				<a class="mailStaff" href="mailto:ciampavicki@yahoo.com">ciampavicki@yahoo.com</a>
			</div>-->
			
			<!-- STAFF NUOVI DAL 13/07/2011 -->
			
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-bra.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/alessandro-d-arcadia.jpg" alt="Alessandro D'Arcadia" />
                <span class="nomeStaff">Alessandro D'Arcadia</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Welcoming Staff Manager
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:alessandro.darcadia@rentalinrome.com">alessandro.darcadia@rentalinrome.com</a>
            </div>
            
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/alessandro-pancaldi.jpg" alt="Alessandro Pancaldi" />
                <span class="nomeStaff">Alessandro Pancaldi</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Apartment Representative
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:alessandro.pancaldi@rentalinrome.com">alessandro.pancaldi@rentalinrome.com</a>
            </div>
            
            
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/fabio-cortese.jpg" alt="Fabio Cortese" />
                <span class="nomeStaff">Fabio Cortese</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            <%=CurrentSource.getSysLangValue("lbl_OrganizzazioneSviluppo")%>
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:fabio.cortese@rentalinrome.com">fabio.cortese@rentalinrome.com</a>
            </div>
            <!--
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/siviglia-calabro.jpg" alt="Siviglia Calabrò" />
                <span class="nomeStaff">Siviglia Calabrò</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Administration & Maintenance Coordinator
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:siviglia.calabro@rentalinrome.com">siviglia.calabro@rentalinrome.com</a>
            </div>
            -->
            
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-rus.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/victoria-shapovalova.jpg" alt="Victoria Shapovalova" />
                <span class="nomeStaff">Victoria Shapovalova</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Sales Account
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:victoria.shapovalova@rentalinrome.com">victoria.shapovalova@rentalinrome.com</a>
            </div>
            
            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-usa.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/giulia-calabro.jpg" alt="Giulia Calabrò" />
                <span class="nomeStaff">Giulia Calabrò</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Sales Account
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:giulia.calabro@rentalinrome.com">giulia.calabro@rentalinrome.com</a>
            </div>
			
            <!-- FINE STAFF NUOVI DAL 13/07/2011 -->

            

            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/antonio-dia.jpg" alt="Antonio Dia" />
                <span class="nomeStaff">Antonio Dia</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Maintenance
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:manutentori@rentalinrome.com">manutentori@rentalinrome.com</a>
            </div>

            <!--<div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" />
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/giada-d-aleo.jpg" alt="Giada D'aleo" />
                <span class="nomeStaff">Giada D'aleo</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            Sales Account
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:giada.daleo@rentalinrome.com">giada.daleo@rentalinrome.com</a>
            </div>

            <div class="staff">
                <div class="langStaff">
                    <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-ita.gif" alt="" /> 
                     <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-eng.gif" alt="" /> 
                     <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/flag-spa.gif" alt="" />
                </div>
                <img src="<%=CurrentAppSettings.ROOT_PATH %>images/staff/yefrey-guerrero.jpg" alt="Yefrey Guerrero" />
                <span class="nomeStaff">Yefrey Guerrero</span>
                <table class="jobStaff" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                           Sales Account
                        </td>
                    </tr>
                </table>
                <a class="mailStaff" href="mailto:yefrey.guerrero@rentalinrome.com">yefrey.guerrero@rentalinrome.com</a>
            </div>-->


			
			<div class="nulla">
			</div>
		</div>
		<div class="nulla">
		</div>
	</div>
</div>
<div class="bigBox" id="expositions">
	<div>
		<h3>
			<%= CurrentSource.getSysLangValue("lblOurExpositions")%>
			</h3>
		<div class="nulla">
		</div>
		<div id="expoCont">
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Bit 2007 (International Tourism Exchange) </span>
					<span class="dataExpo">Fieramilano from February 22 to 25, 2007</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/1a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/1b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Globe 2007 (Travel Exhibition in Rome) </span>
					<span class="dataExpo">Fiera di Roma from March 22 to 24, 2007</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/2a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/2b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">TTI 2007 (Travel Trade Italia) </span>
					<span class="dataExpo">Fiera di Rimini from October 12 to 14, 2007</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/3a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/3b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Globe 2008 (Travel Exhibition in Rome)</span>
					<span class="dataExpo">Fiera di Roma from March 13 to 15, 2008</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/4a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/4b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">TTI 2008 (Travel Trade Italia) </span>
					<span class="dataExpo">Fiera di Rimini from October 24 to 25, 2008</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/5a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/5b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">New York Times Travel Show </span>
					<span class="dataExpo">New York from February 6 to 8, 2009</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/6a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/6b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Globe 2009 (Travel Exhibition in Rome) </span>
					<span class="dataExpo">Fiera di Roma from March 26 to 28, 2009</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/7a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/7b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Italië het evenement </span>
					<span class="dataExpo">Utrecht from May 15 to 17, 2009</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/8a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/8b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
			<div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">Bit 2011 (International Tourism Exchange) </span>
					<span class="dataExpo">Fieramilano from February 17 to 20, 2011</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/9a.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/9b.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>

            <div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">MITT 2012</span>
					<span class="dataExpo">Moscow, from March 21 to 24 2012</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/rental-in-rome-mitt-2012-1.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/rental-in-rome-mitt-2012-2.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
            <div class="expo">
				<h6 class="expoName">
					<span class="nameExpo">TTI 2014 (Travel Trade Italia)</span>
					<span class="dataExpo">Rimini, from October 8 to 11 2014</span>
				</h6>
				<ul class="listExpo">
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/10b.jpg" width="261" height="174" alt="" />
					</li>
					<li>
						<img src="<%=CurrentAppSettings.ROOT_PATH %>images/expo/10a.jpg" width="261" height="174" alt="" />
					</li>
				</ul>
			</div>
		</div>
		<div class="nulla">
		</div>
	</div>
</div>
<div style="float:left;">
	<uc2:UC_static_block ID="UC_static_block1" runat="server" BlockID="4" />
	
	<a class="bannerino" href="<%=CurrentSource.getPagePath("11", "stp", CurrentLang.ID.ToString()) %>" target="_blank">
		<span>
			<%= CurrentSource.getSysLangValue("lblGuestbook")%>
		</span>
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/guestbook-aboutus.jpg" width="117" height="77" alt="<%= CurrentSource.getSysLangValue("lblGuestbook")%>" />
	</a>
	<a class="bannerino" href="http://www.romeasyoffice.com/" target="_blank">
		<span><%= CurrentSource.getSysLangValue("lblOfficeInRome")%></span>
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/officeinrome.jpg" width="117" height="77" alt="<%= CurrentSource.getSysLangValue("lblOfficeInRome")%>" />
	</a>
	<a class="bannerino" href="<%=CurrentSource.getPagePath("15", "stp", CurrentLang.ID.ToString()) %>">
		<span>
			<%= CurrentSource.getSysLangValue("lblRentAnElectricCar")%>
		</span>
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/electric_car.jpg" width="117" height="77" alt="<%= CurrentSource.getSysLangValue("lblRentAnElectricCar")%>" />
	</a>
	
	<div class="nulla">
	</div>
</div>

<uc1:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />

</asp:Content>
