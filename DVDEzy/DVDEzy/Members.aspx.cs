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
using System.Collections.Generic;
using DVDEzy.BusinessLayer;

namespace DVDEzy {
    public partial class Members : System.Web.UI.Page {
        /// <summary>
        /// The page load event adds datasource to the DataGridView, and binds it to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                DVDEzy.CheckSession();
                gridMembers.DataSource = Customer.List(); // Sets the datasource to the List reference.
                gridMembers.DataBind(); // Binds the objects.
            }
        }

        /// <summary>
        /// Fired when an action on a row is performed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMembers_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName != "Page") {
                // Gets the unique object key from the received command argument.
                int key = Convert.ToInt32(gridMembers.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString());
                // If the action is a delete.
                if (e.CommandName == "cmdDelete") {
                    if (Customer.Remove(key)) { // It will remove the object from the list.
                        this.lblMessage.Text = "A member has been deleted.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        gridMembers.DataSource = Customer.List(); // Sets the datasource to the List reference.
                        gridMembers.DataBind(); // Binds the objects.
                    } else {
                        this.lblMessage.Text = "Cannot Delete. Used elsewhere.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Visible = true;
                    }
                    // If the action is a view.
                } else if (e.CommandName == "cmdView") {
                    // It will redirect the user to the view member page, passing the key as a parameter.
                    Response.Redirect("view_member.aspx?view=" + key);
                }
            }
        }

        protected void gridMembers_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridMembers.PageIndex = e.NewPageIndex;
            gridMembers.DataSource = Customer.List(); // Sets the datasource to the List reference.
            gridMembers.DataBind(); // Binds the objects.
        }
    }
}
