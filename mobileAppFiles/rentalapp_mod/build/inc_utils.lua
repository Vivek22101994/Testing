module(..., package.seeall)

_G.FKB_gr = nil;
_G.FKB_inputBg = nil;
_G.FKB_txt = nil;
_G.FKB_destroy = function()
	if(_G.FKB_txt ~= nil) then
		_G.FKB_txt:removeSelf();
		_G.FKB_txt = nil;
	end
	if(_G.FKB_gr ~= nil) then
		_G.FKB_gr:removeSelf();
		_G.FKB_gr = nil;
	end		
end

_G.FKB_close = function(callback)
	native.setKeyboardFocus(nil);
	--display.getCurrentStage():setFocus(nil);
	_G.FKB_destroy();
	if(callback ~= nil) then
		timer.performWithDelay( 1, callback )
	end		
end

_G.FKB_slideUp = function()
	gtStash.FKB_txt= gtween.new( _G.FKB_txt, 0.5, {y = _G.FKB_txt.y-900, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
	gtStash.FKB_inputBg= gtween.new( _G.FKB_inputBg, 0.5, {y = _G.FKB_inputBg.y-900, }, {ease = gtween.easing.outCubic, reflect = false,  delay=0});
	timer.performWithDelay( 600, _G.FKB_setFocus )
end
_G.FKB_setFocus = function()
	native.setKeyboardFocus(_G.FKB_txt);
end

_G.FKB_show = function(obj, title, callback, isTextBox, x, y)
	_G.FKB_destroy();
	local padX = 50;
	local padY = 70;
	local btnY = _G.css.FKB_txtHeigth + padY + 5;
	_G.FKB_gr = display.newGroup()
	local overlayImg = ui.newButton{
		defaultSrc=imgDir.."img_overlay.png", 
		defaultX = display.contentWidth, 
		defaultY = display.contentHeight, 
		overSrc=imgDir.."img_overlay.png", 
		overX = display.contentWidth, 
		overY = display.contentHeight
	}
	overlayImg:setReferencePoint(display.TopLeftReferencePoint);
	overlayImg.alpha = 0.1; overlayImg.oldAlpha = 0.1 
	overlayImg.x = 0; overlayImg.y = 0; 
	overlayImg:addEventListener( "touch",  returnFalseEvent )
	_G.FKB_gr:insert(overlayImg) 
	
	_G.FKB_inputBg = display.newGroup()
	local inputBg = display.newImageRect( "img_input_bg.png", display.contentWidth, _G.css.FKB_txtBgHeigth ); 
	inputBg:setReferencePoint(display.TopLeftReferencePoint);
	inputBg.alpha = 1; inputBg.oldAlpha = 1	   
	inputBg.x = 0; inputBg.y = 10;-- display.contentHeight+400; 
	_G.FKB_inputBg:insert(inputBg) 
	
	local lblTitle = display.newText(title, 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lblTitle.alpha = 1; lblTitle.oldAlpha = 1 
	lblTitle.text = title
	lblTitle:setTextColor(51, 51, 102)
	lblTitle.size = _G.css.defaultFontSize30
	lblTitle:setReferencePoint(display.TopCenterReferencePoint);
	lblTitle.x=display.contentWidth/2;
	lblTitle.y=25;
	_G.FKB_inputBg:insert(lblTitle) 
	
	if(isTextBox) then
		_G.FKB_txt = native.newTextBox( padX, _G.css.FKB_txtBgY+padY+900, display.contentWidth-padX*2, _G.css.FKB_txtBoxHeigth)
		_G.FKB_txt.hasBackground = true
		_G.FKB_txt.isEditable = true
	else
		_G.FKB_txt = native.newTextField( padX, _G.css.FKB_txtBgY+padY+900, display.contentWidth-padX*2, _G.css.FKB_txtHeigth)
	end
	_G.FKB_txt.size = _G.css.FKB_txtFontSize
	_G.FKB_txt.alpha = 1; _G.FKB_txt.oldAlpha = 1 
	_G.FKB_txt:setTextColor(255, 106, 48)
	_G.FKB_txt:setReferencePoint(display.TopLeftReferencePoint);
	_G.FKB_txt.text=obj.text;
	_G.FKB_gr:insert(_G.FKB_txt)
	local function FKB_setText()
		obj.text = _G.FKB_txt.text;
		obj:setReferencePoint(display.TopLeftReferencePoint);
		obj.x=x;
		obj.y=y;
		_G.FKB_close(callback);
	end
	local function FKB_fieldHandler( event )
		if ( "began" == event.phase ) then
		elseif ( "ended" == event.phase ) then
		elseif ( "submitted" == event.phase ) then
			FKB_setText()
			return false;
		end
	end
	_G.FKB_txt:addEventListener( 'userInput', FKB_fieldHandler )
	local btnCancel = ui.newButton{
		defaultSrc=imgDir.."img_close_input.png", 
		defaultX = 46, 
		defaultY = 46, 
		overSrc=imgDir.."img_close_input.png", 
		overX = 45, 
		overY = 45
	}
	btnCancel:setReferencePoint(display.TopRightReferencePoint);
	btnCancel.alpha = 1; btnCancel.oldAlpha = 1	   
	btnCancel.x = display.contentWidth; btnCancel.y = 0; 
	_G.FKB_inputBg:insert(btnCancel) 
	local btnCancelInit = false;
	btnCancel:addEventListener( "touch",  function(event)
			if event.phase=="began" then  
				btnCancelInit = true;
			end 
			if event.phase=="ended" then  
				if(btnCancelInit) then
					btnCancelInit = false;
					FKB_setText()
				end
			end 
		end);
	_G.FKB_inputBg:setReferencePoint(display.TopLeftReferencePoint);
	_G.FKB_inputBg.x=0;
	_G.FKB_inputBg.y = _G.css.FKB_txtBgY+900;
	_G.FKB_gr:insert(_G.FKB_inputBg) 
	
	timer.performWithDelay( 100, _G.FKB_slideUp )
	--display.getCurrentStage():setFocus(_G.FKB_txt);
end
_G.FKB_showOld = function(obj, title, callback)
	_G.FKB_destroy();
	local padX = 30;
	local padY = 40;
	local btnY = _G.css.FKB_txtHeigth + padY + 5;
	_G.FKB_gr = display.newGroup()
	local overlayImg = ui.newButton{
		defaultSrc=imgDir.."img_overlay.png", 
		defaultX = display.contentWidth, 
		defaultY = display.contentHeight, 
		overSrc=imgDir.."img_overlay.png", 
		overX = display.contentWidth, 
		overY = display.contentHeight
	}
	overlayImg:setReferencePoint(display.TopLeftReferencePoint);
	overlayImg.alpha = .5; overlayImg.oldAlpha = .5 	   
	overlayImg.x = 0; overlayImg.y = 0; 
	overlayImg:addEventListener( "touch",  returnFalseEvent )
	_G.FKB_gr:insert(overlayImg) 
	
	local lblTitle = display.newText(title, 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lblTitle.alpha = 1; lblTitle.oldAlpha = 1 
	lblTitle.text = title
	lblTitle:setTextColor(255, 255, 255)
	lblTitle.size = _G.css.defaultFontSize30
	lblTitle:setReferencePoint(display.TopCenterReferencePoint);
	lblTitle.x=display.contentWidth/2;
	lblTitle.y=5;
	_G.FKB_gr:insert(lblTitle) 

	_G.FKB_txt = native.newTextField( padX, padY, display.contentWidth-padX*2, _G.css.FKB_txtHeigth)
	_G.FKB_txt.size = _G.css.FKB_txtFontSize
	_G.FKB_txt.alpha = 1; _G.FKB_txt.oldAlpha = 1 
	_G.FKB_txt:setTextColor(255, 106, 48)
	_G.FKB_txt:setReferencePoint(display.TopLeftReferencePoint);
	_G.FKB_txt.x=padX;
	_G.FKB_txt.y=padY;
	_G.FKB_txt.text=obj.text;
	_G.FKB_gr:insert(_G.FKB_txt)
	local function FKB_setText()
		obj.text = _G.FKB_txt.text;
		_G.FKB_close(callback);
	end
	local function FKB_fieldHandler( event )
		if ( "began" == event.phase ) then
		elseif ( "ended" == event.phase ) then
		elseif ( "submitted" == event.phase ) then
			FKB_setText()
			return false;
		end
	end
	_G.FKB_txt:addEventListener( 'userInput', FKB_fieldHandler )
	
	local btnDone = getBtn_btnGriggio( "Done", 100, 46, function(event)
			if event.phase=="ended" then  
				FKB_setText();
			end 
		end);
	btnDone:setReferencePoint(display.TopLeftReferencePoint);
	btnDone.x=padX;
	btnDone.y=btnY;
	_G.FKB_gr:insert(btnDone)
	local btnCancelInit = false;
	local btnCancel = getBtn_btnGriggio( "Cancel", 100, 46, function(event)
			if event.phase=="began" then  
				btnCancelInit = true;
			end 
			if event.phase=="ended" then  
				if(btnCancelInit) then
					btnCancelInit = false;
					_G.FKB_close(callback);
				end
			end 
		end);
	btnCancel:setReferencePoint(display.TopRightReferencePoint);
	btnCancel.x=display.contentWidth-padX;
	btnCancel.y=btnY;
	_G.FKB_gr:insert(btnCancel)
	
	timer.performWithDelay( 500, _G.FKB_setFocus )
	--display.getCurrentStage():setFocus(_G.FKB_txt);
end
_G.FKB_createTextInput = function(x, y, w, h, text, gr, title, callback, isTextBox)
	txtPadX=5;
	txtPadY=2;
	local txtBorder = display.newRoundedRect(x ,y , w, h, 6)
	txtBorder.strokeWidth = 2
	txtBorder:setStrokeColor(180, 180, 180)
	txtBorder:setReferencePoint(display.TopLeftReferencePoint);
	txtBorder.x=x;
	txtBorder.y=y;
	gr:insert(txtBorder)
	x=x+txtPadX;
	y=y+txtPadY;
	local txt = display.newText(text, x,y, w-txtPadX, h-txtPadY, native.systemFont, _G.css.FKB_fakeInputFontSize);
	txt:setTextColor(255, 106, 48)
	txt:setReferencePoint(display.TopLeftReferencePoint);
	txt.x=x;
	txt.y=y;
	gr:insert(txt)
	txtBorder:addEventListener( 'touch', function(event)
			if event.phase=="began" then  
				_G.FKB_show(txt, title, callback, isTextBox, x, y);
				return false;
			end 
		end);
	return txt;
end


_G.FROM_createCheckBox = function(x, y, w, h, gr, callback)
	chk_gr = display.newGroup()
	local chk;
	local onClick;
	local function createChk(state)
		chk = ui.newButton{
			defaultSrc=imgDir.."img_check_"..state..".png", 
			defaultX = 46, 
			defaultY = 46, 
			overSrc=imgDir.."img_check_"..state..".png", 
			overX = 45, 
			overY = 45
		}
		chk:setReferencePoint(display.TopLeftReferencePoint);
		chk.alpha = 1; chk.oldAlpha = 1	   
		chk.x = 0; chk.y = 0; 
		chk_gr.state=state;
		chk_gr:insert(chk)
		chk:addEventListener( "tap", onClick);
	end
	onClick = function(event)
		chk:removeSelf();
		if(chk_gr.state=="ok") then
			createChk("no");
		else
			createChk("ok");
		end
	end
	createChk("no");
	chk_gr:setReferencePoint(display.TopLeftReferencePoint);
	chk_gr.alpha = 1; chk_gr.oldAlpha = 1	   
	chk_gr.x = x; chk_gr.y = y; 
	gr:insert(chk_gr)
	return chk_gr;
end


_G.DRP_gr = nil;
_G.DRP_select = nil;
_G.DRP_destroy = function()
	-- if(_G.DRP_select ~= nil) then
		-- _G.DRP_select:removeSelf();
		-- _G.DRP_select = nil;
	-- end
	if(_G.DRP_gr ~= nil) then
		_G.DRP_gr:removeSelf();
		_G.DRP_gr = nil;
	end		
end

_G.DRP_close = function(callback)
	native.setKeyboardFocus(nil);
	--display.getCurrentStage():setFocus(nil);
	_G.DRP_destroy();
	if(callback ~= nil) then
		timer.performWithDelay( 1, callback )
	end		
end

_G.DRP_setFocus = function()
	native.setKeyboardFocus(_G.DRP_select);
end

_G.DRP_show = function(obj, title, callback)
	_G.DRP_destroy();
	local padX = 30;
	local btnY = _G.css.DRP_selectTop - 100;
	_G.DRP_gr = display.newGroup()
	_G.DRP_gr.width = display.contentWidth;
	_G.DRP_gr.height = display.contentHeight;
	local overlayImg = ui.newButton{
		defaultSrc=imgDir.."img_overlay.png", 
		defaultX = display.contentWidth, 
		defaultY = display.contentHeight-_G.css.DRP_selectTop, 
		overSrc=imgDir.."img_overlay.png", 
		overX = display.contentWidth, 
		overY = display.contentHeight-_G.css.DRP_selectTop
	}
	overlayImg:setReferencePoint(display.TopLeftReferencePoint);
	overlayImg.alpha = .5; overlayImg.oldAlpha = .5 	   
	overlayImg.x = 0; overlayImg.y = 0; 
	overlayImg:addEventListener( "touch",  returnFalseEvent )
	_G.DRP_gr:insert(overlayImg) 
	
	local lblTitle = display.newText(title, 0,0, native.systemFontBold, _G.css.defaultFontSize30);
	lblTitle.alpha = 1; lblTitle.oldAlpha = 1 
	lblTitle.text = title
	lblTitle:setTextColor(255, 255, 255)
	lblTitle.size = _G.css.defaultFontSize30
	lblTitle:setReferencePoint(display.TopCenterReferencePoint);
	lblTitle.x=display.contentWidth/2;
	lblTitle.y=5;
	_G.DRP_gr:insert(lblTitle) 

	local columnData = {}
	columnData[1] = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }
 
	_G.DRP_select = widget.newPickerWheel{
		left=0,
		top=_G.css.DRP_selectTop,
		columns=columnData
	}
	--_G.DRP_gr:insert(_G.DRP_select.view)
	local function DRP_setText()
		obj.text = _G.DRP_select.text;
		_G.DRP_close(callback);
	end
	local function DRP_fieldHandler( event )
		if ( "began" == event.phase ) then
		elseif ( "ended" == event.phase ) then
		elseif ( "submitted" == event.phase ) then
			DRP_setText()
			return false;
		end
	end
	_G.DRP_select:addEventListener( 'userInput', DRP_fieldHandler )
	
	local btnDone = getBtn_btnGriggio( "Done", 100, 46, function(event)
			if event.phase=="ended" then  
				DRP_setText();
			end 
		end);
	btnDone:setReferencePoint(display.TopLeftReferencePoint);
	btnDone.x=padX;
	btnDone.y=btnY;
	_G.DRP_gr:insert(btnDone)
	local btnCancelInit = false;
	local btnCancel = getBtn_btnGriggio( "Cancel", 100, 46, function(event)
			if event.phase=="began" then  
				btnCancelInit = true;
			end 
			if event.phase=="ended" then  
				if(btnCancelInit) then
					btnCancelInit = false;
					_G.DRP_close(callback);
				end
			end 
		end);
	btnCancel:setReferencePoint(display.TopRightReferencePoint);
	btnCancel.x=display.contentWidth-padX;
	btnCancel.y=btnY;
	_G.DRP_gr:insert(btnCancel)
	
	timer.performWithDelay( 1000, _G.DRP_setFocus )
	--display.getCurrentStage():setFocus(_G.DRP_select);
end
_G.DRP_createSelect = function(x, y, w, h, text, gr, title, callback)
	txtPadX=5;
	txtPadY=2;
	local txtBorder = display.newRoundedRect(x ,y , w, h, 6)
	txtBorder.strokeWidth = 2
	txtBorder:setStrokeColor(180, 180, 180)
	txtBorder:setReferencePoint(display.TopLeftReferencePoint);
	txtBorder.x=x;
	txtBorder.y=y;
	gr:insert(txtBorder)
	local txt = display.newText(text, x,y, w-txtPadX, h-txtPadY, native.systemFontBold, _G.css.DRP_selectFontSize);
	txt:setTextColor(255, 106, 48)
	txt:setReferencePoint(display.TopLeftReferencePoint);
	txt.x=x+txtPadX;
	txt.y=y+txtPadY;
	gr:insert(txt)
	txtBorder:addEventListener( 'touch', function(event)
			if event.phase=="began" then  
				_G.DRP_show(txt, title, callback);
				return false;
			end 
		end);
	return txt;
end


_G.ErrorTtp_gr = nil;
_G.ErrorTtp_show = function(txt, y)
	_G.ErrorTtp_gr = display.newGroup()
	local errorToolTipBg = display.newImageRect( imgDir.. "img_error_tooltip_bg.png", display.contentWidth-0, 80 ); 
	errorToolTipBg:setReferencePoint(display.CenterCenterReferencePoint);	
	_G.ErrorTtp_gr:insert(errorToolTipBg)
	errorToolTipBg.x = (display.contentWidth-0)/2; errorToolTipBg.y = 0; errorToolTipBg.alpha = 1; errorToolTipBg.oldAlpha = 1 
	
	local errorToolTipLbl = display.newText(txt, 0,0,0,0, native.systemFontBold, 28);
	errorToolTipLbl:setTextColor(255, 255, 255)
	errorToolTipLbl:setReferencePoint(display.CenterCenterReferencePoint);	
	_G.ErrorTtp_gr:insert(errorToolTipLbl)
	errorToolTipLbl.x = (display.contentWidth-0)/2; errorToolTipLbl.y = 10; errorToolTipLbl.alpha = 1; errorToolTipLbl.oldAlpha = 1 
	if(errorToolTipLbl.width > display.contentWidth-50) then errorToolTipLbl.width = display.contentWidth-50; errorToolTipLbl.text = txt; end

	-- errorArrow = display.newImageRect( imgDir.. "img_freccia_a_dx.png", 52, 41 ); 
	-- errorArrow.x = txtX-20; errorArrow.y = y; errorArrow.alpha = 1; errorArrow.oldAlpha = 1 
	-- _G.ErrorTtp_gr:insert(errorArrow) 
	-- gtStash.gt_frecciaaddfav_202= gtween.new( frecciaaddfav, 0.3, { x = 578, y = 154, rotation = 0, xScale = 1, yScale = 1, alpha=1}, {ease = gtween.easing.Linear, repeatCount = math.huge, reflect = true,  delay=0}) 

	_G.ErrorTtp_gr:setReferencePoint(display.TopLeftReferencePoint);	
	_G.ErrorTtp_gr.y=y;
	_G.ErrorTtp_gr.x=0;
end

