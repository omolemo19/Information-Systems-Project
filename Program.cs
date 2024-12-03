using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhumlaKamnandiHotelSystem.Business;
using PhumlaKamnandiHotelSystem.DataBase;

namespace PhumlaKamnandiHotelSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            DBtester dBtester = new DBtester();
            dBtester.Start();*/

            /*BookingController bookingController = new BookingController();
            Booking aBooking = new Booking(2, 1, "2023/08/19 00:00", "2023/08/19 00:00", 900);
            bookingController.DataMaintenance(aBooking, DB.DBOperation.Add);
            bookingController.FinalizeChanges(aBooking, DB.DBOperation.Add);*/

            User aUser = new User(Role.RoleType.Guest);
            aUser.Name = " ";
            aUser.Surname = " ";
            aUser.Phone = " ";
            aUser.Email = " ";

            UserController userController = new UserController();
            userController.DataMaintenance( aUser ,DB.DBOperation.Add);
            userController.FinalizeChanges(aUser ,DB.DBOperation.Add);
        }
    }
}
