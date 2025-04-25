<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="limo_transportDuration.aspx.cs" Inherits="RentalInRome.admin.limo_transportDuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .durationMin
        {
            width: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <script type="text/javascript">
        function durationMinSet() {
            $(".durationMin").keydown(function (event) {
                // Allow only backspace and delete
                if (event.keyCode == 46 || event.keyCode == 8) {
                    // let it happen, don't do anything
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.keyCode < 48 || event.keyCode > 57) {
                        event.preventDefault();
                    }
                }
            });
        }
    </script>
    <asp:HiddenField ID="HF_pidZone" runat="server" Visible="false" />
    <asp:HiddenField ID="HF_inOut" runat="server" Value="in" Visible="false" />
    <asp:Literal ID="ltr_zoneTitle" runat="server" Visible="false"></asp:Literal>
    <div id="fascia1">
        <div class="pannello_fascia1">
            <div style="clear: both">
                <h1>
                    Gestione Transfer della zona
                    <%= ltr_zoneTitle.Text%>
                </h1>
                <div class="bottom_agg">
                </div>
            </div>
            <div style="clear: both">
                <div class="filt">
                    <div class="t">
                        <img src="images/filt_t1.gif" width="6" height="6" style="float: left" />
                        <img src="images/filt_t2.gif" width="6" height="6" style="float: right" />
                    </div>
                    <div class="c">
                        <div class="filtro_cont">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                                <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 180px;">
                                                    <label>
                                                        Trasporto da/a:</label>
                                                    <asp:DropDownList ID="drp_PickupPlace" runat="server" CssClass="inp">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 190px;">
                                                    <label>
                                                        Tipo trasporto:</label>
                                                    <table class="inp">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="chkList_TransportType" runat="server">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="HF_dtCreation_from" runat="server" />
                                                    <asp:HiddenField ID="HF_dtCreation_to" runat="server" />
                                                </td>
                                                <td style="width: 190px;">
                                                    <label>
                                                        Orario:</label>
                                                    <table class="inp">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    inizio:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_hourFrom" runat="server" CssClass="inp">
                                                                </asp:DropDownList>                                                            
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    fine: <br/>(compreso)</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drp_hourTo" runat="server" CssClass="inp">
                                                                </asp:DropDownList>                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="HF_inv_dtInvoice_from" runat="server" />
                                                    <asp:HiddenField ID="HF_inv_dtInvoice_to" runat="server" />
                                                </td>
                                                <td style="width: 180px;">
                                                    <label>
                                                        Durata:</label>
                                                    <table class="inp">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDuration" runat="server" CssClass="durationMin"></asp:TextBox>&nbsp;min
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Table seconda riga -->
                                    </td>
                                    <td valign="bottom">
                                        <asp:LinkButton ID="lnk_save" runat="server" CssClass="ricercaris" OnClick="lnk_save_Click"><span>Aggiorna i dati</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="b">
                        <img src="images/filt_b1.gif" width="6" height="6" style="float: left" />
                        <img src="images/filt_b2.gif" width="6" height="6" style="float: right" />
                    </div>
                </div>
            </div>
            <div style="clear: both">
                <asp:ListView ID="LV" runat="server" OnDataBound="LV_DataBound" OnItemDataBound="LV_ItemDataBound">
                    <ItemTemplate>
                        <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("PickupPlace_title") + "&nbsp;(" + Eval("TransportType_title") + ")"%>
                                </span>
                            </td>
                            <asp:ListView ID="LV_inner" runat="server">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_hour" Visible="false" runat="server" Text='<%# Eval("hour") %>' />
                                    <td class="at<%# Eval("hour") %> atAll" onmouseover="$('.at<%# Eval("hour") %>').addClass('tr_current');" onmouseout="$('.at<%# Eval("hour") %>').removeClass('tr_current');">
                                        <asp:TextBox ID="txtAt" runat="server" CssClass="durationMin"></asp:TextBox>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                            <td>
                                <asp:Label ID="lbl_PickupPlace" Visible="false" runat="server" Text='<%# Eval("PickupPlace") %>' />
                                <asp:Label ID="lbl_TransportType" Visible="false" runat="server" Text='<%# Eval("TransportType") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                            <td>
                                <span>
                                    <%# Eval("PickupPlace_title") + "&nbsp;(" + Eval("TransportType_title") + ")"%>
                                </span>
                            </td>
                            <asp:ListView ID="LV_inner" runat="server">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_hour" Visible="false" runat="server" Text='<%# Eval("hour") %>' />
                                    <td class="at<%# Eval("hour") %> atAll" onmouseover="$('.at<%# Eval("hour") %>').addClass('tr_current');" onmouseout="$('.at<%# Eval("hour") %>').removeClass('tr_current');">
                                        <asp:TextBox ID="txtAt" runat="server" CssClass="durationMin"></asp:TextBox>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                            <td>
                                <asp:Label ID="lbl_PickupPlace" Visible="false" runat="server" Text='<%# Eval("PickupPlace") %>' />
                                <asp:Label ID="lbl_TransportType" Visible="false" runat="server" Text='<%# Eval("TransportType") %>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" style="">
                            <tr>
                                <td>
                                    No data was returned.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="table_fascia">
                            <table border="0" cellpadding="0" cellspacing="0" style="table-layout: fixed; width: 1260px">
                                <tr style="text-align: left">
                                    <th style="width: 300px">
                                        Trasporto da/a (Tipo trasporto)
                                    </th>
                                    <asp:ListView ID="LV_inner" runat="server">
                                        <ItemTemplate>
                                            <th class="at<%# Eval("hour") %> atAll" style="width: 40px; text-align: center;">
                                                <%# Eval("title") %>
                                            </th>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <th>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
            <div style="clear: both">
                <div class="salvataggio">
                    <div class="bottom_salva">
                        <asp:LinkButton ID="lnk_saveTable" runat="server" OnClick="lnk_saveTable_Click"><span>Salva Modifiche della Tabella</span></asp:LinkButton>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div class="nulla"> </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
