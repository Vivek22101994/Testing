<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_estate.ascx.cs" Inherits="RentalInRome.admin.uc.UC_rnt_reservation_estate" %>
<asp:HiddenField ID="HF_isEdit" runat="server" />
<asp:HiddenField ID="HF_id" runat="server" />
<asp:HiddenField ID="HF_IdReservation" runat="server" />
<asp:HiddenField ID="HF_pid_city" runat="server" Value="0" />
<asp:HiddenField ID="HF_pid_zone" runat="server" Value="1" />
<asp:HiddenField ID="HF_code" runat="server" />
<asp:HiddenField ID="HF_num_persons_child" runat="server" Value="3" />
<asp:HiddenField ID="HF_num_persons_min" runat="server" />
<asp:HiddenField ID="HF_num_persons_max" runat="server" />
<asp:HiddenField ID="HF_num_rooms_bed" runat="server" />
<asp:HiddenField ID="HF_importance_stars" runat="server" Value="1" />
<asp:HiddenField ID="HF_is_exclusive" runat="server" />
<asp:HiddenField ID="HF_is_ecopulizie" runat="server" />
<asp:HiddenField ID="HF_is_srs" runat="server" />
<asp:HiddenField ID="HF_is_loft" runat="server" />
<asp:HiddenField ID="HF_pr_percentage" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_deposit" runat="server" Value="0" />
<asp:HiddenField ID="HF_pr_depositWithCard" runat="server" Value="0" />
<asp:HiddenField ID="HF_nights_min" runat="server" />
<asp:HiddenField ID="HF_sel_dtStart" runat="server" />
<asp:HiddenField ID="HF_sel_dtEnd" runat="server" />
<asp:HiddenField ID="HF_sel_num_persons" runat="server" />
<asp:HiddenField ID="HF_sel_pr_total" runat="server" Value="0" />
<input type="hidden" id="rtn_price_state" />

<script type="text/javascript">
    function rtn_toggle_price() {
        if ($("#rtn_price_state").val() == "1") {
            //$("#rtn_price_toggler").removeClass("opened");
            //$("#rtn_price_toggler").addClass("closed");
            $("#rtn_price_toggler").html("Visualizza Prezzi");
            $("#rtn_price_cont").slideUp();
            $("#rtn_price_state").val("0");
        }
        else {
            //$("#rtn_price_toggler").removeClass("closed");
            //$("#rtn_price_toggler").addClass("opened");
            $("#rtn_price_toggler").html("Nascondi Prezzi");
            $("#rtn_price_cont").slideDown();
            $("#rtn_price_state").val("1");
        }
    }
    $("#rtn_price_state").val("0");
	</script>

<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;"></div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
            <a target="_blank" href="<%= CurrentSource.getPagePath(HF_id.Value,"pg_estate","1") %>"><%= HF_code.Value%></a>
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" onclick="lnk_edit_Click">Cambia Appartamento</asp:LinkButton>
        <div class="datiapt" style="float: left; width: auto;">
            <table class="guestInfo risric" cellpadding="0" cellspacing="0" style="float: left; width: auto;">
                <tr>
                    <td valign="middle" align="left" style="width: 60px;">
                       Zona:
                    </td>
                    <td style="width: 120px;">
                        <%= CurrentSource.locZone_title(HF_pid_zone.Value.ToInt32(),1,"") %>
                    </td>
                    <td style="width: 30px;">
                    </td>
                    <td valign="middle" align="left" style="width: 60px;">
                        Ecopulizie
                    </td>
                    <td>
                        <%= HF_is_ecopulizie.Value == "1" ? "SI" : "NO"%>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                       Capienza:
                    </td>
                    <td>
                        <%= HF_num_persons_max.Value %>&nbsp;Persone
                    </td>
                    <td>
                    </td>
                    <td valign="middle" align="left">
                        SRS
                    </td>
                    <td>
                        <%= HF_is_srs.Value == "1" ? "SI" : "NO"%>
                    </td>
                </tr>
            </table>
            <br/>
            <a id="rtn_price_toggler" href="javascript:rtn_toggle_price()">Visualizza Prezzi</a>
        </div>
        <div class="price_div" id="rtn_price_cont" style="display:none;">
            <asp:PlaceHolder ID="PH_priceOnRequestCont" runat="server" Visible="false">
                <%= CurrentSource.getSysLangValue("lblOnRequest")%>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_priceListCont" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="priceTable" style="float: left;">
                    <tr>
                        <th style="border-left: none; background: none;">
                            &nbsp;
                        </th>
                        <th colspan="2">
                            <strong>
                                <%= CurrentSource.rntPeriod_title(1, CurrentLang.ID, "Low Season")%></strong><br />
                            <!--11/05/2011 to 22/06/2011 - 23/06/20011 to ...-->
                        </th>
                        <th colspan="2">
                            <strong>
                                <%= CurrentSource.rntPeriod_title(2, CurrentLang.ID, "High Season")%></strong><br />
                            <!--11/05/2011 to 22/06/2011 - 23/06/20011 to ...-->
                        </th>
                        <th colspan="2">
                            <strong>
                                <%= CurrentSource.rntPeriod_title(3, CurrentLang.ID, "Very High Season")%></strong><br />
                            <!--11/05/2011 to 22/06/2011 - 23/06/20011 to ...-->
                        </th>
                    </tr>
                    <tr>
                        <td class="prima" style="border-left: none;">
                            &nbsp;
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblDaily")%>:
                        </td>
                        <td class="prima">
                            <%= CurrentSource.getSysLangValue("lblWeekly")%>:
                        </td>
                    </tr>
                    <%= ltr_priceDetails.Text%>
                </table>
            </asp:PlaceHolder>
            <asp:Literal ID="ltr_priceDetails" runat="server" Visible="false"></asp:Literal>
            <asp:Literal ID="ltr_priceTemplate" runat="server" Visible="false">
				<tr>
					<td style="border-left: none;">
						#num_pers#
					</td>
					<td>
						#pr_1#
					</td>
					<td>
						#pr_1_w#
					</td>
					<td>
						#pr_2#
					</td>
					<td>
						#pr_2_w#
					</td>
					<td>
						#pr_3#
					</td>
					<td>
						#pr_3_w#
					</td>
				</tr>
            </asp:Literal>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>Seleziona Appartamento</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" onclick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <div class="divric">
                <!-- <h4>Filtra Utenti</h4>-->
                <div style="float: left;">
                    <table>
                        <tr>
                            <td>
                                <div class="camporic">
                                    <label>
                                        Nome Struttura
                                    </label>
                                    <div>
                                        <asp:TextBox runat="server" ID="txt_flt_code" Width="250px" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <div class="camporic">
                                    <label>
                                        Zona
                                    </label>
                                    <div>
                                        <asp:ListBox ID="lbx_flt_zone" runat="server" SelectionMode="Multiple" Rows="10" Style="min-width: 255px;">
                                        </asp:ListBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="nulla">
                    </div>
                </div>
                <div style="float: left;">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    Min. #.Posti Letto
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_min_num_persons_max" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td style=" width:20px;">
                            </td>
                            <td>
                                <label>
                                    Min. #.Camere
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_min_num_rooms_bed" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Min. #.Stelle
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_min_importance_stars" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <label>
                                    Min. # Letti M.
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_min_num_bed_double" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Min. # Bagni
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_min_num_rooms_bath" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <label>
                                    In Esclusiva
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_is_exclusive" runat="server">
                                        <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="camporic">
                                    <label>
                                        Ecopulizie
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_is_ecopulizie" runat="server">
                                            <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <div class="camporic">
                                    <label>
                                        Srs
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_is_srs" runat="server">
                                            <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Attico
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_is_loft" runat="server">
                                        <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <div class="camporic">
                                    <label>
                                        Aria Condizionata
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_has_air_condition" runat="server">
                                            <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Internet
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_has_internet" runat="server">
                                        <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <label>
                                    Ascensore
                                </label>
                                <div>
                                    <asp:DropDownList ID="drp_has_lift" runat="server">
                                        <asp:ListItem Text="--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="nulla">
                    </div>
                </div>
                <div class="btnric">
                    <asp:LinkButton ID="lnk_filter" runat="server" OnClick="lnk_filter_Click" OnClientClick="return checkFilter();">Filtra</asp:LinkButton>
                    <span id="lbl_fltError" style="float: right; font-size: 13px; width: 125px; display: none;"></span>
                </div>
                <div class="nulla">
                </div>
            </div>
           
            <div class="risric">
                <asp:ListView ID="LV_flt" runat="server" OnItemCommand="LV_flt_ItemCommand" onitemdatabound="LV_flt_ItemDataBound">
                    <ItemTemplate>
                        <tr onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td style="overflow: hidden; word-wrap: normal;">
                                <%# Eval("code") %>
                            </td>
                            <td style="overflow: hidden; word-wrap: normal;" width="150">
                                <%# CurrentSource.locZone_title(Eval("pid_zone").objToInt32(),1,"-non abbinata-")%>
                            </td>
                            <td width="60">
                                <%# ""+Eval("is_ecopulizie")=="1"?"SI":"NO" %>
                            </td>
                            <td width="60">
                                <%# ""+Eval("is_srs")=="1"?"SI":"NO" %>
                            </td>
                            <td width="120">
                                <asp:Literal ID="ltr_price" runat="server"></asp:Literal>
                            </td>
                            <td width="80">
                                <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id")%>'></asp:Label>
                                <asp:LinkButton ID="lnk_select" CssClass="sel" CommandName="seleziona" runat="server">Seleziona</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        Non ci sono appartamenti con i parametri di ricerca...
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table cellpadding="2" cellspacing="0" border="0" style="table-layout: fixed;">
                            <thead>
                                <tr>
                                    <th>
                                        Nome Struttura
                                    </th>
                                    <th width="150">
                                        Zona
                                    </th>
                                    <th width="60">
                                        Eco
                                    </th>
                                    <th width="60">
                                        SRS
                                    </th>
                                    <th width="120">
                                        Prezzo nel periodo
                                    </th>
                                    <th width="80">
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div style="width: 100%; overflow: auto; height: 250px;">
                            <table cellpadding="2" cellspacing="0" border="0" style="table-layout: fixed;">
                                <tbody>
                                    <tr id="itemPlaceholder" runat="server" />
                                </tbody>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
                <div class="nulla">
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>

<script type="text/javascript">
    function checkFilter() {
        var _zoneList = $("#<%= lbx_flt_zone.ClientID%>").val() || []; // zone selezionate al momento
        if (_zoneList.length == 0) {
            $("#lbl_fltError").show();
            $("#lbl_fltError").html("// Selezionare almeno una zona.");
            return false;
        }
        $("#lbl_fltError").hide();
        $("#lbl_fltError").html("");
        return true;
    }
</script>

