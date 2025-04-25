-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 

 function new() 
	_G.currPageType = "splash";
    local numPages = 5 
    local menuGroup = display.newGroup() 
    local disposeAudios 
    local disposeTweens 



    _G.clConfig.kwk_readMe = 0; 

    local curPage = 1 

    local drawScreen = function() 
       local logosplash  
       local bgsplash  
--(2) regular layer 
       bgsplash = display.newImageRect( imgDir.. "img_splash_bg.png", 1542, 963 ); 
       bgsplash.x = 320; bgsplash.y = 481; bgsplash.alpha = 1; bgsplash.oldAlpha = 1 
       menuGroup:insert(bgsplash) 
       menuGroup.bgsplash = bgsplash 


--(2) regular layer 
       logosplash = display.newImageRect( imgDir.. "img_splash_logo.png", 504, 233 ); 
       logosplash.x = 320; logosplash.y = 478; logosplash.alpha = 1; logosplash.oldAlpha = 1 
       menuGroup:insert(logosplash) 
       menuGroup.logosplash = logosplash 
	   
	local http = require("socket.http")
	if http.request( _G.requestHost ) == nil then
		local function onCloseApp( event )
			if "clicked" == event.action then
					os.exit()
			end
		end
		native.showAlert( "Alert", "An internet connection is required to use this application.", { "Exit" }, onCloseApp )
	else
		_G.clConfig.langId = 2;
		_G.currPageType = "init_search";
		-- _G.clConfig.currEstateID = 1721;
		-- _G.currPageType = "apt_book";
		changeLang();
	end
   end 
   drawScreen() 

   function disposeTweens(event) 
      cancelAllTweens(); 
      cancelAllTimers(); 
      cancelAllTransitions(); 
   end 

   return menuGroup 
end 
