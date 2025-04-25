<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_adminNoMenu.Master" AutoEventWireup="true" CodeBehind="agentContractDett.aspx.cs" Inherits="ModRental.admin.modRental.agentContractDett" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function set_lbl_commissionAmount(val) {
                if (val == "PPB")
                    $("#<%=lbl_commissionAmount.ClientID %>").html("Commissione in %");
                if (val == "PPL")
                    $("#<%=lbl_commissionAmount.ClientID %>").html("Costo per richiesta");
                if (val == "PPS")
                    $("#<%=lbl_commissionAmount.ClientID %>").html("Costo fisso");

        }

        function selectAll(obj) {
            if (document.getElementById(obj.id).checked == true) 
                $(".chk_estates :checkbox").attr('checked', true);
            else
                $(".chk_estates :checkbox").attr('checked', false);
        }
        </script>
    </telerik:RadScriptBlock>
    <style type="text/css">
        .rcTable { margin: 0 !important; }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxPanel ID="pnlDett" runat="server">
        <asp:HiddenField ID="HfIdAgent" Value="0" runat="server" />
        <asp:HiddenField ID="HfCurrId" Value="0" runat="server" />
        <h1 class="titolo_main">
            <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
        </h1>
        <!-- INIZIO MAIN LINE -->
        <div class="salvataggio">
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="dati" OnClick="lnkSave_Click"><span>Salva Modifiche</span></asp:LinkButton>
            </div>
            <div class="bottom_salva">
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"><span>Annulla Modifiche</span></asp:LinkButton>
            </div>
            <div class="nulla">
            </div>
        </div>
        <div class="nulla">
        </div>
        <div class="mainline">
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Dati identificativi</span>
                    <div class="boxmodulo">
                        <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">

                            <tr>
                                <td class="td_title">Contract num.:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_contractNumber" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Start:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_contract_dtStart" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">End:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="rdp_contract_dtEnd" runat="server" Width="100px">
                                        <DateInput DateFormat="dd/MM/yyyy">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Contact type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_contractType" runat="server" onchange="set_lbl_commissionAmount(this.value)">
                                        <asp:ListItem Value="PPB">PPB - percentuale dalla pren</asp:ListItem>
                                        <asp:ListItem Value="PPL">PPL - si paga ogni richiesta</asp:ListItem>
                                        <asp:ListItem Value="PPS">PPS - costo fisso per un periodo</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">
                                    <asp:Label ID="lbl_commissionAmount" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="ntxt_commissionAmount" runat="server" Width="50">
                                        <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">Contact type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" runat="server" onchange="set_lbl_commissionAmount(this.value)">
                                        <asp:ListItem Value="PPB">PPB - percentuale dalla pren</asp:ListItem>
                                        <asp:ListItem Value="PPL">PPL - si paga ogni richiesta</asp:ListItem>
                                        <asp:ListItem Value="PPS">PPS - costo fisso per un periodo</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_title">È il prezzo di invio?
                                </td>
                                <td>
                                    <asp:DropDownList ID="drp_IsSendPrice" runat="server">
                                        <asp:ListItem Value="1"> Yes</asp:ListItem>
                                        <asp:ListItem Value="0">  No</asp:ListItem>

                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <div class="boxmodulo" id="pnl_error" runat="server" visible="false">
                        <div style="border: 1px solid red; padding: 5px; margin-bottom: 10px;">
                            <span class="titoloboxmodulo" style="border: medium none navy; margin: 0pt;">Attenzione</span>
                            <asp:Literal ID="ltr_error" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <span class="titoloboxmodulo">Periodi speciali con cambiamento del prezzo</span>
                    <div class="boxmodulo">
                        <table border="0" cellpadding="0" cellspacing="0" style="">
                            <tr style="text-align: left">
                                <th style="width: 120px">Dal</th>
                                <th style="width: 120px">al </th>
                                <th style="width: 100px; text-align: right;">Costo </th>
                                <th></th>
                            </tr>
                            <asp:ListView ID="LV" runat="server" OnItemCommand="LV_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker ID="rdp_dtStart" runat="server" Width="100px">
                                                <DateInput DateFormat="dd/MM/yyyy">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="rdp_dtEnd" runat="server" Width="100px">
                                                <DateInput DateFormat="dd/MM/yyyy">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td style="text-align: right;">
                                            <telerik:RadNumericTextBox ID="ntxt_commissionAmount" runat="server" Width="50">
                                                <NumberFormat GroupSeparator="" DecimalSeparator="," DecimalDigits="2" />
                                            </telerik:RadNumericTextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("uid") %>' />
                                            <asp:LinkButton ID="lnk_del" runat="server" CssClass="del" CommandName="del" OnClientClick="return confirm('sta per eliminare il periodo?')"> elimina</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="5">Non sono presenti Periodi speciali!
                                            <div class="btn_save">
                                                <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi</asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <tr>
                                        <td colspan="5">
                                            <div class="btn_save">
                                                <asp:LinkButton ID="lnk_add_new" runat="server" OnClick="lnk_add_new_Click">Aggiungi nuovo Periodo</asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server" />
                                </LayoutTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center existingapt">
                    <asp:Label ID="lblExistingApt" runat="server" Text="List of existing apartment in Contract"></asp:Label>
                    <asp:DataList ID="lvExistingApt" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <asp:HiddenField ID="HF_EstateId" runat="server" Value='<%# Container.DataItem.objToInt32() %>' />
                            <span>
                                <%# CurrentSource.rntEstate_code(Container.DataItem.objToInt32(), "") %>
                            </span>
                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/delete.gif" OnClick="imgDel_Click" AlternateText="Del" ToolTip="Delete" CommandArgument='<%# Container.DataItem.objToInt32() %>' />
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

            <div class="mainbox filteragentcontract">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <telerik:RadAjaxPanel ID="rapFilter" runat="server" CssClass="boxmodulo">
                        <div class="filt">
                            <div class="t">
                                <div class="sx">
                                </div>
                                <div class="dx">
                                </div>
                            </div>
                            <div class="c">
                                <div class="filtro_cont">
                                    <table border="0" cellpadding="0" cellspacing="0" id="tbl_filter">
                                        <tr>
                                            <td>
                                                <label>Città:</label>
                                                <asp:DropDownList runat="server" ID="drp_flt_pidCity" Width="200px" CssClass="inp" Style="margin-bottom: 10px;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="drp_flt_pidCity_SelectedIndexChanged" />
                                                <div class="nulla">
                                                </div>
                                                <%-- <asp:DropDownList runat="server" ID="drp_flt_pidZone" Width="200px" CssClass="inp" Style="margin-bottom: 10px;" />
                                                <div class="nulla">
                                                </div>--%>
                                                <label>Proprietario:</label>
                                                <asp:DropDownList runat="server" ID="drp_flt_pidOwner" Width="200px" CssClass="inp" Style="margin-bottom: 10px;" />
                                                <div class="nulla">
                                                </div>
                                                <label>Nome Appartamento:</label>
                                                <asp:TextBox ID="txt_flt_code" runat="server" Width="200px" CssClass="inp"></asp:TextBox>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <label>
                                                    Zona:
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_pidZone" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_pidZone" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                                                </label>
                                                <div class="nulla">
                                                </div>
                                                <div style="max-height: 150px; min-width: 200px; overflow-y: auto;">
                                                    <asp:CheckBoxList ID="chkList_flt_pidZone" runat="server" CssClass="inp" TextAlign="Right" RepeatColumns="6" Style="margin: 0 5px 5px 0;">
                                                    </asp:CheckBoxList>
                                                    <div class="nulla">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:LinkButton ID="lnk_filter" runat="server" CssClass="ricercaris" OnClick="lnk_filter_Click"><span>Filtra</span></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="b">
                                <div class="sx">
                                </div>
                                <div class="dx">
                                </div>
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <span class="titoloboxmodulo" style="margin-bottom: 5px;">Seleziona uno o più appartamenti</span>
                        <div class="nulla">
                        </div>
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="select_pidEstate" Style="color: #E01E15; text-decoration: none;">seleziona tutti</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnk_chkListSelectAll_Click" CommandArgument="deselect_pidEstate" Style="color: #E01E15; text-decoration: none;">deseleziona tutti</asp:LinkButton>
                        <br />
                        <asp:CheckBoxList ID="chkList_estates" runat="server" RepeatColumns="10">
                        </asp:CheckBoxList>
                    </telerik:RadAjaxPanel>
                </div>
                <div class="bottom">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
            </div>
            <div class="nulla">
            </div>

            <%--     <div class="mainbox">
                <div class="top">
                    <div class="sx">
                    </div>
                    <div class="dx">
                    </div>
                </div>
                <div class="center">
                    <span class="titoloboxmodulo">Abilitato per</span>
                   <div class="agentchkall">
                        <asp:CheckBox ID="chk_check_all_apartment" runat="server" onclick="selectAll(this);" Text="Check/UnCheck All Apartments" Style="width: 100px;" />
                    </div>
                    <div class="boxmodulo">
                        <asp:CheckBoxList ID="chkList_estates" runat="server" RepeatColumns="10" CssClass="chk_estates">
                        </asp:CheckBoxList>
                    </div>
                    <div class="bottom">
                        <div class="sx">
                        </div>
                        <div class="dx">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="nulla">
        </div>--%>
    </telerik:RadAjaxPanel>
</asp:Content>
