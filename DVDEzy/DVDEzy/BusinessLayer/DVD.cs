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
using System.Collections.Generic;
using DVDEzy.DataLayer;
using System.Runtime.Serialization;

namespace DVDEzy.BusinessLayer {
    /// <summary>
    /// The DVD class stores the title of the dvd, the director, 
    /// the year released, and the price for both renting & selling.
    /// The DVD also contains a list of DVDCopy objects, which represent the physical copies in the store. 
    /// With each copy containing a barcode (to identify the DVD) and also a field stating the availability; which
    /// gets changed depending on whether the copy has been rented out/ or purchased. 
    /// Author: Lance Baker (c3128034)
    /// </summary>
    public class DVD {
        public int dvdID { get; private set; } // A unqiue identifier which represents the object.
        public string title { get; set; }
        public string director { get; set; }
        public int yearReleased { get; set; }
        public double salePrice { get; set; }
        public double rentalPrice { get; set; }

        public DVD() {
        }

        public DVD(string title, string director, int yearReleased, double salePrice, double rentalPrice) 
            : this(0, title, director, yearReleased, salePrice, rentalPrice) {
        }

        public DVD(int dvdID, string title, string director, int yearReleased, decimal salePrice, decimal rentalPrice)
            : this(dvdID, title, director, yearReleased, (double)salePrice, (double)rentalPrice) {
        }

        public DVD(int dvdID, string title, string director, int yearReleased, double salePrice, double rentalPrice) {
            this.dvdID = dvdID;
            this.title = title;
            this.director = director;
            this.yearReleased = yearReleased;
            this.salePrice = salePrice;
            this.rentalPrice = rentalPrice;
        }

        public List<DVDCopy> copies {
            get {
                return DataDVDCopy.List(this.dvdID);
            }
        }

        public Boolean AddCopy(string barcode, DVDCopy.Status status) {
            return DataDVDCopy.Add(new DVDCopy(this.dvdID, barcode, status));
        }

        /// <summary>
        /// The Save method adds the Customer (this) object to the internal data structure.
        /// </summary>
        public void Save() {
            if (this.dvdID > 0) {
                DataDVD.Update(this);
            } else {
                this.dvdID = DataDVD.Add(this);
            }
        }

        /// <summary>
        /// The static Fetch method interacts with the DataLayer to retrieve a object 
        /// from the internal data structure based on an ID key.
        /// </summary>
        /// <param name="id">The ID key of the object</param>
        /// <returns>The DVD found</returns>
        public static DVD Fetch(int id) {
            return DataDVD.Fetch(id);
        }

        /// <summary>
        /// The static Add method receives a DVD object 
        /// and adds it to the internal data structure in the DataLayer.
        /// </summary>
        /// <param name="dvd">The DVD that you want added to the data structure</param>
        public static void Add(DVD dvd) {
            DataDVD.Add(dvd);
        }

        /// <summary>
        /// The static Remove method interacts with the DataLayer in order to
        /// remove a object corresponding to the received ID key.
        /// </summary>
        /// <param name="id">The ID key corresponding to the object that you want removed</param>
        public static Boolean Remove(int id) {
            return DataDVD.Remove(id);
        }

        /// <summary>
        /// The List method recieves the data structure as a List.
        /// </summary>
        /// <returns>A List of the DVD objects</returns>
        public static List<DVD> List() {
            return DataDVD.List();
        }
    }
}
