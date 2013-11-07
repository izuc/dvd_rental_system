<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add_Member.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Add_Member" Title="Members" %>
<%@ Register Src="Form_Member.ascx" TagName="form" TagPrefix="memberForm" %>
<%@ Import Namespace="DVDEzy"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Add Member</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                 <memberForm:form ID="memberForm" runat="server" />
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>

