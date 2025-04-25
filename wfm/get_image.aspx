<%@ Page Title="" Language="C#" MasterPageFile="MP.Master" AutoEventWireup="true" CodeBehind="get_image.aspx.cs" Inherits="WebFileManager.wfm.get_image" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head" runat="server">

	<script type="text/javascript">
		function showToolTip() {
			$(function() {
				$('tr.tooltip').tooltip({ track: true, delay: 0, showURL: false, top: 3, left: 3 });
			});
		}
	</script>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_main" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="HF_callback" runat="server" Value="" />
			<asp:HiddenField ID="HF_return" runat="server" Value="" />
			<asp:HiddenField ID="HF_current_file" runat="server" Value="" />
			<asp:HiddenField ID="HF_current_id" runat="server" Value="" />
			<asp:HiddenField ID="HF_action" runat="server" Value="Directory_In" />
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td style=" background-color:White;">
						<asp:Literal ID="ltr_folder" runat="server"></asp:Literal>
					</td>
				</tr>
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
												<asp:Panel ID="pnl_directory_out" runat="server">
												<img alt="up" src="ico/folder_up.png" onclick="FileAction('','')" style="cursor: pointer;"/></asp:Panel>
											</td>
											<td>
												<asp:ImageButton ID="ibtn_upload" runat="server" ImageUrl="~/wfm/ico/upload_img.png" onclick="ibtn_upload_Click" />
											</td>
											<td>
												<asp:ImageButton ID="ibtn_create_folder" runat="server" ImageUrl="~/wfm/ico/folder_new.png" OnClick="ibtn_create_folder_Click" />
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
						<asp:ListView ID="LV" runat="server" onitemdatabound="LV_ItemDataBound">
							<ItemTemplate>
								<tr class="tooltip" title="/<%# Eval("PathRoot") %>" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')" onclick="FileAction('<%# Eval("Name") %>','<%# Eval("Extension")%>','<%# Eval("id") %>')">
									<td style=" cursor:pointer;">
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<%# getFileImg("" + Eval("Extension"))%>
									</td>
									<td style="cursor: pointer;">
										<asp:Label ID="lbl_name" runat="server" Text='<%# Eval("Name") %>' />
									</td>
									<td>
										<span>
											<%# Eval("Extension")%></span>
										<td>
											<span>
												<%# Eval("SizeString")%></span>
										</td>
								</tr>
							</ItemTemplate>
							<EmptyDataTemplate>
								<table id="Table1" runat="server" style="">
									<tr>
										<td>
											<h1>Cartella vuota!</h1>.
											<img alt="up" src="ico/folder_up.png" onclick="FileAction('','')" style="cursor: pointer;" />
										</td>
									</tr>
								</table>
							</EmptyDataTemplate>
							<LayoutTemplate>
								<div class="table_fascia">
									<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
										<tr id="Tr1" runat="server" style="text-align: left">
											<th id="Th3" runat="server">
											</th>
											<th id="Th1" runat="server">
												Nome
											</th>
											<th id="Th4" runat="server" style="width: 50px">
												Ext
											</th>
											<th id="Th7" runat="server" style="width: 100px">
												Size
											</th>
										</tr>
										<tr id="itemPlaceholder" runat="server">
										</tr>
									</table>
								</div>
							</LayoutTemplate>
						</asp:ListView>
					</td>
				</tr>
			</table>
			<asp:Button ID="btn_page_update" runat="server" Text="Button" Style="display: none;" onclick="btn_page_update_Click" />
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="btn_page_update" />
		</Triggers>
	</asp:UpdatePanel>

	<script type="text/javascript">
		function FileAction(Name, Extension, Id) {
			if (Extension == "folder") {
				$get("<%=HF_current_file.ClientID %>").value = "" + Name;
				$get("<%=HF_current_id.ClientID %>").value = "" + Id;
				$get("<%=HF_action.ClientID %>").value = "Directory_In";
			}
			else if (Name == "" && Extension == "") {
				$get("<%=HF_current_file.ClientID %>").value = "";
				$get("<%=HF_current_id.ClientID %>").value = "" + Id;
				$get("<%=HF_action.ClientID %>").value = "Directory_Out";
			}
			else if (Name != "" && Extension != "") {
				$get("<%=HF_current_file.ClientID %>").value = "" + Name;
				$get("<%=HF_current_id.ClientID %>").value = "" + Id;
				$get("<%=HF_action.ClientID %>").value = "get";
			}
			ReloadContent();
		}
		var contentUpdater = "<%= btn_page_update.ClientID %>";
		function ReloadContent() {
			buttonPostBack(contentUpdater);return;
			__doPostBack(contentUpdater, "Directory_In");
		}
	</script>
</asp:Content>
