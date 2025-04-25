<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_inv_invoiceNotes.ascx.cs" Inherits="RentalInRome.admin.uc.UC_inv_invoiceNotes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="fasciatitBar" style="overflow: hidden; position: relative;">
    <div id="pnl_lock" runat="server" visible="false" style="background-image: url('images/lock.png'); width: 2000px; height: 2000px; margin: -200px; position: absolute; z-index: 2;">
    </div>
    <asp:Panel ID="pnl_view" runat="server">
        <asp:LinkButton ID="lnk_edit" runat="server" CssClass="changeapt topright" OnClick="lnk_edit_Click">Cambia le note</asp:LinkButton>
        <h3>
            Note sulla fattura</h3>
        <div class="price_div">
            <%=re_description.Content == "" ? "- Nessuna nota inserita - " : re_description.Content%>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnl_edit" runat="server">
        <h3>
            Cambia Note sulla fattura</h3>
        <div class="price_div">
            <telerik:RadEditor runat="server" ID="re_description" SkinID="DefaultSetOfTools" Height="200" Width="600" ToolbarMode="Default" ToolsFile="/admin/common/ToolsFileLimited.xml">
                <CssFiles>
                    <telerik:EditorCssFile Value="/css/styleEditor.css" />
                </CssFiles>
            </telerik:RadEditor>
            <div class="btnric" style="float: left; margin: 50px;">
                <asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click">Salva le note</asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnk_cancel_Click">Annulla</asp:LinkButton>
            </div>
        </div>
        <div class="nulla">
        </div>
    </asp:Panel>
    <div class="nulla">
    </div>
</div>
