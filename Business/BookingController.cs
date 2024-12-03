using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PhumlaKamnandiHotelSystem.DataBase;
using PhumlaKamnandiHotelSystem.Business;
using PhumlaKamnandiHotelSystem.Data;
using static PhumlaKamnandiHotelSystem.DataBase.DB;
using System.Diagnostics;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class BookingController
    {
        #region Data Members
        BookingDB bookingDB;
        Collection<Booking> bookings;
        #endregion

        #region Properties
        public Collection<Booking> AllBookings
        {
            get
            {
                return bookings;
            }
        }
        #endregion

        #region Constructor
        public BookingController()
        {
            // Instantiate the BookingDB object to communicate with the database
            bookingDB = new BookingDB();
            bookings = bookingDB.AllBookings;
        }
        #endregion

        #region Database Communication

        public void DataMaintenance(Booking aBooking, DB.DBOperation operation)
        {
            Debug.WriteLine("Inside DataMaintenance");
            Debug.WriteLine($"Operation: {operation}, Booking ID: {aBooking.BookingID}");

            int index = 0;
            bookingDB.DataSetChange(aBooking, operation);

            switch (operation)
            {
                case DBOperation.Add:
                    Debug.WriteLine("Operation is Add");
                    bookings.Add(aBooking);
                    break;
                case DBOperation.Edit:
                    Debug.WriteLine("Operation is Edit");
                    index = FindIndex(aBooking);
                    bookings[index] = aBooking;
                    break;
                case DB.DBOperation.Delete:
                    index = FindIndex(aBooking);
                    bookings.RemoveAt(index);
                    break;


            }
        }


        public bool FinalizeChanges(Booking aBooking, DB.DBOperation operation)
        {
            // Commit changes to the database
            return bookingDB.UpdateDataSource(aBooking, operation);
        }
        #endregion

        #region Searching through a collection

        // Find a booking by its ID using LINQ
        public Booking Find(string ID)
        {
            int bookingID = Convert.ToInt32(ID);
            return bookings.FirstOrDefault(b => b.BookingID == bookingID);
        }

        // Find the index of a specific booking in the collection
        public int FindIndex(Booking abook)
        {
            return bookings.IndexOf(bookings.FirstOrDefault(b => b.BookingID == abook.BookingID));
        }

        #endregion
    }
}
