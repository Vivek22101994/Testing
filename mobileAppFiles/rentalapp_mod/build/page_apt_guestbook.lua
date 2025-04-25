-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_guestbook";
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
		currGuestbook  = currResponse if(currGuestbook==nil) then currGuestbook = {}; print("no Guestbook"); end
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
		-- Guestbook
		local txtMainTitle = display.newText("Guestbook", 30, 0, native.systemFontBold, _G.css.defaultFontSize40);
		txtMainTitle:setTextColor(253, 102, 52)
		txtMainTitle:setReferencePoint(display.TopLeftReferencePoint);
		txtMainTitle.y = currY;
		currY=currY+64;
		aptDettCenter:insert( txtMainTitle )
		for i=1,#currGuestbook.child do
			local currRow = currGuestbook.child[i]
			tmp = currRow.child[2].value
			if (currRow.child[3].value ~= nil and currRow.child[3].value ~= "") then
				tmp = tmp.. " (" .. currRow.child[3].value ..")"
			end
			local txtNameFull = display.newText(tmp, 25 , 0, native.systemFontBold, _G.css.defaultFontSize30);
			txtNameFull.alpha = 1; txtNameFull.oldAlpha = 1 
			txtNameFull:setTextColor(51, 51, 102)
			txtNameFull:setReferencePoint(display.TopLeftReferencePoint);
			txtNameFull.y = currY;
			aptDettCenter:insert( txtNameFull )
			currY=currY+36;
			tmp = currRow.child[1].value
			local txtDate = display.newText(tmp, 25 , 0, native.systemFontBold, _G.css.defaultFontSize25);
			txtDate.alpha = 1; txtNameFull.oldAlpha = 1 
			txtDate:setTextColor(253, 102, 52)
			txtDate:setReferencePoint(display.TopLeftReferencePoint);
			txtDate.y = currY;
			aptDettCenter:insert( txtDate )
			currY=currY+32;
			tmp = currRow.child[5].value
			local txtComment = display.newText( "", 25, 0, 580, 0, native.systemFont, _G.css.defaultFontSize23 )
			txtComment.alpha = 1; txtComment.oldAlpha = 1 
			txtComment.text = tmp
			txtComment:setTextColor(102, 102, 102)
			txtComment.size = _G.css.defaultFontSize23
			txtComment:setReferencePoint(display.TopLeftReferencePoint);
			txtComment.x=25;
			txtComment.y=currY;
			aptDettCenter:insert( txtComment )
			currY=currY+txtComment.height+40;
		end
		-- Guestbook - end
		
		getAptFooter();
	end  
	 
	local function networkListener( event )
        if ( event.isError ) then
                print ( "Network error - download failed: " .. event.response )
        else
			--print ( "RESPONSE: " .. event.response )
			if(_G.currPageType == "apt_guestbook") then
				hideLoading();
				currResponse = xml:ParseXmlText(event.response);-- xml:loadFile("currlist_.xml", system.TemporaryDirectory)
				fillData()
			end
		end
	end
	local xmlFilter= "";
	xmlFilter=xmlFilter.."&mobilexml=true";
	xmlFilter=xmlFilter.."&action=guestbook";
	xmlFilter=xmlFilter.."&lang=".._G.clConfig.langId;
	xmlFilter=xmlFilter.."&id=".._G.clConfig.currEstateID;
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
