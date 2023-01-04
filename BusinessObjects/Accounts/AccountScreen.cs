using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Infrastructure;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Accounts
{
    public class AccountScreen : EntityClass
    {
        public AccountScreen()
        {
            _accountScreenValues = new List<AccountScreenValue>();
        }

        public int Filter { get; set; }

        private IList<AccountScreenValue> _accountScreenValues;
        public virtual IList<AccountScreenValue> AccountScreenValues
        {
            get { return _accountScreenValues; }
            set { _accountScreenValues = value; }
        }

        private IList<AccountScreenMap> _AccountScreenMaps;
        public virtual IList<AccountScreenMap> AccountScreenMaps
        {
            get { return _AccountScreenMaps; }
            set { _AccountScreenMaps = value; }
        }

    }
}
