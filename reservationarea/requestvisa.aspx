<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/masterPage.Master" AutoEventWireup="true" CodeBehind="requestvisa.aspx.cs" Inherits="RentalInRome.reservationarea.requestvisa" %>

<%@ Register Src="UC_sx.ascx" TagName="UC_sx" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area - Visa Request</title>
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
                    <asp:HiddenField ID="HF_visa_persons" runat="server" Value="0" />
                    <asp:HiddenField ID="HF_controls" runat="server" Value="0" />
                    <h3 class="underlined" style="margin-bottom: 20px; margin-left: 0;">
                        <%=CurrentSource.getSysLangValue("lblMissingDataToTheReservation")%>
                    </h3>
                    <div class="nulla">
                    </div>
                    <span class="tit_sez">
                        Visa Information   
                    </span>
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
                        <asp:ListView ID="LV" runat="server" OnItemDataBound="LV_ItemDataBound">
                            <ItemTemplate>
                                <div class="line" style="border: none;">
                                    <div id="Div1" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("reqFullName")%>*
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_name_full" runat="server" Style="margin-right: 10px; width: 310px;"></asp:TextBox>
                                            <span class="alertErrorSmall txt_name_full_check_<%# Eval("sequence") %>" style="width: 300px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <div class="line" style="border: none;">
                                    <div id="drp_doc_type_cont" class="left">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("pdf_Documento_d_identita")%>*
                                        </label>
                                        <div>
                                            <asp:DropDownList ID="drp_doc_type" runat="server" Style="margin-right: 10px; width: 310px;">
                                            </asp:DropDownList>
                                            <span class="alertErrorSmall drp_doc_type_check_<%# Eval("sequence") %>" style="width: 300px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
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
                                            <asp:TextBox ID="txt_doc_num" runat="server" Style="margin-right: 10px; width: 310px;"></asp:TextBox>
                                            <span class="alertErrorSmall txt_doc_num_check_<%# Eval("sequence") %>" style="width: 300px; float: none; display: none;">
                                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                                        </div>
                                    </div>
                                    <div id="txt_doc_issue_place_cont" class="left" runat="server" visible="false">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("pdf_RilasciatoDal")%>*
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_doc_issue_place" runat="server" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="txt_doc_expiry_date_cont" class="left" runat="server" visible="false">
                                        <label class="desc">
                                            <%=CurrentSource.getSysLangValue("pdf_Data_di_scadenza")%>*
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txt_doc_expiry_date" runat="server" Style="width: 142px"></asp:TextBox>
                                            <asp:HiddenField ID="HF_doc_expiry_date" runat="server" />
                                        </div>
                                    </div>
                                    <div class="nulla">
                                    </div>
                                </div>
                                <div class="line">
                                </div>
                                <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_sequence" runat="server" Text='<%# Eval("sequence") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <div id="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                        </asp:ListView>
                        <asp:LinkButton ID="lnk_saveClientData" CssClass="btn" runat="server" OnClick="lnk_save_Click" OnClientClick="return RNT_validateRequestForm()"><span>Submit</span></asp:LinkButton>
                        <asp:LinkButton ID="lnk_add" CssClass="btn" runat="server" OnClick="lnk_add_Click" OnClientClick="return confirm('All not saved changes will be lost. Want to proceed?')"><span>Add a Person</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script type="text/javascript">
        function RNT_validateRequestForm() {
            var _validate = true;
            var _arr = $("#<%= HF_controls.ClientID%>").val().split("|");
            $("#errorLi").hide();
            //alert(_arr);
            for (var i = 0; i < _arr.length; i++) {
                $(".txt_name_full_check_" + _arr[i]).css("display", "none");
                $(".txt_name_full_" + _arr[i]).removeClass(FORM_errorClass);
                if ($.trim($(".txt_name_full_" + _arr[i]).val()) == "") {
                    //alert(".txt_name_full_" + _arr[i]);
                    $(".txt_name_full_" + _arr[i]).addClass(FORM_errorClass);
                    $(".txt_name_full_check_" + _arr[i]).css("display", "block");
                    _validate = false;
                }

                $(".drp_doc_type_check_" + _arr[i]).css("display", "none");
                $(".drp_doc_type_" + _arr[i]).removeClass(FORM_errorClass);
                if ($.trim($(".drp_doc_type_" + _arr[i]).val()) == "") {
                    $(".drp_doc_type_" + _arr[i]).addClass(FORM_errorClass);
                    $(".drp_doc_type_check_" + _arr[i]).css("display", "block");
                    _validate = false;
                }

                $(".txt_doc_num_check_" + _arr[i]).css("display", "none");
                $(".txt_doc_num_" + _arr[i]).removeClass(FORM_errorClass);
                if ($.trim($(".txt_doc_num_" + _arr[i]).val()) == "") {
                    $(".txt_doc_num_" + _arr[i]).addClass(FORM_errorClass);
                    $(".txt_doc_num_check_" + _arr[i]).css("display", "block");
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

</asp:Content>
