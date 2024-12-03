using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PhumlaKamnandiHotelSystem.Business;
using System.Runtime.ConstrainedExecution;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class Reserve
    {
        #region Data Members
        int BoookingId;
        int periodOfStay;
        decimal Total_Fee;

        //Collection<Booking> bookings;
        Collection<User> users;
        Collection<int> rooms;
        //Collection<Banks> banks;
        UserController userController;
        BookingController bookingController;
        //BankCardController bankCardController;

        #endregion


        public Reserve()
        {
            rooms = new Collection<int>();
            userController = new UserController();
            bookingController = new BookingController();
            //bankCardController = new BankCardController();
            users = userController.allUsers;
            //bookings = bookingController.AllBookings;
            //banks = bankCardController.allBanks;
            UpdateRooms();
        }

        #region Utility Method

        public void UpdateRooms()
        {

            rooms.Add(1);
            rooms.Add(2);
            rooms.Add(3);
            rooms.Add(4);
            rooms.Add(5);
        }

        public Collection<Booking> AllBookings
        {
            get { return bookingController.AllBookings; }
        }
        public Collection<int> makeBooking(DateTime checkIn, DateTime checkout)
        {
            Collection<int> AvailableRooms = new Collection<int>();
            foreach (Booking res in bookingController.AllBookings)
            {
                if ((checkIn >= DateTime.Parse(res.CheckIn)) && (checkIn < DateTime.Parse(res.CheckOut)))
                {
                    rooms.Remove(res.RoomId);
                }
            }

            foreach (int i in rooms)
            {
                AvailableRooms.Add(i);
            }
            UpdateRooms();
            return AvailableRooms;
        }

        public Collection<int> RoomsAvailable(DateTime checkIn)
        {
            Collection<int> AvailableRooms = new Collection<int>();
            foreach (Booking res in bookingController.AllBookings)
            {
                if (checkIn.Day == DateTime.Parse(res.CheckIn).Day)
                {
                    rooms.Remove(res.RoomId);
                }
            }

            foreach (int i in rooms)
            {
                AvailableRooms.Add(i);
            }
            UpdateRooms();
            return AvailableRooms;
        }

        public Collection<Booking> Occupancy(DateTime date)
        {
            Collection<Booking> Occupancy = new Collection<Booking>();
            foreach (Booking res in bookingController.AllBookings)
            {
                if (DateTime.Parse(res.CheckIn).Day == date.Day)
                {
                    Occupancy.Add(res);
                }
            }
            return Occupancy;


        }

        public int OccupancyByMonth(DateTime date)
        {
            int count = 0;
            foreach (Booking res in bookingController.AllBookings)
            {
                if (DateTime.Parse(res.CheckIn).Month == date.Month)
                {
                    count++;
                }
            }
            return count;
        }

        public int AddBooking(int guestId, int roomId, String checkIn, String checkOut, decimal Total_Fee)
        {
            Booking booking = new Booking(guestId, roomId, checkIn, checkOut, Total_Fee);
            bookingController.DataMaintenance(booking, DataBase.DB.DBOperation.Add);
            bookingController.FinalizeChanges(booking, DataBase.DB.DBOperation.Add);
            //bookingController.AllBookings.Add(booking);
            //bookings = bookingController.AllBookings;
            return booking.BookingID;
        }


        public void DeleteBooking(String bookingId)
        {
            var bookingToDelete = bookingController.Find(bookingId);

            if (bookingToDelete != null) 
            {
                // Call DataMaintenance to prepare for deletion
                //Debug.WriteLine(bookings.ToString);
                bookingController.DataMaintenance(bookingToDelete, DataBase.DB.DBOperation.Delete);
               
                // Finalize changes to apply deletion to the database
                bookingController.FinalizeChanges(bookingToDelete, DataBase.DB.DBOperation.Delete);
                //book = bookingController.AllBookings; // Refresh the in-memory list of bookings
            }

            /*bookingController.DataMaintenance(FindBooking(bookingId), DataBase.DB.DBOperation.Delete);
            bookingController.FinalizeChanges(DataBase.DB.DBOperation.Delete);
            bookings = bookingController.AllBookings;*/
        }



        public User FindGuest(string id)
        {
            User guest = null;
            foreach (User user in users)
            {
                if (user.role.getRoleValue == Role.RoleType.Guest)
                {
                    if (user.PersonalId.Equals(id))
                    {
                        guest = user;
                    }
                }
            }
            return guest;
        }

        public bool LogIn(string username, string password)
        {
            bool LogIn = false;
            foreach (User user in users)
            {
                if (user.role.getRoleValue == Role.RoleType.Admin)
                {
                    Admin admin = (Admin)user.role;
                    if (admin.Username.Equals(username))
                    {
                        if (admin.Password.Equals(password))
                        {
                            LogIn = true;
                        }
                    }
                }
            }

            return LogIn;
        }



        public Booking FindBooking(String id)
        {
            return bookingController.Find(id);
        }


        public void AddGuest(User aUser)
        {
            Debug.WriteLine("Inside AddGuest");
            userController.DataMaintenance(aUser, DataBase.DB.DBOperation.Add);
            userController.FinalizeChanges(aUser, DataBase.DB.DBOperation.Add);
        }


        public String GetId(User aUser)
        {
            String id = "";
            foreach (User user in users)
            {
                if (user.PersonalId.Equals(user.PersonalId))
                {
                    id = id + user.PersonalId;
                }
            }

            return id;
        }

        /*public Collection<CreditCardCompany> AllCreditCardCompanies()
        {
            return companies;
        }*/
        #endregion

    }
}
