//In the name of Allah

using System.Collections.Generic;

namespace ThreeS.Report.v2.Controllers
{
    public class StringService
    {
        public static string OutletIdsToString(List<int> OutletIds) 
        {
            string outletIdsAsString = "0";
            foreach (int outletId in OutletIds)
            {
                outletIdsAsString += "," + outletId.ToString();
            }//foreach
            return outletIdsAsString;
        }//func

        public static string DepartmentIdsToString(List<int> departmentIds)
        {
            string departmentIdsAsString = "0";
            foreach (int departmentId in departmentIds)
            {
                departmentIdsAsString += "," + departmentId.ToString();
            }//foreach
            return departmentIdsAsString;
        }//func

        public static string MenuItemIdsToString(List<int> menuItemIds)
        {
            string menuItemIdsAsString = "0";
            foreach (int menuItemId in menuItemIds)
            {
                menuItemIdsAsString += "," + menuItemId.ToString();
            }//foreach
            return menuItemIdsAsString;
        }//func 

        public static string MenuItemCategorisToString(List<string> menuItemCategoris)
        {
            int count = 0;
            string menuItemCategoriesAsString = "";
            foreach (string menuItemCategory in menuItemCategoris)
            {
                if (count == 0)
                {
                    menuItemCategoriesAsString = "'" + menuItemCategory + "'";
                }
                else
                {
                    menuItemCategoriesAsString += "," + "'" + menuItemCategory + "'";
                }//else
                count++;

            }//foreach
            return menuItemCategoriesAsString;
        }//func 

        public static string BrandsToString(List<string> brands)
        {
            int countBrand = 0;
            string brandsAsString = "";
            foreach (string brand in brands)
            {
                if (countBrand == 0)
                {
                    brandsAsString = "'" + brand + "'";
                }
                else
                {
                    brandsAsString += "," + "'" + brand + "'";
                }//else
                countBrand++;

            }//foreach
            return brandsAsString;
        }//func 

    }//class
}//namespace