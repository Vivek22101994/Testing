<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usr_owner_createform.aspx.cs" Inherits="RentalInRome.admin.usr_owner_createform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nuovo Proprietario</title>
    <style type="text/css">
        @import url(/admin/css/adminStyle.css);
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1 class="titolo_main">Nuovo Proprietario</h1>
            <!-- INIZIO MAIN LINE -->
            <div class="salvataggio" style=" width:auto;">
                <div class="bottom_salva">
                    <asp:LinkButton ID="lnk_save" runat="server" ValidationGroup="dati" OnClick="lnk_save_Click"><span>Salva</span></asp:LinkButton>
                </div>
                <div class="bottom_salva">
                    <a href="#" onclick="parent.Shadowbox.close()">
                        <span>Annulla</span></a>
                </div>
                <div class="nulla">
                </div>
            </div>
            <div class="nulla">
            </div>
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
                        <span class="titoloboxmodulo">Dati Identificativi</span>
                        <div class="boxmodulo">
                            <table cellpadding="1" cellspacing="2" style="margin-bottom: 1px">
                                <tr>
                                    <td class="td_title">
                                        Titolo:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drp_honorific" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Nome/Cognome:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_name_full" Width="300px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_name_full" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email" Width="300px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//obbligatorio" ValidationGroup="dati" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator22" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//non valido" ValidationExpression="(^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 2:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_2" Width="300px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//non valido" ValidationExpression="(^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 3:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_3" Width="300px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//non valido" ValidationExpression="(^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Email 4:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_email_4" Width="300px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_contact_email" ErrorMessage="<br/>//non valido" ValidationExpression="(^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$)" ValidationGroup="dati" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Telefono:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_phone" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Cellulare:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_contact_phone_mobile" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_title">
                                        Location:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_country" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == 1">
                                        </asp:LinqDataSource>
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server" visible="false">
                                    <td class="td_title">
                                        Lingua:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="drp_lang" OnDataBound="drp_lang_DataBound" DataSourceID="LDS_lang" DataTextField="title" DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LDS_lang" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="title" TableName="CONT_TBL_LANGs" Where="is_active == 1">
                                        </asp:LinqDataSource>
                                    </td>
                                </tr>
                            </table>
                        </div>
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
    </form>
</body>
</html>
