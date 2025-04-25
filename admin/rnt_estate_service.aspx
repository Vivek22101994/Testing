<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_service.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_service" %>

<%@ Register Src="uc/UC_rnt_estate_service.ascx" TagName="UC_rnt_estate_service" TagPrefix="uc1" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="ltr_pageTitle" runat="server" Visible="false"></asp:Literal>
    <asp:HiddenField runat="server" ID="HF_IdEstate" Value="-1" />
    <h1 class="titolo_main">
        <%=ltr_pageTitle.Text %>
    </h1>
    <div id="fascia1">
        <div style="clear: both; margin: 3px 0 5px 30px;">
            <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
        </div>
        <div class="pannello_fascia1">
            <div class="bottom_agg" id="pnl_srs" runat="server">
                <a href="rnt_estate_service.aspx?service=srs&id=<%=HF_IdEstate.Value %>">
                    <span>Gestisci Srs</span>
                </a>
            </div>
            <div class="bottom_agg" id="pnl_eco" runat="server">
                <a href="rnt_estate_service.aspx?service=eco&id=<%=HF_IdEstate.Value %>">
                    <span>Gestisci Ecopulizie</span>
                </a>
            </div>
        </div>
    </div>
    <div class="nulla">
    </div>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- INIZIO MAIN LINE -->
            <div class="mainline">
                <!-- BOX 1 -->
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" /></div>
                    </div>
                    <div class="center">
                        <uc1:UC_rnt_estate_service ID="UC_rnt_estate_service" runat="server" Service="eco" />
                        <div class="nulla">
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="nulla">
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" /></div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" /></div>
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
