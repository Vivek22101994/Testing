<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProblemSelect.ascx.cs" Inherits="RentalInRome.admin.modRental.uc.ucProblemSelect" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:HiddenField ID="HF_unique" Value="" runat="server" />
<asp:HiddenField ID="HF_isEdit" runat="server" Visible="false" />
<asp:HiddenField ID="HF_isChanged" runat="server" Visible="false" />
<asp:HiddenField ID="HF_problemID" runat="server"  Visible="false"/>
<asp:Literal ID="ltr_problemDesc" runat="server" Visible="false"></asp:Literal>
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <h3>
            <asp:Literal ID="ltrTitle" runat="server">Problemi Segnalati</asp:Literal>
        </h3>
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_ok" runat="server">
                 <tr>
                    <td style="width: 75px;">
                        Problema:
                    </td>
                    <td>
                       <%= rntUtils.getProblem_title(ProblemID, "-non definito-")%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border: 1px dotted rgb(0, 0, 0);">
                        <%= ProblemDesc%>
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px" id="pnl_no" runat="server">
                <tr>
                    <td>
                        Non ci sono Problemi Segnalati
                    </td>
                </tr>
            </table>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Inserisci note</h3>
        <asp:LinkButton ID="lnk_cancel" runat="server" CssClass="changeapt topright" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
        <div class="price_div">
            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                <tr>
                    <td style="width: 75px;">
                        Problema:
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_problemID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadEditor runat="server" ID="re_problemDesc" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="ShowOnFocus" ToolsFile="/admin/common/ToolsFileLimited.xml">
                            <CssFiles>
                                <telerik:EditorCssFile Value="" />
                            </CssFiles>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="salvataggio" id="pnl_buttons" runat="server">
                            <div class="bottom_salva">
                                <asp:LinkButton ID="lnk_salva" runat="server" OnClick="lnk_salva_Click" ValidationGroup="discount"><span>Salva</span></asp:LinkButton>
                            </div>
                            <div class="nulla">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>
