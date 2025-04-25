-- Book created by Kwik for Adobe Photoshop  - Developed by Kwiksher 
-- Copyright (C) 2011 kwiksher.com. All Rights Reserved. 
-- uses Director class, by Ricardo Rauber 

_G.kwk_readMe = 0 

director = require("director") 
gtween = require("gtween") 
sprite = require("sprite") 
ui = require("ui") 
JSCal = require("inc_JSCal").newJSCal() 
xml = require("inc_xml").newParser()
widget = require "widget"
widget.setTheme("theme_ios")
inc_const_lblIDs = require("inc_const_lblIDs")
inc_css = require("inc_css")
inc_utils = require("inc_utils")

--print(system.getInfo("platformVersion"))

_G.requestHost = "http://www.rentalinrome.com";
--_G.requestHost = "http://localhost:2607";

_G.txtUseLbl=(system.getInfo("environment")=="simulator")
if(_G.txtUseLbl==true and system.getInfo("platformName")=="Mac OS X") then _G.txtUseLbl=false; end
_G.currPageType="splash"

zoneListLang = {
	{ -- it
		{ id=0, title="Ricerca per tutte le zone" }
		,{ id=6, title="Fontana di Trevi" }
		,{ id=2, title="Piazza Navona" }
		,{ id=5, title="Piazza di Spagna" }
		,{ id=1, title="San Pietro" }
		,{ id=4, title="Colosseo" }
		,{ id=3, title="Trastevere" }
		,{ id=17, title="Parioli" }
		,{ id=18, title="Termini" }
		,{ id=16, title="Campo de'Fiori" }
		,{ id=9, title="Colosseo - Olmata" }
		,{ id=19, title="Altre zone" }
	}
	,
	{ -- en
		{ id=0, title="Search all the zones" }
		,{ id=6, title="Trevi Fountain" }
		,{ id=2, title="Piazza Navona" }
		,{ id=5, title="Spanish steps" }
		,{ id=1, title="Saint Peter" }
		,{ id=4, title="Colosseum" }
		,{ id=3, title="Trastevere area" }
		,{ id=17, title="Rome Parioli" }
		,{ id=18, title="Termini station" }
		,{ id=16, title="Campo de'Fiori" }
		,{ id=9, title="Colosseum - Olmata" }
		,{ id=19, title="Other zones" }
	}
};
_G.zoneList = zoneListLang[2];


_G.ordinaList = {
	{ id=1 ,orderBy = "title", lbl="lblName", img_asc="img_ico_ordina_nome-crescente.png", img_desc="img_ico_ordina_nome-decrescente.png" }
	,{ id=2 ,orderBy = "price", lbl="lblPrice", img_asc="img_ico_ordina_prezzo-crescente.png", img_desc="img_ico_ordina_prezzo-decrescente.png" }
	,{ id=3 ,orderBy = "vote", lbl="lblRating", img_asc="img_ico_ordina_voto-crescente.png", img_desc="img_ico_ordina_voto-decrescente.png" }
};

_G.clConfig = {
	langId=2
	,langCode="en"
	,clId = 0
	,clDett = {}
	,searchFirstAccess = true
	,selectedZoneIndex = 1
	,selectedZone = _G.zoneList[1]
	,guestsNum = 2
	,nightsNum = 3
	,minCheckin = os.date( "*t", os.time() + 60*60*24)
	,dataCheckin = os.date( "*t", os.time() + 60*60*24*7)
	,dataCheckout = os.date( "*t", os.time() + 60*60*24*10)
	,orderBy = ""
	,orderHow = ""
	,lastListPosition = 0
	,selectedEstates = {}
	,prevPageType = ""
	,currPageType = ""
	,currEstateID = 0
	,currEstateTB = {}
	,currEstateGallery = {}
	,currEstateIndex = 0
	,lastEstateListIds = {}
	,lastError = ""
	,lastInputText = ""
};
_G.langTBL = {
	{ id=1 ,code = "it", title="Italiano" }
	,{ id=2 ,code = "en", title="English" }
	,{ id=3 ,code = "es", title="Español" }
	,{ id=4 ,code = "fr", title="Français" }
	,{ id=5 ,code = "de", title="Deutsch" }
	,{ id=6 ,code = "ru", title="Русский" }
	,{ id=7 ,code = "nl", title="Dutch" }
	,{ id=8 ,code = "ja", title="Japanese" }
	,{ id=9 ,code = "pt", title="Português" }
	,{ id=10 ,code = "fi", title="Finlandese" }
}
function setLang(lang)
	_G.clConfig.langId = lang;
	local myClosure_switch = function()
		disposeTweens() 
		--director:changeScene( "page_".._G.currPageType, "crossfade" )
		director:changeScene( "page_reloader", "crossfade" )
	end 
	timerStash.newTimer_319 = timer.performWithDelay(500, myClosure_switch, 1)
end
function changeLang()
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
			for i=1,#currResponse.child do
				local device = currResponse.child[i]
				if device.child~=nil and system.getInfo("model") == ""..device.properties.model and (device.properties.platformName=="" or device.properties.platformName==system.getInfo( "platformName" )) and (device.properties.platformVersion=="" or device.properties.platformVersion==system.getInfo( "platformVersion" )) then 
					if #device.child>0 then
						for i=1,#device.child do
							local currRow = device.child[i]
							if(currRow.properties.type=="all") then
								_G.cssChange(currRow.properties.value);
							elseif(currRow.properties.type=="cssDefaultSets") then
								_G.cssDefaultSets(currRow.properties.value);
							elseif(currRow.properties.type~=nil and currRow.properties.type~="") then
								_G.css[currRow.properties.type]=currRow.properties.value;
							end
						end
					end
				end
			end
			changeLangNew();
		end
	end
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
	local xmlUrl=_G.requestHost.."/webservice/mobileCssChange.xml?t="..os.time(os.date( '*t' ));
	local xmlHeaders = {}
	xmlHeaders["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
	xmlHeaders["Accept-Encoding"] = "gzip, deflate";
	xmlHeaders["Accept-Language"] = "it-it,it;q=0.8,en-us;q=0.5,en;q=0.3";
	xmlHeaders["Connection"] = "keep-alive";
	xmlHeaders["Host"] = "www.rentalinrome.com";
	xmlHeaders["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:15.0) Gecko/20100101 Firefox/15.0";
	local xmlParams = {}
	xmlParams.headers = xmlHeaders
	network.request( xmlUrl, "GET", networkListener, xmlParams)
end
function changeLangNew()
	lang = _G.clConfig.langId;
	for i=1,#_G.langTBL do
		local tmp = _G.langTBL[i]
		if(lang==tmp.id)then _G.clConfig.langCode=""..tmp.code; end
	end
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
			_G.lbl = {}
			for i=1,#currResponse.child do
				local lbl = currResponse.child[i]
				--if(""..lbl.child[1].value=="mobileCheckOutDate") then print(""..lbl.child[2].value); end
				_G.lbl[""..lbl.child[1].value] = ""..lbl.child[2].value;
			end
			if(zoneListLang[lang]==nil)then _G.zoneList = zoneListLang[2]; else _G.zoneList = zoneListLang[lang]; end
			_G.zoneList[1].title = _G.lbl.mobileSearchAllZones;
			_G.clConfig.selectedZone = _G.zoneList[_G.clConfig.selectedZoneIndex];
			local myClosure_switch = function()
				disposeTweens()
				director:changeScene( "page_".._G.currPageType, "crossfade" )
			end 
			timerStash.newTimer_319 = timer.performWithDelay(500, myClosure_switch, 1)
		end
	end
	local lblIDsString = "";
	for i=1,#_G.lblIDs do
		lblIDsString = lblIDsString.."|".._G.lblIDs[i];
	end
	
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
	xmlFilter=xmlFilter.."&lbls="..lblIDsString;
	local xmlUrl=_G.requestHost.."/webservice/contLabelXml.aspx?t="..os.time(os.date( '*t' ));
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
	network.request( xmlUrl, "POST", networkListener, xmlParams)
end
	   
	   
	   
display.setStatusBar( display.HiddenStatusBar ) 

function getDatesPaxTxt()
	return _G.lbl.mobileDateFrom .." ".. _G.clConfig.dataCheckin.day .."/".. _G.clConfig.dataCheckin.month .."/".. _G.clConfig.dataCheckin.year .. " ".. _G.lbl.mobileDateTo .. " " .. _G.clConfig.dataCheckout.day .."/".. _G.clConfig.dataCheckout.month .."/".. _G.clConfig.dataCheckout.year.." " .. _G.lbl.mobileFor .. " ".. _G.clConfig.guestsNum .." pax"
end


imgDir = ""
audioDir = "audio/"

-- controlli comuni
local mainMenuSelectedEstatesTxt;
local mainMenuSelectedEstatesBtn;
local mainMenuMail;
-- controlli comuni - End

-- funzioni comuni
local gl_loading;
local gl_loadingTxt;
function showLoading()
	-- native.setActivityIndicator( true )
	-- if(1==1) then return false; end
	gl_loading = display.newImageRect( imgDir.. "loading.png", 200, 200 ); 
	gl_loading:setReferencePoint(display.CenterCenterReferencePoint);
	gl_loading.x = 320; gl_loading.y = 480; gl_loading.alpha = 1; gl_loading.oldAlpha = 1 
	gtStash.gt_loading= gtween.new( gl_loading, 0.5, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0})
	gl_loadingTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize23);
	gl_loadingTxt:setTextColor(105, 95, 111)
	gl_loadingTxt.text = "loading..."
	gl_loadingTxt.size = _G.css.defaultFontSize23;
	gl_loadingTxt:setReferencePoint(display.CenterCenterReferencePoint);
	gl_loadingTxt.x=320;
	gl_loadingTxt.y=480;
end
function showLoadingForm(btnGroup)
	-- native.setActivityIndicator( true )
	-- if(1==1) then return false; end
	gl_loading = display.newImageRect( imgDir.. "loading.png", 120, 120 ); 
	gl_loading:setReferencePoint(display.CenterCenterReferencePoint);
	gl_loading.x = 320; gl_loading.y = 905; gl_loading.alpha = 1; gl_loading.oldAlpha = 1 
	gtStash.gt_loading= gtween.new( gl_loading, 0.5, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0})
	gl_loadingTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize23);
	gl_loadingTxt:setTextColor(105, 95, 111)
	gl_loadingTxt.text = "loading..."
	gl_loadingTxt.size = _G.css.defaultFontSize23;
	gl_loadingTxt:setReferencePoint(display.CenterCenterReferencePoint);
	gl_loadingTxt.x=320;
	gl_loadingTxt.y=905;
	btnGroup:insert(gl_loading)
	btnGroup:insert(gl_loadingTxt)
end
function hideLoading()
	-- timer.performWithDelay( 500,  
		 -- function() 
			 -- native.setActivityIndicator( false )
		 -- end)
	-- if(1==1) then return false; end
	gl_loading:removeSelf();
	gl_loadingTxt:removeSelf();
end
function getBtn_btnGriggio( txt, w, h, listener)
	local gr = display.newGroup() 
	local btn = ui.newButton{ 
		defaultSrc=imgDir.."img_btn_griggio.png", 
		defaultX = w, 
		defaultY = h, 
		overSrc=imgDir.."img_btn_griggio_hov.png", 
		overX = w, 
		overY = h
	} 
	if(listener ~= nil) then
		btn:addEventListener( "touch", listener )
	end
	btn:setReferencePoint(display.TopLeftReferencePoint);
	btn.x = 0; btn.y = 0; btn.alpha = 1; btn.oldAlpha = 1 
	gr:insert(btn) 
	local btnTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
	btnTxt.alpha = 1; btnTxt.oldAlpha = 1 
	btnTxt.text = txt
	btnTxt:setTextColor(51, 51, 102)
	btnTxt.size = _G.css.defaultFontSize25
	btnTxt:setReferencePoint(display.CenterCenterReferencePoint);
	btnTxt.x = w/2; btnTxt.y = h/2;
	gr:insert(btnTxt) 
	return gr;
end
function orderByGetImg() 
	if(_G.clConfig.orderBy=="title") then return _G.ordinaList[1]["img_".._G.clConfig.orderHow]; end
	if(_G.clConfig.orderBy=="price") then return _G.ordinaList[2]["img_".._G.clConfig.orderHow]; end
	if(_G.clConfig.orderBy=="vote") then return _G.ordinaList[3]["img_".._G.clConfig.orderHow]; end
	return "img_ico_ordina_default.png";
end 
function selectedEstatesOpen(event) 
	if event.phase=="ended" then  
		director:changeScene( "page_lista_favoriti", "crossfade" ) 
	end 
end 
function selectedEstatesAddNew(id)
	for i=1,#_G.clConfig.selectedEstates do
		if("".._G.clConfig.selectedEstates[i]==""..id) then return false; end
	end
	_G.clConfig.selectedEstates[#_G.clConfig.selectedEstates+1] = id;
	mainMenuSelectedEstatesTxt.text = #_G.clConfig.selectedEstates
	if #_G.clConfig.selectedEstates == 0 then
		mainMenuSelectedEstatesTxt.alpha=0;
	else 
		mainMenuSelectedEstatesTxt.alpha=1;
		mainMenuSelectedEstatesBtn:addEventListener( "touch", selectedEstatesOpen )
	end
end 
function selectedEstatesRemove(id)
	local tmp = {}
	local tmpCount=1;
	for i=1,#_G.clConfig.selectedEstates do
		if("".._G.clConfig.selectedEstates[i]~=""..id) then 
			tmp[tmpCount] = _G.clConfig.selectedEstates[i];
			tmpCount=tmpCount+1;
		end
	end
	_G.clConfig.selectedEstates = tmp;
	mainMenuSelectedEstatesTxt.text = #_G.clConfig.selectedEstates
	if #_G.clConfig.selectedEstates == 0 then
		mainMenuSelectedEstatesTxt.alpha=0;
	else 
		mainMenuSelectedEstatesTxt.alpha=1;
		mainMenuSelectedEstatesBtn:addEventListener( "touch", selectedEstatesOpen )
	end
end 
function selectedEstatesContains(id)
	for i=1,#_G.clConfig.selectedEstates do
		if("".._G.clConfig.selectedEstates[i]==""..id) then return true; end
	end
	return false;
end 
function selectedEstatesToString(sep)
	if(sep==nil) then sep="|"; end
	local tmpStr="";
	local tmpSep="";
	for i=1,#_G.clConfig.selectedEstates do
		tmpStr=tmpStr..tmpSep.._G.clConfig.selectedEstates[i].."";
		tmpSep=sep;
	end
	return tmpStr;
end 
function getMainMenu(menuGroup)
	--img_bg_empty.png
--(2) regular layer 
       background = display.newImageRect( "img_bg_main.png", 722, 962 ); 
       background.x = 333; background.y = 479; background.alpha = 1; background.oldAlpha = 1 
	   menuGroup:insert(background) 
       menuGroup.background = background 
	   background:addEventListener( "tap", function() native.setKeyboardFocus(nil); end  );

--(2) regular layer 
       headbg = display.newImageRect( "img_bg_head.png", 640, 166 ); 
       headbg.x = 320; headbg.y = 83; headbg.alpha = 1; headbg.oldAlpha = 1 
       menuGroup:insert(headbg) 
       menuGroup.headbg = headbg 

	local goToMain = function(event) 
		if event.phase=="ended" then  
			_G.clConfig.currPageType="selectedEstates";
			director:changeScene( "page_init_search", "flip" ) 
		end 
	end 
       logo = display.newImageRect( imgDir.. "img_head_logo.png", 287, 133 ); 
       logo.x = 155; logo.y = 78; logo.alpha = 1; logo.oldAlpha = 1 
       menuGroup:insert(logo) 
       menuGroup.logo = logo 
	logo:addEventListener( "touch", goToMain )
	
	
--	il cliente loggato
	if(_G.clConfig.clId ~= 0 and _G.clConfig.clDett["name_full"] ~= nil and _G.clConfig.clDett["name_full"] ~= "") then 
		local txtWelcome = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize22);
		txtWelcome.alpha = 1; txtWelcome.oldAlpha = 1 
		txtWelcome.text = _G.lbl.mobileWelcome..","
		txtWelcome:setTextColor(255, 255, 255)
		txtWelcome.size = _G.css.defaultFontSize22
		txtWelcome:setReferencePoint(display.TopRightReferencePoint);
		txtWelcome.x=545;
		txtWelcome.y=3;
		menuGroup:insert(txtWelcome) 
		local txtNomeCliente = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
		txtNomeCliente.alpha = 1; txtNomeCliente.oldAlpha = 1 
		txtNomeCliente.text = _G.clConfig.clDett["name_full"]
		txtNomeCliente:setTextColor(253, 102, 52)
		txtNomeCliente.size = _G.css.defaultFontSize27
		txtNomeCliente:setReferencePoint(display.TopRightReferencePoint);
		txtNomeCliente.x=543;
		txtNomeCliente.y=25;
		menuGroup:insert(txtNomeCliente) 
	end	
	if(_G.currPageType == "lista_favoriti") then  -- se gia stai nella pagina preferiti
		mainMenuSelectedEstatesBtn = display.newImageRect( imgDir.. "img_head_favoriteadd_active.png", 76, 73 ); 
		mainMenuSelectedEstatesBtn.x = 577; mainMenuSelectedEstatesBtn.y = 101; mainMenuSelectedEstatesBtn.alpha = 1; mainMenuSelectedEstatesBtn.oldAlpha = 1 
		menuGroup:insert(mainMenuSelectedEstatesBtn) 

		mainMenuSelectedEstatesTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize22);
		mainMenuSelectedEstatesTxt.alpha = 1; 
		mainMenuSelectedEstatesTxt.oldAlpha = 1 
		mainMenuSelectedEstatesTxt.text = #_G.clConfig.selectedEstates
		mainMenuSelectedEstatesTxt:setTextColor(253, 102, 52)
		mainMenuSelectedEstatesTxt.size = _G.css.defaultFontSize22
		
		mainMenuSelectedEstatesTxt:setReferencePoint(display.TopCenterReferencePoint);
		mainMenuSelectedEstatesTxt.x=577;
		mainMenuSelectedEstatesTxt.y=90;
		
		if #_G.clConfig.selectedEstates == 0 then
			mainMenuSelectedEstatesTxt.alpha=0;
		else 
			mainMenuSelectedEstatesTxt.alpha=1;
		end
	   menuGroup:insert(mainMenuSelectedEstatesTxt) 
	else
		if(_G.currPageType == "contactus" and #_G.clConfig.selectedEstates ~= 0) then  -- se stai nella pagina contatti ed hai i preferiti
		local evidenziaPref = display.newImageRect( imgDir.. "img_evidenzia_contatti.png", 92, 270 ); 
		evidenziaPref:setReferencePoint(display.TopRightReferencePoint);	
		evidenziaPref.x = 625; evidenziaPref.y = 59; evidenziaPref.alpha = 1; evidenziaPref.oldAlpha = 1 
		--menuGroup:insert(evidenziaPref)
			mainMenuSelectedEstatesBtn = display.newImageRect( imgDir.. "img_head_favoriteadd_active.png", 76, 73 ); 
		else
			mainMenuSelectedEstatesBtn = display.newImageRect( imgDir.. "img_head_favoriteadd.png", 76, 73 ); 
		end
		mainMenuSelectedEstatesBtn.x = 577; mainMenuSelectedEstatesBtn.y = 101; mainMenuSelectedEstatesBtn.alpha = 1; mainMenuSelectedEstatesBtn.oldAlpha = 1 
		menuGroup:insert(mainMenuSelectedEstatesBtn) 
		if (#_G.clConfig.selectedEstates ~= 0) then
			mainMenuSelectedEstatesBtn:addEventListener( "touch", selectedEstatesOpen )
		end

		mainMenuSelectedEstatesTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize22);
		mainMenuSelectedEstatesTxt.alpha = 1; 
		mainMenuSelectedEstatesTxt.oldAlpha = 1 
		mainMenuSelectedEstatesTxt.text = #_G.clConfig.selectedEstates
		mainMenuSelectedEstatesTxt:setTextColor(253, 102, 52)
		mainMenuSelectedEstatesTxt.size = _G.css.defaultFontSize22
		
		mainMenuSelectedEstatesTxt:setReferencePoint(display.TopCenterReferencePoint);
		mainMenuSelectedEstatesTxt.x=577;
		mainMenuSelectedEstatesTxt.y=90;
		
		if #_G.clConfig.selectedEstates == 0 then
			mainMenuSelectedEstatesTxt.alpha=0;
		else 
			mainMenuSelectedEstatesTxt.alpha=1;
		end
		menuGroup:insert(mainMenuSelectedEstatesTxt) 

	end
	
--(2) regular layer 
	if(_G.currPageType == "contactus") then -- se gia stai nella pagina contatti
		mainMenuMail = display.newImageRect( imgDir.. "img_head_mail_active.png", 81, 59 ); 
		mainMenuMail.x = 490; mainMenuMail.y = 106; mainMenuMail.alpha = 1; mainMenuMail.oldAlpha = 1 
		menuGroup:insert(mainMenuMail) 
		mainMenuMail:addEventListener( "touch", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_deviceinfo", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	else
		mainMenuMail = display.newImageRect( imgDir.. "img_head_mail.png", 81, 59 ); 
		mainMenuMail.x = 490; mainMenuMail.y = 106; mainMenuMail.alpha = 1; mainMenuMail.oldAlpha = 1 
		menuGroup:insert(mainMenuMail) 
		mainMenuMail:addEventListener( "touch", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_contactus", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end

	if(_G.currPageType == "cl_login") then -- se gia stai nella pagina login
		mainLoginBtn = display.newImageRect( imgDir.. "img_head_login_active.png", 67, 79 ); 
		mainLoginBtn.x = 390; mainLoginBtn.y = 106; mainLoginBtn.alpha = 1; mainLoginBtn.oldAlpha = 1 
		menuGroup:insert(mainLoginBtn) 
	else
		
		local omino1 = display.newImageRect( imgDir.. "img_head_omino1.png", 67, 79 ); 
		omino1.x = 390; omino1.y = 110; omino1.alpha = 1; omino1.oldAlpha = 1 
		menuGroup:insert(omino1) 
		omino1:addEventListener( "touch", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_cl_login", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end



	local langswitchGroup = display.newGroup()
	langswitchGroup.x=555; langswitchGroup.y=0;

	local langswitchBtn
	local currentLangFlag

	local langsGroup = display.newGroup()
	langsGroup.x=3; langsGroup.y=-260
       menuGroup:insert(langswitchGroup) 
       menuGroup:insert(langsGroup) 
	   
	   local closeLang

-- Sfondo del selettore della lingua

		local onlangswitchTouch = function(event) 
          if event.phase=="ended" then
			gtStash.langSelDrop= gtween.new( langsGroup, 0.5, {y = 0, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});
          end 
       end 

       langswitchBtn = ui.newButton{ 
           defaultSrc=imgDir.."lang_sel_bg.png", 
           defaultX = 90, 
           defaultY = 50, 
           overSrc=imgDir.."lang_sel_bg_hov.png", 
           overX = 90, 
           overY =50, 
           onRelease=onlangswitchTouch, 
           id="langswitchButton" 
       } 
	langswitchBtn:setReferencePoint(display.TopLeftReferencePoint);
       langswitchBtn.x = 0; langswitchBtn.y = 0; langswitchBtn.alpha = 1; langswitchBtn.oldAlpha = 1 
       langswitchGroup:insert(langswitchBtn) 
       langswitchGroup.langswitchBtn = langswitchBtn
	   
--(2) BANDIERINA LINGUA CORRENTE

       currentLangFlag = display.newImageRect( imgDir.. "img_flag_".._G.clConfig.langCode..".png", 55, 40 ); 
	   currentLangFlag:setReferencePoint(display.TopLeftReferencePoint);
       currentLangFlag.x = 7; currentLangFlag.y = 4; currentLangFlag.alpha = 1; currentLangFlag.oldAlpha = 1 
       langswitchGroup:insert(currentLangFlag) 
       langswitchGroup.currentLangFlag = currentLangFlag 
	   
-- FASCIA DELLE LINGUE (BANDIERINE)

-- Sfondo delle bandierine
	   -- mettiamo un pulsante per evitare il click degli oggetti dietro lo sfondo
		langsBg = ui.newButton{ 
		   defaultSrc="langs_bg.png", 
		   defaultX = 633, 
		   defaultY = 164, 
		   overSrc="langs_bg.png", 
		   overX = 633, 
		   overY = 164
		} 
	   langsBg:addEventListener( "touch",  returnFalseEvent )
	   langsBg:setReferencePoint(display.TopLeftReferencePoint);
       langsBg.x = 0; langsBg.y = 0; langsBg.alpha = 1; langsBg.oldAlpha = 1 
       langsGroup:insert(langsBg) 
       langsGroup.langsBg = langsBg

-- Tasto Chiudi
		local onTouchCloseLang = function(event) 
					if event.phase=="ended" then
						gtStash.langSelDropBack= gtween.new( langsGroup, 0.8, {y = -260, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});
					end 
				end 
				closeLang = ui.newButton{ 
					defaultSrc=imgDir.."lang_close.png", 
					defaultX = 38, 
					defaultY = 37, 
					overSrc=imgDir.."lang_close.png", 
					overX = 33, 
					overY =32, 
					onRelease=onTouchCloseLang
				}
				closeLang:setReferencePoint(display.CenterCenterReferencePoint);
				closeLang.x = 608; closeLang.y = 140; closeLang.alpha = 1; closeLang.oldAlpha = 1 
				langsGroup:insert(closeLang)
				langsGroup.closeLang = closeLang				
	   
-- BANDIERINE
		for i=1,#currResponse.child do
			local lbl = currResponse.child[i]
			_G.lbl[""..lbl.child[1].value] = ""..lbl.child[2].value;
		end
		for i=1,#_G.langTBL do
			local tmp = _G.langTBL[i]
			-- { id=1 ,code = "it", title="Italiano" }
			if(i<11) then
				
				local y=46;
				local x=88;
				if(i>5) then y=112; end
				if(i==2 or i==7) then x=x+112; end
				if(i==3 or i==8) then x=x+((112)*2); end
				if(i==4 or i==9) then x=x+((112)*3); end
				if(i==5 or i==10) then x=x+((112)*4); end
				
				local onTouch = function(event) 
					if event.phase=="ended" then
						setLang(tmp.id);
						gtStash.langSelDropBack= gtween.new( langsGroup, 0.8, {y = -164, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});
					end 
				end 
				flag = ui.newButton{ 
					defaultSrc=imgDir.."img_flag_"..tmp.code..".png", 
					defaultX = 71, 
					defaultY = 52, 
					overSrc=imgDir.."img_flag_"..tmp.code..".png", 
					overX = 60, 
					overY =44, 
					onRelease=onTouch
				}
				flag.x = x; flag.y = y; flag.alpha = 1; flag.oldAlpha = 1 
				langsGroup:insert(flag) 
			end
		end
-- FINE FASCIA E SELEZIONE LINGUE
end
returnFalseEvent = function(event) 
	return false;
end 
-- funzioni comuni - End
function getLastSearchDates()
	--(2) GRUPPO FASCETTA DATE CHECK IN E CHECK OUT
	local dateTopGroup = display.newGroup() 
	--(2) sfondo fascetta per le date
	datebg = display.newImageRect( imgDir.. "img_datebg.png", 648, 62 ); 
	datebg.x = 320; datebg.y = 192; datebg.alpha = 1; datebg.oldAlpha = 1
	dateTopGroup:insert(datebg) 

	-- (6) testo fascetta date

	local datesTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
	datesTxt.alpha = 1; datesTxt.oldAlpha = 1 
	datesTxt.text =  getDatesPaxTxt();
	datesTxt:setTextColor(255, 255, 255)
	datesTxt.size = _G.css.defaultFontSize25

	datesTxt:setReferencePoint(display.TopLeftReferencePoint);
	datesTxt.x=15;
	datesTxt.y=176;
	dateTopGroup:insert(datesTxt)
end
function getBackBtn(pageType,effect,x,y)
	local pageType = pageType or "apt_dett";
	local effect = effect or "moveFromLeft";
	local x = x or 60;
	local y = y or 280;
	local onbtnbackdetTouch = function(event) 
		if event.phase=="ended" then  
			local myClosure_switch = function() 
			disposeTweens() 
			director:changeScene( "page_"..pageType, effect ) 
		end 
			timerStash.newTimer_909 = timer.performWithDelay(0, myClosure_switch, 1) 
		end 
	end 
	btnbackdet = ui.newButton{ 
		defaultSrc=imgDir.."img_btnbackdet.png", 
		defaultX = 124, 
		defaultY = 124, 
		overSrc=imgDir.."img_btnbackdethov.png", 
		overX = 124, 
		overY = 124, 
		onRelease=onbtnbackdetTouch, 
		id="btnbackdetButton" 
	} 
	btnbackdet.width=85; btnbackdet.height=85;
	btnbackdet.x = x; btnbackdet.y = y; btnbackdet.alpha = 1; btnbackdet.oldAlpha = 1 	   
end
function getAptAddToFav()
	if(not selectedEstatesContains(_G.clConfig.currEstateID)) then
		local favGroup = display.newGroup() 
			local onbtnaddtofavTouch = function(event) 
			if event.phase=="ended" then  
				gtStash.gt_frecciaaddfav_202:pause()
				transitionStash.newTransition_198 = transition.to( frecciaaddfav, {alpha=0, y=100, time=500, delay=0}) 
				transitionStash.newTransition_199 = transition.to( btnaddtofav, {alpha=1, xScale = 1, yScale = 1, x=1200, time=900, delay=500}) 
				transitionStash.newTransition_200 = transition.to( btnaddtofavhov, {alpha=1, xScale = 1, yScale = 1, x=1200, time=900, delay=500})
				transitionStash.favplus = transition.to( favoriteadd, {xScale = 1.3, yScale = 1.3, time=200, delay=0})	
				transitionStash.favplusBack = transition.to( favoriteadd, {xScale = 1, yScale = 1, time=300, delay=200})						
				if gtStash.gt_frecciaaddfav_202 then 
					gtStash.gt_frecciaaddfav_202=null 
				end 
				selectedEstatesAddNew(_G.clConfig.currEstateID)
			end 
		end 
		btnaddtofav = ui.newButton{ 
           defaultSrc=imgDir.."img_btnaddtofav.png", 
           defaultX = 80, 
           defaultY = 67, 
           overSrc=imgDir.."img_btnaddtofavhov.png", 
           overX = 80, 
           overY =67, 
           onRelease=onbtnaddtofavTouch, 
           id="btnaddtofavButton" 
		} 

		btnaddtofav.x = 578; btnaddtofav.y = 214; btnaddtofav.alpha = 1; btnaddtofav.oldAlpha = 1 
		favGroup:insert(btnaddtofav)


       frecciaaddfav = display.newImageRect( imgDir.. "img_frecciaaddfav.png", 41, 52 ); 
       frecciaaddfav.x = 578; frecciaaddfav.y = 172; frecciaaddfav.alpha = 1; frecciaaddfav.oldAlpha = 1 
	   favGroup:insert(frecciaaddfav) 
       gtStash.gt_frecciaaddfav_202= gtween.new( frecciaaddfav, 0.3, { x = 578, y = 167, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = true,  delay=0}) 
	end
end
function getAptHeader()
	local currTBL = _G.clConfig.currEstateTB;
	local apt_name = currTBL.child[2].value
	local apt_vote = currTBL.child[3].value; if(apt_vote==nil) then apt_vote = "9"; end
	local zone_name = currTBL.child[6].value; if(zone_name==nil) then zone_name = "Rome"; end
	--(2) nome dell'appartamento
	local dettit = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize35);
	dettit.alpha = 1; dettit.oldAlpha = 1 
	dettit.text = apt_name
	dettit:setTextColor(253, 102, 52)
	dettit.size = _G.css.defaultFontSize35
	dettit:setReferencePoint(display.TopLeftReferencePoint);
	dettit.x = 120; dettit.y = 242;
	if(dettit.width > display.contentWidth-150) then dettit.width = display.contentWidth-150; dettit.text = apt_name; end

	local nomeZonaDet = display.newText("", 0, 0, 300,0, native.systemFontBold, _G.css.defaultFontSize30);
	nomeZonaDet.alpha = 1; dettit.oldAlpha = 1 
	nomeZonaDet.text = zone_name
	nomeZonaDet:setTextColor(51, 51, 102)
	nomeZonaDet.size = _G.css.defaultFontSize30

	nomeZonaDet:setReferencePoint(display.TopLeftReferencePoint);
	nomeZonaDet.x=120;
	nomeZonaDet.y=282;

	--(2) voto dell'appartamento
	voto = display.newImageRect( imgDir.. "img_stellina_voto"..apt_vote..".png", 130, 28 ); 
	voto:setReferencePoint(display.TopRightReferencePoint);
	voto.x = 605; voto.y = 297; voto.alpha = 1; voto.oldAlpha = 1 

	--(2) regular layer 
	lineazonedet = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
	lineazonedet.x = 322; lineazonedet.y = 336; lineazonedet.alpha = 1; lineazonedet.oldAlpha = 1 
end
function getAptFooter()
	local currTBL = _G.clConfig.currEstateTB;
	local videoFilePath = currTBL.child[9].value; if(videoFilePath==nil) then videoFilePath = ""; end
	local has_comments = currTBL.child[10].value; if(has_comments==nil) then has_comments = "0"; end
	local currX=30;
	local btnMargin=15;
	local currGroup = display.newGroup() 
	local function addBtn(txt, width, img, hov, listener)
		local btn = ui.newButton{ 
			defaultSrc=img, 
			defaultX = width, 
			defaultY = 53, 
			overSrc=hov, 
			overX = width, 
			overY = 53
		} 
		btn:setReferencePoint(display.TopLeftReferencePoint);
		btn.x = currX; btn.y = 0; btn.alpha = 1; btn.oldAlpha = 1 
		currGroup:insert(btn)
		if(listener == nil) then
			--non fa nulla
		else
			btn:addEventListener( "touch", listener )
		end
		-- local btnTxt = display.newText( "", 25, 25, 568, 84, native.systemFont, 23 )
		-- btnTxt.alpha = 1; btnTxt.oldAlpha = 1 
		-- btnTxt.text = txt
		-- btnTxt:setTextColor(51, 51, 102)
		-- btnTxt.size = 23
		-- btnTxt:setReferencePoint(display.TopLeftReferencePoint);
		-- btnTxt.x=currX+45;
		-- btnTxt.y=783;
		currX = currX + width + btnMargin;
	end
	if(_G.currPageType == "apt_extrainfo") then
		addBtn(_G.lbl.mobileBtnExtrainfo, 88, "img_apt_btn_info_hov.png", "img_apt_btn_info_hov.png", nil)
	else
		addBtn(_G.lbl.mobileBtnExtrainfo, 88, "img_apt_btn_info.png", "img_apt_btn_info_hov.png", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_apt_extrainfo", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end
	if(_G.currPageType == "apt_price") then
		addBtn(_G.lbl.mobilePriceRates, 88, "img_apt_btn_prices_hov.png", "img_apt_btn_prices_hov.png", nil)
	else
		addBtn(_G.lbl.mobilePriceRates, 88, "img_apt_btn_prices.png", "img_apt_btn_prices_hov.png", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_apt_price", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end
	addBtn(_G.lbl.mobileBtnPhotos, 88, "img_apt_btn_photos.png", "img_apt_btn_photos_hov.png", function(event)
			if event.phase=="ended" then  
				local myClosure_switch = function() 
					disposeTweens() 
					director:changeScene( "page_apt_gallery", "moveFromRight" ) 
				end 
				timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
			end 
		end);
	if(system.getInfo("platformName")=="iPhone OS") then
		addBtn(_G.lbl.mobileBtnGMap, 88, "img_apt_btn_map.png", "img_apt_btn_map_hov.png", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_apt_mappa", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end
	if(videoFilePath~="") then
		addBtn(_G.lbl.mobileBtnVideo, 88, "img_apt_btn_video.png", "img_apt_btn_video_hov.png", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_apt_video", "moveFromRight" ) 
					end 
					--timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
					media.playVideo(videoFilePath, media.RemoteSource, true);
				end 
			end);
	end
	if(_G.currPageType == "apt_guestbook") then
		addBtn(_G.lbl.mobileBtnGuestbook, 88, "img_apt_btn_guestbook_hov.png", "img_apt_btn_guestbook_hov.png", nil)
	elseif(has_comments=="1") then
		addBtn(_G.lbl.mobileBtnGuestbook, 88, "img_apt_btn_guestbook.png", "img_apt_btn_guestbook_hov.png", function(event)
				if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_apt_guestbook", "moveFromRight" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
			end);
	end

	currGroup:setReferencePoint(display.TopCenterReferencePoint);
	currGroup.y = 770
	currGroup.x = display.contentWidth/2


	local oncercaTouch = function(event) 
		if event.phase=="ended" then  
			local myClosure_switch = function() 
				disposeTweens() 
				director:changeScene( "page_apt_book", "crossfade" ) 
			end 
			timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
		end 
	end 
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
	   
	   local testoBtnCerca = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize50);
		testoBtnCerca:setTextColor(255, 255, 255)
		testoBtnCerca.text = "BOOK NOW!"
		testoBtnCerca.size = _G.css.defaultFontSize50;
		testoBtnCerca:setReferencePoint(display.CenterCenterReferencePoint);
       testoBtnCerca.x = 325; testoBtnCerca.y = 905; 
       testoBtnCerca.alpha = 1; testoBtnCerca.oldAlpha = 1 
end


-- Create a main group
local mainGroup = display.newGroup()
local initPage = "splash"


-- Main function
local function main()
	-- Add the group from director class
	mainGroup:insert(director.directorView)
	director:changeScene( "page_"..initPage )
	return true
end

--Clear timers and transitions
timerStash = {}
transitionStash = {}
gtStash = {}

function cancelAllTimers()
    local k, v

    for k,v in pairs(timerStash) do
        timer.cancel( v )
        v = nil; k = nil
    end

    timerStash = nil
    timerStash = {}
end

--

function cancelAllTransitions()
    local k, v

    for k,v in pairs(transitionStash) do
        transition.cancel( v )
        v = nil; k = nil
    end

    transitionStash = nil
    transitionStash = {}
end

--cancel all gtweens
function cancelAllTweens()
    local k, v

    for k,v in pairs(gtStash) do
        v:pause();
        v = nil; k = nil
    end

    gtStash = nil
    gtStash = {}
end

disposeTweens = function (event) 
  cancelAllTweens(); 
  cancelAllTimers(); 
  cancelAllTransitions(); 
end 


-- Begin
main()