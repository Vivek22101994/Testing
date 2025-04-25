-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_book";
	local currResponse;
	local currTBL;
	local currAvv;
	local currGallery;
    local menuGroup = display.newGroup() 
	
    local curPage = 5 
	local Allen = require "inc_AllenFunctions"
	local widget = require "widget"
	widget.setTheme("theme_ios")

    local drawScreen = function() 
		getMainMenu(menuGroup);
	
	

	getBackBtn(nil,nil,nil,nil)
	getLastSearchDates();
    local function fillData() 
-- carica da xml

---	currTBL
--		<apt_id>1</apt_id>
--		<apt_name>Trevi Fountain View Apartment</apt_name>
--		<apt_vote>10</apt_vote>
--		<apt_img>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</apt_img>
--		<zone_id>2</zone_id>
--		<zone_name>Trevi Fountain</zone_name>
--		<summary>Prestigious two levels apartment located on the fifth floor ...</summary>

--- currAvv
--		<is_avv>1</is_avv>
--		<pr_total>200,00</pr_total>

--- currGallery
--		<foto>
--			<thumb>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</thumb>
--			<big>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</big>
--		</foto>
--		<foto>
--			<thumb>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</thumb>
--			<big>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</big>
--		</foto>

		currTBL = currResponse.child[1];
		currAvv = currResponse.child[2];
		local apt_id = currTBL.child[1].value
		local apt_name = currTBL.child[2].value
		local apt_vote = currTBL.child[3].value; if(apt_vote==nil) then apt_vote = "9"; end
		local apt_img = currTBL.child[4].value; if(apt_img==nil) then apt_img = ""; end
		local zone_id = currTBL.child[5].value; if(zone_id==nil) then zone_id = "0"; end
		local zone_name = currTBL.child[6].value; if(zone_name==nil) then zone_name = "Rome"; end
		local summary = currTBL.child[7].value; if(summary==nil) then summary = ""; end

		local is_avv = false; if(currAvv.child[1].value=="1") then is_avv = true; end
		local pr_total = currAvv.child[2].value; if(pr_total==nil) then pr_total = "error"; end
		local pr_part_advance = currAvv.child[3].value; if(pr_part_advance==nil) then pr_part_advance = "error"; end
		local pr_part_onarrival = currAvv.child[4].value; if(pr_part_onarrival==nil) then pr_part_onarrival = "error"; end

		getAptHeader();
	   
--(2) GRUPPO FASCETTA TOTAL PRICE

	   local totalPriceGroup = display.newGroup() 
	   
--(2) sfondo fascetta total price
       totalPricebg = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
       totalPricebg.x = 320; totalPricebg.y = 362; totalPricebg.alpha = 1; totalPricebg.oldAlpha = 1
	   totalPriceGroup:insert(totalPricebg) 
	   
-- (6) testo 1 total price
		
		local totalPriceTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		totalPriceTxt.alpha = 1; totalPriceTxt.oldAlpha = 1 
		totalPriceTxt.text = _G.lbl.mobileAcconto..":"
		totalPriceTxt:setTextColor(51, 51, 102)
		totalPriceTxt.size = _G.css.defaultFontSize30
		
		totalPriceTxt:setReferencePoint(display.TopLeftReferencePoint);
		totalPriceTxt.x=36;
		totalPriceTxt.y=345;
		totalPriceGroup:insert(totalPriceTxt) 
		
-- (6) testo 2 total price
		
		local totalPrice = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		totalPrice.alpha = 1; totalPrice.oldAlpha = 1 
		totalPrice.text = pr_part_advance.."€"
		totalPrice:setTextColor(255, 106, 48)
		totalPrice.size = _G.css.defaultFontSize40
		
		totalPrice:setReferencePoint(display.TopRightReferencePoint);
		totalPrice.x=605;
		totalPrice.y=340;
		totalPriceGroup:insert(totalPrice)
	local lbl_logged_client_name;
	local lbl_logged_client_email;
	local lbl_client_name_first;
	local lbl_client_name_last;
	local lbl_client_contact_email;
	local lbl_client_contact_emailconf;
	local lbl_client_loc_country;
	local lbl_client_contact_phone_mobile;
	
	local txt_logged_client_name;
	local txt_logged_client_email;
	local txt_client_name_first;
	local txt_client_name_last;
	local txt_client_contact_email;
	local txt_client_contact_emailconf;
	local txt_client_loc_country;
	local txt_client_contact_phone_mobile;
		
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
	fldY = 360;
	fldYdiff=_G.css.FKB_singleHeigth+_G.css.FKB_txtPadY;
	lblX = 36;
	txtX = 300;
	txtW = 300;
	local showLoggedClient = function() 	
		fldY = 545;
		if not(formGroup==nil) then formGroup:removeSelf(); end
		formGroup = display.newGroup() 
		currentForm = "logged";
		fldY = fldY+fldYdiff;
		lbl_logged_client_name = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_logged_client_name.alpha = 1; lbl_logged_client_name.oldAlpha = 1 
		lbl_logged_client_name.text = _G.lbl.mobileWelcome..", ".._G.clConfig.clDett["name_full"]
		lbl_logged_client_name:setTextColor(255, 106, 48)
		lbl_logged_client_name.size = _G.css.defaultFontSize30
		lbl_logged_client_name:setReferencePoint(display.TopLeftReferencePoint);
		lbl_logged_client_name.x=lblX;
		lbl_logged_client_name.y=fldY;
		formGroup:insert(lbl_logged_client_name) 

		fldY = fldY+fldYdiff;
		lbl_logged_client_email = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_logged_client_email.alpha = 1; lbl_logged_client_email.oldAlpha = 1 
		lbl_logged_client_email.text = _G.lbl.mobileFormEmail..":"
		lbl_logged_client_email:setTextColor(51, 51, 102)
		lbl_logged_client_email.size = _G.css.defaultFontSize30
		lbl_logged_client_email:setReferencePoint(display.TopLeftReferencePoint);
		lbl_logged_client_email.x=lblX;
		lbl_logged_client_email.y=fldY;
		formGroup:insert(lbl_logged_client_email) 
		txt_logged_client_email = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		txt_logged_client_email.size = _G.css.defaultFontSize30
		txt_logged_client_email.alpha = 1; txt_logged_client_email.oldAlpha = 1 
		txt_logged_client_email.text = "".._G.clConfig.clDett["email"]
		txt_logged_client_email:setTextColor(255, 106, 48)
		txt_logged_client_email:setReferencePoint(display.TopLeftReferencePoint);
		txt_logged_client_email.x=txtX;
		txt_logged_client_email.y=fldY;
		formGroup:insert(txt_logged_client_email) 
	end
	local showNewClient = function() 	
		fldY = 365;
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
		txt_client_name_first = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, _G.lbl.lblName, formInputCallback);

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
	end
	   
 
	   local lblErrorAlert = display.newText("", 0,0,600,0, native.systemFontBold, _G.css.defaultFontSize30);
		lblErrorAlert:setTextColor(255, 0, 0)
		lblErrorAlert.text = ""
		lblErrorAlert.size = _G.css.defaultFontSize30
		lblErrorAlert:setReferencePoint(display.CenterCenterReferencePoint);
       lblErrorAlert.x = 325; lblErrorAlert.y = 760; 
       lblErrorAlert.alpha = 1; lblErrorAlert.oldAlpha = 1 
	   
	if(is_avv==true) then
		if(_G.clConfig.clId ~= 0 and _G.clConfig.clDett["name_full"] ~= nil and _G.clConfig.clDett["name_full"] ~= "") then 
			showLoggedClient();
		else
			showNewClient();
		end
	else
		lblErrorAlert.text = "Not Available"
		return false;
	end
	local btnGroup = display.newGroup() 
	local showHideLoading;

	local oncercaTouch = function(event) 
		if event.phase=="ended" then  
			local function networkListenerBooking( event )
				if ( event.isError ) then
						print ( "Network error - download failed: " .. event.response )
				else
					if(event.response:find("<error>") == nil) then
						lblErrorAlert.text=_G.lbl.mobileMsg_reservationCreated;
						local url = "https://rentalinrome.com/reservationarea/";
						if(event.response:find("<okurl>") ~= nil) then
							url = event.response:gsub( "<okurl>", ""):gsub( "</okurl>", "");
						end
						timerStash.newTimer_041 = timer.performWithDelay(5000, function() 
								system.openURL(url);
							end , 1) 
						timerStash.newTimer_042 = timer.performWithDelay(7000, function() 
								disposeTweens() 
								director:changeScene( "page_init_search", "crossfade" ) 
							end , 1) 
						formGroup:removeSelf();
						btnGroup:removeSelf();
					else
						--<error>No data found</error>
						--<error>Not Avvailable</error>
						--<error>Exists</error>
						--<error>Disabled</error>
						--<error>WrongUsernameOrPassword</error>
						--<error>Disabled</error>
						local _error = event.response:gsub( "<error>", ""):gsub( "</error>", "");
						if(_error=="Exists") then
							_G.clConfig.lastError=_G.lbl.mobileMsg_emailExists;
							_G.clConfig.lastInputText = txt_client_contact_email.text;
							local myClosure_switch = function() 
								disposeTweens() 
								director:changeScene( "page_cl_login", "crossfade" ) 
							end 
							timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
						elseif(_error=="WrongUsernameOrPassword") then
							_G.clConfig.lastError=_G.lbl.mobileMsg_WrongUsernameOrPassword;
							_G.clConfig.lastInputText = txt_client_contact_email.text;
							local myClosure_switch = function() 
								disposeTweens() 
								director:changeScene( "page_cl_login", "crossfade" ) 
							end 
							timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
						else
							lblErrorAlert.text=_error;
						end
						showHideLoading(false);
					end
					--print ( "RESPONSE: " .. event.response )
					--currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
					--fillData()
				end
			end
			local xmlFilter= "";
			xmlFilter=xmlFilter.."&mobilexml=true";
			xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
			xmlFilter=xmlFilter.."&id=".._G.clConfig.currEstateID;
			xmlFilter=xmlFilter.."&dtS="..JSCal:dateToString( _G.clConfig.dataCheckin);
			xmlFilter=xmlFilter.."&dtE="..JSCal:dateToString( _G.clConfig.dataCheckout);
			xmlFilter=xmlFilter.."&numPers_adult=".._G.clConfig.guestsNum.."&numPers_childOver=0&numPers_childMin=0";
			
			if(_G.clConfig.clId ~= 0 and _G.clConfig.clDett["name_full"] ~= nil and _G.clConfig.clDett["name_full"] ~= "") then 
				xmlFilter=xmlFilter.."&client_id=".._G.clConfig.clId;
				xmlFilter=xmlFilter.."&client_contact_email=".._G.clConfig.clDett["email"];
			else
				if(validateForm()==false) then return false; end
				xmlFilter=xmlFilter.."&client_loc_country="..txt_client_loc_country.text;
				xmlFilter=xmlFilter.."&client_contact_email="..txt_client_contact_email.text;
				--xmlFilter=xmlFilter.."&client_name_honorific="..txt_client_name_honorific.text;
				xmlFilter=xmlFilter.."&client_name_full="..txt_client_name_first.text.." "..txt_client_name_last.text;
				xmlFilter=xmlFilter.."&client_contact_phone_mobile="..txt_client_contact_phone_mobile.text;
			end
			xmlFilter=xmlFilter.."";
			xmlFilter=xmlFilter.."";
			local xmlUrl=_G.requestHost.."/webservice/rnt_reservationNewXml.aspx?t="..os.time(os.date( '*t' ));
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
			showLoadingForm(btnGroup)
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
			testoBtnCerca.text = "BOOK NOW!"
			testoBtnCerca.size = _G.css.defaultFontSize50
			testoBtnCerca:setReferencePoint(display.CenterCenterReferencePoint);
			testoBtnCerca.x = 325; testoBtnCerca.y = 905; 
			testoBtnCerca.alpha = 1; testoBtnCerca.oldAlpha = 1 
			btnGroup:insert(testoBtnCerca) 
		end
	end
	showHideLoading(false);
	   
	end  
	 
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			if(_G.currPageType == "apt_book") then
				hideLoading();
				currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
				_G.clConfig.currEstateTB = currResponse.child[1];
				fillData()
			end
		end
	end
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
	xmlFilter=xmlFilter.."&id=".._G.clConfig.currEstateID;
	xmlFilter=xmlFilter.."&dtS="..JSCal:dateToString( _G.clConfig.dataCheckin);
	xmlFilter=xmlFilter.."&dtE="..JSCal:dateToString( _G.clConfig.dataCheckout);
	xmlFilter=xmlFilter.."&numPers_adult=".._G.clConfig.guestsNum.."&numPers_childOver=0&numPers_childMin=0";
	xmlFilter=xmlFilter.."";
	xmlFilter=xmlFilter.."";
	xmlFilter=xmlFilter.."";
	local xmlUrl=_G.requestHost.."/webservice/rnt_estateDettXml.aspx?t="..os.time(os.date( '*t' ));
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
	showLoading();
	network.request( xmlUrl, "POST", networkListener, xmlParams)
	
end 
   
	drawScreen();
   return menuGroup 
end 
