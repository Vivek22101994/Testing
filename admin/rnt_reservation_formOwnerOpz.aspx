<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rnt_reservation_formOwnerOpz.aspx.cs" Inherits="RentalInRome.admin.rnt_reservation_formOwnerOpz" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
        @import url(/jquery/css/ui-core.css);
        @import url(/jquery/css/ui-datepicker.css);
        html, body
        {
            background-color: #FFF;
        }
    </style>
    <script src="../js/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script src="../js/tiny_mce/init.js" type="text/javascript"></script>
    <script src="../jquery/js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui--core.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-effects.min.js" type="text/javascript"></script>
    <script src="../jquery/js/ui-datepicker.min.js" type="text/javascript"></script>
    <script src="../jquery/datepicker-lang/jquery.ui.datepicker-it.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <uc1:UC_loader ID="UC_loader1" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <asp:HiddenField ID="HF_IdEstate" runat="server" />
            <asp:HiddenField ID="HF_ext_ownerdaysinyear" runat="server" />
            <div id="main">
                <span class="titlight">Nuova opzione del proprietario per la struttura <%= CurrentSource.rntEstate_code(HF_IdEstate.Value.ToInt32(), "")%></span>
                <div class="mainline">
                    <div class="prices">
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>
                                Dettagli</h3>
                            <div class="price_div">
                                <table class="selPeriod" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <span class="numStep">Periodo</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left">
                                            da:&nbsp;<input id="txt_dtStart" type="text" readonly="readonly" />
                                            <asp:HiddenField ID="HF_dtStart" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <a class="calendario" id="startCalTrigger"></a>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left">
                                            a:&nbsp;<input id="txt_dtEnd" type="text" readonly="readonly" />
                                            <asp:HiddenField ID="HF_dtEnd" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <a class="calendario" id="endCalTrigger"></a>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" colspan="3">
                                            Commento:<br/>
                                            <asp:TextBox ID="txt_subject" runat="server" Style="width: 250px;"></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <h3>
                                Salvataggio Dati</h3>
                            <div class="price_div" id="pnl_saveNO" runat="server">
                                <table class="selPeriod" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="numStep" Style="color: #FF0000;" Text="Selezionare il periodo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="btnric" style="float: left; margin: 50px;" id="pnl_btnSave" runat="server">
                                                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                                            </div>
                                            <div class="btnric" style="float: left; margin: 50px;">
                                                <a href="javascript:parent.refreshDates();">
                                                    <span>chiudi</span></a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="nulla">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Literal ID="ltr_checkCalDates" runat="server" Visible="false"></asp:Literal>
    <script type="text/javascript">
    var _JSCal_Range;
    function setCal() {
        _JSCal_Range = new JSCal.Range({ dtFormat: "d MM yy", countMin: 1, startCont: "#<%= HF_dtStart.ClientID %>", startView: "#txt_dtStart", startTrigger: "#startCalTrigger", endCont: "#<%= HF_dtEnd.ClientID %>", endView: "#txt_dtEnd", endTrigger: "#endCalTrigger", changeMonth: true, changeYear: true, beforeShowDay: checkCalDates });
    }
    </script>
    </form>
</body>
</html>
