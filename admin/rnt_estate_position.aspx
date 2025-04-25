<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_position.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_position" %>
<%@ Register src="uc/UC_rnt_estate_position.ascx" tagname="UC_rnt_estate_position" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1 class="titolo_main">
                Posizionamento Strutture in HomePage
            </h1>
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
                        <uc1:UC_rnt_estate_position ID="UC_rnt_estate_position1" runat="server" Position="homepage_attention" />
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
