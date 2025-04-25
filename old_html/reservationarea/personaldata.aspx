<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="personaldata.aspx.cs" Inherits="RentalInRome.reservationarea.personaldata" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Personal Data</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function rwdDettClose() {
                $find("<%= pnlSendMailWithVoucher.ClientID%>").ajaxRequest('sendmail');
                return false;
            }
            function confirmSendMailWithVoucher_CallBackFn(arg) {
                if ("" + arg == "true") rwdDettClose()
            } 
            function confirmSendMailWithVoucher() {
                radconfirm('Your personal data has been changed.<br/>Want to receive your voucher via e-mail?', confirmSendMailWithVoucher_CallBackFn, 330, 100, null, 'Data was successfully saved.', "");
            }
        </script>
    </telerik:RadCodeBlock> 
    <telerik:RadAjaxPanel ID="pnlSendMailWithVoucher" runat="server" OnAjaxRequest="pnlSendMailWithVoucher_AjaxRequest">
    </telerik:RadAjaxPanel>
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="Hf_nameFull" runat="server" Value="" Visible="false" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="contatti">
        <div class="sx">
            <h3 class="underlined" style="margin-bottom: 20px; margin-left: 8px;">
                <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <%=CurrentSource.getSysLangValue("lblYourReservation")%>
                </telerik:RadCodeBlock>
            </h3>
            <div id="infoCont">
                <div class="infoBox" style="font-size: 11px;">
                    <uc1:uc_sx id="UC_sx1" runat="server" />
                </div>
            </div>
        </div>
        <div class="dx reservation_details_dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
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
                        <div class="line" style="padding-bottom: 5px; border-bottom-color: #333366;">
                            <div class="left">
                                <label class="desc">
                                    The Undersigned:
                                    <strong style="font-size: 14px; display: block;"><%= Hf_nameFull.Value %></strong>
                                </label>
                            </div>
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
                                    <span id="txt_birth_place_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <asp:DropDownList ID="drp_doc_type" runat="server" Style="width: 150px;">
                                    </asp:DropDownList>
                                    <span id="drp_doc_type_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="txt_doc_num_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_Num_Documento")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_doc_num" runat="server" Style="width: 300px;"></asp:TextBox>
                                    <span id="txt_doc_num_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="Div4" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblDocIssueDate")%>* </label>
                                <div>
                                    <input type="text" id="txt_doc_issue_date" style="width: 142px" />
                                    <asp:HiddenField ID="HF_doc_issue_date" runat="server" />
                                    <span id="val_doc_issue_date" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="txt_doc_issue_place_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_doc_issue_place" runat="server" MaxLength="50"></asp:TextBox>
                                    <span id="txt_doc_issue_place_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <span id="txt_loc_address_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="txt_loc_state_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblRegionState")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_loc_state" runat="server" MaxLength="50"></asp:TextBox>
                                    <span id="txt_loc_state_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="txt_loc_zip_code_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblZipCode")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_loc_zip_code" runat="server" MaxLength="10"></asp:TextBox>
                                    <span id="txt_loc_zip_code_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <span id="txt_loc_city_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="Div1" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqLocation")%>*
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 350px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                    </asp:DropDownList>
                                    <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                        <WhereParameters>
                                            <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <span id="drp_country_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="txt_contact_phone_mobile_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("reqPhoneNumber")%>*
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_contact_phone_mobile" runat="server"></asp:TextBox>
                                    <span id="txt_contact_phone_mobile_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div id="txt_contact_phone_trip_cont" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("lblCellulare_Viaggio")%>
                                </label>
                                <div>
                                    <asp:TextBox ID="txt_contact_phone_trip" runat="server"></asp:TextBox>
                                    <span id="txt_contact_phone_trip_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <span id="txt_contact_phone_check" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                    <span id="txt_contact_email_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;"></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border: none;">
                            <div id="Div3" class="left">
                                <label class="desc">
                                    <%=CurrentSource.getSysLangValue("formVatNumber")%></label>
                                <div>
                                    <asp:TextBox ID="txt_doc_vat_num" runat="server" MaxLength="50" Style="width: 235px;"></asp:TextBox>
                                </div>
                            </div>
                            <div id="Div2" class="left">
                                <label class="desc">Codice Fiscale (Only for Italy)* </label>
                                <div>
                                    <asp:TextBox ID="txt_doc_cf_num" runat="server" MaxLength="50" Style="width: 235px;"></asp:TextBox>
                                    <span id="val_doc_cf_num" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                        <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                </div>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="line" style="border-top: 1px dashed #333366; border-bottom: 1px dashed #333366; padding: 10px 0;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="left" style="margin-bottom: 10px;">
                                        <label class="desc">
                                            <h3><%=CurrentSource.getSysLangValue("lblInvoicedetails")%></h3>
                                            <%=CurrentSource.getSysLangValue("lblInvoicedetailsdown")%>
                                            <asp:DropDownList ID="drp_inv_isDifferent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_inv_isSameAsPersData_SelectedIndexChanged" Style="width: 70px;">
                                                <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </label>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <asp:PlaceHolder ID="PH_invDetails" runat="server">
                                    <div id="Div10" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("reqFullName")%>* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_name_full" runat="server" MaxLength="100" Style="width: 300px;"></asp:TextBox>
                                            <span id="val_inv_name_full" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div id="Div5" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("lblAddress")%>* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_loc_address" runat="server" MaxLength="300" Style="width: 300px;"></asp:TextBox>
                                            <span id="val_inv_loc_address" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div id="Div7" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("lblZipCode")%>* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_loc_zip_code" runat="server" MaxLength="10"></asp:TextBox>
                                            <span id="val_inv_loc_zip_code" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div id="Div8" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("lblCity")%>* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_loc_city" runat="server" MaxLength="50"></asp:TextBox>
                                            <span id="val_inv_loc_city" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div id="Div6" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("lblRegionState")%>* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_loc_state" runat="server" MaxLength="50"></asp:TextBox>
                                            <span id="val_inv_loc_state" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div id="Div9" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("reqLocation")%>* </label>
                                        <div>
                                            <asp:DropDownList runat="server" ID="drp_inv_loc_country" CssClass="field select large" Style="width: 350px;" OnDataBound="drp_inv_loc_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="title" TabIndex="5">
                                            </asp:DropDownList>
                                            <span id="val_inv_loc_country" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    <div id="Div11" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("formVatNumber")%></label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_doc_vat_num" runat="server" MaxLength="50" Style="width: 235px;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="Div12" class="left">
                                        <label class="desc">Codice Fiscale (Only for Italy)* </label>
                                        <div>
                                            <asp:TextBox ID="txt_inv_doc_cf_num" runat="server" MaxLength="50" Style="width: 235px;"></asp:TextBox>
                                            <span id="val_inv_doc_cf_num" class="alertErrorSmall" style="width: 160px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                    </asp:PlaceHolder>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:LinkButton ID="lnk_saveClientData" CssClass="btn bonifico" runat="server" OnClick="lnk_save_Click" OnClientClick="return RNT_validateRequestForm()"><span><%=CurrentSource.getSysLangValue("lblSubmit")%></span></asp:LinkButton>
                        <asp:Label ID="lbl_ok" runat="server" Text="OK" Style="margin: 10px 5px 5px 0pt; display: block; float: left; font-size: 20px; font-weight: bold; color: green;" Visible="false"></asp:Label>
                    </div>
                    <div class="nulla">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            $("#errorLi").hide();

            $("#txt_birth_place_check").hide();
            $("#<%= txt_birth_place.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_birth_place.ClientID%>").val()) == "") {
                $("#<%= txt_birth_place.ClientID%>").addClass(FORM_errorClass);
                $("#txt_birth_place_check").css("display", "block");
                _validate = false;
            }

            $("#drp_doc_type_check").hide();
            $("#<%= drp_doc_type.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= drp_doc_type.ClientID%>").val()) == "") {
                $("#<%= drp_doc_type.ClientID%>").addClass(FORM_errorClass);
                $("#drp_doc_type_check").css("display", "block");
                _validate = false;
            }

            $("#txt_doc_num_check").hide();
            $("#<%= txt_doc_num.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_doc_num.ClientID%>").val()) == "") {
                $("#<%= txt_doc_num.ClientID%>").addClass(FORM_errorClass);
                $("#txt_doc_num_check").css("display", "block");
                _validate = false;
            }

            $("#txt_doc_issue_place_check").hide();
            $("#<%= txt_doc_issue_place.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_doc_issue_place.ClientID%>").val()) == "") {
                $("#<%= txt_doc_issue_place.ClientID%>").addClass(FORM_errorClass);
                $("#txt_doc_issue_place_check").css("display", "block");
                _validate = false;
            }
            $("#val_doc_issue_date").hide();
            $("#txt_doc_issue_date").removeClass(FORM_errorClass);
            if ($.trim($("#<%= HF_doc_issue_date.ClientID%>").val()) == "" || $.trim($("#<%= HF_doc_issue_date.ClientID%>").val()) == "0" || $.trim($("#<%= HF_doc_issue_date.ClientID%>").val()) == "00000000") {
                $("#txt_doc_issue_date").addClass(FORM_errorClass);
                $("#val_doc_issue_date").css("display", "block");
                _validate = false;
            }

            $("#txt_loc_address_check").hide();
            $("#<%= txt_loc_address.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_loc_address.ClientID%>").val()) == "") {
                $("#<%= txt_loc_address.ClientID%>").addClass(FORM_errorClass);
                $("#txt_loc_address_check").css("display", "block");
                _validate = false;
            }

            $("#txt_loc_state_check").hide();
            $("#<%= txt_loc_state.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_loc_state.ClientID%>").val()) == "") {
                $("#<%= txt_loc_state.ClientID%>").addClass(FORM_errorClass);
                $("#txt_loc_state_check").css("display", "block");
                _validate = false;
            }

            $("#txt_loc_zip_code_check").hide();
            $("#<%= txt_loc_zip_code.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_loc_zip_code.ClientID%>").val()) == "") {
                $("#<%= txt_loc_zip_code.ClientID%>").addClass(FORM_errorClass);
                $("#txt_loc_zip_code_check").css("display", "block");
                _validate = false;
            }


            $("#txt_loc_city_check").hide();
            $("#<%= txt_loc_city.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_loc_city.ClientID%>").val()) == "") {
                $("#<%= txt_loc_city.ClientID%>").addClass(FORM_errorClass);
                $("#txt_loc_city_check").css("display", "block");
                _validate = false;
            }

            $("#txt_contact_phone_mobile_check").hide();
            $("#<%= txt_contact_phone_mobile.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_contact_phone_mobile.ClientID%>").val()) == "") {
                $("#<%= txt_contact_phone_mobile.ClientID%>").addClass(FORM_errorClass);
                $("#txt_contact_phone_mobile_check").css("display", "block");
                _validate = false;
            }

            $("#txt_contact_phone_check").hide();
            $("#<%= txt_contact_phone.ClientID%>").removeClass(FORM_errorClass);
            if ($.trim($("#<%= txt_contact_phone.ClientID%>").val()) == "") {
                $("#<%= txt_contact_phone.ClientID%>").addClass(FORM_errorClass);
                $("#txt_contact_phone_check").css("display", "block");
                _validate = false;
            }

            $("#txt_contact_email_check").hide();
            if ($.trim($("#<%= txt_contact_email.ClientID%>").val()) == "") {
                $("#<%= txt_contact_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_contact_email_check").html('This field is required. Please enter a value.');
                $("#txt_contact_email_check").css("display", "block");
                _validate = false;
            }
            else if (!FORM_validateEmail($("#<%= txt_contact_email.ClientID%>").val())) {
                $("#<%= txt_contact_email.ClientID%>").addClass(FORM_errorClass);
                $("#txt_contact_email_check").html('Please enter a valid email address.');
                $("#txt_contact_email_check").css("display", "block");
                _validate = false;
            }
      
            $("#val_doc_cf_num").hide();
            $("#<%= txt_doc_cf_num.ClientID%>").removeClass(FORM_errorClass);
            if ($("#<%= drp_country.ClientID%>").val() == "Italy" && $.trim($("#<%= txt_doc_cf_num.ClientID%>").val()) == "") {
                $("#<%= txt_doc_cf_num.ClientID%>").addClass(FORM_errorClass);
                $("#val_doc_cf_num").css("display", "block");
                _validate = false;
            }

            if ($("#<%= drp_inv_isDifferent.ClientID%>").val() == "1") {

                $("#val_inv_name_full").hide();
                $("#<%= txt_inv_name_full.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= txt_inv_name_full.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_name_full.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_name_full").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_loc_state").hide();
                $("#<%= txt_inv_loc_state.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= txt_inv_loc_state.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_loc_state.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_loc_state").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_loc_city").hide();
                $("#<%= txt_inv_loc_city.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= txt_inv_loc_city.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_loc_city.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_loc_city").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_loc_address").hide();
                $("#<%= txt_inv_loc_address.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= txt_inv_loc_address.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_loc_address.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_loc_address").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_loc_zip_code").hide();
                $("#<%= txt_inv_loc_zip_code.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= txt_inv_loc_zip_code.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_loc_zip_code.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_loc_zip_code").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_loc_country").hide();
                $("#<%= drp_inv_loc_country.ClientID%>").removeClass(FORM_errorClass);
                if ($.trim($("#<%= drp_inv_loc_country.ClientID%>").val()) == "") {
                    $("#<%= drp_inv_loc_country.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_loc_country").css("display", "block");
                    _validate = false;
                }

                $("#val_inv_doc_cf_num").hide();
                $("#<%= txt_inv_doc_cf_num.ClientID%>").removeClass(FORM_errorClass);
                if ($("#<%= drp_inv_loc_country.ClientID%>").val() == "Italy" && $.trim($("#<%= txt_inv_doc_cf_num.ClientID%>").val()) == "") {
                    $("#<%= txt_inv_doc_cf_num.ClientID%>").addClass(FORM_errorClass);
                    $("#val_inv_doc_cf_num").css("display", "block");
                    _validate = false;
                }
            }

            if (!_validate) {
                $("#errorLi").css("display", "block");
                $.scrollTo($("#errorLi"), 500);
            }
            return _validate;
        }
    </script>
    </telerik:RadCodeBlock>
</asp:Content>
