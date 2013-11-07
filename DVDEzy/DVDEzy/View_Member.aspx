<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View_Member.aspx.cs" MasterPageFile="~/DVDEzy.Master" Inherits="DVDEzy.View_Member" Title="View Member" %>
<%@ Register Src="Form_Member.ascx" TagName="form" TagPrefix="memberForm" %>
<%@ Import Namespace="DVDEzy.BusinessLayer"%>
<asp:Content ID="content" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content_container">
        <div id="content_header">
            <div id="content_title"><asp:Label ID="lblMemberTitle" runat="server" Text=""></asp:Label></div>
        </div>
        <div id="content_repeat">
            <div id="content_inner">
                <div id="tabs">
			        <ul>
				        <li><a href="#tabs-1">Details</a></li>
				        <li><a href="#tabs-2">Transactions</a></li>
			        </ul>
			        <div id="tabs-1"><memberForm:form ID="memberForm" runat="server" Visible="False" /></div>
			        <div id="tabs-2">
                        <asp:Label ID="lblMessage" runat="server" Text="" Visible="False" Font-Bold="True" Font-Italic="True" Width="100%"></asp:Label> <br />
                        <asp:GridView ID="gridTransactions" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" GridLines="None" OnPageIndexChanging="gridTransactions_PageIndexChanging" OnRowCommand="gridTransactions_RowCommand" DataKeyNames="transactionID"
                            AlternatingRowStyle-CssClass="alt" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="transactionDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}">
                                    <HeaderStyle Width="25%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quantityRented" HeaderText="Qty. Rented">
                                    <HeaderStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quantityPurchased" HeaderText="Qty. Purchased">
                                    <HeaderStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="totalCost" HeaderText="Total Cost" DataFormatString="${0}">
                                    <HeaderStyle Width="25%" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
	                                    <asp:Button Text="View" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CausesValidation="false" OnClientClick="return validateAddCard(false);" CommandName="cmdView"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
	                                    <asp:Button Text="Delete" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CausesValidation="false" OnClientClick="return validateAddCard(false);" CommandName="cmdDelete"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle CssClass="alt" />
                            <EmptyDataTemplate>
                                There are no Transaction records.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
		        </div>
                <script type="text/javascript">
                    $('#tabs').tabs();
                </script>
            </div> 
        </div>
        <div id="content_footer"></div>
    </div>
</asp:Content>