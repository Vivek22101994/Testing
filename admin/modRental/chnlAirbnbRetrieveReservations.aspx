<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="chnlAirbnbRetrieveReservations.aspx.cs" Inherits="MagaRentalCE.admin.modRental.chnlAirbnbRetrieveReservations" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />
            <%# rntUtils.getAgent_logoForDetailsPage(ChnlHomeAwayProps.IdAdMedia) %>

            <h1 class="titolo_main"><%= contUtils.getLabel("lbl_DettagliPosizioneApt")%>:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>           

            <div class="nulla">
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_retrieve_reservations" runat="server" OnClick="lnk_retrieve_reservations_Click"><span>Retrieve Reservations</span></asp:LinkButton>
                </div>                
                <div class="nulla">
                </div>
            </div> 
            <div class="nulla"></div>
            <div class="mainline mainChannels mainHolidayLettings mainPriceHL">
                <div class="mainbox iCalMainBox">
                    <div class="center">
                        <div class="boxmodulo">                          
                            <div class="nulla"></div>
                            <table>
                                <tr>
                                    <td>Host:
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drp_host" runat="server" style="width:auto;"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>From:
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadDatePicker ID="rdp_rate_dtStart" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>To:
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadDatePicker ID="rdp_rate_dtEnd" runat="server" Width="100px" MinDate="2000-01-01" MaxDate="2050-01-01">
                                            <DateInput DateFormat="dd/MM/yyyy">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Property:
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="drp_estates" runat="server" style="width:auto;"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="nulla"></div>
              
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
