<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_reservation_event.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_event" %>

<%@ Register Src="uc/UC_loader_list.ascx" TagName="UC_loader_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function RNT_openNewReservation() {
            var _url = "rnt_reservation_form.aspx?IdRes=0";
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
        }
        function RNT_openReservationWart(IdRes) {
            var _url = "rnt_reservation_formWART.aspx?IdRes=" + IdRes;
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
            return false;
        }
        function refreshDates() {
            Filter();
        }
        function showFilter() {
			$get("lnk_showfilt").style.display = "none";
			$get("lnk_hidefilt").style.display = "";
			$get("tbl_filter").style.display = "";
			$get("<%=HF_is_filtered.ClientID %>").value = "true";
		}
		function hideFilter() {
			$get("lnk_showfilt").style.display = "";
			$get("lnk_hidefilt").style.display = "none";
			$get("tbl_filter").style.display = "none";
			$get("<%=HF_is_filtered.ClientID %>").value = "false";
		}
		function Filter() {

		    var _filter = "";
		    var _value = "";
		    _value = $get("<%= drp_is_deleted.ClientID%>") != null ? $get("<%= drp_is_deleted.ClientID%>").value : "0";
		    if (_value != "")
		        _filter += "&is_del=" + _value;
		    _value = $("#<%= drp_estate.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&pidest=" + _value;
		    _value = $("#<%= txt_name_full.ClientID%>").val();
		    if (_value != "")
		        _filter += "&name_full=" + _value;
		    _value = $("#<%= txt_code.ClientID%>").val();
		    if (_value != "")
		        _filter += "&code=" + _value;
		    _value = $("#<%= txt_email.ClientID%>").val();
		    if (_value != "")
		        _filter += "&email=" + _value;
		    _value = $("#<%= drp_event.ClientID%>").val();
		    if (_value != "")
		        _filter += "&event=" + _value;

		    
		    _value = $("#<%= HF_dtEvent_from.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtsf=" + _value;
		    _value = $("#<%= HF_dtEvent_to.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtst=" + _value;

		    _value = $("#<%= drp_homeAway.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_HomeAway=" + _value;
		    _value = $("#<%= drp_is_exclusive.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_exclusive=" + _value;
		    _value = $("#<%= drp_is_srs.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_srs=" + _value;
		    _value = $("#<%= drp_is_ecopulizie.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_ecopulizie=" + _value;
		    _value = $("#<%= drp_is_online_booking.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&is_online_booking=" + _value;
		    _value = $("#<%= drp_pr_has_overnight_tax.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&overnight_tax=" + _value;
		    _value = $("#<%= drp_isContratto.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&isContratto=" + _value;


		    window.location = "rnt_reservation_event.aspx?flt=true" + _filter;


		    return;
		}
		function swapColumn(col) {
			if ($("#state_" + col).val() == "0") {
				$("#toggler_" + col).addClass("opened");
				$("#toggler_" + col).removeClass("closed");
				$("#state_" + col).val("1");
				$(".cont_" + col).css("display", "");
			}
			else {
				$("#toggler_" + col).removeClass("opened");
				$("#toggler_" + col).addClass("closed");
				$("#state_" + col).val("0");
				$(".cont_" + col).css("display", "none");
			}
		}
    </script>

    <style type="text/css">
		#column_visualizer{
			clear: both;
			margin-bottom: 8px;
			margin-top: 15px;
			display: block;
		}
		#column_visualizer a.chk{
			width:20px;
			height:20px;
			text-indent:200px;
			cursor:pointer;
			color: #fff;
			display: inline-block;
			margin: 0;
			text-decoration: none;
		}
		#column_visualizer a.opened{
			 background-image:url('../images/ico/chk_ok.png');
		}
		#column_visualizer a.closed{
			 background-image:url('../images/ico/chk_no.png');
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="Literal1" runat="server" Visible="false">
		Riepilogo colonne:<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
		1-ID<br/>
    </asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_is_filtered" runat="server" />
            <asp:HiddenField ID="HF_url_filter" runat="server" />
            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Planner CheckIn/CheckOut</h1>
                        <div class="bottom_agg">
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="filt">
                            <div class="t">
                                <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                            </div>
                            <div class="c">
                                <a class="filto_bottone" style="display: none;" id="lnk_showfilt" onclick="showFilter();">FILTRA</a>
                                <a class="filto_bottone2" id="lnk_hidefilt" onclick="hideFilter();">FILTRA</a>
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <asp:PlaceHolder ID="PH_admin" runat="server">
                                                            <td>
                                                                <label>
                                                                    Eliminati:</label>
                                                                <asp:DropDownList runat="server" ID="drp_is_deleted" Width="70px" CssClass="inp">
                                                                    <asp:ListItem Value="">-tutti-</asp:ListItem>
                                                                    <asp:ListItem Value="1">SI</asp:ListItem>
                                                                    <asp:ListItem Value="0">NO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </asp:PlaceHolder>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Evento</label>
                                                                        <asp:DropDownList runat="server" ID="drp_event" Width="100px" CssClass="inp">
                                                                            <asp:ListItem Text="-tutto-" Value=""></asp:ListItem>
                                                                            <asp:ListItem Text="CHECK-IN" Value="in"></asp:ListItem>
                                                                            <asp:ListItem Text="CHECK-OUT" Value="out"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Codice:</label>
                                                                        <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="85px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <label>
                                                                            Struttura</label>
                                                                        <asp:DropDownList runat="server" ID="drp_estate" Width="200px" CssClass="inp" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Nome Cognome:</label>
                                                            <asp:TextBox ID="txt_name_full" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Email:</label>
                                                            <asp:TextBox ID="txt_email" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Data:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEvent_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEvent_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEvent_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEvent_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtEvent_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtEvent_to" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="nulla"></div>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                Esclusiva</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_exclusive" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                SRS</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_srs" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Ecopulizie</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_ecopulizie" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Online B.</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_online_booking" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Tassa di s.</label>
                                                            <asp:DropDownList runat="server" ID="drp_pr_has_overnight_tax" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                Contratto</label>
                                                            <asp:DropDownList runat="server" ID="drp_isContratto" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label>
                                                                HA Attivo</label>
                                                            <asp:DropDownList runat="server" ID="drp_homeAway" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter()" class="ricercaris">
                                                    <span>Filtra Risultati</span></a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                                <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                            </div>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_lds_filter" runat="server" Visible="false" />
                                <asp:HiddenField ID="HF_id_editAdmin" runat="server" Visible="false" />
                                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_RESERVATION_EVENT" OrderBy="dtEvent, direction desc, dtEventTime">
                                </asp:LinqDataSource>
                                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" Visible="false" OnDataBound="LV_DataBound" onitemdatabound="LV_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                            <td>
                                                <%# ("" + Eval("is_dtTimeChanged") == "1")?"<span style='color:Green;'>":"<span style='color:Red;'>"%>
                                                    CHECK-<%# (""+Eval("direction")).ToUpper()%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("dtEvent")).formatCustom("#dd#/#mm#/#yy#",1,"")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:DropDownList runat="server" ID="drp_dtEventTime_h" Style="width: 55px; margin-top: 2px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    :<asp:DropDownList runat="server" ID="drp_dtEventTime_m" Style="width: 55px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("est_code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_name_honorific") + "&nbsp;" + Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(),"- - -")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_dtEventTime" Visible="false" runat="server" Text='<%# Eval("dtEventTime") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_direction" Visible="false" runat="server" Text='<%# Eval("direction") %>' />
                                                <a <%# (Eval("id").objToInt64()>150000)?" target=\"_blank\" href=\"rnt_reservation_details.aspx?id="+Eval("id")+"\"":"href=\"#\" onclick=\"return RNT_openReservationWart('"+Eval("id")+"')\"" %> style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                            <td>
                                                <a onclick="OpenEasyShuttleReservation(<%# Eval("id") %>); return false;" href="#" class="ico_logo_easy ico_tooltip_right" ttp="Transfer EasyShuttle" style="margin-right: 5px;"></a>
                                            </td>
                                            <td>
                                                <a onclick="OpenSrsReservation(<%# Eval("id") %>); return false;" href="#" class="ico_logo_srs ico_tooltip_right" ttp="Accoglienza Srs" style="margin-right: 5px;"></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                            <td>
                                                <%# ("" + Eval("is_dtTimeChanged") == "1")?"<span style='color:Green;'>":"<span style='color:Red;'>"%>
                                                    CHECK-<%# (""+Eval("direction")).ToUpper()%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code") %>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime)Eval("dtEvent")).formatCustom("#dd#/#mm#/#yy#",1,"")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:DropDownList runat="server" ID="drp_dtEventTime_h" Style="width: 55px; margin-top: 2px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    :<asp:DropDownList runat="server" ID="drp_dtEventTime_m" Style="width: 55px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("est_code") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_name_honorific") + "&nbsp;" + Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(),"- - -")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_dtEventTime" Visible="false" runat="server" Text='<%# Eval("dtEventTime") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <asp:Label ID="lbl_direction" Visible="false" runat="server" Text='<%# Eval("direction") %>' />
                                                <a <%# (Eval("id").objToInt64()>150000)?" target=\"_blank\" href=\"rnt_reservation_details.aspx?id="+Eval("id")+"\"":"href=\"#\" onclick=\"return RNT_openReservationWart('"+Eval("id")+"')\"" %> style="margin-top: 6px; margin-right: 5px;">dettagli</a>
                                            </td>
                                            <td>
                                                <a onclick="OpenEasyShuttleReservation(<%# Eval("id") %>); return false;" href="#" class="ico_logo_easy ico_tooltip_right" ttp="Transfer EasyShuttle" style="margin-right: 5px;"></a>
                                            </td>
                                            <td>
                                                <a onclick="OpenSrsReservation(<%# Eval("id") %>); return false;" href="#" class="ico_logo_srs ico_tooltip_right" ttp="Accoglienza Srs" style="margin-right: 5px;"></a>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>
                                                    No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th>
                                                        Evento
                                                    </th>
                                                    <th style="text-align: center;">
                                                        Codice
                                                    </th>
                                                    <th>
                                                        Data
                                                    </th>
                                                    <th>
                                                        Ore - 
                                                        <asp:LinkButton ID="LinkButton1" OnClick="lnk_updateAllTime_Click" runat="server">Aggiorna gli Orari</asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        Struttura
                                                    </th>
                                                    <th>
                                                        E-mail
                                                    </th>
                                                    <th>
                                                        Nome Cognome
                                                    </th>
                                                    <th>
                                                        Account
                                                    </th>
                                                    <th>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                                <tr>
                                                    <th colspan="3">
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_updateAllTime" OnClick="lnk_updateAllTime_Click" runat="server">Aggiorna gli Orari</asp:LinkButton>
                                                    </th>
                                                    <th colspan="4">
                                                    </th>
                                                </tr>
                                            </table>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" />
                                <uc2:UC_loader_list ID="UC_loader_list1" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_page_update" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
		var contentUpdater = "<%= btn_page_update.ClientID %>";
		function ReloadContent() {
			__doPostBack(contentUpdater, "load_content");
		}
		ReloadContent();
		var cal_dtEvent_from;
		var cal_dtEvent_to;
		function setCal() {
		    cal_dtEvent_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEvent_from.ClientID %>", View: "#txt_dtEvent_from", Cleaner: "#del_dtEvent_from", changeMonth: true, changeYear: true });
		    cal_dtEvent_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEvent_to.ClientID %>", View: "#txt_dtEvent_to", Cleaner: "#del_dtEvent_to", changeMonth: true, changeYear: true });
		}
    </script>

</asp:Content>
