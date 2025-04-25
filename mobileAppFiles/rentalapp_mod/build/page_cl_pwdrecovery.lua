-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "cl_pwdrecovery";
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
	dettit.text = "Password recovery"
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
		
	local lbl_recovery_login;
	local txt_recovery_login;
	local btnGoToLogin;
		
	local errorToolTipFirstTime=true;
	local validateForm = function()
		errorToolTipFirstTime=false;
		if(_G.ErrorTtp_gr ~= nil) then
			_G.ErrorTtp_gr:removeSelf();
			_G.ErrorTtp_gr = nil;
		end
		local isValid = true;
		lbl_recovery_login:setTextColor(51, 51, 102);
		if(txt_recovery_login.text==nil or string.gsub(""..txt_recovery_login.text , "%s", "")=="") then
			_G.ErrorTtp_show(_G.lbl.mobileRequiredField,txt_recovery_login.y+_G.css.FKB_singleHeigth)
			lbl_recovery_login:setTextColor(255, 0, 0);
			return false;
		elseif( Allen.isEmail(""..txt_recovery_login.text)==false ) then
			_G.ErrorTtp_show(_G.lbl.mobileInvalidEmailFormat,txt_recovery_login.y+_G.css.FKB_singleHeigth)
			lbl_recovery_login:setTextColor(255, 0, 0);
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
	btnGoToLogin = getBtn_btnGriggio( "Go to Login", 400, 46, function(event)
			if event.phase=="ended" then  
				local myClosure_switch = function() 
					disposeTweens() 
					director:changeScene( "page_cl_login", "crossfade" ) 
				end 
				timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
			end 
		end);
	btnGoToLogin:setReferencePoint(display.TopRightReferencePoint);
	btnGoToLogin.x=600;
	btnGoToLogin.y=fldY;
	formGroup:insert(btnGoToLogin)

	fldY = fldY+fldYdiff+50;
	lbl_recovery_login = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lbl_recovery_login.alpha = 1; lbl_recovery_login.oldAlpha = 1 
	lbl_recovery_login.text = "E-mail:"
	lbl_recovery_login:setTextColor(51, 51, 102)
	lbl_recovery_login.size = _G.css.defaultFontSize30
	lbl_recovery_login:setReferencePoint(display.TopLeftReferencePoint);
	lbl_recovery_login.x=lblX;
	lbl_recovery_login.y=fldY;
	formGroup:insert(lbl_recovery_login) 
	txt_recovery_login = _G.FKB_createTextInput(txtX, fldY, txtW, _G.css.FKB_singleHeigth, "", formGroup, "E-mail", formInputCallback);
	
	

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
					if(event.response == "ok") then
						lblErrorAlert.text="Password was sent to your e-mail.";
						local myClosure_switch = function() 
							disposeTweens() 
							director:changeScene( "page_cl_login", "crossfade" ) 
						end 
						timerStash.newTimer_041 = timer.performWithDelay(2000, myClosure_switch, 1) 
						formGroup:removeSelf();
						btnGroup:removeSelf();
					else
						--<error>NotExist</error>
						--<error>Disabled</error>
						--<error>MailError</error>
						local _error = event.response:gsub( "<error>", ""):gsub( "</error>", "");
						if(_error=="NotExist") then
							lblErrorAlert.text="No user i registered with this E-mail.";
						elseif(_error=="MailError") then
							lblErrorAlert.text="An error occured while sending the e-mail.\nPlease try again later.";
							local myClosure_switch = function() 
								disposeTweens() 
								director:changeScene( "page_cl_login", "crossfade" ) 
							end 
							timerStash.newTimer_041 = timer.performWithDelay(2000, myClosure_switch, 1) 
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
			xmlFilter=xmlFilter.."&action=recover";
			xmlFilter=xmlFilter.."&recovery_login="..txt_recovery_login.text;
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
			testoBtnCerca.text = "Send"
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
