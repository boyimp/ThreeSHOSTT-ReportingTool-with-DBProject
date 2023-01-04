//In the name of Allah


using BusinessObjects.MenusManager;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Http;
using ThreeS.Domain.Models.Menus;
using ThreeS.Report.v2.Models;

namespace ThreeS.Report.v2.Controllers
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiController
    {
        [HttpGet]
        [Route("GetMenu")]
        public IHttpActionResult GetMenu()
        {
            List<MenuItem> menuItems = MenuItemManager.GetMenuItems();
            return Ok(menuItems);
        }//func

        [HttpGet]
        [Route("GetMenuItemNamesAndIds")]
        public async Task<IHttpActionResult> GetMenuItemNamesAndIds() 
        {
            var result = await MenuItemManager.GetMenuItemNamesAndIds();
            
            return Ok(result);
        }//func

        [HttpGet]
        [Route("GetMenuItemCategoriesNamesAndIds")]
        public async Task<IHttpActionResult> GetMenuItemCategoriesNamesAndIds()
        {
            var result = await MenuItemManager.GetMenuItemCategoriesNamesAndIds();

            return Ok(result);
        }//func


        [HttpGet]
        [Route("GetGroupCode")]
        public IHttpActionResult GetGroupCode()
        {
            DataSet menuItems = MenuItemManager.GetGroupItem();
            return Ok(menuItems.Tables[0]);
        }//func

        [HttpPost]
        [Route("GetMenuItemsByMenuItemIdAndGroupCode")]
        public IHttpActionResult GetMenuItemsByMenuItemIdAndGroupCode([FromBody]ParameterMenuItemIdGroupCode parameter)
        {
            List<MenuItem> menuItems = MenuItemManager.GetMenuItems(parameter.MenuItemId, parameter.GroupCode);
            return Ok(menuItems);
        }//func

        [HttpPost]
        [Route("GetMenuItemsByMenuItemIdAndGroupCodeFaster")] 
        public IHttpActionResult GetMenuItemsByMenuItemIdAndGroupCodeFaster([FromBody] ParameterMenuItemIdGroupCode parameter)
        {
            List<MenuItem> menuItems = MenuItemManager.GetMenuItemsFaster(parameter.MenuItemId, parameter.GroupCode);
            return Ok(menuItems);
        }//func

        [HttpPost]
        [Route("GetRecipesByMenuItemIdAndGroupCodeFaster")]
        public IHttpActionResult GetRecipesByMenuItemIdAndGroupCodeFaster([FromBody] ParameterMenuItemIdGroupCode parameter)
        {
            DataSet recipes = MenuItemManager.GetRecipes(parameter.MenuItemId, parameter.GroupCode);
            return Ok(recipes.Tables[0]);  
        }//func

        //Not working
        [HttpGet]
        [Route("GetMenuItemListByDepartment/{id:int}")]
        public IHttpActionResult GetMenuItemListByDepartment(int id)
        {
            int departmentId = id;
            DataSet menuItems = MenuItemManager.GetMenuItemListByDepertmentId(departmentId);
            return Ok(menuItems.Tables[0]);
        }//func

        //Return GroupCode as GetGroupCode
        [HttpGet]
        [Route("GetMenuItemTag")]
        public IHttpActionResult GetMenuItemTag()
        {
            DataSet menuItems = MenuItemManager.GetMenuItemTag();
            return Ok(menuItems.Tables[0]);
        }//func

        [HttpPost]
        [Route("GetCalculationsOutLetWise")]
        public IHttpActionResult GetCalculationsOutLetWise(ParameterFromDateToDate parameter)
        {
            DataSet calcuation = MenuItemManager.GetCalculationsOutLetWise(parameter.fromDate,parameter.toDate);
            return Ok(calcuation.Tables[0]);
        }//func
         
        [HttpPost]
        [Route("GetTotalOutletWise")]
        public IHttpActionResult GetTotalOutletWise(ParameterFromDateToDate parameter)
        {
            DataSet calcuation = MenuItemManager.GetTotalOutletWise(parameter.fromDate, parameter.toDate);
            return Ok(calcuation.Tables[0]);
        }//func



    }//class
}//namespace



