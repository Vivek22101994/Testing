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

		fldY = 345;
		fldYdiff=40;
		lblX = 36;
		txtX = 300;
		txtW = 300;
		
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
		
	local errorToolTipFirstTime=true;
	local errorToolTipGroup = display.newGroup();
	local errorToolTipBg;
	local errorToolTipLbl;
	local showErrorToolTip = function(txt, y)
		errorToolTipGroup = display.newGroup() 
		errorToolTipBg = display.newImageRect( imgDir.. "img_error_tooltip_bg.png", display.contentWidth-0, 80 ); 
		errorToolTipBg:setReferencePoint(display.CenterCenterReferencePoint);	
		errorToolTipGroup:insert(errorToolTipBg)
		errorToolTipBg.x = (display.contentWidth-0)/2; errorToolTipBg.y = 0; errorToolTipBg.alpha = 1; errorToolTipBg.oldAlpha = 1 
		
		errorToolTipLbl = display.newText(txt, 0,0,0,0, native.systemFontBold, 28);
		errorToolTipLbl:setTextColor(255, 255, 255)
		errorToolTipLbl:setReferencePoint(display.CenterCenterReferencePoint);	
		errorToolTipGroup:insert(errorToolTipLbl)
		errorToolTipLbl.x = (display.contentWidth-0)/2; errorToolTipLbl.y = 10; errorToolTipLbl.alpha = 1; errorToolTipLbl.oldAlpha = 1 
		if(errorToolTipLbl.width > display.contentWidth-50) then errorToolTipLbl.width = display.contentWidth-50; errorToolTipLbl.text = txt; end

		-- errorArrow = display.newImageRect( imgDir.. "img_freccia_a_dx.png", 52, 41 ); 
		-- errorArrow.x = txtX-20; errorArrow.y = y; errorArrow.alpha = 1; errorArrow.oldAlpha = 1 
		-- errorToolTipGroup:insert(errorArrow) 
		-- gtStash.gt_frecciaaddfav_202= gtween.new( frecciaaddfav, 0.3, { x = 578, y = 154, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = true,  delay=0}) 

		errorToolTipGroup:setReferencePoint(display.TopLeftReferencePoint);	
		errorToolTipGroup.y=760;
		errorToolTipGroup.x=0;
	end
		-- local txt_client_login;
		-- local txt_client_pwd;
	local validateForm = function()
		errorToolTipFirstTime=false;
		errorToolTipGroup:removeSelf();
		local isValid = true;
		lbl_client_name_first:setTextColor(51, 51, 102);
		if(txt_client_name_first.text==nil or string.gsub(""..txt_client_name_first.text , "%s", "")=="") then
			showErrorToolTip(_G.lbl.mobileRequiredField,txt_client_name_first.y)
			lbl_client_name_first:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_name_last:setTextColor(51, 51, 102);
		if(txt_client_name_last.text==nil or string.gsub(""..txt_client_name_last.text , "%s", "")=="") then
			showErrorToolTip(_G.lbl.mobileRequiredField,txt_client_name_last.y)
			lbl_client_name_last:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_contact_email:setTextColor(51, 51, 102);
		lbl_client_contact_emailconf:setTextColor(51, 51, 102);
		if(txt_client_contact_email.text==nil or string.gsub(""..txt_client_contact_email.text , "%s", "")=="") then
			showErrorToolTip(_G.lbl.mobileRequiredField,txt_client_contact_email.y)
			lbl_client_contact_email:setTextColor(255, 0, 0);
			return false;
		elseif( Allen.isEmail(""..txt_client_contact_email.text)==false ) then
			showErrorToolTip(_G.lbl.mobileInvalidEmailFormat,txt_client_contact_email.y)
			lbl_client_contact_email:setTextColor(255, 0, 0);
			return false;
		elseif( txt_client_contact_email.text~=txt_client_contact_emailconf.text ) then
			showErrorToolTip(_G.lbl.mobileEmailConfirmError,txt_client_contact_emailconf.y)
			lbl_client_contact_emailconf:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_loc_country:setTextColor(51, 51, 102);
		if(txt_client_loc_country.text==nil or string.gsub(""..txt_client_loc_country.text , "%s", "")=="") then
			showErrorToolTip(_G.lbl.mobileRequiredField,txt_client_loc_country.y)
			lbl_client_loc_country:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_contact_phone_mobile:setTextColor(51, 51, 102);
		if(txt_client_contact_phone_mobile.text==nil or string.gsub(""..txt_client_contact_phone_mobile.text , "%s", "")=="") then
			showErrorToolTip(_G.lbl.mobileRequiredField,txt_client_contact_phone_mobile.y)
			lbl_client_contact_phone_mobile:setTextColor(255, 0, 0);
			return false;
		end
		return isValid;
	end
	local KeyboardGroup;
	local function showKeyboard(obj)
		local mainPadX = 30;
		local mainPadY = 30;
		local mainH = display.contentHeight - _G.css.keyboardHeigth - 50;
		local padLeft = 100;
		local width = display.contentWidth;
		KeyboardGroup = display.newGroup()
		local overlayImg = ui.newButton{ 
			defaultSrc=imgDir.."img_overlay.png", 
			defaultX = display.contentWidth, 
			defaultY = display.contentHeight, 
			overSrc=imgDir.."img_overlay.png", 
			overX = display.contentWidth, 
			overY = display.contentHeight
		}
		overlayImg:setReferencePoint(display.TopLeftReferencePoint);
		overlayImg.alpha = .5; overlayImg.oldAlpha = .5 	   
		overlayImg.x = 0; overlayImg.y = 0; 
		overlayImg.width = display.contentWidth; overlayImg.height = display.contentHeight; 
		KeyboardGroup:insert(overlayImg) 
		
		local txt = native.newTextBox( mainPadX, mainPadY, width-mainPadX*2, mainH-mainPadY*2)
		txt.size = _G.css.inputFontSize
		txt.hasBackground = false;
		txt.isEditable = true;
		txt.alpha = 1; txt.oldAlpha = 1 
		txt:setTextColor(255, 106, 48)
		txt:setReferencePoint(display.TopLeftReferencePoint);
		txt.x=mainPadX;
		txt.y=mainPadY;
		KeyboardGroup:insert(txt) 
		
		if ( "began" == event.phase ) then
			print("_began_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		elseif ( "ended" == event.phase ) then
			if(errorToolTipFirstTime==false) then
				validateForm();
			end
			print("_ended_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		elseif ( "submitted" == event.phase ) then
			native.setKeyboardFocus(nil);
			print("_submitted_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		end
	end
	
	local function fieldHandler( event )
		if ( "began" == event.phase ) then
			print("_began_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		elseif ( "ended" == event.phase ) then
			if(errorToolTipFirstTime==false) then
				validateForm();
			end
			print("_ended_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		elseif ( "submitted" == event.phase ) then
			native.setKeyboardFocus(nil);
			print("_submitted_")
			print("display.contentHeight:"..display.contentHeight)
			print("display.viewableContentHeight:"..display.viewableContentHeight)
			print("_____")
		end
	end
	local currentForm = "";
	local formGroup = display.newGroup() 
	local createTextInput = function(x, y)
		if(_G.txtUseLbl) then
			local txt = display.newText("", 0,0, native.systemFontBold, 30);
			txt.alpha = 1; txt.oldAlpha = 1
			txt.font = native.newFont( native.systemFont );
			txt.size = 30
			txt:setTextColor(255, 106, 48)
			return txt;
		else
			local txt = native.newTextField( 0, 0, txtW, _G.css.inputHeigth )
			--txt.alpha = 1; txt.oldAlpha = 1
			txt.font = native.newFont( native.systemFont, _G.css.inputFontSize );
			txt.size = _G.css.inputFontSize
			txt:setTextColor(255, 106, 48)
			txt:addEventListener( 'userInput', fieldHandler )
			return txt;
		end
	end

	local showNewClient = function() 	
		fldY = 300;
		if not(formGroup==nil) then formGroup:removeSelf(); end
		formGroup = display.newGroup() 
		currentForm = "new";
		fldY = fldY+fldYdiff;
		lbl_client_name_first = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_client_name_first.alpha = 1; lbl_client_name_first.oldAlpha = 1 
		lbl_client_name_first.text = _G.lbl.lblName..":"
		lbl_client_name_first:setTextColor(51, 51, 102)
		lbl_client_name_first.size = _G.css.defaultFontSize30
		lbl_client_name_first:setReferencePoint(display.TopLeftReferencePoint);
		lbl_client_name_first.x=lblX;
		lbl_client_name_first.y=fldY;
		formGroup:insert(lbl_client_name_first) 
		txt_client_name_first = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_name_first.text = "Test1"
		end
		txt_client_name_first:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_name_first.x=txtX;
		txt_client_name_first.y=fldY;
		formGroup:insert(txt_client_name_first)

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
		txt_client_name_last = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_name_last.text = "Maga"
		end
		txt_client_name_last:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_name_last.x=txtX;
		txt_client_name_last.y=fldY;
		--formGroup:insert(txt_client_name_last) 

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
		txt_client_contact_email = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_contact_email.text = "adilet@magadesign.net"
		end
		txt_client_contact_email:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_contact_email.x=txtX;
		txt_client_contact_email.y=fldY;
		formGroup:insert(txt_client_contact_email) 

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
		txt_client_contact_emailconf = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_contact_emailconf.text = "adilet@magadesign.net"
		end
		txt_client_contact_emailconf:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_contact_emailconf.x=txtX;
		txt_client_contact_emailconf.y=fldY;
		formGroup:insert(txt_client_contact_emailconf) 

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
		txt_client_loc_country = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_loc_country.text = "Italy"
		end
		txt_client_loc_country:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_loc_country.x=txtX;
		txt_client_loc_country.y=fldY;
		formGroup:insert(txt_client_loc_country) 

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
		txt_client_contact_phone_mobile = createTextInput(txtX, fldY);
		if(_G.txtUseLbl) then		
			txt_client_contact_phone_mobile.text = "1234"
		end
		txt_client_contact_phone_mobile:setReferencePoint(display.TopLeftReferencePoint);
		txt_client_contact_phone_mobile.x=txtX;
		txt_client_contact_phone_mobile.y=fldY;
		formGroup:insert(txt_client_contact_phone_mobile) 
		
		fldY = fldY+fldYdiff;
		lbl_special_request = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_special_request.alpha = 1; lbl_special_request.oldAlpha = 1 
		lbl_special_request.text = "Special request:"
		lbl_special_request:setTextColor(51, 51, 102)
		lbl_special_request.size = _G.css.defaultFontSize30
		lbl_special_request:setReferencePoint(display.TopLeftReferencePoint);
		lbl_special_request.x=lblX;
		lbl_special_request.y=fldY;
		formGroup:insert(lbl_special_request) 
		fldY = fldY+fldYdiff;
		if(_G.txtUseLbl) then		
			txt_special_request = display.newText("", 0,0, 600, 100, native.systemFontBold, _G.css.defaultFontSize30);
			txt_special_request.size = _G.css.defaultFontSize30
			txt_special_request.text = "... My special request notes ..."
		else
			txt_special_request = native.newTextBox( lblX, fldY, 560, 100)
			txt_special_request.size = _G.css.inputFontSize
			txt_special_request.hasBackground = true;
			txt_special_request.isEditable = true;
		end
		txt_special_request.alpha = 1; txt_special_request.oldAlpha = 1 
		txt_special_request:setTextColor(255, 106, 48)
		txt_special_request:setReferencePoint(display.TopLeftReferencePoint);
		txt_special_request.x=lblX;
		txt_special_request.y=fldY;
		formGroup:insert(txt_special_request) 
		
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
			xmlFilter=xmlFilter.."&special_request="..txt_special_request.text;
			
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
