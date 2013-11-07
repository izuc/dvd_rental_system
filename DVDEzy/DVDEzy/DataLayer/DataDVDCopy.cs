using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataDVDCopy class is used as a layer between the DVDCopy and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataDVDCopy {
        private const string SQL_LIST = "SELECT * FROM DVDCopy WHERE dvd_id = @dvdID Order By barcode ASC";
        private const string SQL_FETCH = "SELECT * FROM DVDCopy WHERE copy_id = @copyID";
        private const string SQL_DELETE = "DELETE FROM DVDCopy WHERE copy_id = @copyID";
        private const string SQL_INSERT = "INSERT INTO DVDCopy(dvd_id, barcode, status) VALUES (@dvdID, @Barcode, @Status)";
        private const string SQL_UPDATE = "UPDATE DVDCopy SET barcode = @Barcode, status = @Status WHERE copy_id = @copyID";

        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. 
        /// It coverts the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="copy">The dvd copy object.</param>
        /// <returns>A boolean indicating whether the record was inserted.</returns>
        public static Boolean Add(DVDCopy copy) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@dvdID", copy.dvdID);
            parameters.Add("@Barcode", copy.barcode);
            parameters.Add("@Status", (int)copy.status);
            return (DataSQL<DVDCopy>.NonQuery(SQL_INSERT, parameters) > 0);
        }

        /// <summary>
        /// The Update method is used to update the record within the database table to the instance
        /// data of the received object. It then coverts the instance data
        /// into a passable dictionary of data, which is then passed to the generic helper method of
        /// the DataSQL class along with the statement. The ID is also added at the end of the dictionary,
        /// since it is the last variable (within the where clause) of the update statement.
        /// </summary>
        /// <param name="copy">The dvd copy object.</param>
        /// <returns>A boolean indicating whether the record was updated.</returns>
        public static Boolean Update(DVDCopy copy) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Barcode", copy.barcode);
            parameters.Add("@Status", (int)copy.status);
            parameters.Add("@copyID", copy.copyID);
            return (DataSQL<DVDCopy>.NonQuery(SQL_UPDATE, parameters) > 0);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="id">The primary key of the object desired to find.</param>
        /// <returns>The instance object.</returns>
        public static DVDCopy Fetch(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@copyID", id);
            return DataSQL<DVDCopy>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Remove method is used to delete a record from the corresponding database table. It receives
        /// the primary key, and will use the generic helper method of the DataSQL class to remove the record.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>A boolean indicating whether the record was removed.</returns>
        public static Boolean Remove(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@copyID", id);
            return (DataSQL<DVDCopy>.NonQuery(SQL_DELETE, parameters) > 0);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <param name="id">The DVD primary key.</param>
        /// <returns>Returns a List of the DVD Copies.</returns>
        public static List<DVDCopy> List(int dvdID) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@dvdID", dvdID);
            return DataSQL<DVDCopy>.List(SQL_LIST, parameters);
        }
    }
}