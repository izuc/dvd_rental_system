using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVDEzy.BusinessLayer;

namespace DVDEzy.DataLayer {
    /// <summary>
    /// The DataDVD class is used as a layer between the DVD and the DataSQL class. It contains the SQL statements
    /// (delcared as constants) and the methods which are used for interacting with the database.
    /// Author: Lance Baker (c3128034).
    /// </summary>
    public class DataDVD {
        private const string SQL_LIST = "SELECT * FROM DVD Order By title ASC";
        private const string SQL_FETCH = "SELECT * FROM DVD WHERE dvd_id = @dvdID";
        private const string SQL_INSERT = "INSERT INTO DVD (title, director, year, sale_price, rental_price) OUTPUT INSERTED.dvd_id VALUES (@Title, @Director, @Year, @SalePrice, @RentalPrice)";
        private const string SQL_DELETE = "DELETE FROM DVD WHERE dvd_id = @dvdID";
        private const string SQL_UPDATE = "UPDATE DVD SET title = @Title, director = @Director, year = @Year, sale_price = @SalePrice, rental_price = @RentalPrice WHERE dvd_id = @dvdID";

        /// <summary>
        /// The AddParameters method is used to convert the instance data of the received object
        /// into a dictionary (with the key corresponding to the data name used in the query).
        /// </summary>
        /// <param name="customer">The object to be converted.</param>
        /// <returns>The instance data as a dictionary.</returns>
        private static Dictionary<string, object> AddParameters(DVD dvd) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Title", dvd.title);
            parameters.Add("@Director", dvd.director);
            parameters.Add("@Year", dvd.yearReleased);
            parameters.Add("@SalePrice", dvd.salePrice);
            parameters.Add("@RentalPrice", dvd.rentalPrice);
            return parameters;
        }

        /// <summary>
        /// The Add method is used to insert the received object into the corresponding database table. It uses the
        /// AddParameters method to covert the object attributes into a dictionary, which is then passed
        /// into the DataSQL helper method along with the insert sql statement.
        /// </summary>
        /// <param name="dvd">The dvd object.</param>
        /// <returns>The inserted record's primary key.</returns>
        public static int Add(DVD dvd) {
            return DataSQL<DVD>.ScalarQuery(SQL_INSERT, AddParameters(dvd));
        }

        /// <summary>
        /// The Update method is used to update the record within the database table to the instance
        /// data of the received object. The AddParameters method is used to covert the instance data
        /// into a passable dictionary of data, which is then passed to the generic helper method of
        /// the DataSQL class along with the statement. The ID is also added at the end of the dictionary,
        /// since it is the last variable (within the where clause) of the update statement.
        /// </summary>
        /// <param name="dvd">The dvd object.</param>
        /// <returns>A int indicating the rows affected.</returns>
        public static int Update(DVD dvd) {
            Dictionary<string, object> parameters = AddParameters(dvd);
            parameters.Add("@dvdID", dvd.dvdID);
            return DataSQL<DVD>.NonQuery(SQL_UPDATE, parameters);
        }

        /// <summary>
        /// The Fetch method will find the record in the database, and return a newly instantiated object containing
        /// the record data. It uses the generic DataSQL Fetch method to dynamically instantiate the object using Reflection,
        /// which will return the instance.
        /// </summary>
        /// <param name="id">The primary key of the object desired to find.</param>
        /// <returns>The instance object.</returns>
        public static DVD Fetch(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@dvdID", id);
            return DataSQL<DVD>.Fetch(SQL_FETCH, parameters);
        }

        /// <summary>
        /// The Remove method is used to delete a record from the corresponding database table. It receives
        /// the primary key, and will use the generic helper method of the DataSQL class to remove the record.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>A boolean indicating whether the record was removed.</returns>
        public static Boolean Remove(int id) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@dvdID", id);
            return (DataSQL<DVD>.NonQuery(SQL_DELETE, parameters) > 0);
        }

        /// <summary>
        /// The List method is used to fetch all objects within the corresponding database table. It uses
        /// the generic List method of the DataSQL class to fetch the instances. It uses Reflection to dynamically
        /// instantiate the object based on the generic type. 
        /// </summary>
        /// <returns>Returns a List of the objects.</returns>
        public static List<DVD> List() {
            return DataSQL<DVD>.List(SQL_LIST, null);
        }
    }
}