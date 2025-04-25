-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
function new() 
    local menuGroup = display.newGroup() 
	
	background = display.newImageRect( "img_bg_main.png", 722, 962 ); 
	background.x = 333; background.y = 479; background.alpha = 1; background.oldAlpha = 1 
	menuGroup:insert(background) 
	showLoading();
	changeLang();
	return menuGroup
end 
