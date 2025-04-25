<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_lastminute.aspx.cs" Inherits="RentalInRome.stp_lastminute" %>

<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
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
    <%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + "</h3>" : ""%>
    <div class="nulla">
    </div>
    <div id="imgLastMinute">
        <img src="images/css/last-minute.jpg" alt="last minute" style="margin-left: -25px;" />
    </div>
    <div class="txtLastMinute">
        <%=CurrentSource.getSysLangValue("lblLastMinuteTxt")%>
        <br />
        <br />
        <label class="desc">
        <%=CurrentSource.getSysLangValue("lblContactUs")%>:</label>
        <br />
        <br />
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" align="left" style="width: 50px; color: #595959">
                    <strong>
                        <%=CurrentSource.getSysLangValue("lblTel")%></strong>
                </td>
                <td valign="top" align="left">
                    +39 06 3220068
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" align="left" style="width: 50px; color: #595959">
                    <strong>Email</strong>
                </td>
                <td valign="top" align="left">
                    <a href="mailto:lastminute@rentalinrome.com">lastminute@rentalinrome.com</a>
                </td>
            </tr>
        </table>
        <div class="nulla">
        </div>
        <br />
        <br />
        <strong><em style="color:#333366">
        <%=CurrentSource.getSysLangValue("lblOnlyArrivals72Hours")%></em></strong>
        <br />
        <div class="nulla">
        </div>
        <div style="float: left; padding-top: 10px;">
            
            <a id="_lpChatBtn" href='http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&byhref=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' target='chat1220380' onclick="lpButtonCTTUrl = 'http://server.iad.liveperson.net/hc/1220380/?cmd=file&file=visitorWantsToChat&site=1220380&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a&referrer='+escape(document.location); lpButtonCTTUrl = (typeof(lpAppendVisitorCookies) != 'undefined' ? lpAppendVisitorCookies(lpButtonCTTUrl) : lpButtonCTTUrl); lpButtonCTTUrl = ((typeof(lpMTag)!='undefined' && typeof(lpMTag.addFirstPartyCookies)!='undefined')?lpMTag.addFirstPartyCookies(lpButtonCTTUrl):lpButtonCTTUrl);window.open(lpButtonCTTUrl,'chat1220380','width=472,height=320,resizable=yes');return false;">
                <img src='http://server.iad.liveperson.net/hc/1220380/?cmd=repstate&site=1220380&channel=web&&ver=1&imageUrl=http://server.iad.liveperson.net/hcp/Gallery/ChatButton-Gallery/English/General/3a' name='hcIcon' border="0"></a></td></tr></table><!-- END LivePerson Button code -->
            </a>
        </div>
    </div>
    
    <div class="nulla">
    </div>
    
    <uc3:uc_apt_in_rome_bottom id="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
