<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_loader.ascx.cs" Inherits="RentalInRome.admin.uc.UC_loader" %>
<div style="width:100%;height:100%;z-index:999;position:fixed;top:0;left:0;display:block;">
    <div class="info_area" id="loader">
	    <img alt="loading" src="<%=CurrentAppSettings.ROOT_PATH%>admin/images/loader.gif" style="float:left;"/><span>Loading...</span>
    </div>
</div>
