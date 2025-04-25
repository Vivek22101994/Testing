module(..., package.seeall)

function newJSCal()

	JSCal = {};
	local currLangs = {
		it = {
			monthNames = { "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre" },
			monthNamesShort = { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic" },
			dayNames = { "Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato" },
			dayNamesShort = { "Dom", "Lun", "Mar", "Mer", "Gio", "Ven", "Sab" },
			dayNamesMin = { "Do", "Lu", "Ma", "Me", "Gi", "Ve", "Sa" }
			},
		en = {
			monthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
			monthNamesShort = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
			dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
			dayNamesShort = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" },
			dayNamesMin = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" }
			},
		es = {
			monthNames = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" },
			monthNamesShort = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" },
			dayNames = { "Domingo", "Lunes", "Martes", "Mi&eacute;rcoles", "Jueves", "Viernes", "S&aacute;bado" },
			dayNamesShort = { "Dom", "Lun", "Mar", "Mi&eacute;", "Juv", "Vie", "S&aacute;b" },
			dayNamesMin = { "Do", "Lu", "Ma", "Mi", "Ju", "Vi", "S&aacute;" }
			},
		fr = {
			monthNames = { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" },
			monthNamesShort = { "Janv.", "Févr.", "Mars", "Avril", "Mai", "Juin", "Juil.", "Août", "Sept.", "Oct.", "Nov.", "Déc." },
			dayNames = { "Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" },
			dayNamesShort = { "Dim.", "Lun.", "Mar.", "Mer.", "Jeu.", "Ven.", "Sam." },
			dayNamesMin = { "D", "L", "M", "M", "J", "V", "S" }
			},
		de = {
			monthNames = { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" },
			monthNamesShort = { "Jan", "Feb", "Mär", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez" },
			dayNames = { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" },
			dayNamesShort = { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" },
			dayNamesMin = { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" }
			},
		ru = {
			monthNames = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" },
			monthNamesShort = { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" },
			dayNames = { "воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота" },
			dayNamesShort = { "вск", "пнд", "втр", "срд", "чтв", "птн", "сбт" },
			dayNamesMin = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" }
			},
		nl = {
			monthNames = { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" },
			monthNamesShort = { "jan", "feb", "maa", "apr", "mei", "jun", "jul", "aug", "sep", "okt", "nov", "dec" },
			dayNames = { "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag" },
			dayNamesShort = { "zon", "maa", "din", "woe", "don", "vri", "zat" },
			dayNamesMin = { "zo", "ma", "di", "wo", "do", "vr", "za" }
			},
		ja = {
			monthNames = { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" },
			monthNamesShort = { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" },
			dayNames = { "日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日" },
			dayNamesShort = { "日", "月", "火", "水", "木", "金", "土" },
			dayNamesMin = { "日", "月", "火", "水", "木", "金", "土" }
			},
		pt = {
			monthNames = { "Janeiro", "Fevereiro", "Mar&ccedil;o", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" },
			monthNamesShort = { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" },
			dayNames = { "Domingo", "Segunda-feira", "Ter&ccedil;a-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "S&aacute;bado" },
			dayNamesShort = { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "S&aacute;b" },
			dayNamesMin = { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "S&aacute;b" }
			},
		fi = {
			monthNames = { "Tammikuu", "Helmikuu", "Maaliskuu", "Huhtikuu", "Toukokuu", "Kes&auml;kuu", "Hein&auml;kuu", "Elokuu", "Syyskuu", "Lokakuu", "Marraskuu", "Joulukuu" },
			monthNamesShort = { "Tammi", "Helmi", "Maalis", "Huhti", "Touko", "Kes&auml;", "Hein&auml;", "Elo", "Syys", "Loka", "Marras", "Joulu" },
			dayNames = { "Su", "Ma", "Ti", "Ke", "To", "Pe", "Su" },
			dayNamesShort = { "Sunnuntai", "Maanantai", "Tiistai", "Keskiviikko", "Torstai", "Perjantai", "Lauantai" },
			dayNamesMin = { "Su", "Ma", "Ti", "Ke", "To", "Pe", "La" }
			}
		};
		    
	function JSCal:formatCustom(os_date, formatString, langCode, alternate)
			langVars = currLangs[langCode];
            if (langVars == nil) then langVars = currLangs["en"]; end
            if (langVars == nil) then return alternate; end
			
            _return = formatString;
            -- day of month
            _d = os_date.day.."";
			_dd = _d;
			if(string.len(_dd) == 1) then _dd="0".._dd; end
            -- day of week 
            --todo
            _D = langVars["dayNamesShort"][os_date.wday];
            _DD = langVars["dayNames"][os_date.wday];
            -- month of year
            _m = os_date.month.."";
			_mm = _m;
			if(string.len(_mm) == 1) then _mm="0".._mm; end
            _M = langVars["monthNamesShort"][os_date.month];
            _MM = langVars["monthNames"][os_date.month];

            _yy = os_date.year.."";
			_y = string.sub(_yy, 3, 4);
			
			_return = _return:gsub( "#yy#", _yy );
			_return = _return:gsub( "#y#", _y );
			_return = _return:gsub( "#mm#", _mm );
			_return = _return:gsub( "#m#", _m );
			_return = _return:gsub( "#dd#", _dd );
			_return = _return:gsub( "#d#", _d );
			_return = _return:gsub( "#DD#", _DD );
			_return = _return:gsub( "#D#", _D );
			_return = _return:gsub( "#MM#", _MM );
			_return = _return:gsub( "#M#", _M );
           return _return;
    end

	function JSCal:dateToString(os_date)
		local day = ""..os_date.day;
		if(string.len(day) == 1) then day="0"..day; end
		local month = ""..os_date.month;
		if(string.len(month) == 1) then month="0"..month; end
		return ""..os_date.year..month..day;
	end
	return JSCal;
end
