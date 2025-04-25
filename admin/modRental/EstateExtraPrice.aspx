<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master"
    AutoEventWireup="true" CodeBehind="EstateExtraPrice.aspx.cs" Inherits="RentalInRome.admin.modRental.EstateExtraPrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
    <script src="../js/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js" type="text/javascript"></script>
    <title>
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function numericvalidation(evt) {

            var key = (evt.which) ? evt.which : event.keyCode

            if ((key >= 48) && (key <= 58) || (key == 46) || (key == 44) || (key == 08) || (key == 127)) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function CalculatePrice() {

            var e = document.getElementById('<%= drp_commissionType.ClientID %>');
            var type = e.options[e.selectedIndex].value;
            var price = $('#<%= txt_costPrice.ClientID %>').val();
            var childPrice = $('#<%= txt_costPrice_child.ClientID %>').val();
            var commissionper = $('#<%= txt_commisssion.ClientID %>').val();
            price = price.replace(",", ".");
            childPrice = childPrice.replace(",", ".");
            commissionper = commissionper.replace(",", ".");

            var commisiion = 0;
            var costprice = 0;
            var childCostprice = 0;

            if (price != "" && commissionper != "") {
                if (type == "0") {
                    commission = (price * commissionper) / 100;
                    costprice = parseFloat(price) + parseFloat(commission);

                }
                else {
                    costprice =parseFloat(price) + parseFloat(commissionper);

                }
                costprice = costprice.toFixed(2);
                costprice = costprice.toString().replace(".", ",");
                $('#<%= txt_price.ClientID %>').val(costprice);

            }

            if (childPrice != "" && commissionper != "") {
                if (type == "0") {
                    commission = (childPrice * commissionper) / 100;
                    childCostprice = parseFloat(childPrice) + parseFloat(commission);

                }
                else {
                    childCostprice = parseFloat(childPrice) + parseFloat(commissionper);

                }
                childCostprice = childCostprice.toFixed(2);
                childCostprice = childCostprice.toString().replace(".", ",");
                $('#<%= txt_price_child.ClientID %>').val(childCostprice);




            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfId" Value="0" runat="server" />
        <asp:HiddenField ID="hfd_price" Value="0" runat="server" />
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <h1 class="titolo_main">
                <%= ltrTitle.Text%>
            </h1>
        </telerik:RadCodeBlock>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton></div>
            <div class="bottom_salva">
                <a onclick="return CloseRadWindow('reload');"><span>Chiudi</span></a>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
        <div class="mainline">
            <div class="mainbox" id="div_price" runat="server">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Prezzo:</span>
                    <div class="boxmodulo">
                        <table cellpadding="3" cellspacing="0">
                            <%--<tr>
                                <td class="td_title">
                                    Gruppo
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_price_group" runat="server">
                                        <asp:ListItem Text="Select Group" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="td_title">
                                    tipologie di prezzo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_priceType" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title" style="width: 120px">
                                    Min Pax:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_min_pax" runat="server">
                                        <asp:ListItem Text="Select Person" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title" style="width: 120px">
                                    Max Pax:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_max_pax" runat="server">
                                        <asp:ListItem Text="Select Person" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Hours:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_hours" runat="server">
                                        <asp:ListItem Text="--no--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Days:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_days" runat="server">
                                        <asp:ListItem Text="--no--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    tipi di pagamento:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_PaymentType" runat="server">
                                        <asp:ListItem Value="forfait">forfait</asp:ListItem>
                                        <asp:ListItem Value="persona">persona</asp:ListItem>
                                        <asp:ListItem Value="notte">notte</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                               <tr>
                                <td class="td_title">
                                    Prezzo di costo per bambino:
                                </td>
                                <td>
                                    <%--  <telerik:RadNumericTextBox ID="txt_costPrice" runat="server" Width="50">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2"/>
                                    </telerik:RadNumericTextBox>--%>
                                    <telerik:RadNumericTextBox ID="txt_costPrice_child" runat="server" Width="50" onchange="CalculatePrice();">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                   <%-- <asp:TextBox ID="txt_costPrice_child" Style="width: 120px" runat="server" onchange="CalculatePrice();"
                                        Width="50px" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Prezzo di costo per adulto:
                                </td>
                                <td>
                                    <%--  <telerik:RadNumericTextBox ID="txt_costPrice" runat="server" Width="50">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2"/>
                                    </telerik:RadNumericTextBox>--%>
                                     <telerik:RadNumericTextBox ID="txt_costPrice" runat="server" Width="50" onchange="CalculatePrice();">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                   <%-- <asp:TextBox ID="txt_costPrice" Style="width: 120px" runat="server" Enabled="false"
                                        Width="50px" />--%>
                                </td>
                            </tr>
                          
                            <tr>
                                <td class="td_title">
                                    Commissione Tipo:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_commissionType" runat="server" onchange="CalculatePrice();">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Commissione:
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="txt_commisssion" Style="width: 120px" runat="server" onkeypress="return numericvalidation(event);"
                                        onchange="CalculatePrice();" />--%>
                                    <telerik:RadNumericTextBox ID="txt_commisssion" runat="server" Width="50" onchange="CalculatePrice();">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                         
                              <tr>
                                <td class="td_title">
                                    Prezzo Vendita per bambino:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pid_Child" runat="server" Visible="false" Text="0" />
                                 <%--   <telerik:RadNumericTextBox ID="txt_price_child" runat="server" Width="50" Enabled="false" onchange="CalculatePrice();">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>--%>
                                       <asp:TextBox ID="txt_price_child" Style="width: 120px" runat="server" Enabled="false"
                                        Width="50px" />

                                    <%-- <asp:TextBox ID="txt_price" Style="width: 120px" runat="server" onkeypress="return numericvalidation(event);"
                                        onchange="CalculatePrice();" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    Prezzo Vendita per adulto:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pid" runat="server" Visible="false" Text="0" />
                                   <%-- <telerik:RadNumericTextBox ID="txt_price" runat="server" Width="50" Enabled="false">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                    <asp:TextBox ID="TextBox1" Style="width: 120px" runat="server" Enabled="false"
                                        Width="50px" />--%>
                                      <asp:TextBox ID="txt_price" Style="width: 120px" runat="server" Enabled="false"
                                        Width="50px" />
                                </td>
                            </tr>
                            
                            <%-- <tr>
                                <td class="td_title">
                                    Prezzo:
                                </td>
                                <td>
                                <asp:TextBox ID="txt_pid" runat="server" Visible="false" Text="0"/>
                                    <telerik:RadNumericTextBox ID="txt_price" runat="server" Width="50" Enabled="false" Value="0,00">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2"/>
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td class="td_title">
                                   Commissione:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_commission" Style="width: 120px" runat="server" onkeypress="return numericvalidation(event);"/>
                                    
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                    <asp:ListView ID="LVPrice" runat="server" OnItemDataBound="LvPrice_ItemDataBound"
                        OnItemCommand="LvPrice_ItemCommand">
                        <ItemTemplate>
                            <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_group"></asp:Label>
                                </td>--%>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_min_pax" Text='<%#Eval("minPax") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_max_pax" Text='<%#Eval("maxPax") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_hour" Text='<%#Eval("Hours") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_days" Text='<%#Eval("Days") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_priceType" Text='<%#Eval("priceType") %>'></asp:Label>
                                </td>
                              
                                  <td align="center">
                                    <asp:Label runat="server" ID="lbl_child_costPrice" Text='<%#Eval("costPriceChild") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_costPrice" Text='<%#Eval("costPrice") %>'></asp:Label>
                                </td>
                               
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_commissionType"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_commisssion" Text='<%#Eval("Commission") %>'></asp:Label>
                                </td>
                                  <td align="center">
                                    <asp:Label runat="server" ID="lbl_child_price" Text='<%#Eval("actualPriceChild") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                    <asp:Label runat="server" ID="lbl_price" Text='<%#Eval("actualPrice") %>'></asp:Label>
                                </td>
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_commission" Text='<%#Eval("Commission") %>'></asp:Label>
                                 </td>--%>
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditPrice" Text="Scheda"
                                        title="Apri Scheda" Style="text-decoration: none; border: 0 none; margin: 5px;
                                        color: #000000;"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeletePrice" Text="Elimina"
                                        OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_group"></asp:Label>
                                </td>--%>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_min_pax" Text='<%#Eval("minPax") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_max_pax" Text='<%#Eval("maxPax") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_hour" Text='<%#Eval("Hours") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_days" Text='<%#Eval("Days") %>'></asp:Label>
                                </td>
                                 <td align="center">
                                    <asp:Label runat="server" ID="lbl_priceType" Text='<%#Eval("priceType") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_child_costPrice" Text='<%#Eval("costPriceChild") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_costPrice" Text='<%#Eval("costPrice") %>'></asp:Label>
                                </td>
                               
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_commissionType"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_commisssion" Text='<%#Eval("Commission") %>'></asp:Label>
                                </td>
                                
                                <td align="center">
                                    <asp:Label runat="server" ID="lbl_child_price" Text='<%#Eval("actualPriceChild") %>'></asp:Label>
                                </td>
                               
                                 <td align="center">
                                    <asp:Label ID="lbl_id" runat="server" Visible="false" Text='<%# Eval("id") %>' Style="display: none;" />
                                    <asp:Label runat="server" ID="lbl_price" Text='<%#Eval("actualPrice") %>'></asp:Label>
                                </td>
                                <%--<td align="center">
                                    <asp:Label runat="server" ID="lbl_commission" Text='<%#Eval("Commission") %>'></asp:Label>
                                 </td>--%>
                                <td align="center">
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditPrice" title="Apri Scheda"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;">Scheda</asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeletePrice" OnClientClick="return confirm('Stai per eliminare senza la possibilità di ripristinare?');"
                                        Style="text-decoration: none; border: 0 none; margin: 5px; color: #000000;">Elimina</asp:LinkButton>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <div style="margin-top: 30px;">
                                <table border="0" cellpadding="0" cellspacing="0" style="">
                                    <tr style="">
                                        <%-- <th style="width: 70px;border-bottom: 1px solid #DFDFDF;" align="left">
                                        Gruppo
                                    </th>--%>
                                        <th style="width: 90px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Min Pax
                                        </th>
                                        <th style="width: 90px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Max Pax
                                        </th>
                                        <th style="width: 90px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Hours
                                        </th>
                                        <th style="width: 90px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Days
                                        </th>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Tariffazione
                                        </th>
                                        <th style="width: 150px; border-bottom: 1px solid #DFDFDF;" align="left">
                                          Prezzo di Costo per bambino 
                                        </th>
                                        <th style="width: 150px; border-bottom: 1px solid #DFDFDF;" align="left">
                                          Prezzo di Costo per adulto 
                                        </th>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Commissione Tipo
                                        </th>
                                        <th style="width: 130px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Commissione
                                        </th>
                                        <th style="width: 150px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Prezzo Vendita per bambino 
                                        </th>
                                        <th style="width: 150px; border-bottom: 1px solid #DFDFDF;" align="left">
                                            Prezzo Vendita per adulto 
                                        </th>
                                        <%-- <th style="width: 130px;border-bottom: 1px solid #DFDFDF;" align="left">
                                       Commissione
                                    </th>--%>
                                        <th style="border-bottom: 1px solid #DFDFDF;">
                                        </th>
                                        <th style="border-bottom: 1px solid #DFDFDF;">
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <asp:HiddenField ID="hfd_cat" runat="server" />
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
