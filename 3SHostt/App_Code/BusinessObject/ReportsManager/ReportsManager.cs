using System;
using System.Collections.Generic;
using System.Xml;
using System.Web;
using System.Data;
using Telerik.Web.UI;
using ThreeS.Domain.Models.Entities;
using Microsoft.Reporting.WebForms;
namespace BusinessObjects.ReportManager
{
    public class ReportManager
    {
        
        
        public static List<ReportParameter> GetReportParams()
        {
            List<ReportParameter> paras = new List<ReportParameter>();
            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);

            string CompanyName = xml.SelectSingleNode("/SettingsObject/CompanyName").InnerText.Replace("#rn", Environment.NewLine); 
            string CompanyAddress = xml.SelectSingleNode("/SettingsObject/CompanyAddress").InnerText.Replace("#rn", Environment.NewLine);;
            string VatReg = xml.SelectSingleNode("/SettingsObject/VatRegistrationNumber").InnerText.Replace("#rn", Environment.NewLine);

            string companyLogoPath = DBUtility.GetFilePathFromAppConfig("CompanyLogo.png");
            string CompanyLogo = new Uri(HttpContext.Current.Server.MapPath("~"+companyLogoPath)).AbsoluteUri;
            string ThreeSLogo = new Uri(HttpContext.Current.Server.MapPath("~/Images/logo.jpg")).AbsoluteUri;
            ReportParameter rptCompanyLogo = new ReportParameter("rptCompanyLogo", CompanyLogo);
            paras.Add(rptCompanyLogo);
            ReportParameter rptCompanyName = new ReportParameter("rptCompanyName", CompanyName);
            paras.Add(rptCompanyName);
            ReportParameter rptCompanyAddress = new ReportParameter("rptCompanyAddress", CompanyAddress);
            paras.Add(rptCompanyAddress);
            ReportParameter rptVatReg = new ReportParameter("rptVatReg", VatReg);
            paras.Add(rptVatReg);
            ReportParameter rpt3SLogo = new ReportParameter("rpt3SLogo", ThreeSLogo);
            paras.Add(rpt3SLogo);
            
            return paras;
        }
        public static List<ReportParameter> GetReportParams(int SyncOutletId)
        {
            List<ReportParameter> paras = new List<ReportParameter>();
            string CompanyName = string.Empty;
            string CompanyAddress = string.Empty;
            string VatReg = string.Empty;
            string CompanyNameOutlet = string.Empty;
            string CompanyAddressOutlet = string.Empty;
            string VatRegOutlet = string.Empty;
            if (SyncOutletId > 0)
            {
                DataTable dt = SyncManager.SyncManager.GetSyncOutletStatus(SyncOutletId);

                CompanyNameOutlet = Entity.GetCustomData(dt.Rows[0]["OutletInfo"].ToString().Trim(), "CompanyName").Replace("#rn", Environment.NewLine);
                CompanyAddressOutlet = Entity.GetCustomData(dt.Rows[0]["OutletInfo"].ToString().Trim(), "CompanyAddress").Replace("#rn", Environment.NewLine);
                VatRegOutlet = Entity.GetCustomData(dt.Rows[0]["OutletInfo"].ToString().Trim(), "VatRegistrationNumber").Replace("#rn", Environment.NewLine);
            }
            if (SyncOutletId == 0 || string.IsNullOrEmpty(CompanyNameOutlet) || string.IsNullOrEmpty(CompanyAddressOutlet) || string.IsNullOrEmpty(VatRegOutlet))
            {
                XmlDocument xml = new XmlDocument();
                string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
                xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);
                CompanyName = xml.SelectSingleNode("/SettingsObject/CompanyName").InnerText.Replace("#rn", Environment.NewLine);
                CompanyAddress = xml.SelectSingleNode("/SettingsObject/CompanyAddress").InnerText.Replace("#rn", Environment.NewLine);
                VatReg = xml.SelectSingleNode("/SettingsObject/VatRegistrationNumber").InnerText.Replace("#rn", Environment.NewLine);
            }
            else
            {
                CompanyName = CompanyNameOutlet;
                CompanyAddress = CompanyAddressOutlet;
                VatReg = VatRegOutlet;
            }

            string companyLogoPath = DBUtility.GetFilePathFromAppConfig("CompanyLogo.png");
            string CompanyLogo = new Uri(HttpContext.Current.Server.MapPath("~" + companyLogoPath)).AbsoluteUri;
            string ThreeSLogo = new Uri(HttpContext.Current.Server.MapPath("~/Images/logo.jpg")).AbsoluteUri;
            ReportParameter rptCompanyLogo = new ReportParameter("rptCompanyLogo", CompanyLogo);
            paras.Add(rptCompanyLogo);
            ReportParameter rptCompanyName = new ReportParameter("rptCompanyName", CompanyName);
            paras.Add(rptCompanyName);
            ReportParameter rptCompanyAddress = new ReportParameter("rptCompanyAddress", CompanyAddress);
            paras.Add(rptCompanyAddress);
            ReportParameter rptVatReg = new ReportParameter("rptVatReg", VatReg);
            paras.Add(rptVatReg);
            ReportParameter rpt3SLogo = new ReportParameter("rpt3SLogo", ThreeSLogo);
            paras.Add(rpt3SLogo);

            return paras;
        }

        public static GridPdfExportingArgs GetRawHTML(GridPdfExportingArgs e, string Header)
        {
            //XmlDocument xml = new XmlDocument();
            //xml.Load(HttpContext.Current.Server.MapPath("~") + "/HOSTTSettings.txt");
            //string CompanyName = xml.SelectSingleNode("/SettingsObject/CompanyName").InnerText;
            //string CompanyAddress = xml.SelectSingleNode("/SettingsObject/CompanyAddress").InnerText;
            //string VatReg = xml.SelectSingleNode("/SettingsObject/VatRegistrationNumber").InnerText;

            XmlDocument xml = new XmlDocument();
            string hosttSettingsPath = DBUtility.GetFilePathFromAppConfig("HOSTTSettings.txt");
            xml.Load(HttpContext.Current.Server.MapPath("~") + hosttSettingsPath);

            string CompanyName = xml.SelectSingleNode("/SettingsObject/CompanyName").InnerText.Replace("#rn", Environment.NewLine);
            string CompanyAddress = xml.SelectSingleNode("/SettingsObject/CompanyAddress").InnerText.Replace("#rn", Environment.NewLine); ;
            string VatReg = xml.SelectSingleNode("/SettingsObject/VatRegistrationNumber").InnerText.Replace("#rn", Environment.NewLine);
            string companyLogoPath = DBUtility.GetFilePathFromAppConfig("CompanyLogo.png");
            string CompanyLogo = new Uri(HttpContext.Current.Server.MapPath("~" + companyLogoPath)).AbsoluteUri;
            string ThreeSLogo = new Uri(HttpContext.Current.Server.MapPath("~/Images/logo.jpg")).AbsoluteUri;

            //string sCompanyLogo = "<img src=\"../images/CompanyLogo.png\" style=\"width:90px;height:90px;float:left;\"/>";
            string sCompanyLogo = string.Format("<img src=\"{0}\" style=\"width:90px;height:90px;float:left;\"/>",CompanyLogo);
            string strCompanyName = "<div style=\"text-align:left;font-size:10px;font-family:Verdana;float:left;\">" + CompanyName +"</div>";
            string strCompanyAddress = "<div style=\"text-align:left;font-size:8px;font-family:Verdana;\">" + CompanyAddress + "</div>";
            string strVatReg = "<div width=\"100%\" style=\"text-align:left;font-size:8px;font-family:Verdana;\">" + VatReg + "</div>";
            string strHeader = "<div width=\"100%\" style=\"text-align:left;font-size:10px;font-family:Verdana;\">" + Header + "</div>";
            string strFooter = "<div width=\"100%\" style=\"font-size:10px;font-family:Verdana;\">Powered By</div>";
            //string strFooterLogo = "<div width=\"100%\"><img src=\"../images/logo.jpg\"/></div>";
            string strFooterLogo =string.Format( "<div width=\"100%\"><img src=\"{0}\"/></div>",ThreeSLogo);
            e.RawHTML = sCompanyLogo + strCompanyName+ strCompanyAddress+ strVatReg+ strHeader+ e.RawHTML + strFooter + strFooterLogo;


            //string test = "<table><tbody><tr><td><img src=\"../images/CompanyLogo.png\" alt=\"W3Schools.com\" style=\"float:left;margin-right:10px;margin-top:20px;\" width=\"100\" height=\"140\"/></td><td><div style=\"font-size:20px;margin-top:0px;\">The Manhattan Fish Market</div><div>Ahmed Tower, Blah Blah</div><div>Vat Reg</div><div>Phone </div></td></tr></tbody></table>";

            //e.RawHTML = test + e.RawHTML;

            return e;
        }
    }
}
