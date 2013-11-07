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
    /// The TransactionItem is the DVDCopies that are added to a particular transaction. They each relate to
    /// a transaction, and a DVD Copy. It also has a type which indicates if it is a rental or a sale. It contains 
    /// methods for retrieving the related copy, updating the copy status, and also clearing the items from a transaction.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class TransactionItem {
        public enum TransactionType { RENTAL, SALE }
        public int transactionID { get; set; } // The transaction ID that the item is for.
        public int copyID { get; set; } // The Copy ID that the item is.
        public TransactionType type { get; set; } // The type of the transaction.

        public TransactionItem(int transactionID, int copyID, int type)
            : this(transactionID, copyID, (TransactionType) type) {
        }

        public TransactionItem(int transactionID, int copyID, TransactionType type) {
            this.transactionID = transactionID;
            this.copyID = copyID;
            this.type = type;
        }

        /// <summary>
        /// The copy property will fetch the DVDCopy based on the copyID.
        /// </summary>
        public DVDCopy copy {
            get {
                return DataDVDCopy.Fetch(this.copyID);
            }
        }

        /// <summary>
        /// The UpdateCopyStatus is a static method, which is used to interact with the data layer. It will
        /// update all the copies relating to a specific transaction to whether they are available or unavailable.
        /// </summary>
        /// <param name="transactionID">The transaction ID</param>
        /// <param name="status">The DVD Copy status.</param>
        public static void UpdateCopyStatus(int transactionID, DVDCopy.Status status) {
            DataTransactionItem.UpdateCopies(transactionID, status);
        }

        /// <summary>
        /// The Clear method is a static method, which is used to interact with the data layer. It will clear all
        /// the items that relate to a transaction ID.
        /// </summary>
        /// <param name="transactionID">The transaction</param>
        public static void Clear(int transactionID) {
            DataTransactionItem.Clear(transactionID);
        }
    }
}
