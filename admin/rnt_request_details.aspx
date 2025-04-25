<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_request_details.aspx.cs" Inherits="RentalInRome.admin.rnt_request_details" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register src="uc/UC_rnt_rl_request_state.ascx" tagname="UC_rnt_rl_request_state" tagprefix="uc2" %>
<%@ Register src="uc/UC_rnt_request_operator.ascx" tagname="UC_rnt_request_operator" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function refreshDates() {
            window.location = "rnt_request_details.aspx?id=<%=HF_id.Value %>";
        }
        function RNT_openSelection(IdEstate) {
            var _url = "rnt_reservation_form.aspx?IdEstate=" + IdEstate + "&IdRes=0&IdRequest=<%= HF_id.Value %>";
            rwdUrl_open(_url)
            //OpenShadowbox(_url, 800, 0);
        }
        function RNT_openOwnerOpz(IdEstate) {
            var _url = "rnt_reservation_formOwnerOpz.aspx?IdEstate=" + IdEstate;
            //alert(_url);
            OpenShadowbox(_url, 800, 0);
        }
    </script>

    <script type="text/javascript">
        var rwdUrl = null;
        function rwdUrl_open(url) {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");

            rwdUrl.set_autoSize(false);
            rwdUrl.set_visibleTitlebar(true);
            //rwdUrl.set_minWidth(700);
            rwdUrl.setUrl(url);
            rwdUrl.show();
            rwdUrl.maximize();
            return false;
        }
        function rwdUrl_close() {
            if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
            rwdUrl.close();
            return false;
        }
        function rwdUrl_OnClientClose(sender, eventArgs) {
        }
    </script>

</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false" OnClientClose="rwdUrl_OnClientClose" MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false">
    </telerik:RadWindow>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
            <asp:HiddenField ID="HF_unique" runat="server" />
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_IdReservation" Value="0" runat="server" />
            <asp:HiddenField ID="HF_state_pid" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_cl_pid_lang" Value="1" runat="server" />
            <asp:HiddenField ID="HF_num_adult" runat="server" Value="2" />
            <asp:HiddenField ID="HF_num_child_over" runat="server" Value="0" />
            <asp:HiddenField ID="HF_num_child_min" runat="server" Value="0" />
            <asp:HiddenField ID="HF_numPersTotal" runat="server" Value="0" />
            <h1 class="titolo_main">Scheda Richiesta</h1>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_modify" Visible="false" runat="server" OnClick="lnk_modify_Click" OnClientClick="removeTinyEditor();"><span>Modifica</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" Visible="false" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click" OnClientClick="removeTinyEditor();"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" Visible="false" runat="server" OnClick="lnk_annulla_Click" OnClientClick="removeTinyEditor();"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>">
                        <span>Torna nel elenco</span></a>
                </div>
                <div class="bottom_salva">
                    <a href="rnt_request_details_old.aspx?id=<%= HF_id.Value %>">
                        <span>Vecchia Scheda</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
								<tr>
									<td class="td_title">
										Data/Ora richiesta:
									</td>
									<td>
										<asp:Literal ID="ltr_date_request" runat="server"></asp:Literal>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Dati Cliente / Prenotazione</span>
						<div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                            <asp:HiddenField ID="HF_dtStart" runat="server" />
                            <asp:HiddenField ID="HF_dtEnd" runat="server" /> 
                            <asp:Literal ID="ltr_date_is_flexible" runat="server" Visible="false"></asp:Literal>
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_datesView" runat="server">
                                <tr>
                                    <td class="td_title">
                                        Nome Cognome:
                                    </td>
                                    <td>
                                        <%= txt_name_full.Text%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        E-mail:
                                    </td>
                                    <td>
                                        <%= txt_email.Text%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Telefono:
                                    </td>
                                    <td>
                                        <%= txt_phone.Text%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nazione/Location:
                                    </td>
                                    <td>
                                        <%= drp_country.SelectedValue%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Lingua:
                                    </td>
                                    <td>
                                        <%= drp_lang.SelectedItem.Text%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">City:
                                    </td>
                                    <td>
                                        <%= drp_pidCity.getSelectedText("")%>
                                    </td>
                                </tr>
								<tr>
									<td class="td_title">
										Check-in:
									</td>
									<td>
                                        <%= HF_dtStart.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", 1, "") %>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Check-out:
									</td>
									<td>
                                        <%= HF_dtEnd.Value.JSCal_stringToDate().formatCustom("#dd# #MM# #yy#", 1, "") %>
                                    </td>
								</tr>
								<tr>
									<td class="td_title">
										Date Flessibili?:
									</td>
									<td>
                                        <%= ltr_date_is_flexible.Text %>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										num. Adulti:
									</td>
									<td>
                                        <% = drp_adult.SelectedValue %>
                                    </td>
								</tr>
								<tr>
									<td class="td_title">
										Bambini (over 3):
									</td>
									<td>
                                        <% = drp_child_over.SelectedValue%>
									</td>
								</tr>
                                <tr>
                                    <td class="td_title">
                                        Bambini (under 3):
                                    </td>
                                    <td>
                                        <% = drp_child_min.SelectedValue %>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="prices">
                                        <div class="fasciatitBar" style="height: 20px; overflow: hidden; position: relative;">
                                            <asp:LinkButton ID="lnk_datesEdit" runat="server" CssClass="changeapt" OnClick="lnk_datesEdit_Click">Cambia dati</asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
							</table>
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_datesEdit" runat="server" visible="false">
                                <tr>
                                    <td class="td_title">
                                        Nome Cognome:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_name_full" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        E-mail:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_email" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Telefono:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_phone" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nazione/Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                            <WhereParameters>
                                                <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                            </WhereParameters>
                                        </asp:LinqDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Lingua:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_lang" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
                                        </asp:LinqDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">City:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_pidCity">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Check-in:
                                    </td>
                                    <td>
                                        <input id="txt_dtStart_<%= Unique %>" type="text" readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Check-out:
                                    </td>
                                    <td>
                                        <input id="txt_dtEnd_<%= Unique %>" type="text" readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Date Flessibili?:
                                    </td>
                                    <td>
                                        <%= ltr_date_is_flexible.Text %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        num. Adulti:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_adult" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Bambini (over 3):
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_child_over" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Bambini (under 3):
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_child_min" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="prices">
                                        <div class="fasciatitBar" style="height: 20px; overflow: hidden; position: relative;">
                                            <asp:LinkButton ID="lnk_datesSave" runat="server" CssClass="changeapt" OnClick="lnk_datesSave_Click">Salva dati</asp:LinkButton>
                                            <asp:LinkButton ID="lnk_datesCancel" runat="server" CssClass="changeapt" OnClick="lnk_datesCancel_Click">Annulla</asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Opzioni</span>
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Trasporto:
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltr_transport" runat="server"></asp:Literal>
                                    </td>
                                </tr>
								<tr>
									<td class="td_title">
										Price Range:
									</td>
									<td>
										<asp:Literal ID="ltr_price_range" runat="server"></asp:Literal>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Servizi:
									</td>
									<td>
										<asp:Literal ID="ltr_services" runat="server"></asp:Literal>
									</td>
								</tr>
                                <tr>
									<td class="td_title" style="width:150px;">
										Richiesta Speciale oggetto:
									</td>
									<td>
										<asp:Literal ID="ltr_subject" runat="server"></asp:Literal>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Richiesta speciale:
									</td>
									<td>
										<asp:Literal ID="ltr_notes" runat="server"></asp:Literal>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="bottom">
						<div style="float: left;">
							<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
					</div>
				</div>
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Preferenze</span>
						<div class="boxmodulo">
                            
                                    <script type="text/javascript">
                                        function RNT_fillList(action, add, remove) {
                                            if (action == "list") SITE_showLoader();
                                            var _url = "/admin/webservice/rnt_requestEstateList.aspx?id=<%=IdRequest %>&add=" + add + "&remove=" + remove;
                                            var _xml = $.ajax({
                                                type: "GET",
                                                url: _url,
                                                dataType: "html",
                                                success: function (html) {
                                                    $("#estateListCont").html(html);
                                                    if (action == "list") {
                                                        SITE_hideLoader(); 
                                                        $("#<%=pnl_editAptList.ClientID %>").hide();
                                                    }
                                                }
                                            });
                                        }
                                        $(window).load(function () {
                                            RNT_fillList("first", "", "");
                                        });
                                    </script>
                                    <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" width="100%">
                                        <tr>
                                            <td id="estateListCont">
                                                <div class="loadingSrc">
                                                    <span>Loading...<br />
                                                        <strong>
                                                            Stiamo caricando l'elenco degli appartamenti preferiti dal cliente.
                                                        <br />
                                                            Vi ringraziamo per la pazienza.
                                                        </strong> 
                                                    </span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="prices">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="fasciatitBar" Style="height: 20px; overflow: hidden; position: relative;">
                                                            <asp:LinkButton ID="lnk_editAptListShow" runat="server" CssClass="changeapt" OnClick="lnk_editAptListShow_Click">Seleziona Altre Strutture</asp:LinkButton>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnl_editAptList" runat="server" Visible="false" CssClass="fasciatitBar" Style="overflow: hidden; position: relative;">
                                                    <h3>
                                                        Seleziona Appartamento</h3>
                                                    <asp:LinkButton ID="lnk_editAptListCancel" runat="server" CssClass="changeapt" OnClick="lnk_editAptListCancel_Click">Annulla</asp:LinkButton>
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
                                                                                    <asp:TextBox runat="server" ID="txt_flt_code" Width="250px" CssClass="aptComplete" />
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div class="camporic">
                                                                                <label>
                                                                                    Città
                                                                                </label>
                                                                                <div>
                                                                                    <asp:DropDownList runat="server" ID="drp_city" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="drp_city_SelectedIndexChanged" />
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
                                                                                <label style="float: right;">
                                                                                    seleziona tutto 
                                                                                    
                                                                                    <input type="checkbox" id="chk_zone_all" onchange="selectAllZones()" />
                                                                                </label>
                                                                                <div>
                                                                                    <asp:ListBox ID="lbx_flt_zone" runat="server" SelectionMode="Multiple" Rows="10" Style="min-width: 255px;"></asp:ListBox>
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
                                                                        <td style="width: 20px;">
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
                                                                            
                                                                        </td>
                                                                    </tr><tr>
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
                                                                        <td style="width: 20px;">
                                                                        </td>
                                                                        <td>
                                                                            <label>
                                                                                Instant Booking
                                                                            </label>
                                                                            <div>
                                                                                <asp:DropDownList ID="drp_is_online_booking" runat="server">
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
                                                                        </td>
                                                                         <td style="width: 20px;">
                                                                        </td>
                                                                         <td>
                                                                            <label>
                                                                               Accessori
                                                                            </label>
                                                                            <div>
                                                                               <asp:checkboxlist id="chk_accessory" runat="server"></asp:checkboxlist>
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
                                                            <asp:ListView ID="LV_flt" runat="server">
                                                                <ItemTemplate>
                                                                    <tr onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                                                        <td style="overflow: hidden; word-wrap: normal;" width="180">
                                                                            <%# Eval("code") %>
                                                                        </td>
                                                                        <td style="overflow: hidden; word-wrap: normal;" width="120">
                                                                            <%# Eval("zone") %>
                                                                        </td>
                                                                        <td width="30">
                                                                            <%# ""+Eval("is_ecopulizie")=="1"?"SI":"NO" %>
                                                                        </td>
                                                                        <td width="30">
                                                                            <%# ""+Eval("is_srs")=="1"?"SI":"NO" %>
                                                                        </td>
                                                                        <td width="30">
                                                                            <%# ""+Eval("is_exclusive")=="1"?"SI":"NO" %>
                                                                        </td>
                                                                        <td width="30">
                                                                            <%# "" + Eval("is_online_booking") == "1" ? "SI" : "NO"%>
                                                                        </td>
                                                                        <td width="60">
                                                                            <%# Eval("pr_percentage").objToInt32() + "&nbsp;% "%>
                                                                        </td>
                                                                        <td width="110" style="text-align: right;">
                                                                            <%# Eval("price").objToDecimal() == 0 ? "Su richiesta" : Eval("price").objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
                                                                        </td>
                                                                        <td width="50">
                                                                            <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id")%>'></asp:Label>
                                                                            <asp:Label ID="lbl_pr_percentage" runat="server" Visible="false" Text='<%# Eval("pr_percentage")%>'></asp:Label>
                                                                            <input type="checkbox" value="<%# Eval("id")%>" class="aptSelector" onclick="check_aptSelector()" style="margin: 0;" />
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
                                                                                <th width="180">
                                                                                    Nome Struttura
                                                                                </th>
                                                                                <th width="120">
                                                                                    Zona
                                                                                </th>
                                                                                <th width="30">
                                                                                    Eco
                                                                                </th>
                                                                                <th width="30">
                                                                                    SRS
                                                                                </th>
                                                                                <th width="30">
                                                                                    Escl.
                                                                                </th>
                                                                                <th width="30">
                                                                                    Ins.B.
                                                                                </th>
                                                                                <th width="60">
                                                                                    Comm.
                                                                                </th>
                                                                                <th width="110" style="text-align: right;">
                                                                                    Prezzo nel periodo
                                                                                </th>
                                                                                <th width="80">
                                                                                    <input id="hf_selList" type="hidden" value=""  />
                                                                                    <asp:LinkButton ID="lnk_selectFromLV" runat="server" OnClientClick="RNT_fillList('list', $('#hf_selList').val(), ''); return true;" CssClass="lnk_selectFromLV sel" Style="display: none;" OnClick="lnk_editAptListSelect_Click">Seleziona</asp:LinkButton>
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
                                                <asp:Panel ID="pnl_showAptList" runat="server" CssClass="fasciatitBar" Style="height: 20px; display:none;  overflow: hidden; position: relative;">
                                                </asp:Panel>

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
                                            function check_aptSelector() {
                                                var tmpStr = "";
                                                var count=0;
                                                $('.aptSelector:checked').each(function (i, selected) {
                                                    tmpStr += $(selected).val() + "|";
                                                    count++;
                                                });
                                                $("#hf_selList").val(tmpStr);
                                                if (count > 0) {
                                                    $(".lnk_selectFromLV").html("Seleziona " + count);
                                                    $(".lnk_selectFromLV").show();
                                                }
                                                else {
                                                    $(".lnk_selectFromLV").hide();
                                                }
                                            }

                                                </script>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                
                        </div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Area / Zona</span>
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
								<tr>
									<td>
										<asp:Literal ID="ltr_area" runat="server"></asp:Literal>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="bottom">
						<div style="float: left;">
							<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
					</div>
				</div>
			</div>
			<div class="mainline">
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<span class="titoloboxmodulo">Correlazione</span>
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
								<tr>
									<td colspan="2">
										<asp:HyperLink ID="HL_related_request" runat="server"></asp:HyperLink>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:ListView runat="server" ID="LV_relatedRequests">
											<ItemTemplate>
												<a href="rnt_request_details.aspx?id=<%# Eval("id")%>">
													rif. <%# Eval("id")%> - <%# Eval("name_full")%> - del <%# Eval("request_date_created")%></a>
												<br />
											</ItemTemplate>
											<AlternatingItemTemplate>
												<a href="rnt_request_details.aspx?id=<%# Eval("id")%>">rif.
													<%# Eval("id")%>
													-
													<%# Eval("name_full")%>
													- del
													<%# Eval("request_date_created")%></a>
												<br />
											</AlternatingItemTemplate>
											<EmptyDataTemplate>
												Richiesta Unica
											</EmptyDataTemplate>
											<LayoutTemplate>
											Richieste Correlate:<br />
												<a id="itemPlaceholder" runat="server" />
											</LayoutTemplate>
										</asp:ListView>
									</td>
								</tr>
								<tr id="pnl_setRelatedRequest" runat="server" visible="false">
									<td colspan="2">
										Imposta come secondaria di: 
										<asp:DropDownList ID="drp_relatedRequests" runat="server">
										</asp:DropDownList>
										<div class="nulla">
										</div>
										<span class="error_text" id="lbl_relatedRequestError" runat="server" visible="false">!Attenzione! Selezionare Una richiesta</span>
										<asp:LinkButton ID="lnk_setRelatedRequest" runat="server" onclick="lnk_setRelatedRequest_Click">Salva</asp:LinkButton>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="center">
						<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<uc3:UC_rnt_request_operator ID="UC_rnt_request_operator1" runat="server" />
							</ContentTemplate>
						</asp:UpdatePanel>
					</div>
					<div class="bottom">
						<div style="float: left;">
							<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
					</div>
				</div>
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<asp:UpdatePanel ID="UpdatePanel_UC_rnt_rl_request_state" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<uc2:UC_rnt_rl_request_state ID="UC_rnt_rl_request_state1" runat="server" />
							</ContentTemplate>
						</asp:UpdatePanel>
					</div>
					<div class="bottom">
						<div style="float: left;">
							<img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
					</div>
				</div>
			</div>
			<div class="nulla">
			</div>
            <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">
				<asp:TextBox runat="server" ID="txt_css" Width="230px"></asp:TextBox>
				Banner Laterali:
				<asp:DropDownList ID="drp_static_collection" runat="server">
				</asp:DropDownList>
			</asp:PlaceHolder>

        </ContentTemplate>
	</asp:UpdatePanel>
    <asp:Literal ID="ltr_items" runat="server" Visible="false"></asp:Literal>
    <script type="text/javascript">
        function selectAllZones() {
            if ($("#chk_zone_all").is(':checked')) {
                $("#<%=lbx_flt_zone.ClientID %> option").attr("selected", "selected");
            }
        }
        var items = [
			<%=ltr_items.Text %>
		];
		
		
		function setAutocomplete(){
			$( ".aptComplete" ).autocomplete({
				source: items
			});
		}
    </script>
    <script type="text/javascript">
    var _JSCal_Range;
    function setCal() {
        _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>", changeMonth: true, changeYear: true });
    }
    </script>
</asp:Content>
