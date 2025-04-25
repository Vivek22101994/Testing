<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_estateListZone.aspx.cs" Inherits="RentalInRome.webservice.rnt_estateListZone" %>

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
                    <a href="javascript:RNT_orderBy('title')" class="#orderBy_title#">
                        <span>#lblName#</span></a>
                    <a href="javascript:RNT_orderBy('price')" class="#orderBy_price#">
                        <span>#lblPrice#</span></a>
                    <a href="javascript:RNT_orderBy('vote')" class="#orderBy_vote#">
                        <span>#lblRating#</span></a>
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
                <a class="content_block" href="/#page_path#">
                    <img width="162" height="89px" alt="#title#" class="fotosmall" src="/#img_preview_1#" />
                    <span class="testi">
                        <span class="tit">
                            <h2>#title#</h2>
                            #onlineBooking#
                            #specialOfferIco#
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
                            #price#
                            <span class="more search">#lblViewDetails#</span>
                        </span>
                        <span class="nulla"></span>
                    </span>
                </a>
            </li>
        </asp:Literal>
    </div>
    </form>
</body>
</html>
