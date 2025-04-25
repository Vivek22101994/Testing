<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlAirbnbAmenity.aspx.cs" Inherits="MagaRentalCE.admin.modRental.ChnlAirbnbAmenity" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../modContent/UCgetImg.ascx" TagName="UCgetImg" TagPrefix="getImg" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>Airbnb Feature Values</h1>
                <div class="salvataggio saveAgencyDetails">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click"><span><%# contUtils.getLabel("lblSaveChanges")%></span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>                   

                </div>
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
                        <table border="0" cellpadding="0" cellspacing="0" id="tbl_new">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <label>
                                                    Code:</label>
                                                <asp:TextBox ID="txt_flt_code" runat="server" CssClass="inp"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="bottom">
                                    <asp:LinkButton ID="lnk_flt" CssClass="ricercaris" runat="server" OnClick="lnk_flt_Click"><span><%# contUtils.getLabel("lblFiltraIRisultati")%></span></asp:LinkButton>
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
            <div class="nulla">
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV_cat" runat="server">
                    <ItemTemplate>
                        <tr>
                            <th><asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("id") %>' />
                                <%#Eval("code") %></th>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                                    <ItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("code")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("description")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_code" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                                <asp:Label ID="lbl_refId" Visible="false" runat="server" Text='<%# Eval("refId") %>' />
                                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td>
                                                <span>
                                                    <%# Eval("code")%></span>
                                            </td>
                                            <td>
                                                <span>
                                                    <%# Eval("description")%></span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_code" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                                <asp:Label ID="lbl_refId" Visible="false" runat="server" Text='<%# Eval("refId") %>' />
                                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>No data was returned.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table_fascia">
                                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                               <%-- <tr style="text-align: left">
                                                    <th style="width: 250px">Code</th>
                                                    <th></th>
                                                    <th></th>
                                                </tr>--%>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </div>
                                    </LayoutTemplate>
                                </asp:ListView>
                            </td>
                        </tr>

                    </ItemTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="">
                                <%-- <tr style="text-align: left">
                                    <th style="width: 250px">Code</th>
                                    <th></th>
                                </tr>--%>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>

            </div>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
</asp:Content>
