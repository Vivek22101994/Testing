<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_rnt_reservation_client.ascx.cs" Inherits="RentalInRome.uc.UC_rnt_reservation_client" %>
<asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
<asp:HiddenField ID="HF_IdReservation" runat="server" Value="0" />
<h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
    <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
</h3>
<div class="nulla">
</div>
<span class="tit_sez">
    <%=CurrentSource.getSysLangValue("pdf_CustomerData")%></span>
<div class="nulla">
</div>
<div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
    <%=CurrentSource.getSysLangValue("reqRequestSent")%>
</div>
<div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;">
    <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
        <h3 id="errorMsgLbl">
            <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
        <p id="errorMsg">
            <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
        </p>
        <div class="nulla">
        </div>
    </div>
    <div class="line" style="border: none;">
        <div id="txt_locBirth_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_Nato_a")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_birth_place" runat="server" MaxLength="50" Style="width: 300px;"></asp:TextBox>
            </div>
        </div>
        <div id="txt_dtBirth_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_Data_di_Nascita")%>
            </label>
            <div>
                <input type="text" id="txt_birth_date" style="width: 142px" />
                <asp:HiddenField ID="HF_birth_date" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    
    <div class="line">
    </div>
    <div class="line" style="border: none;">
        <div id="drp_doc_type_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>*
            </label>
            <div>
                <asp:DropDownList ID="drp_doc_type" runat="server" Style="margin-right: 10px; width: 310px;">
                </asp:DropDownList>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    <div class="line" style="border: none;">
        <div id="txt_doc_num_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_Num_Documento")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_doc_num" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="txt_doc_issue_place_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_doc_issue_place" runat="server" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div id="txt_doc_expiry_date_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza")%>*
            </label>
            <div>
                <input type="text" id="txt_doc_expiry_date" style="width: 142px" />
                <asp:HiddenField ID="HF_doc_expiry_date" runat="server" />
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    
    <div class="line">
    </div>
    <div class="line" style="border: none;">
        <div id="txt_loc_address_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("pdf_IndirizzoResidenza")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_loc_address" runat="server" MaxLength="300"></asp:TextBox>
            </div>
        </div>
        <div id="txt_loc_state_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("lblRegionState")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_loc_state" runat="server" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div id="txt_loc_zip_code_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("lblZipCode")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_loc_zip_code" runat="server" MaxLength="10"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    
  
    <div class="line" style="border: none;">
        <div id="txt_loc_city_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("lblCity")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_loc_city" runat="server" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div id="txt_contact_phone_mobile_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_contact_phone_mobile" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="txt_contact_phone_trip_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("lblCellulare_Viaggio")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_contact_phone_trip" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    
    <div class="line" style="border: none;">
        <div id="txt_contact_phone_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("lblPhone")%>*
            </label>
            <div>
                <asp:TextBox ID="txt_contact_phone" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="txt_contact_fax_cont" class="left">
            <label class="desc">
                Fax
            </label>
            <div>
                <asp:TextBox ID="txt_contact_fax" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="txt_contact_email_cont" class="left">
            <label class="desc">
                <%=CurrentSource.getSysLangValue("reqEmail")%>
            </label>
            <div>
                <asp:TextBox ID="txt_contact_email" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="nulla">
        </div>
    </div>
    
    <asp:LinkButton ID="lnk_saveClientData" CssClass="btn bonifico" runat="server" OnClick="lnk_saveClientData_Click"><span>Submit</span></asp:LinkButton>
</div>
<div class="nulla">
</div>
