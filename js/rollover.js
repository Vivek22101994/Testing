function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}

}
function MM_swapContent(id_elm) { 
    var elm = document.getElementById("" + id_elm);
    elm.style.display = elm.style.display == "none" ? "block" : "none";
}
function MM_swapClass(id_elm, class1, class2) {
    var elm = document.getElementById("" + id_elm);
    elm.className = elm.className == class1 ? class2 : class1;
}
function showLabel(elmId, lblId) {
	var elm = document.getElementById(""+elmId);
	var lbl = document.getElementById(""+lblId);
	if(elm!=null && lbl!=null)
		lbl.style.display = elm.value == "" ? "block" : "none";
}
function hideLabel(elmId, lblId) {	
	var elm = document.getElementById(""+elmId);
	var lbl = document.getElementById(""+lblId);
	if(elm!=null && lbl!=null){
		lbl.style.display = "none";
		elm.focus();
	}
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


function CountWords(this_field, count_cont) {
	var char_count = this_field.value.length;
	var fullStr = this_field.value + " ";
	var initial_whitespace_rExp = /^[^A-Za-z0-9]+/gi;
	var left_trimmedStr = fullStr.replace(initial_whitespace_rExp, "");
	var non_alphanumerics_rExp = rExp = /[^A-Za-z0-9]+/gi;
	var cleanedStr = left_trimmedStr.replace(non_alphanumerics_rExp, " ");
	var splitString = cleanedStr.split(" ");
	var word_count = splitString.length - 1;
	if (fullStr.length < 2) {
		word_count = 0;
	}
	if (count_cont != null && count_cont != "") {
		$("#" + count_cont).css("display", "");
		$("#" + count_cont + ">.word").html("" + word_count);
		$("#" + count_cont + ">.char").html("" + char_count);
	}
}
