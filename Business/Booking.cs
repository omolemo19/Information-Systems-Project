using System;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class Booking
    {
        #region Data Members
        private static int numberOfBookings = 0;
        private int bookingId;
        private string checkIn;
        private string checkOut;
        private int guestId;
        private int roomId;
        private decimal totalFee;
        #endregion

        #region Constructor
        public Booking(int guestId, int roomId, string checkIn, string checkOut, decimal totalFee)
        {
            this.guestId = guestId;
            this.roomId = roomId;
            this.checkIn = checkIn;
            this.checkOut = checkOut;
            this.totalFee = totalFee;
            this.BookingID = numberOfBookings;
            numberOfBookings++;
        }
        #endregion

        #region Property Methods
        public int BookingID
        {
            get { return bookingId; }
            set { bookingId = value; }
        }

        public string CheckIn
        {
            get { return checkIn; }
            set { checkIn = value; }
        }

        public string CheckOut
        {
            get { return checkOut; }
            set { checkOut = value; }
        }

        public int GuestId
        {
            get { return guestId; }
            set { guestId = value; }
        }

        public int RoomId
        {
            get { return roomId; }
            set { roomId = value; }
        }

        public decimal TotalFee
        {
            get { return totalFee; }
            set { totalFee = value; }
        }
        #endregion

        #region Generate BookingID
        public int GetBookingID()
        {
            return this.bookingId;
        }
        #endregion
    }
}
