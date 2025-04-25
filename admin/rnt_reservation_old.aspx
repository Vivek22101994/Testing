<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_reservation_old.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_old" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style type="text/css">
	select
	{
		font-family: verdana;
		font-size: 10px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:Literal ID="ltr_tooltip_template" runat="server" Visible="false">
	<strong>ttp_cl_name_full, [Code. ttp_code]</strong><br/>
	Persone: ttp_persons, Prezzo:&euro; ttp_pr_total<br/>
	Scadenza: ttp_block_expire
	</asp:Literal>
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="1" runat="server" />
			<h1 class="titolo_main">Gestione Prenotazioni e Disponibilità delle Strutture</h1>
			<!-- INIZIO MAIN LINE -->
			<div class="mainline">
				<!-- BOX 1 -->
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center">
						<asp:Literal ID="ltr_empty" runat="server" Visible="false">IG</asp:Literal>
						<asp:Literal ID="ltr_filter_estates" runat="server" Visible="false"></asp:Literal>
						<asp:HiddenField runat="server" ID="HF_pid_estate" Value="0" />
						<asp:HiddenField ID="HF_unique" Value="" runat="server" />
						<table border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 20px;">
							<tr>
								<td>
									<table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
										<tr>
											<td>
												<label class="barraTop">
													Visualizza:
													<span>
														<select id="drp_dateMode" onchange="changeDateMode(this.value)">
															<option value="1">Periodo</option>
															<option value="2">Mese</option>
														</select>
													</span>
													<span style="display:none;">
														<input type="radio" id="rb_dateMode_period" onchange="changeDateMode('1')">
														<label>Periodo</label>
														<input type="radio" id="rb_dateMode_month" onchange="changeDateMode('2')">
														<label>Mese</label>
													</span>
												&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
										</tr>
										<tr>
											<td id="pnl_flt_month" style="display: none;">
												<asp:RadioButtonList ID="rbl_month" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
												</asp:RadioButtonList>
											</td>
											<td id="pnl_flt_period" style="display: none;">
												<table class="inp">
													<tr>
														<td>
															<label>
																dal:</label>
														</td>
														<td>
															<input type="text" id="cal_date_from" style="width: 120px" />
															<img src="" id="del_date_from" style="cursor: pointer;" alt="X" title="Pulisci" />
														</td>
													</tr>
													<tr>
														<td>
															<label>
																al (compreso):</label>
														</td>
														<td>
															<input type="text" id="cal_date_to" style="width: 120px" />
															<img src="" id="del_date_to" style="cursor: pointer;" alt="X" title="Pulisci" />
														</td>
													</tr>
												</table>
												<asp:HiddenField ID="HF_date_from" runat="server" />
												<asp:HiddenField ID="HF_date_to" runat="server" />
											</td>
										</tr>
										<tr>
											<td>
												con stato:&nbsp;
												<asp:DropDownList ID="drp_flt_state" runat="server">
												</asp:DropDownList>
											</td>
										</tr>
									</table>
									<asp:HiddenField ID="HF_dateMode" runat="server" Value="1" />
									
									<script type="text/javascript">
										function changeDateMode(_dateMode) {
											$get("<%= HF_dateMode.ClientID %>").value = _dateMode;
											$get("drp_dateMode").options[parseInt(_dateMode)-1].selected = true;
											//$get("rb_dateMode_month").checked = _dateMode == "2";
											$get("pnl_flt_month").style.display = _dateMode == "1" ? "none" : "";
											$get("pnl_flt_period").style.display = _dateMode == "1" ? "" : "none";
										}
									</script>
								</td>
								<td id="pnl_flt_estate_zone" runat="server">
									<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
									<table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
										<tr>
											<td>
												<label class="barraTop">
													Zone:</label>
												<asp:ListBox ID="lbx_zone" runat="server" SelectionMode="Multiple" Rows="10" AutoPostBack="true" onselectedindexchanged="lbx_zone_SelectedIndexChanged"></asp:ListBox>
											</td>
										</tr>
									</table>
									<table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px; float: left;">
										<tr>
											<td>
												<label class="barraTop">
													Appartamenti:</label>
												<asp:ListBox ID="lbx_estate" runat="server" SelectionMode="Multiple" Rows="10"></asp:ListBox>
											</td>
										</tr>
									</table>
									</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td id="pnl_flt_prop" runat="server" >
									<table cellpadding="0" cellspacing="0" style="background-color: #f0f0f0; margin-right: 10px;">
										<tr>
											<td colspan="2">
												<label class="barraTop">
													Filtro caratteristiche:</label>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Min. #.Posti Letto
											</td>
											<td>
												<asp:DropDownList ID="drp_min_num_persons_max" runat="server">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Min. #.Camere
											</td>
											<td>
												<asp:DropDownList ID="drp_min_num_rooms_bed" runat="server">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Min. #.Stelle
											</td>
											<td>
												<asp:DropDownList ID="drp_min_importance_stars" runat="server">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Aria Condizionata
											</td>
											<td>
												<asp:DropDownList ID="drp_has_air_condition" runat="server">
													<asp:ListItem Text="--" Value="-1"></asp:ListItem>
													<asp:ListItem Text="SI" Value="1"></asp:ListItem>
													<asp:ListItem Text="NO" Value="0"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												In Esclusiva
											</td>
											<td>
												<asp:DropDownList ID="drp_is_exclusive" runat="server">
													<asp:ListItem Text="--" Value="-1"></asp:ListItem>
													<asp:ListItem Text="SI" Value="1"></asp:ListItem>
													<asp:ListItem Text="NO" Value="0"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Attico
											</td>
											<td>
												<asp:DropDownList ID="drp_is_loft" runat="server">
													<asp:ListItem Text="--" Value="-1"></asp:ListItem>
													<asp:ListItem Text="SI" Value="1"></asp:ListItem>
													<asp:ListItem Text="NO" Value="0"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td class="td_title">
												Internet
											</td>
											<td>
												<asp:DropDownList ID="drp_has_internet" runat="server">
													<asp:ListItem Text="--" Value="-1"></asp:ListItem>
													<asp:ListItem Text="Adsl o Wifi" Value="2or3"></asp:ListItem>
													<asp:ListItem Text="solo Adsl" Value="2"></asp:ListItem>
													<asp:ListItem Text="solo Wifi" Value="3"></asp:ListItem>
													<asp:ListItem Text="Adsl e Wifi" Value="2and3"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr runat="server" visible="false">
											<td class="td_title">
												Solo Con Internet
											</td>
											<td>
												<asp:DropDownList ID="DropDownList6" runat="server">
												</asp:DropDownList>
											</td>
										</tr>
									</table>
								</td>
								<td>
									<div class="salvataggio">
										<div class="bottom_salva">
											<asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra Risultati</span></asp:LinkButton>
										</div>
										<div class="bottom_salva">
											<asp:LinkButton Visible="false" ID="lnk_stamp" runat="server" CssClass="ricercaris" Style="margin-left: 20px;" OnClick="lnk_stamp_Click"><span>Stampa</span></asp:LinkButton>
										</div>
										<div class="nulla">
										</div>
									</div>
								</td>
							</tr>
						</table>
						<table width="100%">
							<tr>
								<td>
									<asp:LinqDataSource ID="LDS_availability" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_LK_RESERVATION_STATEs" OrderBy="title" 
										Where="title != @title && id!=3">
										<WhereParameters>
											<asp:Parameter Name="title" DefaultValue="" Type="String" />
										</WhereParameters>
									</asp:LinqDataSource>
									<asp:ListView ID="LV_availability" DataSourceID="LDS_availability" runat="server" EnableViewState="false">
										<ItemTemplate>
											<td>
												<span class="<%# (""+Eval("css_class")).ToLower() %>" style="width: 30px;"></span>
											</td>
											<td>
												<span>
													<%# Eval("abbr") + "&nbsp;-&nbsp;" + Eval("title")%></span>
											</td>
											<td style="width: 10px;">
											</td>
										</ItemTemplate>
										<EmptyDataTemplate>
											empty
										</EmptyDataTemplate>
										<LayoutTemplate>
											<table>
												<tr>
													<td>
														<span class="res_free" style="width:30px;"></span>
													</td>
													<td>
														<span>
															Disponibile</span>
													</td>
													<td style="width: 10px;">
													</td>
													<td id="itemPlaceholder" runat="server" />
												</tr>
											</table>
										</LayoutTemplate>
									</asp:ListView>
									<asp:LinqDataSource ID="LDS_estates" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE">
									</asp:LinqDataSource>
									<asp:ListView ID="LV_estates" runat="server" OnItemDataBound="LV_estates_ItemDataBound" OnDataBound="LV_estates_DataBound">
										<ItemTemplate>
											<tr class="tr_normal" onmouseout="SetClassName(this,'tr_normal')" onmouseover="SetClassName(this,'tr_current')">
												<td class="nomiTab">
													<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
													<%# (Eval("code") + " rif." + Eval("id")).htmlNoBreakSpace()%>
												</td>
												<asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
													<ItemTemplate>
														<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
														<td id="td_cont" runat="server">
															<asp:Label ID="lbl_unique_id1" runat="server" Style="display: none;" />
															<asp:Label ID="lbl_unique_id2" runat="server" Style="display: none;" />
															<asp:Label ID="lbl_stato1" runat="server" />
															<asp:Label ID="lbl_stato2" runat="server" />
														</td>
													</ItemTemplate>
													<EmptyDataTemplate>
														empty
													</EmptyDataTemplate>
													<LayoutTemplate>
														<td id="itemPlaceholder" runat="server" />
													</LayoutTemplate>
												</asp:ListView>
											</tr>
										</ItemTemplate>
										<AlternatingItemTemplate>
											<tr class="tr_alternate" onmouseout="SetClassName(this,'tr_alternate')" onmouseover="SetClassName(this,'tr_current')">
												<td class="nomiTab">
													<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
													<%# (Eval("code") + " rif." + Eval("id")).htmlNoBreakSpace()%>
												</td>
												<asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
													<ItemTemplate>
														<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
														<td id="td_cont" runat="server">
															<asp:Label ID="lbl_unique_id1" runat="server" Style="display: none;" />
															<asp:Label ID="lbl_unique_id2" runat="server" Style="display: none;" />
															<asp:Label ID="lbl_stato1" runat="server" />
															<asp:Label ID="lbl_stato2" runat="server" />
														</td>
													</ItemTemplate>
													<EmptyDataTemplate>
														empty
													</EmptyDataTemplate>
													<LayoutTemplate>
														<td id="itemPlaceholder" runat="server" />
													</LayoutTemplate>
												</asp:ListView>
											</tr>
										</AlternatingItemTemplate>
										<EmptyDataTemplate>
										</EmptyDataTemplate>
										<LayoutTemplate>
											<table cellpadding="1" cellspacing="0" class="tabDays" border="1">
												<tr>
													<td>
													</td>
													<asp:ListView ID="LV_date" runat="server" OnItemDataBound="LV_date_ItemDataBound">
														<ItemTemplate>
															<td class="dateTab" id="td_cont" runat="server">
																<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
																<asp:Label ID="lbl_stato" runat="server"/>
															</td>
														</ItemTemplate>
														<EmptyDataTemplate>
															empty
														</EmptyDataTemplate>
														<LayoutTemplate>
															<td id="itemPlaceholder" runat="server" />
														</LayoutTemplate>
													</asp:ListView>
												</tr>
												<tr id="itemPlaceholder" runat="server" />
											</table>
											<div class="page">
												<asp:DataPager ID="DataPager2" runat="server" PageSize="20" style="border-right: medium none;">
													<Fields>
														<asp:NumericPagerField ButtonCount="20" />
													</Fields>
												</asp:DataPager>
												<asp:Label ID="lbl_record_count_top" runat="server" CssClass="total" Text=""></asp:Label>
												<div class="nulla">
												</div>
											</div>
										</LayoutTemplate>
									</asp:ListView>
								</td>
							</tr>
						</table>
					</div>
					<div class="nulla">
					</div>
					<div class="nulla">
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
			<asp:Literal ID="ltr_tooltip_cont" runat="server"></asp:Literal>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
