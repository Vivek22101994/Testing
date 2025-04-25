<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_static_collection.ascx.cs" Inherits="RentalInRome.WLRental.uc.UC_static_collection" %>
<asp:HiddenField runat="server" ID="HF_id" Value="0" />
<asp:HiddenField runat="server" ID="HF_pid_lang" Value="0" />
<asp:ListView ID="LV" runat="server" DataSourceID="LDS">
	<ItemTemplate>
		<%# Eval("description") %>
	</ItemTemplate>
	<EmptyDataTemplate>
		&nbsp;
	</EmptyDataTemplate>
	<LayoutTemplate>
		<a id="itemPlaceHolder" runat="server" />
	</LayoutTemplate>
</asp:ListView>
<asp:LinqDataSource ID="LDS" runat="server" ContextTypeName="RentalInRome.data.magaContent_DataContext" OrderBy="sequence" TableName="CONT_VIEW_RL_COLLECTION_BLOCKs" Where="pid_collection == @pid_collection &amp;&amp; pid_lang == @pid_lang">
	<WhereParameters>
		<asp:ControlParameter ControlID="HF_id" Name="pid_collection" PropertyName="Value" Type="Int32" />
		<asp:ControlParameter ControlID="HF_pid_lang" Name="pid_lang" PropertyName="Value" Type="Int32" />
	</WhereParameters>
</asp:LinqDataSource>