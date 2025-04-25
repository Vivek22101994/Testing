<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="transfer.aspx.cs" Inherits="RentalInRome.reservationarea.transfer" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Transfer</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_id" runat="server" Visible="false" />
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
        <div class="dx" id="transfert">
            <h3 style="margin-bottom: 20px; margin-left: 0;" class="underlined">Servizi</h3>

            <div class="box_reservation_gen">
                <div class="linetitpuls">
                    <span class="tit1">Servizio</span>
                    <span class="tit2">TRANSFERT</span>
                </div>
                <asp:ListView runat="server" ID="LV">
                    <ItemTemplate>
                        <li>
                            <div class="lasx" style="background-image: url(/images/arearis_transf_van.jpg);">
                                <span class="titauto1">
                                    <%# Eval("title") %> <%# Eval("tripDirection") %></span>
                                <span class="titauto2">
                                    <%# "min " + Eval("numGuestMin") + " pers"%><br />
                                    <%# "max " + Eval("numGuestMax") + " pers"%>
                                </span>
                            </div>
                            <div class="ladx">
                                <span class="ladxtx">
                                    <%# Eval("prTotal") %> Lorem ipsum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor...</span>
                                <a href="#" class="ric_serv">&gt; Richiedi servizio</a>
                            </div>
                            <div class="nulla">
                            </div>
                        </li>
                        <asp:Label runat="server" ID="lbl_id" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label1" Text='<%# Eval("tripDirection") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("pax") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label3" Text='<%# Eval("prSingle") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label4" Text='<%# Eval("prTotal") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label5" Text='<%# Eval("code") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label6" Text='<%# Eval("title") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label7" Text='<%# Eval("imgPreview") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label8" Text='<%# Eval("numGuestMin") %>' Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="Label9" Text='<%# Eval("numGuestMax") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <ul class="listaauto">
                            <li id="itemPlaceholder" runat="server"/>
                        </ul>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <div class="nulla">
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
                <ul class="listaauto">
                    <li>
                        <div class="lasx" style="background-image:url(/images/arearis_transf_mercedes.jpg);">
                            <span class="titauto1">Transfert NCC</span>
                            <span class="titauto2">pulminio condiviso<br />max 5 pers.</span>
                        </div>
                        <div class="ladx">
                            <span class="ladxtx">Lorem ipsum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor...</span>
                            <a href="#" class="ric_serv">&gt; Richiedi servizio</a>
                        </div>
                        <div class="nulla"></div>
                    </li>
                    <!-- SERIVIZIO GIA PRENOTATO -->
                    <li>
                        <div class="lasx" style="background-image:url(/images/arearis_transf_van.jpg);">
                            <span class="titauto1">Transfert NCC</span>
                            <span class="titauto2">pulminio condiviso<br />max 8 pers.</span>
                            <!-- AGGIUNTO DIV -->
                            <div class="servprenotato">
                                Servizio già prenotato
                            </div>
                        </div>
                        <div class="ladx">
                            <span class="ladxtx">Lorem ipsum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor...</span>
                            <!-- NO DEVE ESSERCI LA A -->
                        </div>
                        <div class="nulla"></div>
                    </li>
                </ul>

            </div>


            <div class="box_reservation_gen">
                <div class="linetitpuls">
                    <span class="tit1">Servizio</span>
                    <span class="tit2">Tour e Visite Guidate</span>
                </div>
                <ul class="listavisite">
                    <li>
                        <div class="lasx">
                           <div class="contfoto"><img src="../images/css/colosseo.jpg" /></div>
                        </div>
                        <div class="ladx">
                            <div class="ladxtit">
                                <span class="titauto1">Castelli Romani e le sue tradizioni</span>
                                <span class="titauto2">Viaggio nelle stupende terre intorno a roma -  max 8 pers. - 2 h</span>
                            </div>
                            <span class="ladxtx">Lorem ipsum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet...</span>
                            
                            <div style="">
                                <div class="icone">
                                    //icone
                                </div>
                                <div style="float: right;">
                                <a href="#" class="ric_serv">&gt; Richiedi servizio</a>
                                <!--div class="servprenotato">
                                    Servizio già prenotato
                                </div-->
                                </div>
                                
                            </div>

                        </div>
                        <div class="nulla"></div>
                    </li>
                    <!-- SERIVIZIO GIA PRENOTATO -->
                    <li>
                        <div class="lasx">
                           <div class="contfoto"><img src="../images/css/colosseo.jpg" /></div>
                        </div>
                        <div class="ladx">
                            <div class="ladxtit">
                                <span class="titauto1">Castelli Romani e le sue tradizioni</span>
                                <span class="titauto2">Viaggio nelle stupende terre intorno a roma -  max 8 pers. - 2 h</span>
                            </div>
                            <span class="ladxtx">Lorem ipsum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet lorem ispum dolor sit amet...</span>
                            
                            <div style="">
                                <div class="icone">
                                    //icone
                                </div>
                                <div style="float: right;">
                                <!--a href="#" class="ric_serv">&gt; Richiedi servizio</a-->
                                <div class="servprenotato">
                                    Servizio già prenotato
                                </div>
                                </div>
                                
                            </div>

                        </div>
                        <div class="nulla"></div>
                    </li>
                </ul>

            </div>


            <div class="nulla"></div>
        </div>
    </div>
</asp:Content>
