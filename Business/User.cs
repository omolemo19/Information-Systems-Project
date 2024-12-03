using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandiHotelSystem.Business;

namespace PhumlaKamnandiHotelSystem
{
    public class User
    {
       
        #region data members
        private string id, personId, name, surname, phoneNumber, email;
        #endregion

        #region Properties
        public String ID
        {
            get { return id; }
            set { id = value; }
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

        public string Phone
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string Email
        {
            set { email = value; }
            get { return email; }
        }
        public string PersonalId
        {
            set { personId = value; }
            get { return personId; }
        }



        public Role role;

        #endregion

        #region Construtor
        public User()
        {
            id = "";
            name = "";
            Phone = "";
        }

        public User(Role.RoleType roleValue)
        {

            switch (roleValue)
            {
                case Role.RoleType.NoRole:
                    role = new Role();
                    break;
                case Role.RoleType.Guest:
                    role = new Guest();
                    break;
                case Role.RoleType.Admin:
                    role = new Admin();
                    break;
            }
        }



        #endregion

        #region ToStringMethod
        public override string ToString()
        {
            return name + '\n' + Phone;
        }
        #endregion
    }
}


