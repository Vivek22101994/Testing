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
			elseif(_G.currPageType == "lista_search") then
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
	_G.currPageType = "lista_search";
    local numPages = 5 
    local menuGroup = display.newGroup() 

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
	   local datesTxt
       local headbg  
       local background  
	   local scrollbarLista
	   local targhetta

	   local loadingDesc
	   
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
loadingDesc:removeSelf();


--(10) TASTO MAPPA APPARTAMENTI
	   
	   	-- local onbtnMapTouch = function(event) 
          -- if event.phase=="ended" then  
          
          -- end 
       -- end 
       -- btnMap = ui.newButton{ 
           -- defaultSrc=imgDir.."map_btn.png", 
           -- defaultX = 110, 
           -- defaultY = 110, 
           -- overSrc=imgDir.."map_btn_hov.png", 
           -- overX = 110, 
           -- overY = 110, 
           -- onRelease=onbtnMapTouch, 
           -- id="btnbacklistButton" 
       -- } 
	   -- btnMap:setReferencePoint(display.BottomRightReferencePoint);
		-- btnMap.x = 640; btnMap.y = 960; btnMap.alpha = 1; btnMap.oldAlpha = 1 

	_G.clConfig.lastEstateListIds = {}
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
		_G.clConfig.lastEstateListIds[i]=apt_id;
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
					local xPos, yPos = scrollView:getContentPosition();
					_G.clConfig.lastListPosition = yPos;
					_G.clConfig.currEstateIndex = i;
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

-- (5) nome dell'appartamento
		local nomeApt = display.newText("", 0,0, 568, 32, nil, _G.css.defaultFontSize30);
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
		local nomeZona = display.newText("", 0,0, 332, 60, nil, _G.css.defaultFontSize27);
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
	
	
	-- MENU A TENDINA ORDINA

local zoneListGroup = display.newGroup() 

zoneListGroup.x = 120; 
zoneListGroup.y = 315;
zoneListGroup.alpha = 0;
zoneListGroup.oldAlpha = 0;
zoneListGroup.yScale = 0.1; 



-- SFONDO TENDINA ORDINA
		bgzonelist = ui.newButton{ 
		   defaultSrc="img_bgzonelist.png", 
		   defaultX = 411, 
		   defaultY = 246, 
		   overSrc="img_bgzonelist.png", 
		   overX = 411, 
		   overY = 246
		} 
	   bgzonelist:addEventListener( "touch",  returnFalseEvent )
      --bgzonelist = display.newImageRect( imgDir.. "img_bgzonelist.png", 411, 246 ); 
	   bgzonelist:setReferencePoint(display.TopLeftReferencePoint);
       bgzonelist.x = 0; bgzonelist.y = 0; bgzonelist.alpha = 1; bgzonelist.oldAlpha = 1 
       zoneListGroup:insert(bgzonelist) 
       zoneListGroup.bgzonelist = bgzonelist

-- ITEM ORDINA

 local zoneItemHeight=0
for i=1,#_G.ordinaList do

		local zoneItemGroup = display.newGroup() 
		zoneItemGroup.x = 26; zoneItemGroup.y = 50+zoneItemHeight;
	   
-- SFONDO ITEM TENDINA ORDINA


	   
	   bgzoneitem = display.newImageRect( imgDir.. "img_bgzoneitem.png", 358, 46 ); 
	   bgzoneitem.alpha = 1; bgzoneitem.oldAlpha = 1 	   
	   bgzoneitem:setReferencePoint(display.CenterLeftReferencePoint);
       bgzoneitem.x = 0; bgzoneitem.y = 0; bgzoneitem.alpha = 1; bgzoneitem.oldAlpha = 1 
	   
       zoneItemGroup:insert(bgzoneitem) 

-- TESTO ITEM TENDINA ZONE 

	   nomeZonaItem = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
       nomeZonaItem.alpha = 1; nomeZonaItem.oldAlpha = 1 
	   nomeZonaItem.text = _G.lbl[_G.ordinaList[i].lbl]
	   nomeZonaItem:setTextColor(51, 51, 102)
	   nomeZonaItem.size = _G.css.defaultFontSize25
	   nomeZonaItem:setReferencePoint(display.CenterLeftReferencePoint);
       nomeZonaItem.x = 20; nomeZonaItem.y = 0; 
	   zoneItemGroup:insert(nomeZonaItem) 
	   
	   
	   
-- ICONE

      local onselectIcoAscTouch = function(event) 
          if event.phase=="ended" then
		
			gtStash.gt_tendinadown2= gtween.new( zoneListGroup, 0.3, { x = 120, y = 315, maskY = 950, rotation = 0, xScale = 1, yScale = 0.01, alpha=0}, {ease = gtween.easing.inCubic, reflect = false,  delay=0});		  
			listIsOpen = false;		
			ordinaIco:removeSelf();
			_G.clConfig.orderBy = _G.ordinaList[i].orderBy;
			_G.clConfig.orderHow = "asc";
			ordinaIco = display.newImageRect( orderByGetImg(), 46, 45 ); 
		   ordinaIco.alpha = 1; ordinaIco.oldAlpha = 1 
		   ordinaIco:setReferencePoint(display.CenterCenterReferencePoint);
		   ordinaIco.x = 293;
		   ordinaIco.y = 275; 
			
			gtStash.nomeZonaZoom= gtween.new( ordinaIco, 0.2, { xScale = 1.5, yScale = 1.5, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
			
			gtStash.nomeZonaZoomBack= gtween.new( ordinaIco, 0.3, { xScale = 1, yScale = 1, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});			
			loadList();
  end 
       end 

       selectIco1 = ui.newButton{ 
           defaultSrc=imgDir.."btn_round.png", 
           defaultX = 63, 
           defaultY = 63, 
           overSrc=imgDir.."btn_round_hov.png", 
           overX = 63, 
           overY = 63, 
           onRelease=onselectIcoAscTouch, 
           id="selectIco1Button" 
       } 
	   selectIco1:setReferencePoint(display.CenterLeftReferencePoint);
	   selectIco1.x = 211; selectIco1.y = 0; 
	   zoneItemGroup:insert(selectIco1) 
	   
	   ordinaIcoList1 = display.newImageRect( imgDir.. _G.ordinaList[i].img_asc, 46, 45 ); 
	   ordinaIcoList1.alpha = 1; ordinaIcoList1.oldAlpha = 1 
	   ordinaIcoList1:setReferencePoint(display.CenterLeftReferencePoint);
       ordinaIcoList1.x = 220;
	   ordinaIcoList1.y = 0; 
	   zoneItemGroup:insert(ordinaIcoList1)


local onselectIcoDescTouch = function(event) 
          if event.phase=="ended" then
		
			gtStash.gt_tendinadown2= gtween.new( zoneListGroup, 0.3, { x = 120, y = 315, maskY = 950, rotation = 0, xScale = 1, yScale = 0.01, alpha=0}, {ease = gtween.easing.inCubic, reflect = false,  delay=0});		  
			listIsOpen = false;		
			ordinaIco:removeSelf();
			_G.clConfig.orderBy = _G.ordinaList[i].orderBy;
			_G.clConfig.orderHow = "desc";
			ordinaIco = display.newImageRect( orderByGetImg(), 46, 45 ); 
		   ordinaIco.alpha = 1; ordinaIco.oldAlpha = 1 
		   ordinaIco:setReferencePoint(display.CenterCenterReferencePoint);
		   ordinaIco.x = 293;
		   ordinaIco.y = 275; 
			
			gtStash.nomeZonaZoom= gtween.new( ordinaIco, 0.2, { xScale = 1.5, yScale = 1.5, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
			
			gtStash.nomeZonaZoomBack= gtween.new( ordinaIco, 0.3, { xScale = 1, yScale = 1, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});			
			loadList();
  end 
       end 

       selectIco2 = ui.newButton{ 
           defaultSrc=imgDir.."btn_round.png", 
           defaultX = 63, 
           defaultY = 63, 
           overSrc=imgDir.."btn_round_hov.png", 
           overX = 63, 
           overY = 63, 
           onRelease=onselectIcoDescTouch, 
           id="selectIco1Button" 
       } 
	   selectIco2:setReferencePoint(display.CenterLeftReferencePoint);
	   selectIco2.x = 281; selectIco2.y = 0; 
	   zoneItemGroup:insert(selectIco2) 	   
	   
	   ordinaIcoList2 = display.newImageRect( imgDir.. _G.ordinaList[i].img_desc, 46, 45 ); 
	   ordinaIcoList2.alpha = 1; ordinaIcoList2.oldAlpha = 1 
	   ordinaIcoList2:setReferencePoint(display.CenterLeftReferencePoint);
       ordinaIcoList2.x = 290;
	   ordinaIcoList2.y = 0; 
	   zoneItemGroup:insert(ordinaIcoList2) 
	   
	   zoneListGroup:insert(zoneItemGroup) 
	   
	   zoneItemHeight=zoneItemHeight+zoneItemGroup.height+7
		
end


	   
--(2) tasto ordina

		local onOrdinaTouch = function(event) 
          if event.phase=="ended" then 
		  
				if listIsOpen == false then
				
				gtStash.gt_tendinadown= gtween.new( zoneListGroup, 0.3, { x = 120, y = 315, maskY = 240, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.outCubic, reflect = false,  delay=0});		  
				listIsOpen = true;
				gtStash.gt_btnzoneRotate= gtween.new( btnzone, 0.3, { rotation = 180 }, {ease = gtween.easing.inCircular, reflect = false,  delay=0});
				
					
				else
				
				gtStash.gt_tendinadown= gtween.new( zoneListGroup, 0.3, { x = 120, y = 315, maskY = 950, rotation = 0, xScale = 1, yScale = 0.01, alpha=0}, {ease = gtween.easing.inCubic, reflect = false,  delay=0});		  
				listIsOpen = false;
				gtStash.gt_btnzoneRotate= gtween.new( btnzone, 0.3, { rotation = 0}, {ease = gtween.easing.inCircular, reflect = false,  delay=0});				
				end
          end  
       end 
	   
       tastoordina = ui.newButton{ 
           defaultSrc=imgDir.."img_btn_ordina.png", 
           defaultX = 223, 
           defaultY = 76, 
           overSrc=imgDir.."img_btn_ordina_hov.png", 
           overX = 223, 
           overY = 76, 
           onRelease=onOrdinaTouch, 
           id="ordinaButton" 
       }	
		tastoordina:setReferencePoint(display.CenterLeftReferencePoint);
		tastoordina.x = 110; tastoordina.y = 275; tastoordina.alpha = 1; tastoordina.oldAlpha = 1 	   
	   
	   
--(2) testo ordina
		listatittxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize23);
		listatittxt.alpha = 1; listatittxt.oldAlpha = 1 
		listatittxt.text = _G.lbl.mobileOrderBy..":";
		listatittxt:setTextColor(255, 255, 255)
		listatittxt.size = _G.css.defaultFontSize23
		
		listatittxt:setReferencePoint(display.CenterLeftReferencePoint);
		listatittxt.x=135;
		listatittxt.y = 275;
		
-- icona tipo di ordina selezionato

       ordinaIco = display.newImageRect( orderByGetImg(), 46, 45 ); 
	   ordinaIco.alpha = 1; ordinaIco.oldAlpha = 1 
	   ordinaIco:setReferencePoint(display.CenterCenterReferencePoint);
       ordinaIco.x = 293;
	   ordinaIco.y = 275; 
		
		
--(2) tasto ricerca avanzata (filtri)

		local onFiltraTouch = function(event) 
          if event.phase=="ended" then  
            
          end 
       end 
	if(1==2) then -- AdvancedSearch - per ora non c'e   
       tastofiltra = ui.newButton{ 
           defaultSrc=imgDir.."img_btn_filtri.png", 
           defaultX = 251, 
           defaultY = 75, 
           overSrc=imgDir.."img_btn_filtri_hov.png", 
           overX = 251, 
           overY = 75, 
           onRelease=onFiltraTouch, 
           id="FiltraButton" 
       }	
		tastofiltra:setReferencePoint(display.CenterLeftReferencePoint);
		tastofiltra.x = 341; tastofiltra.y = 275; tastofiltra.alpha = 1; tastofiltra.oldAlpha = 1 	   
	   
	   
--(2) testo ricerca avanzata
		filtriTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
		filtriTxt.alpha = 1; filtriTxt.oldAlpha = 1 
		filtriTxt.text =  _G.lbl.mobileAdvancedSearch;
		filtriTxt:setTextColor(255, 255, 255)
		filtriTxt.size = _G.css.defaultFontSize25
		
		filtriTxt:setReferencePoint(display.CenterLeftReferencePoint);
		filtriTxt.x=366;
		filtriTxt.y = 275;
	end
		



end 	   
local function networkListener( event )
        if ( event.isError ) then
			print ( "Network error - download failed " )
			print ( "RESPONSE: " .. event.response )
			listEmpty();
        else
			--print ( "RESPONSE: " .. event.response )
			if((""..event.response):find("listEmpty")==nil) then
				currList = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
				createList()
				if(_G.clConfig.lastListPosition~=0) then
					scrollView:scrollToPosition( 0, _G.clConfig.lastListPosition, 0 )
					_G.clConfig.lastListPosition=0;
				end
			else
				listEmpty();
			end
        end
end
function listEmpty()
	hideLoading()
	loadingDesc:removeSelf();
	loadingDesc = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	loadingDesc:setTextColor(105, 95, 111)
	loadingDesc.text = "No data found"
	loadingDesc.size = _G.css.defaultFontSize30
	loadingDesc:setReferencePoint(display.CenterCenterReferencePoint);
	loadingDesc.x=320;
	loadingDesc.y=650;
	scrollView:insert(loadingDesc)
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
	xmlFilter=xmlFilter.."&currZoneList=".._G.clConfig.selectedZone.id;
	xmlFilter=xmlFilter.."";
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

	scrollView:scrollToTop();
	
	showLoading();
	loadingDesc = display.newText("", 0,0,620,0, native.systemFontBold, _G.css.defaultFontSize30);
	loadingDesc:setTextColor(105, 95, 111)
	loadingDesc.text = _G.lbl.mobileLoadingLista;
	loadingDesc.size = _G.css.defaultFontSize27
	loadingDesc:setReferencePoint(display.TopCenterReferencePoint);
	loadingDesc.x=320;
	loadingDesc.y=650;
	scrollView:insert(loadingDesc)
	
end
-- carica la lista
loadList();
		


   end 
   drawScreen() 

   return menuGroup 
end 
