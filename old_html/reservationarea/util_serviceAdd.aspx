<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="util_serviceAdd.aspx.cs"
    Inherits="RentalInRome.reservationarea.util_serviceAdd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/css/style.css" />    
    <%-- <link rel="stylesheet" type="text/css" href="/css/style.css" />--%>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%if (App.WLAgentId > 0)
          {%>
        <link rel="stylesheet" type="text/css" href="/WLRental/css/<%= WL.getWLCSS() %>.css<%="?tmp="+DateTime.Now.Ticks %>" />
        <%}%>

        <script src="<%= App.RP %>jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            function toggleCheckBoxes(source) {
                var listView = document.getElementById('table1');
                var inputs = listView.getElementsByTagName('input');
                if (source.checked) {
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[j].type == "radio") {
                            inputs[j].checked = false;

                        }
                    }

                    source.checked = true;
                    var price = document.getElementById(source.id.replace("rdbCheck", "lvlPrice")).innerHTML;
                    var childprice = document.getElementById(source.id.replace("rdbCheck", "lvlChildPrice")).innerHTML;
                    var pricetype = document.getElementById(source.id.replace("rdbCheck", "lblPriceType")).innerHTML;
                    var pricetypeid = document.getElementById(source.id.replace("rdbCheck", "HFCurrPriceTypeId")).value;
                    var paymenttype = document.getElementById(source.id.replace("rdbCheck", "HFCurrPaymentType")).value;
                    var maxperson = document.getElementById(source.id.replace("rdbCheck", "HFCurrMAxPerson")).value;
                    var costprice = document.getElementById(source.id.replace("rdbCheck", "HFCurrCostPrice")).value;
                    var childcostprice = document.getElementById(source.id.replace("rdbCheck", "HFCurrChildCostPrice")).value; 

                    var res_adult = document.getElementById('HFRes_adult').value;
                    var res_child = document.getElementById('HFRes_child').value;
                    var paymenttype = document.getElementById('HFPaymentType').value;
                   
                    document.getElementById('HFPrice').value = price;
                    document.getElementById('HFPriceType').value = pricetype;
                    document.getElementById('HFPriceTypeId').value = pricetypeid;
                    document.getElementById('HFPaymentType').value = paymenttype;
                    document.getElementById('HFMAxPerson').value = maxperson;
                    document.getElementById('HFChildPrice').value = childprice;
                    document.getElementById('HFCostPrice').value = costprice;
                    document.getElementById('HFChildCostPrice').value = childcostprice;

                    if (parseInt(maxperson) < parseInt(res_adult) + parseInt(res_child)) {


                        if (parseInt(res_adult) >= maxperson) {

                            document.getElementById('drp_adult').value = maxperson;
                            document.getElementById('drp_child_over').value = 0;


                        }
                        else {
                            var child = maxperson - res_adult;
                            document.getElementById('drp_adult').value = res_adult;
                            document.getElementById('drp_child_over').value = child;


                        }
                    }
                    else {
                        document.getElementById('drp_adult').value = res_adult;
                        document.getElementById('drp_child_over').value = res_child;
                    }

                    document.getElementById('HFOld_adult').value = document.getElementById('drp_adult').value;
                    //alert(document.getElementById('drp_child_over').value);
                    document.getElementById('HFOld_child').value = document.getElementById('drp_child_over').value;
                    //alert(document.getElementById('HFOld_child').value);
                    calculatePrice();

                }

            }
        </script>
        <script type="text/javascript">
            function calculatePrice() {

                var drp_adult = document.getElementById('drp_adult');
                var adult = drp_adult.options[drp_adult.selectedIndex].value;
                var drp_child_over = document.getElementById('drp_child_over');
                var children = drp_child_over.options[drp_child_over.selectedIndex].value;
                var price = document.getElementById('HFPrice').value;
                var childprice = document.getElementById('HFChildPrice').value;
                var paymenttype = document.getElementById('HFPaymentType').value;
                var maxpax = document.getElementById('HFMAxPerson').value;
                var oldadult = document.getElementById('HFOld_adult').value;
                var oldchild = document.getElementById('HFOld_child').value;
                var anticipo = document.getElementById('HFAnticipo').value;
                var costprice = document.getElementById('HFCostPrice').value;
                var childcostprice = document.getElementById('HFChildCostPrice').value;
                var message = document.getElementById('HF_messgae').value; 
                
                var totalprice = 0;
                var totalcostprice = 0;

                price = price.replace(",", ".");
                costprice = costprice.replace(",", ".");
                childprice = childprice.replace(",", ".");
                childcostprice = childcostprice.replace(",", ".");
                if (maxpax != "") {
                    if (maxpax >= parseInt(adult) + parseInt(children)) {
                        if (paymenttype == "forfait") {

                            totalprice = price;
                            totalcostprice = costprice;
                            if (children != 0 && childprice != "") {
                                totalprice = (parseFloat(totalprice) + parseFloat(childprice)).toFixed(2);
                                totalcostprice = (parseFloat(totalcostprice) + parseFloat(childcostprice)).toFixed(2);

                            }
                            else {
                                totalprice = parseFloat(totalprice).toFixed(2);
                                totalcostprice = parseFloat(totalcostprice).toFixed(2);
                            }


                        }
                        else {

                            totalprice = price * adult;
                            totalcostprice = costprice * adult;

                            if (children != 0 && childprice != "") {

                                totalprice = (totalprice + childprice * children).toFixed(2);
                                totalcostprice = (totalcostprice + childcostprice * children).toFixed(2);


                            }
                            else {
                                totalprice = totalprice.toFixed(2);
                                totalcostprice = totalcostprice.toFixed(2);
                            }

                        }
                        totalprice = totalprice.toString().replace(".", ",");
                        document.getElementById('lblTotalPrice').innerHTML = totalprice;
                        document.getElementById('HFTotalPrice').value = totalprice;
                        document.getElementById('HFOld_adult').value = adult;
                        document.getElementById('HFOld_child').value = children;
                        if (anticipo == 1) {
                            document.getElementById('lbl_commission').innerHTML = (parseFloat(totalprice) - parseFloat(totalcostprice)).toFixed(2).toString().replace(".", ",");
                            document.getElementById('HFCommission').value = (parseFloat(totalprice) - parseFloat(totalcostprice)).toFixed(2).toString().replace(".", ",");
                        }

                    }
                    else {
                       
                        alert(message);
                        document.getElementById('drp_adult').value = oldadult;
                        document.getElementById('drp_child_over').value = oldchild;


                    }
                    //totalprice = totalprice.toFixed(2);

                }

            }
        </script>
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function CloseRadWindow(arg) {
                rw = GetRadWindow();
                if (rw) {
                    rw.close(arg);
                    var parentwindow = window.parent;

                    parentwindow.parent.location.href = parentwindow.parent.location.href;

                }
                else
                    window.location = "/";
                return false;
            }
            function CloseRadWindowRequest(arg) {
                rw = GetRadWindow();
                if (rw) {

                    function callbackFn(arg)//the value indicates how the dialog was closed
                    {
                        rw.close(arg);
                    }
                    var successmsg = document.getElementById('<%=hfd_success.ClientID%>').value;                  
                    radalert(successmsg, 340, 110, 'Client RadAlert', callbackFn);
   
                }
                else
                    window.location = "/";
                return false;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div class="pop_up">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <asp:UpdateProgress ID="loading_cont" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <%="<div id=\"loading_big_cont\"></div><div id=\"overlay_site\"></div>" %>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" CssClass="num_app_carrello">
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <asp:HiddenField ID="HFcloseArgs" runat="server" />
                <asp:HiddenField ID="HFserviceId" runat="server" />
                <asp:HiddenField ID="HFresId" runat="server" />
                <asp:HiddenField ID="HFmultiple" runat="server" />
                <asp:HiddenField ID="HFPrice" runat="server" />
                <asp:HiddenField ID="HFCostPrice" runat="server" />
                <asp:HiddenField ID="HFChildCostPrice" runat="server" />
                 <asp:HiddenField ID="HFAnticipo" runat="server" Value="0" />
                <asp:HiddenField ID="HFChildPrice" runat="server" />
                <asp:HiddenField ID="HFPriceType" runat="server" />
                <asp:HiddenField ID="HFPriceTypeId" runat="server" />
                <asp:HiddenField ID="HFPaymentType" runat="server" />
                <asp:HiddenField ID="HFMAxPerson" runat="server" />
                <asp:HiddenField ID="HFTotalPrice" runat="server" />
                <asp:HiddenField ID="HFCommission" runat="server" />
                <asp:HiddenField ID="HFLang" runat="server" />
                <asp:HiddenField ID="HFRes_adult" runat="server" />
                <asp:HiddenField ID="HFRes_child" runat="server" />
                <asp:HiddenField ID="HFOld_adult" runat="server" />
                <asp:HiddenField ID="HFOld_child" runat="server" />
                <asp:HiddenField ID="HF_messgae" runat="server" />
                 <asp:HiddenField ID="hfd_success" runat="server" />
                
                <div id="prices">
                    <h3 id="priceBar">
                        <asp:Label ID="lblPopupServiceName" runat="server"></asp:Label></h3>
                    <div>
                        <div id="dvSinglePrice" runat="server">
                            <strong class="titdvSinglePrice">
                                <%= CurrentSource.getSysLangValue("lblPriceType")%>:</strong>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPriceType" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblAdultPrice")%>:
                                        <asp:Label ID="lblPrice" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <%= CurrentSource.getSysLangValue("lblChildPrice")%>:
                                        <asp:Label ID="lblChildPrice" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                       <%= CurrentSource.getSysLangValue("lbl_minPerson")%>:
                                        <asp:Label ID="lbl_minPax" runat="server"></asp:Label>
                                    </td>
                                    <td style="width:250px;">
                                        <%= CurrentSource.getSysLangValue("lbl_maxPersons")%>:
                                        <asp:Label ID="lbl_maxPax" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="dvmultiplePrice" runat="server">
                            <h1 class="titdvSinglePrice">
                                <%= CurrentSource.getSysLangValue("lblChoosePriceType")%>:
                            </h1>
                            <table style="margin-bottom: 15px;" width="20%" cellpadding="0" cellspacing="2" id="table1">
                                <asp:ListView ID="lvPriceType" runat="server" OnItemDataBound="lvPriceType_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td valign="middle" align="center">
                                                <asp:RadioButton ID="rdbCheck" runat="server" onclick="toggleCheckBoxes(this);" />
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:HiddenField ID="HFCurrPriceTypeId" runat="server" Value='<%# Eval("priceType") %>' />
                                                <asp:HiddenField ID="HFCurrPaymentType" runat="server" Value='<%# Eval("paymentType") %>' />
                                                <asp:HiddenField ID="HFCurrMAxPerson" runat="server" Value='<%# Eval("maxPax") %>' />
                                                <asp:HiddenField ID="HFCurrCostPrice" runat="server" Value='<%# Eval("costPrice") %>' />
                                                <asp:HiddenField ID="HFCurrChildCostPrice" runat="server" Value='<%# Eval("costPriceChild") %>' />
                                                <asp:Label ID="lblPriceType" runat="server"></asp:Label>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Label ID="lvlPrice" runat="server" Text='<%# Eval("actualPrice") %>'></asp:Label>
                                               

                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Label ID="lvlChildPrice" runat="server" Text='<%# Eval("actualPriceChild") %>'></asp:Label>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Label ID="lbl_minPax" runat="server" Text='<%# Eval("minPax") %>'></asp:Label>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:Label ID="lbl_maxPax" runat="server" Text='<%# Eval("maxPax") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <LayoutTemplate>
                                        <tr>
                                            <th valign="middle" align="center">
                                            </th>
                                            <th valign="middle" align="left">
                                                <telerik:RadCodeBlock runat="server">
                                                     <%= CurrentSource.getSysLangValue("lblPriceType")%></telerik:RadCodeBlock>
                                            </th>
                                            <th valign="middle" align="center">
                                                <telerik:RadCodeBlock runat="server">
                                                    <%= CurrentSource.getSysLangValue("lblAdultPrice")%></telerik:RadCodeBlock>
                                            </th>
                                            <th valign="middle" align="center">
                                                <telerik:RadCodeBlock runat="server">
                                                    <%= CurrentSource.getSysLangValue("lblChildPrice")%></telerik:RadCodeBlock>
                                            </th>
                                            <th valign="middle" align="center">
                                                <telerik:RadCodeBlock runat="server">
                                                    <%= CurrentSource.getSysLangValue("lbl_minPerson")%></telerik:RadCodeBlock>
                                            </th>
                                            <th valign="middle" align="center">
                                                <telerik:RadCodeBlock runat="server">
                                                    <%= CurrentSource.getSysLangValue("lbl_maxPersons")%></telerik:RadCodeBlock>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </LayoutTemplate>
                                </asp:ListView>
                            </table>
                        </div>
                    </div>
                    <div class="boxStep" style="margin-left: 20px;">
                        <span class="numStep"><strong>1. </strong>
                            <%= CurrentSource.getSysLangValue("lblChooseDate")%></span>
                        <table class="selPeriod" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_date" runat="server">
                                    </asp:DropDownList>
                                    <%-- <input id="txt_dtStart" type="text" readonly="readonly" runat="server" />
                                        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />--%>
                                    <%-- </td>
                                    <td>
                                        <a class="calendario" id="startCalTrigger"></a>--%>
                                </td>
                            </tr>
                        </table>
                        <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
                    </div>
                    <div class="boxStep" style="width: 260px;" id="pnlGuestInfo" runat="server">
                        <span class="numStep"><strong>2. </strong>
                            <%= CurrentSource.getSysLangValue("lblGuestInformation")%></span>
                        <table class="guestInfo" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" align="left" style="width: 50px;">
                                    <asp:DropDownList ID="drp_adult" runat="server" onchange="calculatePrice();">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("reqAdults")%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_over" runat="server" onchange="calculatePrice();">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:DropDownList ID="drp_child_min" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_closingDays">
                        <asp:ListView ID="LV_closingDays" runat="server" OnItemDataBound="LV_closingDays_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_dtStart" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_dtEnd" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_closingDays" runat="server" Text='<%# Eval("closingday") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                            <table>
                                <tr>
                                    <th style="width: 70px;">
                                        Start Date
                                    </th>
                                    <th style="width: 70px;">
                                        End Date
                                    </th>
                                    <th style="width: 120px;">
                                        Closing Days
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                    <asp:Label ID="lbl_anticipo_msg" runat="server" Visible="false"><%= CurrentSource.getSysLangValue("lbl_anticipo")%></asp:Label>
                    <div id="yourBookingResAr" runat="server" visible="true">
                        <h3 class="titBar">
                            <%= CurrentSource.getSysLangValue("lblYourBooking")%>
                        </h3>
                        <div class="boxStep" style="width: 300px;">
                            <table id="infoBookPrice" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" align="left" class="col1">
                                        <%= CurrentSource.getSysLangValue("reqCheckInDate")%>
                                    </td>
                                    <td valign="middle" align="left" class="col2">
                                        <span id="lb_sel_dtStart">
                                            <asp:Label ID="lblCheckinDate" runat="server"></asp:Label></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left" class="col1">
                                        <%= CurrentSource.getSysLangValue("reqCheckOutDate")%>
                                    </td>
                                    <td valign="middle" align="left" class="col2">
                                        <span id="lb_sel_dtEnd">
                                            <asp:Label ID="lblCheckOut" runat="server"></asp:Label></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left" class="col1">
                                        <%= CurrentSource.getSysLangValue("lblTotalNights")%>
                                    </td>
                                    <td valign="middle" align="left" class="col2">
                                        <span id="lb_sel_dtCount">
                                            <asp:Label ID="lblTotalnights" runat="server"></asp:Label></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" align="left" class="col1">
                                        <%= CurrentSource.getSysLangValue("lblPax")%>
                                    </td>
                                    <td valign="middle" align="left" class="col2">
                                        <span id="lb_sel_personsCount">
                                            <asp:Label ID="lblAdult" runat="server"></asp:Label><%= CurrentSource.getSysLangValue("reqAdults")%>
                                            <asp:Label ID="lblChildren" runat="server"></asp:Label>
                                            <%= CurrentSource.getSysLangValue("lblChildren3OrOver")%>
                                            <asp:Label ID="lblinfants" runat="server"></asp:Label>
                                            <%= CurrentSource.getSysLangValue("lblChildrenUnder3")%></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_request" runat="server" style="width: 550px; float: left; display: none;">
                            Note:<br />
                            <br />
                            <asp:TextBox ID="txt_note" runat="server" TextMode="MultiLine" Style="width:404px; float:left; min-height:90px;"></asp:TextBox>
                            <asp:LinkButton ID="lnk_request_save" runat="server" OnClick="lnk_request_save_Click" CssClass="btnCalcola" Style="float: right; margin: 36px 10px 0 0;"><span><%=CurrentSource.getSysLangValue("lblRequestAdd")%></span></asp:LinkButton>
                        </div>
                        <div id="totale" runat="server">
                            <asp:PlaceHolder ID="PH_bookPriceOK" runat="server">
                                <div class="totalebox">
                                    <div>
                                        <span class="priceDettExSe1">
                                            <div class="flo_left89">
                                            <strong>
                                            <%= CurrentSource.getSysLangValue("lblTotalPrice")%>:</strong>
                                            <asp:Label ID="lblTotalPrice" runat="server"></asp:Label></div>
                                            <div class="cr"></div>
                                            <div class="flo_left89">
                                           <strong><asp:Label ID="lbl_commission_text" runat="server"> <%= CurrentSource.getSysLangValue("lbl_payNow")%>:</asp:Label></strong>
                                           <asp:Label ID="lbl_commission" runat="server"></asp:Label></div>
                                             <div class="cr"></div>
                                            <asp:LinkButton ID="lnk_Save" runat="server" OnClick="lnkSave_Click">
                                                <span><asp:Image ImageUrl="../images/css/focusnv_icoaddshop1_res_ar.gif" runat="server"></asp:Image></span>
                                                <strong><%=CurrentSource.getSysLangValue("lblAdd")%></strong>
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            
                        </div>
                    </div>
                </div>
            </telerik:RadCodeBlock>
        </telerik:RadAjaxPanel>
    </div>
    </form>
</body>
</html>
