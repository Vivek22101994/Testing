<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="bedselection.aspx.cs" Inherits="RentalInRome.reservationarea.bedselection" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Bed selection</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="HF_IdEstate" runat="server" Value="" Visible="false" />
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
                    <asp:Literal ID="ltr_error" runat="server" Visible="false"></asp:Literal>
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
                    </h3>
                    <div class="nulla">
                    </div>
                    <span class="tit_sez"><%=CurrentSource.getSysLangValue("lblBedSelection")%></span>
                    <div class="nulla">
                    </div>
                    <div id="riepilogoLetti" class="box_client_booking">
                        <div class="titflag"><div><span><%=CurrentSource.getSysLangValue("rntBedSelectionTitle")%></span></div></div>
                        <%=CurrentSource.getSysLangValue("rntBedSelectionDesc")%>
                        <div class="line" style="margin-top:15px;"></div>
                        <strong style="font-size:14px; font-style:italic;"><%=CurrentSource.getSysLangValue("lblRiepilogo")%></strong>
                        <div class="line"></div>
                        <ul class="riepilogo">
                            <%= currBedsConfig.bedDouble > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("lblBedDouble") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedDouble + "</span></li>" : ""%>
                            <%= currBedsConfig.bedDoubleD > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("lblBedDoubleDivisible") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedDoubleD + "</span></li>" : ""%>
                            <%= currBedsConfig.bedSingle > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("lblBedSingle") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedSingle + "</span></li>" : ""%>
                            <%= currBedsConfig.bedSofaDouble > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("lblSofaBedDouble") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedSofaDouble + "</span></li>" : ""%>
                            <%= currBedsConfig.bedSofaSingle > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("lblSofaBedSingle") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedSofaSingle + "</span></li>" : ""%>
                            <%= currBedsConfig.bedDouble2level > 0 ? "<li><span class=\"nomeLetto\">" + CurrentSource.getSysLangValue("letto a castello") + "</span><span class=\"numPersLetto\">x" + currBedsConfig.bedDouble2level + "</span></li>" : ""%>
                            <li style="padding-top: 15px">
                                <span class="nomeLetto" style="font-size: 13px;">Min <%=CurrentSource.getSysLangValue("lblNumPersons")%>:</span>
                                <span class="numPersLetto" style="font-size: 18px; font-weight: bold;"><%= currBedsConfig.persMin%></span>
                            </li>
                            <li style="padding-top:15px">
                                 <span class="nomeLetto" style="font-size:13px;">Max <%=CurrentSource.getSysLangValue("lblNumPersons")%>:</span>
                                 <span class="numPersLetto" style="font-size:18px; font-weight:bold; "><%= currBedsConfig.persMax%></span>
                            </li>
                        </ul>
                        <asp:LinkButton ID="lnk_save" CssClass="btn bonifico" runat="server" OnClick="lnk_save_Click"><span><%=CurrentSource.getSysLangValue("lblSubmit")%></span></asp:LinkButton>
                    </div>

                    <div id="listaLetti">
                        <div class="letto" id="pnl_bedDouble" runat="server">
                            <img src="/images/css/letto-matrimonale-standard.gif" alt="<%=CurrentSource.getSysLangValue("lblBedDouble")%>" />
                            <span class="titLetto">
                                <%=CurrentSource.getSysLangValue("lblBedDouble")%>
                            </span>
                            <div class="numBedCont">
                                <span class="txtLetto">
                                    <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                </span>
                                <asp:DropDownList ID="drp_bedDouble" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div class="letto" id="pnl_bedDoubleD" runat="server">
                            <img src="/images/css/letto-matrimonale-divisibile.gif" alt="<%=CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>" />
                            <span class="titLetto">
                                <%=CurrentSource.getSysLangValue("lblBedDoubleDivisible")%>
                            </span>
                            <div class="numBedCont" id="pnl_bedDoubleDConfig" runat="server">
                               <span class="txtLetto"><%=CurrentSource.getSysLangValue("rntWantSeparated")%></span>
                                <asp:DropDownList ID="drp_bedDoubleDConfig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="numBedCont" style="">
                                 <span class="txtLetto">
                                     <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                 </span>
                                <asp:DropDownList ID="drp_bedDoubleD" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            
                        </div>

                         <div class="letto" id="pnl_bedSingle" runat="server">
                            <img src="/images/css/letto-singolo.gif" alt="<%=CurrentSource.getSysLangValue("lblBedSingle")%>" />
                             <span class="titLetto">
                                 <%=CurrentSource.getSysLangValue("lblBedSingle")%>
                             </span>
                            <div class="numBedCont">
                                <span class="txtLetto">
                                    <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                </span>
                                <asp:DropDownList ID="drp_bedSingle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="letto" id="pnl_bedSofaDouble" runat="server">
                            <img src="/images/css/divano-letto.gif" alt="<%=CurrentSource.getSysLangValue("lblSofaBedDouble")%>" />
                            <span class="titLetto">
                                <%=CurrentSource.getSysLangValue("lblSofaBedDouble")%>
                            </span>
                            <div class="numBedCont">
                                <span class="txtLetto">
                                    <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                </span>
                                <asp:DropDownList ID="drp_bedSofaDouble" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="letto" id="pnl_bedSofaSingle" runat="server">
                            <img src="/images/css/poltrona-letto.gif" alt="<%=CurrentSource.getSysLangValue("lblSofaBedSingle")%>" />
                            <span class="titLetto">
                                <%=CurrentSource.getSysLangValue("lblSofaBedSingle")%>
                            </span>
                            <div class="numBedCont">
                                <span class="txtLetto">
                                    <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                </span>
                                <asp:DropDownList ID="drp_bedSofaSingle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                         <div class="letto" id="pnl_bedDouble2level" runat="server">
                            <img src="/images/css/letto-castello.gif" alt="letto a castello" />
                            <span class="titLetto">letto a castello</span>
                            <div class="numBedCont">
                                <span class="txtLetto">
                                    <%=CurrentSource.getSysLangValue("rntHowManyBeds")%>
                                </span>
                                <asp:DropDownList ID="drp_bedDouble2level" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpBeds_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
