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
    public partial class DVDs : System.Web.UI.Page {
        /// <summary>
        /// The page load event adds datasource to the DataGridView, and binds it to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                DVDEzy.CheckSession();
                // Binds the DataGridView to the DVD List
                gridDVDS.DataSource = DVD.List();
                gridDVDS.DataBind();
            }
        }

        /// <summary>
        /// Fired when an action on a row is performed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDVDS_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName != "Page") {
                // Gets the ID key that was passed as the command argument
                int key = Convert.ToInt32(gridDVDS.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString());
                // If it was the delete button
                if (e.CommandName == "cmdDelete") {
                    // It will remove the object
                    if (DVD.Remove(key)) {
                        this.lblMessage.Text = "A DVD has been deleted.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Visible = true;
                        gridDVDS.DataSource = DVD.List();
                        gridDVDS.DataBind();
                    } else {
                        this.lblMessage.Text = "Cannot Delete. Used elsewhere.";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Visible = true;
                    }
                    // Otherwise, if it was the view button
                } else if (e.CommandName == "cmdView") {
                    // It will redirect the user to the view dvd page passing the key as a parameter.
                    Response.Redirect("view_dvd.aspx?view=" + key);
                }
            }
        }

        protected void gridDVDS_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            gridDVDS.PageIndex = e.NewPageIndex;
            gridDVDS.DataSource = DVD.List();
            gridDVDS.DataBind();
        }
    }
}
