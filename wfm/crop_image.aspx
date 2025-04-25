<%@ Page Title="" Language="C#" MasterPageFile="~/wfm/MP.Master" AutoEventWireup="true" CodeBehind="crop_image.aspx.cs" Inherits="WebFileManager.wfm.crop_image" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head" runat="server">
	<link href="css/jquery.cropzoom.css" rel="stylesheet" type="text/css" />
	<link href="css/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />
	<script src="js/jquery.cropzoom.js" type="text/javascript"></script>

	<style type="text/css">
        .buttons
        {
            width: 150px;
        }
        .send_data, .restore_data
        {
            border: 1px solid #FFF;
            float: left;
            padding: 5px;
            width: auto;
            height: 20px;
            background: #333;
            color: #FFF;
            margin: 10px 0 0 5px;
        }
        .send_data:hover, .restore_data:hover
        {
            background: #FFF;
            color: #333;
            border: 1px solid #333;
            cursor: pointer;
        }
        .images {
            margin: 10px;
            width: 850px;
        }
        #cropzoom_container
        {
            float: left;
        }
        .result
        {
            float: left;
            margin: 10px;
            border: 1px solid #333;
            width: 400px;
            height: 300px;
        }
        .txt
        {
            width: 230px;
            margin: 75px auto auto 80px;
            position: absolute;
        }
    </style>

	<script type="text/javascript">
	var cropzoom;
		$(document).ready(function() {
			cropzoom = $('#cropzoom_container').cropzoom({
				width: <%=HF_imgContWidth.Value %>,
				height: <%=HF_imgContHeight.Value %>,
				bgColor: '#CCC',
				enableRotation: true,
				enableZoom: true,
				selector: {
					w: <%=HF_sel_w.Value %>,
					h: <%=HF_sel_h.Value %>,
					centered: true,
					borderColor: 'blue',
					borderColorHover: 'yellow',
					maxWidth: <%=HF_sel_mw.Value %>,
					maxHeight: <%=HF_sel_mh.Value %>,
					startWithOverlay: false,
					hideOverlayOnDragAndResize: true,
					aspectRatio: <%=HF_sel_ar.Value %>
				},
				image: {
					source: '/<%=HF_file_name.Value %>',
					width: <%=HF_imgWidth.Value %>,
					height: <%=HF_imgHeight.Value %>,
					minZoom: 10,
					maxZoom: 110
				}
			});
		});
		function cropSendData()
		{
			cropzoom.send('ProcessImage.ashx', 'POST', { imageCropped: $("#<%=HF_file_name_new.ClientID %>").val() }, function(imgRet) {
				$(".result").find(".txt").hide();
				$("#generated").attr("src", imgRet + '?' + (new Date()).getTime());
				$("#<%=HF_file_name_new.ClientID %>").val(imgRet); ;
			});
		}
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_main" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="HF_sel_ar" runat="server" Value="" />
			<asp:HiddenField ID="HF_sel_h" runat="server" Value="" />
			<asp:HiddenField ID="HF_sel_w" runat="server" Value="" />
			<asp:HiddenField ID="HF_sel_mh" runat="server" Value="" />
			<asp:HiddenField ID="HF_sel_mw" runat="server" Value="" />
			<asp:HiddenField ID="HF_callback" runat="server" Value="" />
			<asp:HiddenField ID="HF_return" runat="server" Value="" />
			<asp:HiddenField ID="HF_referer" runat="server" Value="" />
			<asp:HiddenField ID="HF_folder" runat="server" Value="" />
			<asp:HiddenField ID="HF_file_name" runat="server" Value="" />
			<asp:HiddenField ID="HF_file_name_new" runat="server" Value="" />
			<asp:HiddenField ID="HF_imgHeight" runat="server" Value="" />
			<asp:HiddenField ID="HF_imgWidth" runat="server" Value="" />
			<asp:HiddenField ID="HF_imgContHeight" runat="server" Value="" />
			<asp:HiddenField ID="HF_imgContWidth" runat="server" Value="" />
			<asp:HiddenField ID="HF_isVertical" runat="server" Value="false" />
			<div class="buttons">
				<a href="javascript:cropSendData()">Crop</a>
				<asp:LinkButton ID="lnk_save" runat="server" OnClick="lnk_save_Click">salva</asp:LinkButton>
				<div style="clear: both">
				</div>
			</div>
			<div style="clear: both">
			</div>
			<div class="images">
				<div id="cropzoom_container">
				</div>
				<%if(HF_isVertical.Value.Trim().ToLower()=="true"){ %>
				<div style="clear: both">
				</div>
				<%} %>
				<div class="result" style="width: <%=HF_imgContWidth.Value %>px; height: <%=HF_imgContHeight.Value %>px;">
					<div class="txt">
						Here you will see the cropped image</div>
					<img id="generated" src="<%=HF_file_name_new.Value %>" />
				</div>
				<div style="clear: both">
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
