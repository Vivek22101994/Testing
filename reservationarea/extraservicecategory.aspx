<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master"
    AutoEventWireup="true" CodeBehind="extraservicecategory.aspx.cs" Inherits="RentalInRome.reservationarea.extraservicecategory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<%@ Register Src="UC_service.ascx" TagName="UC_service" TagPrefix="uc2" %>
<%@ Register Src="UC_reservedService.ascx" TagName="UC_reservedService" TagPrefix="uc3" %>
<%@ Register Src="UC_breadcrumb.ascx" TagName="UC_breadcrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
      <script type="text/javascript">

          function Shadowbox_init() {
              Shadowbox.init({
                  handleOversize: "resize",
                  onOpen: showTooltip,
                  displayNav:true
              });
          }
          Shadowbox_init();
          function openShadowboxImg(path,width,height) {
              Shadowbox.open({
                  content: "" + path,
                  player: 'img',
                  height: height,
                  width: width
              });
          }
          function OpenShadowbox(url, width, height) {
              Shadowbox.open({
                  content: url,
                  player: 'iframe',
                  height: height,
                  width: width
              });
          }


    </script>
    <%-- <script src="../js/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
        <script src="../js/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
        <link href="../css/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />

            <script language="javascript" type="text/javascript">
                $(document).ready(function () {
                    $("a[rel=example_group]").fancybox({
                        'transitionIn': 'none',
                        'transitionOut': 'none',
                        'titlePosition': 'over',
                        'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
                            //return '<span id="">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? ' &nbsp; ' + title : '') + '</span>';
                        }
                    });
                });
    </script>--%>
    <script type="text/javascript">
        function togglemoreInfoServBox1(obj) {
            var msgAdd = document.getElementById('<%=hfd_visible.ClientID%>').value;           
            var msgHide = document.getElementById('<%=hfd_hide.ClientID%>').value;
            var mainId = obj.id.substring(0, obj.id.lastIndexOf('_'));
            if ($("#" + mainId + "_moreInfoServBox1").height() == 0) {
                $("#" + mainId + "_moreInfoServBox1").attr("class", "moreInfoServBox moreInfoOpen");

                $(obj).html(msgHide);
            }
            else {
                $("#" + mainId + "_moreInfoServBox1").attr("class", "moreInfoServBox");
                $(obj).html(msgAdd);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            var rwdUrl_onClose = null;
            var rwdUrl = null;
            function rwdUrl_OnClientClose(sender, eventArgs) {
                if (rwdUrl_onClose)
                    rwdUrl_onClose(sender, eventArgs);
            }  
            function setUrl(type, qs, onClose, closeArgs) {
                if (!closeArgs) closeArgs = "";
                if (rwdUrl == null) rwdUrl = $find("<%=rwdUrl.ClientID  %>");
                var url;
                if (type == "serviceAdd") {
                    var service = document.getElementById('<%=hfd_addService.ClientID%>').value;
                    url = "util_serviceAdd.aspx?lang=<%= App.LangID %>&closeArgs=" + closeArgs + "&" + qs;
                    rwdUrl_onClose = onClose;
                    rwdUrl.set_autoSize(true);
                    rwdUrl.set_title(service);
                    rwdUrl.set_visibleTitlebar(true);
                    rwdUrl.set_minWidth(862);
                    rwdUrl.setUrl(url);
                    rwdUrl.show();
                }
                return false;
            }
            function serviceAdd_OnClientClose(sender, eventArgs) {
            }
           
        </script>

       

<%--<link rel="stylesheet" href="../css/lightbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../js/lightbox/prototype.js"></script>
<script type="text/javascript" src="../js/lightbox/scriptaculous.js?load=effects"></script>

<script type="text/javascript" src="../js/lightbox/lightbox.js"></script>--%>

    </telerik:RadCodeBlock>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <uc4:UC_breadcrumb ID="UC_breadcrumb1" runat="server" />
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" />
    <asp:HiddenField ID="hfd_hide" runat="server" />
    <asp:HiddenField ID="hfd_visible" runat="server" />
   <asp:HiddenField ID="hfd_addService" runat="server" />
    <asp:HiddenField ID="HFLang" runat="server" />
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
            <uc3:UC_reservedService ID="UC_reservedService1" runat="server" />
            <uc2:UC_service ID="UC_Service1" runat="server" />
           <%-- <h3 class="underlined">
            <%=CurrentSource.getSysLangValue("lblYourServices")%>
            
            
            </h3>
            <div id="Div1">
                <div class="infoBox" style="font-size: 11px;">
                    <uc2:UC_service ID="UC_Service1" runat="server" />
                </div>
            </div>--%>
      
        
        </div>
        <div class="dx reservation_details_dx" style="margin-right: 20px;">
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <asp:Label ID="lblCateogryName" runat="server"></asp:Label>
                    </h3>
                    <div class="nulla">
                    </div>
                    <asp:Image ID="img_category" runat="server" class="categoryImgExSer" />
                    <%-- <img class="categoryImgExSer" src="/images/img-cat-arearis.jpg" alt="category name" />--%>
                    <div class="txtExServCat">
                        <asp:Label ID="lbl_cat_desc" runat="server"></asp:Label></div>
                    <div class="nulla">
                    </div>
                     <%-- <telerik:RadAjaxPanel ID="rapLVlist" runat="server" OnAjaxRequest="rapLVlist_AjaxRequest">--%>
                       <%-- <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">--%>

                    <ul class="ordini extraservices">
                        <asp:ListView ID="LVExtraServices" runat="server" OnItemDataBound="LVExtraServices_ItemDataBound"
                            OnItemCommand="LVExtraServices_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <div class="service">
                                        <div class="mainServBox">
                                            <div class="imgServiceMain">
                                                <img src="/<%# Eval("imgThumb") %>" />
                                            </div>
                                            <div class="dettServBox">
                                                <div style="float: left; width: 335px;">
                                                    <h3 class="titExtraServ">
                                                        <asp:Label ID="lblServiceName" runat="server"></asp:Label></h3>
                                                    <div class="txtExtraServ">
                                                        <div>
                                                            <asp:Label ID="lbl_id" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblSommario" runat="server"></asp:Label>
                                                        </div>
                                                        <asp:LinkButton ID="showMore1" runat="server" class="showMoreInfoExSe" OnClientClick="javascript:togglemoreInfoServBox1(this);return false;"><%=CurrentSource.getSysLangValue("lblShowMore")%></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="priceExtraServ" id="div_price" runat="server">
                                                    <span class="priceDettExSe1"><strong>Min Person:</strong><em><asp:Label ID="lblMinPerson"
                                                        runat="server"></asp:Label></em></span> <span class="nulla"></span><span class="priceDettExSe1">
                                                            <strong><telerik:RadCodeBlock ID="RadCodeBlock3" runat="server"><%=CurrentSource.getSysLangValue("lbl_maxPerson")%></telerik:RadCodeBlock></strong>
                                                            <em><asp:Label ID="lblMaxPerson" runat="server"></asp:Label></em></span>
                                                    <span class="nulla"></span><span class="priceDettExSe1"><strong>
                                                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server"><%=CurrentSource.getSysLangValue("lblHours")%></telerik:RadCodeBlock>:</strong><em><asp:Label
                                                        ID="lblhours" runat="server"></asp:Label></em></span> <span class="nulla"></span>
                                                    <span class="priceDettExSe1"><strong>
                                                        <telerik:RadCodeBlock ID="RadCodeBlock5" runat="server"><%=CurrentSource.getSysLangValue("lblDays")%></telerik:RadCodeBlock>:</strong><em><asp:Label ID="lblDays" runat="server"></asp:Label></em></span>
                                                    <span class="nulla"></span><span class="priceDettExSe1"><strong>
                                                        <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server"><%=CurrentSource.getSysLangValue("lblPrice")%></telerik:RadCodeBlock>:</strong><em><asp:Label
                                                        ID="lblPrice" runat="server"></asp:Label></em></span> <a onclick="return setUrl('serviceAdd', 'id=<%# Eval("id") %>&res_id=<%# UC_sx1.CurrentIdReservation %>', serviceAdd_OnClientClose, 150)"
                                                            class="addExtraServBtn"><span>+</span><%=CurrentSource.getSysLangValue("lblCalculate")%></a>
                                                    <%--<asp:LinkButton ID="lnkAddPrice" CommandArgument='<%# Eval("id") %>' CommandName="AddPrice" runat="server" CssClass="addExtraServBtn" onclick="return setUrl('cartAdd', 'id=<%# Eval("id") %>', cartAdd_OnClientClose, 150)"><span>+</span>Add</asp:LinkButton>--%>
                                                </div>
                                                 <div class="priceExtraServ" id="div_request" runat="server" style="display:none; background:none;">
                                                 <a onclick="return setUrl('serviceAdd', 'id=<%# Eval("id") %>&res_id=<%# UC_sx1.CurrentIdReservation %>', serviceAdd_OnClientClose, 150)"
                                                            class="addExtraServBtn" style="margin-top:80px;"><span>+</span><%=CurrentSource.getSysLangValue("Richiesta")%></a>
                                               
                                                 </div>
                                                <%-- <div class="priceExtraServ">
                                                    <span class="priceDettExSe1">TOTALE:</span> <strong class="priceDettExSe">€30</strong>
                                                    <span class="priceDettExSe2">per bambino</span> <a href="#" class="addExtraServBtn">
                                                        <span>+</span>Add</a>
                                                </div>--%>
                                            </div>
                                        </div>
                                        <div class="moreInfoServBox" id="moreInfoServBox1" runat="server">
                                            <div class="txtMoreInfoExSe">
                                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                            </div>
                                            <div class="galleryExtraServ">
                                                <asp:DataList ID="dlstIamges" runat="server" RepeatColumns="7" RepeatDirection="Horizontal">
                                                    <ItemTemplate>
                                                       <a href="/<%# Eval("img_banner") %>" rel ="shadowbox[<%# Eval("pid_estate_extra") %>]">
                                                      <%--  <a>--%>
                                                            <img src="/<%# Eval("img_thumb") %>" alt="" height="71" width="71" />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <ul>
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <EmptyDataTemplate>
                                <div class="nulla">
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </ul>
                  <%--  </telerik:RadCodeBlock>--%>
                     <telerik:RadWindow runat="server" ID="rwdUrl" Modal="true" AutoSize="true" ShowContentDuringLoad="false"
                        MinWidth="450" VisibleStatusbar="false" VisibleTitlebar="false" OnClientClose="rwdUrl_OnClientClose">
                    </telerik:RadWindow>
                   <%-- </telerik:RadCodeBlock>
                    </telerik:RadAjaxPanel>--%>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="nulla">
        </div>
    </div>
</asp:Content>
