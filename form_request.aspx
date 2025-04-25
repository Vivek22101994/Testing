<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="form_request.aspx.cs" Inherits="RentalInRome.form_request" %>

<%@ Register Src="~/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
    <style type="text/css">
        @import url(/css/req-style.css);
        @import url(/css/req-theme.css);
        @import url(/css/common.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-datepicker.css);
        @import url(/jquery/css/ui-autocomplete.css);
    </style>
    <script src="<%=CurrentAppSettings.ROOT_PATH %>-js/req-dynamic.js" type="text/javascript"></script>
    <script src="/jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui--core.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-effects.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-widget.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-position.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-autocomplete.min.js" type="text/javascript"></script>
    <script src="/jquery/js/ui-datepicker.min.js" type="text/javascript"></script>
    <script src="/jquery/datepicker-lang/jquery.ui.datepicker-<%= CurrentLang.JS_CAL_FILE %>" type="text/javascript"></script>



	<script type="text/javascript">
	var items = [
			<%=ltr_items.Text %>
		];
		
		
		function setAutocomplete(){
			$( ".aptComplete" ).autocomplete({
				source: items
			});
		}
		$(function() {$("#<% =txt_email.ClientID %>,#txt_email_conf").bind("cut copy paste", function(event) { event.preventDefault();}); });
	</script>

</head>
<body>
	<div id="container">
		<h1 id="logo">
		<a>Wufoo</a>
		</h1>
		<form id="form1" class="wufoo leftLabel page1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
			<ProgressTemplate>
				<uc3:UC_loader ID="UC_loader1" runat="server" />
			</ProgressTemplate>
		</asp:UpdateProgress>
		<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<asp:Literal ID="ltr_items" runat="server" Visible="false"></asp:Literal>
				<div id="header" class="info">
					<h2>
						<span class="titoloform">
							<%=CurrentSource.getSysLangValue("reqHeadTitle")%>
						</span>
					</h2>
					<div>
						(<%= CurrentLang.TITLE%>)</div>
				</div>
				<ul id="pnl_request" runat="server">
					<li id="errorLi" style="display: none;">
						<h3 id="errorMsgLbl">
							<%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
						<p id="errorMsg">
							<%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
						</p>
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
						<label class="desc" id="Label1" for="txt_email">
							<%=CurrentSource.getSysLangValue("reqEmailConfirm")%>
							<span id="Span2" class="req">*</span>
						</label>
						<div>
							<input id="txt_email_conf" name="Field_email" class="field text large" value="" maxlength="255" tabindex="1" type="text" />
						</div>
						<p class="error" style="">
							<%=CurrentSource.getSysLangValue("reqEmailConfirmError")%></p>
					</li>
					<li id="txt_name_first_cont" class="">
						<label class="desc" id="title21" for="txt_name_first">
							<%=CurrentSource.getSysLangValue("reqFullName")%>
							<span id="req_21" class="req">*</span>
						</label>
						<span>
							<input id="txt_name_first" runat="server" name="Field21" class="field text fn" value="" size="8" tabindex="2" type="text" />
							<label for="Field21">
								<%=CurrentSource.getSysLangValue("reqFirstName")%></label>
						</span>
						<span>
							<input id="txt_name_last" runat="server" name="Field22" class="field text ln" value="" size="14" tabindex="3" type="text" />
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
							<input id="txt_phone" runat="server" class="field text medium" name="Field23" tabindex="4" maxlength="255" value="" type="text" />
						</div>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqRequiredField")%></p>
						<p class="instruct" id="instruct23">
							<small>e.g.
								<%=CurrentSource.getSysLangValue("reqPhoneExample")%></small></p>
					</li>
					<li id="drp_country_cont" class="      ">
						<label class="desc" id="title25" for="drp_country">
							<%=CurrentSource.getSysLangValue("reqLocation")%>
							<span id="Span1" class="req">*</span>
						</label>
						<div>
							<asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id"
								TabIndex="5">
							</asp:DropDownList>
							<asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
								<WhereParameters>
									<asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
								</WhereParameters>
							</asp:LinqDataSource>
						</div>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></p>
						<p class="instruct" id="instruct25">
							<small>e.g. United States</small></p>
					</li>
					<li id="fo1li29" class="      ">
						<label class="desc">
							<%=CurrentSource.getSysLangValue("reqApartments")%></label>
						<span class="descrsx" style="clear: both; width: 100%; height: auto;">
							<%=CurrentSource.getSysLangValue("reqAddApartmentsDesc")%>
						</span>
						<div id="selectedItemsCont" style="clear: both; width: 100%;">
							<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<asp:ListView runat="server" ID="LV_apts" OnItemCommand="LV_apts_ItemCommand">
										<ItemTemplate>
											<table class="selected_item">
												<tr>
													<td class="selected_delete">
														<asp:LinkButton ID="lnk_del" runat="server" CommandName="elimina"></asp:LinkButton>
													</td>
													<td class="selected_num">
														<asp:Label ID="lbl_id" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
														<asp:Label ID="lbl_idEstate" runat="server" Visible="false" Text='<%# Eval("IdEstate") %>'></asp:Label>
													</td>
													<td class="selected_text">
														<asp:TextBox ID="txt_choice" runat="server" CssClass="aptComplete field text larg" Text='<%# Eval("Apt") %>' Width="300"></asp:TextBox>
													</td>
												</tr>
											</table>
										</ItemTemplate>
										<EmptyDataTemplate>
										</EmptyDataTemplate>
										<LayoutTemplate>
											<table id="itemPlaceholder" runat="server" />
										</LayoutTemplate>
									</asp:ListView>
									<table class="selected_item">
										<tr>
											<td>
												<asp:LinkButton ID="lnk_addItem" runat="server" CssClass="tastoBianco" OnClick="lnk_addItem_Click">
													<span style=" display:block;"><%=CurrentSource.getSysLangValue("reqAddAnotherChoice")%> 
													<span style=" display:none; background: url('images/css/plus.gif') no-repeat scroll left top transparent; display: inline; float: right;margin: 0; padding:0; width:16px; height:16px;"></span>
													</span>
												</asp:LinkButton>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</li>
					<li id="fo1li452" class="      ">
						<label class="desc" id="title452" for="drp_area">
							<%=CurrentSource.getSysLangValue("reqOrAndArea")%>
						</label>
						<div id="area">
							<asp:CheckBoxList ID="chkList_area" runat="server">
							</asp:CheckBoxList>
						</div>
						<p class="instruct" id="instruct452">
							<small>e.g.
								<%=CurrentSource.getSysLangValue("reqAreaOpt1") %></small></p>
					</li>
					<li id="fo1li454" class="">
						<label class="desc" id="title454" for="chkList_price_range">
							<%=CurrentSource.getSysLangValue("reqOrAndPriceRange")%>
						</label>
						<div id="price_range">
							<asp:CheckBoxList ID="chkList_price_range" runat="server">
							</asp:CheckBoxList>
						</div>
						<p class="instruct" id="instruct454">
							<small>e.g. High quality</small></p>
					</li>
					<li id="fo1li137" class="date      ">
						<label class="desc" id="title137" for="Field137">
							<%=CurrentSource.getSysLangValue("reqCheckInDate")%>
						</label>
						<div>
							<input type="text" id="txt_dtStart" style="width: 120px" readonly="readonly" />
							<img id="trigger_dtStart" class="datepicker" src="images/icons/calendar.png" alt="Pick a date." />
							<asp:HiddenField ID="HF_dtStart" runat="server" />
						</div>
						<p class="instruct" id="instruct137">
							<small>e.g. 20 Febbraio 2011</small></p>
					</li>
					<li id="HF_dtEnd_cont" class="date      ">
						<label class="desc" id="title138" for="Field138">
							<%=CurrentSource.getSysLangValue("reqCheckOutDate")%>
						</label>
						<div>
							<input type="text" id="txt_dtEnd" style="width: 120px" readonly="readonly" />
							<img id="trigger_dtEnd" class="datepicker" src="images/icons/calendar.png" alt="Pick a date." />
							<asp:HiddenField ID="HF_dtEnd" runat="server" />
						</div>
						<p class="instruct" id="instruct138">
							<small>e.g. 28 Febbraio 2011</small></p>
						<p class="error">
							<%=CurrentSource.getSysLangValue("reqCheckOutError")%></p>
					</li>
					<li id="fo1li149" class="     ">
						<label class="desc" id="title149" for="chk_date_is_flexible">
						</label>
						<div>
							<span>
								<input id="chk_date_is_flexible" runat="server" name="Field149" class="field checkbox" value="My travel dates are flexible" tabindex="19" type="checkbox" />
								<label class="choice" for="Field149">
									<%=CurrentSource.getSysLangValue("reqTravelDatesFlexible")%></label>
							</span>
						</div>
					</li>
					<li id="fo1li140" class="      ">
						<label class="desc" id="title140" for="drp_adult_num">
							<%=CurrentSource.getSysLangValue("reqAdults")%>
						</label>
						<div>
							<select id="drp_adult_num" runat="server" name="Field140" class="field select small" tabindex="20">
								<option value="1">1 </option>
								<option value="2" selected="selected">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="8">8 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct140">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo1li142" class="      ">
						<label class="desc" id="title142" for="drp_child_num">
							<%=CurrentSource.getSysLangValue("reqChildren")%>
						</label>
						<div>
							<select id="drp_child_num" runat="server" name="Field142" class="field select small" tabindex="21">
								<option value="0" selected="selected"></option>
								<option value="1">1 </option>
								<option value="2">2 </option>
								<option value="3">3 </option>
								<option value="4">4 </option>
								<option value="5">5 </option>
								<option value="6">6 </option>
								<option value="7">7 </option>
								<option value="8">8 </option>
								<option value="9">9 </option>
								<option value="10">10 </option>
							</select>
						</div>
						<p class="instruct" id="instruct142">
							<small>e.g. 2</small></p>
					</li>
					<li id="fo1li657" class="      ">
						<label class="desc" id="title657" for="drp_transport">
							<%=CurrentSource.getSysLangValue("reqMeansOfTransport")%>
						</label>
						<div>
							<select id="drp_transport" runat="server" name="Field657" class="field select large" tabindex="22">
							</select>
						</div>
						<p class="instruct" id="instruct657">
							<small>e.g. Taxi Limousine / Service</small></p>
					</li>
					<li id="fo1li554" class="section      ">
						<h3 id="title554">
							<%=CurrentSource.getSysLangValue("reqOptions")%></h3>
					</li>
					<li id="fo1li555" class="     ">
						<label class="desc" id="title555" for="Field555">
							<%=CurrentSource.getSysLangValue("reqServices")%>
						</label>
						<div id="services">
							<asp:CheckBoxList ID="chkList_services" runat="server">
							</asp:CheckBoxList>
						</div>
						<p class="instruct" id="instruct555">
							<small>e.g. Limo service</small></p>
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
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
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
						$get("drp_country_cont").className = "";
						if ($get("drp_country").value == "") {
							$get("drp_country_cont").className = "error";
							_validation = false;
						}

						$get("HF_dtEnd_cont").className = "";
						try {
							var _startStr = $get("HF_dtStart").value;
							var _endStr = $get("HF_dtEnd").value;
							var _startInt = parseInt(_startStr);
							var _endInt = parseInt(_endStr);
							if (_startInt > _endInt) {
								$get("HF_dtEnd_cont").className = "error";
								_validation = false;
							}
						}
						catch (ex) { }
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
                    var _JSCal_Range;
                    function setCal() {
                        _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", startTrigger: "#trigger_dtStart", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", endTrigger: "#trigger_dtEnd", changeMonth: true, changeYear: true });
                    }
                    setCal();
                </script>

			</ContentTemplate>
		</asp:UpdatePanel>
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
