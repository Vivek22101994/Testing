<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_special_offer_list.aspx.cs" Inherits="RentalInRome.admin.rnt_special_offer_list" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Literal ID="ltrLDSfiltter" runat="server" Visible="false"></asp:Literal>
    <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaRental_DataContext" TableName="RNT_VIEW_SPECIAL_OFFER" OrderBy="dtEnd desc" Where="pid_lang == 1">
    </asp:LinqDataSource>
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>Elenco offerte speciali / lastminute</h1>
                <div class="bottom_agg">
                    <a href="rnt_special_offer_details.aspx?id=0">
                        <span>+ Nuova Offerta</span></a>
                </div>
                <div class="nulla">
                </div>
                <div class="filt">
                    <div class="t">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                    <div class="c">
                        <div class="filtro_cont">
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <label>
                                            Valido in date:</label>
                                        <table class="inp">
                                            <tr>
                                                <td>
                                                    <label>
                                                        da:</label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="rdp_flt_dtFrom" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        a:</label>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="rdp_flt_dtTo" runat="server" Width="100px" CssClass="inp">
                                                        <DateInput DateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="nulla">
                                        </div>
                                    </td>
                                    <td>
                                        <label>
                                            Città:</label>
                                        <asp:DropDownList runat="server" ID="drp_flt_pidCity" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drp_flt_pidCity_SelectedIndexChanged" CssClass="inp" Style="margin-bottom: 10px;" />
                                        <div class="nulla">
                                        </div>
                                        <label>
                                            Proprietario:</label>
                                        <asp:DropDownList runat="server" ID="drp_flt_pidOwner" Width="200px" CssClass="inp" Style="margin-bottom: 10px;" />
                                        <div class="nulla">
                                        </div>
                                        <label>
                                            Nome Appartamento:</label>
                                        <asp:TextBox ID="txt_flt_code" runat="server" Width="200px" CssClass="inp"></asp:TextBox>
                                    </td>
                                    <td style="width: 20px;">
                                    </td>
                                    <td>
                                        <label>
                                            Zona:
                                            <br />
                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_pidZone" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_pidZone" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                                        </label>
                                        <div class="nulla">
                                        </div>
                                        <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                            <asp:CheckBoxList ID="chkList_flt_pidZone" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="6" Style="margin: 0 5px 5px 0;">
                                            </asp:CheckBoxList>
                                            <div class="nulla">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="b">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                    <ItemTemplate>
                        <tr class="" onmouseout="SetClassName(this,'')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("id") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("class_type") %></span>
                            </td>
                            <td>
                               <span><%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "---")%></span>
                            </td>
                            <td>
                                <span><%# Eval("title") %></span>
                            </td>
                            <td>
                                <%# "" + Eval("is_active") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>
                            <td>
                                <%# "" + Eval("img_thumb") == "1" ? "<a onclick='SITE_showLoader()' href='rnt_special_offer_list.aspx?sethome=true&id=" + Eval("id") + "' style=\"color:#0f0\">SI</a>" : "<a onclick='SITE_showLoader()' href='rnt_special_offer_list.aspx?sethome=true&id=" + Eval("id") + "' style=\"color:#f00\">NO</a>"%>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", 1, "")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", 1, "")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="rnt_special_offer_details.aspx?id=<%# Eval("id") %>">modifica</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseout="SetClassName(this,'alternate')" onmouseover="SetClassName(this,'current')">
                            <td>
                                <span>
                                    <%# Eval("id") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("class_type") %></span>
                            </td>
                            <td>
                                <span>
                                    <%# CurrentSource.rntEstate_code(Eval("pid_estate").objToInt32(), "---")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# Eval("title") %></span>
                            </td>
                            <td>
                                <%# "" + Eval("is_active") == "1" ? "<span style=\"color:#0f0\">SI</span>" : "<span style=\"color:#f00\">NO</span>"%>
                            </td>
                            <td>
                                <%# "" + Eval("img_thumb") == "1" ? "<a onclick='SITE_showLoader()' href='rnt_special_offer_list.aspx?sethome=true&id=" + Eval("id") + "' style=\"color:#0f0\">SI</a>" : "<a onclick='SITE_showLoader()' href='rnt_special_offer_list.aspx?sethome=true&id=" + Eval("id") + "' style=\"color:#f00\">NO</a>"%>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtStart")).formatCustom("#dd# #MM# #yy#", 1, "")%></span>
                            </td>
                            <td>
                                <span>
                                    <%# ((DateTime)Eval("dtEnd")).formatCustom("#dd# #MM# #yy#", 1, "")%></span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <a href="rnt_special_offer_details.aspx?id=<%# Eval("id") %>">modifica</a>
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
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table id="itemPlaceholderContainer" runat="server" border="0" cellpadding="0" cellspacing="0" style="">
                                <tr id="Tr1" runat="server" style="text-align: left">
                                    <th id="Th2" runat="server" style="width: 30px">
                                        ID
                                    </th>
                                    <th id="Th8" runat="server" style="width: 120px">
                                        Classe
                                    </th>
                                    <th id="Th4" runat="server" style="width: 300px">
                                        Struttura
                                    </th>
                                    <th id="Th1" runat="server" style="width: 300px">
                                        Titolo Offerta
                                    </th>
                                    <th id="Th7" runat="server" style="width: 50px">
                                        Attivo?
                                    </th>
                                    <th id="Th9" runat="server" style="width: 50px">
                                        In home?
                                    </th>
                                    <th id="Th5" runat="server" style="width: 120px">
                                        Inizio Offerta
                                    </th>
                                    <th id="Th6" runat="server" style="width: 120px">
                                        Fine Offerta
                                    </th>
                                    <th id="Th3" runat="server">
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div class="nulla">
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
