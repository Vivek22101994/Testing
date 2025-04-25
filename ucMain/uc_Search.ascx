<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Search.ascx.cs" Inherits="RentalInRome.ucMain.uc_Search" %>
<style>
    .calendar-input {
        cursor: pointer !important;
    }
</style>
<asp:UpdatePanel ID="pnl_Search" runat="server" class="form">
    <ContentTemplate>
        <asp:HiddenField ID="HF_pidStp" runat="server" Value="6" />
        <div class="sidebar col-sm-4 bookingFormDetCont">
            <div class="gray bookingFormDet">
                <h2 class="section-title"><%= contUtils.getLabel("lblSearchInRome") %></h2>
                <div class="form-group">
                    <div class="col-sm-12">
                        <input id="txt_dtStart_<%= Unique %>" type="text" style="" class="form-control calendar-input" placeholder="Check-in" readonly="readonly" />
                        <a class="cal" id="startCalTrigger_<%= Unique %>"></a>
                        <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                        <%--<input class="form-control calendar-input" type="text" placeholder="Check-in" name="checkin">--%>

                        <input id="txt_dtEnd_<%= Unique %>" type="text" class="form-control calendar-input" style="" placeholder="Check-out" readonly="readonly" />
                        <a class="cal" id="endCalTrigger_<%= Unique %>"></a>
                        <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />

                        <asp:TextBox ID="txt_dtCount_night" runat="server" CssClass="form-control" placeholder="Nights" ReadOnly="true" Style="background-color: white;"></asp:TextBox>
                        <%--<input class="form-control calendar-input" type="text" placeholder="Check-out" name="checkout">--%>

                        <asp:DropDownList ID="drp_zone" runat="server" data-placeholder="Zone">
                        </asp:DropDownList>

                        <asp:DropDownList ID="drp_numPers_adult" runat="server" data-placeholder="Adults" AutoPostBack="true" OnSelectedIndexChanged="drp_numPers_adult_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:DropDownList ID="drp_numPers_children" runat="server" data-placeholder="Children">
                        </asp:DropDownList>

                        <asp:DropDownList ID="drp_numPers_infants" runat="server" data-placeholder="Infants">
                        </asp:DropDownList>

                        <%-- <select id="book_adults" name="book_adults" data-placeholder="Adults">
                            <option value="---">Adults</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                        </select>--%>

                        <%--  <select id="book_children" name="book_children" data-placeholder="Children">
                            <option value="---">Children</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                        </select>

                        <select id="book_infants" name="book_infants" data-placeholder="Infants">
                            <option value="---">Infants</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                        </select>--%>
                    </div>

                    <p class="center">
                        <asp:LinkButton ID="lnk_search" runat="server" class="btn btn-default-color btn-book-now" OnClick="lnk_search_Click"><%=contUtils.getLabel("lblSearch") %></asp:LinkButton>
                        <%-- <button type="submit" class="btn btn-default-color btn-book-now">Search apartment</button>--%>
                    </p>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="HF_unique" runat="server" />
<script type="text/javascript">
     
    var _JSCal_Range_<%= Unique %>;

    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {       
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", 
            countCont: "#<%= txt_dtCount_night.ClientID %>", countLabel:"<%= contUtils.getLabel("lblNights") %>",
            changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }

    function setCal1_<%= Unique %>(dtStartInt, dtEndInt) {
        console.log('set Calendar' );
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>",
            startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt,
            startTrigger: "#startCalTrigger_<%= Unique %>", endTrigger: "#endCalTrigger_<%= Unique %>",
            countCont: "#<%= txt_dtCount_night.ClientID %>", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
    }
    <%= ltr_checkCalDates.Text %>
 
    function ChoosenSelectForSearchForm() {
        //console.log("ChoosenSelectForSearchForm");
        if ($('select').length) {
            $("select").chosen({
                allow_single_deselect: true,
                disable_search_threshold: 12
            });
        }
    }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ChoosenSelectForSearchForm);
</script>
<asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>