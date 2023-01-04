using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Data;
using ThreeS.Domain.Models.Entities;
using System.IO;
using ThreeS.Core.Cipher;

namespace BusinessObjects
{
    public class DBUtility
    {
        public static HttpContext Current { get; set; }
        public static string GetDataFetchFrom()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                string hosttSettingsPath = GetFilePathFromAppConfig("HOSTTSettings.txt");
                xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
                return xml.SelectSingleNode("/SettingsObject/FetchDataFrom").InnerText;
            }
            catch (Exception)
            {
                return "FromLegacy";
            }
        }
        public static string GetConnectionString()
        {            
            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
            var stringHashed = xml.SelectSingleNode("/SettingsObject/ConnectionString").InnerText;
            return Encryption.Decrypt(stringHashed) + ";Connection Timeout=12000;";
        }
        public static string GetFilePathFromAppConfig(string File)
        {
            try
            {
                if (HttpContext.Current == null)
                    HttpContext.Current = Current;

                string domainName = ConfigurationManager.AppSettings["DomainName"];
                string subdomain = HttpContext.Current.Request.Url.Host.Replace("www.", "").Replace(domainName, "").Replace(".", "");
                string appPath = HttpContext.Current.Request.ApplicationPath.ToString();
                string path = appPath == "/" && subdomain == string.Empty ? appPath + "App_Config/" + File : (subdomain == string.Empty ? "/App_Config" + appPath + "_" + File : "/SubDomain/" + subdomain + "_" + File);                
                return path;
            }
            catch (Exception)
            {
                throw;
            }
        }        
        public static double GetVATIncludingDenominator()
        {
            if (HttpContext.Current == null)
                HttpContext.Current = Current;
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
            if (SyncOutletId > 0)
            {
                DataTable dt = SyncManager.SyncManager.GetSyncOutletStatus(SyncOutletId);
                sVATIncludingDenominatorOutlet = Entity.GetCustomData(dt.Rows[0]["OutletInfo"].ToString().Trim(), "VATIncludingDenominator");
            }
            string sVATIncludingDenominator;
            if (SyncOutletId == 0 || string.IsNullOrEmpty(sVATIncludingDenominatorOutlet))
            {
                XmlDocument xml = new XmlDocument();
                string hosttSettingsPath = GetFilePathFromAppConfig("HOSTTSettings.txt");
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
