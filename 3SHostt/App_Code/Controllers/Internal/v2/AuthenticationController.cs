//In the name of Allah

using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using ThreeS.Report.v2.Utils;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : ApiController
    {
        [HttpGet]
        [Route("GetIsSessionActive")]
        public IHttpActionResult GetIsSessionActive()
        {
            if (HttpContext.Current.Session[GlobalData.CURRENT_USER] != null)
            {
                string currentUser = HttpContext.Current.Session[GlobalData.CURRENT_USER].ToString();
                Debug.WriteLine(currentUser);
                return Ok(true);
            }//if
            return Ok(false);
        }//func



        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody]User tryingToLoginUser)
        {
            Debug.WriteLine("username : " + tryingToLoginUser.Username);
            Debug.WriteLine("password : " + tryingToLoginUser.Password);

            

            if (UserManager.Exist(tryingToLoginUser.Username, tryingToLoginUser.Password))
            {
                HttpContext.Current.Session[GlobalData.CURRENT_USER] = tryingToLoginUser.Username;
                Debug.WriteLine("session : " + HttpContext.Current.Session[GlobalData.CURRENT_USER]);
                return Ok(tryingToLoginUser);
            }//if
            else
            {
                return NotFound();
            }//else

        }//func

        [HttpGet]
        [Route("LogOut")]
        public IHttpActionResult LogOut()
        {
            var Session = HttpContext.Current.Session;
            Session.Abandon();
            Session.Clear();
            if (HttpContext.Current.Session[GlobalData.CURRENT_USER] == null)
            {
                return Ok(true);
            }
            return Ok(false);
                

        }//func

    }//class

}//namespace