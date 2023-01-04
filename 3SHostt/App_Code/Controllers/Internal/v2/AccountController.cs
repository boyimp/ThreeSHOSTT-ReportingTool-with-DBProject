//In the name of Allah
using BusinessObjects.AccountsManager;
using System.Data;
using System.Web.Http;
using ThreeS.Report.v2.Models;
using ThreeS.Report.v2.Utils;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("GetAccountGroups")]
        public IHttpActionResult GetAccountGroups()
        {
            DataSet groupItems = AccountManager.GetGroupItem();
            return Ok(groupItems.Tables[0]);
        }//func

        [HttpGet]
        [Route("GetAccountType")]
        public IHttpActionResult GetAccountType()
        {
            DataTable accountTypes = AccountManager.GetAccountType();
            return Ok(accountTypes);
        }//func

        [HttpGet]
        [Route("GetAccountById/{id:int}")]
        public IHttpActionResult GetAccountById(int id)
        {
            DataTable account = AccountManager.GetAccountType(id);
            return Ok(account);
        }//func

        [HttpGet]
        [Route("GetAccountsByName/{name:string}")]
        public IHttpActionResult GetAccountsByName(string name)
        {
            string accountName = name;
            DataSet account = AccountManager.GetAccounts(accountName);
            return Ok(account.Tables[0]);
        }//func

        //untested
        [HttpPost]
        [Route("GetAccoutTypeWiseBalance")]
        public IHttpActionResult GetAccoutTypeWiseBalance(
            [FromBody] ParameterAccountTypeNameFromDateToDateIsOpeningBalance parameter)
        {
            //{
            //    "accountTypeName": null,
            //    "fromDate": "2022-07-27 02:37:33.447",
            //    "toDate": "2022-08-02 18:19:57.133",
            //    "isOpeningBalance": false
            //}
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);

            DataSet account = AccountManager.GetAccoutTypeWiseBalance(
                parameter.accountTypeName,
                workPeriodDateTimes.from.ToString(GlobalData.DATE_TIEM_FORMATE),
                workPeriodDateTimes.to.ToString(GlobalData.DATE_TIEM_FORMATE),
                parameter.isOpeningBalance
                );

            //[
            //    {
            //        "row_number": 1,
            //        "TranDate": "27 Jul 2022",
            //        "AccountsTypeID": 2,
            //        "AccountTypeName": "Sales Accounts",
            //        "Debit": 0,
            //        "Credit": 50,
            //        "Balance": -50
            //    }
            //]

            return Ok(account.Tables[0]);
        }//func

        //untested
        [HttpPost]
        [Route("GetAccoutWiseBalanceByAccountTypeNameAndAccountName")]
        public IHttpActionResult GetAccoutWiseBalanceByAccountTypeNameAndAccountName(
            [FromBody] ParameterAccountTypeNameAccountNameFromDateToDateIsOpeningBalance parameter) 
        {
            //{
            //    "accountTypeName": "Sales Accounts",
            //    "accountName": "Sales",
            //    "fromDate": "2022-07-27 02:37:33.447",
            //    "toDate": "2022-08-02 18:19:57.133",
            //    "isOpeningBalance": false
            //}

            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);

            DataSet accountBalances = AccountManager.GetAccoutWiseBalance(
                parameter.accountTypeName, 
                parameter.accountName,
                workPeriodDateTimes.from.ToString(GlobalData.DATE_TIEM_FORMATE),
                workPeriodDateTimes.to.ToString(GlobalData.DATE_TIEM_FORMATE),
                parameter.isOpeningBalance
                );

            //[
            //    {
            //        "row_number": 1,
            //        "TrandDate": "27 Jul 2022",
            //        "AccountsTypeID": 2,
            //        "AccountTypeName": "Sales Accounts",
            //        "AccountsID": 1,
            //        "AccountName": "Sales",
            //        "Debit": 0,
            //        "Credit": 50,
            //         "Balance": -50
            //    }
            //]
            return Ok(accountBalances.Tables[0]); 
        }//func

        //untested
        [HttpPost]
        [Route("GetAccoutWiseBalanceByAccountTypeNameAndAccountNameAndOutletId")]
        public IHttpActionResult GetAccoutWiseBalanceByAccountTypeNameAndAccountNameAndOutletId(
            [FromBody] ParameterAccountTypeNameAccountNameFromDateToDateOutletId parameter) 
        {
            //{
            //    "accountTypeName": "Sales Accounts",
            //    "accountName": "Sales",
            //    "fromDate": "2022-07-27 02:37:33.447",
            //    "toDate": "2022-08-02 18:19:57.133",
            //    "outletId": 0
            //}
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(parameter.fromDate, parameter.toDate);

            DataSet account = AccountManager.GetAccoutWiseBalance(
                parameter.accountTypeName,
                parameter.accountName,
                workPeriodDateTimes.from.ToString(GlobalData.DATE_TIEM_FORMATE),
                workPeriodDateTimes.to.ToString(GlobalData.DATE_TIEM_FORMATE),
                parameter.outletId
                );

            //[
            //    {
            //        "AccountType": "Sales Accounts",
            //        "AccountId": 1,
            //        "AccountName": "Sales",
            //        "Debit": 0,
            //        "Credit": 5590,
            //        "Balance": -5590
            //    }
            //]
            return Ok(account.Tables[0]);
        }//func

        //untested
        [HttpPost]
        [Route("GetAccountDrillThrough")]
        public IHttpActionResult GetAccountDrillThrough([FromBody] ParameterAccountIdFromDateToDateOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate, 
                parameter.toDate
                );

            DataSet account = AccountManager.GetAccountDrillThrough(
                parameter.accountId,
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to, 
                parameter.outletId
                );

            return Ok(account.Tables[0]);
        }//func

        //not finish yet
        [HttpPost]
        [Route("GetCurrentBalanceOfAChead")]
        public IHttpActionResult GetCurrentBalanceOfAChead([FromBody] ParameterFromDateToDateOutletId parameter)
        {
            WorkPeriodDateTimes workPeriodDateTimes = DateTimeService.GenerateWorkPeriodWiseDateTime(
                parameter.fromDate,
                parameter.toDate
                );

            DataSet account = AccountManager.GetCurrentBalanceOfAChead(
                workPeriodDateTimes.from, 
                workPeriodDateTimes.to, 
                parameter.outletId
                );

            return Ok(account.Tables[0]);
        }//func
        

    }//calss
}//namespace