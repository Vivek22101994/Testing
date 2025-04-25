<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rntEstateOffersList.aspx.cs" Inherits="RentalInRome.webservice.rntEstateOffersList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            #flt_location# #flt_act_pid_type# #fltFormatPriceRange# #fltFormatInnerAreaRange# #fltFormatEstatesOfTotal#
        </div>
        <!-- fine line1 DescRicerca-->
        <!-- line1 DescRicerca -->
        <div class="line2">
            <div class="utility">
                <div class="tipolista">
                    <a href="javascript:list_type_change('1');" class="ico1"></a>
                    <a href="javascript:list_type_change('2');" class="ico2"></a>
                    <a href="javascript:list_type_change('3');" class="ico3" style="display: none;"></a>
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
            <div class="ordina" style="width: 461px;">
                <span class="ord_sx">#lblOrderBy#</span>
                <div class="ord_dx">
                    <a href="javascript:RNT_orderBy('price')" class="#orderBy_price#">
                        <span>#lblPrice#</span></a>
                    <a href="javascript:RNT_orderBy('dtStart')" class="#orderBy_dtStart#">
                        <span>#lbldtStart#</span></a>
                    <a href="javascript:RNT_orderBy('dtEnd')" class="#orderBy_dtEnd#">
                        <span>#lbldtEnd#</span></a>
                </div>
            </div>
            <div class="ico">
                <a class="switch_thumb" href="#"></a>
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
            <div class="listEmpty error"><span>#lblApartmentSearchError#</span></div>
        </asp:Literal>
        <asp:Literal ID="ltrItemTemplate" runat="server">
            <li>
                <a class="content_block specOff" href="/#page_path##specOffert">
                    <img width="162" height="89px" alt="#title#" class="fotosmall" src="/#img_preview_1#" />
                    <span class="testi">
                        <span class="tit">
                            <h2>#title#</h2>
                            #onlineBooking#
                            <span class="nulla"></span>
                            <span class="cat">#zoneTitle#</span>
                        </span>
                        <span class="voto">
                            <img src="/images/estate_vote/vote#vote#.gif" alt="#vote#">#vote#/10</span>
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
                            
				            <span class="dateSpecOff">
                                #dateSpecOff#
				            </span>
				            #price#
                        </span>
                        <span class="nulla"></span>
                    </span>
                </a>
            </li>
        </asp:Literal>
        <asp:Literal ID="ltrLayoutTemplate_newsletter" runat="server">
            <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
            <html xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
            </head>
            <body style="margin:0; font-family:Arial, Helvetica, sans-serif; background:#333366;">
            <div style="width:100%; display:block; clear:both; font-size:11px; color:#666666; padding:10px 0; text-align:center; color:#FFF;"><a href="#lnkPagePath#" style=" color:#FFF;">#lblLinkViewNewsletter#</a></div>
            <div style="width:700px; margin:0 auto; clear:both; padding:10px; background:#FFF;">
                <img src="http://www.rentalinrome.com/images/css/logo.gif" alt="" style="float:left;" />
                <img src="http://www.rentalinrome.com/images/banner/app_top_#langCode#.png" alt="" style="float:right; width:468px; margin-right:-35px;" />
                #zonePlaceHolder#
            </div>
            <div style="width:100%; display:block; clear:both; font-size:11px; color:#666666; padding:10px 0; text-align:center; color:#FFF;">
             <strong>Rental in Rome S.r.l. - P. IVA: 07824541002</strong>
            <br/>
            <strong style="color: #FFF">
	            Administrative Office
            </strong>
            <br/>
            Via Appia Nuova, 677
            <br/>
            00179 Roma
            <br/><br/>
           #lblLinkToRemoveFromNewsletter#
            </div>
            </body>
            </html>
        </asp:Literal>
        
        <asp:Literal ID="ltrZoneTemplate_newsletter" runat="server">
            <div style=" width:700px; float:left; clear:both; margin:20px 0 0 0;">
                <span style="width:680px; padding:5px 10px; background:#E4501F; color:#FFF; text-transform:uppercase; display:block; float:left; clear:both; font-size:15px; margin-bottom:10px; font-weight:bold; border-radius:5px; -moz-border-radius:5px; -webkit-border-radius:5px;">#title#</span>
                <table>
                    #itemPlaceHolder#
                </table>
            </div>
            <div style="height:0;clear:both;">
            </div>
        </asp:Literal>
        <asp:Literal ID="ltrItemTemplate_newsletter" runat="server">
            <tr>
                <td>
                    <a style="display:block; float:left; clear:both; width:680px; text-decoration:none; font-size:12px; line-height:16px; margin-top: 15px;" href="http://www.rentalinrome.com/#page_path#">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <h2 style="margin:0; padding:0; display:block; float:left; max-width:600px; font-size:14px; color:#E4501F;">#title#</h2>
                                </td>
                                <td>
                                    #onlineBooking#
                                </td>
                            </tr>
                            <tr>
                                <td>
				                    <span style="float:left; display:block; width:400px;">
                                        #dateSpecOff#
				                    </span>
                                </td>
                                <td>
				                    <span style="font-weight: bold; color: rgb(51, 51, 102); float: right; padding: 7px 10px 3px; font-size: 31px;">
                                        #price#
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </a>
                </td>
            </tr>
        </asp:Literal>
        <asp:Literal ID="ltr_onlineBookingOK" runat="server"><span style="margin-left: 30px; font-weight: bold;"><span style="color: #333366;">Instant</span>&nbsp;<span style="color: #F76332;">Booking</span></span></asp:Literal>
        <asp:Literal ID="ltr_onlineBookingNO" runat="server"><span style="margin-left: 30px; color: #F76332; font-weight: bold;">#onlineBooking#</span></asp:Literal>
        <asp:Literal ID="Literal1" runat="server">
            <div style=" width:700px; float:left; clear:both; margin:20px 0 0 0;">
                <span style="width:680px; padding:5px 10px; background:#E4501F; color:#FFF; text-transform:uppercase; display:block; float:left; clear:both; font-size:15px; margin-bottom:10px; font-weight:bold; border-radius:5px; -moz-border-radius:5px; -webkit-border-radius:5px;">#title#</span>
                <ul style="margin:0; padding:0; float:left; clear:both; list-style:none; width:980px; margin:0 0 0 10px;">
                    #itemPlaceHolder#
                </ul>
            </div>
            <div style="height:0;clear:both;">
            </div>
        </asp:Literal>
        <asp:Literal ID="Literal2" runat="server">
            <li style="margin:0; padding:0; float:left; clear:both; width:680px; display:block; margin-bottom:10px; padding-bottom:10px; border-bottom:1px dashed #CCC;">
                <a style="display:block; float:left; clear:both; width:680px; text-decoration:none; font-size:12px; line-height:16px;" href="http://www.rentalinrome.com/#page_path#">
                    <span style="display:block; clear:both; float:left; width:100%;">
                        <h2 style="margin:0; padding:0; display:block; float:left; max-width:600px; font-size:14px; color:#E4501F;">#title#</h2>
                        #onlineBooking#
                    </span>
                    <span style="display:block; clear:both; float:left; width:100%;">
				        <span style="float:left; display:block; width:400px;">
                            #dateSpecOff#
				        </span>
				        <span style="font-weight: bold; color: rgb(51, 51, 102); float: right; padding: 7px 10px 3px; font-size: 31px;">
                        #price#
                        </span>
                    </span>
                </a>
            </li>
        </asp:Literal>
    </div>
    </form>
</body>
</html>
