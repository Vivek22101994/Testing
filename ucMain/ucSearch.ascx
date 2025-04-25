<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearch.ascx.cs" Inherits="RentalInRome.ucMain.ucSearch" %>
<style>
    .calendar-input {
        cursor: pointer !important;
    }
</style>
<asp:UpdatePanel ID="pnl_Search" runat="server" class="form">
    <ContentTemplate>
        <asp:HiddenField ID="HF_pidStp" runat="server" Value="6" />
        <div class="form-group">
            <div class="form-control-large r-pad">
                <input id="txt_dtStart_<%= Unique %>" type="text" style="" class="form-control calendar-input" placeholder="Check-in" readonly="readonly" />
                <a class="cal" id="startCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />

            </div>

            <div class="form-control-large l-pad">
                <input id="txt_dtEnd_<%= Unique %>" type="text" class="form-control calendar-input" style="" placeholder="Check-out" readonly="readonly" />
                <a class="cal" id="endCalTrigger_<%= Unique %>"></a>
                <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
            </div>

            <div class="form-control-small r-pad">
                <asp:TextBox ID="txt_dtCount_night" runat="server" CssClass="form-control" placeholder="Nights" ReadOnly="true" Style="background-color: white;"></asp:TextBox>
            </div>

            <div class="form-control-small l-pad">
                <asp:DropDownList ID="drp_zone" runat="server" data-placeholder="Zone">
                </asp:DropDownList>
            </div>

            <div class="form-control-small r-pad">
                <asp:DropDownList ID="drp_numPers_adult" runat="server" data-placeholder="Adults" AutoPostBack="true" OnSelectedIndexChanged="drp_numPers_adult_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <div class="form-control-small l-pad">
                <asp:DropDownList ID="drp_numPers_children" runat="server" data-placeholder="Children">
                </asp:DropDownList>
            </div>

            <div class="form-control-small r-pad">
                <asp:DropDownList ID="drp_numPers_infants" runat="server" data-placeholder="Infants">
                </asp:DropDownList>
            </div>
            <asp:LinkButton ID="lnk_search" runat="server" class="btn btn-fullcolor " OnClick="lnk_search_Click"><%=contUtils.getLabel("lblSearch") %></asp:LinkButton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="HF_unique" runat="server" />

<script type="text/javascript">
     
    var _JSCal_Range_<%= Unique %>;

    function setCal_<%= Unique %>(dtStartInt, dtEndInt) {       
        _JSCal_Range_<%= Unique %> = new JSCal.Range({ dtFormat: "d MM yy", minDateInt: <%= DateTime.Now.JSCal_dateToInt() %>, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart_<%= Unique %>", startDateInt: dtStartInt, startTrigger: "#startCalTrigger_<%= Unique %>", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd_<%= Unique %>", endDateInt: dtEndInt, endTrigger: "#endCalTrigger_<%= Unique %>", 
            countCont: "#<%= txt_dtCount_night.ClientID %>", 
            countLabel:"<%= contUtils.getLabel("lblNights") %>",   changeMonth: true, changeYear: true, beforeShowDay: checkCalDates_<%= Unique %> });
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


