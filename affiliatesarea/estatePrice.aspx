<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="estatePrice.aspx.cs" Inherits="RentalInRome.affiliatesarea.estatePrice" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
        @import url(/css/style.css<%="?tmp="+DateTime.Now.Ticks %>);
        @import url(/css/common.css<%="?tmp="+DateTime.Now.Ticks %>);
        @import url(/js/shadowbox/shadowbox.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-slider.css);
        @import url(/jquery/css/ui-datepicker.css);
        @import url(/jquery/css/ui-slider.css);
        @import url(/jquery/css/ui-autocomplete.css);
        @import url(/css/styleagent.css);
        @import url(/admin/css/adminStyle.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
        <div class="agentEstateDetail">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="HF_id" Value="" runat="server" />
                    <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
                   <div class="agentEstateDett">
                       <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                           <tr>
                               <td class="td_title" style="width: auto;">
                                   Property Name:
                               </td>
                               <td>
                                   <asp:TextBox ID="txt_estateTitle_agent" runat="server"></asp:TextBox>
                               </td>
                           </tr>
                           <tr>
                               <td>
                                   <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click">
                                    <span> <%=CurrentSource.getSysLangValue("lblSaveChanges")%></span>
                                   </asp:LinkButton>
                               </td>
                               <td>
                                   <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click">
                                 <span><%=CurrentSource.getSysLangValue("lblCancelChanges")%></span>
                                   </asp:LinkButton>
                               </td>
                           </tr>
                       </table>
                    </div>
                    <h1 class="titolo_main">Property Price:
                    <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
                    <div class="nulla">
                    </div>
                    <asp:Panel ID="pnlContent" runat="server" Width="100%">
                        <!-- INIZIO MAIN LINE -->
                        <div class="mainline agentEstatePrice">
                            <!-- BOX 1 -->
                            <div class="mainbox">
                                <div class="top">
                                    <div style="float: left;">
                                        <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                                    </div>
                                    <div style="float: right;">
                                        <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                                    </div>
                                </div>
                                <div class="center">
                                    <%--<span class="titoloboxmodulo" style="margin-bottom: 5px;">Stagionalità della struttura</span>                  
                                    <asp:Label runat="server" ID="lblSeason" CssClass="season" style="display:none"></asp:Label>--%>
                                    <div class="boxmodulo">
                                        <table border="0" cellspacing="3" cellpadding="0">
                                            <tr>
                                                <td colspan="6">
                                                    Price for max persons:&nbsp;&nbsp;<span style="font-weight: normal;"><asp:Label ID="lbl_pr_basePersons" runat="server"></asp:Label>&nbsp;persons </span> 
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td colspan="5" style="border: 1px dotted; padding: 5px;">
                                                    <a id="lnk_pr_basePersons" class="inlinebtn" onclick="toggle_pr_basePersons()" style="cursor: pointer;">Vedi Note</a>
                                                    <br />
                                                    <div id="div_pr_basePersons" style="display: none;">
                                                        <script type="text/javascript">
                                                            function toggle_pr_basePersons() {
                                                                var tmp = $("#div_pr_basePersons");
                                                                if (tmp.css("display") == "none") {
                                                                    tmp.css("display", "block");
                                                                    $("#lnk_pr_basePersons").html("Nascondi Note");
                                                                }
                                                                else {
                                                                    tmp.css("display", "none");
                                                                    $("#lnk_pr_basePersons").html("Vedi Note");
                                                                }
                                                            }
                                                        </script>
                                                        Prezzo base indica il prezzo minimo che viene accettato.<br />
                                                        Max persone per Prezzo Base indica il numero persone ospitate senza Letto aggiuntivo.<br />
                                                        Es:<br />
                                                        Max persone per Prezzo Base: <strong>4</strong><br />
                                                        Prezzo base: <strong>150</strong>&euro;<br />
                                                        Letto aggiuntivo: <strong>30</strong>&euro;<br />
                                                        <br />
                                                        1 pax = 150&euro;,<br />
                                                        2 pax = 150&euro;,<br />
                                                        3 pax = 150&euro;,<br />
                                                        4 pax = 150&euro;,<br />
                                                        5 pax = 180&euro;,<br />
                                                        6 pax = 210&euro;,<br />
                                                        7 pax = 240&euro;<br />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Season/Persons</th>
                                                <th>Rental Price</th>
                                                <th>Your Price</th>
                                                <th>Extra Bed</th>
                                                <th>Rental Price</th>
                                                <th>Your Price</th>

                                            </tr>
                                            <tr>
                                                <td colspan="6" class="td_title">
                                                    <span class="titoloboxmodulo">Low Season</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Price (2 persons):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_1" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_1_agent" runat="server"></asp:Label>
                                                </td>
                                                <td class="td_title">Extra Bed (+1 person):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_1" runat="server"></asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lbl_price_optional_1_agent" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" class="td_title">
                                                    <span class="titoloboxmodulo">Middle Season </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Price (2 persons):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_4" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_4_agent" runat="server"></asp:Label>
                                                </td>
                                                <td class="td_title">Extra Bed (+1 person):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_4" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_4_agent" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" class="td_title">
                                                    <span class="titoloboxmodulo">High Season</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Price (2 persons):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_2" runat="server"></asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:Label ID="lbl_price_2_agent" runat="server"></asp:Label>
                                                </td>
                                                <td class="td_title">Extra Bed (+1 person):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_2" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_2_agent" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" class="td_title">
                                                    <span class="titoloboxmodulo">Very High Season (+20%)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Price (2 persons):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_3" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_3_agent" runat="server"></asp:Label>
                                                </td>
                                                <td class="td_title">Extra Bed (+1 person):
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_3" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_price_optional_3_agent" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                         <table border="0" cellspacing="3" cellpadding="0">
                                            <tr>
                                                <td colspan="2" class="td_title">
                                                    <span class="titoloboxmodulo">LastMinute</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title" style="width: 50%;">Hours before Check in:
                                                </td>
                                                <td style="width: 50%;">
                                                    <asp:Label ID="lbl_lm_inhours" runat="server"></asp:Label>&nbsp;hour                                              
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Discount:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_lm_discount" runat="server"></asp:Label>&nbsp;%
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Min. stay:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_lm_nights_min" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_title">Max Nights:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_lm_nights_max" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <%-- <tr>
                                            <td colspan="5" class="td_title">
                                                <span class="titoloboxmodulo">Sconto a periodi lunghi</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_title">
                                                <strong>Sistema:</strong>
                                            </td>
                                            <td>
                                                 <asp:Label ID="lbl_pr_dcSUsed" runat="server"></asp:Label>                                           
                                            </td>
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr id="pnl_dcSUsed_1" runat="server">
                                            <td colspan="5">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="td_title">Settimanale (7gg):
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_pr_discount7days" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                        <td style="width: 30px;"></td>
                                                        <td id="Td1" class="td_title" runat="server" visible="false">Mensile (30gg):
                                                            <br />
                                                            *non utilizzato
                                                        </td>
                                                        <td id="Td2" runat="server" visible="false">
                                                            <asp:Label ID="lbl_pr_discount30days" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <strong>Attenzione!</strong><br />
                                                            Lo sconto inserito si applica per Ogni 7 notti della prenotazione.<br />
                                                            Es:<br />
                                                            Settimanale (7gg): <strong>5</strong> %<br />
                                                            <br />
                                                            in un pren. di 6 notti il prezzo si calcola senza sconto,<br />
                                                            7 notti - (7 notti scontato di 5%)<br />
                                                            8 notti - (7 notti scontato di 5%) + 1 notte senza sconto<br />
                                                            13 notti - (7 notti scontato di 5%) + 6 notti senza sconto<br />
                                                            14 notti - (14 notti scontato di 5%)<br />
                                                            15 notti - (14 notti scontato di 5%) + 1 notte senza sconto<br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="pnl_dcSUsed_2" runat="server" visible="false">
                                            <td colspan="5">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 80px;"></td>
                                                        <td style="width: 100px;"></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 1:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_1_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_1_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 2:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_2_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_2_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 3:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_3_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_3_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr2" runat="server" visible="false">
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 4:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_4_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_4_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr3" runat="server" visible="false">
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 5:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_5_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_5_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr4" runat="server" visible="false">
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 6:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_6_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_6_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr5" runat="server" visible="false">
                                                        <td style="border-bottom: thin dotted;">
                                                            <strong>Periodo 7:</strong>
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Dopo&nbsp;<asp:Label ID="lbl_pr_dcS2_7_inDays" runat="server"></asp:Label>&nbsp;notti
                                                        </td>
                                                        <td style="border-bottom: thin dotted;">Sconto&nbsp;<asp:Label ID="lbl_pr_dcS2_7_percent" runat="server"></asp:Label>&nbsp;%
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <strong>Attenzione!</strong><br />
                                                            Gli sconti inseriti si applicano per il periodo intero della prenotazione.<br />
                                                            Es:<br />
                                                            P.1 - Dopo <strong>6</strong> notti Sconto <strong>5</strong> %<br />
                                                            P.2 - Dopo <strong>9</strong> notti Sconto <strong>10</strong> %<br />
                                                            P.3 - Dopo <strong>14</strong> notti Sconto <strong>15</strong> %<br />
                                                            <br />
                                                            in un pren. di 6 notti il prezzo si calcola senza sconto,<br />
                                                            7 notti - 5% di sconto<br />
                                                            9 notti - 5% di sconto<br />
                                                            10 notti - 10% di sconto<br />
                                                            14 notti - 10% di sconto<br />
                                                            15 notti e più - 15% di sconto 
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>   --%>
                                        </table>
                                    </div>
                                </div>
                                <div class="bottom">
                                    <div style="float: left;">
                                        <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                                    </div>
                                    <div style="float: right;">
                                        <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                                    </div>
                                </div>
                            </div>
                            <div class="salvataggio">
                                <div class="nulla">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
