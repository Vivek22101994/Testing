<%@ Page Title="" Language="C#" MasterPageFile="~/admin/common/MP_admin.Master" AutoEventWireup="true" CodeBehind="EstateDett_iCalImport.aspx.cs" Inherits="ModRental.admin.modRental.EstateDett_iCalImport" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/admin/uc/UC_rnt_estate_navlinks.ascx" TagName="UC_rnt_estate_navlinks" TagPrefix="uc1" %>
<%@ Register Src="~/admin/modContent/UCgetFile.ascx" TagName="UCgetFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        <style type="text/css">.main div.mainbox {width:100%;}</style>

            <asp:HiddenField ID="HF_IdEstate" Value="0" runat="server" />

            

            <h1 class="titolo_main">
                Importazione della disponibilita da iCal:
                <asp:Literal runat="server" ID="ltr_apartment"></asp:Literal>
            </h1>

            <img src="/admin/images/logo-google-cal.png" style="position:absolute; right: 30px; top: 190px;" />

            <div id="fascia1">
                <div class="tabsTop">
                    <uc1:UC_rnt_estate_navlinks ID="UC_rnt_estate_navlinks1" runat="server" />
                </div>
            </div>
            <div class="salvataggio">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_saveOnly" runat="server" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_annulla" runat="server" OnClick="lnk_annulla_Click"><span>Annulla</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="/admin/rnt_estate_list.aspx"><span>Torna nel elenco</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
            <div class="mainline">
                <div class="mainbox iCalMainBox">
                    
                    <div class="center">
                        <div class="boxmodulo">
                            <table cellpadding="2" cellspacing="2">
                                <tr>
                                    <td valign="middle" colspan="2">
                                    </td>
                                    <td valign="middle">Importa in automatico
                                    </td>
                                    <td valign="middle">
                                        Url del calendario<br />
                                        es: <i><b>http://</b>www.domain.com/example.ics</i> o <i><b>https://</b>securedomain.com/example.ics</i>
                                    </td>
                                    <td valign="middle">
                                        Test prima di salvare
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" colspan="2">Predefinito
                                    </td>
                                    <td valign="middle">Si
                                    </td>
                                    <td valign="middle">
                                        <asp:TextBox ID="txt_iCalUrl" runat="server" Width="700px"></asp:TextBox>
                                    </td>
                                    <td valign="middle">
                                        <asp:LinkButton ID="lnkImport" runat="server" OnClick="lnk_import_Click"><span>Importa</span></asp:LinkButton>
                                    </td>
                                </tr>
                            <asp:ListView ID="LvChannelManagers" runat="server" OnItemCommand="LvChannelManagers_ItemCommand">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td valign="middle">
                                            <span>
                                                <%# Eval("title")%></span>
                                        </td>
                                        <td valign="middle" align="center">
                                            <span>
                                                <%# Eval("imgLogo") + "" != "" ? "<img alt='' src='/"+Eval("imgLogo")+"' style='max-height: 50px;' />" : ""%></span>
                                        </td>
                                        <td valign="middle">
                                            Importa in automatico:<br/>
                                            <asp:DropDownList ID="drp_iCalImportEnabled" runat="server">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="middle">
                                            <asp:TextBox ID="txt_iCalImportUrl" runat="server" onfocus="this.select()" style="width: 500px;"></asp:TextBox>
                                        </td>
                                        <td valign="middle">
                                            <asp:LinkButton ID="lnkImport" runat="server" CommandName="import"><span>Importa</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <asp:Label ID="lbl_id" Visible="false" runat="server" Text='<%# Eval("code") %>' />
                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td valign="middle">
                                            <span>
                                                <%# Eval("title")%></span>
                                        </td>
                                        <td valign="middle" align="center">
                                            <span>
                                                <%# Eval("imgLogo") + "" != "" ? "<img alt='' src='/"+Eval("imgLogo")+"' style='max-height: 50px;' />" : ""%></span>
                                        </td>
                                        <td valign="middle">Importa in automatico:<br />
                                            <asp:DropDownList ID="drp_iCalImportEnabled" runat="server">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="middle">
                                            <asp:TextBox ID="txt_iCalImportUrl" runat="server" onfocus="this.select()" Style="width: 500px;"></asp:TextBox>
                                        </td>
                                        <td valign="middle">
                                            <asp:LinkButton ID="lnkImport" runat="server" CommandName="import"><span>Importa</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                            </table>
                            <asp:ListView ID="LV" runat="server" Visible="false">
                                <ItemTemplate>
                                    <tr class="" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td>
                                            <span>
                                                <%# ((DateTime)Eval("iCalDtStart")).formatITA(false)%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# ((DateTime)Eval("iCalDtEnd")).formatITA(false)%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("iCalComment")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# "Bloccato da pren #"+Eval("reservationId")+" ("+Eval("reservationStateName")+")"%></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                        <td>
                                            <span>
                                                <%# ((DateTime)Eval("iCalDtStart")).formatITA(false)%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# ((DateTime)Eval("iCalDtEnd")).formatITA(false)%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("iCalComment")%></span>
                                        </td>
                                        <td>
                                            <span>
                                                <%# "Bloccato da pren #"+Eval("reservationId")+" ("+Eval("reservationStateName")+")"%></span>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <span class="titoloboxmodulo">Calendario importato con successo</span>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <span class="titoloboxmodulo">Calendario importato con alcuni errori</span>
                                    <table border="0" cellpadding="0" cellspacing="0" style="">
                                        <tr class="alternate" onmouseover="$(this).addClass('tr_current');" onmouseout="$(this).removeClass('tr_current');">
                                            <td style="width: 100px;">Check-In</td>
                                            <td style="width: 100px;">Check-Out</td>
                                            <td style="width: 200px;">Comment</td>
                                            <td style="width: 300px;">Motivo</td>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                   
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
