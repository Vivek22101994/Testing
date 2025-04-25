-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 

-- SCROLLVIEW

local widget = require "widget"
imagesArray = {};
lastLoadedImg = 1
local currCount=3;
local totalCount=0;
local listatittxt;
local numerazioneTxt;
local function setImgProps(fotoapt, listItemCont, x, y)
	
	if listItemCont.listItem then 
	fotoapt.alpha = 1; fotoapt.oldAlpha = 1;
	fotoapt.height=95;
	fotoapt.width=185;
	fotoapt:setReferencePoint(display.TopLeftReferencePoint);
	listItemCont.gtween:pause();
	listItemCont.listItem:insert(fotoapt);
	fotoapt.x = defPosX; 
	fotoapt.y = defPosY+50; 
	else
	print("not listItemCont.listItem");
	end
end
local function loadImgIfExists(listItemCont)
	local path = system.pathForFile("apt_img_"..listItemCont.apt_id..".jpg", system.TemporaryDirectory)
	local fhd = io.open( path )
	-- Determine if file exists
	if fhd then
		--fhd.close() <- Produces a Runtime Error. Use io.close( fhd )
		io.close( fhd )
		setImgProps(display.newImageRect("apt_img_"..listItemCont.apt_id..".jpg", system.TemporaryDirectory, 185, 95 ), listItemCont)
		return true;
	else
		return false;
	end
end
local function loadAptImgs(i, yPos)
	if(imagesArray[i]==nil) then return; end
	if(not imagesArray[i].isLoaded) then 
		if(imagesArray[i].showOn < yPos) then return; end
		--print( "- showOn:"..imagesArray[i].showOn);
		imagesArray[i].isLoaded = true;
		lastLoadedImg = i;
		function imgLoaded( event ) 
			if ( event.isError ) then
					print ( "Network error - download failed" )
			else
				if(imagesArray[i].listItem==nil) then return; end
				setImgProps(event.target, imagesArray[i])
				if(i < #imagesArray) then loadAptImgs(i+1, yPos) end
			end
			--print ( "RESPONSE: " .. event.response )
		end
		if loadImgIfExists( imagesArray[i]) then
			if(i < #imagesArray) then loadAptImgs(i+1, yPos) end
		else
			display.loadRemoteImage( imagesArray[i].apt_img, "GET", imgLoaded, "apt_img_"..imagesArray[i].apt_id..".jpg", system.TemporaryDirectory, -2000, -2000);
		end
	else
		if(i < #imagesArray) then loadAptImgs(i+1, yPos) end
	end
end
local function scrollViewListener( event )
	if event.type == "endedScroll" then
		local s = event.target    -- reference to scrollView object
		local xPos, yPos = s:getContentPosition()
		--print( "-endedScroll event type "..yPos .. " ")
		loadAptImgs(lastLoadedImg+1, yPos);
		local x = math.round(((-yPos)+30)/202.5);
		currCount = (x+3)
		numerazioneTxt.text = currCount.."/"..totalCount
	end
end
local scrollView = widget.newScrollView{
	top =0,
    width = 640,
    height = 960,
    scrollWidth = 640,
    scrollHeight = 660,
    maskFile="img_mask-scroll.png",
	--bgColor={0, 0, 0, 0},
	hideBackground=true,
    listener = scrollViewListener
}
scrollView.isHitTestMasked = true;
scrollView.content.horizontalScrollDisabled = true;
-- FINESCROLLVIEW


 function new() 
	if(_G.currPageType ~= "lista_favoriti") then 
		_G.prevPageType = _G.currPageType;
	end
	_G.currPageType = "lista_favoriti";
    local numPages = 5 
    local menuGroup = display.newGroup() 
    local disposeAudios 
    local disposeTweens 

    local curPage = 4 

    local drawScreen = function() 
		getMainMenu(menuGroup);
       local omino1  
       local omino2  
       local mail  
       local favoriteadd  
       local btnbacklisthov  
       local btnbacklist  
       local fotoapt  
       local prezzoaptbg  
       local voto6  
       local listitembghov  
       local listitembg  
       local logo  
	   local dateTopGroup
	   local datebg
       local headbg  
       local background  
	   local scrollbarLista
	   local targhetta
	   
	   local tastoordina  
	   local ordinaIco
	   local tastofiltra 
	   
	   local filtriTxt
	   
	   local selectIco1
	   
	   local ordinaIcoList1
	   local ordinaIcoList2
	   
	   local listIsOpen = false;
	   
	   local btnMap
	   
		getLastSearchDates();
	   
-- INIZIO DELLA LIST ITEM	
loadList_firstLoad = true;
local itemHeight=0
local groupList = {}
local currList;
-- carica da xml
--		<apt_id>1</apt_id>
--		<apt_name>Trevi Fountain View Apartment</apt_name>
--		<apt_vote>10</apt_vote>
--		<apt_img>http://www.rentalinrome.com/romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</apt_img>
--		<zone_id>2</zone_id>
--		<zone_name>Trevi Fountain</zone_name>
--		<pr_total>200,00</pr_total>
local function createList()
	itemHeight=0
	groupList = {}
	totalCount = currList.properties.pagesTotalCount;
-- RIMUOVI QUI IL LOADING
hideLoading();


--(10) TASTO CONTATTI PER PREFERITI
	   
	   	local onbtnContactTouch = function(event) 
          if event.phase=="ended" then  
					local myClosure_switch = function() 
						disposeTweens() 
						director:changeScene( "page_contactus", "moveFromBottom" ) 
					end 
					timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
				end 
       end 
       btnContact = ui.newButton{ 
           defaultSrc=imgDir.."img_btncontactfav.png", 
           defaultX = 90, 
           defaultY =90, 
           overSrc=imgDir.."img_btncontactfav_hov.png", 
           overX = 90, 
           overY = 90, 
           onRelease=onbtnContactTouch, 
           id="btnContactDx" 
       } 
	   btnContact:setReferencePoint(display.BottomRightReferencePoint);
		btnContact.x = 640; btnContact.y = 960; btnContact.alpha = 1; btnContact.oldAlpha = 1 


	for i=1,#currList.child do
		local currTBL = currList.child[i]
		local apt_id = currTBL.child[1].value
		local apt_name = currTBL.child[2].value
		local apt_vote = currTBL.child[3].value; if(apt_vote==nil) then apt_vote = "9"; end
		local apt_img = currTBL.child[4].value; if(apt_img==nil) then apt_img = ""; end
		local zone_id = currTBL.child[5].value; if(zone_id==nil) then zone_id = "0"; end
		local zone_name = currTBL.child[6].value; if(zone_name==nil) then zone_name = "Rome"; end
		local pr_total = currTBL.child[7].value; if(pr_total==nil) then pr_total = "error"; end
		--if i==2 then print("item height:"..itemHeight) end
		local listItem = display.newGroup()
		groupList[i] = listItem
		listItem.x= 335; listItem.y=420+itemHeight;
	   defPosX=-269;
	   defPosY=-77;
--(1) sfondo dell'item list che funge da pulsante 
       local onlistitemTouch = function(event) 
			if event.phase == "moved" then
				local dx = math.abs( event.x - event.xStart )
				local dy = math.abs( event.y - event.yStart )

				if dx > 5 or dy > 5 then
					scrollView:takeFocus( event )
				end
			elseif event.phase == "ended" then
				local myClosure_switch = function()
					--system.openURL( apt_img )
					disposeTweens() 
					_G.clConfig.currEstateID = apt_id;
					director:changeScene( "page_apt_dett", "flip" ) 
				end 
				timerStash.newTimer_478 = timer.performWithDelay(0, myClosure_switch, 1) 
			end
			return true
       end 
	  
       listitembg = ui.newButton{ 
           defaultSrc=imgDir.."img_listitem.png", 
           defaultX = 580, 
           defaultY = 173, 
           overSrc=imgDir.."img_listitemhov.png", 
           overX = 580, 
           overY = 173, 
           --onEvent =onlistitemTouch, 
           id="listitemButton" 
       } 
	   listitembg.width=587;
       listitembg.x = -20; listitembg.y = 0; listitembg.alpha = 1; listitembg.oldAlpha = 1 
       listitembg:addEventListener( "touch", onlistitemTouch )
       listItem:insert(listitembg) 
	   
	   -- Tasto Elimina appartamento dai preferiti
		local onTouchDeleteAptFav = function(event) 
			if event.phase == "ended" then
				selectedEstatesRemove(apt_id);
				if #_G.clConfig.selectedEstates == 0 then
					local myClosure_switch = function()
						disposeTweens() 
						_G.clConfig.currEstateID = apt_id;
						director:changeScene( "page_".._G.prevPageType, "crossfade" ) 
					end 
					timerStash.newTimer_478 = timer.performWithDelay(0, myClosure_switch, 1) 
				else 
					loadList();
				end
			end
		end 
		deleteAptFav = ui.newButton{ 
			defaultSrc=imgDir.."lang_close.png", 
			defaultX = 45, 
			defaultY = 44, 
			overSrc=imgDir.."lang_close.png", 
			overX = 40, 
			overY =39, 
			onRelease=onTouchDeleteAptFav
		}
		deleteAptFav:setReferencePoint(display.TopRightReferencePoint);
		deleteAptFav.x = 260; deleteAptFav.y = -72; deleteAptFav.alpha = 1; deleteAptFav.oldAlpha = 1 
		listItem:insert(deleteAptFav)
		listItem.deleteAptFav = deleteAptFav	

-- (5) nome dell'appartamento
		local nomeApt = display.newText("", 0,0, 568, 30, nil, _G.css.defaultFontSize30);
		nomeApt:setTextColor(253, 102, 52)
		nomeApt.text = apt_name
		nomeApt.size = _G.css.defaultFontSize30
		nomeApt:setReferencePoint(display.TopLeftReferencePoint);
		listItem:insert(nomeApt) 
		
		nomeApt.x=defPosX;
		nomeApt.y=defPosY+6;


--(4) foto appartamento
	local fotoapt = display.newImageRect( imgDir.. "img_loading_imglista.png", 95,95 ); 
	fotoapt.alpha = 1; fotoapt.oldAlpha = 1;
	--fotoapt:setReferencePoint(display.TopLeftReferencePoint);
	listItem:insert(fotoapt);
	fotoapt.x = defPosX+47; 
	fotoapt.y = defPosY+50+47; 
	   
	local currImg = {};
	currImg.listItem=listItem;
	currImg.img=fotoapt;
	currImg.apt_id=apt_id;
	currImg.apt_img=apt_img;
	currImg.gtween = gtween.new( fotoapt, 0.7, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0});
	currImg.showOn = 650-itemHeight;
	currImg.isLoaded = loadImgIfExists(currImg);
	imagesArray[i]=currImg;
	
-- (6) nome della zona
		local nomeZona = display.newText("", 0,0, 330, 60, nil, _G.css.defaultFontSize27);
		nomeZona:setTextColor(51, 51, 102)
		nomeZona.text = zone_name
		nomeZona.size = _G.css.defaultFontSize27
		nomeZona:setReferencePoint(display.TopLeftReferencePoint);
		listItem:insert(nomeZona) 
		nomeZona.x=defPosX+200;
		nomeZona.y=defPosY+50; 

		
--(2) voto dell'appartamento
       voto6 = display.newImageRect( imgDir.. "img_stellina_voto"..apt_vote..".png", 110, 23 ); 
	   voto6.alpha = 1; voto6.oldAlpha = 1 
		voto6:setReferencePoint(display.TopLeftReferencePoint);
		listItem:insert(voto6) 
       voto6.x = defPosX+200;
	   voto6.y = defPosY+123; 
      
--(3) sfondo prezzo appartamento
       prezzoaptbg = display.newImageRect( imgDir.. "img_prezzoaptbg.png", 198, 63 ); 
	   prezzoaptbg.alpha = 1; prezzoaptbg.oldAlpha = 1 
       listItem:insert(prezzoaptbg) 
		prezzoaptbg:setReferencePoint(display.TopLeftReferencePoint);
		prezzoaptbg.x = defPosX+355;
		prezzoaptbg.y = defPosY+110; 
       
		
		-- (7) prezzo dell'appartamento
		local prezzoApt = display.newText("", 0,0,nil, _G.css.defaultFontSize35);
		prezzoApt:setTextColor(255, 255, 255)
		prezzoApt.text = pr_total.. " €"
		prezzoApt.size = _G.css.defaultFontSize35
		prezzoApt:setReferencePoint(display.TopRightReferencePoint);
		listItem:insert(prezzoApt) 
		prezzoApt.x= defPosX+547;
		prezzoApt.y=defPosY+125; 
		
		--listItem:setReferencePoint(display.TopLeftReferencePoint);
	    scrollView:insert( listItem )
		itemHeight=itemHeight+listItem.height+20
		
-- FINE DELLA LIST ITEM	  
	end

	loadAptImgs(1, 0);
	if loadList_firstLoad then
		createListControls();
		loadList_firstLoad = false;
	else
		if tonumber(totalCount)>3 then
			numerazioneTxt.text = "3/"..totalCount
		else
			numerazioneTxt.text = totalCount.."/"..totalCount
		end
	end
end
function createListControls()	
--(2) sfondo targhetta num. risultati
       targhetta = display.newImageRect( imgDir.. "img_targhetta.png", 143, 55 ); 
	   targhetta:setReferencePoint(display.BottomLeftReferencePoint);
       targhetta.x = 25; targhetta.y = 960; targhetta.alpha = 1; targhetta.oldAlpha = 1
	   
--(2) numerazione
		numerazioneTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
		numerazioneTxt.alpha = 1; numerazioneTxt.oldAlpha = 1 
		if tonumber(totalCount)>3 then
			numerazioneTxt.text = "3/"..totalCount
		else
			numerazioneTxt.text = totalCount.."/"..totalCount
		end

		numerazioneTxt:setTextColor(255, 255, 255)
		numerazioneTxt.size = _G.css.defaultFontSize25
		
		numerazioneTxt:setReferencePoint(display.CenterCenterReferencePoint);
		numerazioneTxt.x=98;
		numerazioneTxt.y = 938;
		
--(2) sfondo fascetta titolo
       local titlebg = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
	   titlebg:setReferencePoint(display.CenterCenterReferencePoint);
       titlebg.x = 320; titlebg.y = 275; titlebg.alpha = 1; titlebg.oldAlpha = 1
	
		local onbtnbacklistTouch = function(event) 
          if event.phase=="ended" then  
            local myClosure_switch = function() 
                disposeTweens() 
                director:changeScene( "page_init_search", "crossfade" ) 
            end 
            timerStash.newTimer_449 = timer.performWithDelay(0, myClosure_switch, 1) 
          end 
       end 
--(10) regular layer 
       btnbacklist = ui.newButton{ 
           defaultSrc=imgDir.."img_btnbacklist.png", 
           defaultX = 95, 
           defaultY = 95, 
           overSrc=imgDir.."img_btnbacklisthov.png", 
           overX = 95, 
           overY = 95, 
           onRelease=onbtnbacklistTouch, 
           id="btnbacklistButton" 
       } 

       btnbacklist.x = 57; btnbacklist.y = 275; btnbacklist.alpha = 1; btnbacklist.oldAlpha = 1 
	   --scrollView:insert( btnbacklist ) 
	   
	   	

	
	   
-- TESTO PREFERITI 

	   local titleFavorites = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize35);
       titleFavorites.alpha = 1; titleFavorites.oldAlpha = 1 
	   titleFavorites:setTextColor(254, 102, 52)
	   titleFavorites.size = _G.css.defaultFontSize35
	   titleFavorites.text = _G.lbl.mobilePgPreferitiTitle
	   titleFavorites:setReferencePoint(display.CenterRightReferencePoint);
       titleFavorites.x = 608; titleFavorites.y = 275; 



end 	   
local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			currList = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
			createList()
        end
end
function loadList()
	for i=1,#groupList do
		display.remove( groupList[i] )
	end
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
	xmlFilter=xmlFilter.."&dtS="..JSCal:dateToString( _G.clConfig.dataCheckin);
	xmlFilter=xmlFilter.."&dtE="..JSCal:dateToString( _G.clConfig.dataCheckout);
	xmlFilter=xmlFilter.."&numPers_adult=".._G.clConfig.guestsNum.."&numPers_childOver=0&numPers_childMin=0";
	xmlFilter=xmlFilter.."&currZoneList=0";
	xmlFilter=xmlFilter.."&preflist="..selectedEstatesToString("|");
	xmlFilter=xmlFilter.."";
	xmlFilter=xmlFilter.."";
	xmlFilter=xmlFilter.."";
	xmlFilter=xmlFilter.."&orderBy=".._G.clConfig.orderBy;
	xmlFilter=xmlFilter.."&orderHow=".._G.clConfig.orderHow;
	xmlFilter=xmlFilter.."&title=&currConfigList=&prRange=1|999999&voteRange=0|10&agentID=0&currPage=0&numPerPage=1";
	local xmlUrl=_G.requestHost.."/webservice/rnt_estateListSearch.aspx?t="..os.time(os.date( '*t' ));
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

	scrollView:scrollToTop()
	showLoading();
end
-- se vuoto reindirizza in homepage
if #_G.clConfig.selectedEstates == 0 then
	_G.currPageType = "init_search";
	timerStash.newTimer_042 = timer.performWithDelay(500, function() 
			disposeTweens() 
			director:changeScene( "page_reloader", "crossfade" ) 
		end , 1) 
else -- altrimenti carica normalmente
	loadList();
end
		


   end 
   drawScreen() 

   function disposeTweens(event) 
      cancelAllTweens(); 
      cancelAllTimers(); 
      cancelAllTransitions(); 
   end 

   return menuGroup 
end 
