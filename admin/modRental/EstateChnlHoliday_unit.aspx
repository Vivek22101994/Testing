<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlHoliday_unit.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateChnlHoliday_unit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucHLEstateDetailsTab.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function displayEssentials() {
            if (Page_ClientValidate("dati") && Page_ClientValidate("dati1")) {
                document.getElementById('div_essentials').style.display = "block";
                document.getElementById('div_features').style.display = "none";
                document.getElementById('div_suitability').style.display = "none";
            }
        }
        function displayFacilities() {
            if (Page_ClientValidate("dati")) {
                document.getElementById('div_essentials').style.display = "none";
                document.getElementById('div_features').style.display = "block";
                document.getElementById('div_suitability').style.display = "none";
                calculateTotalBath();
            }
        }
        function displaySuitability() {
            if (Page_ClientValidate("dati") && Page_ClientValidate("dati1")) {
                document.getElementById('div_essentials').style.display = "none";
                document.getElementById('div_features').style.display = "none";
                document.getElementById('div_suitability').style.display = "block";
            }
        }
        function displaySecureParking() {            
            if (document.getElementById('<%=chk_parking.ClientID%>').checked == true) {
                document.getElementById('<%=td_secure_parking.ClientID%>').style.display = "block";                
            }
            else
                document.getElementById('<%=td_secure_parking.ClientID%>').style.display = "none";
        }
        function calculateTotalBath() {
            var family_bath = document.getElementById('<%=drp_bathrooms.ClientID%>').value;
            var en_suite = document.getElementById('<%=drp_en_suite.ClientID%>').value;
            var shower_rooms = document.getElementById('<%=drp_shower_rooms.ClientID%>').value;
            var total_bath = parseInt(family_bath) + parseInt(en_suite) + parseInt(shower_rooms);
            document.getElementById('<%=txt_total_bathrooms.ClientID%>').value = total_bath;
            document.getElementById('<%=HF_total_bathrooms.ClientID%>').value = total_bath;
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <img src="/images/css/hl-integrated.jpg" class="homeAwayLogo" alt="Integrated with Holiday Lettings" style="height:65px;width:130px;" />
            <h1 class="titolo_main">Details of Unit for Holiday Lettings:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>
            <div id="fascia1">
                <div class="tabsTop" id="tabsHomeaway">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                    <div class="nulla" style="height: 20px;"></div>
                    <a onclick="displayEssentials()" class="tab">Step-1: Essentials</a>
                    <a onclick="displayFacilities()" class="tab">Step-2: Facilities and features</a>
                    <a onclick="displaySuitability()" class="tab">Step-3 :Suitability</a>
                </div>
            </div>

            <%-- <div class="copiaIncolla">
                <asp:LinkButton ID="lnk_copyLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_copyLang_Click" CssClass="btnCopia">copia</asp:LinkButton>
                <asp:LinkButton Visible="false" ID="lnk_pasteLang" runat="server" OnClientClick="removeTinyEditor()" OnClick="lnk_pasteLang_Click" CssClass="btnCopia">incolla</asp:LinkButton><asp:HiddenField ID="HF_copyLang" Value="0" runat="server" Visible="false" />
            </div>--%>


            <div class="nulla">
            </div>

            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" ValidationGroup="dati" OnClick="lnk_saveOnly_Click"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="mainline">
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
                                    <div id="div_essentials">
                                        <table cellpadding="3" cellspacing="0" style="width: 96%;">
                                            <tr>
                                                <td colspan="2">
                                                    <span class="titoloboxmodulo" style="background: none;">The Essentials</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td><strong>Home Id</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_home_id" runat="server" ReadOnly="true" Style="width: 200px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><strong>Site Id</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_SiteId" runat="server" ReadOnly="true" Style="width: 200px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><strong>ActiveState</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_ActiveState" runat="server" ReadOnly="true" Style="width: 200px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><strong>Property name *:</strong>
                                                </td>
                                                <td>
                                                    <span class="error_text" id='count_txt_name' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                    <br />

                                                    <asp:TextBox ID="txt_name" runat="server" MaxLength="30" Style="width: 200px;"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_name" ErrorMessage="**" Display="Dynamic" ValidationGroup="dati" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txt_name" Display="Dynamic" Text="//30 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,30}$" ValidationGroup="dati" runat="server" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><strong>Main Property Type</strong></td>
                                                <td>
                                                    <asp:DropDownList ID="drp_property_type" runat="server" Style="float: left;">
                                                        <asp:ListItem Text="Appartamento" Value="apt"></asp:ListItem>
                                                        <asp:ListItem Text="Villa" Value="villa"></asp:ListItem>
                                                        <asp:ListItem Text="Residence" Value="residence"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td><strong>No. of bedrooms *:</strong>
                                                </td>
                                                <td style="width: 60px;">
                                                    <asp:DropDownList ID="drp_num_bedrooms" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drp_num_bedrooms" ErrorMessage="**" Display="Dynamic" InitialValue="" ValidationGroup="dati" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    <%--  <asp:TextBox runat="server" ID="txt_num_rooms_bed" Width="30px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_num_rooms_bed" ErrorMessage="**" ValidationGroup="dati"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_num_rooms_bed" ErrorMessage="**Value must be greater than zero" ValidationExpression="^[1-9][0-9]*$" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><strong>Maximum guests *</strong></td>
                                                <td>
                                                    <asp:DropDownList ID="drp_max_guest" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drp_max_guest" ErrorMessage="**" Display="Dynamic" InitialValue="0" ValidationGroup="dati" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    <%--<asp:TextBox runat="server" ID="txt_max_sleep" Width="30px" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_max_sleep" ErrorMessage="**" ValidationGroup="dati"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_max_sleep" ErrorMessage="**Value must be greater than zero" ValidationExpression="^[1-9][0-9]*$" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>

                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <strong>Description:</strong>

                                                    <br />
                                                    <br />
                                                    <asp:TextBox ID="txt_unit_description" runat="server" TextMode="MultiLine" MaxLength="270" Height="400" Width="800"></asp:TextBox>
                                            </tr>
                                        </table>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td><strong>Search results summary
                                                </strong></td>
                                                <td>
                                                    <span class="error_text" id='count_txt_summary' style="display: none"><span class="char"></span>&nbsp;caratteri e&nbsp; <span class="word"></span>&nbsp;parole </span>
                                                    <br />
                                                    <asp:TextBox ID="txt_summary" runat="server" TextMode="MultiLine" MaxLength="270" Height="200" Width="800"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txt_summary" Display="Dynamic" Text="//270 caratteri massimo<br/>" ValidationExpression="^[\s\S]{0,270}$" ValidationGroup="dati" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="nulla"></div>
                                    </div>
                                    <div id="div_features" style="display: none;">
                                        <table style="width: 96%;">
                                            <tr>
                                                <td colspan="2">
                                                    <span class="titoloboxmodulo" style="background: none;">Facilities and Features</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="nulla"></div>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td style="width: 300px;">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span style="background: none; font-weight: bold;">Beds</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Single beds :</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_single_beds" runat="server"></asp:DropDownList>
                                                                <%-- <asp:TextBox runat="server" ID="txt_num_bed_single" Width="30px" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txt_num_bed_single" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Double beds:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_double_beds" runat="server"></asp:DropDownList>
                                                                <%--<asp:TextBox runat="server" ID="txt_num_bed_double" Width="30px" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txt_num_bed_double" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                --%>                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Sofa beds :</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_sofa_beds" runat="server"></asp:DropDownList>
                                                                <%--<asp:TextBox runat="server" ID="txt_num_sofa" Width="30px" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txt_num_sofa" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                --%>  </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Cots:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_cots" runat="server"></asp:DropDownList>
                                                                <%--<asp:TextBox runat="server" ID="txt_cots" Width="30px" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_cots" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 300px;">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span style="background: none; font-weight: bold;">Bathrooms</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Family bathrooms :</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_bathrooms" runat="server" onchange="calculateTotalBath()"></asp:DropDownList>
                                                                <%--<asp:TextBox runat="server" ID="txt_bath_rooms" Width="30px" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_bath_rooms" ErrorMessage="**" ValidationGroup="dati"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_bath_rooms" ErrorMessage="**Value must be greater than zero" ValidationExpression="^[1-9][0-9]*$" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>En suites :</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_en_suite" runat="server" onchange="calculateTotalBath()"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Shower rooms:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_shower_rooms" runat="server" onchange="calculateTotalBath()"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Total bathrooms:</strong>
                                                            </td>
                                                            <td>
                                                               <asp:TextBox ID="txt_total_bathrooms" ReadOnly="true" runat="server"></asp:TextBox>
                                                                <asp:HiddenField ID="HF_total_bathrooms" runat="server" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_total_bathrooms" ErrorMessage="**" ValidationGroup="dati1"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_total_bathrooms" ErrorMessage="**Value must be greater than zero" ValidationExpression="^[1-9][0-9]*$" ValidationGroup="dati1" Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span style="background: none; font-weight: bold;">Seating</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Dining seats:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_dining_seats" runat="server"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Lounge seats:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_longue_seats" runat="server"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility: hidden;">
                                                            <td><strong></strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility: hidden;">
                                                            <td><strong>Lounge seats:</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="nulla"></div>

                                        <table style="width: 96%;">
                                            <tr>
                                                <td>
                                                    <span class="titoloboxmodulo" style="background: none;">Indoors</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_cooker" runat="server" Text="Cooker" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_tv" runat="server" Text="TV" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_log_fire" runat="server" Text="Log fire" />

                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_friedge" runat="server" Text="Fridge" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_satelite_tv" runat="server" Text="Satellite TV" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_central_heating" runat="server" Text="Central heating" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_frezer" runat="server" Text="Freezer" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_video_player" runat="server" Text="Video player" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_air_condition" runat="server" Text="Air conditioning" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_micro_wave" runat="server" Text="Microwave" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_dvd" runat="server" Text="DVD player" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_linen" runat="server" Text="Linen provided" />
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_kettle" runat="server" Text="Kettle" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_internet" runat="server" Text="Internet access" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_sauna" runat="server" Text="Sauna" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_washing_machine" runat="server" Text="Washing machine" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_telephone" runat="server" Text="Telephone" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_table_tennis" runat="server" Text="Table tennis" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_dishwasher" runat="server" Text="Dishwasher" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_wifi" runat="server" Text="Wi-Fi available" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_gym" runat="server" Text="Gym" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_cloth_dryer" runat="server" Text="Clothes dryer" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_fax" runat="server" Text="Fax machine" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_pool_snooker" runat="server" Text="Pool or snooker table" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_iron" runat="server" Text="Iron" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_hair_dryer" runat="server" Text="Hair dryer" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_games_room" runat="server" Text="Games room" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_high_chair" runat="server" Text="High chair" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_safe" runat="server" Text="Safe" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_staffed_property" runat="server" Text="Staffed property" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_toaster" runat="server" Text="Toaster" />
                                                </td>
                                                <td style="display:none;">
                                                    <asp:CheckBox ID="chk_cd" runat="server" Text="CD player" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_towels" runat="server" Text="Towels provided" />
                                                </td>
                                            </tr>
                                        </table>

                                        <div class="nulla"></div>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td>
                                                    <span class="titoloboxmodulo" style="background: none;">Outdoors</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td style="width: 300px;">
                                                    <asp:CheckBox ID="chk_shared_outoor_heated_pool" runat="server" Text="Shared outdoor pool (heated)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_shared_tennis" runat="server" Text="Shared tennis court" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_roof_terrace" runat="server" Text="Solarium or roof terrace" />

                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_shared_outoor_unheated_pool" runat="server" Text="Shared outdoor pool (unheated)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_tennis" runat="server" Text="Private tennis court" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_balcony_terrace" runat="server" Text="Balcony or terrace" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_outdoor_heated_pool" runat="server" Text="Private outdoor pool (heated)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_garden" runat="server" Text="Private garden" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_fishing_lake" runat="server" Text="Private fishing lake or river" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_outdoor_unheated_pool" runat="server" Text="Private outdoor pool (unheated)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_climbing_frame" runat="server" Text="Climbing frame" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_boat" runat="server" Text="Boat available" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_private_indoor_pool" runat="server" Text="Private indoor pool" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_swing_set" runat="server" Text="Swing set" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_bicycles" runat="server" Text="Bicycles available" />
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_children_pool" runat="server" Text="Children's pool" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_bbq" runat="server" Text="BBQ" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_seaview" runat="server" Text="Sea view" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_shared_indoor_pool" runat="server" Text="Shared indoor pool" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_trampoline" runat="server" Text="Trampoline" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_parking" runat="server" Text="Parking" onchange="displaySecureParking()"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_hot_tub" runat="server" Text="Jacuzzi or hot tub" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_shared_garden" runat="server" Text="Shared garden" />
                                                </td>
                                                <td id="td_secure_parking" runat="server">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chk_secure_parking" runat="server" Text="Secure parking"/>
                                                </td> 
                                            </tr>

                                        </table>

                                    </div>

                                    <div id="div_suitability">

                                        <table cellpadding="3" cellspacing="0" style="width: 96%;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label1" runat="server" Style="font-size: 12px; color: red;"></asp:Label>
                                                    <span class="titoloboxmodulo" style="background: none;">Suitability</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 96%;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_long_term_lets" runat="server" Text="Long term lets (over 1 month)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_corporate_bookings" runat="server" Text="Corporate bookings" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_house_swap" runat="server" Text="House swap" />

                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chk_short_break" runat="server" Text="Short breaks (1-4 days)" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_hen_stag_break" runat="server" Text="Hen or stag breaks" />
                                                </td>
                                                <td></td>
                                            </tr>

                                        </table>
                                        <div class="nulla"></div>
                                        <table>
                                            <tr>
                                                <td>Children
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_chidren" runat="server">
                                                        <asp:ListItem Text="Not suitable for children" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Suitable for children over 5" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Great for children of all ages" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Pets
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_pets" runat="server">
                                                        <asp:ListItem Text="No pets allowed" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Some pets are welcome - please contact the owner" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Pets welcome" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Smokers
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_smokers" runat="server">
                                                        <asp:ListItem Text="No smoking at this property" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Yes, smoking allowed" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Restricted mobility 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_restricted_mobility" runat="server">
                                                        <asp:ListItem Text="Not suitable" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Yes, lift access to property" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Yes, suitable" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Wheelchair users 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drp_wheelchair_users" runat="server">
                                                        <asp:ListItem Text="Not suitable" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Property accessible for wheelchair users" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Property adapted for wheelchair users" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
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
