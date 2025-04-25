<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_country_lang.aspx.cs" Inherits="RentalInRome.admin.usr_country_lang" %>

<%@ Register Src="uc/UC_usr_rl_country_lang.ascx" TagName="UC_usr_rl_country_lang" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
	<asp:HiddenField ID="HF_id" Value="1" runat="server" />
	<h1 class="titolo_main">Gestione Lingue e Account per Location </h1>
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
							<table>
								<tr>
									<td>
										Location:<br/>
										<asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" DataSourceID="LDS_country" DataTextField="title" DataValueField="id" 
											onselectedindexchanged="drp_country_SelectedIndexChanged" AutoPostBack="true">
										</asp:DropDownList>
										<asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
											<WhereParameters>
												<asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
											</WhereParameters>
										</asp:LinqDataSource>
									</td>
									<td>
										Lingua:<br/>
										<asp:DropDownList ID="drp_lang" runat="server" OnSelectedIndexChanged="drp_lang_SelectedIndexChanged" AutoPostBack="true">
										</asp:DropDownList>
									</td>
									<td>
										Account:<br />
										<asp:DropDownList ID="drp_admin" runat="server" OnSelectedIndexChanged="drp_admin_SelectedIndexChanged" AutoPostBack="true">
										</asp:DropDownList>
									</td>
								</tr>
							</table>
							<div class="nulla">
							</div>
						</div>
						<div class="center">
							<uc1:UC_usr_rl_country_lang ID="UC_usr_rl_country_lang" runat="server" />
							<div class="nulla">
							</div>
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
					</ContentTemplate>
				</asp:UpdatePanel>
</asp:Content>
