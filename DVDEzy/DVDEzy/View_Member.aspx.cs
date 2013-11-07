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
    public partial class View_Member : System.Web.UI.Page {
        /// <summary>
        /// The page load event will Fetch the object which corresponds to the unqiue key received,
        /// and invoke the ShowRecord method on the form usercontrol passing the object as an argument.
        /// It will then set the visibility of the form to true. It will also show the transactions relating
        /// to the specific customer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
                DVDEzy.CheckSession();
                try {
                    Customer customer = Customer.Fetch(Convert.ToInt32(Request.QueryString["view"]));
                    if (customer != null) {
                        memberForm.ShowRecord(customer);
                        memberForm.Visible = true;
                        this.lblMemberTitle.Text = "Member: " + customer.fullName;
                        this.lblMemberTitle.Visible = true;
                        if (!IsPostBack) {
                            gridTransactions.DataSource = Transaction.List(Convert.ToInt32(Request.QueryString["view"]));  // Sets the datasource to the List reference.
                            gridTransactions.DataBind(); // Binds the objects.
                        }
                    }
                }
                catch (Exception ex) { }
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
                        Response.Redirect("view_member.aspx?view=" + Convert.ToInt32(Request.QueryString["view"]) + "#tabs-2");
                    }
                } else if (e.CommandName == "cmdView") { // If the action is a view.
                    // It will redirect the user to the view transaction page, passing the key as a parameter.
                    Response.Redirect("view_transaction.aspx?view=" + key);
                }
            }
        }

        protected void gridTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridTransactions.PageIndex = e.NewPageIndex;
            try {
                gridTransactions.DataSource = Transaction.List(Convert.ToInt32(Request.QueryString["view"]));  // Sets the datasource to the List reference.
                gridTransactions.DataBind(); // Binds the objects.
            }
            catch (Exception ex) { }
        }
    }
}
