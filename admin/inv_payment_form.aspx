<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inv_payment_form.aspx.cs" Inherits="RentalInRome.admin.inv_payment_form" %>

<%@ Register Src="~/admin/uc/UC_loader.ascx" TagName="UC_loader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                <uc1:uc_loader id="UC_loader1" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel_main" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" runat="server" />
            <asp:HiddenField ID="HF_IdReservation" runat="server" />
            <div id="main">
                <span class="titlight">Gestione Pagamenti</span>
                <div class="mainline">
                    <div class="prices">
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
                            </div>
                            <h3>
                                Pagamento #<%= HF_code.Value%></h3>
                            <asp:HiddenField ID="HF_code" runat="server" Value="0" />
                            <asp:HiddenField ID="HF_pay_cause" runat="server" Value="0" />
                            <asp:HiddenField ID="HF_pr_total" runat="server" Value="0" />
                            <asp:HiddenField ID="HF_is_complete" runat="server" Value="0" />
                            <div class="price_div">
                                <table class="selPeriod risric" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 110px;">
                                            Causale
                                        </td>
                                        <td align="right">
                                            <%=  invUtils.invPayment_causeTitle("" + HF_pay_cause.Value, "- - -")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Importo
                                        </td>
                                        <td align="right">
                                            <%=HF_pr_total.Value + "&nbsp;&euro;"%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pagato?
                                        </td>
                                        <td align="right">
                                            <%= (HF_is_complete.Value + "" == "1") ? "SI" : "NO"%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                        <div class="nulla">
                        </div>
                        <div class="fasciatitBar" style="overflow: hidden; position: relative;">
                            <div class="price_div" id="pnl_saveNO" runat="server" visible="false">
                                <div class="btnric" style="float: left; margin: 50px;" id="pnl_btnSave" runat="server">
                                    <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                                </div>
                            </div>
                            <div class="price_div" id="pnl_saveOK" runat="server">
                                <div class="btnric" style="float: left; margin: 50px;" runat="server">
                                    <a href="javascript:parent.Shadowbox.close();">
                                        <span>chiudi</span></a>
                                </div>
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
    </form>
</body>
</html>
