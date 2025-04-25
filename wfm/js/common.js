function GetXmlHttpObject() {
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        return new XMLHttpRequest();
    }
    if (window.ActiveXObject) {
        // code for IE6, IE5
        return new ActiveXObject("Microsoft.XMLHTTP");
    }
    return null;
}
function showLabel(elmId, lblId) {
	var elm = document.getElementById("" + elmId);
	var lbl = document.getElementById("" + lblId);
	if (elm != null && lbl != null)
		lbl.style.display = elm.value == "" ? "block" : "none";
}
function hideLabel(elmId, lblId) {
	var elm = document.getElementById("" + elmId);
	var lbl = document.getElementById("" + lblId);
	if (elm != null && lbl != null) {
		lbl.style.display = "none";
		elm.focus();
	}
}
function SetClassName(elm, cl) {
	elm.className = "" + cl;
}
function clickButton(e, buttonid) {
	var evt = e ? e : window.event;
	var bt = document.getElementById(buttonid);
	if (bt) {
		if (evt.keyCode == 13) {
			bt.click();
			return false;
		}
	}
}
function buttonPostBack(buttonid) {
	var bt = document.getElementById(buttonid);
	if (bt) {
		bt.click();
		return false;
	}
}