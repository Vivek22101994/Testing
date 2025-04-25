-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_price";
	local currResponse;
	local currTBL;
	local currAvv;
	local currRowPrice;
    local numPages = 5 
    local menuGroup = display.newGroup() 
	
    local curPage = 5 
    local drawScreen = function() 
		getMainMenu(menuGroup);
	getBackBtn(nil,nil,nil,nil)
	   
	-- se non c'e gia nei preferiti
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
           defaultX = 112, 
           defaultY = 95, 
           overSrc=imgDir.."img_btnaddtofavhov.png", 
           overX = 112, 
           overY = 95, 
           onRelease=onbtnaddtofavTouch, 
           id="btnaddtofavButton" 
		} 

		btnaddtofav.x = 578; btnaddtofav.y = 210; btnaddtofav.alpha = 1; btnaddtofav.oldAlpha = 1 
		favGroup:insert(btnaddtofav)


       frecciaaddfav = display.newImageRect( imgDir.. "img_frecciaaddfav.png", 41, 52 ); 
       frecciaaddfav.x = 578; frecciaaddfav.y = 160; frecciaaddfav.alpha = 1; frecciaaddfav.oldAlpha = 1 
	   favGroup:insert(frecciaaddfav) 
       --menuGroup:insert(favGroup) 

       gtStash.gt_frecciaaddfav_202= gtween.new( frecciaaddfav, 0.3, { x = 578, y = 154, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = true,  delay=0}) 
	end
	

	getLastSearchDates();
    local function fillData() 
-- carica da xml

---	pax_price
--	<price pax="1-2 pax">
--		<night_s1>100,00</night_s1>
--		<night_s2>120,00</night_s2>
--		<night_s3>130,00</night_s3>
--	</price>
		currTBL = _G.clConfig.currEstateTB;
		currRowPrice  = currResponse.child[1]; if(currRowPrice==nil) then currRowPrice = {};print("no prices"); end
		currSeasonList  = currResponse.child[2]; if(currSeasonList==nil) then currSeasonList = {};print("no seasons"); end
		currOfferList  = currResponse.child[3]; if(currOfferList==nil) then currOfferList = {};print("no discounts"); end
		currLongStayDiscountList  = currResponse.child[4]; if(currLongStayDiscountList==nil) then currLongStayDiscountList = {};print("no LongStayDiscounts"); end
		local apt_id = currTBL.child[1].value
		local apt_name = currTBL.child[2].value
		local apt_vote = currTBL.child[3].value; if(apt_vote==nil) then apt_vote = "9"; end
		local apt_img = currTBL.child[4].value; if(apt_img==nil) then apt_img = ""; end
		local zone_id = currTBL.child[5].value; if(zone_id==nil) then zone_id = "0"; end
		local zone_name = currTBL.child[6].value; if(zone_name==nil) then zone_name = "Rome"; end
		local summary = currTBL.child[7].value; if(summary==nil) then summary = ""; end

		getAptHeader();
		local aptDettCenter = widget.newScrollView{
			top =340,
			height = 410,
			maskFile="img_mask_apt_dett_center.png",
			hideBackground=true
		}
		aptDettCenter.isHitTestMasked = true;
		aptDettCenter.content.horizontalScrollDisabled = true;
		local currY=0;
		local function addMainTitle( txt )
			--sfondo fascetta titoli
			titleBg = display.newImageRect( imgDir.. "img_totalpricebg.png", 648, 62 ); 
			titleBg.x = 320; titleBg.y = currY+22; titleBg.alpha = 1; titleBg.oldAlpha = 1
			aptDettCenter:insert(titleBg) 
			local text = display.newText(txt, 30, 0, native.systemFontBold, _G.css.defaultFontSize40);
			text:setTextColor(253, 102, 52)
			text:setReferencePoint(display.TopLeftReferencePoint);
			text.y = currY;
			currY=currY+64;
			aptDettCenter:insert( text )
		end
		local function getRowText( txt, x )
			local totalPriceTxt = display.newText(txt, x , 0, native.systemFontBold, _G.css.defaultFontSize25);
			totalPriceTxt.alpha = 1; totalPriceTxt.oldAlpha = 1 
			totalPriceTxt:setTextColor(51, 51, 102)
			totalPriceTxt:setReferencePoint(display.TopLeftReferencePoint);
			totalPriceTxt.y = currY;
			return totalPriceTxt;
		end
		local function getRowTextGrigio( txt, x )
			local totalPriceTxt = display.newText(txt, x , 0, native.systemFontBold, _G.css.defaultFontSize25);
			totalPriceTxt.alpha = 1; totalPriceTxt.oldAlpha = 1 
			totalPriceTxt:setTextColor(102, 102, 102)
			totalPriceTxt:setReferencePoint(display.TopLeftReferencePoint);
			totalPriceTxt.y = currY;
			return totalPriceTxt;
		end
		
		
		-- Sconti LongStay
		if currLongStayDiscountList.child~=nil and #currLongStayDiscountList.child>0 then
			addMainTitle( _G.lbl.mobileLongStayDiscount )
			for i=1,#currLongStayDiscountList.child do
				local currRow = currLongStayDiscountList.child[i]
				aptDettCenter:insert( getRowText( currRow.value,35 ) )
				currY=currY+48;
			end
		end
		-- Sconti LongStay - end
		
		-- Offerte
		if currOfferList.child~=nil and #currOfferList.child>0 then
			addMainTitle( _G.lbl.mobileSpecialOffers )
			Offer_colWidth = 200
			Offer_tableMargin = 35
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileDateFrom, Offer_tableMargin ) )
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileDateTo, Offer_colWidth+Offer_tableMargin ) )
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileDiscount, Offer_colWidth*2+Offer_tableMargin ) )
			currY=currY+48;
			for i=1,#currOfferList.child do
				local currRow = currOfferList.child[i]
				aptDettCenter:insert( getRowText( currRow.child[1].value, 0+Offer_tableMargin ) )
				aptDettCenter:insert( getRowText( currRow.child[2].value, Offer_colWidth+Offer_tableMargin ) )
				aptDettCenter:insert( getRowText( "-"..currRow.child[3].value.."%", Offer_colWidth*2+Offer_tableMargin ) )
				currY=currY+48;
			end
		end
		-- Offerte - end
		
		-- Tabella prezzi
		addMainTitle( _G.lbl.mobilePriceRates )
		Price_colWidth = 150
		Price_tableMargin = 35
		aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileSeason_1, Price_colWidth+Price_tableMargin ) )
		aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileSeason_2, Price_colWidth*2+Price_tableMargin ) )
		aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileSeason_3, Price_colWidth*3+Price_tableMargin ) )
		currY=currY+48;
		
		for i=1,#currRowPrice.child do
			local currRow = currRowPrice.child[i]
			aptDettCenter:insert( getRowTextGrigio( currRow.properties.pax, 0+Price_tableMargin ) )
			aptDettCenter:insert( getRowText( "€"..currRow.child[1].value, Price_colWidth+Price_tableMargin ) )
			aptDettCenter:insert( getRowText( "€"..currRow.child[2].value, Price_colWidth*2+Price_tableMargin ) )
			aptDettCenter:insert( getRowText( "€"..currRow.child[3].value, Price_colWidth*3+Price_tableMargin ) )
			currY=currY+48;
		end
		-- Tabella prezzi - end
		
		-- Stagioni
		if currSeasonList.child~=nil and #currSeasonList.child>0 then
			addMainTitle(  _G.lbl.mobileSeasonReference )
			Season_colWidth = 200
			Season_tableMargin = 35
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileDateFrom, Season_tableMargin ) )
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileDateTo, Season_colWidth+Season_tableMargin ) )
			aptDettCenter:insert( getRowTextGrigio( _G.lbl.mobileSeason, Season_colWidth*2+Season_tableMargin ) )
			currY=currY+48;
			
			for i=1,#currSeasonList.child do
				local currRow = currSeasonList.child[i]
				aptDettCenter:insert( getRowText( currRow.child[1].value, 0+Season_tableMargin ) )
				aptDettCenter:insert( getRowText( currRow.child[2].value, Season_colWidth+Season_tableMargin ) )
				aptDettCenter:insert( getRowText( _G.lbl["mobileSeason_"..currRow.child[3].properties.id], Season_colWidth*2+Season_tableMargin ) )
				currY=currY+48;
			end
		end
		-- Stagioni - end
		
		getAptFooter();
	end  
	 
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			if(_G.currPageType == "apt_price") then
				hideLoading();
				currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
				fillData()
			end
		end
	end
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&action=price";
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
