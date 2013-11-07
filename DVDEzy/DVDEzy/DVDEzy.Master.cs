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

namespace DVDEzy
{
    public partial class DVDEzy : System.Web.UI.MasterPage
    {
        public static void CheckSession() {
            if (HttpContext.Current.Session["login"] == null) {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e) {

        }
    }
}
