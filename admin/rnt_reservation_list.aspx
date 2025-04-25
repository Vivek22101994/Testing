<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_reservation_list.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_list" %>

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
            _value = $("#<%= drp_is_deleted.ClientID%>").lenght != 0 ? $("#<%= drp_is_deleted.ClientID%>").val() : "0";
            if (_value != "")
                _filter += "&is_del=" + _value;
            _value = $("#<%= drp_state_pid.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&state=" + _value;
		    _value = $("#<%= txt_code.ClientID%>").val();
		    if (_value != "")
		        _filter += "&code=" + _value;
		    _value = $("#<%= drp_flt_pid_creator.ClientID%>").val();
		    if (_value != "")
		        _filter += "&creator=" + _value;
		    _value = $("#<%= lbx_flt_zone.ClientID%>").val();
		    if (_value != "")
		        _filter += "&zone=" + _value;
		    _value = $("#<%= drp_is_srs.ClientID%>").val();
		    if (_value != "-1")
		        _filter += "&srs=" + _value;
		    _value = $("#<%= drp_flt_city.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&city=" + _value;
		    _value = $("#<%= drp_flt_isActiveEstate.ClientID%>").val();
		    if (_value != "1")
		        _filter += "&isestact=" + _value;
		    _value = $("#<%= lbx_flt_estate.ClientID%>").val();
		    if (_value != "0")
		        _filter += "&est=" + _value;
		    _value = $("#<%= drp_account.ClientID%>").val();
		    if (_value != "")
		        _filter += "&account=" + _value;
		    _value = $("#<%= txt_name_full.ClientID%>").val();
		    if (_value != "")
		        _filter += "&name_full=" + _value;
		    _value = $("#<%= txt_email.ClientID%>").val();
		    if (_value != "")
		        _filter += "&email=" + _value;
		    _value = $("#<%= drp_pidAgent.ClientID%>").val();
		    if (_value != "")
		        _filter += "&agent=" + _value;


		    _value = $("#<%= HF_dtStart_from.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtsf=" + _value;
		    _value = $("#<%= HF_dtStart_to.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtst=" + _value;

		    _value = $("#<%= HF_dtEnd_from.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtef=" + _value;
		    _value = $("#<%= HF_dtEnd_to.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtet=" + _value;

		    _value = $("#<%= HF_dtCreation_from.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtcf=" + _value;
		    _value = $("#<%= HF_dtCreation_to.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtct=" + _value;

		    _value = $("#<%= HF_state_date_from.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtstf=" + _value;
		    _value = $("#<%= HF_state_date_to.ClientID%>").val();
		    if (_value != "0" && $.trim(_value) != "")
		        _filter += "&dtstt=" + _value;

		    _value = getCheckBoxListValues("<%= chkList_flt_problemID.ClientID%>", "|");
		    if (_value != "")
		        _filter += "&problem=" + _value;

		    _value = $("#<%= drp_iCal.ClientID%>").val();
		    if (_value != "")
		        _filter += "&ical=" + _value;
		    
		    window.location = "rnt_reservation_list.aspx?flt=true" + _filter;


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
        #column_visualizer {
            clear: both;
            margin-bottom: 8px;
            margin-top: 15px;
            display: block;
        }

            #column_visualizer a.chk {
                width: 20px;
                height: 20px;
                text-indent: 200px;
                cursor: pointer;
                color: #fff;
                display: inline-block;
                margin: 0;
                text-decoration: none;
            }

            #column_visualizer a.opened {
                background-image: url('../images/ico/chk_ok.png');
            }

            #column_visualizer a.closed {
                background-image: url('../images/ico/chk_no.png');
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

            <asp:HiddenField ID="HF_state_date_daysbefore" runat="server" Value="7" />

            <div id="fascia1">
                <div class="pannello_fascia1">
                    <div style="clear: both">
                        <h1>Elenco prenotazioni e disponibilità</h1>
                        <div class="bottom_agg">
                            <a href="#" onclick="return RNT_openNewReservation();"><span>+ Nuovo</span></a>
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
                                                        <td style="width: 230px;">
                                                            <label>
                                                                Stato Pren.:</label>
                                                            <asp:DropDownList runat="server" ID="drp_state_pid" Width="150px" CssClass="inp">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Codice Pren.:</label>
                                                            <asp:TextBox ID="txt_code" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Nome Cognome del Cliente:</label>
                                                            <asp:TextBox ID="txt_name_full" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Email del Cliente:</label>
                                                            <asp:TextBox ID="txt_email" runat="server" CssClass="inp" Width="150px"></asp:TextBox>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Blocchi iCal:</label>
                                                            <asp:DropDownList runat="server" ID="drp_iCal" Width="70px" CssClass="inp">
                                                                <asp:ListItem Value="0">NO</asp:ListItem>
                                                                <asp:ListItem Value="1">SI</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                SRS</label>
                                                            <asp:DropDownList runat="server" ID="drp_is_srs" Width="60px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                <ContentTemplate>
                                                                    <table class="inp">
                                                                        <tr>
                                                                            <td style="width: 180px;">
                                                                                <asp:DropDownList runat="server" ID="drp_flt_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_flt_city_SelectedIndexChanged" CssClass="inp" Style="margin-bottom: 10px;" />
                                                                            </td>
                                                                            <td style="width: 250px;">
                                                                                <asp:DropDownList runat="server" ID="drp_flt_isActiveEstate" AutoPostBack="True" OnSelectedIndexChanged="drp_flt_isActiveEstate_SelectedIndexChanged" CssClass="inp" Style="margin-bottom: 10px;">
                                                                                    <asp:ListItem Value="1" Text="Appartamenti Attivi"></asp:ListItem>
                                                                                    <asp:ListItem Value="0" Text="Appartamenti Non attivi"></asp:ListItem>
                                                                                    <asp:ListItem Value="-1" Text="Attivi e Non attivi"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 180px;">
                                                                                <label>
                                                                                    Zona:</label>
                                                                                <asp:ListBox ID="lbx_flt_zone" runat="server" SelectionMode="Multiple" Width="150px" Rows="11" CssClass="inp" AutoPostBack="true" OnSelectedIndexChanged="lbx_flt_zone_SelectedIndexChanged"></asp:ListBox>
                                                                            </td>
                                                                            <td style="width: 250px;">
                                                                                <label>
                                                                                    Struttura:</label>
                                                                                <asp:ListBox ID="lbx_flt_estate" runat="server" SelectionMode="Multiple" Width="220px" Rows="11" CssClass="inp"></asp:ListBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data check-in:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtStart_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtStart_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtStart_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtStart_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtStart_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtStart_to" runat="server" />
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Data check-out:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEnd_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEnd_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtEnd_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtEnd_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtEnd_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtEnd_to" runat="server" />
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <label>
                                                                Data Creazione:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtCreation_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtCreation_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_dtCreation_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_dtCreation_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_dtCreation_from" runat="server" />
                                                            <asp:HiddenField ID="HF_dtCreation_to" runat="server" />
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Data Modifica stato:</label>
                                                            <table class="inp">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            da:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_state_date_from" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_state_date_from" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            a:</label>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txt_state_date_to" style="width: 120px" />
                                                                        <img src="../images/ico/ico_del.gif" id="del_state_date_to" style="cursor: pointer;" alt="X" title="Pulisci" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="HF_state_date_from" runat="server" />
                                                            <asp:HiddenField ID="HF_state_date_to" runat="server" />
                                                        </td>
                                                        <td>
                                                            <label>
                                                                con Problemi
                                                                <br />
                                                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="inlinebtn" OnClick="lnk_chkListSelectAll_Click" CommandArgument="problemID select">tutti</asp:LinkButton>
                                                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="inlinebtn" OnClick="lnk_chkListSelectAll_Click" CommandArgument="problemID deselect">nessuna</asp:LinkButton>
                                                            </label>
                                                            <asp:CheckBoxList ID="chkList_flt_problemID" runat="server" Visible="false">
                                                            </asp:CheckBoxList>
                                                            <asp:Literal ID="chkList_flt_problemID_clientMode" runat="server"></asp:Literal>
                                                            <div class="nulla">
                                                            </div>
                                                            <label>
                                                                Agenzia</label>
                                                            <asp:DropDownList runat="server" ID="drp_pidAgent" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="Td1" runat="server" visible="false">
                                                            <label>
                                                                Account</label>
                                                            <asp:DropDownList runat="server" ID="drp_account" CssClass="inp" Width="120px">
                                                            </asp:DropDownList>
                                                            <div class="nulla" style="height: 10px;">
                                                            </div>
                                                            <label>
                                                                Da</label>
                                                            <asp:DropDownList runat="server" ID="drp_flt_pid_creator" Width="100px" CssClass="inp">
                                                                <asp:ListItem Text="-tutti-" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="solo dal Sito" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="solo da info" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- Table seconda riga -->
                                            </td>
                                            <td valign="bottom">
                                                <a href="javascript: Filter()" class="ricercaris">
                                                    <span>Filtra Risultati</span></a>
                                            </td>
                                        </tr>
                                        <tr>
                                          <td colspan="2">
                                            <a href="javascript: Filter()" class="ricercaris" style="margin-top: 10px;">
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
                                <asp:HiddenField ID="HF_LDS_orderBy" runat="server" Value="state_date desc" Visible="false" />
                                <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TBL_RESERVATION">
                                </asp:LinqDataSource>
                                <asp:ListView ID="LV" runat="server" DataSourceID="LDS" Visible="false" OnDataBound="LV_DataBound" OnItemDataBound="LV_ItemDataBound" OnItemCommand="LV_ItemCommand">
                                    <ItemTemplate>
                                        <tr class="<%# Eval("agentID").objToInt64() != 0 ? "resAgenzia" : ""%>" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" <%# ("" + Eval("IdAdMedia") == "Ha" || "" + Eval("IdAdMedia") == "HA") ? " bgcolor='#bde4f7'" : ""%>>
                                            <td>
                                                <%# "" + Eval("notesInner") != "" ? "<a class=\"ico_list_info ico_tooltip_right\" title=\"div_note_" + Eval("id") + "\">?</a>" : ""%>
                                                <div id='tooltip_div_note_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <%# (""+Eval("notesInner")).htmlNoWrap()%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code")%></span>
                                            </td>
                                            <td>
                                                <span <%# Eval("state_pid") + "" == "4" ? " class=\"res_prt\" style=\"width: auto; height: auto; color: rgb(255, 255, 255); padding-left: 10px;\"" : ""%> <%# Eval("state_pid") + "" == "3" ? " class=\"res_can\" style=\"width: auto; height: auto; padding-left: 10px;\"" : ""%>>
                                                    <%# rntUtils.rntReservation_getStateName(Eval("state_pid").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <%# (Eval("state_pid") + "" == "3" && Eval("requestRenewal").objToInt32() > 0) ? "<span style=\"font-size: 16px; font-weight: bold; color: red;\">!</span>" : ""%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("state_date")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("state_pid_user").objToInt32(),"- - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("dtCreation")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "- - - - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime?)Eval("dtStart")).formatITA(false) + " Ore " + ("" + Eval("dtStartTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime?)Eval("dtEnd")).formatITA(false) + " Ore " + ("" + Eval("dtEndTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("num_adult").objToInt32() + Eval("num_child_over").objToInt32()) + "&nbsp;(" + Eval("num_adult").objToInt32() + "+" + Eval("num_child_over").objToInt32() + ")&nbsp;+&nbsp;" + Eval("num_child_min").objToInt32()%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;"+Eval("pr_part_payment_total").objToDecimal().ToString("N2")+""%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("agentCommissionPrice").objToDecimal().ToString("N2") + ""%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_part_owner").objToDecimal().ToString("N2") + ""%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2") + ""%>
                                                </span>
                                            </td>
                                            <td payed="<%# Eval("payed_total")%>">
                                                <%# Eval("payed_total").objToDecimal() == 0 ? "<span style=\"color:red;\">NO</span>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() < Eval("pr_part_payment_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() >= Eval("pr_part_payment_total").objToDecimal() && Eval("payed_total").objToDecimal() < Eval("pr_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"color:green; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() >= Eval("pr_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"background-color: #00d000; border: 1px solid #04a504; border-radius: 4px; color: #f00 !important; cursor: pointer; display: inline-block; font-size: 11px; font-weight: bold; margin: 5px; padding: 1px 3px !important;\">SI</a>" : ""%>
                                                <div id='tooltip_div_pay_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Pagato:
                                                                </td>
                                                                <td>
                                                                    <%# "&euro;&nbsp;" + Eval("payed_total").objToDecimal().ToString("N2") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tramite:
                                                                </td>
                                                                <td>
                                                                    <%# invUtils.invPayment_modeTitle(Eval("payed_mode")+"","- - -")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Data:
                                                                </td>
                                                                <td>
                                                                    <%# ((DateTime?)Eval("payed_date"))!=null?((DateTime)Eval("payed_date")).formatITA(false) + " Ore " + ((DateTime)Eval("payed_date")).TimeOfDay.JSTime_toString(false, true):"--/--/--"%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Utente:
                                                                </td>
                                                                <td>
                                                                    <%# AdminUtilities.usr_adminName(Eval("payed_user").objToInt32(),"- - -")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <%# Eval("prRefundTotal").objToDecimal() != 0 ? "<a class=\"ico_tooltip_right\" title=\"div_refund_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : "<span style=\"color:green;\">NO</span>"%>
                                                <div id='tooltip_div_refund_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Rimborsato:
                                                                </td>
                                                                <td>
                                                                    <%# "&euro;&nbsp;" + Eval("prRefundTotal").objToDecimal().ToString("N2") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tramite:
                                                                </td>
                                                                <td>
                                                                    <%# invUtils.invPayment_modeTitle(Eval("prRefundPayMode")+"","- - -")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Data:
                                                                </td>
                                                                <td>
                                                                    <%# ((DateTime?)Eval("prRefundDate"))!=null?((DateTime)Eval("prRefundDate")).formatITA(false) + " Ore " + ((DateTime)Eval("prRefundDate")).TimeOfDay.JSTime_toString(false, true):"--/--/--"%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <%# Eval("problemID").objToInt32() != 0 ? "<a class=\"ico_tooltip_right\" title=\"div_problemID_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : "<span style=\"color:green;\">NO</span>"%>
                                                <div id='tooltip_div_problemID_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Problema:
                                                                </td>
                                                                <td>
                                                                    <%# rntUtils.getProblem_title(Eval("problemID").objToInt32(), "-non definito-")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border: 1px dotted rgb(0, 0, 0);">
                                                                    <%# Eval("problemDesc")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# rntUtils.getAgent_nameCompany(Eval("agentId").objToInt64(), "")%></span>
                                            </td>
                                            <asp:PlaceHolder ID="PH_view" runat="server">
                                                <td>
                                                    <span>
                                                        <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(), "-! non assegnato !-")%>
                                                        <asp:ImageButton ID="ibtn_edit" CommandName="edit_operator" AlternateText="edit" runat="server" ImageUrl="~/images/ico/ico_modif.gif" ToolTip="Cambia Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px; margin-left: 5px;" />
                                                    </span>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none"> </asp:LinkButton>
                                                </td>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="PH_edit" runat="server">
                                                <td>
                                                    <asp:DropDownList runat="server" ID="drp_admin" Style="margin-top: 2px; margin-right: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="ibtn_save" AlternateText="save" runat="server" CommandName="save_operator" ImageUrl="~/images/ico/ico_save.gif" ToolTip="Salva Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                    <asp:ImageButton ID="ibtn_cancel" AlternateText="cancel" runat="server" CommandName="cancel_operator" ImageUrl="~/images/ico/ico_annulla.gif" ToolTip="Annulla" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                </td>
                                            </asp:PlaceHolder>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "System")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a <%# (Eval("id").objToInt64()>150000)?" target=\"_blank\" href=\"rnt_reservation_details.aspx?id="+Eval("id")+"\"":"href=\"#\" onclick=\"return RNT_openReservationWart('"+Eval("id")+"')\"" %> style="margin-top: 6px; margin-right: 5px;">det.</a>
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
                                        <tr class="alternate <%# Eval("agentID").objToInt64() != 0 ? "resAgenzia" : ""%>" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');" <%# ("" + Eval("IdAdMedia") == "Ha" || "" + Eval("IdAdMedia") == "HA") ? " bgcolor='#bde4f7'" : ""%>>
                                            <td>
                                                <%# "" + Eval("notesInner") != "" ? "<a class=\"ico_list_info ico_tooltip_right\" title=\"div_note_" + Eval("id") + "\">?</a>" : ""%>
                                                <div id='tooltip_div_note_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <%# (""+Eval("notesInner")).htmlNoWrap()%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("code")%></span>
                                            </td>
                                            <td>
                                                <span <%# Eval("state_pid") + "" == "4" ? " class=\"res_prt\" style=\"width: auto; height: auto; color: rgb(255, 255, 255); padding-left: 10px;\"" : ""%> <%# Eval("state_pid") + "" == "3" ? " class=\"res_can\" style=\"width: auto; height: auto; padding-left: 10px;\"" : ""%>>
                                                    <%# rntUtils.rntReservation_getStateName(Eval("state_pid").objToInt32())%></span>
                                            </td>
                                            <td>
                                                <%# (Eval("state_pid") + "" == "3" && Eval("requestRenewal").objToInt32() > 0) ? "<span style=\"font-size: 16px; font-weight: bold; color: red;\">!</span>" : ""%>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("state_date")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("state_pid_user").objToInt32(),"- - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("dtCreation")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "- - - - -")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime?)Eval("dtStart")).formatITA(false) + " Ore " + ("" + Eval("dtStartTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# ((DateTime?)Eval("dtEnd")).formatITA(false) + " Ore " + ("" + Eval("dtEndTime")).JSTime_stringToTime().JSTime_toString(false, true)%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# (Eval("num_adult").objToInt32() + Eval("num_child_over").objToInt32()) + "&nbsp;(" + Eval("num_adult").objToInt32() + "+" + Eval("num_child_over").objToInt32() + ")&nbsp;+&nbsp;" + Eval("num_child_min").objToInt32()%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;"+Eval("pr_part_payment_total").objToDecimal().ToString("N2")+""%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("agentCommissionPrice").objToDecimal().ToString("N2") + ""%>
                                                </span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_part_owner").objToDecimal().ToString("N2") + ""%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# "&euro;&nbsp;" + Eval("pr_total").objToDecimal().ToString("N2") + ""%>
                                                </span>
                                            </td>
                                            <td payed="<%# Eval("payed_total")%>">
                                                <%# Eval("payed_total").objToDecimal() == 0 ? "<span style=\"color:red;\">NO</span>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() < Eval("pr_part_payment_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() >= Eval("pr_part_payment_total").objToDecimal() && Eval("payed_total").objToDecimal() < Eval("pr_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"color:green; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : ""%>
                                                <%# Eval("payed_total").objToDecimal() != 0 && Eval("payed_total").objToDecimal() >= Eval("pr_total").objToDecimal() ? "<a class=\"ico_tooltip_right\" title=\"div_pay_" + Eval("id") + "\" style=\"background-color: #00d000; border: 1px solid #04a504; border-radius: 4px; color: #f00 !important; cursor: pointer; display: inline-block; font-size: 11px; font-weight: bold; margin: 5px; padding: 1px 3px !important;\">SI</a>" : ""%>
                                                <div id='tooltip_div_pay_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Pagato:
                                                                </td>
                                                                <td>
                                                                    <%# "&euro;&nbsp;" + Eval("payed_total").objToDecimal().ToString("N2") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tramite:
                                                                </td>
                                                                <td>
                                                                    <%# invUtils.invPayment_modeTitle(Eval("payed_mode")+"","- - -")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Data:
                                                                </td>
                                                                <td>
                                                                    <%# ((DateTime?)Eval("payed_date"))!=null?((DateTime)Eval("payed_date")).formatITA(false) + " Ore " + ((DateTime)Eval("payed_date")).TimeOfDay.JSTime_toString(false, true):"--/--/--"%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Utente:
                                                                </td>
                                                                <td>
                                                                    <%# AdminUtilities.usr_adminName(Eval("payed_user").objToInt32(),"- - -")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <%# Eval("prRefundTotal").objToDecimal() != 0 ? "<a class=\"ico_tooltip_right\" title=\"div_refund_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : "<span style=\"color:green;\">NO</span>"%>
                                                <div id='tooltip_div_refund_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Rimborsato:
                                                                </td>
                                                                <td>
                                                                    <%# "&euro;&nbsp;" + Eval("prRefundTotal").objToDecimal().ToString("N2") %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tramite:
                                                                </td>
                                                                <td>
                                                                    <%# invUtils.invPayment_modeTitle(Eval("prRefundPayMode")+"","- - -")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Data:
                                                                </td>
                                                                <td>
                                                                    <%# ((DateTime?)Eval("prRefundDate"))!=null?((DateTime)Eval("prRefundDate")).formatITA(false) + " Ore " + ((DateTime)Eval("prRefundDate")).TimeOfDay.JSTime_toString(false, true):"--/--/--"%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <%# Eval("problemID").objToInt32() != 0 ? "<a class=\"ico_tooltip_right\" title=\"div_problemID_" + Eval("id") + "\" style=\"color:red; display: block; font-size: 11px; margin: 5px; cursor: pointer;\">SI</a>" : "<span style=\"color:green;\">NO</span>"%>
                                                <div id='tooltip_div_problemID_<%# Eval("id") %>' style="display: none;">
                                                    <div class="box_dett_day">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80px;"></td>
                                                                <td style="width: 220px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Problema:
                                                                </td>
                                                                <td>
                                                                    <%# rntUtils.getProblem_title(Eval("problemID").objToInt32(), "-non definito-")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border: 1px dotted rgb(0, 0, 0);">
                                                                    <%# Eval("problemDesc")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_email") %></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("cl_name_full")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# rntUtils.getAgent_nameCompany(Eval("agentId").objToInt64(), "")%></span>
                                            </td>
                                            <asp:PlaceHolder ID="PH_view" runat="server">
                                                <td>
                                                    <span>
                                                        <%# AdminUtilities.usr_adminName(Eval("pid_operator").objToInt32(), "-! non assegnato !-")%>
                                                        <asp:ImageButton ID="ibtn_edit" CommandName="edit_operator" AlternateText="edit" runat="server" ImageUrl="~/images/ico/ico_modif.gif" ToolTip="Cambia Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px; margin-left: 5px;" />
                                                    </span>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none"> </asp:LinkButton>
                                                </td>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="PH_edit" runat="server">
                                                <td>
                                                    <asp:DropDownList runat="server" ID="drp_admin" Style="margin-top: 2px; margin-right: 5px; font-size: 11px;">
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="ibtn_save" AlternateText="save" runat="server" CommandName="save_operator" ImageUrl="~/images/ico/ico_save.gif" ToolTip="Salva Account" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                    <asp:ImageButton ID="ibtn_cancel" AlternateText="cancel" runat="server" CommandName="cancel_operator" ImageUrl="~/images/ico/ico_annulla.gif" ToolTip="Annulla" Style="text-decoration: none; border: 0 none; margin-top: 2px; margin-right: 5px;" />
                                                </td>
                                            </asp:PlaceHolder>
                                            <td>
                                                <span>
                                                    <%# AdminUtilities.usr_adminName(Eval("pid_creator").objToInt32(), "System")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_pid_related_request" Visible="false" runat="server" Text='<%# Eval("pid_related_request") %>' />
                                                <asp:Label ID="lbl_pid_operator" Visible="false" runat="server" Text='<%# Eval("pid_operator") %>' />
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                                <a <%# (Eval("id").objToInt64()>150000)?" target=\"_blank\" href=\"rnt_reservation_details.aspx?id="+Eval("id")+"\"":"href=\"#\" onclick=\"return RNT_openReservationWart('"+Eval("id")+"')\"" %> style="margin-top: 6px; margin-right: 5px;">det.</a>
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
                                                <td>No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager2" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                                <tr>
                                                    <th style="text-align: center;"></th>
                                                    <th style="text-align: center;">Codice
                                                    </th>
                                                    <th>Stato
                                                    </th>
                                                    <th></th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_state_date" runat="server" CommandName="orderBy" CommandArgument="state_date">Modifica stato</asp:LinkButton>
                                                    </th>
                                                    <th>Utente
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_dtCreation" runat="server" CommandName="orderBy" CommandArgument="dtCreation">Data Creazione</asp:LinkButton>
                                                    </th>
                                                    <th>Struttura
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_dtStart" runat="server" CommandName="orderBy" CommandArgument="dtStart">Data Check-In</asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_dtEnd" runat="server" CommandName="orderBy" CommandArgument="dtEnd">Data Check-Out</asp:LinkButton>
                                                    </th>
                                                    <th>Persone
                                                    </th>
                                                    <th>Acconto
                                                    </th>
                                                    <th>
                                                        <img src="/images/css/icothsoldi.png" class="ico_tooltip" alt="" ttp="Scalata comm. Agenzia" />
                                                    </th>
                                                    <th>+Saldo
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_pr_total" runat="server" CommandName="orderBy" CommandArgument="pr_total">=Totale</asp:LinkButton>
                                                    </th>
                                                    <th style="text-align: center;">Pagato?
                                                    </th>
                                                    <th style="text-align: center;">Refund?
                                                    </th>
                                                    <th style="text-align: center;">Prob? </th>
                                                    <th>E-mail
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_orderBy_cl_name_full" runat="server" CommandName="orderBy" CommandArgument="cl_name_full">Nome Cognome</asp:LinkButton>
                                                    </th>
                                                    <th style="width: 200px;">Agenzia
                                                    </th>
                                                    <th style="width: 200px;">Account
                                                    </th>
                                                    <th>Creato da
                                                    </th>
                                                    <th></th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server" />
                                                <tr id="Tr1" runat="server" visible="false">
                                                    <th></th>
                                                    <th>
                                                        <asp:LinkButton ID="lnk_updateAllOperators" OnClick="lnk_updateAllOperators_Click" runat="server">Aggiorna gli Account</asp:LinkButton>
                                                    </th>
                                                    <th></th>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="page">
                                            <asp:DataPager ID="DataPager1" runat="server" PageSize="50" style="border-right: medium none;" QueryStringField="pg">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="20" />
                                                </Fields>
                                            </asp:DataPager>
                                            <asp:Label ID="lbl_record_count" runat="server" CssClass="total" Text=""></asp:Label>
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
        var cal_dtStart_from;
        var cal_dtStart_to;

        var cal_dtEnd_from;
        var cal_dtEnd_to;

        var cal_dtCreation_from;
        var cal_dtCreation_to;

        var cal_state_date_from;
        var cal_state_date_to;
        function setCal() {
            cal_dtStart_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtStart_from.ClientID %>", View: "#txt_dtStart_from", Cleaner: "#del_dtStart_from", changeMonth: true, changeYear: true });
            cal_dtStart_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtStart_to.ClientID %>", View: "#txt_dtStart_to", Cleaner: "#del_dtStart_to", changeMonth: true, changeYear: true });

            cal_dtEnd_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEnd_from.ClientID %>", View: "#txt_dtEnd_from", Cleaner: "#del_dtEnd_from", changeMonth: true, changeYear: true });
            cal_dtEnd_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtEnd_to.ClientID %>", View: "#txt_dtEnd_to", Cleaner: "#del_dtEnd_to", changeMonth: true, changeYear: true });

            cal_dtCreation_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_from.ClientID %>", View: "#txt_dtCreation_from", Cleaner: "#del_dtCreation_from", changeMonth: true, changeYear: true });
            cal_dtCreation_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_dtCreation_to.ClientID %>", View: "#txt_dtCreation_to", Cleaner: "#del_dtCreation_to", changeMonth: true, changeYear: true });

            cal_state_date_from = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_state_date_from.ClientID %>", View: "#txt_state_date_from", Cleaner: "#del_state_date_from", changeMonth: true, changeYear: true });
            cal_state_date_to = new JSCal.Single({ dtFormat: "d MM yy", Cont: "#<%= HF_state_date_to.ClientID %>", View: "#txt_state_date_to", Cleaner: "#del_state_date_to", changeMonth: true, changeYear: true });
        }
    </script>

</asp:Content>
