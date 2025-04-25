<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="ChnlRentalsUnitedInternsSubTypeList.aspx.cs" Inherits="RentalInRome.admin.modRental.ChnlRentalsUnitedInternsSubTypeList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="fascia1">
        <telerik:RadAjaxPanel ID="pnlFascia" runat="server" CssClass="pannello_fascia1" OnAjaxRequest="pnlFascia_AjaxRequest">
            <asp:HiddenField ID="HfId" Value="0" runat="server" />
            <div style="clear: both">
                <h1>RentalsUnited <%# ltrType.Text %> Sub Types</h1>
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
                                                    Room Sub Type:</label>
                                                <asp:DropDownList ID="drp_flt_type" runat="server" CssClass="inp" AutoPostBack="true" OnSelectedIndexChanged="drp_flt_type_SelectedIndexChanged">
                                                    <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Bed Room" Value="Bedroom"></asp:ListItem>
                                                    <asp:ListItem Text="Bath Room" Value="Bathroom"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
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
                <asp:ListView ID="LV" runat="server">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                                <asp:Label ID="lbl_id" runat="server" Text='<%#Eval("id") %>' Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
                            </td>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("code")%></span>
                                <asp:Label ID="lbl_id" runat="server" Text='<%#Eval("id") %>' Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drp_refId" runat="server"></asp:DropDownList>
                            </td>
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
                                <tr style="text-align: left">
                                    <th style="width: 250px">Room  SubType</th>
                                    <th>RentalsUnited Room SubType</th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <asp:Literal ID="ltrType" runat="server" Visible="false" Text=" Room "></asp:Literal>
        </telerik:RadAjaxPanel>
        <div class="nulla">
        </div>
    </div>
    <div class="nulla">
    </div>
</asp:Content>

