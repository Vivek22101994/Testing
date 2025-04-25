<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlAtraveo_main.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlAtraveo_main" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlAtraveoTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
            .tabsTop.tabsChannelsTop table td a[title="Unit"], #tabsHomeaway.tabsTop table td a[title="Unit"]  {
	            background:#848484;
	            border-color:#606060;
	            color:#FFF;
            }
            .chklist input{
                float: left !important;
            }
            .chklist label{
                float: left !important;
                margin: 3px 0 7px 5px;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/atraveo-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with Atraveo" />

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_DettagliPosizioneApt")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1" style="margin-bottom: 10px;">
                <div class="tabsTop tabsChannelsTop tabsAtraveoTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>

            <%-- <div class="copiaIncolla">
                <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click" CssClass="btnCopia">copia</asp:LinkButton>
                <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click" CssClass="btnCopia">incolla</asp:LinkButton><asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
            </div>--%>

            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" ValidationGroup="dati"><span><%= contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span><%= contUtils.getLabel("lblCancelChanges")%></span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span><%= contUtils.getLabel("lblTornaElenco")%></span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline" id="pnlError" runat="server" visible="false">
                <div class="mainbox">
                    <div class="top">
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:Literal ID="ltrErorr" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="bottom">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline mainChannels mainHomeAway mainUnit">
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="tableBoxBooking" cellpadding="0" cellspacing="0" style="width: 60%; min-width:700px;">
                                        <tr>
                                            <td valign="middle" align="left">Active </td>
                                            <td valign="middle" align="left" colspan="3">
                                              <asp:DropDownList ID="drp_isActive" runat="server" style="width:50px;" >
                                                  <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                  <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                
                                              </asp:DropDownList>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Type </td>
                                            <td valign="middle" align="left" colspan="3">
                                                <asp:DropDownList ID="drpType" runat="server">
                                                    <asp:ListItem Text="apartment" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="house" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="mobile home" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="tent" Value="4"></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Master Apartment</td>
                                            <td valign="middle" align="right" colspan="3">
                                              <asp:DropDownList ID="drp_pidMasterEstate" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Name</td>
                                            <td valign="middle" align="right" colspan="3">
                                                <asp:TextBox ID="txt_Name" runat="server" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">Arrival Days</td>
                                            <td valign="middle" align="right" colspan="3" class="chklist">
                                                <asp:CheckBoxList ID="chkList_ArrivalDays" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="7"></asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left">MinStay</td>
                                            <td valign="middle" align="right" colspan="3">
                                                <telerik:RadNumericTextBox ID="ntxtMinStay" runat="server" Width="50">
                                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="0" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                    </table>



                                    <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                    <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="isActive" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <span>No data was returned.</span>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <div class="menu2">
                                                <a id="itemPlaceholder" runat="server" />
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <strong>Object Description:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="reDescription" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <strong>Object AddInfos:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="reAddInfos" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <strong>Facility Description:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="reDescriptionFacilities" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                </table>
                                           
                               </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </div>
                    </div>
                    <div class="bottom">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

