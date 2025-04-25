function JSCal_single(view, cont, trigger, date_int_def, date_format, cleaner) {
	this.date_format = "%d %B %Y"; // date format
	this.date_str = null; // string date
	this.date_int = null; // int date
	this.date_int_def = null; // int date default if = ""
	this.date_int_last = null; // int date last selected
	this.view = null; // object to view single date
	this.cont = null; // object container to view single date
	this.trigger = null; // object trigger
	this.cleaner = null; // object cleaner
	this.view_day = null; // object to view multi date DAY
	this.view_month = null; // object to view multi date MONTH
	this.view_year = null; // object to view multi date YEAR
	this.cal = null; // object Calendar
	var obj = this;
	this.showCal = function() {
		obj.cal.popup(obj.trigger);
	}
	this.clear = function() {
		obj.date_int = null;
		date_int_last = null;
		obj.cont.value = "";
		obj.view.value = "";
	}
	this.onSelect = function(date) {
		obj.date_int = date;
		if (obj.date_int == obj.date_int_last) return;
		obj.cont.value = "" + date;
		obj.date_int_last = obj.date_int;
		obj.view.value = Calendar.printDate(Calendar.intToDate(obj.date_int), obj.date_format);
		obj.cal.hide();
	}
	if (date_format != null && date_format != "") obj.date_format = date_format;
	if (date_int_def != null) obj.date_int_def = date_int_def;
	if (view != null && document.getElementById(view)) obj.view = document.getElementById(view);
	if (cont != null && document.getElementById(cont)) obj.cont = document.getElementById(cont);
	if (obj.cont == null) return; 
	if (trigger == null) obj.trigger = null;
	else if (document.getElementById(trigger)) obj.trigger = document.getElementById(trigger);
	else obj.trigger = obj.view;
	if (cleaner != null && document.getElementById(cleaner)) { obj.cleaner = document.getElementById(cleaner); obj.cleaner.onclick = function() { obj.clear(); } }
	obj.cal = Calendar.setup({ animation: false });
	obj.cal.addEventListener('onSelect', function() { var date = this.selection.get(); obj.onSelect(date); })
	if (obj.trigger != null) obj.trigger.onclick = function() { obj.showCal(); }
	if (obj.cont.value != "") { obj.date_int = parseInt(obj.cont.value); obj.view.value = Calendar.printDate(Calendar.intToDate(obj.date_int), obj.date_format); }
}

function JSCal_object()
{
	this.min = 1; // minimum stay
	this.max = 0; // maximum stay
	this.start = 0; // JSCal int start
	this.start_cal = null; // JSCal object start
	this.start_view = null; // object start to client view
	this.start_cont = null; // object width start int value
	this.end = 0; // JSCal int end
	this.end_cal = null; // JSCal object end
	this.end_view = null; // object end to client view
	this.end_cont = null; // object width end int value
	this.count = 1; // difference in days (end-start)
	this.count_cont = null;
	this.setCalStrFnc = function() { };
	this.alertMinFnc = null;
	this.alertMaxFnc = null;
	var obj = this;
	this.setTxt_fromCal = function() {
		if (obj.start == 0 || obj.end == 0) return;
		var one_day = 1000 * 60 * 60 * 24;
		var date_start = Calendar.intToDate(obj.start);
		var date_start_t = date_start.getTime();
		var date_end = Calendar.intToDate(obj.end);
		var date_end_t = date_end.getTime();
		var _date_min = Calendar.intToDate(obj.start + obj.min);
		var _date_max = Calendar.intToDate(obj.start + obj.max);
		obj.end_cal.args.min = _date_min;
		obj.end_cal.redraw();
		obj.count = Math.ceil((date_end_t - date_start_t) / (one_day));
		if (obj.count_cont != null)
			obj.count_cont.value = obj.count;
	}
	this.setCal_start = function(date) {
		if (obj.start_cal == null || obj.end_cal == null) return;
		obj.start = date;
		var _date_start = Calendar.intToDate(obj.start);
		var _date_min = Calendar.intToDate(obj.start + obj.min);
		var _date_max = Calendar.intToDate(obj.start + obj.max);
		// set end date if not set or start > end
		if (obj.start == 0 || obj.end <= obj.start) {
			obj.end = Calendar.dateToInt(_date_min);
			obj.end_cal.selection.set(obj.end);
			obj.end_cal.moveTo(_date_min);
		}
		// set max date if is set
		var _date_end = Calendar.intToDate(obj.end);
		if (obj.max != 0) {
			obj.end_cal.args.max = _date_max;
			// change end date if is > then max
			if (_date_end > _date_max) {
				obj.end = Calendar.dateToInt(_date_max);
				obj.end_cal.selection.set(obj.end);
				obj.end_cal.moveTo(_date_max);
			}
		}
		obj.end_cal.args.min = _date_min;
		obj.end_cal.redraw();
		obj.start_cal.redraw();
		obj.setTxt_fromCal();
		obj.setCalStrFnc();
	}
	this.setCal_end = function(date) {
		obj.end = date;
		var _date_end = Calendar.intToDate(obj.end);
		obj.end_cal.redraw();
		obj.start_cal.redraw();
		obj.setTxt_fromCal();
		obj.setCalStrFnc();
	}
	this.setCal_fromTxt = function() {
		if (obj.start == 0 || obj.end == 0 || obj.count_cont == null) return;
		var _count = parseInt(obj.count_cont.value);
		if ("" + _count == "NaN") { _count = obj.min;}
		var one_day = 1000 * 60 * 60 * 24;
		var date_end = Calendar.intToDate(obj.start + _count);
		if (obj.max != 0 && obj.max < _count) {
			obj.count_cont.value = "" + obj.max;
			date_end = Calendar.intToDate(obj.start + obj.max);
			if (obj.alertMaxFnc != null) obj.alertMaxFnc();		
		}
		if (obj.min != 0 && obj.min > _count) {
			obj.count_cont.value = "" + obj.min;
			date_end = Calendar.intToDate(obj.start + obj.min);
			if (obj.alertMinFnc != null) obj.alertMinFnc();
		}
		obj.setCal_end(Calendar.dateToInt(date_end));
	}
}

