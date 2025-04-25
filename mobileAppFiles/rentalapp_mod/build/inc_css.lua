module(..., package.seeall)

_G.css = {};
_G.css.defaultFontSize22 = 22;
_G.css.defaultFontSize23 = 23;
_G.css.defaultFontSize25 = 25;
_G.css.defaultFontSize27 = 27;
_G.css.defaultFontSize30 = 30;
_G.css.defaultFontSize35 = 35;
_G.css.defaultFontSize40 = 40;
_G.css.defaultFontSize43 = 43;
_G.css.defaultFontSize50 = 50;
_G.css.FKB_setFakeInputText = "";
_G.css.FKB_fakeInputFontSize = 28;
_G.css.FKB_txtFontSize = 16;
_G.css.FKB_txtHeigth = 36;
_G.css.FKB_txtBoxHeigth = 100;
_G.css.FKB_txtBgY = 500;
_G.css.FKB_txtBgHeigth = 136;
_G.css.FKB_singleHeigth = 36;
_G.css.FKB_txtPadY = 10;
_G.css.DRP_selectTop = 460;

_G.cssChange = function(change)
	_G.css.defaultFontSize22 = 22+(change*2);
	_G.css.defaultFontSize23 = 23+(change*2);
	_G.css.defaultFontSize25 = 25+(change*2);
	_G.css.defaultFontSize27 = 27+(change*2);
	_G.css.defaultFontSize30 = 30+(change*3);
	_G.css.defaultFontSize35 = 35+(change*3);
	_G.css.defaultFontSize40 = 40+(change*4);
	_G.css.defaultFontSize43 = 43+(change*4);
	_G.css.defaultFontSize50 = 50+(change*5);
end
_G.cssDefaultSets = function(setType)
	if(setType=="SumsungGalaxy") then
		_G.css.FKB_fakeInputFontSize = 26;
		_G.css.FKB_txtFontSize = 18;
		_G.css.FKB_txtHeigth = 70;
		_G.css.FKB_txtBoxHeigth = 100;
		_G.css.FKB_txtBgY = 460;
		_G.css.FKB_txtBgHeigth = 170;
		_G.css.FKB_singleHeigth = 36;
		_G.css.FKB_txtPadY = 10;
		_G.css.DRP_selectTop = 460;
	elseif(setType=="xxxx") then
	else
	end
end
