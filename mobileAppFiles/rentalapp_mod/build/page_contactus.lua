-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.prevPageType = _G.currPageType;
	_G.currPageType = "contactus";
	local currResponse;
	local currTBL;
    local menuGroup = display.newGroup()
	
    local curPage = 5 
	local Allen = require "inc_AllenFunctions"
	local widget = require "widget"
	widget.setTheme("theme_ios")

    local drawScreen = function() 
		getMainMenu(menuGroup);
	getBackBtn(_G.prevPageType,nil,nil,nil)
	getLastSearchDates();
    local function fillData() 
		local dettit = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize35);
		dettit.alpha = 1; dettit.oldAlpha = 1 
		dettit.text = "Contact us"
		dettit:setTextColor(253, 102, 52)
		dettit.size = _G.css.defaultFontSize35
		dettit:setReferencePoint(display.TopLeftReferencePoint);
		dettit.x = 120; dettit.y = 242;
		local nomeZonaDet = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		nomeZonaDet.alpha = 1; dettit.oldAlpha = 1 
		nomeZonaDet.text = "Fill the form below"
		nomeZonaDet:setTextColor(51, 51, 102)
		nomeZonaDet.size = _G.css.defaultFontSize30

		nomeZonaDet:setReferencePoint(display.TopLeftReferencePoint);
		nomeZonaDet.x=120;
		nomeZonaDet.y=282;
		
		lineazonedet = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
		lineazonedet.x = 322; lineazonedet.y = 336; lineazonedet.alpha = 1; lineazonedet.oldAlpha = 1 

		
	local lbl_client_login;
	local lbl_client_pwd;
	local lbl_client_name_first;
	local lbl_client_name_last;
	local lbl_client_contact_email;
	local lbl_client_contact_emailconf;
	local lbl_client_loc_country;
	local lbl_client_contact_phone_mobile;
	local lbl_special_request;
	
	local txt_client_login;
	local txt_client_pwd;
	local txt_client_name_first;
	local txt_client_name_last;
	local txt_client_contact_email;
	local txt_client_contact_emailconf;
	local txt_client_loc_country;
	local txt_client_contact_phone_mobile;
	local txt_special_request;
	local chk_recontactPhone;
		
	local errorToolTipFirstTime=true;
	local validateForm = function()
		errorToolTipFirstTime=false;
		if(_G.ErrorTtp_gr ~= nil) then
			_G.ErrorTtp_gr:removeSelf();
			_G.ErrorTtp_gr = nil;
		end
		local isValid = true;

		lbl_client_name_first:setTextColor(51, 51, 102);
		if(txt_client_name_first.text==nil or string.gsub(""..txt_client_name_first.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_name_first.y+_G.css.FKB_singleHeigth)
			lbl_client_name_first:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_name_last:setTextColor(51, 51, 102);
		if(txt_client_name_last.text==nil or string.gsub(""..txt_client_name_last.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_name_last.y+_G.css.FKB_singleHeigth)
			lbl_client_name_last:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_contact_email:setTextColor(51, 51, 102);
		lbl_client_contact_emailconf:setTextColor(51, 51, 102);
		if(txt_client_contact_email.text==nil or string.gsub(""..txt_client_contact_email.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_contact_email.y+_G.css.FKB_singleHeigth)
			lbl_client_contact_email:setTextColor(255, 0, 0);
			return false;
		elseif( Allen.isEmail(""..txt_client_contact_email.text)==false ) then
			_G.ErrorTtp_show(_G.lbl.mobileInvalidEmailFormat,txt_client_contact_email.y+_G.css.FKB_singleHeigth)
			lbl_client_contact_email:setTextColor(255, 0, 0);
			return false;
		elseif( txt_client_contact_email.text~=txt_client_contact_emailconf.text ) then
			_G.ErrorTtp_show(_G.lbl.mobileEmailConfirmError,txt_client_contact_emailconf.y+_G.css.FKB_singleHeigth)
			lbl_client_contact_emailconf:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_loc_country:setTextColor(51, 51, 102);
		if(txt_client_loc_country.text==nil or string.gsub(""..txt_client_loc_country.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_loc_country.y+_G.css.FKB_singleHeigth)
			lbl_client_loc_country:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_contact_phone_mobile:setTextColor(51, 51, 102);
		if(txt_client_contact_phone_mobile.text==nil or string.gsub(""..txt_client_contact_phone_mobile.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_contact_phone_mobile.y+_G.css.FKB_singleHeigth)
			lbl_client_contact_phone_mobile:setTextColor(255, 0, 0);
			return false;
		end
		return isValid;
	end
	formInputCallback = function()
		if(errorToolTipFirstTime==false) then
			validateForm();
		end
	end
	
	local currentForm = "";
	local formGroup = display.newGroup() 

	local showNewClient = function() 	
		fldY = 360;
		fldYdiff=_G.css.FKB_singleHeigth+_G.css.FKB_txtPadY;
		lblX = 36;
		txtX = 300;
		txtW = 300;
		if not(formGroup==nil) then formGroup:removeSelf(); end
		formGroup = display.newGroup() 
		currentForm = "new";
		lbl_client_name_first = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_name_first.alpha = 1; lbl_client_name_first.oldAlpha = 1 
		lbl_client_name_first.text = _G.lbl.lblName..":"
		lbl_client_name_first:setTextColor(51, 51, 102)
		lbl_client_name_first.size = _G.css.defaultFontSize30
		lbl_client_name_first:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_name_first.x=lblX;
		lbl_client_name_first.y=fldY;
		formGroup:insert(lbl_client_name_first) 
		txt_client_name_first = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, _G.css.FKB_setFakeInputText, formGroup, _G.lbl.lblName, formInputCallback);

		fldY = fldY+fldYdiff;
		lbl_client_name_last = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_name_last.alpha = 1; lbl_client_name_last.oldAlpha = 1 
		lbl_client_name_last.text = _G.lbl.lblSurname..":"
		lbl_client_name_last:setTextColor(51, 51, 102)
		lbl_client_name_last.size = _G.css.defaultFontSize30
		lbl_client_name_last:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_name_last.x=lblX;
		lbl_client_name_last.y=fldY;
		formGroup:insert(lbl_client_name_last)
		txt_client_name_last = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.lblSurname, formInputCallback);

		fldY = fldY+fldYdiff;
		lbl_client_contact_email = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_contact_email.alpha = 1; lbl_client_contact_email.oldAlpha = 1 
		lbl_client_contact_email.text = _G.lbl.mobileFormEmail..":"
		lbl_client_contact_email:setTextColor(51, 51, 102)
		lbl_client_contact_email.size = _G.css.defaultFontSize30
		lbl_client_contact_email:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_contact_email.x=lblX;
		lbl_client_contact_email.y=fldY;
		formGroup:insert(lbl_client_contact_email) 
		txt_client_contact_email = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.mobileFormEmail, formInputCallback);

		fldY = fldY+fldYdiff;
		lbl_client_contact_emailconf = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_contact_emailconf.alpha = 1; lbl_client_contact_emailconf.oldAlpha = 1 
		lbl_client_contact_emailconf.text = _G.lbl.mobileFormEmailConfirm..":"
		lbl_client_contact_emailconf:setTextColor(51, 51, 102)
		lbl_client_contact_emailconf.size = _G.css.defaultFontSize30
		lbl_client_contact_emailconf:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_contact_emailconf.x=lblX;
		lbl_client_contact_emailconf.y=fldY;
		formGroup:insert(lbl_client_contact_emailconf) 
		txt_client_contact_emailconf = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.mobileFormEmailConfirm, formInputCallback);
		fldY = fldY+fldYdiff;
		lbl_client_loc_country = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_loc_country.alpha = 1; lbl_client_loc_country.oldAlpha = 1 
		lbl_client_loc_country.text = _G.lbl.reqLocation..":"
		lbl_client_loc_country:setTextColor(51, 51, 102)
		lbl_client_loc_country.size = _G.css.defaultFontSize30
		lbl_client_loc_country:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_loc_country.x=lblX;
		lbl_client_loc_country.y=fldY;
		formGroup:insert(lbl_client_loc_country) 
		txt_client_loc_country = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.reqLocation, formInputCallback);

		fldY = fldY+fldYdiff;
		lbl_client_contact_phone_mobile = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_contact_phone_mobile.alpha = 1; lbl_client_contact_phone_mobile.oldAlpha = 1 
		lbl_client_contact_phone_mobile.text = _G.lbl.reqPhoneNumber..":"
		lbl_client_contact_phone_mobile:setTextColor(51, 51, 102)
		lbl_client_contact_phone_mobile.size = _G.css.defaultFontSize30
		lbl_client_contact_phone_mobile:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_contact_phone_mobile.x=lblX;
		lbl_client_contact_phone_mobile.y=fldY;
		formGroup:insert(lbl_client_contact_phone_mobile) 
		txt_client_contact_phone_mobile = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.reqPhoneNumber, formInputCallback);
		
		fldY = fldY+fldYdiff;
		lbl_recontactPhone = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_recontactPhone.alpha = 1; lbl_recontactPhone.oldAlpha = 1 
		lbl_recontactPhone.text = _G.lbl.mobileFormVuoiEssereRichiamato
		lbl_recontactPhone:setTextColor(51, 51, 102)
		lbl_recontactPhone.size = _G.css.defaultFontSize30
		lbl_recontactPhone:setReferencePoint(display.TopLeftReferencePoint);
		lbl_recontactPhone.x=lblX;
		lbl_recontactPhone.y=fldY;
		formGroup:insert(lbl_recontactPhone) 
		chk_recontactPhone = _G.FROM_createCheckBox(lblX+lbl_recontactPhone.width+20, fldY, 46, 46, formGroup, nil)
		
		-- fldY = fldY+fldYdiff;
		-- lbl_special_request = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		-- lbl_special_request.alpha = 1; lbl_special_request.oldAlpha = 1 
		-- lbl_special_request.text = _G.lbl.reqSpecialRequest..":"
		-- lbl_special_request:setTextColor(51, 51, 102)
		-- lbl_special_request.size = _G.css.defaultFontSize30
		-- lbl_special_request:setReferencePoint(display.TopLeftReferencePoint);
		-- lbl_special_request.x=lblX;
		-- lbl_special_request.y=fldY;
		-- formGroup:insert(lbl_special_request) 
		-- fldY = fldY+fldYdiff;
		-- txt_special_request = _G.FKB_createTextInput(lblX, fldY, 560, 100, "", formGroup, _G.lbl.reqSpecialRequest, nil, true);
		
	end
	   
	   local lblErrorAlert = display.newText("", 0,0,600,72, native.systemFontBold, _G.css.defaultFontSize30);
		lblErrorAlert:setTextColor(255, 0, 0)
		lblErrorAlert.text = ""
		lblErrorAlert.size = _G.css.defaultFontSize30
		lblErrorAlert:setReferencePoint(display.CenterCenterReferencePoint);
       lblErrorAlert.x = 325; lblErrorAlert.y = 760; 
       lblErrorAlert.alpha = 1; lblErrorAlert.oldAlpha = 1 
	   
	showNewClient();
	local btnGroup = display.newGroup() 
	local showHideLoading;

	local oncercaTouch = function(event) 
		if event.phase=="ended" then  
			local function networkListenerBooking( event )
				if ( event.isError ) then
					print ( "Network error - download failed: " .. event.response )
					lblErrorAlert.text="There was an error on sending your mail.\nPlease retry later.";
				else
					if(event.response == "ok") then
						lblErrorAlert.text="We have received your request.\nYou will be contacted by our staff soon.";
						_G.clConfig.selectedEstates = {}
					else
						lblErrorAlert.text="There was an error on sending your mail.\nPlease retry later.";
					end
				end
				formGroup:removeSelf();
				btnGroup:removeSelf();
				timerStash.newTimer_319 = timer.performWithDelay(3000, function()
						disposeTweens()
						director:changeScene( "page_".._G.prevPageType, "crossfade" )
					end , 1)
			end
			local xmlFilter= "";
			xmlFilter=xmlFilter.."&mobilexml=true";
			xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
			xmlFilter=xmlFilter.."&id=".._G.clConfig.currEstateID;
			xmlFilter=xmlFilter.."&preflist="..selectedEstatesToString("|");
			xmlFilter=xmlFilter.."&dtS="..JSCal:dateToString( _G.clConfig.dataCheckin);
			xmlFilter=xmlFilter.."&dtE="..JSCal:dateToString( _G.clConfig.dataCheckout);
			xmlFilter=xmlFilter.."&numPers_adult=".._G.clConfig.guestsNum.."&numPers_childOver=0&numPers_childMin=0";
			
			if(validateForm()==false) then return false; end
			xmlFilter=xmlFilter.."&client_loc_country="..txt_client_loc_country.text;
			xmlFilter=xmlFilter.."&client_contact_email="..txt_client_contact_email.text;
			--xmlFilter=xmlFilter.."&client_name_honorific="..txt_client_name_honorific.text;
			xmlFilter=xmlFilter.."&client_name_full="..txt_client_name_first.text.." "..txt_client_name_last.text;
			xmlFilter=xmlFilter.."&client_contact_phone_mobile="..txt_client_contact_phone_mobile.text;
			if(chk_recontactPhone.state=="ok") then
				xmlFilter=xmlFilter.."&special_request=vuole essere richiamato";
			end
			
			xmlFilter=xmlFilter.."";
			xmlFilter=xmlFilter.."";
			local xmlUrl=_G.requestHost.."/webservice/util_mailContactXml.aspx?t="..os.time(os.date( '*t' ));
			local xmlHeaders = {}
			xmlHeaders["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			xmlHeaders["Accept-Encoding"] = "gzip, deflate";
			xmlHeaders["Accept-Language"] = "it-it,it;q=0.8,en-us;q=0.5,en;q=0.3";
			xmlHeaders["Connection"] = "keep-alive";
			xmlHeaders["Host"] = "www.rentalinrome.com";
			xmlHeaders["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:15.0) Gecko/20100101 Firefox/15.0";
			local xmlParams = {}
			xmlParams.headers = xmlHeaders
			xmlParams.body = "formpost=true"..xmlFilter
			--print ( "xmlFilter: " .. xmlFilter )
			showHideLoading(true);
			network.request( xmlUrl, "POST", networkListenerBooking, xmlParams)
		end 
	end
	showHideLoading = function(showLoading) 
		if not(btnGroup==nil) then btnGroup:removeSelf(); end
		btnGroup = display.newGroup() 
		if(showLoading) then
			showLoadingForm(btnGroup);
		else
			cerca = ui.newButton{ 
				defaultSrc=imgDir.."btn_big.png", 
				defaultX = 655, 
				defaultY = 121, 
				overSrc=imgDir.."btn_big_hov.png", 
				overX = 655, 
				overY = 121, 
				id="cercaButton" 
			}
			cerca:addEventListener( "touch", oncercaTouch )
			cerca.x = 319; cerca.y = 905; cerca.alpha = 1; cerca.oldAlpha = 1 
			btnGroup:insert(cerca) 

			local testoBtnCerca = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize50);
			testoBtnCerca:setTextColor(255, 255, 255)
			testoBtnCerca.text = "Submit"
			testoBtnCerca.size = _G.css.defaultFontSize50
			testoBtnCerca:setReferencePoint(display.CenterCenterReferencePoint);
			testoBtnCerca.x = 325; testoBtnCerca.y = 905; 
			testoBtnCerca.alpha = 1; testoBtnCerca.oldAlpha = 1 
			btnGroup:insert(testoBtnCerca) 
		end
		
		
	end
	
	showHideLoading(false);
	   
	end  
	
	fillData();
end 
   
   
   
	drawScreen();
   return menuGroup 
end 
