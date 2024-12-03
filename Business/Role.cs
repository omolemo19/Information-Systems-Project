using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiHotelSystem.Business
{
    public class Role
    {

        #region Dat Members
        public enum RoleType
        {
            NoRole = 0,
            Guest = 1,
            Admin = 2
        }

        protected RoleType roleVal;


        #endregion

        #region Property Methods
        public RoleType getRoleValue
        {
            get { return roleVal; }
            set { roleVal = value; }
        }

        #endregion

        #region Constructor
        public Role()
        {
            roleVal = Role.RoleType.NoRole;
        }
        #endregion
    }
}
