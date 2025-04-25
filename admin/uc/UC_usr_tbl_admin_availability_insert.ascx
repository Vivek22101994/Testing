<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_usr_tbl_admin_availability_insert.ascx.cs" Inherits="RentalInRome.admin.uc.UC_usr_tbl_admin_availability_insert" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td>
			<label>
				Periodo:</label>
			<table class="inp">
				<tr>
					<td>
						<label>
							dal:</label>
					</td>
					<td>
						<input type="text" id="cal_dtIns_from" style="width: 120px" />
						<img src="" id="del_date_from" style="cursor: pointer;" alt="X" title="Pulisci" />
					</td>
				</tr>
				<tr>
					<td>
						<label>
							al (compreso):</label>
					</td>
					<td>
						<input type="text" id="cal_dtIns_to" style="width: 120px" />
						<img src="" id="del_date_to" style="cursor: pointer;" alt="X" title="Pulisci" />
					</td>
				</tr>
			</table>
			<asp:HiddenField ID="HF_date_from" runat="server" />
			<asp:HiddenField ID="HF_date_to" runat="server" />
		</td>
		<td>
			<asp:LinqDataSource ID="LDS_users" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_ADMIN" OrderBy="name" Where="pid_role == @pid_role">
				<WhereParameters>
					<asp:Parameter DefaultValue="2" Name="pid_role" Type="Int32" />
				</WhereParameters>
			</asp:LinqDataSource>
			<label>
				Account:</label>
			<asp:DropDownList ID="drp_admins" runat="server" CssClass="inp">
			</asp:DropDownList>
		</td>
		<td>
			<label>
				Tipo di Disponibilità:</label>
			<asp:DropDownList ID="drp_availability" runat="server" CssClass="inp">
			</asp:DropDownList>
			<asp:LinqDataSource ID="LDS_availability" runat="server" ContextTypeName="RentalInRome.data.magaUser_DataContext" TableName="USR_LK_ADMIN_AVAILABILITies" OrderBy="title" 
				Where="title != @title">
				<WhereParameters>
					<asp:Parameter Name="title" DefaultValue="" Type="String" />
				</WhereParameters>
			</asp:LinqDataSource>
		</td>
	</tr>
</table>
<asp:LinkButton ID="lnk_ins" runat="server" CssClass="ricercaris" OnClick="lnk_ins_Click"><span>Inserisci</span></asp:LinkButton>
