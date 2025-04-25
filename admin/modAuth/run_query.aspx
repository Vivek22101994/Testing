<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="run_query.aspx.cs" Inherits="MagaRentalCE.admin.modRental.run_query" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="spManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="pnlGrid" runat="server">
            <ContentTemplate>
                <div style="width: 750px">
                    <div style="float: left">
                        <asp:TextBox ID="txt_query" runat="server" TextMode="MultiLine" Width="650px"></asp:TextBox>

                    </div>
                    <div style="float: right">
                        <asp:LinkButton runat="server" ID="btnRun" OnClick="btnRun_Click" OnClientClick="showLoader();"> Run Query </asp:LinkButton>
                    </div>
                </div>
                <div style="clear: both">
                    <div id="divProgress">
                        <div style="width: 100%; height: 100%; display: block;">
                            <div class="info_area" id="loader">
                                <img alt="loading" src="<%=CurrentAppSettings.ROOT_PATH%>admin/images/loader.gif" style="float: left;" /><span>Loading...</span>
                            </div>
                        </div>
                    </div>
                    <div id="divResult">
                        <asp:GridView ID="grdResult" runat="server" BorderStyle="Dashed" AutoGenerateColumns="true" AlternatingRowStyle-BackColor="WhiteSmoke"></asp:GridView>
                    </div>

                </div>

                <div id="divMessage" runat="server"></div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <script src="../../jquery/js/jquery-1.7.2.min.js"></script>
        <script type="text/javascript">
            function showLoader() {
                $('#divProgress').show();
                $('#divResult').hide();
                return true;
            }

            function hideLoader() {
                $('#divResult').show();
                $('#divProgress').hide();
            }
        </script>
    </form>

</body>
</html>
