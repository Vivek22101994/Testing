-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	if(_G.currPageType ~= "cl_login" and _G.currPageType ~= "cl_pwdrecovery") then 
		_G.prevPageType = _G.currPageType;
	end
	_G.currPageType = "cl_login";
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
	getBackBtn(_G.prevPageType,nil,nil,nil)
	getLastSearchDates();
	local showHideLoading;
	local currentForm = "";
	local dettit = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize35);
	dettit.alpha = 1; dettit.oldAlpha = 1 
	dettit.text = "Login"
	dettit:setTextColor(253, 102, 52)
	dettit.size = _G.css.defaultFontSize35
	dettit:setReferencePoint(display.TopLeftReferencePoint);
	dettit.x = 120; dettit.y = 242;
	local nomeZonaDet = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	nomeZonaDet.alpha = 1; dettit.oldAlpha = 1 
	nomeZonaDet.text = ""
	nomeZonaDet:setTextColor(51, 51, 102)
	nomeZonaDet.size = _G.css.defaultFontSize30

	nomeZonaDet:setReferencePoint(display.TopLeftReferencePoint);
	nomeZonaDet.x=120;
	nomeZonaDet.y=282;
	
	lineazonedet = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
	lineazonedet.x = 322; lineazonedet.y = 336; lineazonedet.alpha = 1; lineazonedet.oldAlpha = 1 

	
	fldY = 345;
	fldYdiff=50;
	lblX = 36;
	txtX = 300;
	txtW = 300;
		
	local lbl_client_login;
	local lbl_client_pwd;
	
	local txt_client_login;
	local txt_client_pwd;
	
	local btnPwdRecovery;
		
	local errorToolTipFirstTime=true;
	local validateForm = function()
		errorToolTipFirstTime=false;
		if(_G.ErrorTtp_gr ~= nil) then
			_G.ErrorTtp_gr:removeSelf();
			_G.ErrorTtp_gr = nil;
		end
		local isValid = true;
		lbl_client_login:setTextColor(51, 51, 102);
		if(txt_client_login.text==nil or string.gsub(""..txt_client_login.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_login.y+_G.css.FKB_singleHeigth)
			lbl_client_login:setTextColor(255, 0, 0);
			return false;
		end
		lbl_client_pwd:setTextColor(51, 51, 102);
		if(txt_client_pwd.text==nil or string.gsub(""..txt_client_pwd.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_client_pwd.y+_G.css.FKB_singleHeigth)
			lbl_client_pwd:setTextColor(255, 0, 0);
			return false;
		end
		return isValid;
	end
	formInputCallback = function()
		if(errorToolTipFirstTime==false) then
			validateForm();
		end
	end
	
	local formGroup = display.newGroup() 
	fldY = 350;
	
	fldY = fldY+fldYdiff;
	btnPwdRecovery = getBtn_btnGriggio( "Forgotten your password?", 400, 46, function(event)
			if event.phase=="ended" then  
				local myClosure_switch = function() 
					disposeTweens() 
					director:changeScene( "page_cl_pwdrecovery", "crossfade" ) 
				end 
				timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
			end 
		end);
	btnPwdRecovery:setReferencePoint(display.TopRightReferencePoint);
	btnPwdRecovery.x=600;
	btnPwdRecovery.y=fldY;
	formGroup:insert(btnPwdRecovery)
	
	fldY = fldY+fldYdiff+50;
	lbl_client_login = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lbl_client_login.alpha = 1; lbl_client_login.oldAlpha = 1 
	lbl_client_login.text = "Username:"
	lbl_client_login:setTextColor(51, 51, 102)
	lbl_client_login.size = _G.css.defaultFontSize30
	lbl_client_login:setReferencePoint(display.TopLeftReferencePoint);
	lbl_client_login.x=lblX;
	lbl_client_login.y=fldY;
	formGroup:insert(lbl_client_login) 
	txt_client_login = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, "Username", formInputCallback);

	fldY = fldY+fldYdiff;
	lbl_client_pwd = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lbl_client_pwd.alpha = 1; lbl_client_pwd.oldAlpha = 1 
	lbl_client_pwd.text = "Password:"
	lbl_client_pwd:setTextColor(51, 51, 102)
	lbl_client_pwd.size = _G.css.defaultFontSize30
	lbl_client_pwd:setReferencePoint(display.TopLeftReferencePoint);
	lbl_client_pwd.x=lblX;
	lbl_client_pwd.y=fldY;
	formGroup:insert(lbl_client_pwd) 
	txt_client_pwd = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, "Password", formInputCallback);
	

	local lblErrorAlert = display.newText("", 0,0,600,72, native.systemFontBold, _G.css.defaultFontSize30);
	lblErrorAlert:setTextColor(255, 0, 0)
	lblErrorAlert.text = ""
	lblErrorAlert.size = _G.css.defaultFontSize30
	lblErrorAlert:setReferencePoint(display.CenterCenterReferencePoint);
	lblErrorAlert.x = 325; lblErrorAlert.y = 760; 
	lblErrorAlert.alpha = 1; lblErrorAlert.oldAlpha = 1 
	   
	if(_G.clConfig.lastError~="") then
		lblErrorAlert.text = _G.clConfig.lastError;
		_G.clConfig.lastError = "";
	end
	local btnGroup = display.newGroup() 
	local showHideLoading;
	local oncercaTouch = function(event) 
		if event.phase=="ended" then  
			if(validateForm()==false) then return false; end
			local function networkListenerBooking( event )
				if ( event.isError ) then
						print ( "Network error - download failed: " .. event.response )
				else
					--print ( "response:" .. event.response )
					if(event.response:find("<error>") == nil) then
                        -- returnString += "<client>";
                        -- returnString += "<id>" + _client.id + "</id>";
                        -- returnString += "<name_full>" + _client.name_full + "</name_full>";
                        -- returnString += "<email>" + _client.contact_email + "</email>";
                        -- returnString += "<country>" + _client.loc_country + "</country>";
                        -- returnString += "<contact_phone>" + _client.contact_phone + "</contact_phone>";
                        -- returnString += "</client>";
						currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
						--fillData()
						_G.clConfig.clId = currResponse.child[1].value
						_G.clConfig.clDett["name_full"] = currResponse.child[2].value
						_G.clConfig.clDett["email"] = currResponse.child[3].value
						_G.clConfig.clDett["country"] = currResponse.child[4].value
						_G.clConfig.clDett["contact_phone"] = currResponse.child[5].value
						lblErrorAlert.text = _G.lbl.mobileWelcome..", ".._G.clConfig.clDett["name_full"];
						local myClosure_switch = function() 
							disposeTweens() 
							director:changeScene( "page_".._G.prevPageType, "crossfade" ) 
						end 
						timerStash.newTimer_041 = timer.performWithDelay(2000, myClosure_switch, 1) 
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
						if(_error=="WrongUsernameOrPassword") then
							lblErrorAlert.text=_G.lbl.mobileMsg_WrongUsernameOrPassword;
						else
							lblErrorAlert.text=_error;
						end
						showHideLoading(false);
					end
				end
			end
			local xmlFilter= "";
			xmlFilter=xmlFilter.."&mobilexml=true";
			xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
			xmlFilter=xmlFilter.."&action=login";
			xmlFilter=xmlFilter.."&client_login="..txt_client_login.text;
			xmlFilter=xmlFilter.."&client_pwd="..txt_client_pwd.text;
			xmlFilter=xmlFilter.."";
			xmlFilter=xmlFilter.."";
			local xmlUrl=_G.requestHost.."/webservice/authClientLoginXml.aspx?t="..os.time(os.date( '*t' ));
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
			testoBtnCerca.text = "Login"
			testoBtnCerca.size = _G.css.defaultFontSize50
			testoBtnCerca:setReferencePoint(display.CenterCenterReferencePoint);
			testoBtnCerca.x = 325; testoBtnCerca.y = 905; 
			testoBtnCerca.alpha = 1; testoBtnCerca.oldAlpha = 1 
			btnGroup:insert(testoBtnCerca) 
		end

	end 
	showHideLoading(false);
end
	drawScreen();
   return menuGroup 
end 
