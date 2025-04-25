<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateChnlAtraveo_fees.aspx.cs" Inherits="MagaRentalCE.admin.modRental.EstateChnlAtraveo_fees" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/modRental/uc/ucEstateChnlAtraveoTab.ascx" TagName="ucNav" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <img src="/images/css/atraveo-integrated-magarental.png" class="homeAwayLogo" alt="Integrated with Atraveo" />

    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <h1 class="titolo_main">Fees of Property for Atraveo:<asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
        </h1>
    </telerik:RadCodeBlock>
    <div id="fascia1" style="margin-bottom: 10px;">
        <div class="tabsTop" id="tabsHomeaway">
            <uc1:ucNav ID="ucNav" runat="server" />
        </div>
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1">
            <asp:HiddenField ID="HF_IdEstate" Value="" runat="server" />
            <asp:HiddenField ID="HF_uid" Value="" runat="server" />
            <div style="clear: both">
                <div class="table_fascia">
                    <table border="0" cellpadding="0" cellspacing="0" style="">
                        <tr style="text-align: left">
                            <th> </th>
                            <th>Code</th>
                            <th>drpUnit</th>
                            <th>Type</th>
                            <th>Cost</th>
                            <th>IntervalType</th>
                            <th>MandatoryCode</th>
                            <th>PayTime</th>
                            <th></th>
                        </tr>
                        <asp:ListView ID="Lv" runat="server" OnItemCommand="LvItemCommand">
                            <ItemTemplate>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                                <tr style="text-align: left">
                                    <td>#
                                        <asp:Literal ID="ltrNumber" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Code" runat="server" Style="height: 25px;" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpUnit" runat="server" Style="height: 25px;" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CostType" runat="server" Style="height: 25px;" Width="100px">
                                            <asp:ListItem Value="Cost">Flat</asp:ListItem>
                                            <asp:ListItem Value="Percent">Percent</asp:ListItem>
                                            <asp:ListItem Value="DayRent">DayRent</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="Cost" runat="server" Width="50">
                                            <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                        </telerik:RadNumericTextBox>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="IntervalType" runat="server" Style="height: 25px;" Width="100px">
                                            <asp:ListItem Value="">per stay</asp:ListItem>
                                            <asp:ListItem Value="wo">per week</asp:ListItem>
                                            <asp:ListItem Value="ta">per day</asp:ListItem>
                                            <asp:ListItem Value="we">per weekend</asp:ListItem>
                                            <asp:ListItem Value="kw">per short weekend</asp:ListItem>
                                            <asp:ListItem Value="mw">per midweek</asp:ListItem>
                                            <asp:ListItem Value="nv">use</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="MandatoryCode" runat="server" Style="height: 25px;" Width="100px">
                                            <asp:ListItem Value="0">optional</asp:ListItem>
                                            <asp:ListItem Value="1">compulsory</asp:ListItem>
                                            <asp:ListItem Value="2">compulsory with pet</asp:ListItem>
                                            <asp:ListItem Value="3">to be brought by you</asp:ListItem>
                                            <asp:ListItem Value="4">to be done by you</asp:ListItem>
                                            <asp:ListItem Value="8">none (only for visitor's tax and deposit)</asp:ListItem>
                                            <asp:ListItem Value="9">inclusive</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="LocationOrder" runat="server" Style="height: 25px;" Width="100px">
                                            <asp:ListItem Value="Order">paid in advance</asp:ListItem>
                                            <asp:ListItem Value="Location">paid on site</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="inlinebtn" CommandName="mod">Modifica</asp:LinkButton>
                                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="inlinebtn" CommandName="sav">Salva</asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="server" CssClass="inlinebtn" CommandName="canc">Annulla</asp:LinkButton>
                                        <asp:LinkButton ID="lnkDel" runat="server" CssClass="inlinebtn" CommandName="del" OnClientClick="return confirm('Vuoi eliminare?')">Elimina</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <tr style="text-align: left">
                            <td>Aggiungi
                            </td>
                            <td>
                                <asp:DropDownList ID="Code" runat="server" Style="height: 25px;" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpUnit" runat="server" Style="height: 25px;" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="CostType" runat="server" Style="height: 25px;" Width="100px">
                                    <asp:ListItem Value="Cost">Flat</asp:ListItem>
                                    <asp:ListItem Value="Percent">Percent</asp:ListItem>
                                    <asp:ListItem Value="DayRent">DayRent</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="Cost" runat="server" Width="50">
                                    <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <asp:DropDownList ID="IntervalType" runat="server" Style="height: 25px;" Width="100px">
                                    <asp:ListItem Value="">per stay</asp:ListItem>
                                    <asp:ListItem Value="wo">per week</asp:ListItem>
                                    <asp:ListItem Value="ta">per day</asp:ListItem>
                                    <asp:ListItem Value="we">per weekend</asp:ListItem>
                                    <asp:ListItem Value="kw">per short weekend</asp:ListItem>
                                    <asp:ListItem Value="mw">per midweek</asp:ListItem>
                                    <asp:ListItem Value="nv">use</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="MandatoryCode" runat="server" Style="height: 25px;" Width="100px">
                                    <asp:ListItem Value="0">optional</asp:ListItem>
                                    <asp:ListItem Value="1">compulsory</asp:ListItem>
                                    <asp:ListItem Value="2">compulsory with pet</asp:ListItem>
                                    <asp:ListItem Value="3">to be brought by you</asp:ListItem>
                                    <asp:ListItem Value="4">to be done by you</asp:ListItem>
                                    <asp:ListItem Value="8">none (only for visitor's tax and deposit)</asp:ListItem>
                                    <asp:ListItem Value="9">inclusive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="LocationOrder" runat="server" Style="height: 25px;" Width="100px">
                                    <asp:ListItem Value="Order">paid in advance</asp:ListItem>
                                    <asp:ListItem Value="Location">paid on site</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="inlinebtn" OnClick="lnkSave_Click">Salva</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="inlinebtn" OnClick="lnkCancel_Click">Annulla</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>                
            </div>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlFascia.ClientID%>").ajaxRequest('rwdDett_Closing');
                //RadAjaxManager_ajaxRequest('rwdDett_Closing');
                return false;
            }
            function rwdDett_OnClientClose(sender, eventArgs) {
                return rwdDettClose();
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
