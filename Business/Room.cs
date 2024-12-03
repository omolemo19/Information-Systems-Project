using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiHotelSystem.Business
{
    class Room
    {
        #region Data Members
        private int room_Number;
        private bool roomStatus = false;

        #endregion

        #region Property Methods
        public int RoommNu
        {
            set { room_Number = value; }
            get { return room_Number; }
        }

        public bool Status
        {
            set { roomStatus = value; }
            get { return roomStatus; }
        }

        #endregion

        #region Constructor

        public Room(int room_number)
        {
            this.room_Number = room_number;
        }

        #endregion


        #region Utility Methods
        public void ReserveRoom()
        {
            roomStatus = true;
        }

        public void checkout()
        {
            roomStatus = false;
        }

        #endregion



    }
}