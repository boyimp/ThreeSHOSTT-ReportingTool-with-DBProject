using ThreeS.Domain.Models.Accounts;

namespace ThreeS.Domain.Models
{
    public class AccountData
    {
        public AccountData(int accountId)
        {
            AccountId = accountId;
        }
        public AccountData(int accountId, double filter)
        {
            Filter = (int)filter;
            AccountId = accountId;
        }        
        public AccountData(int accountTypeId, int accountId)
        {
            AccountTypeId = accountTypeId;
            AccountId = accountId;
        }

        public AccountData(Account account)
        {
            AccountTypeId = account.AccountTypeId;
            AccountId = account.Id;
        }

        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
        public int Filter { get; set; }
    }
}