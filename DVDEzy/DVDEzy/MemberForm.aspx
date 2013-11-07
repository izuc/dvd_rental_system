                        <form id="loginForm" runat="server">
                            <asp:HiddenField ID="customerID" runat="server" />
                            <table>
                                <tr>
                                    <td>Firstname: </td>
                                    <td><asp:TextBox ID="firstName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Last name: </td>
                                    <td><asp:TextBox ID="lastName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Gender</td>
                                    <td>
                                        <asp:RadioButtonList id="gender" runat="server">
                                            <asp:ListItem>Male</asp:ListItem>
                                            <asp:ListItem>Female</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Street address:</td>
                                    <td><asp:TextBox ID="streetAddress" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Billing address:</td>
                                    <td><asp:TextBox ID="billingAddress" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Postcode</td>
                                    <td><asp:TextBox ID="postcode" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" /></td>
                                </tr>
                            </table>
                        </form>