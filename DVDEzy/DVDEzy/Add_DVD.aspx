<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add_DVD.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Add_DVD" Title="Add DVD" %>
<%@ Register Src="Form_DVD.ascx" TagName="form" TagPrefix="dvdForm" %>
<%@ Import Namespace="DVDEzy"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Add DVD</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <dvdForm:form ID="dvdForm" runat="server" />
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>