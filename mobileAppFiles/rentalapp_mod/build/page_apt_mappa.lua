-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_mappa";
	local currTBL = _G.clConfig.currEstateTB;
    local menuGroup = display.newGroup() 

	local widget = require "widget"
	widget.setTheme("theme_ios")

    local drawScreen = function() 
		getMainMenu(menuGroup);
	
	
       local omino1  
       local omino2  
       local mail  
       local favoriteadd  
       local btnbackdethov  
       local btnbackdet  
       local dettit  
       local logo  
       local headbg  
       local background 
	   local datebg 
	   local datesTxt 
	   local frecciaaddfav  
       local btnaddtofavhov  
       local btnaddtofav  	   
	   local nomeZonaDet

	   local voto
	   local lineazonedet  
	   local imgdet1
	   local imgdet2
	   local imgdet3
	   local imgdet4
	   local imgdet5
	   local imgdet6
	   local lineagallery 
	   local totalPricebg
	   local totalPriceTxt
	   local textApt
	   local leggiTutto
	   local btnCommenti
	   local lineaDesc
	   
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
		getAptHeader();
		aptGmapLat = currTBL.child[8].properties.lat;
		aptGmapLng = currTBL.child[8].properties.lng;
		local apt_name = currTBL.child[2].value
		local zone_name = currTBL.child[6].value; if(zone_name==nil) then zone_name = "Rome"; end
		
		myMap = native.newMapView( 0, 346, display.contentWidth, display.contentHeight-346 )
		myMap.mapType = "standard" -- other mapType options are "satellite" or "hybrid"
		 
		-- The MapView is just another Corona display object, and can be moved or rotated, etc.
		--myMap.x = display.contentWidth / 2
		--myMap.y = 120
		 
		-- Initialize map to a real location, since default location (0,0) is not very interesting
		myMap:setCenter( aptGmapLat, aptGmapLng )
		myMap:addMarker( aptGmapLat, aptGmapLng, { title=""..apt_name, subtitle=""..zone_name })
		local function callMap()
				-- Fetch the user's current location
				-- Note: in XCode Simulator, the current location defaults to Apple headquarters in Cupertino, CA
				local currentLocation = myMap:getUserLocation()
				local currentLatitude = currentLocation.latitude
				local currentLongitude = currentLocation.longitude
				
				-- Move map so that current location is at the center
				--myMap:setCenter( currentLatitude, currentLongitude, true )
				
				-- Look up nearest address to this location (this is returned as a "mapAddress" event, handled above)
				myMap:nearestAddress( currentLatitude, currentLongitude )
		end
		 
		-- A function to handle the "mapAddress" event (also known as "reverse geocoding")
		--
		local mapAddressHandler = function( event )
				local locationText =
						"Latitude: " .. currentLatitude .. 
						", Longitude: " .. currentLongitude ..
						", Address: " .. event.streetDetail .. " " .. event.street ..
						", " .. event.city ..
						", " .. event.region ..
						", " .. event.country ..
						", " .. event.postalCode
						
				local alert = native.showAlert( "You Are Here", locationText, { "OK" } )
		end
		 
		-- A listener for the address lookup result
		-- (This could also be a table listener on the map itself, in case you have more than one simultaneous map.)
		--Runtime:addEventListener( "mapAddress", mapAddressHandler )
		--timer.performWithDelay( 5000, callMap )         -- get current location after 1 second

	   
	end  
	 
	fillData() 
	
end 
   
	drawScreen();
   return menuGroup 
end 
