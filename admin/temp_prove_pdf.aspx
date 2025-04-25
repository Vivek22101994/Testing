<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="temp_prove_pdf.aspx.cs" Inherits="RentalInRome.admin.temp_prove_pdf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
				<span class="titoloboxmodulo">..</span>
				<div class="boxmodulo">
					<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
						<tr>
							<td colspan="2">
								<asp:LinkButton ID="lnk_create" runat="server" OnClick="lnk_create_Click">crea</asp:LinkButton>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								Url<br/>
								<asp:TextBox ID="txt_content" runat="server" Width="900"></asp:TextBox>
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
	<div class="nulla">
	</div>
</asp:Content>
