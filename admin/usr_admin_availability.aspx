<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_admin_availability.aspx.cs" Inherits="RentalInRome.admin.usr_admin_availability" %>

<%@ Register Src="uc/UC_usr_tbl_admin_availability_view.ascx" TagName="UC_usr_tbl_admin_availability_view" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_usr_tbl_admin_availability_insert.ascx" TagName="UC_usr_tbl_admin_availability_insert" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="1" runat="server" />
			<h1 class="titolo_main">Gestione disponibilità dei Produttori</h1>
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
						<uc1:UC_usr_tbl_admin_availability_view ID="UC_usr_tbl_admin_availability_view1" runat="server" />
						<uc1:UC_usr_tbl_admin_availability_insert ID="UC_usr_tbl_admin_availability_insert1" runat="server" />
						<div class="nulla">
						</div>
						<div class="salvataggio">
							<div class="bottom_salva">
								<asp:LinkButton ID="lnk_view" runat="server" onclick="lnk_view_Click"><span>Visualizza</span></asp:LinkButton>
							</div>
							<div class="bottom_salva">
								<asp:LinkButton ID="lnk_insert" runat="server" onclick="lnk_insert_Click"><span>Inserisci</span></asp:LinkButton>
							</div>
							<div class="nulla">
							</div>
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
