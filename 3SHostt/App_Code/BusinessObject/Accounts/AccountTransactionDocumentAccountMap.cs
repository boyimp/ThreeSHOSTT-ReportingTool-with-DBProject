using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Accounts
{
    public class AccountTransactionDocumentAccountMap : ValueClass
    {
        public int AccountTransactionDocumentTypeId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int MappedAccountId { get; set; }
        public string MappedAccountName { get; set; }
    }
}
