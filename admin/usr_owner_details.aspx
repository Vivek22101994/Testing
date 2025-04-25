<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="usr_owner_details.aspx.cs" Inherits="RentalInRome.admin.usr_owner_details" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="uc/UC_get_image.ascx" TagName="UC_get_image" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_usr_owner_password.ascx" TagName="UC_usr_owner_password" TagPrefix="uc2" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
            <h1 class="titolo_main">Scheda Proprietario<%= HF_id.Value=="0"?"&nbsp;Nuovo Proprietario":"" %></h1>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 2:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_2" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 3:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_3" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 4:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_4" Width="300px" />
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
                                <tr runat="server" visible="false">
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
                        <span class="titoloboxmodulo">Note Interne</span>
                        <div class="boxmodulo">
                            <telerik:RadEditor runat="server" ID="re_notesInner" SkinID="DefaultSetOfTools" Height="200" Width="400" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                                <CssFiles>
                                    <telerik:EditorCssFile Value="" />
                                </CssFiles>
                            </telerik:RadEditor>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
                <div class="mainbox" style="clear: right; float:none;">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <uc2:UC_usr_owner_password ID="UC_usr_owner_password1" runat="server" />
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
				<div class="mainbox">
					<div class="top">
						<div style="float: left;">
							<img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
						<div style="float: right;">
							<img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
					</div>
					<div class="center"><br />
						<span class="titoloboxmodulo">
							Appartamenti correlati</span>
						<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_TB_ESTATE" Where="pid_owner==@pid_owner" OrderBy="sequence">
							<WhereParameters>
								<asp:ControlParameter ControlID="HF_id" Name="pid_owner" Type="Int32" />
							</WhereParameters>
						</asp:LinqDataSource>
						<asp:ListView ID="LV" runat="server" DataSourceID="LDS" >
							<ItemTemplate>
								<tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
									<td>
										<span>
											<%# Eval("id") %></span>
									</td>
									<td>
										<span class="">
											<%# Eval("code")%></span>
									</td>
									<td class="ico">
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<a href='rnt_estate_details.aspx?id=<%# Eval("id") %>' class="scheda" target="_blank" title="Scheda">
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
										<span class="">
											<%# Eval("code")%></span>
									</td>
									<td class="ico">
										<asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
										<a href='rnt_estate_details.aspx?id=<%# Eval("id") %>' class="scheda" target="_blank" title="Scheda">
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
							<InsertItemTemplate>
							</InsertItemTemplate>
							<LayoutTemplate>
								<div class="table_fascia">
									<table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
										<tr id="Tr1" runat="server" style="">
											<th style="width:30px; padding-right:20px;">ID </th>
											<th id="Th1" runat="server" style="width:150px; padding-right: 20px;">
												Nome<%--<asp:LinkButton runat="server" CssClass="lnk_view" Text="Nome &#9660;" ID="lnk_code" OnClick="lnk_code_Click"></asp:LinkButton>--%>
											</th>
											<th id="Th6" runat="server" style="width: auto; padding-right: 20px;"></th>
										</tr>
										<tr id="itemPlaceholder" runat="server">
										</tr>
									</table>
								</div>
							</LayoutTemplate>
						</asp:ListView>
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
