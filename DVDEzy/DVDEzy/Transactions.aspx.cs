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
    public partial class Transactions : System.Web.UI.Page {
        /// <summary>
        /// The page load event adds datasource to the DataGridView, and binds it to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                DVDEzy.CheckSession();
                gridTransactions.DataSource = Transaction.List();  // Sets the datasource to the List reference.
                gridTransactions.DataBind(); // Binds the objects.
            }
        }

        /// <summary>
        /// Fired when an action on a row is performed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridTransactions_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName != "Page") {
                // Gets the unique object key from the received command argument.
                int key = Convert.ToInt32(gridTransactions.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString());
                if (e.CommandName == "cmdDelete") { // If the action is a delete.
                    if (Transaction.Remove(key)) { // It will remove the object from the list.
                        this.lblMessage.Text = "A transaction has been deleted.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        gridTransactions.DataSource = Transaction.List();  // Sets the datasource to the List reference.
                        gridTransactions.DataBind(); // Binds the objects.
                    } else {
                        this.lblMessage.Text = "Cannot Delete. Used elsewhere.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Visible = true;
                    }
                } else if (e.CommandName == "cmdView") { // If the action is a view.
                    // It will redirect the user to the view transaction page, passing the key as a parameter.
                    Response.Redirect("view_transaction.aspx?view=" + key);
                }
            }
        }

        protected void gridTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridTransactions.PageIndex = e.NewPageIndex;
            gridTransactions.DataSource = Transaction.List();  // Sets the datasource to the List reference.
            gridTransactions.DataBind(); // Binds the objects.
        }
    }
}
