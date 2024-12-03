using PhumlaKamnandiHotelSystem.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace PhumlaKamnandiHotelSystem.DataBase
{
    public class DB
    {
        #region Variable Declaration
        private string strConn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\School_INF2011\PhumlaKamnandiHotelSystem\PhumlaKamnandiHotelSystem\HotelDB.mdf;Integrated Security=True;";
        protected SqlConnection cnMain;
        protected DataSet dsMain;
        protected SqlDataAdapter daMain;

        public enum DBOperation
        {
            Add = 0,
            Edit = 1,
            Delete = 2
        }
        #endregion

        #region Constructor
        public DB()
        {
            try
            {
                cnMain = new SqlConnection(strConn);
                dsMain = new DataSet();
            }
            catch (SystemException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error");
                return;
            }
        }
        #endregion

        #region Update the DateSet
        public void FillDataSet(string aSQLstring, string aTable)
        {
            try
            {
                daMain = new SqlDataAdapter(aSQLstring, strConn);
                cnMain.Open();
                daMain.Fill(dsMain, aTable);
                cnMain.Close();
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);
            }
        }
        #endregion

        #region Update the data source 
        protected bool UpdateDataSource(string sqlLocal, string table)
        {
            bool success;
            try
            {
                cnMain.Open();

                //rowFinder(dsMain);

                daMain.Update(dsMain, table);

                //rowFinder(dsMain);

                cnMain.Close();

                FillDataSet(sqlLocal, table);
                success = true;
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);
                success = false;

            }
            finally
            {
            }
            return success;
        }
        #endregion

        /*public void rowFinder(DataSet dsMain)
        {
            // Iterate through each DataRow in the 'HeadWaiter' DataTable
            foreach (DataRow row in dsMain.Tables["Bookings"].Rows)
            {
                // Check if the row is marked as Added (for insertion)
                if (row.RowState == DataRowState.Added)
                {
                    // Create a string to hold the current values of the added row
                    string addedRowValues = "Added Row Values: ";

                    // Iterate through each column in the row
                    foreach (DataColumn column in dsMain.Tables["Bookings"].Columns)
                    {
                        // Access the current value of the added row for each column
                        string currentValue = row[column.ColumnName]?.ToString() ?? "NULL";
                        addedRowValues += $"{column.ColumnName}: {currentValue}, ";
                    }

                    // Print the current values of the added row
                    Debug.WriteLine(addedRowValues.TrimEnd(',', ' ')); // Trim the last comma and space
                }
            }
        }*/
    }
}
