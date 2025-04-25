<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_footer.ascx.cs" Inherits="RentalInRome.WLRental.uc.UC_footer" %>
<%@ Register src="UC_static_block.ascx" tagname="UC_static_block" tagprefix="uc1" %>
<asp:HiddenField ID="HF_pid_lang" runat="server" Value="1" />
		<uc1:UC_static_block ID="UC_static_block1" runat="server" BlockID="0" />

<table class="indirizzi">
	<tr>
		<td align="center" valign="middle" style="width:100%">
			 <strong><%= WL.getWLName() %></strong> - © 2015 all rights reserved - <a href="http://www.magarental.com" target="_blank">magarental</a>
		</td>
		
	</tr>
</table>

<div class="nulla">
</div>


<asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
</asp:PlaceHolder>



<%--<table class="members">
	
	<tr>
		
		<td align="center" valign="middle" style="width: 590px;" id="shinystatContent">
            <asp:PlaceHolder ID="PH_shinystat_tmp" runat="server" Visible="false">
                <script type="text/javascript">
                    var shinystatContent = ''
  			            //+ '<' + 'script type="text/javascript" language="JavaScript" src="http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1"><' + '/' + 'script' + '>'
                        + '<' + 'noscript>'
                        + '    <a href="http://www.shinystat.it/cgi-bin/shinystatv.cgi?USER=rentalinromejack" target="_top">'
                        + '        <img src="http://www.shinystat.it/cgi-bin/shinystat.cgi?USER=rentalinromejack&NC=1" border="0" /></a>'
                        + '</' + 'noscript' + '>';

                    $(document).ready(function () {
                        $(window).load(function () {
                            $('#shinystatContent').html(shinystatContent);
                            //var script = document.createElement('script');
                            //script.type = 'text/javascript';
                            //script.src = "http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1";
                            //$("#shinystatContent").append(script);
                            var th = document.getElementById("shinystatContent");
                            var s = document.createElement('script');
                            s.setAttribute('type', 'text/javascript');
                            s.setAttribute('src', "http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1");
                            th.appendChild(s);

                        });
                    });
                </script>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PH_shinystat" runat="server" Visible="false">
                <script type="text/javascript" language="JavaScript" src="http://codicepro.shinystat.it/cgi-bin/getcod.cgi?USER=rentalinromejack&P=1"></script>
                <noscript>
                    <a href="http://www.shinystat.it/cgi-bin/shinystatv.cgi?USER=rentalinromejack" target="_top">
                        <img src="http://www.shinystat.it/cgi-bin/shinystat.cgi?USER=rentalinromejack&NC=1" border="0" /></a>
                </noscript>
            </asp:PlaceHolder>
        </td>
		
	</tr>
</table>--%>



