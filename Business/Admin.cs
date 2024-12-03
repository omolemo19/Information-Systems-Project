using System;
using System.Data;

namespace PhumlaKamnandiHotelSystem.Business
{
    class Admin : Role
    {
        #region Data Members

        private string name;
        private string surname;
        private string username;
        private string password;
        private string phoneNumber;
        private string email;
        private int adminID;

        #endregion

        #region Constructor
        public Admin() : base()
        {
            getRoleValue = RoleType.Admin;  // Set the role as Admin
        }
        #endregion

        #region Property Methods

        public int AdminID
        {
            get { return adminID; }
            set { adminID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        #endregion
    }
}
