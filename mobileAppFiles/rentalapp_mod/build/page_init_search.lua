-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 

 function new() 
	_G.currPageType = "init_search";
    local numPages = 5 
    local menuGroup = display.newGroup() 




    local curPage = 2 

    local drawScreen = function() 
		getMainMenu(menuGroup);
		local nomeZonaItem
	   local bgzoneitemhov  
       local bgzoneitem  
       local bgzonelist  
       local omino1  
       local omino2  
       local mail  
       local favoriteadd  
       local testocheckout  
       local testocheckin  
       local btncalhov  
       local btncal  
       local campocheckin  
       local testoinizelenco  
       local cercahov  
       local cerca  
       local btnzonehov  
       local btnzone  
       local elencozone  
       local zone  
       local guestforapts  
       local numberofnights  
       local calcologuest  
       local menoGuesthov  
       local menoGuest  
       local plusGuestshov  
       local plusGuests  
       local campoguest  
       local calcolonotti  
	   local calcologuest
       local checkoutdate  
       local checkindate  
       local lineazone  
       local lineasottosecda  
       local lineasottoprdat  
       local menoNottihov  
       local menoNotti  
       local piuNottihov  
       local piuNotti  
       local camponotti  
       local logo  
       local headbg  
       local background  
	   local zonetxt 
		local calcolonotti
	    --local highlightcal_sheet  
       --local highlightcal_set
	   
	   local listIsOpen = false;
	   
--(2) sfondo del campo notti
       camponotti = display.newImageRect( imgDir.. "img_camponotti.png", 274, 58 ); 
       camponotti.x = 469; camponotti.y = 534; camponotti.alpha = 1; camponotti.oldAlpha = 1 
       menuGroup:insert(camponotti) 
       menuGroup.camponotti = camponotti 
	   
	   local zonetxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
       zonetxt.alpha = 1; zonetxt.oldAlpha = 1 
	   zonetxt.text = _G.lbl.lblZone..":"
	   zonetxt:setTextColor(253, 102, 52)
	   zonetxt.size = _G.css.defaultFontSize27
	   zonetxt:setReferencePoint(display.CenterLeftReferencePoint);
	   zonetxt.x = 36; zonetxt.y = 249;
	   
-- MENU A TENDINA ZONE 

local zoneListGroup = display.newGroup() 

zoneListGroup.x = 137; 
zoneListGroup.y = 264;
zoneListGroup.alpha = 0;
zoneListGroup.oldAlpha = 0;
zoneListGroup.yScale = 0.1; 



-- SFONDO TENDINA ZONE 

       bgzonelist = display.newImageRect( imgDir.. "img_bgzonelist.png", 411, 710 ); 
	   bgzonelist:setReferencePoint(display.TopLeftReferencePoint);
       bgzonelist.x = 0; bgzonelist.y = 0; bgzonelist.alpha = 1; bgzonelist.oldAlpha = 1 
	   
       zoneListGroup:insert(bgzonelist) 
       zoneListGroup.bgzonelist = bgzonelist
 local zoneItemHeight=0
 
for i=1,#_G.zoneList do

		local zoneItemGroup = display.newGroup() 
		zoneItemGroup.x = 26; zoneItemGroup.y = 35+zoneItemHeight;
	   
-- SFONDO ITEM TENDINA ZONE 

      local onbgzoneitemTouch = function(event) 
          if event.phase=="ended" then
			_G.clConfig.selectedZone =_G.zoneList[i];
			testoinizelenco.text = _G.clConfig.selectedZone.title;
			gtStash.gt_tendinadown= gtween.new( zoneListGroup, 0.3, { x = 137, y = 264, maskY = 950, rotation = 0, xScale = 1, yScale = 0.01, alpha=0}, {ease = gtween.easing.inCubic, reflect = false,  delay=0});		  
			listIsOpen = false;		
			gtStash.nomeZonaZoom= gtween.new( testoinizelenco, 0.2, { xScale = 1.1, yScale = 1.1, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
			gtStash.nomeZonaZoomBack= gtween.new( testoinizelenco, 0.3, { xScale = 1, yScale = 1, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0.2});			
			gtStash.gt_btnzoneRotate= gtween.new( btnzone, 0.3, { rotation = 0 }, {ease = gtween.easing.inCircular, reflect = false,  delay=0});
  end 
       end 

       bgzoneitem = ui.newButton{ 
           defaultSrc=imgDir.."img_bgzoneitem.png", 
           defaultX = 358, 
           defaultY = 46, 
           overSrc=imgDir.."img_bgzoneitemhov.png", 
           overX = 358, 
           overY = 46, 
           onRelease=onbgzoneitemTouch, 
           id="bgzoneitemButton" 
       } 
		bgzoneitem:setReferencePoint(display.CenterLeftReferencePoint);
		
       bgzoneitem.x = 0; bgzoneitem.y = 0; bgzoneitem.alpha = 1; bgzoneitem.oldAlpha = 1 
	   
       zoneItemGroup:insert(bgzoneitem) 

-- TESTO ITEM TENDINA ZONE 

	   nomeZonaItem = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
       nomeZonaItem.alpha = 1; nomeZonaItem.oldAlpha = 1 
	   nomeZonaItem.text = _G.zoneList[i].title
	   nomeZonaItem:setTextColor(51, 51, 102)
	   nomeZonaItem.size = _G.css.defaultFontSize25;
	   nomeZonaItem:setReferencePoint(display.CenterLeftReferencePoint);
       nomeZonaItem.x = 20; nomeZonaItem.y = 0; 
	   zoneItemGroup:insert(nomeZonaItem) 
	   
	   zoneListGroup:insert(zoneItemGroup) 
	   
	   zoneItemHeight=zoneItemHeight+zoneItemGroup.height+10
		
end


	   
	   
-- NOTTI
       local moreNights = function(event) 
          if event.phase=="ended" then 
				_G.clConfig.nightsNum = _G.clConfig.nightsNum+1;
				calcolonotti.text = _G.clConfig.nightsNum
				_G.clConfig.dataCheckout = os.date( "*t",os.time(_G.clConfig.dataCheckin) + 60*60*24*_G.clConfig.nightsNum )
				testocheckout.text = JSCal:formatCustom(_G.clConfig.dataCheckout, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckout.day .."/".. _G.clConfig.dataCheckout.month .."/".. _G.clConfig.dataCheckout.year)

          end 
       end 
--(10) regular layer 
       piuNotti = ui.newButton{ 
           defaultSrc=imgDir.."img_plus1.png", 
           defaultX = 73, 
           defaultY = 66, 
           overSrc=imgDir.."img_plus1hov.png", 
           overX = 73, 
           overY = 66, 
           onRelease=moreNights, 
           id="piuNottiButton" 
       } 

       piuNotti.x = 573; piuNotti.y = 534; piuNotti.alpha = 1; piuNotti.oldAlpha = 1 
       menuGroup:insert(piuNotti) 
       menuGroup.piuNotti = piuNotti 

       local lessNights = function(event) 
          if event.phase=="ended" then  
		  
			
			if _G.clConfig.nightsNum==1 then
				_G.clConfig.nightsNum=1;
				calcolonotti.text = _G.clConfig.nightsNum;
			else 
				_G.clConfig.nightsNum = _G.clConfig.nightsNum-1;
				calcolonotti.text = _G.clConfig.nightsNum;
			end
				_G.clConfig.dataCheckout = os.date( "*t",os.time(_G.clConfig.dataCheckin) + 60*60*24*_G.clConfig.nightsNum )
				testocheckout.text = JSCal:formatCustom(_G.clConfig.dataCheckout, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckout.day .."/".. _G.clConfig.dataCheckout.month .."/".. _G.clConfig.dataCheckout.year)
		  
          end 
       end 
--(10) regular layer 
       menoNotti = ui.newButton{ 
           defaultSrc=imgDir.."img_meno1.png", 
           defaultX = 73, 
           defaultY = 66, 
           overSrc=imgDir.."img_meno1hov.png", 
           overX = 73, 
           overY = 66, 
           onRelease=lessNights, 
           id="menoNottiButton" 
       } 

       menoNotti.x = 365; menoNotti.y = 534; menoNotti.alpha = 1; menoNotti.oldAlpha = 1 


--(2) regular layer 
       lineasottoprdat = display.newImageRect( imgDir.. "img_lineasottoprdat.png", 654, 12 ); 
       lineasottoprdat.x = 322; lineasottoprdat.y = 598; lineasottoprdat.alpha = 1; lineasottoprdat.oldAlpha = 1 
       menuGroup:insert(lineasottoprdat) 
       menuGroup.lineasottoprdat = lineasottoprdat 

--(2) regular layer 
       lineasottosecda = display.newImageRect( imgDir.. "img_lineasottosecda.png", 654, 12 ); 
       lineasottosecda.x = 324; lineasottosecda.y = 722; lineasottosecda.alpha = 1; lineasottosecda.oldAlpha = 1 
       menuGroup:insert(lineasottosecda) 
       menuGroup.lineasottosecda = lineasottosecda 

--(2) regular layer 
       lineazone = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
       lineazone.x = 322; lineazone.y = 334; lineazone.alpha = 1; lineazone.oldAlpha = 1 
       menuGroup:insert(lineazone) 
       menuGroup.lineazone = lineazone 

--(2) regular layer 
	   local checkindate = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
       checkindate.alpha = 1; checkindate.oldAlpha = 1 
	   checkindate.text = _G.lbl.mobileCheckInDate..":"
	   checkindate:setTextColor(253, 102, 52)
	   checkindate.size = _G.css.defaultFontSize27
	   checkindate:setReferencePoint(display.CenterLeftReferencePoint);
       checkindate.x = 36; checkindate.y = 415;

--(2) regular layer 

	   local checkoutdate = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
       checkoutdate.alpha = 1; checkoutdate.oldAlpha = 1 
	   checkoutdate.text = _G.lbl.mobileCheckOutDate..":"
	   checkoutdate:setTextColor(253, 102, 52)
	   checkoutdate.size = _G.css.defaultFontSize27
	   checkoutdate:setReferencePoint(display.CenterLeftReferencePoint);
       checkoutdate.x = 36; checkoutdate.y = 659; 

--(2) regular layer 

	   calcolonotti = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize43);
		calcolonotti:setTextColor(104, 96, 111)
		calcolonotti.text = _G.clConfig.nightsNum
		calcolonotti.size = _G.css.defaultFontSize43
		calcolonotti:setReferencePoint(display.CenterCenterReferencePoint);
       calcolonotti.x = 467; calcolonotti.y = 535;


--(2) regular layer 
       campoguest = display.newImageRect( imgDir.. "img_camponotti.png", 274, 59 ); 
       campoguest.x = 469; campoguest.y = 789; campoguest.alpha = 1; campoguest.oldAlpha = 1 
       menuGroup:insert(campoguest) 
       menuGroup.campoguest = campoguest 

       local moreGuests = function(event) 
          if event.phase=="ended" then  
				_G.clConfig.guestsNum = _G.clConfig.guestsNum+1;
				calcologuest.text = _G.clConfig.guestsNum;
          end 
       end 
--(10) regular layer 
       plusGuests = ui.newButton{ 
           defaultSrc=imgDir.."img_plus2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_plus2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=moreGuests, 
           id="plusGuestsButton" 
       } 

       plusGuests.x = 573; plusGuests.y = 789; plusGuests.alpha = 1; plusGuests.oldAlpha = 1 


       local lessGuests = function(event) 
          if event.phase=="ended" then 
			if _G.clConfig.guestsNum==1 then
				_G.clConfig.guestsNum=1;
				calcologuest.text = _G.clConfig.guestsNum;
			else 
				_G.clConfig.guestsNum = _G.clConfig.guestsNum-1;
				calcologuest.text = _G.clConfig.guestsNum;
			end
          end 
       end 
--(10) regular layer 
       menoGuest = ui.newButton{ 
           defaultSrc=imgDir.."img_meno2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_meno2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=lessGuests, 
           id="menoGuestButton" 
       } 

       menoGuest.x = 365; menoGuest.y = 789; menoGuest.alpha = 1; menoGuest.oldAlpha = 1 


--(2) regular layer 
		calcologuest = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize43);
		calcologuest:setTextColor(104, 96, 111)
		calcologuest.text = _G.clConfig.guestsNum;
		calcologuest.size = _G.css.defaultFontSize43
		calcologuest:setReferencePoint(display.CenterCenterReferencePoint);
       calcologuest.x = 467; calcologuest.y = 790;
      

--(2) regular layer 

	   
	   	numberofnights = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
		numberofnights:setTextColor(253, 102, 52)
		numberofnights.text = _G.lbl.mobileNumOfNights..":";
		numberofnights.size = _G.css.defaultFontSize27
		numberofnights:setReferencePoint(display.CenterLeftReferencePoint);
       numberofnights.x = 36; numberofnights.y = 535;

--(2) regular layer 
	   	guestforapts = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize27);
		guestforapts:setTextColor(253, 102, 52)
		guestforapts.text = _G.lbl.mobileNumOfGuests..":";
		guestforapts.size = _G.css.defaultFontSize27
		guestforapts:setReferencePoint(display.CenterLeftReferencePoint);
       guestforapts.x = 36; guestforapts.y = 792; 


--(2) regular layer 
       elencozone = display.newImageRect( imgDir.. "img_elencozone.png", 432, 59 ); 
       elencozone.x = 361; elencozone.y = 248; elencozone.alpha = 1; elencozone.oldAlpha = 1 
       menuGroup:insert(elencozone) 
       menuGroup.elencozone = elencozone 

       local onbtnzoneTouch = function(event) 
          if event.phase=="ended" then 
		  
				if listIsOpen == false then
				
				gtStash.gt_tendinadown= gtween.new( zoneListGroup, 0.3, { x = 137, y = 264, maskY = 240, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.outCubic, reflect = false,  delay=0});		  
				listIsOpen = true;
				gtStash.gt_btnzoneRotate= gtween.new( btnzone, 0.3, { rotation = 180 }, {ease = gtween.easing.inCircular, reflect = false,  delay=0});
				
					
				else
				
				gtStash.gt_tendinadown= gtween.new( zoneListGroup, 0.3, { x = 137, y = 264, maskY = 950, rotation = 0, xScale = 1, yScale = 0.01, alpha=0}, {ease = gtween.easing.inCubic, reflect = false,  delay=0});		  
				listIsOpen = false;
				gtStash.gt_btnzoneRotate= gtween.new( btnzone, 0.3, { rotation = 0}, {ease = gtween.easing.inCircular, reflect = false,  delay=0});				
				end
          end 
       end 
--(10) regular layer 
       btnzone = ui.newButton{ 
           defaultSrc=imgDir.."img_btnzone.png", 
           defaultX = 97, 
           defaultY = 97, 
           overSrc=imgDir.."img_btnzonehov.png", 
           overX = 97, 
           overY = 97, 
		   --onRelease=showDropDown,
           onRelease=onbtnzoneTouch, 
           id="btnzoneButton" 
       }

       btnzone.x = 573; btnzone.y = 248; btnzone.alpha = 1; btnzone.oldAlpha = 1 
       menuGroup:insert(btnzone) 
       menuGroup.btnzone = btnzone 

       local oncercaTouch = function(event) 
          if event.phase=="ended" then  
            local myClosure_switch = function() 
                disposeTweens() 
				_G.clConfig.lastListPosition=0;
                director:changeScene( "page_lista_search", "crossfade" ) 
            end 
            timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
			if _G.clConfig.searchFirstAccess == true then
				lightcal:removeSelf();
				_G.clConfig.searchFirstAccess=false;
			end
          end 
       end 
--(10) regular layer 
       cerca = ui.newButton{ 
           defaultSrc=imgDir.."btn_big.png", 
           defaultX = 655, 
           defaultY = 121, 
           overSrc=imgDir.."btn_big_hov.png", 
           overX = 655, 
           overY = 121, 
           onRelease=oncercaTouch, 
           id="cercaButton" 
       }

       cerca.x = 319; cerca.y = 905; cerca.alpha = 1; cerca.oldAlpha = 1 
       menuGroup:insert(cerca) 
       menuGroup.cerca = cerca 
	   
	   --(2) regular layer 

	   local testoBtnCerca = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		testoBtnCerca:setTextColor(255, 255, 255)
		testoBtnCerca.text = _G.lbl.mobileSearchAccomodation;
		testoBtnCerca.size = _G.css.defaultFontSize40
		testoBtnCerca:setReferencePoint(display.CenterCenterReferencePoint);
       testoBtnCerca.x = 325; testoBtnCerca.y = 905; 
       testoBtnCerca.alpha = 1; testoBtnCerca.oldAlpha = 1 

--(2) regular layer 

	   testoinizelenco = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		testoinizelenco:setTextColor(104, 96, 111)
		testoinizelenco.text = _G.clConfig.selectedZone.title
		testoinizelenco.size = _G.css.defaultFontSize30
		testoinizelenco:setReferencePoint(display.CenterCenterReferencePoint);
       testoinizelenco.x = 340; testoinizelenco.y = 248; 
       testoinizelenco.alpha = 1; testoinizelenco.oldAlpha = 1 

--(2) regular layer 
       campocheckin = display.newImageRect( imgDir.. "img_campocheckin.png", 324, 59 ); 
       campocheckin.x = 415; campocheckin.y = 413; campocheckin.alpha = 1; campocheckin.oldAlpha = 1 
       menuGroup:insert(campocheckin) 
       menuGroup.campocheckin = campocheckin 
	   
--(2) regular layer 

		if _G.clConfig.searchFirstAccess == true then
       lightcal = display.newImageRect( imgDir.. "light_cal.png", 114, 114 ); 
       lightcal.x = 573; lightcal.y = 413; lightcal.alpha = 1; lightcal.oldAlpha = 1 
	   
	   gtStash.gt_light_cal= gtween.new( lightcal, 0.7, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0})
		end
		
       local onbtncalTouch = function(event) 
          if event.phase=="ended" then  
		  
            local myClosure_switch = function() 
                disposeTweens() 
				
				--local parameters = {prova="pippo", prova2="pluto"}
                --director:changeScene( parameters, "page_select_date", "moveFromRight" ) 
				director:changeScene( "page_select_date", "moveFromRight" ) 
            end 
            timerStash.newTimer_418 = timer.performWithDelay(0, myClosure_switch, 1)
			
			if _G.clConfig.searchFirstAccess == true then
				lightcal:removeSelf();
				_G.clConfig.searchFirstAccess=false;
			end
		  
          end 
       end 
--(10) regular layer 
       btncal = ui.newButton{ 
           defaultSrc=imgDir.."img_btncal.png", 
           defaultX = 97, 
           defaultY = 97, 
           overSrc=imgDir.."img_btncalhov.png", 
           overX = 97, 
           overY = 97, 
           onRelease=onbtncalTouch, 
           id="btncalButton" 
       } 

       btncal.x = 573; btncal.y = 413; btncal.alpha = 1; btncal.oldAlpha = 1 
       menuGroup:insert(btncal) 
       menuGroup.btncal = btncal 
	   


--(2) regular layer 


	   testocheckin = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize25);
		testocheckin:setTextColor(104, 96, 111)
		testocheckin.text = JSCal:formatCustom(_G.clConfig.dataCheckin, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckin.day .."/".. _G.clConfig.dataCheckin.month .."/".. _G.clConfig.dataCheckin.year)
		testocheckin.size = _G.css.defaultFontSize25
		testocheckin:setReferencePoint(display.CenterLeftReferencePoint);
        testocheckin.x = 274; testocheckin.y = 415;
       testocheckin.alpha = 1; testocheckin.oldAlpha = 1 	   

--(2) regular layer 
	   testocheckout = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		testocheckout:setTextColor(104, 96, 111)
		_G.clConfig.dataCheckout = os.date( "*t",os.time(_G.clConfig.dataCheckin) + 60*60*24*_G.clConfig.nightsNum )
		testocheckout.text = JSCal:formatCustom(_G.clConfig.dataCheckout, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckout.day .."/".. _G.clConfig.dataCheckout.month .."/".. _G.clConfig.dataCheckout.year); 
		testocheckout.size = _G.css.defaultFontSize30
		testocheckout:setReferencePoint(display.CenterRightReferencePoint);
       testocheckout.x = 607; testocheckout.y = 658; 
	   
--(2) ANIMAZIONE DI HIGHLIGHT DEL CALENDARIO
	   
	  -- highlightcal_sheet = sprite.newSpriteSheet(imgDir.. "highlight_cal_sprite.png", 48, 48 ); 
      -- highlightcal_set = sprite.newSpriteSet( highlightcal_sheet, 1, 8 ); 
      -- sprite.add( highlightcal_set, "highlightcal_anim", 1, 8, 1000, 0); 
      -- highlightcal = sprite.newSprite ( highlightcal_set ); 
      -- highlightcal:prepare ( "highlightcal_anim" ); 
      -- highlightcal:play(); 
      -- highlightcal.x = 558; highlightcal.y = 398; highlightcal.alpha = 1; highlightcal.oldAlpha = 1 


   end 
   drawScreen() 

   return menuGroup 
end 
