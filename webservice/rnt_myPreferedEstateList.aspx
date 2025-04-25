<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_myPreferedEstateList.aspx.cs" Inherits="RentalInRome.webservice.rnt_myPreferedEstateList" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="ltr_itemTemplate" runat="server">
        <div class="item">
            <!-- Set width to 4 columns for grid view mode only -->
            <div class="image">
                <div id="listItemGallery_4" class="owl-carousel owl-theme listItemGallery">
                    #imageList#
                    <%--<div class="item">
                        <img src="http://www.rentalinrome.com/romeapartmentsphoto/roma-vatican-milizie-apartment_2137/gallery/thumb/Roma_Vatican_Milizie_Apartment_-_Rental_in_Rome_-1_33855.jpg" alt="" />
                    </div>
                    <div class="item">
                        <img src="http://www.rentalinrome.com/romeapartmentsphoto/la-casa-di-rosa_2141/gallery/thumb/La_Casa_di_Rosa_-_Rental_in_Rome_-3_37758.jpg" alt="">
                    </div>
                    <div class="item">
                        <img src="http://www.rentalinrome.com/romeapartmentsphoto/vatican-terrace-apartment_2092/gallery/thumb/Vatican_Terrace_Apartment_-_Rental_in_Rome-19_29905.jpg" alt="">
                    </div>--%>
                </div>

            </div>
            <a class="price" href="#" onclick="javascript:req_deleteFromMyList(#id#);">
                <i class="fa fa-times"></i>#lblRemove#
            </a>
            <div class="info">
                <h3>
                    <a href="#page_path#" target="_blank"> #title#</a>
                    <small>#zoneTitle#</small>
                </h3>
                <img src="/images/vote#rating#.gif" alt="rating: #rating#" />
                <p> #summary#</p>
            </div>
        </div>
        </asp:Literal>

        <asp:Literal ID="ltr_ImageTemplate" runat="server">
             <div class="item">
                        <img src="/#src#" alt="" />
                    </div>
        </asp:Literal>
    </form>
</body>
</html>
