<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View_DVD.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.View_DVD" Title="View DVD" %>
<%@ Register Src="Form_DVD.ascx" TagName="form" TagPrefix="dvdForm" %>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">View DVD</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <dvdForm:form ID="dvdForm" runat="server" Visible="False" />
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>