/***********************************************************************
* FILE: jquery.ptTimeSelect.js
* 
* 		jQuery plug in for displaying a popup that allows a user
* 		to define a time and set that time back to a form's input
* 		field.
* 
* 
* AUTHOR:
* 
* 		*Paul T.*
* 
* 		- <http://www.purtuga.com>
* 		- <http://pttimeselect.sourceforge.net>
* 
* 
* DEPENDECIES:
* 
* 		-	jQuery.js
* 			<http://docs.jquery.com/Downloading_jQuery>
* 
* 
* LICENSE:
* 
* 		Copyright (c) 2007 Paul T. (purtuga.com)
*		Dual licensed under the:
*
* 		-	MIT
* 			<http://www.opensource.org/licenses/mit-license.php>
* 
* 		-	GPL
* 			<http://www.opensource.org/licenses/gpl-license.php>
* 
* INSTALLATION:
* 
* There are two files (.css and .js) delivered with this plugin and
* that must be incluced in your html page after the jquery.js library,
* and prior to making any attempts at using it. Both of these are to
* be included inside of the 'head' element of the document.
* |
* |	<link rel="stylesheet" type="text/css" href="jquery.ptTimeSelect.css" />
* |	<script type="text/javascript" src="jquery.ptTimeSelect.js"></script>
* |
* 
* USAGE:
* 
* 	-	See <$(ele).ptTimeSelect()>
* 
* 
* 
* LAST UPDATED:
* 
* 		- $Date: 2010/05/28 18:45:26 $
* 		- $Author: paulinho4u $
* 		- $Revision: 1.5 $
* 
* 
**********************************************************************/

jQuery.ptTimeSelect = {};

/***********************************************************************
* PROPERTY: jQuery.ptTimeSelect.options
* 		The default options for all timeselect attached elements. Can be
* 		overwriten wiht <jQuery.fn.ptTimeSelect()>
* 
* 	containerClass	-	
* 
* 
*/
jQuery.ptTimeSelect.options = {
	timeCont: undefined,
	timeView: undefined,
	timeToggler: undefined,
	viewAsToggler: false,
	containerClass: undefined,
	containerWidth: undefined,
	hoursLabel: 'Hour',
	minutesLabel: 'Minutes',
	setButtonLabel: 'Set',
	popupImage: undefined,
	onFocusDisplay: true,
	zIndex: 10,
	onBeforeShow: undefined,
	onClose: undefined
};


/***********************************************************************
* METHOD: jQuery.ptTimeSelect._ptTimeSelectInit()
* 		Internal method. Called when page is initalized to add the time
* 		selection area to the DOM.
* 
* PARAMS:
* 
* 		none.
* 
* RETURNS:
* 
* 		nothing.
* 
* 
*/
jQuery.ptTimeSelect._ptTimeSelectInit = function() {
	jQuery(document).ready(
		function() {
			//if the html is not yet created in the document, then do it now
			if (!jQuery('#ptTimeSelectCntr').length) {
				jQuery("body").append(
						'<div id="ptTimeSelectCntr" class="">'
					+ '		<div class="ui-widget ui-widget-content ui-corner-all">'
					+ '		<div class="ui-widget-header ui-corner-all">'
					+ '			<div id="ptTimeSelectCloseCntr" style="float: right;">'
					+ '				<a href="javascript: void(0);" '
					+ '						onmouseover="jQuery(this).removeClass(\'ui-state-default\').addClass(\'ui-state-hover\');" '
					+ '						onmouseout="jQuery(this).removeClass(\'ui-state-hover\').addClass(\'ui-state-default\');"'
					+ '						class="ui-corner-all ui-state-default">'
					+ '					<span class="ui-icon ui-icon-circle-close">X</span>'
					+ '				</a>'
					+ '			</div>'
					+ '			<div id="ptTimeSelectUserTime" style="float: left;">'
					+ '				<span id="ptTimeSelectUserSelHr">1</span> : '
					+ '				<span id="ptTimeSelectUserSelMin">00</span> '
					+ '				<span id="ptTimeSelectUserSelAmPm">AM</span>'
					+ '				<span id="ptTimeSelectUserSelFull" style="display:none;"></span>'
					+ '			</div>'
					+ '			<br style="clear: both;" /><div></div>'
					+ '		</div>'
					+ '		<div class="ui-widget-content ui-corner-all">'
					+ '			<div>'
					+ '				<div class="ptTimeSelectTimeLabelsCntr">'
					+ '					<div class="ptTimeSelectLeftPane" style="width: 50%; text-align: center; float: left;" class="">Hour</div>'
					+ '					<div class="ptTimeSelectRightPane" style="width: 50%; text-align: center; float: left;">Minutes</div>'
					+ '				</div>'
					+ '				<div>'
					+ '					<div style="float: left; width: 50%;">'
					+ '						<div class="ui-widget-content ptTimeSelectLeftPane">'
					+ '							<div class="ptTimeSelectHrAmPmCntr">'
					+ '								<a id="ptTimeSelectAmPm_AM" class="ptTimeSelectAmPm ui-state-default" href="javascript: void(0);" '
					+ '										style="display: block; width: 45%; float: left;">AM</a>'
					+ '								<a id="ptTimeSelectAmPm_PM" class="ptTimeSelectAmPm ui-state-default" href="javascript: void(0);" '
					+ '										style="display: block; width: 45%; float: left;">PM</a>'
					+ '								<br style="clear: left;" /><div></div>'
					+ '							</div>'
					+ '							<div class="ptTimeSelectHrCntr">'
					+ '								<a id="ptTimeSelectHr_01" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">1</a>'
					+ '								<a id="ptTimeSelectHr_02" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">2</a>'
					+ '								<a id="ptTimeSelectHr_03" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">3</a>'
					+ '								<a id="ptTimeSelectHr_04" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">4</a>'
					+ '								<a id="ptTimeSelectHr_05" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">5</a>'
					+ '								<a id="ptTimeSelectHr_06" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">6</a>'
					+ '								<a id="ptTimeSelectHr_07" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">7</a>'
					+ '								<a id="ptTimeSelectHr_08" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">8</a>'
					+ '								<a id="ptTimeSelectHr_09" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">9</a>'
					+ '								<a id="ptTimeSelectHr_10" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">10</a>'
					+ '								<a id="ptTimeSelectHr_11" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">11</a>'
					+ '								<a id="ptTimeSelectHr_12" class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">12</a>'
					+ '								<br style="clear: left;" /><div></div>'
					+ '							</div>'
					+ '						</div>'
					+ '					</div>'
					+ '					<div style="width: 50%; float: left;">'
					+ '						<div class="ui-widget-content ptTimeSelectRightPane">'
					+ '							<div class="ptTimeSelectMinCntr">'
					+ '								<a id="ptTimeSelectMin_00" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">00</a>'
					+ '								<a id="ptTimeSelectMin_05" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">05</a>'
					+ '								<a id="ptTimeSelectMin_10" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">10</a>'
					+ '								<a id="ptTimeSelectMin_15" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">15</a>'
					+ '								<a id="ptTimeSelectMin_20" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">20</a>'
					+ '								<a id="ptTimeSelectMin_25" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">25</a>'
					+ '								<a id="ptTimeSelectMin_30" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">30</a>'
					+ '								<a id="ptTimeSelectMin_35" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">35</a>'
					+ '								<a id="ptTimeSelectMin_40" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">40</a>'
					+ '								<a id="ptTimeSelectMin_45" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">45</a>'
					+ '								<a id="ptTimeSelectMin_50" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">50</a>'
					+ '								<a id="ptTimeSelectMin_55" class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">55</a>'
					+ '								<br style="clear: left;" /><div></div>'
					+ '							</div>'
					+ '						</div>'
					+ '					</div>'
					+ '				</div>'
					+ '			</div>'
					+ '			<div style="clear: left;"></div>'
					+ '		</div>'
					+ '		<div id="ptTimeSelectSetButton">'
					+ '			<a id="ptTimeSelectSetBtn" href="javascript: void(0);"'
					+ '					onmouseover="jQuery(this).removeClass(\'ui-state-default\').addClass(\'ui-state-hover\');" '
					+ '						onmouseout="jQuery(this).removeClass(\'ui-state-hover\').addClass(\'ui-state-default\');"'
					+ '						class="ui-corner-all ui-state-default">'
					+ '				SET'
					+ '			</a>'
					+ '			<br style="clear: both;" /><div></div>'
					+ '		</div>'
					+ '		<!--[if lte IE 6.5]>'
					+ '			<iframe style="display:block; position:absolute;top: 0;left:0;z-index:-1;'
					+ '				filter:Alpha(Opacity=\'0\');width:3000px;height:3000px"></iframe>'
					+ '		<![endif]-->'
					+ '	</div></div>'
				);

				var e = jQuery('#ptTimeSelectCntr');

				// Add the events to the functions
				e.find('.ptTimeSelectMin')
					.bind("click", function() {
						jQuery.ptTimeSelect.setMin($(this).text());
					});

				e.find('.ptTimeSelectHr')
					.bind("click", function() {
						jQuery.ptTimeSelect.setHr($(this).text());
					});
				e.find('.ptTimeSelectAmPm')
					.bind("click", function() {
						jQuery.ptTimeSelect.setHr($(this).text());
					});

				//$(document).mousedown(jQuery.ptTimeSelect._doCheckMouseClick);
			} //end if 
		}
	);
} (); /* jQuery.ptTimeSelectInit() */


jQuery.ptTimeSelect.setHr = function(h) {
	h = "" + h;
	if (h == "") {
		jQuery('.ptTimeSelectAmPm').removeClass("ui-state-active");
		jQuery('.ptTimeSelectHr').removeClass("ui-state-active");
		return;
	}
	if (h.length == 1)
		h = "0" + h;
	if (h.toLowerCase() == "am"
		|| h.toLowerCase() == "pm"
	) {
		jQuery('#ptTimeSelectUserSelAmPm').empty().append(h);
		jQuery('.ptTimeSelectAmPm').removeClass("ui-state-active");
		jQuery('#ptTimeSelectAmPm_' + h).addClass("ui-state-active");
	} else {
		jQuery('#ptTimeSelectUserSelHr').empty().append(h);
		jQuery('.ptTimeSelectHr').removeClass("ui-state-active");
		jQuery('#ptTimeSelectHr_' + h).addClass("ui-state-active");
	}
	jQuery.ptTimeSelect.setFull();
}  /* END setHr() function */
jQuery.ptTimeSelect.setFull = function() {
	_hr = $('#ptTimeSelectUserSelHr').text();
	_min = $('#ptTimeSelectUserSelMin').text();
	_AmPm = $('#ptTimeSelectUserSelAmPm').text().toLowerCase();
	if (_AmPm == "am" && _hr == "12") {
		_hr = "00";
	}
	if (_AmPm == "pm" && _hr != "12") {
	    var int_hr = parseInt(_hr, 10);
		_hr = "" + (int_hr + 12);
	}
	$('#ptTimeSelectUserSelFull').text("" + _hr + _min + "00");
}
jQuery.ptTimeSelect.setMin = function(m) {
	if ("" + m == "") {
		jQuery('.ptTimeSelectMin').removeClass("ui-state-active");
		return;
	}
	jQuery('#ptTimeSelectUserSelMin').empty().append(m);
	jQuery('.ptTimeSelectMin').removeClass("ui-state-active");
	jQuery('#ptTimeSelectMin_' + m).addClass("ui-state-active");
	jQuery.ptTimeSelect.setFull();
} /* END setMin() function */

jQuery.ptTimeSelect._doCheckMouseClick = function(ev) {
	if (!$("#ptTimeSelectCntr:visible").length) {
		return;
	}
	if (!jQuery(ev.target).closest("#ptTimeSelectCntr").length) {
		jQuery.ptTimeSelect.closeCntr();
	}

} /* jQuery.ptTimeSelect._doCheckMouseClick */


/***********************************************************************
* METHOD: $(ele).ptTimeSelect()
* 	Attaches a ptTimeSelect widget to each matched element. Matched
* 	elements must be input fields that accept a values (input field).
* 	Each element, when focused upon, will display a time selection 
* 	popoup where the user can define a time.
* 
* PARAMS:
* 
* 	@param {OBJECT}	opt -	(Optional) An object with the options for
* 							the time selection widget.
* 
* OPTIONS:
* 
* 	containerClass	-	String. A class to be assocated with the popup widget.
* 						(default: none)
* 	containerWidth	-	String. Css width for the container. (default: none)
* 	hoursLabel		-	String. Label for the Hours. (default: Hours)
* 	minutesLabel	-	String. Label for the Mintues. (default: Minutes)
* 	setButtonLabel	-	String. Label for the Set button. (default: SET)
* 	popupImage		-	String. The html element (ex. img or text) to be
* 						appended next to each input field and that will display
* 						the time select widget upon click.
* 	zindex			-	Int. Interger for the popup widget z-index.
* 	onBeforeShow	-	Function. Function to be called before the widget is
* 						made visible to the user. Function is passed 2 arguments:
* 						1) the input field as a jquery object and 2) the popup
* 						widget as a jquery object.
* 	onClose			-	Function. Function to be called after closing the
* 						popup widget. Function is passed 1 argument: the input
* 						field as a jquery object.
* 	onFocusDisplay	-	Boolean. True or False indicating if popup is auto
* 						displayed upon focus of the input field. (default:true)
* 
* 
* RETURNS:
* 
* 		- @return {object} jQuery
* 
* 
* EXAMPLE:
* 
* 	|		$('#fooTime').ptTimeSelect();
* 
 
 
jQuery.ptTimeSelect.options = {
timeCont: undefined,
timeView: undefined,
containerClass: undefined,
containerWidth: undefined,
hoursLabel: 'Hour',
minutesLabel: 'Minutes',
setButtonLabel: 'Set',
popupImage: undefined,
onFocusDisplay: true,
zIndex: 10,
onBeforeShow: undefined,
onClose: undefined
};

*/
timePicker = function(opt) {
    this.view = null;
    this.cont = null;
    this.toggler = null;
    this.Opt = {};
    var obj = this;

    obj.Opt = $.extend(obj.Opt, jQuery.ptTimeSelect.options, opt);
    if (obj.Opt.timeCont === undefined || $("#" + obj.Opt.timeCont).length == 0) return;
    this.setTime = function() {
        obj.cont.val(jQuery('#ptTimeSelectUserSelFull').text());
        obj.drawTime();
        obj.close();
    };
    this.drawTime = function() {
        obj.view.val(obj.get_hr() + ":" + obj.get_min() + " " + obj.get_AmPm());
        obj.view.html(obj.get_hr() + ":" + obj.get_min() + " " + obj.get_AmPm());
    };
    this.get_hr = function() {
        var intVal = parseInt(obj.cont.val().substring(0, 2), 10);
        if (intVal < 1)
            intVal += 12;
        else if (intVal >= 13)
            intVal -= 12;
        return (("" + intVal).length == 1) ? ("0" + intVal) : "" + intVal;
    };
    this.get_min = function() {
        return obj.cont.val().substring(2, 4);
    };
    this.get_AmPm = function() {
        var intVal = parseInt(obj.cont.val(), 10);
        return (intVal > 120000) ? "PM" : "AM";
    };
    this.close = function() {
        var e = $("#ptTimeSelectCntr");
        if (e.is(":visible") == true) {

            // If IE, then check to make sure it is realy visible
            if (jQuery.support.tbody == false) {
                if (!(e[0].offsetWidth > 0) && !(e[0].offsetHeight > 0)) {
                    return;
                }
            }

            jQuery('#ptTimeSelectCntr')
			.css("display", "none")
			.removeClass()
			.css("width", "");
            obj.cont.removeClass("isPtTimeSelectActive");
            if (obj.Opt.onClose) {
                obj.Opt.onClose();
            }
        }
        return;
    }; /* END setTime() function */

    this.show = function() {
        var cntr = jQuery("#ptTimeSelectCntr");
        cntr.css("display", "none");
        $(".isPtTimeSelectActive").removeClass("isPtTimeSelectActive");
        obj.cont.addClass("isPtTimeSelectActive");
        var style = obj.view.offset();
        style['z-index'] = obj.Opt.zIndex;
        style.top = (style.top + obj.view.outerHeight());
        if (obj.Opt.containerWidth) {
            style.width = obj.Opt.containerWidth;
        }
        if (obj.Opt.containerClass) {
            cntr.addClass(obj.Opt.containerClass);
        }
        cntr.css(style);
        var hr = 1;
        var min = '00';
        var tm = 'AM';
        if (obj.view.val()) {
            var re = /([0-9]{1,2}).*:.*([0-9]{2}).*(PM|AM)/i;
            var match = re.exec(obj.view.val());
            if (match) {
                hr = match[1] || 1;
                min = match[2] || '00';
                tm = match[3] || 'AM';
            }
        }


        $.ptTimeSelect.setHr(hr);
        $.ptTimeSelect.setMin(min);
        $.ptTimeSelect.setHr(tm);
        //cntr.find("#ptTimeSelectUserSelHr").empty().append(hr);
        //cntr.find("#ptTimeSelectUserSelMin").empty().append(min);
        //cntr.find("#ptTimeSelectUserSelAmPm").empty().append(tm);
        cntr.find(".ptTimeSelectTimeLabelsCntr .ptTimeSelectLeftPane")
			.empty().append(obj.Opt.hoursLabel);
        cntr.find(".ptTimeSelectTimeLabelsCntr .ptTimeSelectRightPane")
			.empty().append(obj.Opt.minutesLabel);
        cntr.find("#ptTimeSelectSetButton a").empty().append(obj.Opt.setButtonLabel);
        $('#ptTimeSelectCloseCntr a').unbind();
        cntr.find("#ptTimeSelectCloseCntr a").bind("click", obj.close);
        $('#ptTimeSelectSetButton a').unbind();
        cntr.find("#ptTimeSelectSetButton a").bind("click", obj.setTime);
        if (obj.Opt.onBeforeShow) {
            obj.Opt.onBeforeShow(obj.view, cntr);
        }
        cntr.slideDown("fast");

    }; /* END openCntr() function */

    obj.cont = $("#" + obj.Opt.timeCont);

    if (obj.Opt.timeView === undefined || $("#" + obj.Opt.timeView).length == 0) obj.view = obj.cont;
    else obj.view = $("#" + obj.Opt.timeView);

    if (obj.Opt.timeToggler != undefined || $("#" + obj.Opt.timeToggler).length != 0) obj.toggler = $("#" + obj.Opt.timeToggler);
    if (obj.toggler != null) obj.toggler.bind("click", obj.show);
    if (obj.view != null && obj.Opt.viewAsToggler) obj.view.click(this.show);
    //if (!obj.cont.hasClass('hasPtTimeSelect')) obj.cont.addClass('hasPtTimeSelect').data("ptTimeSelectOptions", obj.Opt);
    if (obj.cont.val() == "") obj.cont.val("000000");
    //	    if (obj.Opt.onFocusDisplay) {
    //	        obj.view.focus(function() {
    //	            //alert('ok');
    //	            obj.show();
    //	        });
    //	    }
    obj.drawTime();
};