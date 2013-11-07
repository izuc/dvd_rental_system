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
    public partial class View_Transaction : System.Web.UI.Page {
        /// <summary>
        /// The page load event will Fetch the object which corresponds to the unqiue key received,
        /// and invoke the ShowRecord method on the form usercontrol passing the object as an argument.
        /// It will then set the visibility of the form to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            DVDEzy.CheckSession();
            try {
                Transaction transaction = Transaction.Fetch(Convert.ToInt32(Request.QueryString["view"]));
                if (transaction != null) {
                    transactionForm.ShowRecord(transaction);
                    transactionForm.Visible = true;
                }
            } catch (Exception ex) {}
        }
    }
}
