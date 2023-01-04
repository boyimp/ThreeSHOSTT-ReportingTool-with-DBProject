using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Data;
using ThreeS.Domain.Models.Entities;

namespace BusinessObjects
{
    public class DBUtility
    {
        public static string GetConnectionString()
        {
            //return ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;
            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
            var stringHashed = xml.SelectSingleNode("/SettingsObject/ConnectionString").InnerText;
            return Encryption.Decrypt(stringHashed) + ";Connection Timeout=12000;";
        }
        public static string GetFilePathFromAppConfig(string File)
        {
            string path = string.Empty;
            string appPath = HttpContext.Current.Request.ApplicationPath.ToString();
            path = appPath == "/" ? appPath + "App_Config/"+ File : "/App_Config" + appPath + "_" + File;
            return path;
        }
        public static double GetVATIncludingDenominator()
        {
            //return ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString;
            double VATIncludingDenominator = 0;
            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
            VATIncludingDenominator = Convert.ToDouble(xml.SelectSingleNode("/SettingsObject/VATIncludingDenominator").InnerText);
            return VATIncludingDenominator;
        }
        public static double GetVATIncludingDenominator(int SyncOutletId)
        {
            string sVATIncludingDenominatorOutlet = string.Empty;
            string sVATIncludingDenominator = string.Empty;
            if (SyncOutletId > 0)
            {
                DataTable dt = SyncManager.SyncManager.GetSyncOutletStatus(SyncOutletId);
                sVATIncludingDenominatorOutlet = Entity.GetCustomData(dt.Rows[0]["OutletInfo"].ToString().Trim(), "VATIncludingDenominator");
            }
            if (SyncOutletId == 0 || string.IsNullOrEmpty(sVATIncludingDenominatorOutlet))
            {
                XmlDocument xml = new XmlDocument();
                string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
                xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
                sVATIncludingDenominator = xml.SelectSingleNode("/SettingsObject/VATIncludingDenominator").InnerText;
            }
            else
            {
                sVATIncludingDenominator = sVATIncludingDenominatorOutlet;
            }
            return Convert.ToDouble(sVATIncludingDenominator);
        }
        public static string GetTimethreshold()
        {
            return ConfigurationManager.AppSettings["Timethreshold"];
        }

    }
}
