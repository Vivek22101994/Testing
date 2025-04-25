<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="form_request_flight_date.aspx.cs" Inherits="RentalInRome.form_request_flight_date" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/req-style.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/req-theme.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>css/common.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/shadowbox.css" rel="stylesheet" type="text/css" />
	<link href="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/css/jscal2.css" type="text/css" rel="stylesheet" />


	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/req-dynamic.js" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/shadowbox.js" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/players/shadowbox-iframe.js" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/shadowbox/players/shadowbox-html.js" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/js/jscal2.js" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/jscalendar/js/lang/<%=CurrentLang.JS_CAL_FILE %>" type="text/javascript"></script>

	<script src="<%=CurrentAppSettings.ROOT_PATH %>js/JSCal_utils.js" type="text/javascript"></script>
	<style type="text/css">
		#price_range input, #price_range label, #services input, #services label {clear:none;display:inline;}
	</style>
</head>
<body>
	<div id="container">
		<h1 id="logo">
		<a>Wufoo</a>
		</h1>
		<form id="form1" class="wufoo leftLabel page1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<div id="header" class="info">
			<h2>
				<%=CurrentSource.getSysLangValue("reqHeadTitle")%></h2>
			<div>
				(<%= CurrentLang.TITLE%>)</div>
		</div>
		<ul id="pnl_request" runat="server">
			<li id="errorLi" style="display:none;">
				<h3 id="errorMsgLbl">
					<%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
				<p id="errorMsg">
					<%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
					
				</p>
			</li>
			<li id="txt_email_cont" class="">
				<label class="desc" id="titleemail" for="txt_email">
					Email
					<span id="req__email" class="req">*</span>
				</label>
				<div>
					<input id="txt_email" runat="server" name="Field_email" class="field text large" value="" maxlength="255" tabindex="1" onkeyup="handleInput(this);" onchange="handleInput(this);" type="text" />
				</div>
				<p class="error" id="rfv_txt_email" style=" display:none;">
					<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
				<p class="error" id="rev_txt_email" style="display: none;">
					<%=CurrentSource.getSysLangValue("reqInvalidEmailFormat")%></p>
				<p class="instruct" id="instruct19">
					<small>e.g. john@gmail.com</small></p>
			</li>
			<li id="txt_name_first_cont" class="     ">
				<label class="desc" id="title21" for="txt_name_first">
					<%=CurrentSource.getSysLangValue("reqFullName")%>
					<span id="req_21" class="req">*</span>
				</label>
				<span>
					<input id="txt_name_first" runat="server" name="Field21" class="field text fn" value="" size="8" tabindex="2" onkeyup="handleInput(this);" onchange="handleInput(this);" type="text" />
					<label for="Field21">
						<%=CurrentSource.getSysLangValue("reqFirstName")%></label>
				</span>
				<span>
					<input id="txt_name_last" runat="server" name="Field22" class="field text ln" value="" size="14" tabindex="3" onkeyup="handleInput(this);" onchange="handleInput(this);" type="text" />
					<label for="Field22">
						<%=CurrentSource.getSysLangValue("reqLastName")%></label>
				</span>
				<p class="error">
					<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
				<p class="instruct" id="instruct21">
					<small>e.g. John Brown</small></p>
			</li>
			<li id="txt_phone_cont" class="     ">
				<label class="desc" id="title23" for="txt_phone">
					<%=CurrentSource.getSysLangValue("reqPhoneNumber")%>
					<span id="req_23" class="req">*</span>
				</label>
				<div>
					<input id="txt_phone" runat="server" class="field text medium" name="Field23" tabindex="4" onkeyup="handleInput(this);" onchange="handleInput(this);" maxlength="255" value="" type="text" />
				</div>
				<p class="error">
					<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
				<p class="instruct" id="instruct23">
					<small>e.g. <%=CurrentSource.getSysLangValue("reqPhoneExample")%></small></p>
			</li>
			<li id="Li1" class="     ">
				<label class="desc" id="Label1" for="txt_aiport_in">
					<%=CurrentSource.getSysLangValue("reqAirport")%>					
				</label>
				<div>
					<input id="txt_aiport_in" runat="server" class="field text medium" name="Field23" tabindex="4" onkeyup="handleInput(this);" onchange="handleInput(this);" maxlength="255" value="" type="text" />
				</div>
				<p class="instruct" id="P1">
					<small>e.g. <%=CurrentSource.getSysLangValue("reqAirportExample")%></small></p>
			</li>
			<li id="fo1li137" class="date      ">
				<label class="desc" id="title137" for="Field137">
					<%=CurrentSource.getSysLangValue("reqCheckInDate")%>
				</label>
				<div>
					<input type="text" id="cal_date_start" style="width: 120px" readonly="readonly" />
					<img id="cal_date_start_trigger" class="datepicker" src="images/icons/calendar.png" alt="Pick a date."/>
					<asp:HiddenField ID="HF_date_start" runat="server" />
				</div>
				<p class="instruct" id="instruct137">
					<small>e.g. 20 Febbraio 2011</small></p>
				<div>
                 <%=CurrentSource.getSysLangValue("lblHour")%>
				 <asp:DropDownList ID="drp_hour_in" runat="server">
                     <asp:ListItem>0</asp:ListItem>
                     <asp:ListItem>1</asp:ListItem>
                     <asp:ListItem>2</asp:ListItem>
                     <asp:ListItem>3</asp:ListItem>
                     <asp:ListItem>4</asp:ListItem>
                     <asp:ListItem>5</asp:ListItem>
                     <asp:ListItem>6</asp:ListItem>
                     <asp:ListItem>7</asp:ListItem>
                     <asp:ListItem>8</asp:ListItem>
                     <asp:ListItem>9</asp:ListItem>
                     <asp:ListItem>10</asp:ListItem>
                     <asp:ListItem>11</asp:ListItem>
                     <asp:ListItem>12</asp:ListItem>
                     <asp:ListItem>13</asp:ListItem>
                     <asp:ListItem>14</asp:ListItem>
                     <asp:ListItem>15</asp:ListItem>
                     <asp:ListItem>16</asp:ListItem>
                     <asp:ListItem>17</asp:ListItem>
                     <asp:ListItem>18</asp:ListItem>
                     <asp:ListItem>19</asp:ListItem>
                     <asp:ListItem>20</asp:ListItem>
                     <asp:ListItem>21</asp:ListItem>
                     <asp:ListItem>22</asp:ListItem>
                     <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
				</div>					
				<div>
                 <%=CurrentSource.getSysLangValue("lblHour")%>
				 <asp:DropDownList ID="drp_minute_in" runat="server">
                     <asp:ListItem>0</asp:ListItem>
                     <asp:ListItem>1</asp:ListItem>
                     <asp:ListItem>2</asp:ListItem>
                     <asp:ListItem>3</asp:ListItem>
                     <asp:ListItem>4</asp:ListItem>
                     <asp:ListItem>5</asp:ListItem>
                     <asp:ListItem>6</asp:ListItem>
                     <asp:ListItem>7</asp:ListItem>
                     <asp:ListItem>8</asp:ListItem>
                     <asp:ListItem>9</asp:ListItem>
                     <asp:ListItem>10</asp:ListItem>
                     <asp:ListItem>11</asp:ListItem>
                     <asp:ListItem>12</asp:ListItem>
                     <asp:ListItem>13</asp:ListItem>
                     <asp:ListItem>14</asp:ListItem>
                     <asp:ListItem>15</asp:ListItem>
                     <asp:ListItem>16</asp:ListItem>
                     <asp:ListItem>17</asp:ListItem>
                     <asp:ListItem>18</asp:ListItem>
                     <asp:ListItem>19</asp:ListItem>
                     <asp:ListItem>20</asp:ListItem>
                     <asp:ListItem>21</asp:ListItem>
                     <asp:ListItem>22</asp:ListItem>
                     <asp:ListItem>23</asp:ListItem>
                     <asp:ListItem>24</asp:ListItem>
                     <asp:ListItem>25</asp:ListItem>
                     <asp:ListItem>26</asp:ListItem>
                     <asp:ListItem>27</asp:ListItem>
                     <asp:ListItem>28</asp:ListItem>
                     <asp:ListItem>29</asp:ListItem>
                     <asp:ListItem>30</asp:ListItem>
                     <asp:ListItem>31</asp:ListItem>
                     <asp:ListItem>32</asp:ListItem>
                     <asp:ListItem>33</asp:ListItem>
                     <asp:ListItem>34</asp:ListItem>
                     <asp:ListItem>35</asp:ListItem>
                     <asp:ListItem>36</asp:ListItem>
                     <asp:ListItem>37</asp:ListItem>
                     <asp:ListItem>38</asp:ListItem>
                     <asp:ListItem>39</asp:ListItem>
                     <asp:ListItem>40</asp:ListItem>
                     <asp:ListItem>41</asp:ListItem>
                     <asp:ListItem>42</asp:ListItem>
                     <asp:ListItem>43</asp:ListItem>
                     <asp:ListItem>44</asp:ListItem>
                     <asp:ListItem>45</asp:ListItem>
                     <asp:ListItem>46</asp:ListItem>
                     <asp:ListItem>47</asp:ListItem>
                     <asp:ListItem>48</asp:ListItem>
                     <asp:ListItem>49</asp:ListItem>
                     <asp:ListItem>50</asp:ListItem>
                     <asp:ListItem>51</asp:ListItem>
                     <asp:ListItem>52</asp:ListItem>
                     <asp:ListItem>53</asp:ListItem>
                     <asp:ListItem>54</asp:ListItem>
                     <asp:ListItem>55</asp:ListItem>
                     <asp:ListItem>56</asp:ListItem>
                     <asp:ListItem>57</asp:ListItem>
                     <asp:ListItem>58</asp:ListItem>
                     <asp:ListItem>59</asp:ListItem>                     
                </asp:DropDownList>
				</div>					
				
			</li>
			
			<li id="Li2" class="     ">
				<label class="desc" id="Label2" for="txt_aiport_out">
					<%=CurrentSource.getSysLangValue("reqAirport")%>					
				</label>
				<div>
					<input id="txt_airport_out" runat="server" class="field text medium" name="Field23" tabindex="4" onkeyup="handleInput(this);" onchange="handleInput(this);" maxlength="255" value="" type="text" />
				</div>
				<p class="instruct" id="P2">
					<small>e.g. <%=CurrentSource.getSysLangValue("reqAirportExample")%></small></p>
			</li>
			
			<li id="HF_date_end_cont" class="date      ">
				<label class="desc" id="title138" for="Field138">
					<%=CurrentSource.getSysLangValue("reqCheckOutDate")%>
				</label>
				<div>
					<input type="text" id="cal_date_end" style="width: 120px" readonly="readonly" />
					<img id="cal_date_end_trigger" class="datepicker" src="images/icons/calendar.png" alt="Pick a date."/>
					<asp:HiddenField ID="HF_date_end" runat="server" />
				</div>
				<p class="instruct" id="instruct138">
					<small>e.g. 28 Febbraio 2011</small></p>
				<p class="error">
					<%=CurrentSource.getSysLangValue("reqCheckOutError")%></p>
				<div>
                 <%=CurrentSource.getSysLangValue("lblHour")%>
				 <asp:DropDownList ID="drp_hour_out" runat="server">
                     <asp:ListItem>0</asp:ListItem>
                     <asp:ListItem>1</asp:ListItem>
                     <asp:ListItem>2</asp:ListItem>
                     <asp:ListItem>3</asp:ListItem>
                     <asp:ListItem>4</asp:ListItem>
                     <asp:ListItem>5</asp:ListItem>
                     <asp:ListItem>6</asp:ListItem>
                     <asp:ListItem>7</asp:ListItem>
                     <asp:ListItem>8</asp:ListItem>
                     <asp:ListItem>9</asp:ListItem>
                     <asp:ListItem>10</asp:ListItem>
                     <asp:ListItem>11</asp:ListItem>
                     <asp:ListItem>12</asp:ListItem>
                     <asp:ListItem>13</asp:ListItem>
                     <asp:ListItem>14</asp:ListItem>
                     <asp:ListItem>15</asp:ListItem>
                     <asp:ListItem>16</asp:ListItem>
                     <asp:ListItem>17</asp:ListItem>
                     <asp:ListItem>18</asp:ListItem>
                     <asp:ListItem>19</asp:ListItem>
                     <asp:ListItem>20</asp:ListItem>
                     <asp:ListItem>21</asp:ListItem>
                     <asp:ListItem>22</asp:ListItem>
                     <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
				</div>					
				<div>
                 <%=CurrentSource.getSysLangValue("lblHour")%>
				 <asp:DropDownList ID="drp_min_out" runat="server">
                     <asp:ListItem>0</asp:ListItem>
                     <asp:ListItem>1</asp:ListItem>
                     <asp:ListItem>2</asp:ListItem>
                     <asp:ListItem>3</asp:ListItem>
                     <asp:ListItem>4</asp:ListItem>
                     <asp:ListItem>5</asp:ListItem>
                     <asp:ListItem>6</asp:ListItem>
                     <asp:ListItem>7</asp:ListItem>
                     <asp:ListItem>8</asp:ListItem>
                     <asp:ListItem>9</asp:ListItem>
                     <asp:ListItem>10</asp:ListItem>
                     <asp:ListItem>11</asp:ListItem>
                     <asp:ListItem>12</asp:ListItem>
                     <asp:ListItem>13</asp:ListItem>
                     <asp:ListItem>14</asp:ListItem>
                     <asp:ListItem>15</asp:ListItem>
                     <asp:ListItem>16</asp:ListItem>
                     <asp:ListItem>17</asp:ListItem>
                     <asp:ListItem>18</asp:ListItem>
                     <asp:ListItem>19</asp:ListItem>
                     <asp:ListItem>20</asp:ListItem>
                     <asp:ListItem>21</asp:ListItem>
                     <asp:ListItem>22</asp:ListItem>
                     <asp:ListItem>23</asp:ListItem>
                     <asp:ListItem>24</asp:ListItem>
                     <asp:ListItem>25</asp:ListItem>
                     <asp:ListItem>26</asp:ListItem>
                     <asp:ListItem>27</asp:ListItem>
                     <asp:ListItem>28</asp:ListItem>
                     <asp:ListItem>29</asp:ListItem>
                     <asp:ListItem>30</asp:ListItem>
                     <asp:ListItem>31</asp:ListItem>
                     <asp:ListItem>32</asp:ListItem>
                     <asp:ListItem>33</asp:ListItem>
                     <asp:ListItem>34</asp:ListItem>
                     <asp:ListItem>35</asp:ListItem>
                     <asp:ListItem>36</asp:ListItem>
                     <asp:ListItem>37</asp:ListItem>
                     <asp:ListItem>38</asp:ListItem>
                     <asp:ListItem>39</asp:ListItem>
                     <asp:ListItem>40</asp:ListItem>
                     <asp:ListItem>41</asp:ListItem>
                     <asp:ListItem>42</asp:ListItem>
                     <asp:ListItem>43</asp:ListItem>
                     <asp:ListItem>44</asp:ListItem>
                     <asp:ListItem>45</asp:ListItem>
                     <asp:ListItem>46</asp:ListItem>
                     <asp:ListItem>47</asp:ListItem>
                     <asp:ListItem>48</asp:ListItem>
                     <asp:ListItem>49</asp:ListItem>
                     <asp:ListItem>50</asp:ListItem>
                     <asp:ListItem>51</asp:ListItem>
                     <asp:ListItem>52</asp:ListItem>
                     <asp:ListItem>53</asp:ListItem>
                     <asp:ListItem>54</asp:ListItem>
                     <asp:ListItem>55</asp:ListItem>
                     <asp:ListItem>56</asp:ListItem>
                     <asp:ListItem>57</asp:ListItem>
                     <asp:ListItem>58</asp:ListItem>
                     <asp:ListItem>59</asp:ListItem>                     
                </asp:DropDownList>
				</div>					
					
			</li>		
			<li id="fo1li147" class="section      ">
				<h3 id="title147">
					<%=CurrentSource.getSysLangValue("reqTermsOfService")%></h3>
				<div id="instruct147">
					<%=CurrentSource.getSysLangValue("reqTermsOfServiceContent")%>
					</div>
			</li>
			<li id="fo1li351" class="     ">
				<label class="desc" id="title351" for="Field351">
					<span id="req_351" class="req">*</span>
				</label>
				<div>
					<span>
						<input id="Field351" name="Field351" class="field checkbox" value="I accept." tabindex="28" onchange="handleInput(this);" checked="checked" type="checkbox">
						<label class="choice" for="Field351">
							<%=CurrentSource.getSysLangValue("reqAccept")%></label>
					</span>
				</div>
				<p class="error">
					<%=CurrentSource.getSysLangValue("reqRequiredAcceptTermsOfService")%></p>
			</li>
			<li class="buttons ">
				<div>
					<asp:Button ID="Button1" runat="server" CssClass="btTxt submit" Text="Submit" OnClientClick="return sp_validateForm()" OnClick="lnk_send_Click" />
				</div>
			</li>
		</ul>
		<div id="pnl_request_sent" runat="server" visible="false">
			<%=CurrentSource.getSysLangValue("reqRequestSent")%>
			
		</div>
		<script type="text/javascript">
			function sp_validateForm() {
				var _validation = true;
				var _focused = false;
				$get("txt_email_cont").className="";
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
				$get("txt_name_first_cont").className = "";
				if ($get("txt_name_first").value == "" || $get("txt_name_last").value == "") {
					$get("txt_name_first_cont").className = "error";
					_validation = false;
				}
				$get("txt_phone_cont").className = "";
				if ($get("txt_phone").value == "") {
					$get("txt_phone_cont").className = "error";
					_validation = false;
				}				

				$get("HF_date_end_cont").className = "";
				try{
					var _startStr = $get("HF_date_start").value;
					var _endStr = $get("HF_date_end").value;
					var _startInt = parseInt(_startStr);
					var _endInt = parseInt(_endStr);
					if (_startInt > _endInt) {
						$get("HF_date_end_cont").className = "error";
						_validation = false;
					}
				}
				catch(ex){}
				if (!$get('Field351').checked) {
					$get("fo1li351").className = "error";
					_validation = false;
				}
				$get("fo1li351").className = "";
				if (!$get('Field351').checked) {
					$get("fo1li351").className = "error";
					_validation = false;
				}
				if (_validation) {
					$get("errorLi").style.display = "none";
				} else {
				$get("errorLi").style.display = "block";
				window.location = "#errorLi";
				}
				return _validation;
			}
			var cal_date_start_from = new JSCal_single('cal_date_start', '<%= HF_date_start.ClientID %>', 'cal_date_start_trigger', null, '', 'del_date_start');
			var cal_date_end_from = new JSCal_single('cal_date_end', '<%= HF_date_end.ClientID %>', 'cal_date_end_trigger', null, '', 'del_date_end');
		</script>
		</form>
	</div>
	<!--container-->
	<img id="bottom" src="images/bottom.png" alt="">
	<!-- JavaScript -->

	<script type="text/javascript">
		__RULES = [];
		__ENTRY = [];
		__PRICES = null;
	</script>

</body>
</html>
