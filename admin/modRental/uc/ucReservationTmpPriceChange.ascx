<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReservationTmpPriceChange.ascx.cs" Inherits="ModRental.admin.modRental.uc.ucReservationTmpPriceChange" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    function calculateTotals(sender, eventArgs) {
        setTimeout(function () { calculateTotalsAction(sender, eventArgs) }, 0);
    }
    var set_valueCalled = false;
    function calculateTotalsAction(sender, eventArgs) {
        if (!set_valueCalled) {
            set_valueCalled = true;

            ntxt_prTotalRate = $find("<%= ntxt_prTotalRate.ClientID %>").get_value();
            ntxt_pr_discount_commission = 0;
            ntxt_pr_discount_owner = 0;
            <% if(pnlDiscount.Visible){%>
            ntxt_pr_discount_commission = $find("<%= ntxt_pr_discount_commission.ClientID %>").get_value();
            ntxt_pr_discount_owner = $find("<%= ntxt_pr_discount_owner.ClientID %>").get_value();
            <% }%>
            ntxt_prDiscountSpecialOffer = $find("<%= ntxt_prDiscountSpecialOffer.ClientID %>").get_value();
            ntxt_prDiscountLastMinute = $find("<%= ntxt_prDiscountLastMinute.ClientID %>").get_value();
            ntxt_prDiscountLongStay = $find("<%= ntxt_prDiscountLongStay.ClientID %>").get_value();
            ntxt_prDiscountLongRange = $find("<%= ntxt_prDiscountLongRange.ClientID %>").get_value();
            ntxt_pr_part_agency_fee = $find("<%= ntxt_pr_part_agency_fee.ClientID %>").get_value();
            ntxt_pr_ecoPrice = $find("<%= ntxt_pr_ecoPrice.ClientID %>").get_value();
            ntxt_pr_srsPrice = $find("<%= ntxt_pr_srsPrice.ClientID %>").get_value();
            ntxt_agentCommissionPrice = $find("<%= ntxt_agentCommissionPrice.ClientID %>").get_value();
            drp_agentDiscountNotPayed = $("#<%= drp_agentDiscountNotPayed.ClientID %>").val();
            ntxt_pr_total = ntxt_prTotalRate + ntxt_pr_part_agency_fee - ntxt_prDiscountSpecialOffer - ntxt_prDiscountLastMinute - ntxt_prDiscountLongStay - ntxt_prDiscountLongRange - ntxt_pr_discount_commission - ntxt_pr_discount_owner;

            ntxt_prTotalCommission = $find("<%= ntxt_prTotalCommission.ClientID %>").get_value();
            ntxt_prTotalOwner = ntxt_pr_total - ntxt_prTotalCommission - ntxt_pr_part_agency_fee - ntxt_agentCommissionPrice;
            $find("<%= ntxt_prTotalOwner.ClientID %>").set_value(ntxt_prTotalOwner);
            $find("<%= ntxt_prTotalCommissionWithAgencyFee.ClientID %>").set_value((ntxt_prTotalCommission + ntxt_pr_part_agency_fee));
            $find("<%= ntxt_prTotalCommissionWithAgencyFeeAndAgentCommission.ClientID %>").set_value((ntxt_prTotalCommission + ntxt_pr_part_agency_fee + ntxt_agentCommissionPrice));

            ntxt_pr_total += (ntxt_pr_srsPrice + ntxt_pr_ecoPrice);
            if (drp_agentDiscountNotPayed == "1")
                ntxt_pr_total -= ntxt_agentCommissionPrice;
            $find("<%= ntxt_pr_total.ClientID %>").set_value(ntxt_pr_total);
            ntxt_prPercentage = $find("<%= ntxt_prPercentage.ClientID %>").get_value();
            ntxt_prPercentagePrice = ((ntxt_pr_total - ntxt_pr_part_agency_fee) * ntxt_prPercentage / 100).toFixed(2);
            $("#ntxt_prPercentagePrice").html(("" + ntxt_prPercentagePrice).replace(".", ","));
            ntxt_pr_part_payment_total = $find("<%= ntxt_pr_part_payment_total.ClientID %>").get_value();
            $("#ntxt_prPercentagePriceCont").css("display", (ntxt_prPercentagePrice == ntxt_pr_part_payment_total ? "none" : ""))
            ntxt_pr_part_owner = ntxt_pr_total - ntxt_pr_part_payment_total;
            $find("<%= ntxt_pr_part_owner.ClientID %>").set_value(ntxt_pr_part_owner);
            
            set_valueCalled = false;
        }
    }
</script>
<script type="text/javascript">
    function addOptioni(obj) {
        calculateTotals();
    }
    function changeServivePrice(sender, eventArgs) {
        calculateTotals();
    }
</script>
<asp:HiddenField ID="HF_pr_percentage" runat="server" />
<asp:HiddenField ID="HF_estate" runat="server" />
<asp:HiddenField ID="HF_adult" runat="server" />
<asp:HiddenField ID="HF_child" runat="server" />
<asp:HiddenField ID="HF_days" runat="server" />
<asp:HiddenField ID="HF_resId" runat="server" />
<asp:HiddenField ID="HF_tmp" runat="server" Value="0" />
<telerik:RadNumericTextBox ID="ntxt_prPercentage" runat="server" Width="100" Style="display: none;"></telerik:RadNumericTextBox>

<table class="selPeriod risric" cellpadding="0" cellspacing="0" id="pnlEdit" runat="server">
    <tr>
        <td style="width: 250px;">Totale Affitto
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prTotalRate" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <script type="text/javascript">
                $(document).ready(function () {
                    calculateTotals();
                });
            </script>
        </td>
    </tr>
    <tr id="pnlDiscount" runat="server" visible="false">
        <td style="width: 250px;">Sconto (Commissione + Proprietario)
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_discount_commission" runat="server" Width="80" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            +
            <telerik:RadNumericTextBox ID="ntxt_pr_discount_owner" runat="server" Width="80" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:TextBox ID="txt_pr_discount_desc" runat="server" Width="200"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px;">Offerta speciale
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prDiscountSpecialOffer" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:TextBox ID="TextBox1" runat="server" Width="250"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px;">Sconto LastMinute
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prDiscountLastMinute" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:TextBox ID="txt_prDiscountLastMinuteDesc" runat="server" Width="250"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px;">Sconto LongStay
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prDiscountLongStay" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:TextBox ID="txt_prDiscountLongStayDesc" runat="server" Width="250"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px;">Sconto LongRange
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prDiscountLongRange" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:TextBox ID="txt_prDiscountLongRangeDesc" runat="server" Width="250"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Comm. Agenzia
        </td>
        <td valign="middle" align="right">
            <telerik:RadNumericTextBox ID="ntxt_agentCommissionPrice" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
            <asp:DropDownList ID="drp_agentDiscountNotPayed" runat="server" onchange="calculateTotals()">
                <asp:ListItem Value="0" Text="Compreso nel totale"></asp:ListItem>
                <asp:ListItem Value="1" Text="Scalato dal totale"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Agency Fee
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_part_agency_fee" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Pulizia finale
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_ecoPrice" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Accoglienza
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_srsPrice" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Totale Prenotazione
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_total" runat="server" Width="100" ReadOnly="true" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr id="pnl_ntxt_bcom_totalForOwner" runat="server">
        <td>Totale che vede il Prop.
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_bcom_totalForOwner" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 20px;">&nbsp;
        </td>
    </tr>
    <tr>
        <td>Acconto <span id="ntxt_prPercentagePriceCont">(<%=ntxt_prPercentage.Value.objToInt32() %>% = <span id="ntxt_prPercentagePrice"></span>)</span>
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_part_payment_total" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Saldo
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_pr_part_owner" runat="server" Width="100" ReadOnly="true" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>
            Abilita Pagamento del saldo
        </td>
        <td align="right">
            <asp:DropDownList ID="drp_requestFullPayAccepted" runat="server">
                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            Prezzi manuali / Non da tariffa
        </td>
        <td align="right">
            <asp:DropDownList ID="drp_pr_part_modified" runat="server">
                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 20px;">&nbsp;
        </td>
    </tr>
    <tr>
        <td>Comm. R
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prTotalCommission" runat="server" Width="100" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents OnValueChanged="calculateTotals" />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>
            Comm. R + Agency Fee
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prTotalCommissionWithAgencyFee" runat="server" Width="100" ReadOnly="true" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Comm. R + Agency Fee + Comm. Agenzia 
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prTotalCommissionWithAgencyFeeAndAgentCommission" runat="server" Width="100" ReadOnly="true" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
    <tr>
        <td>Netto Proprietario
        </td>
        <td align="right">
            <telerik:RadNumericTextBox ID="ntxt_prTotalOwner" runat="server" Width="100" ReadOnly="true" Style="text-align: right;">
                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                <ClientEvents />
            </telerik:RadNumericTextBox>
        </td>
    </tr>
</table>
<table class="selPeriod risric" cellpadding="0" cellspacing="0" id="pnlView" runat="server">
    <tr>
        <td style="width: 250px;">Totale Affitto
        </td>
        <td align="right">
            <%=ntxt_prTotalRate.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr id="pnlDiscount2" runat="server" visible="false">
        <td style="width: 250px;">Sconto (Commissione + Proprietario)<br />
            <i><%=txt_pr_discount_desc.Text%></i>
        </td>
        <td align="right">
            <%= ntxt_pr_discount_commission.Value.objToDecimal().ToString("N2")+"&nbsp;&euro; + "+ntxt_pr_discount_owner.Value.objToDecimal().ToString("N2")+"&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Offerta speciale
        </td>
        <td valign="middle" align="right">
            <%=ntxt_prDiscountSpecialOffer.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>Sconto LastMinute<br />
            <i><%=txt_prDiscountLastMinuteDesc.Text%></i>
        </td>
        <td valign="middle" align="right">
            <%=ntxt_prDiscountLastMinute.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>Sconto LongStay<br />
            <i><%=txt_prDiscountLongStayDesc.Text%></i>
        </td>
        <td valign="middle" align="right">
            <%=ntxt_prDiscountLongStay.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>Sconto LongRange<br />
            <i><%=txt_prDiscountLongRangeDesc.Text%></i>
        </td>
        <td valign="middle" align="right">
            <%=ntxt_prDiscountLongRange.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>Comm. Agenzia
        </td>
        <td valign="middle" align="right">
            <%=ntxt_agentCommissionPrice.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;&nbsp;"%>
            <%=drp_agentDiscountNotPayed.getSelectedValueInt()==0?"Compreso nel totale":"Scalato dal totale"%>
        </td>
    </tr>
    <tr>
        <td>Agency Fee
        </td>
        <td align="right">
            <%=ntxt_pr_part_agency_fee.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Pulizia finale
        </td>
        <td align="right">
            <%=ntxt_pr_ecoPrice.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Accoglienza
        </td>
        <td align="right">
            <%=ntxt_pr_srsPrice.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Totale Prenotazione
        </td>
        <td align="right">
            <%=ntxt_pr_total.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr id="pnl_ntxt_bcom_totalForOwner2" runat="server">
        <td style="width: 250px;">Totale che vede il Prop.
        </td>
        <td align="right">
            <%=ntxt_bcom_totalForOwner.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 20px;">&nbsp;
        </td>
    </tr>
    <tr runat="server">
        <td>Acconto
        </td>
        <td align="right">
            <%=ntxt_pr_part_payment_total.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>Saldo
        </td>
        <td align="right">
            <%=ntxt_pr_part_owner.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
    <tr>
        <td>
            Abilitato Pagamento del saldo
        </td>
        <td align="right">
            <%=drp_requestFullPayAccepted.getSelectedValueInt()==1?"Si":"No"%>
        </td>
    </tr>
    <tr>
        <td>
            Prezzi manuali / Non da tariffa
        </td>
        <td align="right">
            <%=drp_pr_part_modified.getSelectedValueInt()==1?"Si":"No"%>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 20px;">&nbsp;
        </td>
    </tr>
    <tr>
        <td>Comm. R
        </td>
        <td align="right">
            <%=ntxt_prTotalCommission.Value.objToDecimal().ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>
            Comm. R + Agency Fee 
        </td>
        <td align="right">
            <%=(ntxt_prTotalCommission.Value.objToDecimal()+ntxt_pr_part_agency_fee.Value.objToDecimal()).ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Comm. R + Agency Fee + Comm. Agenzia 
        </td>
        <td align="right">
            <%=(ntxt_prTotalCommission.Value.objToDecimal()+ntxt_pr_part_agency_fee.Value.objToDecimal()+ntxt_agentCommissionPrice.Value.objToDecimal()).ToString("N2")+ "&nbsp;&euro;" %>
        </td>
    </tr>
    <tr>
        <td>Netto Proprietario
        </td>
        <td align="right">
            <%=ntxt_prTotalOwner.Value.objToDecimal().ToString("N2") + "&nbsp;&euro;"%>
        </td>
    </tr>
</table>
