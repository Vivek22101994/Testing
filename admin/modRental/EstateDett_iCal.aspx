<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateDett_iCal.aspx.cs" Inherits="ModRental.admin.modRental.EstateDett_iCal" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        <style type="text/css">.main div.mainbox {width:100%;}</style>

            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />

            

            <h1 class="titolo_main">
                Esportazione della disponibilita in iCal:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>

            <img src="/admin/images/logo-google-cal.png" style="position:absolute; right: 30px; top: 190px;" />

            <div id="fascia1">
                <div class="tabsTop">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio" runat="server" visible="false">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click"><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span>Torna nel elenco</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox iCalMainBox">
                    
                    <div class="center">
                        <div class="boxmodulo">
                            <asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="ModRental.DCmodRental" TableName="dbRntChannelManagerTBLs" OrderBy="code" EntityTypeName="" Where="isActive==1">
                            </asp:LinqDataSource>
                            <asp:ListView ID="LV" runat="server" DataSourceID="LDS">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td valign="middle">
                                            <span>
                                                <%# Eval("title")%></span>
                                        </td>
                                        <td valign="middle" align="center">
                                            <span>
                                                <%# Eval("imgLogo") + "" != "" ? "<img alt='' src='/"+Eval("imgLogo")+"' style='max-height: 50px;' />" : ""%></span>
                                        </td>
                                        <td valign="middle">
                                            <input onfocus="this.select()" style="width: 500px;" value="<%= App.HOST%>/channelmanager/<%# Eval("code")%>/ical/get/<%= HF_IdEstate.Value%>.ics" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td valign="middle">
                                            <span>
                                                <%# Eval("title")%></span>
                                        </td>
                                        <td valign="middle" align="center">
                                            <span>
                                                <%# Eval("imgLogo") + "" != "" ? "<img alt='' src='/"+Eval("imgLogo")+"' style='max-height: 50px;' />" : ""%></span>
                                        </td>
                                        <td valign="middle">
                                            <input onfocus="this.select()" style="width: 500px;" value="<%= App.HOST%>/channelmanager/<%# Eval("code")%>/ical/get/<%= HF_IdEstate.Value%>.ics" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table id="Table1" runat="server" style="">
                                        <tr>
                                            <td>
                                                Non ci sono channel manager attive.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                   
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
