using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DVDEzy.DataLayer;

namespace DVDEzy.BusinessLayer {
    /// <summary>
    /// The DVDCopy class is used to store the copy information relating to a DVD. It will have data such
    /// as the barcode, a status indicating whether its available or unavailable, and also a id key to the parent
    /// DVD object (for ease in queries).
    /// Author: Lance Baker (c3128034).
    /// </summary>
    [Serializable]
    public class DVDCopy {
        private const string COLON = ": ";
        public enum Status { AVAILABLE, UNAVAILABLE }
        public int copyID { get; set; } // The primary key of the Copy.
        public int dvdID { get; set; } // The foriegn key to the DVD.
        public string barcode { get; set; } // The barcode of the copy.
        public Status status { get; set; } // The status.

        public DVDCopy(int dvdID, string barcode, Status status)
            : this(0, dvdID, barcode, status) {
        }

        public DVDCopy(int copyID, int dvdID, string barcode, int status)
            : this(copyID, dvdID, barcode, (Status)status) {
        }

        public DVDCopy(int copyID, int dvdID, string barcode, Status status) {
            this.copyID = copyID;
            this.dvdID = dvdID;
            this.barcode = barcode.Trim();
            this.status = status;
        }

        /// <summary>
        /// The dvd property will fetch the corresponding DVD object based on
        /// the dvdID stored.
        /// </summary>
        public DVD dvd {
            get {
                return DataDVD.Fetch(this.dvdID);
            }
        }

        /// <summary>
        /// The isAvailable property is a boolean test against status to see whether
        /// the copy is available.
        /// </summary>
        public Boolean isAvailable {
            get {
                return (this.status == Status.AVAILABLE);
            }
        }

        /// <summary>
        /// The toString will just show the copy details.
        /// </summary>
        /// <returns>The barcode and DVD title</returns>
        public override string ToString() {
            return this.barcode + COLON + this.dvd.title;
        }

        /// <summary>
        /// The UpdateCopy method is a static method, which is used to interact with the data layer.
        /// It will update the record relating to the copy object received.
        /// </summary>
        /// <param name="copy">The copy object.</param>
        /// <returns></returns>
        public static Boolean UpdateCopy(DVDCopy copy) {
            return DataDVDCopy.Update(copy);
        }

        /// <summary>
        /// The FetchCopy method is a static method, which is used to interact with the data layer.
        /// It will return the copy instance based on the received ID.
        /// </summary>
        /// <param name="id">The Copy primary key.</param>
        /// <returns>The DVDCopy instance</returns>
        public static DVDCopy FetchCopy(int id) {
            return DataDVDCopy.Fetch(id);
        }

        /// <summary>
        /// The RemoveCopy method is a static method, which is used to interact with the data layer.
        /// It will remove a copy relating to the received ID.
        /// </summary>
        /// <param name="id">The Copy primary key.</param>
        /// <returns>A boolean indicating success.</returns>
        public static Boolean RemoveCopy(int id) {
            return DataDVDCopy.Remove(id);
        }
    }
}
