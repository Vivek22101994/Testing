<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_electriccar.aspx.cs" Inherits="RentalInRome.stp_electriccar" %>

<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
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
    <div id="testoMain" style="width:365px; background:none;">
       
        <div class="mainContent">
         <%= ltr_title.Text != "" ? "<h3 class='underlined' style='margin-left: 0;'>" + ltr_title.Text + "</h3>" : ""%>
            <div class="nulla">
            </div>
            <%=ltr_description.Text %>
        </div>
    </div>
    <div id="media" style="margin:20px 20px 0 0;">
    <div id="videoApt" style="height: 326px">
        <object width="506" height="320">
            <param name="movie" value="http://www.youtube.com/v/ZmlKIM1Rnh4?fs=1&amp;hl=it_IT&amp;hd=1"></param>
            <param name="allowFullScreen" value="true"></param>
            <param name="allowscriptaccess" value="always"></param>
            <embed src="http://www.youtube.com/v/ZmlKIM1Rnh4?fs=1&amp;hl=it_IT&amp;hd=1" type="application/x-shockwave-flash" width="506" height="320" allowscriptaccess="always" allowfullscreen="true"></embed></object>
    </div>
        
    </div>
    <table class="priceTable" cellspacing="0" cellpadding="0" style="margin:20px 20px 20px 0;">
            <tr>
                <th valign="middle" align="left">
                    <%=CurrentSource.getSysLangValue("lblPrices")%>
                </th>
                <th valign="middle" align="center">
                    1 
                    <%=CurrentSource.getSysLangValue("lblHour")%>
                </th>
                
                <th valign="middle" align="center">
                    1 
                    <%=CurrentSource.getSysLangValue("lblDay")%>
                </th>
                
            </tr>
            <tr>
                <td valign="middle" align="left">
                    
                </td>
                <td valign="middle" align="center">
                    25,00€
                </td>
                
                <td valign="middle" align="center">
                    120,00€
                </td>
            </tr>
        </table>
     
    <div class="galleryTour" style="margin:0 9px 0 0; width:528px; float:right;">
        <a rel="shadowbox" href="/images/auto-elettriche/foto1.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t1.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto2.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t2.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto3.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t3.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto4.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t4.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto5.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t5.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto6.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t6.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto7.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t7.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto8.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t8.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto9.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t9.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto10.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t10.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto11.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t11.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto12.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t12.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto13.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t13.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto14.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t14.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto15.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t15.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto16.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t16.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto17.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t17.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto18.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t18.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto19.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t19.jpg">
        </a>
        <a rel="shadowbox" href="/images/auto-elettriche/foto20.jpg">
            <img alt="" src="/images/auto-elettriche/foto-t20.jpg">
        </a>
    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
