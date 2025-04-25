<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="pickupservice.aspx.cs" Inherits="RentalInRome.reservationarea.pickupservice" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Pickup Service</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:UC_sx ID="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
                    </h3>
                    <div class="nulla">
                    </div>
                    <span class="tit_sez">Servizi Aggiuntivi</span>
                    <div class="nulla">
                    </div>
                    <div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
                        <div>
                            <div class="line mode_in date">
                                <div id="Div3" class="left">
                                    <label class="desc" style="border-bottom: 1px dotted #333366; color: #333366; display: block; font-size: 13px; font-weight: bold; margin-bottom: 10px; padding-bottom: 5px; width: 545px;">
                                        Times
                                    </label>
                                </div>
                                <div class="nulla">
                                </div>
                                <div id="txt_dtBirth_cont" class="left">
                                    <label>
                                        Check-In date
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txt_dtStart" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="Div8" class="left">
                                    <label>
                                        Check-In time
                                    </label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="drp_dtStartTime_h" Style="width: 40px; margin-top: 2px; font-size: 11px;">
                                        </asp:DropDownList>
                                        :<asp:DropDownList runat="server" ID="drp_dtStartTime_m" Style="width: 40px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="Div1" runat="server" visible="false">
                                        <span id="in_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                                        <img id="in_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                                        <asp:HiddenField ID="HF_in_time_cont" runat="server" />
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="line mode_out date">
                                <div id="Div16" class="left">
                                    <label>
                                        Check-Out date
                                    </label>
                                    <div>
                                        <asp:TextBox ID="txt_dtEnd" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="Div17" class="left">
                                    <label>
                                        Check-Out time
                                    </label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="drp_dtEndTime_h" Style="width: 40px; margin-top: 2px; font-size: 11px;">
                                        </asp:DropDownList>
                                        :<asp:DropDownList runat="server" ID="drp_dtEndTime_m" Style="width: 40px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="Div2" runat="server" visible="false">
                                        <span id="out_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                                        <img id="out_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                                        <asp:HiddenField ID="HF_out_time_cont" runat="server" />
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="line">
                                <div id="Div20" class="left">
                                    <label class="desc" style="border-bottom: 1px dotted #333366; color: #333366; display: block; font-size: 13px; font-weight: bold; margin-bottom: 10px; padding-bottom: 5px; width: 545px;">
                                        Luggage
                                    </label>
                                </div>
                                <div class="nulla">
                                </div>
                                <div class="left" style="margin-right: 30px;">
                                    <label>
                                        Large suitecases
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_num_case_l" runat="server" Width="50">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="left" style="margin-right: 30px;">
                                    <label>
                                        Medium suitecases
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_num_case_m" runat="server" Width="50">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="left">
                                    <label>
                                        Small suitecases
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="drp_num_case_s" runat="server" Width="50">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <div class="nulla">
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                            <a class="btn bonifico">
                                <span>Submit</span>
                            </a>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
