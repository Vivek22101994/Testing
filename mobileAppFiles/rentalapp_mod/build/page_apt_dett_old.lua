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
--		<foto host="http://www.rentalinrome.com">
--			<thumb>romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</thumb>
--			<big>romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</big>
--		</foto>
--		<foto host="http://www.rentalinrome.com">
--			<thumb>romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/thumb/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</thumb>
--			<big>romeapartmentsphoto/trevi-fountain_view-apartment2/gallery/Trevi_Fountain_Luxury_Apartment_-_Rental_in_Rome-22.jpg</big>
--		</foto>

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
		if(i<7) then
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
--       lineagallery = display.newImageRect( imgDir.. "img_lineazone.png", 654, 12 ); 
--      lineagallery.x = 322; lineagallery.y = 536; lineagallery.alpha = 1; lineagallery.oldAlpha = 1 
	   
	   
--(2) GRUPPO FASCETTA TOTAL PRICE

	   local totalPriceGroup = display.newGroup() 
	   
--(2) sfondo fascetta total price
       totalPricebg = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
       totalPricebg.x = 320; totalPricebg.y = 625; totalPricebg.alpha = 1; totalPricebg.oldAlpha = 1
	   totalPriceGroup:insert(totalPricebg) 
	   
-- (6) testo 1 total price
		
		local totalPriceTxt = display.newText("", 0,0, native.systemFontBold, 30);
		totalPriceTxt.alpha = 1; totalPriceTxt.oldAlpha = 1 
		totalPriceTxt.text = _G.lbl.mobileTotalPrice..":"
		totalPriceTxt:setTextColor(51, 51, 102)
		totalPriceTxt.size = 30
		
		totalPriceTxt:setReferencePoint(display.TopLeftReferencePoint);
		totalPriceTxt.x=36;
		totalPriceTxt.y=608;
		totalPriceGroup:insert(totalPriceTxt) 
		
-- (6) testo 2 total price
		
		local totalPrice = display.newText("", 0,0, native.systemFontBold, 39);
		totalPrice.alpha = 1; totalPrice.oldAlpha = 1 
		totalPrice.text = pr_total.."€"
		totalPrice:setTextColor(255, 106, 48)
		totalPrice.size = 39
		
		totalPrice:setReferencePoint(display.TopRightReferencePoint);
		totalPrice.x=460;
		totalPrice.y=603;
		totalPriceGroup:insert(totalPrice)

	local btnPriceOnTouch = function(event) 
		if event.phase=="ended" then  
			local myClosure_switch = function() 
				disposeTweens() 
				director:changeScene( "page_apt_price", "moveFromRight" ) 
			end 
			timerStash.newTimer_041 = timer.performWithDelay(0, myClosure_switch, 1) 
		end 
	end 
	   
	local btnPrice = ui.newButton{ 
		defaultSrc=imgDir.."apt_btn_prices.png", 
		defaultX = 145, 
		defaultY = 70, 
		overSrc=imgDir.."apt_btn_prices_hov.png", 
		overX = 145, 
		overY = 70, 
		onRelease=btnPriceOnTouch, 
		id="PriceButton" 
	} 
	
	btnPrice:setReferencePoint(display.TopLeftReferencePoint);
	btnPrice.x = 470; btnPrice.y = 590; btnPrice.alpha = 1; btnPrice.oldAlpha = 1 
	
	btnPrice:addEventListener( "touch", btnPriceOnTouch )
	
	--(5) testo Prices
	   local textbtnPrice = display.newText( "", 25, 25, 568, 84, native.systemFont, 23 )

		textbtnPrice.alpha = 1; textbtnPrice.oldAlpha = 1 
		textbtnPrice.text = _G.lbl.mobilePriceRates
		textbtnPrice:setTextColor(255, 255, 255)
		textbtnPrice.size = 23
		textbtnPrice:setReferencePoint(display.TopLeftReferencePoint);
		textbtnPrice.x=525;
		textbtnPrice.y=612; 
		
		
		local gr_pr_part_advance = display.newGroup() 
		--bg_pr_part_advance = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
		--bg_pr_part_advance.x = 320; bg_pr_part_advance.y = 625; bg_pr_part_advance.alpha = 1; bg_pr_part_advance.oldAlpha = 1
		--gr_pr_part_advance:insert(bg_pr_part_advance) 
		local lbl_pr_part_advance_label = display.newText("", 0,0, native.systemFontBold, 30);
		lbl_pr_part_advance_label.alpha = 1; lbl_pr_part_advance_label.oldAlpha = 1 
		lbl_pr_part_advance_label.text = _G.lbl.mobileAcconto..":"
		lbl_pr_part_advance_label:setTextColor(51, 51, 102)
		lbl_pr_part_advance_label.size = 30
		lbl_pr_part_advance_label:setReferencePoint(display.TopLeftReferencePoint);
		lbl_pr_part_advance_label.x=36;
		lbl_pr_part_advance_label.y=680;
		gr_pr_part_advance:insert(lbl_pr_part_advance_label) 
		local lbl_pr_part_advance_Price = display.newText("", 0,0, native.systemFontBold, 39);
		lbl_pr_part_advance_Price.alpha = 1; lbl_pr_part_advance_Price.oldAlpha = 1 
		lbl_pr_part_advance_Price.text = pr_part_advance.."€"
		lbl_pr_part_advance_Price:setTextColor(255, 106, 48)
		lbl_pr_part_advance_Price.size = 39
		lbl_pr_part_advance_Price:setReferencePoint(display.TopRightReferencePoint);
		lbl_pr_part_advance_Price.x=460;
		lbl_pr_part_advance_Price.y=675;
		gr_pr_part_advance:insert(lbl_pr_part_advance_Price)

		local gr_pr_part_onarrival = display.newGroup() 
		--bg_pr_part_onarrival = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
		--bg_pr_part_onarrival.x = 320; bg_pr_part_onarrival.y = 625; bg_pr_part_onarrival.alpha = 1; bg_pr_part_onarrival.oldAlpha = 1
		--gr_pr_part_onarrival:insert(bg_pr_part_onarrival) 
		local lbl_pr_part_onarrival_label = display.newText("", 0,0, native.systemFontBold, 30);
		lbl_pr_part_onarrival_label.alpha = 1; lbl_pr_part_onarrival_label.oldAlpha = 1 
		lbl_pr_part_onarrival_label.text = _G.lbl.mobileSaldoArrivo..":"
		lbl_pr_part_onarrival_label:setTextColor(51, 51, 102)
		lbl_pr_part_onarrival_label.size = 30
		lbl_pr_part_onarrival_label:setReferencePoint(display.TopLeftReferencePoint);
		lbl_pr_part_onarrival_label.x=36;
		lbl_pr_part_onarrival_label.y=732;
		gr_pr_part_onarrival:insert(lbl_pr_part_onarrival_label) 
		local lbl_pr_part_onarrival_Price = display.newText("", 0,0, native.systemFontBold, 39);
		lbl_pr_part_onarrival_Price.alpha = 1; lbl_pr_part_onarrival_Price.oldAlpha = 1 
		lbl_pr_part_onarrival_Price.text = pr_part_onarrival.."€"
		lbl_pr_part_onarrival_Price:setTextColor(255, 106, 48)
		lbl_pr_part_onarrival_Price.size = 39
		lbl_pr_part_onarrival_Price:setReferencePoint(display.TopRightReferencePoint);
		lbl_pr_part_onarrival_Price.x=460;
		lbl_pr_part_onarrival_Price.y=725;
		gr_pr_part_onarrival:insert(lbl_pr_part_onarrival_Price)

		getAptFooter();
	end  
	 
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
			_G.clConfig.currEstateTB = currResponse.child[1];
			_G.clConfig.currEstateGallery =  currResponse.child[3]; if(_G.clConfig.currEstateGallery==nil) then _G.clConfig.currEstateGallery = {};print("no gallery"); end
			
			fillData()
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
	network.request( xmlUrl, "POST", networkListener, xmlParams)
	
end 
   
	drawScreen();
   return menuGroup 
end 
