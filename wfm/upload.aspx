<%@ Page Title="" Language="C#" MasterPageFile="~/wfm/MP.Master" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="WebFileManager.wfm.upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_main" runat="server">
		<asp:HiddenField ID="HF_callback" runat="server" Value="" />
		<asp:HiddenField ID="HF_return" runat="server" Value="" />
		<asp:HiddenField ID="HF_referer" runat="server" Value="" />
		<asp:HiddenField ID="HF_folder" runat="server" Value="" />
		<table>
			<tr>
				<td>
					<table cellpadding="0" cellspacing="0" border="0" width="100%">
						<tr>
							<td class="sub_menu_sx_dot">
							</td>
							<td class="sub_menu_bg">
								<table cellpadding="0" cellspacing="0" border="0">
									<tr>
										<td>
											<asp:ImageButton ID="ibtn_back" runat="server" ImageUrl="~/wfm/ico/back.png" Height="20px" OnClick="ibtn_back_Click" />
										</td>
									</tr>
								</table>
							</td>
							<td class="sub_menu_dx_dot">
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<asp:FileUpload ID="FU_uplaod" runat="server" /><br />
					<asp:Button ID="btn_uplaod" runat="server" Text="Upload" onclick="btn_uplaod_Click" />
				</td>
			</tr>
		</table>
</asp:Content>
