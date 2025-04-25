<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_client_details.aspx.cs" Inherits="RentalInRome.admin.usr_client_details" %>

<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_usr_client_password.ascx" TagName="UC_usr_client_password" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:HiddenField ID="HF_id" Value="0" runat="server" />
			<asp:HiddenField ID="HF_lang" Value="1" runat="server" />
			<h1 class="titolo_main">Scheda Cliente</h1>
			<!-- INIZIO MAIN LINE -->
			<div class="salvataggio">
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_modify" runat="server" OnClick="lnk_modify_Click"><span>Modifica</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_salva" runat="server" ValidationGroup="dati" OnClick="lnk_salva_Click"><span>Salva Modifiche</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
				</div>
				<div class="bottom_salva">
					<a href="<%= listPage %>">
						<span>Torna nel elenco</span></a>
				</div>
				<div class="nulla">
				</div>
			</div>
			<div class="nulla">
			</div>
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
						<span class="titoloboxmodulo">Dati Identificativi</span>
						<div class="boxmodulo">
							<table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Titolo:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_honorific" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
									<td class="td_title">
										Nome/Cognome:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_name_full" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Email:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_contact_email" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Telefono:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_contact_phone" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Cellulare:
									</td>
									<td>
										<asp:TextBox runat="server" ID="txt_contact_phone_mobile" Width="300px" />
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Location:
									</td>
									<td>
										<asp:DropDownList runat="server" ID="drp_country" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id">
										</asp:DropDownList>
										<asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == 1">
										</asp:LinqDataSource>
									</td>
								</tr>
								<tr>
									<td class="td_title">
										Lingua:
									</td>
									<td>
										<asp:DropDownList runat="server" ID="drp_lang" OnDataBound="drp_lang_DataBound" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
										</asp:DropDownList>
										<asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
										</asp:LinqDataSource>
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
						<uc2:UC_usr_client_password ID="UC_usr_client_password1" runat="server" />
						<div class="nulla">
						</div>
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
			<asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false"></asp:PlaceHolder>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
