<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_limousineservice.aspx.cs" Inherits="RentalInRome.stp_limousineservice" %>


<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_lang" runat="server" Value="2" />
    <div id="limoServPage">

        <h3 id="priceBar" class="titBar">
            <%= ltr_title.Text%>
        </h3>

        <div class="nulla">
        </div>
        <div class="txtLeft">
            <%= ltr_description.Text%>
            <div class="nulla"></div>


            <a class="btn" href="<%=CurrentSource.getPagePath("32", "stp", CurrentLang.ID.ToString()) %>" style="margin-top: 20px;">
                <span><%=CurrentSource.getSysLangValue("lblPickupReservation")%></span>
            </a>
            <a class="btn" href="<%=CurrentSource.getPagePath("17", "stp", CurrentLang.ID.ToString()) %>" style="margin-top: 20px;">
                <span><%=CurrentSource.getSysLangValue("lblTermsAndConditions")%></span>
            </a>

            <uc1:UC_static_block ID="UC_static_block2" runat="server" BlockID="5" />


            <a class="rentCar" href="<%=CurrentSource.getPagePath("15", "stp", CurrentLang.ID.ToString()) %>">
                <span>
                    <%=CurrentSource.getSysLangValue("lblRentAnElectricCar")%>
                </span>
            </a>

        </div>

        <div id="media">
            <div id="videoApt" style="height: 320">
                <object width="506" height="320">
                    <param name="movie" value="http://www.youtube.com/v/QuME84eQOBg?fs=1&amp;hl=it_IT&amp;rel=0"></param>
                    <param name="allowFullScreen" value="true"></param>
                    <param name="allowscriptaccess" value="always"></param>
                    <embed src="http://www.youtube.com/v/QuME84eQOBg?fs=1&amp;hl=it_IT&amp;rel=0" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="506" height="320"></embed></object>
            </div>
        </div>

        <div class="nulla">
        </div>

        <div id="pickupPrices">
            <h3 class="titBar">
                <%=CurrentSource.getSysLangValue("lblPickupInfoPrices")%>
            </h3>


            <div class="infoPickup">
                <uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="6" />
            </div>

            <table class="priceTable" cellspacing="0" cellpadding="0">
                <tr>
                    <th valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblArrivalsDepartures")%>
                    </th>
                    <th valign="middle" align="center">1–3 <%=CurrentSource.getSysLangValue("lblPax")%></th>
                    <th valign="middle" align="center">4–5 <%=CurrentSource.getSysLangValue("lblPax")%></th>
                    <th valign="middle" align="center">6 <%=CurrentSource.getSysLangValue("lblPax")%></th>
                    <th valign="middle" align="center">7–8 <%=CurrentSource.getSysLangValue("lblPax")%></th>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblAirports")%></td>
                    <td valign="middle" align="center">50,00€</td>
                    <td valign="middle" align="center">65,00€</td>
                    <td valign="middle" align="center">75,00€</td>
                    <td valign="middle" align="center">85,00€</td>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblRailwayStation")%></td>
                    <td valign="middle" align="center">40,00€</td>
                    <td valign="middle" align="center">48,00€</td>
                    <td valign="middle" align="center">58,00€</td>
                    <td valign="middle" align="center">68,00€</td>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblRoundTrip")%>
                    </td>
                    <td valign="middle" align="center">95,00€
                    </td>
                    <td valign="middle" align="center">120,00€
                    </td>
                    <td valign="middle" align="center">140,00€
                    </td>
                    <td valign="middle" align="center">160,00€
                    </td>
                </tr>
            </table>

            <table class="priceTable" cellspacing="0" cellpadding="0">
                <tr>
                    <th valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblArrivalsDepartures")%>
                    </th>
                    <th valign="middle" align="center">1–2 <%=CurrentSource.getSysLangValue("lblPax")%>
                    </th>
                    <th valign="middle" align="center">3–5 <%=CurrentSource.getSysLangValue("lblPax")%>
                    </th>
                    <th valign="middle" align="center">6-8 <%=CurrentSource.getSysLangValue("lblPax")%>
                    </th>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="text-transform: uppercase;">
                        <%=CurrentSource.getSysLangValue("lblCivitavecchia")%>
                    </td>
                    <td valign="middle" align="center">150,00€
                    </td>
                    <td valign="middle" align="center">170,00€
                    </td>
                    <td valign="middle" align="center">190,00€
                    </td>
                </tr>
            </table>

        </div>
    </div>
    <div id="excPage">
        <h3 id="excBar" class="titBar"><%=CurrentSource.getSysLangValue("lblPrivateExcursionsAndCityTours")%></h3>

        <div id="excList">
            <asp:ListView ID="LV_tour" runat="server" DataSourceID="LDS_tour">
                <ItemTemplate>
                    <a class="exc" href="/<%# Eval("page_path") %>">
                        <img src="/<%# Eval("img_banner") %>" alt="<%# Eval("title") %>" />
                        <span class="titExc"><%# Eval("title") %></span>
                        <span class="txtExc">
                            <%# Eval("summary") %></span>
                    </a>
                </ItemTemplate>
                <LayoutTemplate>
                    <a id="itemPlaceholder" runat="server" />
                </LayoutTemplate>
            </asp:ListView>
            <asp:LinqDataSource ID="LDS_tour" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" TableName="CONT_VIEW_TOURs" Where="is_acitve == 1 && pid_lang == @pid_lang">
                <WhereParameters>
                    <asp:ControlParameter ControlID="HF_lang" Name="pid_lang" PropertyName="Value" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
        </div>


        <div class="txtRight">

            <uc1:UC_static_block ID="UC_static_block7" runat="server" BlockID="7" />

            <table class="priceTable" style="width: 255px;" cellpadding="0" cellspacing="0">
                <tr>
                    <th valign="middle" align="left" colspan="2"><%=CurrentSource.getSysLangValue("lblPrice")%>*</th>
                </tr>
                <tr>
                    <td valign="middle" align="left">1-3 <%=CurrentSource.getSysLangValue("lblPax")%> (4 <%=CurrentSource.getSysLangValue("lblHours")%>)
                    </td>
                    <td valign="middle" align="left">150€
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">4-6 <%=CurrentSource.getSysLangValue("lblPax")%> (4 <%=CurrentSource.getSysLangValue("lblHours")%>)
                    </td>
                    <td valign="middle" align="left">175€
                    </td>
                </tr>
            </table>
            <div style="float: right; width: 255px; font-size: 11px; color: #FE6634; line-height: 12px;">
                *<%=CurrentSource.getSysLangValue("lblCancellationOfYourReservation")%>
            </div>
        </div>

    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
