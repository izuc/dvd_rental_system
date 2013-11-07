<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Members.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.Members" Title="Members" %>
<%@ Import Namespace="DVDEzy.BusinessLayer"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">Members</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label> <br />
                <asp:GridView ID="gridMembers" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                    CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" OnPageIndexChanging="gridMembers_PageIndexChanging" OnRowCommand="gridMembers_RowCommand" DataKeyNames="customerID"
                    AlternatingRowStyle-CssClass="alt" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="firstName" HeaderText="First name" >
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="lastName" HeaderText="Last name" >
                            <HeaderStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="gender" HeaderText="Gender" >
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="streetAddress" HeaderText="Street Address" >
                            <HeaderStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="postcode" HeaderText="Postcode" >
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
	                            <asp:Button Text="View" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdView"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
	                            <asp:Button Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="cmdDelete"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="alt" />
                    <EmptyDataTemplate>
                        There are no Member records.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>
