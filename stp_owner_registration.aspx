<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_owner_registration.aspx.cs" Inherits="RentalInRome.stp_owner_registration" %>

<%@ Register Src="uc/UC_menu_breadcrumb.ascx" TagName="UC_menu_breadcrumb" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal> 
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <asp:HiddenField ID="HF_unique" Value="" runat="server" />
    <asp:HiddenField ID="HF_num_persons_max" runat="server" Value="0" />
    <uc1:UC_menu_breadcrumb ID="UC_menu_breadcrumb1" runat="server" />
    


    <div id="contatti">
        
        <div style="float:left; width:255px; padding-left:25px;">
        <img src="<%=CurrentAppSettings.ROOT_PATH %>images/css/owner.jpg" alt="owner registration" style=" float: left; margin:0 0 10px -25px;" />
        <div class="nulla">
        </div>
         <% if (ltr_description.Text != "")
                   {%>
                <%= ltr_description.Text%>
                <% }
                   else
                   { %>
                
        <strong style="font-size: 14px; color: #333366;">
Do you want to insert your apartment into RentalinRome.com web site ?
</strong>
<br/>
<br/>
No expense is previewed, contacts our Staff to the following telephone numbers:
<br/><br/>
<ul class="detailsList">
<li><span class="skype_pnh_print_container_1314947354">+39 06 99320047</span><span class="skype_pnh_container" dir="ltr" tabindex="-1"><span class="skype_pnh_mark"></span></span></li>
<li><span class="skype_pnh_print_container_1314947354">+39 06 9905533</span><span class="skype_pnh_container" dir="ltr" tabindex="-1"><span class="skype_pnh_mark"></span></span>;</li>
</ul>
<div class="nulla">
</div>
<br/>
Or send us a <strong>fax</strong> to this number: 
<br/><br/>
<ul class="detailsList">
<li><span class="skype_pnh_print_container_1314947354">+39 06 23328717</span><span class="skype_pnh_container" dir="ltr" tabindex="-1"><span class="skype_pnh_mark"></span></span></li>
</ul>
<div class="nulla">
</div>
<br/>
or an <strong>email</strong> to:
<br/><br/>
<ul class="detailsList">
<li><a href="mailto:info@rentalinrome.com">info@rentalinrome.com</a> </li>
</ul>
<div class="nulla">
</div>
<br/>
Rental in Rome reserves the right to approve of every publication to
the inside of the own pages web.
<br/>
<br/>
    
    <% } %>
        </div>
         
            <div style="float:left;">
        <h2 class="underlined" style="margin-top:0; text-transform:uppercase;">
        Owner Registration form</h2>
        </div>

        <div class="dx" id="contatti_dx">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <div id="pnl_request_sent" class="box_client_booking" runat="server" visible="false" style="width: 545px;">
                <%=CurrentSource.getSysLangValue("reqRequestSent")%>
            </div>
            <div id="pnl_request_cont" class="box_client_booking" runat="server" style="width: 545px;  margin-bottom:0;">
                <div id="errorLi" class="line" style="color: red; margin-bottom: 30px; width: 550px; display: none;">
                    <h3 id="errorMsgLbl">
                        <%=CurrentSource.getSysLangValue("reqThereWasProblem")%></h3>
                    <p id="errorMsg">
                        <%=CurrentSource.getSysLangValue("reqErrorsHaveBeenHighlighted")%>
                    </p>
                    <div class="nulla">
                    </div>
                </div>
             

                <div class="line">
                    <div id="txt_email_cont" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblName")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="txt_email" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="txt_email_check" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div id="Div1" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblSurname")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox1" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span1" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>

                <div class="line">
                    <div id="Div2" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("reqEmail")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox2" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span2" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div id="Div3" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblPhone")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox3" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span3" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>

                <div class="line">
                    <div id="drp_country_cont" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("reqLocation")%>*
                        </label>
                        <div>
                            <asp:DropDownList runat="server" ID="drp_country" CssClass="field select large" Style="width: 262px;" OnDataBound="drp_country_DataBound" DataSourceID="LDS_country" DataTextField="title" DataValueField="id" TabIndex="5">
                            </asp:DropDownList>
                            <asp:LinqDataSource ID="LDS_country" runat="server" ContextTypeName="RentalInRome.data.magaLocation_DataContext" OrderBy="title" TableName="LOC_LK_COUNTRies" Where="is_active == @is_active">
                                <WhereParameters>
                                    <asp:Parameter DefaultValue="1" Name="is_active" Type="Int32" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <span id="drp_country_check" class="alertErrorSmall" style="width: 262px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredCountrySelect")%></span>
                        </div>
                    </div>
                   <div id="Div4" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblCity")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox4" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span4" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>

                <div class="line">
                    <div id="Div5" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblZone")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox5" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span5" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div id="Div6" class="left">
                        <label class="desc">
                            CAP/Zip Code*
                        </label>
                        <div>
                            <asp:TextBox ID="TextBox6" runat="server" Style="width: 254px;"></asp:TextBox>
                            <span id="Span6" class="alertErrorSmall" style="width: 254px; float: none; display: none;"></span>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>
                
                <div class="line">
                    <div id="txt_name_first_cont" class="left">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblAddress")%>*
                        </label>
                        <div>
                            <asp:TextBox ID="txt_name_full" runat="server" Style="width: 526px;"></asp:TextBox>
                            <span id="txt_name_full_check" class="alertErrorSmall" style="width: 500px; float: none; display: none;">
                                <%=CurrentSource.getSysLangValue("reqRequiredField")%></span>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>

                <div class="line">
                    <div class="left check_list">
                        <label class="desc">
                            Note
                        </label>
                        <div>
                            <textarea id="txt_note" runat="server" rows="10" cols="50" tabindex="27" style="height: 150px; width: 530px; border:1px solid #BCBCCF"></textarea>
                        </div>
                    </div>
                    <div class="nulla">
                    </div>
                </div>

                <div class="line2" style="width: 500px;">
                    <div class="left" style="width: 490px;">
                        <label class="desc">
                            <%=CurrentSource.getSysLangValue("lblPrivacyPolicy")%>
                            <asp:HyperLink ID="HL_getPdf_privacy" runat="server" Target="_blank">
                                    <img src="/images/ico/ico_pdf_100.png" alt="pdf" title="download pdf" style='height: 30px;' />
                            </asp:HyperLink>
                        </label>
                        <div class="div_terms" style="width: 470px;">
                            <asp:Literal ID="ltr_privacy" runat="server"></asp:Literal>
                        </div>
                        <div class="accettocheck" style="height: 20px;" id="chk_privacyAgree_cont">
                            <input type="checkbox" id="chk_privacyAgree" />
                            <label for="chk_privacyAgree" style="width: auto;">
                                <%=CurrentSource.getSysLangValue("lblAcceptPersonalDataPrivacy")%>*</label>
                        </div>
                        <span id="chk_privacyAgree_check" class="alertErrorSmall" style="width: 300px; float: none; display: none;">
                            <%=CurrentSource.getSysLangValue("reqRequiredAcceptPrivacy")%></span>
                    </div>
                   
                    <div class="nulla">
                    </div>
                </div>
                
                
                <asp:LinkButton ID="lnk_send" CssClass="btn bonifico" runat="server" OnClick="lnk_send_Click" OnClientClick="return RNT_validateRequestForm()"><span>Send Request</span></asp:LinkButton>
            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
        <div class="nulla">
        </div>
    </div>
    <uc3:uc_apt_in_rome_bottom id="UC_apt_in_rome_bottom1" runat="server" />
</asp:Content>
