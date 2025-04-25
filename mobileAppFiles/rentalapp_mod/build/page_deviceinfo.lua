-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.prevPageType = _G.currPageType;
	_G.currPageType = "deviceinfo";
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
		local dettit = display.newText("", 0,0, native.systemFontBold, 20);
		dettit.alpha = 1; dettit.oldAlpha = 1 
		dettit.text = "Device info"
		dettit:setTextColor(253, 102, 52)
		dettit.size = 35
		dettit:setReferencePoint(display.TopLeftReferencePoint);
		dettit.x = 120; dettit.y = 242;
		local nomeZonaDet = display.newText("", 0,0, native.systemFontBold, 12);
		nomeZonaDet.alpha = 1; dettit.oldAlpha = 1 
		nomeZonaDet.text = "Send us your device info to report bug"
		nomeZonaDet:setTextColor(51, 51, 102)
		nomeZonaDet.size = 30

		nomeZonaDet:setReferencePoint(display.TopLeftReferencePoint);
		nomeZonaDet.x=120;
		nomeZonaDet.y=282;
		
		lineazonedet = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
		lineazonedet.x = 322; lineazonedet.y = 336; lineazonedet.alpha = 1; lineazonedet.oldAlpha = 1 

		local tmpStr="";
		--tmpStr=tmpStr.."name=>"..system.getInfo( "name" ).."\n";
		tmpStr=tmpStr.."model=>"..system.getInfo( "model" ).."\n";
		--tmpStr=tmpStr.."deviceID=>"..system.getInfo( "deviceID" ).."\n";
		tmpStr=tmpStr.."environment=>"..system.getInfo( "environment" ).."\n";
		tmpStr=tmpStr.."platformName=>"..system.getInfo( "platformName" ).."\n";
		tmpStr=tmpStr.."platformVersion=>"..system.getInfo( "platformVersion" ).."\n";
		tmpStr=tmpStr.."build=>"..system.getInfo( "build" ).."\n";
		tmpStr=tmpStr.."textureMemoryUsed=>"..system.getInfo( "textureMemoryUsed" ).."\n";
		tmpStr=tmpStr.."maxTextureSize=>"..system.getInfo( "maxTextureSize" ).."\n";
		tmpStr=tmpStr.."architectureInfo=>"..system.getInfo( "architectureInfo" ).."\n";
		-- tmpStr=tmpStr.."name=>"..system.getInfo( "name" ).."\n";
		-- tmpStr=tmpStr.."name=>"..system.getInfo( "name" ).."\n";
		-- tmpStr=tmpStr.."name=>"..system.getInfo( "name" ).."\n";

		local txtDeviceInfo = display.newText( "", 25, 25, 605, 0, native.systemFont, 23 )
		txtDeviceInfo.alpha = 1; txtDeviceInfo.oldAlpha = 1 
		txtDeviceInfo.text = tmpStr
		txtDeviceInfo:setTextColor(102, 102, 102)
		txtDeviceInfo.size = 23
		txtDeviceInfo:setReferencePoint(display.TopLeftReferencePoint);
		txtDeviceInfo.x=25;
		txtDeviceInfo.y=345;
	local btnGroup = display.newGroup() 
	local showHideLoading;
	   
	   local lblErrorAlert = display.newText("", 0,0,600,72, native.systemFontBold, 30);
		lblErrorAlert:setTextColor(255, 0, 0)
		lblErrorAlert.text = ""
		lblErrorAlert.size = 30
		lblErrorAlert:setReferencePoint(display.CenterCenterReferencePoint);
       lblErrorAlert.x = 325; lblErrorAlert.y = 760; 
       lblErrorAlert.alpha = 1; lblErrorAlert.oldAlpha = 1 

	local oncercaTouch = function(event) 
		if event.phase=="ended" then  
			local function networkListenerBooking( event )
				if ( event.isError ) then
					print ( "Network error - download failed: " .. event.response )
					lblErrorAlert.text="There was an error on sending your mail.\nPlease retry later.";
				else
					if(event.response == "ok") then
						lblErrorAlert.text="We have received your request.\nYou will be contacted by our staff soon.";
					else
						lblErrorAlert.text="There was an error on sending your mail.\nPlease retry later.";
					end
				end
				btnGroup:removeSelf();
				timerStash.newTimer_319 = timer.performWithDelay(3000, function()
						disposeTweens()
						director:changeScene( "page_".._G.prevPageType, "crossfade" )
					end , 1)
			end
			local xmlFilter= "";
			xmlFilter=xmlFilter.."&mobilexml=true";
			xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
			xmlFilter=xmlFilter.."&deviceinfo=true";
			xmlFilter=xmlFilter.."&special_request="..tmpStr;
			
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
			loading = display.newImageRect( imgDir.. "loading.png", 120, 120 ); 
			loading:setReferencePoint(display.CenterCenterReferencePoint);
			loading.x = 319; loading.y = 905; loading.alpha = 1; loading.oldAlpha = 1 
			btnGroup:insert( loading )
			gtStash.gt_loading= gtween.new( loading, 0.7, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0})

			loadingTxt = display.newText("", 0,0, native.systemFontBold, 23);
			loadingTxt:setTextColor(105, 95, 111)
			loadingTxt.text = "loading..."
			loadingTxt.size = 23
			loadingTxt:setReferencePoint(display.CenterCenterReferencePoint);
			loadingTxt.x=325;
			loadingTxt.y=905;
			btnGroup:insert(loadingTxt)
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

			local testoBtnCerca = display.newText("", 0,0, native.systemFontBold, 50);
			testoBtnCerca:setTextColor(255, 255, 255)
			testoBtnCerca.text = "Submit"
			testoBtnCerca.size = 50
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
