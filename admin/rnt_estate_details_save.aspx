<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="rnt_estate_details_save.aspx.cs" Inherits="RentalInRome.admin.rnt_estate_details_save" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HF_id" Value="0" runat="server" />
            <asp:HiddenField ID="HF_updater_newId" Value="0" runat="server" />
            <asp:HiddenField ID="HF_updater_args" Value="0" runat="server" />
            <h1 class="titolo_main">Scheda Struttura</h1>
            <div id="fascia1">
                <div style="clear: both; margin: 3px 0 5px 30px;">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio hiddenbeforload" style="display: none;">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_saveOnly_Click" ><span>Salva Modifiche</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_salva" runat="server"  OnClick="lnk_salva_Click"><span>Salva e torna nella lista</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="<%=listPage %>"><span>Torna nella lista senza salvare</span> </a>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click" ><span>Annulla Modifiche</span></asp:LinkButton>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">                
                <div class="mainbox">
                    <div class="top">
                        <div style="float: left;">
                            <img src="images/mainbox1.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox2.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                    <div class="center">
                        <span class="titoloboxmodulo">Min Nights</span>
                        <div class="boxmodulo">                      
                            <table>
                                <tr>
                                    <td>Min. Notti:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nights_min" Width="30px" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txt_nights_min" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                    </td>
                                    <td>&nbsp;
                                    </td>                                    
                                </tr>  
                                 <tr>
                                    <td>Min. Notti Altissima:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_nights_minVHSeason" Width="30px" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator23" runat="server" ControlToValidate="txt_nights_minVHSeason" ErrorMessage="**" ValidationExpression="(^(-)?\d*?$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                    </td>
                                    <td>&nbsp;
                                    </td>                                    
                                </tr>                            
                            </table>
                        </div>
                    </div>
                    <div class="bottom">
                        <div style="float: left;">
                            <img src="images/mainbox3.gif" width="10" height="10" alt="" />
                        </div>
                        <div style="float: right;">
                            <img src="images/mainbox4.gif" width="10" height="10" alt="" />
                        </div>
                    </div>
                </div>     
                <div class="nulla">
                </div>
            </div>            
            <div class="nulla">
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>