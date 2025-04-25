<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_inout.ascx.cs" Inherits="RentalInRome.uc.UC_rnt_reservation_inout" %>
<asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" />
<h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
    <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
</h3>
<div class="nulla">
</div>
<span class="tit_sez">
    <%=CurrentSource.getSysLangValue("pdf_ArrivalAndDeparture")%></span>
<div class="nulla">
</div>
<div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
    <%=CurrentSource.getSysLangValue("reqRequestSent")%>
</div>
<div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
<div>
    <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
        <h3 id="errorMsgLbl">
            <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
        <p id="errorMsg">
            <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
        </p>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in place" style="padding-bottom: 5px; border-bottom-color: #333366;">
        <div id="Div1" class="left">
            <label class="desc" style="border:none; margin:0;">
            <h3>Arrival to Rome</h3>
            </label>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in place">
        <div id="drp_in_place_cont" class="left">
            <label>
                Arrival place
            </label>
            <div>
                <asp:DropDownList ID="drp_in_place" runat="server" onchange="checkArrivalDeparture()">
                    <asp:ListItem Value="" Text=""></asp:ListItem>
                    <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                    <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                    <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                    <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="Div18" class="left">
            <label>
                Arrival time
            </label>
            <div>
                <asp:DropDownList runat="server" ID="drp_limo_in_datetime_h" Style="width: 40px; margin-top: 2px; font-size: 11px;">
                </asp:DropDownList>
                :<asp:DropDownList runat="server" ID="drp_limo_in_datetime_m" Style="width: 40px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in air hide">
        <div id="drp_in_air_place_cont" class="left">
            <label>
                Airport name
            </label>
            <div>
                <asp:DropDownList ID="drp_in_air_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in air dett hide">
        <div id="Div3" class="left">
            <label>
                Airline / flight n.
            </label>
            <div>
                <asp:TextBox ID="txt_in_air_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in sea hide">
        <div id="Div2" class="left">
            <label>
                Seaport name
            </label>
            <div>
                <asp:DropDownList ID="drp_in_sea_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in sea dett hide">
        <div id="Div4" class="left">
            <label>
                Shipping company
            </label>
            <div>
                <asp:TextBox ID="txt_in_sea_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in train hide">
        <div id="Div5" class="left">
            <label>
                Railway stat. name
            </label>
            <div>
                <asp:DropDownList ID="drp_in_train_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in train dett hide">
        <div id="Div6" class="left">
            <label>
                Train n.
            </label>
            <div>
                <asp:TextBox ID="txt_in_train_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in other hide">
        <div id="Div7" class="left">
            <label>
                Other place
            </label>
            <div>
                <asp:TextBox ID="txt_in_other_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
        <div id="Div23" class="left">
            <label>
                <%=CurrentSource.getSysLangValue("pdf_info_orario_arrivo_appartamento", "Se non comunicato, il check-in verrà effettuato entro 1h30 dall’arrivo in aeroporto o 30 min. in stazione (non prima delle 11.30).")%>
            </label>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_in date">
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
                <%=CurrentSource.getSysLangValue("pdf_orario_arrivo_appartamento", "Orario d’arrivo all’appartamento")%>
            </label>
            <div>
                <asp:DropDownList runat="server" ID="drp_dtStartTime_h" Style="width: 40px; margin-top: 2px; font-size: 11px;">
                </asp:DropDownList>
                :<asp:DropDownList runat="server" ID="drp_dtStartTime_m" Style="width: 40px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                </asp:DropDownList>
            </div>
            <div runat="server" visible="false">
                <span id="in_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                <img id="in_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                <asp:HiddenField ID="HF_in_time_cont" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
        <div id="Div21" class="left">
            <label>
                <%= CurrentSource.getSysLangValue("terms_checkin")%>
            </label>
        </div>
        <div class="nulla">
        </div>
    </div>
    
    <div class="line mode_out place" style="padding-bottom: 5px; border-bottom-color:#333366;">
        <div id="Div9" class="left">
            <label class="desc" style="border: none; margin: 0;">
                <h3>Departure from Rome</h3>
            </label>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out place">
        <div id="drp_out_place_cont" class="left">
            <label>
                Departure place
            </label>
            <div>
                <asp:DropDownList ID="drp_out_place" runat="server" onchange="checkArrivalDeparture()">
                    <asp:ListItem Value="" Text=""></asp:ListItem>
                    <asp:ListItem Value="air" Text="Airport"></asp:ListItem>
                    <asp:ListItem Value="sea" Text="Seaport"></asp:ListItem>
                    <asp:ListItem Value="train" Text="Railway station"></asp:ListItem>
                    <asp:ListItem Value="other" Text="Other"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="Div19" class="left">
            <label>
                Departure time
            </label>
            <div>
                <asp:DropDownList runat="server" ID="drp_limo_out_datetime_h" Style="width: 40px; margin-top: 2px; font-size: 11px;">
                </asp:DropDownList>
                :<asp:DropDownList runat="server" ID="drp_limo_out_datetime_m" Style="width: 40px; margin-top: 2px; margin-left: 5px; font-size: 11px;">
                </asp:DropDownList>
            </div>
            <div id="Div20" runat="server" visible="false">
                <span id="Span1" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                <img id="Img1" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out air hide">
        <div id="drp_out_air_place_cont" class="left">
            <label>
                Airport name
            </label>
            <div>
                <asp:DropDownList ID="drp_out_air_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out air dett hide">
        <div id="Div10" class="left">
            <label>
                Airline / flight n.
            </label>
            <div>
                <asp:TextBox ID="txt_out_air_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out sea hide">
        <div id="Div11" class="left">
            <label>
                Seaport name
            </label>
            <div>
                <asp:DropDownList ID="drp_out_sea_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out sea dett hide">
        <div id="Div12" class="left">
            <label>
                Shipping company
            </label>
            <div>
                <asp:TextBox ID="txt_out_sea_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out train hide">
        <div id="Div13" class="left">
            <label>
                Railway stat. name
            </label>
            <div>
                <asp:DropDownList ID="drp_out_train_place" runat="server" onchange="checkArrivalDeparture()" CssClass="limoLarge">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out train dett hide">
        <div id="Div14" class="left">
            <label>
                Train n.
            </label>
            <div>
                <asp:TextBox ID="txt_out_train_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line mode_out other hide">
        <div id="Div15" class="left">
            <label>
                Other place
            </label>
            <div>
                <asp:TextBox ID="txt_out_other_dett" runat="server" CssClass="limoLarge"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366; padding-top: 5px; border-top-color: #333366;">
        <div id="Div24" class="left">
            <label>
                <%= CurrentSource.getSysLangValue("terms_checkout")%>
            </label>
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
            <div runat="server" visible="false">
                <span id="out_time_view" runat="server" title="click to select the time" style="cursor: pointer;"></span>
                <img id="out_time_toggler" src="images/ico/clock.png" alt="set" title="click to select the time" style="cursor: pointer;" /><br />
                <asp:HiddenField ID="HF_out_time_cont" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <asp:LinkButton ID="lnk_saveInOutData" CssClass="btn bonifico" runat="server" OnClick="lnk_saveInOutData_Click"><span>Submit</span></asp:LinkButton>
    <asp:Label ID="lbl_ok" runat="server" Text="OK" Style="margin: 10px 5px 5px 0pt; display: block; float: left; font-size: 20px; font-weight: bold; color: green;" Visible="false"></asp:Label>
    </div>
</div>
<div class="nulla">
</div>
