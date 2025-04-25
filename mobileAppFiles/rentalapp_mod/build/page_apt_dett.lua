-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_dett";
	local currResponse;
	local currTBL;
	local currAvv;
	local currGallery;
    local numPages = 5 
    local menuGroup = display.newGroup() 
	
    local curPage = 5 
    local drawScreen = function() 
		getMainMenu(menuGroup);

	getBackBtn("lista_search","flip",nil,nil)
	getAptAddToFav();
	getLastSearchDates();
	
    local function fillData() 
		currTBL = _G.clConfig.currEstateTB;
		currAvv = currResponse.child[2];
		currGallery  = _G.clConfig.currEstateGallery;
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
		local pr_total_rate = currAvv.child[5].value; if(pr_total_rate==nil) then pr_total_rate = "error"; end
		local pr_discount_longstay = currAvv.child[6].value; if(pr_discount_longstay==nil or pr_discount_longstay..""=="0,00") then pr_discount_longstay = "0"; end
		local pr_discount_specialoffer = currAvv.child[7].value; if(pr_discount_specialoffer==nil or pr_discount_specialoffer..""=="0,00") then pr_discount_specialoffer = "0"; end
		local pr_agency_fee = currAvv.child[8].value; if(pr_agency_fee==nil or pr_agency_fee..""=="0,00") then pr_agency_fee = "0"; end
		local pr_welcome_service = currAvv.child[9].value; if(pr_welcome_service==nil or pr_welcome_service..""=="0,00") then v = "0"; end
		local pr_cleaning_service = currAvv.child[10].value; if(pr_cleaning_service==nil or pr_cleaning_service..""=="0,00") then pr_cleaning_service = "0"; end
		--local pr_part_onarrival = currAvv.child[4].value; if(pr_part_onarrival==nil) then pr_part_onarrival = "error"; end

		getAptHeader();
	imagesArray = {}
	local viewGallery = function(event)
		if event.phase=="ended" then  
			local myClosure_switch = function() 
				disposeTweens() 
				director:changeScene( "page_apt_gallery", "moveFromRight" ) 
			end 
			timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
		end 
	end
	local function loadGalleryImgs(i)
		if(imagesArray[i]==nil) then print( "imagesArray["..i.."]==nil"); return; end
		if(not imagesArray[i].isLoaded) then 
			--print( "- loading:"..imagesArray[i].thumb);
			imagesArray[i].isLoaded = true;
			lastLoadedImg = i;
			function imgLoaded( event ) 
				if ( event.isError ) then
						print ( "Network error - download failed" )
				elseif(_G.currPageType == "apt_dett") then
					local imgdet = event.target; 
					imgdet.width = 177;
					imgdet.height = 99;
					imgdet:setReferencePoint(display.TopLeftReferencePoint);
					imgdet.x = imagesArray[i].x; 
					imgdet.y = imagesArray[i].y; 
					imgdet.alpha = 1; imgdet.oldAlpha = 1 
					imgdet:addEventListener( "touch", viewGallery )
					if(imagesArray[i].img) then imagesArray[i].img:removeSelf(); end
					if(i < #imagesArray) then loadGalleryImgs(i+1) end
				end
				--print( "- loaded:"..imagesArray[i].thumb);
			end
			display.loadRemoteImage( imagesArray[i].thumb, "GET", imgLoaded, "apt_img_dett_"..apt_id.."-"..i..".jpg", system.TemporaryDirectory, -2000, -2000);
		else
			if(i < #imagesArray) then loadGalleryImgs(i+1) end
		end
	end
	for i=1,#currGallery.child do
		local currImg = currGallery.child[i]
		currImg.thumb = currImg.properties.host.."/cache/177x99/"..currImg.child[1].value;--.."/cache/177x99/"
		currImg.big = currImg.properties.host.."/"..currImg.child[2].value
		if(i<4) then
			currImg.y=362;
			currImg.x=34;
			if(i>3) then currImg.y=currImg.y+99+19; end
			if(i==2 or i==5) then currImg.x=currImg.x+177+21; end
			if(i==3 or i==6) then currImg.x=currImg.x+((177+21)*2); end
			
			local fotoapt = display.newImageRect( imgDir.. "img_loading_imglista.png", 95,95 ); 
			fotoapt.alpha = 1; fotoapt.oldAlpha = 1;
			--fotoapt:setReferencePoint(display.TopLeftReferencePoint);
			fotoapt.x = currImg.x+65; 
			fotoapt.y = currImg.y+45; 
			   
			currImg.img=fotoapt;
			currImg.apt_id=apt_id;
			currImg.gtween = gtween.new( fotoapt, 0.7, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0});
			--currImg.showOn = 650-itemHeight;
			currImg.isLoaded = false;
			imagesArray[i]=currImg;
		end
	end
	loadGalleryImgs(1);
--(2) regular layer 
      lineagallery = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
     lineagallery.x = 322; lineagallery.y = 482; lineagallery.alpha = 1; lineagallery.oldAlpha = 1 
	   
	   
--(2) GRUPPO FASCETTA TOTAL PRICE
	currY = 500
	pricePad = 31
	labelposX = 36
	labelsize = _G.css.defaultFontSize25
	labelRefpoint = display.TopLeftReferencePoint;
	priceposX = 600
	pricesize = _G.css.defaultFontSize30
	priceRefpoint = display.TopRightReferencePoint;
	local gr_pr_total_rate = display.newGroup() 
	--bg_pr_total_rate = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
	--bg_pr_total_rate.x = 320; bg_pr_total_rate.y = currY; bg_pr_total_rate.alpha = 1; bg_pr_total_rate.oldAlpha = 1
	--gr_pr_total_rate:insert(bg_pr_total_rate) 
	local lbl_pr_total_rate_label = display.newText("", 0,0, native.systemFontBold, 30);
	lbl_pr_total_rate_label.alpha = 1; lbl_pr_total_rate_label.oldAlpha = 1 
	lbl_pr_total_rate_label.text = _G.lbl.mobileTotalRate..":"
	lbl_pr_total_rate_label:setTextColor(51, 51, 102)
	lbl_pr_total_rate_label.size = labelsize
	lbl_pr_total_rate_label:setReferencePoint(labelRefpoint);
	lbl_pr_total_rate_label.x=labelposX;
	lbl_pr_total_rate_label.y=currY;
	gr_pr_total_rate:insert(lbl_pr_total_rate_label) 
	local lbl_pr_total_rate_Price = display.newText("", 0,0, native.systemFontBold, 39);
	lbl_pr_total_rate_Price.alpha = 1; lbl_pr_total_rate_Price.oldAlpha = 1 
	lbl_pr_total_rate_Price.text = pr_total_rate.."€"
	lbl_pr_total_rate_Price:setTextColor(255, 106, 48)
	lbl_pr_total_rate_Price.size = pricesize
	lbl_pr_total_rate_Price:setReferencePoint(priceRefpoint);
	lbl_pr_total_rate_Price.x=priceposX;
	lbl_pr_total_rate_Price.y=currY-5;
	gr_pr_total_rate:insert(lbl_pr_total_rate_Price)
	currY = currY+pricePad;
	
	if(pr_discount_longstay..""~="0") then
	local gr_pr_discount_longstay = display.newGroup() 
	--bg_pr_discount_longstay = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
	--bg_pr_discount_longstay.x = 320; bg_pr_discount_longstay.y = currY; bg_pr_discount_longstay.alpha = 1; bg_pr_discount_longstay.oldAlpha = 1
	--gr_pr_discount_longstay:insert(bg_pr_discount_longstay) 
	local lbl_pr_discount_longstay_label = display.newText("", 0,0, native.systemFontBold, 30);
	lbl_pr_discount_longstay_label.alpha = 1; lbl_pr_discount_longstay_label.oldAlpha = 1 
	lbl_pr_discount_longstay_label.text = _G.lbl.lblDiscountLongStay..":"
	lbl_pr_discount_longstay_label:setTextColor(51, 51, 102)
	lbl_pr_discount_longstay_label.size = labelsize
	lbl_pr_discount_longstay_label:setReferencePoint(labelRefpoint);
	lbl_pr_discount_longstay_label.x=labelposX;
	lbl_pr_discount_longstay_label.y=currY;
	gr_pr_discount_longstay:insert(lbl_pr_discount_longstay_label) 
	local lbl_pr_discount_longstay_Price = display.newText("", 0,0, native.systemFontBold, 39);
	lbl_pr_discount_longstay_Price.alpha = 1; lbl_pr_discount_longstay_Price.oldAlpha = 1 
	lbl_pr_discount_longstay_Price.text = "-"..pr_discount_longstay.."€"
	lbl_pr_discount_longstay_Price:setTextColor(255, 106, 48)
	lbl_pr_discount_longstay_Price.size = pricesize
	lbl_pr_discount_longstay_Price:setReferencePoint(priceRefpoint);
	lbl_pr_discount_longstay_Price.x=priceposX;
	lbl_pr_discount_longstay_Price.y=currY-5;
	gr_pr_discount_longstay:insert(lbl_pr_discount_longstay_Price)
	currY = currY+pricePad;
	end
	
	if(pr_discount_specialoffer..""~="0") then
	local gr_pr_discount_specialoffer = display.newGroup() 
	--bg_pr_discount_specialoffer = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
	--bg_pr_discount_specialoffer.x = 320; bg_pr_discount_specialoffer.y = currY; bg_pr_discount_specialoffer.alpha = 1; bg_pr_discount_specialoffer.oldAlpha = 1
	--gr_pr_discount_specialoffer:insert(bg_pr_discount_specialoffer) 
	local lbl_pr_discount_specialoffer_label = display.newText("", 0,0, native.systemFontBold, 30);
	lbl_pr_discount_specialoffer_label.alpha = 1; lbl_pr_discount_specialoffer_label.oldAlpha = 1 
	lbl_pr_discount_specialoffer_label.text = _G.lbl.lblDiscountSpecialOffer..":"
	lbl_pr_discount_specialoffer_label:setTextColor(51, 51, 102)
	lbl_pr_discount_specialoffer_label.size = labelsize
	lbl_pr_discount_specialoffer_label:setReferencePoint(labelRefpoint);
	lbl_pr_discount_specialoffer_label.x=labelposX;
	lbl_pr_discount_specialoffer_label.y=currY;
	gr_pr_discount_specialoffer:insert(lbl_pr_discount_specialoffer_label) 
	local lbl_pr_discount_specialoffer_Price = display.newText("", 0,0, native.systemFontBold, 39);
	lbl_pr_discount_specialoffer_Price.alpha = 1; lbl_pr_discount_specialoffer_Price.oldAlpha = 1 
	lbl_pr_discount_specialoffer_Price.text = "-"..pr_discount_specialoffer.."€"
	lbl_pr_discount_specialoffer_Price:setTextColor(255, 106, 48)
	lbl_pr_discount_specialoffer_Price.size = pricesize
	lbl_pr_discount_specialoffer_Price:setReferencePoint(priceRefpoint);
	lbl_pr_discount_specialoffer_Price.x=priceposX;
	lbl_pr_discount_specialoffer_Price.y=currY-5;
	gr_pr_discount_specialoffer:insert(lbl_pr_discount_specialoffer_Price)
	currY = currY+pricePad;
	end
	
	if(pr_agency_fee..""~="0") then
	local gr_pr_agency_fee = display.newGroup() 
	--bg_pr_agency_fee = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
	--bg_pr_agency_fee.x = 320; bg_pr_agency_fee.y = currY; bg_pr_agency_fee.alpha = 1; bg_pr_agency_fee.oldAlpha = 1
	--gr_pr_agency_fee:insert(bg_pr_agency_fee) 
	local lbl_pr_agency_fee_label = display.newText("", 0,0, native.systemFontBold, 30);
	lbl_pr_agency_fee_label.alpha = 1; lbl_pr_agency_fee_label.oldAlpha = 1 
	lbl_pr_agency_fee_label.text = _G.lbl.rnt_pr_part_agency_fee..":"
	lbl_pr_agency_fee_label:setTextColor(51, 51, 102)
	lbl_pr_agency_fee_label.size = labelsize
	lbl_pr_agency_fee_label:setReferencePoint(labelRefpoint);
	lbl_pr_agency_fee_label.x=labelposX;
	lbl_pr_agency_fee_label.y=currY;
	gr_pr_agency_fee:insert(lbl_pr_agency_fee_label) 
	local lbl_pr_agency_fee_Price = display.newText("", 0,0, native.systemFontBold, 39);
	lbl_pr_agency_fee_Price.alpha = 1; lbl_pr_agency_fee_Price.oldAlpha = 1 
	lbl_pr_agency_fee_Price.text = pr_agency_fee.."€"
	lbl_pr_agency_fee_Price:setTextColor(255, 106, 48)
	lbl_pr_agency_fee_Price.size = pricesize
	lbl_pr_agency_fee_Price:setReferencePoint(priceRefpoint);
	lbl_pr_agency_fee_Price.x=priceposX;
	lbl_pr_agency_fee_Price.y=currY-5;
	gr_pr_agency_fee:insert(lbl_pr_agency_fee_Price)
	currY = currY+pricePad;
	end

	currY = currY+15;
	local totalPriceGroup = display.newGroup() 
	local gr_totalPrice = display.newGroup() 
	   
--(2) sfondo fascetta total price
       totalPricebg = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
       totalPricebg.x = 320; totalPricebg.y = currY+18; totalPricebg.alpha = 1; totalPricebg.oldAlpha = 1
	   gr_totalPrice:insert(totalPricebg) 
	   
-- (6) testo 1 total price
	local lbl_totalPrice_label = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lbl_totalPrice_label.alpha = 1; lbl_totalPrice_label.oldAlpha = 1 
	lbl_totalPrice_label.text = _G.lbl.mobileTotalPrice..":"
	lbl_totalPrice_label:setTextColor(51, 51, 102)
	lbl_totalPrice_label.size = _G.css.defaultFontSize30
	lbl_totalPrice_label:setReferencePoint(labelRefpoint);
	lbl_totalPrice_label.x=labelposX;
	lbl_totalPrice_label.y=currY;
	gr_totalPrice:insert(lbl_totalPrice_label) 
	local lbl_totalPrice_Price = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
	lbl_totalPrice_Price.alpha = 1; lbl_totalPrice_Price.oldAlpha = 1 
	lbl_totalPrice_Price.text = pr_total.."€"
	lbl_totalPrice_Price:setTextColor(255, 106, 48)
	lbl_totalPrice_Price.size = _G.css.defaultFontSize40
	lbl_totalPrice_Price:setReferencePoint(priceRefpoint);
	lbl_totalPrice_Price.x=priceposX;
	lbl_totalPrice_Price.y=currY-5;
	gr_totalPrice:insert(lbl_totalPrice_Price) 
	currY = currY+50;

	local btnPriceOnTouch = function(event) 
		if event.phase=="ended" then  
			local myClosure_switch = function() 
				disposeTweens() 
				director:changeScene( "page_apt_price", "moveFromRight" ) 
			end 
			timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
		end 
	end 
		
		
		local gr_pr_part_advance = display.newGroup() 
		--bg_pr_part_advance = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
		--bg_pr_part_advance.x = 320; bg_pr_part_advance.y = 625; bg_pr_part_advance.alpha = 1; bg_pr_part_advance.oldAlpha = 1
		--gr_pr_part_advance:insert(bg_pr_part_advance) 
		local lbl_pr_part_advance_label = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize30);
		lbl_pr_part_advance_label.alpha = 1; lbl_pr_part_advance_label.oldAlpha = 1 
		lbl_pr_part_advance_label.text = _G.lbl.mobileAcconto..":"
		lbl_pr_part_advance_label:setTextColor(51, 51, 102)
		lbl_pr_part_advance_label.size = _G.css.defaultFontSize30
		lbl_pr_part_advance_label:setReferencePoint(labelRefpoint);
		lbl_pr_part_advance_label.x=labelposX;
		lbl_pr_part_advance_label.y=currY;
		gr_pr_part_advance:insert(lbl_pr_part_advance_label) 
		local lbl_pr_part_advance_Price = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize40);
		lbl_pr_part_advance_Price.alpha = 1; lbl_pr_part_advance_Price.oldAlpha = 1 
		lbl_pr_part_advance_Price.text = pr_part_advance.."€"
		lbl_pr_part_advance_Price:setTextColor(51, 51, 102)
		lbl_pr_part_advance_Price.size = _G.css.defaultFontSize40
		lbl_pr_part_advance_Price:setReferencePoint(priceRefpoint);
		lbl_pr_part_advance_Price.x=priceposX;
		lbl_pr_part_advance_Price.y=currY-5;
		gr_pr_part_advance:insert(lbl_pr_part_advance_Price)
		currY = currY+40;

		local gr_pr_part_onarrival = display.newGroup() 
		--bg_pr_part_onarrival = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
		--bg_pr_part_onarrival.x = 320; bg_pr_part_onarrival.y = 625; bg_pr_part_onarrival.alpha = 1; bg_pr_part_onarrival.oldAlpha = 1
		--gr_pr_part_onarrival:insert(bg_pr_part_onarrival) 
		local lbl_pr_part_onarrival_label = display.newText("", 0,0, native.systemFontBold, 30);
		lbl_pr_part_onarrival_label.alpha = 1; lbl_pr_part_onarrival_label.oldAlpha = 1 
		lbl_pr_part_onarrival_label.text = _G.lbl.mobileSaldoArrivo..":"
		lbl_pr_part_onarrival_label:setTextColor(51, 51, 102)
		lbl_pr_part_onarrival_label.size = labelsize
		lbl_pr_part_onarrival_label:setReferencePoint(labelRefpoint);
		lbl_pr_part_onarrival_label.x=labelposX;
		lbl_pr_part_onarrival_label.y=currY;
		gr_pr_part_onarrival:insert(lbl_pr_part_onarrival_label) 
		local lbl_pr_part_onarrival_Price = display.newText("", 0,0, native.systemFontBold, 39);
		lbl_pr_part_onarrival_Price.alpha = 1; lbl_pr_part_onarrival_Price.oldAlpha = 1 
		lbl_pr_part_onarrival_Price.text = pr_part_onarrival.."€"
		lbl_pr_part_onarrival_Price:setTextColor(255, 106, 48)
		lbl_pr_part_onarrival_Price.size = pricesize
		lbl_pr_part_onarrival_Price:setReferencePoint(priceRefpoint);
		lbl_pr_part_onarrival_Price.x=priceposX;
		lbl_pr_part_onarrival_Price.y=currY-5;
		gr_pr_part_onarrival:insert(lbl_pr_part_onarrival_Price)
		
		
		if(_G.clConfig.currEstateIndex>1) then
			local btnPrevApt
			local onbtnPrevAptTouch = function(event) 
				if event.phase=="began" then  
					gtStash.PrevBtnSlides= gtween.new( btnPrevApt, 0.2, {x = 0, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
				end
				if event.phase=="ended" then  
					gtStash.PrevBtnSlides= gtween.new( btnPrevApt, 0.2, {x = -28, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
					local myClosure_switch = function()
						disposeTweens() 
						_G.clConfig.currEstateIndex = _G.clConfig.currEstateIndex-1;
						_G.clConfig.currEstateID = _G.clConfig.lastEstateListIds[_G.clConfig.currEstateIndex];
						director:changeScene( "page_reloader", "moveFromLeft" )
					end 
					timerStash.newTimer_319 = timer.performWithDelay(200, myClosure_switch, 1)
				end 
			end 
			btnPrevApt = ui.newButton{ 
				defaultSrc=imgDir.."img_slide_apt_sx.png", 
				defaultX = 100, 
				defaultY = 158, 
				overSrc=imgDir.."img_slide_apt_sx.png", 
				overX = 100, 
				overY =158
			}
			btnPrevApt:setReferencePoint(display.TopLeftReferencePoint);
			btnPrevApt.x = -28; btnPrevApt.y = 470; btnPrevApt.alpha = 1; btnPrevApt.oldAlpha = 1 
			btnPrevApt:addEventListener( "touch", onbtnPrevAptTouch )
		end
		
		if(_G.clConfig.currEstateIndex<#_G.clConfig.lastEstateListIds) then
			local btnNextApt
			local onbtnNextAptTouch = function(event) 
				if event.phase=="began" then  
					gtStash.PrevBtnSlides= gtween.new( btnNextApt, 0.2, {x = 642, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
					
				end 
				if event.phase=="ended" then  
					gtStash.NextBtnSlides= gtween.new( btnNextApt, 0.2, {x = 668, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
					local myClosure_switch = function()
						disposeTweens() 
						_G.clConfig.currEstateIndex = _G.clConfig.currEstateIndex+1;
						_G.clConfig.currEstateID = _G.clConfig.lastEstateListIds[_G.clConfig.currEstateIndex];
						director:changeScene( "page_reloader", "moveFromRight" )
					end 
					timerStash.newTimer_319 = timer.performWithDelay(200, myClosure_switch, 1)
				end 
			end 
			btnNextApt = ui.newButton{ 
				defaultSrc=imgDir.."img_slide_apt_dx.png", 
				defaultX = 100, 
				defaultY = 158, 
				overSrc=imgDir.."img_slide_apt_dx.png", 
				overX = 100, 
				overY =158
			} 
			btnNextApt:setReferencePoint(display.TopRightReferencePoint);
			btnNextApt.x = 668; btnNextApt.y = 470; btnNextApt.alpha = 1; btnNextApt.oldAlpha = 1 
			btnNextApt:addEventListener( "touch", onbtnNextAptTouch )
		end
		
		getAptFooter();
	end  
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			if(_G.currPageType == "apt_dett") then
				hideLoading();
				currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
				_G.clConfig.currEstateTB = currResponse.child[1];
				_G.clConfig.currEstateGallery =  currResponse.child[3]; if(_G.clConfig.currEstateGallery==nil) then _G.clConfig.currEstateGallery = {};print("no gallery"); end
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
