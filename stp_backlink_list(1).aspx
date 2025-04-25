<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_backlink_list.aspx.cs" Inherits="RentalInRome.stp_backlink_list" %>

<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title><%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <div id="testoMain" style="background:none;">
        <%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + "</h3>" : ""%>
        <div class="mainContent">
            <%=ltr_description.Text %>
            <div class="box_continenti">
                <div>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont0.png" />
                        </span>
                        <span class="t">International Links</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont1.png" />
                        </span>
                        <span class="t">Nord America</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont2.png" />
                        </span>
                        <span class="t">Sud America</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont3.png" />
                        </span>
                        <span class="t">Oceania</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont4.png" />
                        </span>
                        <span class="t">Europa</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont5.png" />
                        </span>
                        <span class="t">Africa</span>
                        <span class="st">go to the list</span>
                    </a>
                    <a href="#">
                        <span class="imgworld">
                            <img src="images/css/cont6.png" />
                        </span>
                        <span class="t">Asia</span>
                        <span class="st">go to the list</span>
                    </a>
                    <div class="nulla"></div>
                </div>
            </div>
        
            <div class="listtravel">
                <span class="tit">Special Link</span>
                <a target="_blank" class="linktl" href="http://www.rentalinrome.com">
                    <h4>www.rentalinrome.com</h4>
                    <p>Rental in Rome</p>
                </a>
            </div>
            <div class="nulla">
            </div>

        </div>

    </div>
    <div id="colDx">
        <div class="formdx">
            <span class="tit">Richiesta inserimento sito <br />nella pagina <span style="color:#FE6634;">Travel Link</span></span>

            Dopo aver inserito il nostro link sul tuo sito compila il form.

            <label>Nome sito</label>
            <input />
            <span class="nulla" style="margin-bottom: 6px;"></span>
            <label>Url (http://www.vostrosito.it)</label>
            <input style="width:282px;" />
            <span class="nulla" style="margin-bottom: 6px;"></span>
            <label>Continente</label>
            <select>
                <option>Europa</option>
                <option>Oceania</option>
            </select>
            <span class="nulla" style="margin-bottom: 6px;"></span>
            <label>Email referente</label>
            <input />
            <span class="nulla" style="margin-bottom: 6px;"></span>
            <label>Descrizione (XXXX caratteri)</label>
            <textarea></textarea>
            <a href="#" class="sendtravellink">Send Travel link</a>
        </div>
    </div>
    <uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
