<%@ Page Title="" Language="C#" MasterPageFile="~/affiliatesarea/masterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RentalInRome.affiliatesarea.Default" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=CurrentSource.getSysLangValue("lblAffiliatesArea")%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <div class="colsx">
        <h3 style="margin-bottom: 20px; margin-left: 8px;" class="underlined"><%=CurrentSource.getSysLangValue("lblRiepilogo")%></h3>
        <asp:Literal ID="ltrAgentDiscount_layoutTemplate" runat="server" Visible="false">
            <table class="infoComm1" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="middle" align="left">
                        #mese#:
                    </td>
                    <td valign="middle" align="left">
                        <strong>#currMonth#</strong>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        #currSumLabel#:
                    </td>
                    <td valign="middle" align="left">
                        &euro;&nbsp;<strong style="color: #fe6634;">#currSum#</strong>
                    </td>
                </tr>
            </table>
            <div class="boxdiscountagent">
                <table class="infoComm2" cellpadding="0" cellspacing="0">
                    <tr>
                        <th colspan="2" valign="middle" align="left">#lblCommissions#: </th>
                    </tr>
                    #discountList#
                </table>
            </div>
        </asp:Literal>
        <asp:Literal ID="ltrAgentDiscount_itemTemplate" runat="server" Visible="false">
            <tr class="#cssClass#">
                <td valign="middle" align="left" class="colSn">
                    #toChange#
                </td>
                <td valign="middle" align="right" class="colDs">
                    #discount#%
                </td>
            </tr>
        </asp:Literal>
        <asp:Literal ID="ltr_stampaRiepilogo" runat="server"></asp:Literal>
    </div>
    <div class="coldx">
                    


        <div class="nulla"></div>
    </div>

</asp:Content>
