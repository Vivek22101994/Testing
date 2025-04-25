<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="chnlAirbnbRetrieveReservation.aspx.cs" Inherits="MagaRentalCE.admin.modRental.chnlAirbnbRetrieveReservation" %>
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
                    <asp:LinkButton ID="lnk_retrieve_reservation" runat="server" OnClick="lnk_retrieve_reservation_Click"><span>Retrieve Reservation</span></asp:LinkButton>
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
                                    <td>Airbnb Confirmation Code:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID ="txt_confirmation_code" runat="server"></asp:TextBox>
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
