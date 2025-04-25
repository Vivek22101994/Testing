-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_extrainfo";
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
		currTBL = _G.clConfig.currEstateTB;
		currAmenities =  currResponse.child[4]; if(currAmenities==nil) then currAmenities = {};print("no Amenities"); end
		currSleeps =  currResponse.child[5]; 
		currExtrainfo =  currResponse.child[6]; if(currExtrainfo==nil) then currExtrainfo = {};print("no Extrainfo"); end
		getAptHeader();
		local aptDettCenter = widget.newScrollView{
			top =340,
			height = 410,
			maskFile="img_mask_apt_dett_center.png",
			hideBackground=true
		}
		aptDettCenter.isHitTestMasked = true;
		aptDettCenter.content.horizontalScrollDisabled = true;
		local currY=30;
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
		
		
		addMainTitle( _G.lbl.mobileBtnExtrainfo )
		aptDettCenter:insert( getRowText( currSleeps.properties.title, 23 ) )
		currY=currY+40;
		if #currSleeps.child==nil then
			-- solo per il controllo in piu
		elseif #currSleeps.child>0 then
			for i=1,#currSleeps.child do
				local currRow = currSleeps.child[i]
				aptDettCenter:insert( getRowText( "- "..currRow.value, 50 ) )
				currY=currY+40;
			end
		end
		if #currExtrainfo.child==nil then
			-- solo per il controllo in piu
		elseif #currExtrainfo.child>0 then
			for i=1,#currExtrainfo.child do
				local currRow = currExtrainfo.child[i]
				aptDettCenter:insert( getRowText( currRow.value, 25 ) )
				currY=currY+40;
			end
		end
		if #currAmenities.child==nil then
			-- solo per il controllo in piu
		elseif #currAmenities.child>0 then
		currY=currY+18;
			addMainTitle( _G.lbl.lblAmenities )
			currAmenitiesRows={};
			imgW=72;
			imgH=64;
			imgPad=5;
			indexRow = 1;
			indexColumn = 1;
			rowWidth = 0;
			
			currY=currY+10;
			
			for i=1,#currAmenities.child do
				local currRow = currAmenities.child[i]
				local title = currRow.properties.title
				local img = currRow.properties.img
				if currAmenitiesRows[indexRow] == nil then currAmenitiesRows[indexRow] = display.newGroup(); end
				local tmp = display.newImageRect(img, imgW, imgH);
				tmp:setReferencePoint(display.TopLeftReferencePoint);
				tmp.width=imgW
				tmp.height=imgH
				tmp.y=0
				tmp.x=25+rowWidth
				currAmenitiesRows[indexRow]:insert(tmp)
				indexColumn = indexColumn+1;
				rowWidth = rowWidth + imgW + imgPad;
				if rowWidth + imgW+50 > display.contentWidth then indexRow = indexRow+1; indexColumn = 1; rowWidth = 0; end
			end
			for i=1,#currAmenitiesRows do
				aptDettCenter:insert( currAmenitiesRows[i] )
				currAmenitiesRows[i].x=0;
				currAmenitiesRows[i].y=currY;
				currY=currY+currAmenitiesRows[i].height+imgPad;
			end
			
		end
		currY=currY+27;
		local ApartmentDescription = currTBL.child[7].value; if(ApartmentDescription==nil) then ApartmentDescription = ""; end
		if ApartmentDescription~="" then
			addMainTitle( _G.lbl.mobileApartmentDescription )
			local txtApartmentDescription = display.newText( "", 25, 25, 605, 0, native.systemFont, _G.css.defaultFontSize23 )
			txtApartmentDescription.alpha = 1; txtApartmentDescription.oldAlpha = 1 
			txtApartmentDescription.text = ApartmentDescription
			txtApartmentDescription:setTextColor(102, 102, 102)
			txtApartmentDescription.size = _G.css.defaultFontSize23
			txtApartmentDescription:setReferencePoint(display.TopLeftReferencePoint);
			txtApartmentDescription.x=25;
			txtApartmentDescription.y=currY;
			aptDettCenter:insert( txtApartmentDescription )
			currY=currY+txtApartmentDescription.height;
			aptDettCenter:insert( getRowText( "", 25 ) )
			currY=currY+60;
		end
		

		getAptFooter();
	end  
	 
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			if(_G.currPageType == "apt_extrainfo") then
				hideLoading();
				currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
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
