<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_search.ascx.cs" Inherits="RentalInRome.WLRental.uc.UC_search" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="cont_form">
            <div class="preinput preinputdue">
                <label id="lb_QuickSearch" style="position: absolute; display: block; width: 190px;"> <%=CurrentSource.getSysLangValue("lblQuickSearch")%></label>
                <asp:TextBox ID="txt_title" runat="server" CssClass="search_aptComplete floatnone" Width="195"></asp:TextBox>
                <asp:HiddenField ID="HF_estateId" runat="server" Value="0" />
                <asp:HiddenField ID="HF_estatePath" runat="server" Value="" />
            </div>
            <div class="preinput">
                <label>
                   <%-- <%=CurrentSource.getSysLangValue("reqCheckInDate")%>--%>
                   Check-In
                </label>
                <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                <a class="ico_cal" id="startCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
            </div>
            <div class="preinput">
                <label>
                   <%-- <%=CurrentSource.getSysLangValue("reqCheckOutDate")%>--%>
                   Check-Out
                </label> 
                <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" style="width: 90px" />
                <a class="ico_cal" id="endCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
            </div>
            <div class="preinput">
                <label>
                    <%=CurrentSource.getSysLangValue("lblNights")%>
                </label>
                <asp:TextBox ID="txt_dtCount" runat="server"></asp:TextBox>
            </div>
            <div class="preinput">
                <label>
                    <%=CurrentSource.getSysLangValue("lblPersons")%>
                </label>
                <asp:DropDownList ID="drp_num_persons_max" runat="server">
                </asp:DropDownList>
            </div>
            <div class="preinput" id="zoneCont_<%= Unique %>">
                <label>
                    <%=CurrentSource.getSysLangValue("lblZone")%>
                </label>
                <asp:DropDownList ID="drp_zone" runat="server">
                </asp:DropDownList>
            </div>
            <asp:LinkButton ID="lnk_search" runat="server" CssClass="search_but" onclick="lnk_search_Click">
                <span>
                    <%=CurrentSource.getSysLangValue("lblBeginSearch")%></span>
            </asp:LinkButton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    $(function() {
        $(window).load(function () {
            //$("#<%= lnk_search.ClientID %>").css("display", "block");
    });
});
    function submitForm_<%= Unique %>()
    {
        var _dtStart = $("#<%= HF_dtStart.ClientID %>").val();
        var _dtEnd = $("#<%= HF_dtEnd.ClientID %>").val();
        var _num_persons = $("#<%= drp_num_persons_max.ClientID %>").val();
        var _zone = $("#<%= drp_zone.ClientID %>").val();
        var _searchPage = '<%=CurrentSource.getPagePath("6", "stp", CurrentLang.ID.ToString()) %>';
        var _estatePage = $( "#<%= HF_estatePath.ClientID%>" ).val();
        var _currRedirect = _estatePage!="" ? _estatePage : _searchPage;
        var _titleSearch = _estatePage!="" ? "" : "&title="+$("#<%= txt_title.ClientID%>" ).val();
        window.location = _currRedirect+'?dtS='+_dtStart+'&dtE='+_dtEnd+'&numPers='+_num_persons+'&currZone='+_zone+''+_titleSearch;
    }
    var _JSCal_Range_<%= Unique %>;
    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.AddDays(1).JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", countCont: "#<%= txt_dtCount.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }
    <%= ltr_checkCalDates.Text %>
    RNT = {};
    RNT.EstateOptions = {
        id: 0,
        path: "",
        label: "",
        pid_zone: 0
    };
    var _estateList_<%= Unique %> = new Array();
	function getEstateXml_<%= Unique %>(){
        var _xml = $.ajax({
            type: "GET",
            url: "/webservice/rnt_estate_list_xml.aspx?lang=<%= CurrentLang.ID %>&SESSION_ID=<%= CURRENT_SESSION_ID %>",
			dataType: "xml",
			success: function(xml) {
			    $(xml).find('item').each(function() {
			        var _estOpt = {
			            id: parseInt($(this).find('id').text(), 10),
			            path: $(this).find('path').text(),
			            label: $(this).find('title').text(),
			            pid_zone: parseInt($(this).find('pid_zone').text(), 10)
			        };
			        _estateList_<%= Unique %>.push(_estOpt);
				});
			    setAutocomplete_<%= Unique %>();
			}
		});
    }
    function setAutocomplete_<%= Unique %>(){
        $( ".search_aptComplete" ).autocomplete({
            source: _estateList_<%= Unique %>,
		    search: function( event, ui ) {
		        $( "#<%= HF_estateId.ClientID%>" ).val( '0' );
			    $( "#<%= HF_estatePath.ClientID%>" ).val( '' );
			},
		    select: function( event, ui ) {
		        window.location = "/"+ui.item.path;
		        return;
		        $( "#<%= HF_estateId.ClientID%>" ).val( ui.item.id );
                $( "#<%= HF_estatePath.ClientID%>" ).val( ui.item.path );
			    if(ui.item.id!=0 && ui.item.path!='')
			        $( "#zoneCont_<%= Unique %>" ).hide();
				else
				    $( "#zoneCont_<%= Unique %>" ).show();
			}
		});
	    //alert(_estateList_<%= Unique %>);
	}
    getEstateXml_<%= Unique %>();
    SITE_addDinamicLabel('lb_QuickSearch', '<%= txt_title.ClientID %>');
</script>


<asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>