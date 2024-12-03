using PhumlaKamnandiHotelSystem.Business;
using PhumlaKamnandiHotelSystem.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace PhumlaKamnandiHotelSystem.DataBase
{
    class UserDB : DB
    {
        private string guestTable = "Guests";  // Updated table name
        private string adminTable = "Admins";   // Updated table name
        private string sqlGuestQuery = "SELECT * FROM Guests";  // Updated SQL query
        private string sqlAdminQuery = "SELECT * FROM Admins";  // Updated SQL query
        private Collection<User> users;

        #region Property Method: Collection
        public Collection<User> AllUsers
        {
            get
            {
                return users;
            }
        }
        #endregion

        #region Constructor
        public UserDB() : base()
        {
            users = new Collection<User>();
            FillDataSet(sqlGuestQuery, guestTable);
            AddToCollection(guestTable);
            FillDataSet(sqlAdminQuery, adminTable);
            AddToCollection(adminTable);
        }
        #endregion

        #region Utility Methods
        public DataSet GetDataSet()
        {
            return dsMain;
        }

        private void AddToCollection(string table)
        {
            DataRow myRow = null;
            User aUser;
            Guest aGuest;
            Admin anAdmin;
            Role.RoleType roleValue = Role.RoleType.NoRole;

            switch (table)
            {
                case "Guests":
                    roleValue = Role.RoleType.Guest;
                    break;
                case "Admins":
                    roleValue = Role.RoleType.Admin;
                    break;
            }

            //READ from the table  
            foreach (DataRow myRowLoop in dsMain.Tables[table].Rows)
            {
                myRow = myRowLoop;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    // Instantiate a new User object
                    aUser = new User(roleValue);
                    aUser.Name = Convert.ToString(myRow["Name"]).TrimEnd();
                    aUser.Surname = Convert.ToString(myRow["Surname"]).TrimEnd();
                    aUser.Phone = Convert.ToString(myRow["PhoneNumber"]).TrimEnd();
                    aUser.Email = Convert.ToString(myRow["Email"]).TrimEnd();

                    switch (table)
                    {
                        case "Guests":
                            aUser.PersonalId = Convert.ToString(myRow["GuestID"]).TrimEnd();
                            aGuest = (Guest)aUser.role;
                            aGuest.BankCardNumber = Convert.ToString(myRow["BankCardNumber"]).TrimEnd(); // Updated attribute
                            aGuest.Address = Convert.ToString(myRow["Address"]).TrimEnd();
                            aGuest.PostalCode = Convert.ToString(myRow["PostalCode"]).TrimEnd(); // New attribute
                            break;
                        case "Admins":
                            aUser.PersonalId = Convert.ToString(myRow["AdminID"]).TrimEnd();
                            anAdmin = (Admin)aUser.role; // Adjusted from Clerk to Admin
                            anAdmin.Username = Convert.ToString(myRow["Username"]).TrimEnd();
                            anAdmin.Password = Convert.ToString(myRow["Password"]).TrimEnd();
                            break;
                    }

                    users.Add(aUser);
                }
            }
        }

        private void FillRow(DataRow aRow, User aUser, DB.DBOperation operation)
        {
            Debug.WriteLine("Filling Row with User Data");
            Guest aGuest;
            Admin anAdmin;

            aRow["Name"] = aUser.Name;
            aRow["Surname"] = aUser.Surname; // Added Surname
            aRow["PhoneNumber"] = aUser.Phone;
            aRow["Email"] = aUser.Email;

            //*** For each role add the specific data variables
            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    Debug.WriteLine("Assigning guest attribute values");
                    aGuest = (Guest)aUser.role;
                    aRow["Address"] = aGuest.Address;
                    Debug.WriteLine($"Address: {aGuest.Address}");
                    aRow["BankCardNumber"] = aGuest.BankCardNumber; // Updated attribute
                    Debug.WriteLine($"BankCardNumber: {aGuest.BankCardNumber}");
                    aRow["PostalCode"] = aGuest.PostalCode; // New attribute
                    Debug.WriteLine($"PostalCode: {aGuest.PostalCode}");

                    break;
                case Role.RoleType.Admin:
                    anAdmin = (Admin)aUser.role; // Adjusted from Clerk to Admin
                    aRow["Username"] = anAdmin.Username;
                    aRow["Password"] = anAdmin.Password;
                    break;
            }
        }

        private int FindRow(User aUser, string table)
        {
            int rowIndex = 0;
            DataRow myRow;
            int returnValue = -1;
            foreach (DataRow myRowLoop in dsMain.Tables[table].Rows)
            {
                myRow = myRowLoop;
                // Ignore rows marked as deleted in dataset
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    if ((aUser.role.getRoleValue == Role.RoleType.Guest && aUser.PersonalId.Equals(Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["GuestID"]))) ||
                        (aUser.role.getRoleValue == Role.RoleType.Admin && aUser.PersonalId.Equals(Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["AdminID"]))))
                    {
                        returnValue = rowIndex;
                    }
                }

                rowIndex ++;
            }
            return returnValue;
        }
        #endregion

        #region Database Operations CRUD
        public void DataSetChange(User aUser, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = guestTable;
            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    Debug.WriteLine("Role is Guest");
                    dataTable = guestTable;
                    break;
                case Role.RoleType.Admin:
                    dataTable = adminTable; // Changed from Clerk to Admin
                    break;
            }

            switch (operation)
            {
                case DBOperation.Add:
                    Debug.WriteLine("Operation is Add");
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aUser, DBOperation.Add);
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aUser, dataTable)];
                    FillRow(aRow, aUser, operation);
                    break;
                case DB.DBOperation.Delete:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aUser, dataTable)];
                    aRow.Delete();
                    break;

            }
        }
        #endregion

        #region Build Parameters, Create Commands & Update database
        private void Build_INSERT_Parameters(User aUser)
        {
            Debug.WriteLine("Invoke Build_INSERT_Parameters");
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@Name", SqlDbType.NVarChar, 50, "Name");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Surname", SqlDbType.NVarChar, 50, "Surname"); // Added Surname
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 15, "PhoneNumber");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 100, "Email");
            daMain.InsertCommand.Parameters.Add(param);

            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    param = new SqlParameter("@BankCardNumber", SqlDbType.NVarChar, 20, "BankCardNumber"); // Updated attribute
                    daMain.InsertCommand.Parameters.Add(param);

                    param = new SqlParameter("@Address", SqlDbType.NVarChar, 255, "Address"); // Updated to accommodate full address
                    daMain.InsertCommand.Parameters.Add(param);

                    param = new SqlParameter("@PostalCode", SqlDbType.NVarChar, 10, "PostalCode"); // Added PostalCode
                    daMain.InsertCommand.Parameters.Add(param);
                    break;
                case Role.RoleType.Admin:
                    param = new SqlParameter("@Username", SqlDbType.NVarChar, 50, "Username");
                    daMain.InsertCommand.Parameters.Add(param);

                    param = new SqlParameter("@Password", SqlDbType.NVarChar, 255, "Password");
                    daMain.InsertCommand.Parameters.Add(param);
                    break;
            }
        }

        private void Build_UPDATE_Parameters(User aUser)
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@Name", SqlDbType.NVarChar, 50, "Name");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Surname", SqlDbType.NVarChar, 50, "Surname"); // Added Surname
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 15, "PhoneNumber");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Email", SqlDbType.NVarChar, 100, "Email");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    param = new SqlParameter("@BankCardNumber", SqlDbType.NVarChar, 20, "BankCardNumber");
                    param.SourceVersion = DataRowVersion.Current;
                    daMain.UpdateCommand.Parameters.Add(param);

                    param = new SqlParameter("@Address", SqlDbType.NVarChar, 255, "Address"); // Updated to accommodate full address
                    param.SourceVersion = DataRowVersion.Current;
                    daMain.UpdateCommand.Parameters.Add(param);

                    param = new SqlParameter("@PostalCode", SqlDbType.NVarChar, 10, "PostalCode"); // Added PostalCode
                    param.SourceVersion = DataRowVersion.Current;
                    daMain.UpdateCommand.Parameters.Add(param);

                    param = new SqlParameter("@Original_GuestID", SqlDbType.Int, 5, "GuestID"); // Correct type for GuestID
                    param.SourceVersion = DataRowVersion.Original;
                    daMain.UpdateCommand.Parameters.Add(param);
                    break;
                case Role.RoleType.Admin:
                    param = new SqlParameter("@Username", SqlDbType.NVarChar, 50, "Username");
                    param.SourceVersion = DataRowVersion.Current;
                    daMain.UpdateCommand.Parameters.Add(param);

                    param = new SqlParameter("@Password", SqlDbType.NVarChar, 255, "Password");
                    param.SourceVersion = DataRowVersion.Current;
                    daMain.UpdateCommand.Parameters.Add(param);

                    param = new SqlParameter("@Original_AdminID", SqlDbType.Int, 5, "AdminID"); // Correct type for AdminID
                    param.SourceVersion = DataRowVersion.Original;
                    daMain.UpdateCommand.Parameters.Add(param);
                    break;
            }
        }

        private void Create_INSERT_Command(User aUser)
        {
            Debug.WriteLine("Inside Create_INSERT_Command");
            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    daMain.InsertCommand = new SqlCommand(
                        "INSERT INTO Guests (Name, Surname, PhoneNumber, Email, Address, PostalCode, BankCardNumber) VALUES (@Name, @Surname, @PhoneNumber, @Email, @Address, @PostalCode, @BankCardNumber)",
                        cnMain);
                    //daMain.InsertCommand.ExecuteNonQuery();
                    break;
                case Role.RoleType.Admin:
                    daMain.InsertCommand = new SqlCommand(
                        "INSERT INTO Admins (Name, Surname, Username, Password, PhoneNumber, Email) VALUES (@Name, @Surname, @Username, @Password, @PhoneNumber, @Email)",
                        cnMain);
                    break;
            }

            // Build parameters for the insert command
            Build_INSERT_Parameters(aUser);

            // Log the command text and parameters
            Debug.WriteLine($"Insert Command: {daMain.InsertCommand.CommandText}");
            foreach (SqlParameter param in daMain.InsertCommand.Parameters)
            {
                Debug.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}");
            }
        }

        private void Create_UPDATE_Command(User aUser)
        {
            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    daMain.UpdateCommand = new SqlCommand("UPDATE Guests SET Name=@Name, Surname=@Surname, PhoneNumber=@PhoneNumber, Email=@Email, Address=@Address, PostalCode=@PostalCode, BankCardNumber=@BankCardNumber WHERE GuestID=@Original_GuestID", cnMain);
                    break;
                case Role.RoleType.Admin:
                    daMain.UpdateCommand = new SqlCommand("UPDATE Admins SET Name=@Name, Surname=@Surname, Username=@Username, Password=@Password, PhoneNumber=@PhoneNumber, Email=@Email WHERE AdminID=@Original_AdminID", cnMain);
                    break;
            }

            Build_UPDATE_Parameters(aUser);
        }

        private void Create_DELETE_Command(User aUser)
        {
            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    daMain.DeleteCommand = new SqlCommand("DELETE FROM Guests WHERE GuestID=@GuestID", cnMain);
                    break;
                case Role.RoleType.Admin:
                    daMain.DeleteCommand = new SqlCommand("DELETE FROM Admins WHERE AdminID=@AdminID", cnMain);
                    break;
            }

            Build_DELETE_Parameters(aUser);
        }

        private void Build_DELETE_Parameters(User aUser)
        {
            SqlParameter param = default(SqlParameter);

            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    param = new SqlParameter("@GuestID", SqlDbType.Int);
                    param.SourceColumn = "GuestID";
                    param.SourceVersion = DataRowVersion.Current;
                    break;
                case Role.RoleType.Admin:
                    param = new SqlParameter("@AdminID", SqlDbType.Int);
                    param.SourceColumn = "AdminID";
                    param.SourceVersion = DataRowVersion.Current;
                    break;
            }

            daMain.DeleteCommand.Parameters.Add(param);
        }

        public bool UpdateDataSource(User aUser, DB.DBOperation operation)
        {
            Debug.WriteLine("Inside UpdateDataSource");
            String sqlLocal = "";
            String table = "";

            switch (operation)
            {
                case DBOperation.Add:
                    Debug.WriteLine("Invoke Create_INSERT_Command");
                    Create_INSERT_Command(aUser);
                    break;
                case DBOperation.Edit:
                    Create_UPDATE_Command(aUser);
                    break;
                case DBOperation.Delete:
                    Create_DELETE_Command(aUser);
                    break;
            }

            switch (aUser.role.getRoleValue)
            {
                case Role.RoleType.Guest:
                    sqlLocal = sqlGuestQuery;
                    table = guestTable;
                    break;
                case Role.RoleType.Admin:
                    sqlLocal = sqlAdminQuery;
                    table = adminTable;
                    break;
            }

            return UpdateDataSource(sqlLocal, table);
        }
        #endregion
    }
}
    