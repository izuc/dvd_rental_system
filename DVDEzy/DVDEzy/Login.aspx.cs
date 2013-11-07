using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DVDEzy.BusinessLayer;

namespace DVDEzy {
    public partial class Login : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            // If the user is currently logged in
            if (Session["login"] != null) {
                // And if the logout parameter was passed in the query string
                if (Request.QueryString["logout"] != null) {
                    // Then it will remove the session
                    Session.Remove("login");
                    // And sign them out completely.
                    FormsAuthentication.SignOut();
                } else {
                    // Otherwise it will redirect them to the default page.
                    Response.Redirect("default.aspx");
                }
            }
        }

        /// <summary>
        /// The action associated with pressing the login button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e) {
            // Ensures the username & password fields were entered.
            if ((username.Text.Length > 0) && (password.Text.Length > 0)) {
                // Checks if the username and password is valid credentials
                if ((username.Text.Equals("david")) && (password.Text.Equals("smith"))) {
                    // Creates the login variable in the session.
                    Session["login"] = true;
                    // Redirects from the login page
                    FormsAuthentication.RedirectFromLoginPage("david", false);
                // Otherwise, it will show a error message.
                } else {
                    label_message.Text = "Error: Invalid login details.";
                }
            // Otherwise, it will show a error message.
            } else {
                label_message.Text = "Error: Must enter username and password.";
            }
        }
    }
}
