<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHA_unit.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHA_unit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHAEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         function enablemaster(obj) {
             if (obj.checked == true) {
                 document.getElementById("<%=drp_master_estate.ClientID%>").disabled = false;
            }
            else {
                document.getElementById("<%=drp_master_estate.ClientID%>").disabled = true;
            }
        }

    </script>
    <script type="text/javascript">
        function checkmasterestate(oSrc, args) {
            if ((document.getElementById("<%=chk_slave.ClientID%>").checked == true) && args.Value == 0) {
                args.IsValid = false;
                return false;
            }
            else {
                args.IsValid = true;
                return true;
            }
        }
        </script>
        <style type="text/css">
            .tabsTop.tabsChannelsTop table td a[title="Unit"], #tabsHomeaway.tabsTop table td a[title="Unit"]  {
	            background:#848484;
	            border-color:#606060;
	            color:#FFF;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/homeaway-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with HomeAway" />

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_DettagliPosizioneApt")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1" style="margin-bottom:10px;">
                <div class="tabsTop tabsChannelsTop tabsHomeAwayTop" id="tabsHomeaway">
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
                                              <asp:DropDownList ID="drp_is_active" runat="server" style="width:50px;" >
                                                  <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                                                  <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                                
                                              </asp:DropDownList>
                                                
                                            </td>
                                        </tr>
                                         <tr>
                                            <td valign="middle" align="left">slave? </td>
                                            <td valign="middle" align="right" colspan="3">
                                             <asp:CheckBox ID="chk_slave" runat="server" onclick="enablemaster(this);" />
                                                
                                            </td>
                                        </tr>
                                         <tr>
                                            <td valign="middle" align="left">Master Apartment</td>
                                            <td valign="middle" align="right" colspan="3">
                                              <asp:DropDownList ID="drp_master_estate" runat="server"></asp:DropDownList>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<br/>//obbligatorio" ClientValidationFunction="checkmasterestate" ValidationGroup="dati" Display="Dynamic" ControlToValidate="drp_master_estate"></asp:CustomValidator>

                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left" style="width:30%">Area</td>
                                            <td style="width:30%;">

                                            </td>
                                            <td valign="middle" align="left" style="width:15%;">
                                                <asp:TextBox runat="server" ID="txt_area" style="width:30px; height:15px;"/></td>
                                            <td valign="middle" align="left" style="width:25%;">
                                                <asp:DropDownList ID="drp_unit" runat="server">
                                                    <asp:ListItem Text="Meters Squared" Value="METERS_SQUARED"></asp:ListItem>
                                                     <asp:ListItem Text="Square Feet" Value="SQUARE_FEET"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                    </table>



                                    <asp:HiddenField ID="HF_lang" Value="1" runat="server" />
                                    <asp:LinqDataSource ID="LDS_langs" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="id" TableName="CONT_TBL_LANGs" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:ListView ID="LV_langs" runat="server" DataSourceID="LDS_langs" OnItemCommand="LV_langs_ItemCommand" OnItemDataBound="LV_langs_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_lang" CommandName="change_lang" CssClass="tab_item" runat="server">
                                                <span>
                                                    <%# Eval("title") %></span>
                                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("abbr") %>' />
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

                                                   <%-- <tr>
                                                        <td colspan="2">
                                                            <span class="titoloboxmodulo">Visualizzazione</span>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td colspan="2"><strong>Unit Name</strong><br />
                                                            <br />
                                                           <asp:TextBox ID="txt_unit_name" runat="server" style="float:left; width:99%; margin:0;" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <strong>Unit Description:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_unit_description" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                      <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            <strong>Bathroom Details:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_bathroom_details" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                     </tr>
                                                      <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            <strong>Bedroom Details:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_bedroom_details" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            <strong>Features Description:</strong><br />
                                                            <br />
                                                            <telerik:RadEditor runat="server" ID="re_features_description" SkinID="DefaultSetOfTools" Height="400" Width="100%" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileAdvanced.xml">
                                                                <CssFiles>
                                                                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                                                                </CssFiles>
                                                            </telerik:RadEditor>
                                                        </td>
                                                    </tr>


                                                </table>
                                           
                                    <table cellpadding="3" cellspacing="0" style="width: 50%">
                                        
                                        <tr>
                                            <td><strong>Main Property Type</strong></td>
                                           <td><asp:DropDownList ID="drp_property_type" runat="server" style="float:left;"></asp:DropDownList></td>
                                                                                   
                                        </tr>
                                        <tr>
                                            <td><span class="titoloboxmodulo" style="background:none;border-bottom:none;">
                                                Unit Feature Values:</span>
                                            </td>
                                        </tr>
                                        </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Accomodation</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_accomodation" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Amenities</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_amenities" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                              <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                                </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Entertaiment</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_entertaiment" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Kitchen and Dining</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_kitchen" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Outdoor</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_outdoor" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Pool and Spa</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_pool" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Suitability</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_suitability" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="titoloboxmodulo" style="background:none;">Theme</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <asp:DataList ID="dl_theme" runat="server" RepeatColumns="3" OnItemDataBound="dl_ItemDataBound">
                                            <ItemTemplate>

                                                <td>
                                                    <asp:CheckBox ID="chk" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="nomeLabel" runat="server" Text='<%# Eval("title") %>' />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_count" runat="server" Style="width: 45px;">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("code") %>' />
                                                </td>

                                            </ItemTemplate>
                                        </asp:DataList>
                                            </td>
                                        </tr>
                                        </table>
                                    <table>
                                         <tr>
                                            <td><strong>Lounge Seating</strong></td>                                        

                                            <td>
                                               <asp:DropDownList ID="drp_lounge_seating" runat="server" style="float:left;"></asp:DropDownList>
                                            </td>                                         
                                        </tr>
                                         <tr>
                                            <td><strong>Dining Seating</strong></td>                                        

                                            <td>
                                               <asp:DropDownList ID="drp_dining_seating" runat="server" style="float:left;"></asp:DropDownList>
                                            </td>                                         
                                        </tr>
                                         <tr>
                                            <td><strong>Max Sleep</strong></td>                                        

                                            <td>
                                               <asp:DropDownList ID="drp_max_sleep" runat="server" style="float:left;"></asp:DropDownList>
                                            </td>
                                         
                                        </tr>
                                        <tr>
                                            <td><strong>Max Sleep in Beds</strong></td>                                        

                                            <td>
                                               <asp:DropDownList ID="drp_max_sleep_bed" runat="server" style="float:left;"></asp:DropDownList>
                                            </td>                                         
                                        </tr>

                                    </table>
                               </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>