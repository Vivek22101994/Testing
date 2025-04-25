<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_usr_tbl_admin_availability_view.ascx.cs" Inherits="RentalInRome.admin.uc.UC_usr_tbl_admin_availability_view" %>
<asp:Literal ID="ltr_empty" runat="server" Visible="false">IG</asp:Literal>
<asp:Literal ID="ltr_filter_users" runat="server" Visible="false"></asp:Literal>
<asp:HiddenField runat="server" ID="HF_pid_admin" Value="0" />
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<table border="0" cellpadding="0" cellspacing="0" style="margin-bottom:20px;">
				<tr>
				
				<td>
					<table cellpadding="0" cellspacing="0" style="background-color:#f0f0f0; margin-right:10px;">
						<tr>
					<td style="width:100px;">
						<label class="barraTop">Visualizza:</label>
						<asp:RadioButtonList ID="rbl_mode" runat="server" RepeatDirection="Horizontal" onselectedindexchanged="rbl_mode_SelectedIndexChanged" AutoPostBack="true">
							<asp:ListItem Text="Periodo" Value="1"></asp:ListItem>
							<asp:ListItem Text="Mese" Value="2"></asp:ListItem>
						</asp:RadioButtonList>	
					</td>
					<td id="pnl_flt_month" runat="server" style="width: 250px; background-color: #333366; color:#FFF;">
						<asp:RadioButtonList ID="rbl_month" runat="server">
						</asp:RadioButtonList>
					</td>
					</tr>
					<tr>
					<td id="pnl_flt_period" runat="server" colspan="2" style="width: 250px;">
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
					</table>
				</td>	
					
					<td id="pnl_flt_admins" runat="server" style="background-color:#f0f0f0;">
						<label class="barraTop">
							Produttori:
							<span>
							Seleziona/Deseleziona tutti <asp:CheckBox ID="chk_admins_all" runat="server" Checked="true"  oncheckedchanged="chk_admins_all_CheckedChanged" AutoPostBack="true" />
							</span>
						</label>
						<asp:CheckBoxList ID="chkList_admins" runat="server" RepeatColumns="4" AutoPostBack="true" OnSelectedIndexChanged="chkList_admins_SelectedIndexChanged">
						</asp:CheckBoxList>
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
			<asp:LinqDataSource ID="LDS_availability" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_LK_ADMIN_AVAILABILITies" OrderBy="title" Where="title != @title">
				<WhereParameters>
					<asp:Parameter Name="title" DefaultValue="" Type="String" />
				</WhereParameters>
			</asp:LinqDataSource>
			<asp:ListView ID="LV_availability" DataSourceID="LDS_availability" runat="server">
				<ItemTemplate>
					<td class="state_<%# (""+Eval("title")).ToLower() %>">
						<span><%# Eval("title")%></span>
					</td>
					<td>
						<span>
							<%# Eval("description") %></span>
					</td>
					<td style="width:10px;">
					</td>
				</ItemTemplate>
				<EmptyDataTemplate>
					empty
				</EmptyDataTemplate>
				<LayoutTemplate>
					<table>
						<tr>
							<td id="itemPlaceholder" runat="server" />
						</tr>
					</table>
				</LayoutTemplate>
			</asp:ListView>
			<asp:LinqDataSource ID="LDS_admins" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_ADMIN">
			</asp:LinqDataSource>
			<asp:ListView ID="LV_admins" runat="server" OnItemDataBound="LV_admins_ItemDataBound" DataSourceID="LDS_admins" OnDataBound="LV_admins_DataBound">
				<ItemTemplate>
					<tr class="tr_normal" onmouseout="SetClassName(this,'tr_normal')" onmouseover="SetClassName(this,'tr_current')">
						<td class="nomiTab">
							<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
							<%# Eval("name")%> <%# Eval("surname")%>
						</td>
						<asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
							<ItemTemplate>
								<td id="td_cont" runat="server">
									<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
									<asp:Label ID="lbl_stato" runat="server" />
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
							<%# Eval("name")%> <%# Eval("surname")%>
						</td>
						<asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
							<ItemTemplate>
								<td ID="td_cont" runat="server">
									<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
									<asp:Label ID="lbl_stato" runat="server" />
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
					<table cellpadding="2" cellspacing="0" class="tabDays">
						<tr>
							<td>
							</td>
							<asp:ListView ID="LV_date" runat="server" OnItemDataBound="LV_date_ItemDataBound">
								<ItemTemplate>
									<td class="dateTab">
										<asp:Label ID="lbl_date" Visible="false" runat="server" Text='<%# ((DateTime)Container.DataItem).JSCal_dateToString() %>' />
										<asp:Label ID="lbl_stato" runat="server" />
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
				</LayoutTemplate>
			</asp:ListView>
		</td>
	</tr>
</table>

<script type="text/javascript">
		//var cal_date_from = new JSCal_single('cal_date_from', '<%= HF_date_from.ClientID %>', 'cal_date_from', null, '', 'del_date_from');
		//var cal_date_to = new JSCal_single('cal_date_to', '<%= HF_date_to.ClientID %>', 'cal_date_to', null, '', 'del_date_to');
</script>

