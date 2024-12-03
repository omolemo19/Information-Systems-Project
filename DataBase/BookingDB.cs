using PhumlaKamnandiHotelSystem.Business;
using PhumlaKamnandiHotelSystem.DataBase;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace PhumlaKamnandiHotelSystem.Data
{
    public class BookingDB : DB
    {
        #region Data members        
        private string table = "Bookings";  // Update table name to match your schema
        private string sqlLocal = "SELECT * FROM Bookings";  // Update SQL query to match your schema
        private Collection<Booking> bookings;
        #endregion

        #region Property Method: Collection
        public Collection<Booking> AllBookings
        {
            get
            {
                return bookings;
            }
        }
        #endregion

        #region Constructor
        public BookingDB() : base()
        {
            bookings = new Collection<Booking>();
            FillDataSet(sqlLocal, table);
            AddToCollection(table);
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
            Booking aBooking;

            // READ from the table and add to collection f Bookings
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    aBooking = new Booking(Convert.ToInt32(myRow["GuestID"]), Convert.ToInt32(myRow["RoomNumber"])
                        , Convert.ToString(myRow["CheckInDate"]).TrimEnd(), Convert.ToString(myRow["CheckInDate"]).TrimEnd()
                        , Convert.ToDecimal(myRow["TotalCost"]));

                    aBooking.BookingID = Convert.ToInt32(myRow["BookingID"]);
                    bookings.Add(aBooking);
                }
            }
        }

        private void FillRow(DataRow aRow, Booking aBooking, DB.DBOperation operation)
        {
            if (operation == DBOperation.Add)
            {
                aRow["BookingID"] = aBooking.BookingID;
                aRow["GuestID"] = aBooking.BookingID;
                aRow["RoomNumber"] = aBooking.RoomId;
                aRow["CheckInDate"] = DateTime.Parse(aBooking.CheckIn);
                aRow["CheckOutDate"] = DateTime.Parse(aBooking.CheckOut);
                aRow["TotalCost"] = aBooking.TotalFee;

            }
        }


        private int FindRow(Booking aBooking, string table)
        {
            int rowIndex = 0;
            DataRow myRow = null;
            int returnValue = -1;

            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (myRow.RowState != DataRowState.Deleted)
                {
                    if (aBooking.BookingID == Convert.ToInt32(dsMain.Tables[table].Rows[rowIndex]["BookingID"]))
                    {
                        returnValue = rowIndex;
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region Database Operations CRUD

        public void DataSetChange(Booking aBooking, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table;
            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aBooking, operation);
                    dsMain.Tables[dataTable].Rows.Add(aRow); //Add to the dataset
                    break;
                case DB.DBOperation.Edit:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aBooking, dataTable)];
                    FillRow(aRow, aBooking, operation);
                    break;
                case DB.DBOperation.Delete:
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aBooking, dataTable)];
                    aRow.Delete();
                    break;

                // Add other operations here.
            }
        }
        #endregion

        #region Build Parameters, Create Commands & Update database

        private void Build_INSERT_Parameters(Booking aBooking)
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@BookingID", SqlDbType.Int) { Value = aBooking.BookingID };
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@GuestID", SqlDbType.Int) { Value = aBooking.GuestId};
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@RoomNumber", SqlDbType.Int) { Value = aBooking.RoomId };
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CheckInDate", SqlDbType.DateTime) { Value = DateTime.Parse(aBooking.CheckIn) };
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CheckOutDate", SqlDbType.DateTime) { Value = DateTime.Parse(aBooking.CheckOut) };
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("TotalCost", SqlDbType.Decimal) { Value = aBooking.TotalFee };
            daMain.InsertCommand.Parameters.Add(param);
        }

        private void Build_UPDATE_Parameters(Booking aBooking)
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@OginalBookingID", SqlDbType.Int) { SourceColumn = "BookingID", SourceVersion = DataRowVersion.Original };
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@GuestID", SqlDbType.Int) { Value = aBooking.GuestId };
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@RoomNumber", SqlDbType.Int) { Value = aBooking.RoomId };
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CheckInDate", SqlDbType.DateTime) { Value = DateTime.Parse(aBooking.CheckIn) };
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CheckOutDate", SqlDbType.DateTime) { Value = DateTime.Parse(aBooking.CheckOut) };
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("TotalCost", SqlDbType.Decimal) { Value = aBooking.TotalFee };
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);
        }

        private void Build_DELETE_Parameters(Booking aBooking)
        {
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@BookingID", SqlDbType.Int) { Value = aBooking.BookingID };
            daMain.DeleteCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Booking aBooking)
        {
            daMain.InsertCommand = new SqlCommand("INSERT into Bookings (BookingID, GuestID, RoomNumber, CheckInDate, CheckOutDate, TotalCost) " +
                                                  "VALUES (@BookingID, @GuestID, @RoomNumber, @CheckInDate, @CheckOutDate, @TotalCost) ", cnMain);
            Build_INSERT_Parameters(aBooking);
        }

        private void Create_UPDATE_Command(Booking aBooking)
        {
            daMain.UpdateCommand = new SqlCommand("UPDATE  Bookings SET GuestID =@GuestID, RoomNumber = @RoomNumber, CheckInDate =@CheckInDate, CheckOutDate =@CheckOutDate, TotalCost =@TotalCost " + "WHERE OriginalBookingID = @BookingID", cnMain);
            Build_UPDATE_Parameters(aBooking);
        }

        private void Create_Delete_Command(Booking aBooking)
        {
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Bookings WHERE BookingID = @BookingID",cnMain);
            Build_DELETE_Parameters(aBooking);
        }

        public bool UpdateDataSource(Booking aBooking, DB.DBOperation operation)
        {
            bool success = true;
            switch (operation)
            {
                case DBOperation.Add:
                    Create_INSERT_Command(aBooking);
                    break;
                case DBOperation.Edit:
                    Create_UPDATE_Command(aBooking);
                    break;
                case DBOperation.Delete:
                    Create_Delete_Command(aBooking);
                    break;
                default:
                    break;
            }
            success = UpdateDataSource(sqlLocal, table);
            return success;
        }

        #endregion
    }
}
