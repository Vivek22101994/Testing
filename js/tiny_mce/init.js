function LoadTiny() {
    tinyMCE.init({
        // General options
        mode: "exact",
        //elements: ""+id,
        //editor_selector: "mceAdvanced",
        //editor_deselector : "mceNoEditor",
        theme: "advanced",
        setup: function(ed) {
            ed.makeReadOnly = function(ro) {
                var t = this, s = t.settings, DOM = tinymce.DOM, d = t.getDoc();

                if (!s.readonly && ro) {
                    if (!tinymce.isIE) {
                        try {
                            d.designMode = 'Off';
                        } catch (ex) {

                        }
                    } else {
                        // It will not steal focus if we hide it while setting contentEditable
                        b = t.getBody();
                        DOM.hide(b);
                        b.contentEditable = false;
                        DOM.show(b);
                    }
                    s.readonly = true;
                } else if (s.readonly && !ro) {
                    if (!tinymce.isIE) {
                        try {
                            d.designMode = 'On';
                        } catch (ex) {

                        }
                    } else {
                        // It will not steal focus if we hide it while setting contentEditable
                        b = t.getBody();
                        DOM.hide(b);
                        b.contentEditable = true;
                        DOM.show(b);
                    }
                    s.readonly = false;
                }
                return s.readonly;
            };

            if (ed.settings.readonly) {
                ed.settings.readonly = false;
                ed.onInit.add(function(ed) {
                    toggleReadOnly(ed);
                });
            }
        }
,
        plugins: "pagebreak,style,layer,table,advhr,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist",
        convert_urls : false,
        forced_root_block: false,
        force_p_newlines: false,
        remove_linebreaks: false,
        force_br_newlines: true,
        remove_trailing_nbsp: false,
        verify_html: true,
        dialog_type: "modal",
        debug: false,
        remove_script_host: true,

        // Theme options
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,code",
        theme_advanced_buttons2: "", //"bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,preview,|,forecolor,backcolor",
        theme_advanced_buttons3: "", //"removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,advhr,|,ltr,rtl,|,fullscreen,|,styleprops,|,attribs,|,visualchars,nonbreaking,template,pagebreak",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: false,


        // Example content CSS (should be your site CSS)
        content_css: "css/editorStyle.css",

        // Drop lists for link/image/media/template dialogs
        template_external_list_url: "lists/template_list.js",
        external_link_list_url: "lists/link_list.js",
        external_image_list_url: "lists/image_list.js",
        media_external_list_url: "lists/media_list.js",

        extended_valid_elements: "iframe[src|width|height|name|align|style]",

        // Style formats
        style_formats: [
			            { title: 'keyWord Inline', inline: 'h1', classes: 'edit' }
		                    ]
    });
}
LoadTiny();
function removeTinyEditor_single(Editor, callback) {
    if (tinyMCE.getInstanceById(Editor) != null) {
        try { tinyMCE.execCommand('mceFocus', false, Editor); } catch (ex) { }
        try { tinyMCE.execCommand('mceRemoveControl', false, Editor); } catch (ex) { alert(ex); }
    }
    //$("#" + Editor).empty()
    if (callback != null) { callback(); }
}
function removeTinyEditors(Editors) {
    for (var i = 0; i < Editors.length; i++) {
        removeTinyEditor_single(Editors[i], function() { });
    }
    return;
}
function setTinyEditor_single(Editor) {
    if (tinyMCE.getInstanceById(Editor) == null || tinyMCE.getInstanceById(Editor) == "undefined") {
        tinyMCE.execCommand('mceAddControl', false, Editor);
    }
}
function setTinyEditors(Editors, IsReadOnly) {
    for (var i = 0; i < Editors.length; i++) {
        if (tinyMCE.getInstanceById(Editors[i]) == null || tinyMCE.getInstanceById(Editors[i]) == "undefined") {
            tinyMCE.execCommand('mceAddControl', true, Editors[i]);
            //setTinyEditorReadOnly(Editors[i], IsReadOnly);
        }
    }
}
function setTinyEditorReadOnly(ID, IsReadOnly) {
    var ed = tinyMCE.get(ID);
    if (ed != null) {
        //TinyMCEReadOnlySetup(ed, IsReadOnly)
        //ed.makeReadOnly(IsReadOnly); alert(IsReadOnly);
        //ed.switchReadOnly(IsReadOnly)
    }
    else {
        //setTimeout("setTinyEditorReadOnly('"+ID+"', " + IsReadOnly + ")", 100);
    }
}
