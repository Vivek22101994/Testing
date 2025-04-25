<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_usr_rl_country_lang.ascx.cs" Inherits="RentalInRome.admin.uc.UC_usr_rl_country_lang" %>
<asp:HiddenField runat="server" ID="HF_pid_country" Value="-1" />
<asp:HiddenField runat="server" ID="HF_pid_admin" Value="-1" />
<asp:HiddenField runat="server" ID="HF_pid_lang" Value="-1" />
<asp:HiddenField runat="server" ID="HF_isEdit" Value="0" />
<div class="boxmodulo pannello_fascia1">
	<asp:ListView ID="LV" runat="server" DataSourceID="LDS" OnItemCommand="LV_ItemCommand" onitemdatabound="LV_ItemDataBound" ondatabound="LV_DataBound">
		<ItemTemplate>
			<tr onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
				<td>
					<span>
						<%# getCountryName((int)Eval("pid_country"))  %></span>
				</td>
				<td>
					<span>
						<%# getLangName((int)Eval("pid_lang"))  %></span>
				</td>
				<td>
					<span <%# (""+Eval("sequence")=="1")?"style='color:red'":""%><%# (""+Eval("sequence")=="2")?"style='color:green'":""%>>
						<%# AdminUtilities.usr_adminName((int) Eval("pid_admin"),"")%></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminEmail((int)Eval("pid_admin"), "")%></span>
				</td>
				<td>
					<asp:PlaceHolder ID="PH_edit" runat="server">
						<asp:Label ID="lbl_pid_country" runat="server" Text='<%# Eval("pid_country") %>' Visible="false"></asp:Label>
						<asp:Label ID="lbl_pid_admin" runat="server" Text='<%# Eval("pid_admin") %>' Visible="false"></asp:Label>
						<asp:Label ID="lbl_pid_lang" runat="server" Text='<%# Eval("pid_lang") %>' Visible="false"></asp:Label>
						&nbsp;&nbsp;&nbsp;
						<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
						&nbsp;&nbsp;&nbsp;
						<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
					</asp:PlaceHolder>
					&nbsp;&nbsp;&nbsp;
					<asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare associaciazione del produttore a questo location?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
				<td>
					<span>
						<%# getCountryName((int)Eval("pid_country"))  %></span>
				</td>
				<td>
					<span>
						<%# getLangName((int)Eval("pid_lang"))%></span>
				</td>
				<td>
					<span <%# (""+Eval("sequence")=="1")?"style='color:red'":""%><%# (""+Eval("sequence")=="2")?"style='color:green'":""%>>
						<%# AdminUtilities.usr_adminName((int)Eval("pid_admin"), "")%></span>
				</td>
				<td>
					<span>
						<%# AdminUtilities.usr_adminEmail((int)Eval("pid_admin"), "")%></span>
				</td>
				<td>
					<asp:PlaceHolder ID="PH_edit" runat="server">
						<asp:Label ID="lbl_pid_country" runat="server" Text='<%# Eval("pid_country") %>' Visible="false"></asp:Label>
						<asp:Label ID="lbl_pid_admin" runat="server" Text='<%# Eval("pid_admin") %>' Visible="false"></asp:Label>
						<asp:Label ID="lbl_pid_lang" runat="server" Text='<%# Eval("pid_lang") %>' Visible="false"></asp:Label>
						&nbsp;&nbsp;&nbsp;
						<asp:ImageButton ID="ibtn_up" runat="server" ToolTip="Su" AlternateText="Su" CommandName="move_up" ImageUrl="~/images/ico/Go_up.png" />
						&nbsp;&nbsp;&nbsp;
						<asp:ImageButton ID="ibtn_down" runat="server" ToolTip="Giù" AlternateText="Giù" CommandName="move_down" ImageUrl="~/images/ico/Go_down.png" />
					</asp:PlaceHolder>
					&nbsp;&nbsp;&nbsp;
					<asp:LinkButton ID="lnk_delete" CommandName="elimina" ToolTip="Elimina" runat="server" OnClientClick="return confirm('Vuoi eliminare associaciazione del produttore a questo location?')"><span style="color:#333366; margin:0;">Elimina</span></asp:LinkButton>
				</td>
			</tr>
		</AlternatingItemTemplate>
		<EmptyDataTemplate>
			<table id="Table1" runat="server" style="">
				<tr>
					<td>
						Nessuna associazione
						<br />
						<div class="salvataggio">
							<div class="bottom_salva">
								<asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Crea nuova</span></asp:LinkButton>
							</div>
							<div class="nulla">
							</div>
						</div>
						<div class="nulla">
						</div>
					</td>
				</tr>
			</table>
		</EmptyDataTemplate>
		<LayoutTemplate>
			<div class="table_fascia">
				<div class="salvataggio">
					<div class="bottom_salva">
						<asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click"><span>Crea nuova</span></asp:LinkButton>
					</div>
					<div class="nulla">
					</div>
				</div>
				<div class="nulla">
				</div>
				<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
					<tr id="Tr1" runat="server" style="text-align: left">
						<th id="Th1" runat="server" style="width: 230px;">
							Location
						</th>
						<th id="Th3" runat="server" style="width: 100px;">
							Lingua
						</th>
						<th id="Th6" runat="server" style="width: 200px;">
							Account
						</th>
						<th id="Th5" runat="server">
						</th>
						<th id="Th2" runat="server">
						</th>
					</tr>
					<tr id="itemPlaceholder" runat="server">
					</tr>
				</table>
			</div>
		</LayoutTemplate>
	</asp:ListView>
	<table id="pnl_new" runat="server" visible="false">
		<tr>
			<td>
				Location:<br />
				<asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" DataSourceID="LDS_country" DataTextField="title" DataValueField="id">
				</asp:DropDownList>
				<asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
					<WhereParameters>
						<asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
					</WhereParameters>
				</asp:LinqDataSource>
			</td>
			<td>
				Lingua:<br />
				<asp:DropDownList ID="drp_lang" runat="server">
				</asp:DropDownList>
			</td>
			<td>
				Account:<br />
				<asp:DropDownList ID="drp_admin" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<asp:Label ID="lbl_error" runat="server" Visible="false" CssClass="error_text"></asp:Label>
			</td>
		</tr>
		<tr colspan="3">
			<td>
				<div class="salvataggio">
					<div class="bottom_salva">
						<asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="invoice"><span>Salva Modifiche</span></asp:LinkButton>
					</div>
					<div class="bottom_salva">
						<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
					</div>
					<div class="nulla">
					</div>
				</div>
			</td>
		</tr>
	</table>
<div class="nulla">
</div>
</div>
<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" OrderBy="pid_country, pid_lang, sequence" TableName="USR_RL_COUNTRY_LANGs" Where="(pid_country == @pid_country || @pid_country==-1) &&(pid_admin == @pid_admin || @pid_admin==-1)&&(pid_lang == @pid_lang || @pid_lang==-1)">
	<WhereParameters>
		<asp:ControlParameter ControlID="HF_pid_country" Name="pid_country" PropertyName="Value" Type="Int32" />
		<asp:ControlParameter ControlID="HF_pid_admin" Name="pid_admin" PropertyName="Value" Type="Int32" />
		<asp:ControlParameter ControlID="HF_pid_lang" Name="pid_lang" PropertyName="Value" Type="Int32" />
	</WhereParameters>
</asp:LinqDataSource>
