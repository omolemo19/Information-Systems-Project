using System;
using System.Data;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class Guest : Role
    {
        #region Data Members
        private int guestID;
        private string name;
        private string surname;
        private string phoneNumber;
        private string email;
        private string bankCardNumber;
        private string address;
        private string postalCode;
        #endregion

        #region Constructor
        public Guest() : base()
        {
            getRoleValue = RoleType.Guest;  // Set the role as Guest
        }
        #endregion

        #region Property Methods

        public int GuestID
        {
            get { return guestID; }
            set { guestID = value; }
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

        public string BankCardNumber
        {
            get { return bankCardNumber; }
            set { bankCardNumber = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }
        #endregion
    }
}
