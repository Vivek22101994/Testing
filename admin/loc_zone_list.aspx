<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="loc_zone_list.aspx.cs" Inherits="RentalInRome.admin.loc_zone_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" TableName="LOC_TB_ZONEs">
	</asp:LinqDataSource>
	<div id="fascia1">
		<div class="pannello_fascia1">
			<div style="clear: both">
				<h1>Elenco Zone</h1>
                <div class="bottom_agg">
                    <a href="loc_zone_details.aspx?id=0"><span>+ Nuovo</span></a>
                </div>
            </div>
			<div style="clear: both">
				<asp:ListView ID="LV" runat="server" DataSourceID="LDS">
					<ItemTemplate>
						<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
							<td>
								<span>
									<%# Eval("id") %></span>
							</td>
							<td>
								<span>
									<%# Eval("img_thumb")%></span>
							</td>
							<td>
								<span>
									<%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(),1,"-! non assegnato !-")  %></span>
							</td>
							<td>
								<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
								<a href="loc_zone_details.aspx?id=<%# Eval("id") %>" style="color: #9b3513; margin-top: 4px;">modifica</a>
                                <a href="limo_transportDuration.aspx?IdZone=<%# Eval("id") %>" target="_blank" style="color: #9b3513; margin-top: 4px; margin-left: 10px;">Gestione Transfer</a>
                            </td>
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
							<td>
								<span>
									<%# Eval("id") %></span>
							</td>
							<td>
								<span>
									<%# Eval("img_thumb")%></span>
							</td>
							<td>
								<span>
									<%# CurrentSource.loc_cityTitle(Eval("pid_city").objToInt32(),1,"-! non assegnato !-")  %></span>
							</td>
							<td>
								<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
								<a href="loc_zone_details.aspx?id=<%# Eval("id") %>" style="color: #9b3513; margin-top: 4px;">modifica</a>
                                <a href="limo_transportDuration.aspx?IdZone=<%# Eval("id") %>" target="_blank" style="color: #9b3513; margin-top: 4px; margin-left: 10px;">Gestione Transfer</a>
                            </td>
						</tr>
					</AlternatingItemTemplate>
					<EmptyDataTemplate>
						<table id="Table1" runat="server" style="">
							<tr>
								<td>
									No data was returned.
								</td>
							</tr>
						</table>
					</EmptyDataTemplate>
					<LayoutTemplate>
						<div class="table_fascia">
							<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
								<tr id="Tr1" runat="server" style="text-align: left">
									<th id="Th2" runat="server" style="width: 30px">
										ID
									</th>
									<th id="Th1" runat="server" style="width: 300px">
										Nome Interno
									</th>
									<th id="Th4" runat="server" style="width: 150px">
										Città
									</th>
									<th id="Th3" runat="server">
									</th>
								</tr>
								<tr id="itemPlaceholder" runat="server">
								</tr>
							</table>
						</div>
					</LayoutTemplate>
				</asp:ListView>
			</div>
		</div>
		<div style="clear: both">
		</div>
	</div>
	<div class="nulla">
	</div>
</asp:Content>
