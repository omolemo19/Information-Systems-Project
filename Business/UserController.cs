using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PhumlaKamnandiHotelSystem.DataBase;
using PhumlaKamnandiHotelSystem.Data;
using System.Diagnostics;
using System.Data;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class UserController
    {

        #region Data Members

        UserDB userDB;
        Collection<User> users;

        #endregion

        #region Properties

        public Collection<User> allUsers
        {
            get
            {
                return users;
            }
        }
        #endregion

        #region Constructor
        public UserController()
        {
            //***instantiate the UserDB object to communicate with the database
            userDB = new UserDB();
            users = userDB.AllUsers;
        }

        #endregion

        #region Database Communication

        public void DataMaintenance(User aUser, DB.DBOperation operation)
        {
            Debug.WriteLine("Inside DataMaintenance");
            int index = 0;
            userDB.DataSetChange(aUser, operation);

            switch (operation)
            {
                case DB.DBOperation.Add:
                    Debug.WriteLine("operation is Add");
                    users.Add(aUser);
                    break;

                case DB.DBOperation.Edit:
                    index = FindIndex(aUser);
                    users[index] = aUser;
                    break;

                case DB.DBOperation.Delete:
                    index = FindIndex(aUser);
                    users.RemoveAt(index);
                    break;

            }
        }

        public bool FinalizeChanges(User user, DB.DBOperation operation)
        {
            Debug.WriteLine("Inside FinalizeChanges");
            return userDB.UpdateDataSource(user, operation);
        }
        #endregion

        #region Searching through a collection

        /*public Collection<User> FindByRole(Collection<User> users, Role.RoleType roleVal)
        {
            Collection<User> matches = new Collection<User>();
            foreach (User user in users)
            {
                if (user.role.getRoleValue == roleVal)
                {
                    matches.Add(user);
                }
            }
            return matches;
        }*/

        public User Find(string ID)
        {
            int index = 0;
            bool found = (users[index].PersonalId.Equals(ID));
            int count = users.Count;
            while (!(found) && (index < users.Count - 1))
            {
                index = index + 1;
                found = (users[index].PersonalId.Equals(ID));
            }
            return users[index];
        }

        public int FindIndex(User aUser)
        {
            int counter = 0;
            bool found = false;
            found = (aUser.PersonalId.Equals(users[counter].PersonalId));   //using a Boolean Expression to initialise found
            while (!(found) & counter < users.Count - 1)
            {
                counter += 1;
                found = (aUser.PersonalId.Equals(users[counter].PersonalId));
            }
            if (found)
            {
                return counter;
            }
            else
            {
                return -1;
            }

        }
        #endregion
    }
}
