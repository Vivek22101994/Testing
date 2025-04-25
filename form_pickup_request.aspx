<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="form_pickup_request.aspx.cs" Inherits="RentalInRome.form_pickup_request" %>

<%@ Register Src="~/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/req-style.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/req-theme.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/common.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/shadowbox.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/css/jscal2.css" type="text/css" rel="stylesheet" />
	<link href="jquery/css/ui-timepicker-custom.css" rel="stylesheet" type="text/css" rel="stylesheet" />

	<script type="text/javascript" src="jquery/js/jquery-1.4.4.min.js"></script>
    <script src="/jquery/js/ui--core.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-effects.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-widget.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-position.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-autocomplete.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-datepicker.min.js" type="text/javascript"></script>
    <script src="/jquery/datepicker-lang/jquery.ui.datepicker-<%= CurrentLang.JS_CAL_FILE %>" type="text/javascript"></script>
    <script type="text/javascript" src="jquery/js/ui-timepicker-custom.js"></script>

	<link href="<%=CurrentAppSettings.ROOT_PATH %>jquery/css/jquery-ui-1.8.7.custom.css" type="text/css" rel="stylesheet" />

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/shadowbox.js" type="text/javascript"></script>
	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/players/shadowbox-iframe.js" type="text/javascript"></script>
	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/players/shadowbox-html.js" type="text/javascript"></script>
	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/js/jscal2.js" type="text/javascript"></script>
	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/js/lang/<%=CurrentLang.JS_CAL_FILE %>" type="text/javascript"></script>
	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/JSCal_utils.js" type="text/javascript"></script>

	<script type="text/javascript">
	var items = [
			<%=ltr_items.Text %>
		];
		
		
		function setAutocomplete(){
			$( ".aptComplete" ).autocomplete({
				source: items
			});
		}
		$(function() {
        $("#<% =txt_email.ClientID %>,#txt_email_conf").bind("cut copy paste", function(event) {
                event.preventDefault();
            });
        });
	</script>

</head>
<body>
	<div id="container">
		<h1 id="logo">
		<a>Wufoo</a>
		</h1>
		<form id="form1" runat="server" name="form14" class="wufoo leftLabel page1">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
			<ProgressTemplate>
				<uc3:UC_loader ID="UC_loader1" runat="server" />
			</ProgressTemplate>
		</asp:UpdateProgress>
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<asp:Literal ID="ltr_items" runat="server" Visible="false"></asp:Literal>
				<div id="header" class="info">
					<h2>
						Limo Service Reservation (EN) | Rental in Rome</h2>
					<div>
						(English)</div>
				</div>
				<ul id="pnl_request" runat="server">
					<li id="errorLi" style="display: none;">
						<h3 id="errorMsgLbl">
							<%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
						<p id="errorMsg">
							<%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
						</p>
					</li>
					<li id="txt_name_full_cont" class="     ">
						<label class="desc" id="Label1" for="txt_name_first">
							<%=CurrentSource.getSysLangValue("reqFullName")%>
							<span id="req_21" class="req">*</span>
						</label>
						<div>
							<input id="txt_name_full" runat="server" name="Field_email" class="field text large" value="" maxlength="255" tabindex="1" type="text" />
						</div>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
						<p class="instruct" id="P1">
							<small>e.g. John Brown</small></p>
					</li>
					<li id="txt_email_cont" class="">
						<label class="desc" id="titleemail" for="txt_email">
							<%=CurrentSource.getSysLangValue("reqEmail")%>
							<span id="req__email" class="req">*</span>
						</label>
						<div>
							<input id="txt_email" runat="server" name="Field_email" class="field text large" value="" maxlength="255" tabindex="1" type="text" />
						</div>
						<p class="error" id="rfv_txt_email" style="display: none;">
							<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
						<p class="error" id="rev_txt_email" style="display: none;">
							<%=CurrentSource.getSysLangValue("reqInvalidEmailFormat")%></p>
						<p class="instruct" id="instruct19">
							<small>e.g. john@gmail.com</small></p>
					</li>
					<li id="txt_email_conf_cont" class="">
						<label class="desc" id="Label14" for="txt_email">
							<%=CurrentSource.getSysLangValue("reqEmailConfirm")%>
							<span id="Span2" class="req">*</span>
						</label>
						<div>
							<input id="txt_email_conf" name="Field_email" class="field text large" value="" maxlength="255" tabindex="1" type="text" />
						</div>
						<p class="error" style="">
							<%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></p>
					</li>
					<li id="txt_phone_cont" class="     ">
						<label class="desc" id="Label2" for="txt_phone">
							<%=CurrentSource.getSysLangValue("reqPhoneNumber")%>
							<span id="req_23" class="req">*</span>
						</label>
						<div>
							<input id="txt_phone" runat="server" class="field text large" name="Field23" tabindex="4" maxlength="255" value="" type="text" />
						</div>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
						<p class="instruct" id="P2">
							<small>e.g.
								<%=CurrentSource.getSysLangValue("reqPhoneExample")%></small></p>
					</li>
					<li id="fo14li156" class="">
						<label class="desc" id="title156" for="Field156">
							Arrival / departure
						</label>
						<div>
							<input id="radioDefault_156" name="Field156" value="" type="hidden">
							<span>
								<asp:DropDownList ID="drp_trip_mode" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
									<asp:ListItem Value="1" Text="Arrival to Rome"></asp:ListItem>
									<asp:ListItem Value="2" Text="Departure from Rome"></asp:ListItem>
									<asp:ListItem Value="3" Text="Arrival to Rome + Departure from Rome"></asp:ListItem>
								</asp:DropDownList>
							</span>
						</div>
						<p class="instruct" id="instruct156">
							<small>e.g. Arrival to Rome</small></p>
					</li>


					<li id="fo14li24" class="mode_in section      ">
					</li>
					<li id="fo14li160" class="mode_in place section      ">
						<h3 id="title160">
							Arrival to Rome:</h3>
					</li>
					<li id="fo14li21" class="mode_in place">
						<label class="desc" id="title21" for="Field21">
							Arrival place
						</label>
						<div>
							<asp:DropDownList ID="drp_in_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="air" Text="Airport"></asp:ListItem>
								<asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
								<asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
								<asp:ListItem Value="other" Text="Other"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="instruct21">
							<small>e.g. Airport</small></p>
					</li>
					<li id="fo14li175" class="mode_in air hide">
						<label class="desc" id="title175" for="Field175">
							Airport name:
						</label>
						<div>
							<asp:DropDownList ID="drp_in_air_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
								<asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="instruct175">
							<small>e.g. (FCO) Fiumicino</small></p>
					</li>
					<li id="fo14li25" class="mode_in air dett hide">
						<label class="desc" id="title25" for="Field25">
							Airline / flight n.
						</label>
						<div>
							<asp:TextBox ID="txt_in_air_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="instruct25">
							<small>e.g. AA 6510</small></p>
					</li>
					<li id="fo14li177" class="mode_in sea hide">
						<label class="desc" id="title177" for="Field177">
							Seaport name:
						</label>
						<div>
							<asp:DropDownList ID="drp_in_sea_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
								<asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
								<asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="instruct177">
							<small>e.g. Civitavecchia</small></p>
					</li>
					<li id="fo14li26" class="mode_in sea dett hide">
						<label class="desc" id="title26" for="Field26">
							Shipping company:
						</label>
						<div>
							<asp:TextBox ID="txt_in_sea_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="instruct26">
							<small>e.g. Tirrenia</small></p>
					</li>
					<li id="fo14li178" class="mode_in train hide">
						<label class="desc" id="title178" for="Field178">
							Railway stat. name:
						</label>
						<div>
							<asp:DropDownList ID="drp_in_train_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
								<asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
								<asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="instruct178">
							<small>e.g. Roma Termini</small></p>
					</li>
					<li id="fo14li29" class="mode_in train dett hide">
						<label class="desc" id="title29" for="Field29">
							Train n.
						</label>
						<div>
							<asp:TextBox ID="txt_in_train_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="instruct29">
							<small>e.g. 9333</small></p>
					</li>
					<li id="fo14li32" class="mode_in other hide">
						<label class="desc" id="title32" for="Field32">
							Other place
						</label>
						<div>
							<asp:TextBox ID="txt_in_other_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
					</li>
					<li id="fo14li23" class="mode_in dest">
						<label class="desc" id="title23" for="Field23">
							Departure city
						</label>
						<div>
							<asp:TextBox ID="txt_in_from_place" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="instruct23">
							<small>e.g. London, UK</small></p>
					</li>
					<li id="HF_in_date_cont" class="mode_in date">
						<label class="desc" id="title137" for="Field137">
							Arrival date
						</label>
						<div>
							<input type="text" id="cal_in_date" style="width: 120px" readonly="readonly" />
							<img id="cal_in_date_trigger" class="datepicker" src="images/icons/calendar.png" alt="Pick a date." />
							<asp:HiddenField ID="HF_in_date" runat="server" />
						</div>
                        <p class="error">
                            <%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
                        <p class="instruct" id="instruct137">
							<small>e.g. 20 Febbraio 2011</small></p>
					</li>
					<li id="fo14li9" class="mode_in time      ">
						<label class="desc" id="title9" for="Field9">
							Arrival time
						</label>
						<span id="in_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span> 
						<img id="in_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
						<asp:HiddenField ID="HF_in_time_cont" runat="server" />
						<script type="text/javascript">
							var in_time = new timePicker({ timeCont: '<%= HF_in_time_cont.ClientID %>', timeView: 'in_time_view', timeToggler: 'in_time_toggler', viewAsToggler: true });
						</script>
						<p class="instruct" id="instruct9">
							<small>e.g. 10:00 AM</small></p>
					</li>
					<li id="Li16" class="mode_in pick">
						<label class="desc" id="Label15" for="Field165">
							Destination Apartment
						</label>
						<div>
							<input id="txt_dest_apt" name="Field165" class="aptComplete field text large" value="" maxlength="255" tabindex="31" type="text" runat="server">
						</div>
						<p class="instruct" id="P13">
							<small>e.g. Sole apartment</small></p>
					</li>
					<li id="Li17" class="mode_in pick">
						<label class="desc" id="Label16" for="Field36">
							or other address
						</label>
						<div>
							<input id="txt_dest_other" name="Field36" class="field text large" value="" maxlength="255" tabindex="32" type="text" runat="server">
						</div>
						<p class="instruct" id="P14">
							<small>e.g. Via del Mattonato</small></p>
					</li>
					<li id="Li1" class="mode_out section      "></li>
					<li id="Li2" class="mode_out place section      ">
						<h3 id="H1">
							Departure from Rome:</h3>
					</li>
					<li id="Li3" class="mode_out place">
						<label class="desc" id="Label3" for="Field21">
							Departure place
						</label>
						<div>
							<asp:DropDownList ID="drp_out_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="air" Text="Airport"></asp:ListItem>
								<asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
								<asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
								<asp:ListItem Value="other" Text="Other"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="P3">
							<small>e.g. Airport</small></p>
					</li>
					<li id="Li4" class="mode_out air hide">
						<label class="desc" id="Label4" for="Field175">
							Airport name:
						</label>
						<div>
							<asp:DropDownList ID="drp_out_air_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="(FCO) Fiumicino airport" Text="(FCO) Fiumicino airport"></asp:ListItem>
								<asp:ListItem Value="(CPO) Ciampino airport" Text="(CPO) Ciampino airport"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="P4">
							<small>e.g. (FCO) Fiumicino</small></p>
					</li>
					<li id="Li5" class="mode_out air dett hide">
						<label class="desc" id="Label5" for="Field25">
							Airline / flight n.
						</label>
						<div>
							<asp:TextBox ID="txt_out_air_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="P5">
							<small>e.g. AA 6510</small></p>
					</li>
					<li id="Li6" class="mode_out sea hide">
						<label class="desc" id="Label6" for="Field177">
							Seaport name:
						</label>
						<div>
							<asp:DropDownList ID="drp_out_sea_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="Civitavecchia seaport" Text="Civitavecchia seaport"></asp:ListItem>
								<asp:ListItem Value="Fiumicino seaport" Text="Fiumicino seaport"></asp:ListItem>
								<asp:ListItem Value="Gaeta seaport" Text="Gaeta seaport"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="P6">
							<small>e.g. Civitavecchia</small></p>
					</li>
					<li id="Li7" class="mode_out sea dett hide">
						<label class="desc" id="Label7" for="Field26">
							Shipping company:
						</label>
						<div>
							<asp:TextBox ID="txt_out_sea_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="P7">
							<small>e.g. Tirrenia</small></p>
					</li>
					<li id="Li8" class="mode_out train hide">
						<label class="desc" id="Label8" for="Field178">
							Railway stat. name:
						</label>
						<div>
							<asp:DropDownList ID="drp_out_train_place" runat="server" CssClass="field select large" onchange="checkArrivalDeparture()">
								<asp:ListItem Value="" Text=""></asp:ListItem>
								<asp:ListItem Value="Roma Termini railway station" Text="Roma Termini railway station"></asp:ListItem>
								<asp:ListItem Value="Roma Tiburtina railway station" Text="Roma Tiburtina railway station"></asp:ListItem>
								<asp:ListItem Value="Roma Ostiense railway station" Text="Roma Ostiense railway station"></asp:ListItem>
							</asp:DropDownList>
						</div>
						<p class="instruct" id="P8">
							<small>e.g. Roma Termini</small></p>
					</li>
					<li id="Li9" class="mode_out train dett hide">
						<label class="desc" id="Label9" for="Field29">
							Train n.
						</label>
						<div>
							<asp:TextBox ID="txt_out_train_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="P9">
							<small>e.g. 9333</small></p>
					</li>
					<li id="Li10" class="mode_out other hide">
						<label class="desc" id="Label10" for="Field32">
							Other place
						</label>
						<div>
							<asp:TextBox ID="txt_out_other_dett" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
					</li>
					<li id="Li11" class="mode_out dest">
						<label class="desc" id="Label11" for="Field23">
							Destination city
						</label>
						<div>
							<asp:TextBox ID="txt_out_from_place" runat="server" CssClass="field text large"></asp:TextBox>
						</div>
						<p class="instruct" id="P10">
							<small>e.g. London, UK</small></p>
					</li>
					<li id="HF_out_date_cont" class="mode_out date      ">
						<label class="desc" id="Label12" for="Field137">
							Departure date
						</label>
						<div>
							<input type="text" id="cal_out_date" style="width: 120px" readonly="readonly" />
							<img id="cal_out_date_trigger" class="datepicker" src="images/icons/calendar.png" alt="Pick a date." />
							<asp:HiddenField ID="HF_out_date" runat="server" />
						</div>
                        <p class="error">
                            <%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
                        <p class="instruct" id="P11">
							<small>e.g. 20 Febbraio 2011</small></p>
					</li>
					<li id="Li13" class="mode_out time      ">
						<label class="desc" id="Label13" for="Field9">
							Departure time
						</label>
						<span id="out_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
						<img id="out_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
						<asp:HiddenField ID="HF_out_time_cont" runat="server" />

						<script type="text/javascript">
							var out_time = new timePicker({ timeCont: '<%= HF_out_time_cont.ClientID %>', timeView: 'out_time_view', timeToggler: 'out_time_toggler', viewAsToggler: true });
						</script>

						<p class="instruct" id="P12">
							<small>e.g. 10:00 AM</small></p>
					</li>
					<li id="Li18" class="mode_out pick">
						<label class="desc" id="Label17" for="Field165">
							Pickup Apartment
						</label>
						<div>
							<input id="txt_pick_apt" name="Field165" class="aptComplete field text large" value="" maxlength="255" tabindex="31" type="text" runat="server">
						</div>
						<p class="instruct" id="P15">
							<small>e.g. Sole apartment</small></p>
					</li>
					<li id="Li19" class="mode_out pick">
						<label class="desc" id="Label18" for="Field36">
							or other address
						</label>
						<div>
							<input id="txt_pick_other" name="Field36" class="field text large" value="" maxlength="255" tabindex="32" type="text" runat="server">
						</div>
						<p class="instruct" id="P16">
							<small>e.g. Via del Mattonato</small></p>
					</li>
					<li id="Li14" class="section"></li>
					<li id="Li15" class="section">
						<h3></h3>
					</li>
					<li id="fo14li11" class="      ">
						<label class="desc" id="title11" for="Field11">
							Adults
						</label>
						<div>
							<select id="drp_num_adult" runat="server" class="field select small" tabindex="17">
								<option value="1">1 </option>
								<option value="2" selected="selected">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct11">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo14li12" class="      ">
						<label class="desc" id="title12" for="Field12">
							Children
						</label>
						<div>
							<select id="drp_num_child" runat="server" class="field select small" tabindex="18">
								<option value="0" selected="selected"></option>
								<option value="1">1 </option>
								<option value="2">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct12">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo14li154" class="section      "></li>
					<li id="fo14li17" class="      ">
						<label class="desc" id="title17" for="Field17">
							Small suitecases
						</label>
						<div>
							<select id="drp_num_case_s" runat="server" class="field select small" tabindex="19">
								<option value="0"></option>
								<option value="1">1 </option>
								<option value="2" selected="selected">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct17">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo14li18" class="      ">
						<label class="desc" id="title18" for="Field18">
							Medium suitecases
						</label>
						<div>
							<select id="drp_num_case_m" runat="server" class="field select small" tabindex="20">
								<option value="0"></option>
								<option value="1" selected="selected">1 </option>
								<option value="2">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct18">
							<small>e.g. 1</small></p>
					</li>
					<li id="fo14li16" class="      ">
						<label class="desc" id="title16" for="Field16">
							Large suitecases
						</label>
						<div>
							<select id="drp_num_case_l" runat="server" class="field select small" tabindex="21">
								<option value="0" selected="selected"></option>
								<option value="1">1 </option>
								<option value="2">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct16">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo14li13" class="hide">
						<label class="desc" id="title13" for="Field13">
							Meeting point
						</label>
						<div>
							<select id="Field13" name="Field13" class="field select large" onclick="handleInput(this);" onkeyup="handleInput(this);" tabindex="7">
								<option value="Your arrival terminal" selected="selected">Your arrival terminal </option>
								<option value="Terminal arrival meeting point column">Terminal arrival meeting point column </option>
								<option value="Civitavecchia port">Civitavecchia port </option>
								<option value="Track #24 (railway station)">Track #24 (railway station) </option>
							</select>
						</div>
						<p class="instruct" id="instruct13">
							<small>e.g. Terminal</small></p>
					</li>
					<li id="fo14li186" class="hide">
						<label class="desc" id="title186" for="Field186">
							Meeting point
						</label>
						<div>
							<select id="Field186" name="Field186" class="field select large" onclick="handleInput(this);" onkeyup="handleInput(this);" tabindex="8">
								<option value="In front of the apartment" selected="selected">In front of the apartment </option>
								<option value="Other">Other </option>
							</select>
						</div>
						<p class="instruct" id="instruct186">
							<small>e.g. In front of the apt.</small></p>
					</li>
					<li id="fo14li187" class="hide">
						<label class="desc" id="title187" for="Field187">
							Other meeting point
						</label>
						<div>
							<input id="Field187" name="Field187" class="field text large" value="" maxlength="255" tabindex="9" onkeyup="handleInput(this); " onchange="handleInput(this);" type="text">
						</div>
					</li>
					<li id="fo14li150" class="section      ">
						<h3 id="title150">
							Options</h3>
					</li>
					<li id="fo1li143" class="     ">
						<label class="desc" id="title143" for="Field143">
							<%=CurrentSource.getSysLangValue("reqSpecialRequest")%>
						</label>
						<div>
							<textarea id="txt_note" runat="server" name="Field143" class="field textarea small" rows="10" cols="50" tabindex="27"></textarea>
						</div>
					</li>
					<li id="fo1li147" class="section      ">
						<h3 id="title147">
							<%=CurrentSource.getSysLangValue("reqTermsOfService")%></h3>
						<div id="instruct147">
							<%=CurrentSource.getSysLangValue("reqTermsOfServiceContent")%>
						</div>
					</li>
					<li id="chk_terms_cont">
						<label class="desc" id="title351" for="Field351">
							<span id="req_351" class="req">*</span>
						</label>
						<div>
							<span>
								<input id="chk_terms" class="field checkbox" value="I accept." tabindex="28" checked="checked" type="checkbox">
								<label class="choice" for="Field351">
									<%=CurrentSource.getSysLangValue("reqAccept")%></label>
							</span>
						</div>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></p>
					</li>
					<li class="buttons ">
						<div>
							<asp:LinkButton ID="lnkSend" CssClass="tastoBianco" runat="server" OnClientClick="return sp_validateForm()" OnClick="lnk_send_Click"></asp:LinkButton>
						</div>
					</li>
				</ul>
				<div id="pnl_request_sent" runat="server" visible="false">
					<%=CurrentSource.getSysLangValue("reqRequestSent")%>
				</div>

				<script type="text/javascript">
					function checkArrivalDeparture() {
						var _modeInt = $("#<%=drp_trip_mode.ClientID %>").val();
						$(".mode_out").addClass("hide");
						$(".mode_in").addClass("hide");
						if (_modeInt == "1") {
							setMode("mode_in");
						}
						else if (_modeInt == "2") {
							setMode("mode_out");
						}
						else {
							setMode("mode_in");
							setMode("mode_out");
						}

						function setMode(_mode) {
							if (_mode == "") return;
							var _place = "";
							var _place_select = "";
							$("." + _mode + ".pick").removeClass("hide");
							$("." + _mode + ".dest").removeClass("hide");
							$("." + _mode + ".date").removeClass("hide");
							$("." + _mode + ".time").removeClass("hide");
							$("." + _mode + ".place").removeClass("hide");
							_place = _mode == "mode_in" ? $("#<%=drp_in_place.ClientID %>").val() : $("#<%=drp_out_place.ClientID %>").val();

							if (_place != "")
								$("." + _mode + "." + _place).removeClass("hide");
							if (_mode == "mode_in") {
								if (_place == "air")
									_place_select = "#<%=drp_in_air_place.ClientID %>";
								if (_place == "sea")
									_place_select = "#<%=drp_in_sea_place.ClientID %>";
								if (_place == "train")
									_place_select = "#<%=drp_in_train_place.ClientID %>";
							}
							else {
								if (_place == "air")
									_place_select = "#<%=drp_out_air_place.ClientID %>";
								if (_place == "sea")
									_place_select = "#<%=drp_out_sea_place.ClientID %>";
								if (_place == "train")
									_place_select = "#<%=drp_out_train_place.ClientID %>";
							}

							if ($(_place_select).val() == "" || _place == "")
								$("." + _mode + "." + _place + ".dett").addClass("hide");
							else
								$("." + _mode + "." + _place + ".dett").removeClass("hide");
						}

					}
					checkArrivalDeparture();
				</script>

				<script type="text/javascript">
					function sp_validateForm() {
						var _validation = true;
						var _focused = false;
						$get("txt_email_cont").className = "";
						if ($get("txt_email").value == "") {
							$get("txt_email_cont").className = "error";
							$get("rfv_txt_email").style.display = "";
							$get("rev_txt_email").style.display = "none";
							_validation = false;
						}
						else if (!$get("txt_email").value.match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/)) {
							$get("rfv_txt_email").style.display = "none";
							$get("rev_txt_email").style.display = "";
							$get("txt_email_cont").className = "error";
							_validation = false;
						}
						$get("txt_email_conf_cont").className = "";
						if ($get("txt_email_conf").value != $get("txt_email").value) {
							$get("txt_email_conf_cont").className = "error";
							_validation = false;
						}
						$get("txt_name_full_cont").className = "";
						if ($get("txt_name_full").value == "") {
							$get("txt_name_full_cont").className = "error";
							_validation = false;
						}
						$get("txt_phone_cont").className = "";
						if ($get("txt_phone").value == "") {
							$get("txt_phone_cont").className = "error";
							_validation = false;
			            }
			            $("#HF_in_date_cont").removeClass("error");
			            if ( ($get("drp_trip_mode").value == "1" || $get("drp_trip_mode").value == "3")&&$get("HF_in_date").value == "") {
			                $("#HF_in_date_cont").addClass("error");
			                _validation = false;
			            }
			            $("#HF_out_date_cont").removeClass("error");
			            if ( ($get("drp_trip_mode").value == "2" || $get("drp_trip_mode").value == "3")&&$get("HF_out_date").value == "") {
			                $("#HF_out_date_cont").addClass("error");
			                _validation = false;
			            }
			            $get("chk_terms_cont").className = "";
						if (!$get('chk_terms').checked) {
							$get("chk_terms_cont").className = "error";
							_validation = false;
						}
						//_validation = false;
						if (_validation) {
							$get("errorLi").style.display = "none";
						} else {
							$get("errorLi").style.display = "block";
							window.location = "#errorLi";
						}
						return _validation;
					}
					var cal_in_date = new JSCal_single('cal_in_date', '<%= HF_in_date.ClientID %>', 'cal_in_date_trigger', null, '', 'del_in_date');
					var cal_out_date = new JSCal_single('cal_out_date', '<%= HF_out_date.ClientID %>', 'cal_out_date_trigger', null, '', 'del_out_date');
                </script>

			</ContentTemplate>
		</asp:UpdatePanel>
		</form>
	</div>
	<img id="bottom" src="/images/bottom.png" alt="">
</body>
</html>
