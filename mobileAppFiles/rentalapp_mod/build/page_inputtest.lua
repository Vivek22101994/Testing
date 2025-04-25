-- Code created by Kwik - Copyright: kwiksher.com 
-- Version: 1.9.7f 
module(..., package.seeall) 
imgDir = "" 
audioDir = "" 
function new() 
	_G.prevPageType = _G.currPageType;
	_G.currPageType = "contactus";
	local currResponse;
	local currTBL;
    local menuGroup = display.newGroup()
	
    local curPage = 5 
	local Allen = require "inc_AllenFunctions"
	local widget = require "widget"
	widget.setTheme("theme_ios")

       background = display.newImageRect( "img_bg_main.png", 722, 962 ); 
       background.x = 333; background.y = 479; background.alpha = 1; background.oldAlpha = 1 
	   menuGroup:insert(background) 
       menuGroup.background = background 
	fldY = 90;
	fldYdiff=40;

	local txt_1 = native.newTextField( 50, fldY, 300, 50 )
	txt_1.size = 10
	txt_1:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+50;

	local txt_2 = native.newTextField( 50, fldY, 300, 50 )
	txt_1.size = 16
	txt_1:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+50;

	local txt_3 = native.newTextField( 50, fldY, 300, 50 )
	txt_3.size = 24
	txt_3:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+50;

	local txt_4 = native.newTextField( 50, fldY, 300, 60 )
	txt_4.size = 10
	txt_4:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+60;

	local txt_5 = native.newTextField( 50, fldY, 300, 60 )
	txt_5.size = 16
	txt_5:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+60;

	local txt_6 = native.newTextField( 50, fldY, 300, 60 )
	txt_6.size = 24
	txt_6:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+60;

	local txt_7 = native.newTextField( 50, fldY, 300, 70 )
	txt_7.size = 10
	txt_7:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+70;

	local txt_8 = native.newTextField( 50, fldY, 300, 70 )
	txt_8.size = 16
	txt_8:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+70;

	local txt_9 = native.newTextField( 50, fldY, 300, 70 )
	txt_9.size = 24
	txt_9:setTextColor(255, 106, 48)
	fldY = fldY+fldYdiff+70;


	return menuGroup 
end 
