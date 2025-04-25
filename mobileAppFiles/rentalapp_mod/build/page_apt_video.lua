-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.currPageType = "apt_video";
	local currTBL;
    local numPages = 6
    local menuGroup = display.newGroup() 
    local disposeAudios 
    local disposeTweens 
	
    local curPage = 6 
	
	local widget = require "widget"
	widget.setTheme("theme_ios")

    local drawScreen = function() 
		getMainMenu(menuGroup);
		
	getBackBtn(nil,nil,nil,nil)
	getAptAddToFav();
	getLastSearchDates();
    local function fillData() 
		getAptHeader();
		

	   
	end  
	 
	currTBL = _G.clConfig.currEstateTB;
	fillData() 
	
end 
   
	drawScreen();
   function disposeTweens(event) 
      cancelAllTweens(); 
      cancelAllTimers(); 
      cancelAllTransitions(); 
   end 
   return menuGroup 
end 

-- Declare the visible width and height of the screen as  constants
_W = display.viewableContentWidth
_H = display.viewableContentHeight

local Button = {};
Button.new = function(params)
	
	local btn = display.newGroup();
	
	local offIMG = params and params.off or "";
	local onIMG = params and params.on or "";
	
	local off = display.newImageRect(offIMG, 120, 120);
	local on = display.newImageRect(onIMG, 120, 120);

	on.alpha = 0;

	btn:insert(off);
	btn:insert(on);
	
	btn.x = params and params.x or 0;
	btn.y = params and params.y or 0;
	
	function btn:touch(e)
		if(e.phase == "began") then
			on.alpha = 1;
			display.getCurrentStage():setFocus(self);
			self.hasFocus = true;
		elseif(self.hasFocus) then
			if(e.phase == "ended") then
				
				--Execute the callback function, if present
				if(params) then
					if(params.callback) then
						params.callback(e);
					end
				end
				
				on.alpha = 0;
				display.getCurrentStage():setFocus(nil);
				self.hasFocus = false;
			end
		end
	end
	
	btn:addEventListener("touch",btn);
	
	return btn;
end --end Button class declaration

local videoButton = Button.new({
	off = "playVideo.png",
	on = "playVideoOver.png",
	x = _W*0.5,
	y = _H*0.5,
	callback = function(e)
		media.playVideo("http://www.rentalinrome.com/video/rome-parioli_archimede-penthouse.mp4", media.RemoteSource, true);
	end
});