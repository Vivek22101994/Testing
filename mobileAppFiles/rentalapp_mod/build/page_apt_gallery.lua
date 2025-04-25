-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
local screenW, screenH = display.contentWidth, display.contentHeight
local viewableScreenW, viewableScreenH = display.viewableContentWidth, display.viewableContentHeight
local screenOffsetW, screenOffsetH = display.contentWidth -  display.viewableContentWidth, display.contentHeight - display.viewableContentHeight
 
local imgNum = nil
local images = nil
local touchListener, nextImage, prevImage, cancelMove, initImage
local background
local imageNumberText, imageNumberTextShadow
local currTBL;
function new() 
	_G.currPageType = "apt_gallery";
	currTBL = _G.clConfig.currEstateTB;
	currGallery  = _G.clConfig.currEstateGallery;
	local apt_id = currTBL.child[1].value
	local slideView = require "inc_slideView"
    local menuGroup = display.newGroup() 
	local loading;
	local loadingTxt;
    local drawScreen = function() 
		getMainMenu(menuGroup);
		getLastSearchDates();
		getBackBtn(nil,nil,nil,nil);
		getAptHeader();
		
		loading = display.newImageRect( imgDir.. "loading.png", 200, 200 ); 
		loading:setReferencePoint(display.CenterCenterReferencePoint);
		loading.x = 320; loading.y = 480; loading.alpha = 1; loading.oldAlpha = 1 
		
		menuGroup:insert( loading )

		gtStash.gt_loading= gtween.new( loading, 0.5, { rotation = 360 }, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = false,  delay=0})
		
	-- (5) scritta loading
		loadingTxt = display.newText("", 0,0, native.systemFontBold, _G.css.defaultFontSize23);
		loadingTxt:setTextColor(105, 95, 111)
		loadingTxt.text = "loading..."
		loadingTxt.size = _G.css.defaultFontSize23
		loadingTxt:setReferencePoint(display.CenterCenterReferencePoint);
		loadingTxt.x=320;
		loadingTxt.y=480;
		menuGroup:insert(loadingTxt)
		
        local slideBackground=nil
        local pad = 0
        local top = 346 
        local bottom = 0

		local imageSet = {}               
		imagesArray = {}
		imgW = display.contentWidth;
		imgH = imgW*540/960
		for i=1,#currGallery.child do
			local currImg = currGallery.child[i]
			imagesArray[i]=currImg.properties.host.."/cache/"..imgW.."x"..imgH.."/"..currImg.child[2].value
		end
		local function loadGalleryImgs(i)
			function imgLoaded( event ) 
				if ( event.isError ) then
					print ( "Network error - download failed" )
				elseif(_G.currPageType == "apt_gallery") then
					imageSet[i] = "apt_img_big_"..apt_id.."-"..i..".jpg";
					if(i < #imagesArray) then 
						loadGalleryImgs(i+1) 
					else 
						loading:removeSelf();
						loadingTxt:removeSelf();
						slideView.new( imageSet, slideBackground, top, bottom ) 
					end
				end
			end
			print ( "Downloading: "..imagesArray[i] )
			network.download( imagesArray[i], "GET", imgLoaded, "apt_img_big_"..apt_id.."-"..i..".jpg", system.TemporaryDirectory )
		end
		loadGalleryImgs(1);		  
		
		local function loadAptImgs(i)
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

	end  
	 
	
   
	drawScreen();
   function disposeTweens(event) 
      cancelAllTweens(); 
      cancelAllTimers(); 
      cancelAllTransitions(); 
   end 
   return menuGroup 
end 
