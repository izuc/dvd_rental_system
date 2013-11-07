<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DVDs.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.DVDs" Title="DVDs"%>
<%@ Import Namespace="DVDEzy.BusinessLayer"%>
<%@ Import Namespace="System.Collections.Generic"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title">DVDs</div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label> <br />
                <asp:GridView ID="gridDVDS" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                    CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" OnPageIndexChanging="gridDVDS_PageIndexChanging" OnRowCommand="gridDVDS_RowCommand" DataKeyNames="dvdID"
                    AlternatingRowStyle-CssClass="alt" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="title" HeaderText="Title">
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="director" HeaderText="Director">
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="yearReleased" HeaderText="Year">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="salePrice" HeaderText="Sale Price" 
                            DataFormatString="${0}">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="rentalPrice" HeaderText="Rental Price" 
                            DataFormatString="${0}">
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
                        There are no DVD records.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>