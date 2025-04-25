

--	function new(parameters)
--		print(parameters.prova, parameters.prova2)
--	end

-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 

 function new() 
	_G.currPageType = "select_date";
    local numPages = 5 
    local menuGroup = display.newGroup() 
    local disposeAudios 
    local disposeTweens 
	


    local curPage = 3 
	

    local drawScreen = function() 
		getMainMenu(menuGroup);

       local omino1  
       local omino2  
       local mail  
       local favoriteadd  
       local btnbackcalhov  
       local btnbackcal  
--       local backtit  
--       local calendariotemp  
	   local selectdatetxt
       local logo  
       local headbg  
       local background  
--(2) regular layer 
       selectdatetxt = display.newImageRect( imgDir.. "img_lbl_selectcheckindate-".._G.clConfig.langCode..".png", 565, 85 ); 
       selectdatetxt.x = 321; selectdatetxt.y = 290; selectdatetxt.alpha = 1; selectdatetxt.oldAlpha = 1 
       menuGroup:insert(selectdatetxt) 
       menuGroup.selectdatetxt = selectdatetxt 

 local widget = require "widget"
		widget.setTheme("theme_ios")
	   

       local myGiorniPress = function(event) 
            local myClosure_switch = function() 
                disposeTweens() 
                director:changeScene( "page_init_search", "moveFromLeft" ) 
            end 
            timerStash.newTimer_107 = timer.performWithDelay(200, myClosure_switch, 1) 
       end 
	   
	  
	   local myGiorni= widget.newButton{
		style="rentalBtn",
		id= "button1",
		left = 36,
		top = 833,
		label = _G.lbl.mobileSetDate,
		width = 255, height = 75,
		cornerRadius = 8,
		onPress= myGiorniPress
		}
	   
--(2) LINEE
       local linea1 = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
       linea1.x = 322; linea1.y = 404; linea1.alpha = 1; linea1.oldAlpha = 1 
	  
       local linea2 = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
       linea2.x = 322; linea2.y = 752; linea2.alpha = 1; linea2.oldAlpha = 1 
	   
-- Testo DATA FINALE   
	   	local selectedDate = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		selectedDate:setTextColor(253, 102, 52)
		selectedDate.text = JSCal:formatCustom(_G.clConfig.dataCheckin, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckin.day .."/".. _G.clConfig.dataCheckin.month .."/".. _G.clConfig.dataCheckin.year)
		selectedDate.size = _G.css.defaultFontSize30
		selectedDate:setReferencePoint(display.CenterRightReferencePoint);
       selectedDate.x = 580; selectedDate.y = 870;
	   
--(2) sfondo DATA FINALE
       campoData = display.newImageRect( imgDir.. "img_campoguest.png", 310, 70 ); 
	   campoData:setReferencePoint(display.CenterCenterReferencePoint);
       campoData.x = 455; campoData.y = 870; campoData.alpha = 0.5; campoData.oldAlpha = 1 
	   
	   
	   
-- DAY
		function displayDate() 
			_G.clConfig.dataCheckin = os.date( "*t",os.time(_G.clConfig.dataCheckin))
			if(os.time(_G.clConfig.dataCheckin) < os.time(_G.clConfig.minCheckin)) then _G.clConfig.dataCheckin = _G.clConfig.minCheckin; end
			calcoloday.text = _G.clConfig.dataCheckin.day;
			calcolomonth.text = _G.clConfig.dataCheckin.month;
			calcoloyear.text = _G.clConfig.dataCheckin.year;
			selectedDate.text = JSCal:formatCustom(_G.clConfig.dataCheckin, "#d# #MM# #yy#", _G.clConfig.langCode, _G.clConfig.dataCheckin.day .."/".. _G.clConfig.dataCheckin.month .."/".. _G.clConfig.dataCheckin.year)
		end 
		
		
		
	   	local dayTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		dayTxt:setTextColor(253, 102, 52)
		dayTxt.text = _G.lbl.lblDay..":"
		dayTxt.size = _G.css.defaultFontSize40
		dayTxt:setReferencePoint(display.CenterLeftReferencePoint);
       dayTxt.x = 36; dayTxt.y = 480;
	
	   
--(10) tasto più 
       local moreDay = function(event) 
          if event.phase=="ended" then  
				_G.clConfig.dataCheckin = os.date( "*t",os.time(_G.clConfig.dataCheckin) + 60*60*24 )
				displayDate() 
          end 
       end
	   
	   
       plusDay = ui.newButton{ 
           defaultSrc=imgDir.."img_plus2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_plus2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=moreDay, 
           id="plusDayButton" 
       } 

       plusDay.x = 573; plusDay.y = 480; plusDay.alpha = 1; plusDay.oldAlpha = 1 	   
--(10) tasto meno	   

	local lessDay= function(event) 
          if event.phase=="ended" then 
			_G.clConfig.dataCheckin = os.date( "*t",os.time(_G.clConfig.dataCheckin) - 60*60*24 )
			displayDate() 
          end 
       end 
	   
       menoDay = ui.newButton{ 
           defaultSrc=imgDir.."img_meno2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_meno2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=lessDay, 
           id="menoDayButton" 
       } 

       menoDay.x = 365; menoDay.y = 480; menoDay.alpha = 1; menoDay.oldAlpha = 1 
	   
--(2) testo giorno
		calcoloday = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize43);
		calcoloday:setTextColor(104, 96, 111)
		calcoloday.text = _G.clConfig.dataCheckin.day;
		calcoloday.size = _G.css.defaultFontSize43
		calcoloday:setReferencePoint(display.CenterCenterReferencePoint);
       calcoloday.x = 467; calcoloday.y = 480;
	   
--(2) sfondo Day
       campoday = display.newImageRect( imgDir.. "img_campoguest.png", 274, 59 ); 
	   campoday:setReferencePoint(display.CenterCenterReferencePoint);
       campoday.x = 469; campoday.y = 480; campoday.alpha = 1; campoday.oldAlpha = 1 
	   
-- MONTH
	   	local monthTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		monthTxt:setTextColor(253, 102, 52)
		monthTxt.text = _G.lbl.lblMonth..":"
		monthTxt.size = _G.css.defaultFontSize40
		monthTxt:setReferencePoint(display.CenterLeftReferencePoint);
       monthTxt.x = 36; monthTxt.y = 580;
--(10) tasto più 
       local moreMonth = function(event) 
          if event.phase=="ended" then  
			tmpDate = os.date( "*t",os.time(_G.clConfig.dataCheckin))
			monthNum=tmpDate.month
			if monthNum==12 then
				tmpDate.month = 1;
				tmpDate = tmpDate.year+1;
			else
				tmpDate.month = tmpDate.month+1;
			end
			tmpDate = os.date( "*t",os.time(tmpDate))
			if tmpDate.day<_G.clConfig.dataCheckin.day and _G.clConfig.dataCheckin.month+1<tmpDate.month then
				for i=1,31 do
					if _G.clConfig.dataCheckin.month+1 == tmpDate.month then 
						break 
					end
					tmpDate = os.date( "*t",os.time(tmpDate) - 60*60*24)
				end
			end
			_G.clConfig.dataCheckin=tmpDate
			displayDate() 
          end 
       end
	   
       plusMonth = ui.newButton{ 
           defaultSrc=imgDir.."img_plus2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_plus2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=moreMonth, 
           id="plusMonthButton" 
       } 

       plusMonth.x = 573; plusMonth.y = 580; plusMonth.alpha = 1; plusMonth.oldAlpha = 1 	   
--(10) tasto meno	   

	local lessMonth= function(event) 
          if event.phase=="ended" then 
			tmpDate = os.date( "*t",os.time(_G.clConfig.dataCheckin))
			monthNum=tmpDate.month
			if monthNum==1 then
				tmpDate.month = 12;
				tmpDate = tmpDate.year-1;
			else
				tmpDate.month = tmpDate.month-1;
			end
			tmpDate = os.date( "*t",os.time(tmpDate))
			if tmpDate.day<_G.clConfig.dataCheckin.day and _G.clConfig.dataCheckin.month==tmpDate.month then
				for i=1,31 do
					if _G.clConfig.dataCheckin.month-1 == tmpDate.month then 
						break 
					end
					tmpDate = os.date( "*t",os.time(tmpDate) - 60*60*24)
				end
			end
			_G.clConfig.dataCheckin=tmpDate
			displayDate() 
          end 
       end 
	   
       menoMonth = ui.newButton{ 
           defaultSrc=imgDir.."img_meno2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_meno2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=lessMonth, 
           id="menoMonthButton" 
       } 

       menoMonth.x = 365; menoMonth.y = 580; menoMonth.alpha = 1; menoMonth.oldAlpha = 1 
	   
--(2) testo mese
		calcolomonth = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize43);
		calcolomonth:setTextColor(104, 96, 111)
		calcolomonth.text = _G.clConfig.dataCheckin.month;
		calcolomonth.size = _G.css.defaultFontSize43
		calcolomonth:setReferencePoint(display.CenterCenterReferencePoint);
       calcolomonth.x = 467; calcolomonth.y = 580;
	   
--(2) sfondo Mese
       campomonth = display.newImageRect( imgDir.. "img_campoguest.png", 274, 59 ); 
	   campomonth:setReferencePoint(display.CenterCenterReferencePoint);
       campomonth.x = 469; campomonth.y = 580; campomonth.alpha = 1; campomonth.oldAlpha = 1 
	   
-- YEAR 
	   	local yearTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		yearTxt:setTextColor(253, 102, 52)
		yearTxt.text = _G.lbl.lblYear..":"
		yearTxt.size = _G.css.defaultFontSize40
		yearTxt:setReferencePoint(display.CenterLeftReferencePoint);
       yearTxt.x = 36; yearTxt.y = 680;
	   
--(10) tasto più 
       local moreYear = function(event) 
          if event.phase=="ended" then  
			tmpDate = os.date( "*t",os.time(_G.clConfig.dataCheckin))
			tmpDate.year = tmpDate.year+1;
			tmpDate = os.date( "*t",os.time(tmpDate))
			if tmpDate.day<_G.clConfig.dataCheckin.day and _G.clConfig.dataCheckin.month<tmpDate.month then
				for i=1,31 do
					if _G.clConfig.dataCheckin.month == tmpDate.month then 
						break 
					end
					tmpDate = os.date( "*t",os.time(tmpDate) - 60*60*24)
				end
			end
			_G.clConfig.dataCheckin=tmpDate
			displayDate() 
          end 
       end
	   
       plusYear = ui.newButton{ 
           defaultSrc=imgDir.."img_plus2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_plus2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=moreYear, 
           id="plusYearButton" 
       } 

       plusYear.x = 573; plusYear.y = 680; plusYear.alpha = 1; plusYear.oldAlpha = 1 	   
--(10) tasto meno	   

	local lessYear= function(event) 
          if event.phase=="ended" then 
			tmpDate = os.date( "*t",os.time(_G.clConfig.dataCheckin))
			tmpDate.year = tmpDate.year-1;
			tmpDate = os.date( "*t",os.time(tmpDate))
			if tmpDate.day<_G.clConfig.dataCheckin.day and _G.clConfig.dataCheckin.month<tmpDate.month then
				for i=1,31 do
					if _G.clConfig.dataCheckin.month == tmpDate.month then 
						break 
					end
					tmpDate = os.date( "*t",os.time(tmpDate) - 60*60*24)
				end
			end
			_G.clConfig.dataCheckin=tmpDate
			displayDate() 
          end 
       end 
	   
       menoYear = ui.newButton{ 
           defaultSrc=imgDir.."img_meno2.png", 
           defaultX = 73, 
           defaultY = 67, 
           overSrc=imgDir.."img_meno2hov.png", 
           overX = 73, 
           overY = 67, 
           onRelease=lessYear, 
           id="menoYearButton" 
       } 

       menoYear.x = 365; menoYear.y = 680; menoYear.alpha = 1; menoYear.oldAlpha = 1 
	   
--(2) testo mese
		calcoloyear = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize43);
		calcoloyear:setTextColor(104, 96, 111)
		calcoloyear.text = _G.clConfig.dataCheckin.year;
		calcoloyear.size = _G.css.defaultFontSize43
		calcoloyear:setReferencePoint(display.CenterCenterReferencePoint);
       calcoloyear.x = 467; calcoloyear.y = 680;
	   
--(2) sfondo Mese
       campoyear = display.newImageRect( imgDir.. "img_campoguest.png", 274, 59 ); 
	   campoyear:setReferencePoint(display.CenterCenterReferencePoint);
       campoyear.x = 469; campoyear.y = 680; campoyear.alpha = 1; campoyear.oldAlpha = 1 
	   
	   displayDate(); 

   end 
   drawScreen() 

   function disposeTweens(event) 
      cancelAllTweens(); 
      cancelAllTimers(); 
      cancelAllTransitions(); 
   end 

   return menuGroup 
end 

