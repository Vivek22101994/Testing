<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_estateListSearch.aspx.cs" Inherits="RentalInRome.webservice.rnt_estateListSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <input type="hidden" id="hf_listType" value="#listType#" />
            <input type="hidden" id="hf_orderBy" value="#orderBy#" />
            <input type="hidden" id="hf_orderHow" value="#orderHow#" />
            <input type="hidden" id="hf_currPage" value="#currPage#" />
            <div class="line1">
                #estEstateType#:<strong> #flt_type#</strong><br />
                #flt_location#
                #flt_act_pid_type#
                #fltFormatPriceRange#
                #fltFormatInnerAreaRange#
                #fltFormatEstatesOfTotal#
            </div>
            <!-- fine line1 DescRicerca-->

            <!-- line1 DescRicerca -->
            <div class="line2">
                <div class="utility">
                    <div class="tipolista">
                        <a href="javascript:list_type_change('1');" class="ico1"></a>
                        <a href="javascript:list_type_change('2');" class="ico2"></a>
                        <a href="javascript:list_type_change('3');" class="ico3" style=" display:none;"></a>
                    </div>
                    <div class="filtro">
                        <a href="#" onclick="return RNT_orderBy('title')" id="hl_orderBy_title" class="#orderBy_title#">Nome</a>
                        <a href="#" onclick="return RNT_orderBy('mq_inner')" id="hl_orderBy_mq_inner" class="#orderBy_mq_inner#">Dimensioni</a>
                        <a href="#" onclick="return RNT_orderBy('price')" id="hl_orderBy_price" class="#orderBy_price#">Prezzo</a></div>
                </div>
                <div>
                </div>
            </div>
        <asp:Literal ID="ltrLayoutTemplate" runat="server">
            <input type="hidden" id="hf_voteMin" value="#hf_voteMin#" />
            <input type="hidden" id="hf_voteMax" value="#hf_voteMax#" />
            <input type="hidden" id="hf_voteRange" value="#hf_voteRange#" />
            <input type="hidden" id="hf_voteTemp" value="#hf_voteRange#" />
            <input type="hidden" id="hf_prMin" value="#hf_prMin#" />
            <input type="hidden" id="hf_prMax" value="#hf_prMax#" />
            <input type="hidden" id="hf_prRange" value="#hf_prRange#" />
            <input type="hidden" id="hf_prTemp" value="#hf_prRange#" />
            <input type="hidden" id="hf_currPage" value="#currPage#"/>
            <div class="dati_ricercati" id="div_searchDetails">
                <div class="left_dati">
                    #fltDetails#
                </div>
                <div class="ico">
                    <a class="switch_thumb" href="#"></a>
                </div>
            </div>
            <div class="ordina">
                <span class="ord_sx">#lblOrderBy#</span>
                <div class="ord_dx">
                    <%--				
                    <a href="#" class="up"><span></span></a> 
					<a href="#" class="down"><span></span></a>
                    --%>
                    <a href="javascript:RNT_orderBy('title')" class="#orderBy_title#">
                        <span>#lblName#</span></a>
                    <a href="javascript:RNT_orderBy('price')" class="#orderBy_price#">
                        <span>#lblPrice#</span></a>
                    <a href="javascript:RNT_orderBy('vote')" class="#orderBy_vote#">
                        <span>#lblRating#</span></a>
                </div>
            </div>
            <div class="lista lista_dettagli">
                #pagerPlaceHolder#
                <ul class="display dett_view">
                    #itemPlaceHolder#
                </ul>
                #pagerPlaceHolder#
            </div>
            <div class="nulla">
            </div>
        </asp:Literal>
        <asp:Literal ID="ltrEmptyDataTemplate" runat="server">
            <input type="hidden" id="hf_voteMin" value="#hf_voteMin#" />
            <input type="hidden" id="hf_voteMax" value="#hf_voteMax#" />
            <input type="hidden" id="hf_voteRange" value="#hf_voteRange#" />
            <input type="hidden" id="hf_voteTemp" value="#hf_voteRange#" />
            <input type="hidden" id="hf_prMin" value="#hf_prMin#" />
            <input type="hidden" id="hf_prMax" value="#hf_prMax#" />
            <input type="hidden" id="hf_prRange" value="#hf_prRange#" />
            <input type="hidden" id="hf_prTemp" value="#hf_prRange#" />
            <input type="hidden" id="hf_currPage" value="#currPage#"/>
            <div class="listEmpty error"><span>#lblApartmentSearchError#</span></div>
        </asp:Literal>
        <asp:Literal ID="ltrItemTemplate" runat="server">
            <li>
                <a class="content_block" href="/#page_path#?bts=true">
                    <img width="162" height="89px" alt="#title#" class="fotosmall" src="/#img_preview_1#" />
                    <span class="testi">
                        <span class="tit">
                            <h2>#title#</h2>
                            #onlineBooking#
                            <span class="nulla"></span>
                            <span class="cat">#zoneTitle#</span>
                        </span>
                        <span class="voto">
                            <img src="/images/estate_vote/vote#vote#.gif" alt="#vote#" />#vote#/10</span>
                        <span class="nulla"></span>
                    </span>
                    <span class="dettaglio">
                        <span class="cont_foto">
                            <img width="162" height="89px" alt="#title#" class="foto" src="/#img_preview_1#"/>
                            <img width="162" height="89px" alt="#title#" class="foto" src="/#img_preview_2#"/>
                            <img width="162" height="89px" alt="#title#" class="foto" src="/#img_preview_3#" style="margin-right: 0;" />
                        </span>
                        <span class="tx_dett">
                            <p>#summary#</p>
                        </span>
                        <span class="dx_dett">
                            #price#
                            <span class="more search">#lblViewDetails#</span>
                        </span>
                        <span class="nulla"></span>
                    </span>
                </a>
            </li>
        </asp:Literal>
<asp:Literal ID="ltrLayoutTemplate_mobileXml" runat="server">
<list  voteMin="#hf_voteMin#" voteMax="#hf_voteMax#" voteRange="#hf_voteRange#" prMin="#hf_prMin#" prMax="#hf_prMax#" prRange="#hf_prRange#" currPage="#currPage#" pagesTotalCount="#pagesTotalCount#">
#itemPlaceHolder#
</list>
</asp:Literal>
<asp:Literal ID="ltrItemTemplate_mobileXml" runat="server">
 	<apt_item>
		<apt_id>#id#</apt_id>
		<apt_name>#title#</apt_name>
		<apt_vote>#vote#</apt_vote>
		<apt_img>http://www.rentalinrome.com/#img_preview_1#</apt_img>
		<zone_id>#zoneId#</zone_id>
		<zone_name>#zoneTitle#</zone_name>
		<pr_total>#price#</pr_total>
	</apt_item>
</asp:Literal>
        <asp:Literal ID="ltrLayoutTemplate_affiliatesarea" runat="server">
            <input type="hidden" id="hf_voteMin" value="#hf_voteMin#" />
            <input type="hidden" id="hf_voteMax" value="#hf_voteMax#" />
            <input type="hidden" id="hf_voteRange" value="#hf_voteRange#" />
            <input type="hidden" id="hf_voteTemp" value="#hf_voteRange#" />
            <input type="hidden" id="hf_prMin" value="#hf_prMin#" />
            <input type="hidden" id="hf_prMax" value="#hf_prMax#" />
            <input type="hidden" id="hf_prRange" value="#hf_prRange#" />
            <input type="hidden" id="hf_prTemp" value="#hf_prRange#" />
            <input type="hidden" id="hf_currPage" value="#currPage#"/>
            <div class="table_fascia tfagency">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 597px;">
                    <tr>
                        <th style="width:170px;">                    
                            <a href="javascript:RNT_orderBy('title')" class="#orderBy_title#" >
                            #lblName#
                            </a>
                        </th>
                        <th>Zone</th>
                        <th style="text-align:center;">                    
                            <a href="javascript:RNT_orderBy('vote')" class="#orderBy_vote#">
                            #lblRating#
                            </a>
                        </th>
                        <th style="text-align:center;">                    
                            <a href="javascript:RNT_orderBy('price')" class="#orderBy_price#">
                            #lblPrice#
                            </a>
                        </th>
                        <th style="text-align:center;">
                            <!-- #lblYourCommission# -->
                            <img src="/images/css/icothsoldi.jpg" class="ico_tooltip" alt="" ttp="#lblYourCommission#"/>
                        </th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                    #itemPlaceHolder#
                </table>
            </div>
            #pagerPlaceHolder#
            <div class="nulla">
            </div>
        </asp:Literal>
        <asp:Literal ID="ltrItemTemplate_affiliatesarea" runat="server">
            <tr class="#cssClass#" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                <td>
                    <a target="_blank" href="/#page_path#" style="margin-top: 6px; margin-right: 5px; font-weight:bold;">#title#</a>
                </td>
                <td>
                    <span style="font-size: 10px;">#zoneTitle#</span>
                </td>
                <td style="text-align:center;">
                    <span>#vote#/10</span>
                </td>
                <td style="text-align:center;">
                    <span>&euro;&nbsp;#price#</span>
                </td>
                <td style="text-align:center;">
                    <strong>&euro;&nbsp;#pr_agentCommissionPrice#</strong>
                </td>
                <td>
                    <a href="reservationComplete.aspx?#bookingLink#" class="abookit"><span>Book it!</span></a>
                </td>
                 <td>
                      <input type="text" id="txt_markup#pdflinkid#" value="0" />
                </td>
                 <td>                   
                    <a id="lnk_pdf#pdflinkid#" href="#" customlnk="/pdfgenerator/generator.aspx?url=#generatepdf#&filename=#fname#" target="_blank" class="abookit" onclick="setmkvalue(this.id)"><span>Pdf</span></a>
                </td>
            </tr>
        </asp:Literal>
        <asp:Literal ID="ltrPriceTemplate_ok" runat="server">
            #priceWithoutDiscount#
            <span class="euro" style="height: 23px;">
                #price#<span style="font-size: 24px;">€</span>
            </span>
        </asp:Literal>
        <asp:Literal ID="ltrPrezzoBarrato" runat="server">
            <span class="prezzobarrato">
                 <span>#lblDiscountSpecialOffer# &nbsp</span>  
                 <strong>#price#&euro;<em></em></strong>
            </span>
        </asp:Literal>
        <asp:Literal ID="ltrPriceTemplate_request" runat="server">
            <span class="euro richiesta">
                #lblOnRequest#
            </span>
        </asp:Literal>
        <asp:Literal ID="ltrPriceTemplate_error1" runat="server">
            <span class="euro richiesta">
                #lblOnRequest#
                <span style="font-weight: normal; font-size: 10px; line-height: 11px; display: block; margin-top: -3px;">Instant booking:<br/>#lblMinStayVHSeason#</span>
            </span>
        </asp:Literal>
        <asp:Literal ID="ltrPriceTemplate_agency" runat="server">
            <span class="euro" style="height: auto;">
                #price#<span style="font-size: 24px;">€</span>
                <span style="margin: 2px 0pt 0pt; padding-left: 30px;" class="agencyComm ico_tooltip" ttp="#lblYourCommission#">
                    <strong style="text-align:center;">#pr_agentCommissionPrice#&nbsp;€ </strong></span>
                <span class="nulla"></span>
            </span>
        </asp:Literal>
    </div>
    </form>
</body>
</html>
