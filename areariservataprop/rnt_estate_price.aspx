<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/areariservataprop/common/MP.Master" AutoEventWireup="true" CodeBehind="rnt_estate_price.aspx.cs" Inherits="RentalInRome.areariservataprop.rnt_estate_price" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/areariservataprop/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Src="~/areariservataprop/uc/UC_contactStaff.ascx" TagName="UC_contactStaff" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_ESTATE_PRICEs" OrderBy="dtStart desc, priority desc" Where="pid_estate == @pid_estate &amp;&amp; is_active == @is_active">
                <WhereParameters>
                    <asp:ControlParameter ControlID="HF_IdEstate" Name="pid_estate" PropertyName="Value" Type="Int32" />
                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                </WhereParameters>
            </asp:LinqDataSource>
            <h1 class="titolo_main">Prezzi della struttura:  <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal></h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
                <div class="pannello_fascia1" id="pnlList" runat="server" visible="false">
                    <div style="clear: both">
                        <h1></h1>
                        <div class="bottom_agg">
                            <asp:LinkButton ID="lnk_change" runat="server" OnClick="lnk_change_Click"><span>Cambia Prezzi</span></asp:LinkButton>
                            <asp:LinkButton ID="lnk_new" runat="server" OnClick="lnk_new_Click" Visible="false" ><span>+ Nuovo periodo</span></asp:LinkButton>
                        </div>
                    </div>
                    <div style="clear: both">
                        <asp:ListView ID="LV" runat="server" OnSelectedIndexChanging="LV_SelectedIndexChanging">
                            <ItemTemplate>
                                <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# CurrentSource.rntPeriod_title(Eval("pid_period").objToInt32(),1,"")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtStart")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtEnd")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price_optional")%></span>
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:LinkButton ID="lnk_select" runat="server" CommandName="select">scheda</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" OnClientClick="return confirm('Sta per eliminare Record?');removeTinyEditor();">elimina</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                                    <td>
                                        <span>
                                            <%# CurrentSource.rntPeriod_title(Eval("pid_period").objToInt32(), 1, "")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtStart")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtEnd")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price_optional")%></span>
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                        <asp:LinkButton ID="lnk_select" runat="server" CommandName="select">scheda</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnk_delete" runat="server" CommandName="elimina" OnClientClick="return confirm('Sta per eliminare Record?');removeTinyEditor();">elimina</asp:LinkButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            No data was returned.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <InsertItemTemplate>
                            </InsertItemTemplate>
                            <LayoutTemplate>
                                <div class="table_fascia">
                                    <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="Tr1" runat="server" style="">
                                            <th style="width: 150px">
                                                Stagione
                                            </th>
                                            <th style="width: 120px">
                                                Data Inizio
                                            </th>
                                            <th style="width: 120px">
                                                Data Fine
                                            </th>
                                            <th style="width: 100px">
                                                Prezzo (2pax)
                                            </th>
                                            <th id="Th2" runat="server" style="width: 100px">
                                                letto aggiuntivo (+1pax)
                                            </th>
                                            <th id="Th6" runat="server" visible="false">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                            <SelectedItemTemplate>
                                <tr class="current">
                                    <td>
                                        <span>
                                            <%# CurrentSource.rntPeriod_title(Eval("pid_period").objToInt32(), 1, "")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtStart")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# ((DateTime)Eval("dtEnd")).formatITA_Long(false, false)%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price")%></span>
                                    </td>
                                    <td>
                                        <span>
                                            <%# Eval("price_optional")%></span>
                                    </td>
                                    <td runat="server" visible="false">
                                        <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="nulla">
            </div>
            <asp:Panel ID="pnlContent" runat="server" Width="100%">
                <!-- INIZIO MAIN LINE -->
                <div class="mainline">
                    <!-- BOX 1 -->
                    <div class="mainbox">
                        <div class="top">
                            <div style="float: left;">
                                <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                        </div>
                        <div class="center">
                            <div class="boxmodulo">
                                <table border="0" cellspacing="3" cellpadding="0">
                                    <tr>
                                        <td colspan="5" class="td_title">
                                            <span class="titoloboxmodulo">Bassa Stagione</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_1" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ntxt_price_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width:30px;">
                                        </td>
                                        <td class="td_title">
                                            Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_1" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ntxt_price_optional_1" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">Alta Stagione</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_2" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="ntxt_price_2" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td class="td_title">
                                            Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_2" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="ntxt_price_optional_2" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">Altissima Stagione ( +20%)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Prezzo (2pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_3" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="ntxt_price_3" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td class="td_title">
                                            Letto aggiuntivo (+1pax):
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="ntxt_price_optional_3" runat="server" Width="50" Type="Currency">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="ntxt_price_optional_3" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">Sconto a periodi lunghi</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Settimanale (7gg):
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pr_discount7days" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txt_pr_discount7days" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" ControlToValidate="txt_pr_discount7days" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td class="td_title">
                                            Mensile (30gg):
                                            <br/>*non utilizzato
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pr_discount30days" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="txt_pr_discount30days" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" ControlToValidate="txt_pr_discount30days" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo">LastMinute</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Ore prima del checkin:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_inhours" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;ore
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="txt_lm_inhours" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" ControlToValidate="txt_lm_inhours" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td class="td_title">
                                            Sconto:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_discount" runat="server" Style="width: 80px"></asp:TextBox>&nbsp;%
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="txt_lm_discount" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" ControlToValidate="txt_lm_discount" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_title">
                                            Min notti:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_nights_min" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txt_lm_nights_min" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txt_lm_nights_min" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td class="td_title">
                                            Max notti:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_lm_nights_max" runat="server" Style="width: 80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txt_lm_nights_max" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" ControlToValidate="txt_lm_nights_max" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <uc1:UC_contactStaff ID="UC_contactStaff" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td_title">
                                            <span class="titoloboxmodulo" id="lbl_changeSaved" runat="server" visible="false">Le modifiche sono state salvate...</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bottom">
                            <div style="float: left;">
                                <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                            <div style="float: right;">
                                <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                        </div>
                    </div>
                    <div class="salvataggio">
                        <div class="nulla">
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <script type="text/javascript">
                
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:PlaceHolder ID="PH_hidden" runat="server" Visible="false">

        <script type="text/javascript">
                var _JSCal_Range;
                function setCal(dtStartInt, dtEndInt) {
                    _JSCal_Range = new JSCal.Range({ startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", startDateInt: dtStartInt, endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", endDateInt: dtEndInt, beforeShowDay: CheckDisabledDate });
                }
        </script>

        <table border="0" cellspacing="3" cellpadding="0">
            <tr>
                <td class="td_title">
                    Stagione:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drp_period" DataTextField="title" DataValueField="id" />
                </td>
            </tr>
            <tr>
                <td class="td_title">
                    Data di inizio:
                </td>
                <td>
                    <input id="txt_dtStart" type="text" readonly="readonly" style="width: 150px" />
                    <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td class="td_title">
                    Data di fine:
                </td>
                <td>
                    <input id="txt_dtEnd" type="text" readonly="readonly" style="width: 150px" />
                    <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                </td>
            </tr>
            <tr>
                <td class="td_title">
                    Prezzo (2pax):
                </td>
                <td>
                    &euro;&nbsp;<input type="text" runat="server" id="txt_price" style="width: 120px" />
                    <asp:RequiredFieldValidator runat="server" ID="RFV_1" ControlToValidate="txt_price" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="REV_txt_price" ControlToValidate="txt_price" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="td_title">
                    Letto aggiuntivo (+1pax):
                </td>
                <td>
                    &euro;&nbsp;<input type="text" runat="server" id="txt_price_optional" style="width: 120px" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txt_price_optional" ForeColor="Red" ErrorMessage="<br/>// obbligatorio" ValidationGroup="price" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txt_price_optional" CssClass="errore_form" ValidationGroup="price" ValidationExpression="(^(-)?\d*?$)" runat="server" ErrorMessage="<br/>// formato non valido" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
