using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class ReportviewerWithoutMaster : System.Web.UI.Page
{
    string _path;
    string _sReportName;
    private DataSet _oDataSet;
    ReportParameter _rParam = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //_path = Server.MapPath("~") + @"bin" + @"\LOGO_RED.bmp";
        if (!IsPostBack)
        {
            _oDataSet = (DataSet)Session["oDataSet"];
            _sReportName = (string)Session["oReportName"];
            LoadData();
        }
    }
    public void LoadData()
    {
        try
        {
            rptViewer.ShowPrintButton = true;
            rptViewer.LocalReport.DataSources.Clear();
            _oDataSet.DataSetName = "rptDataset";


            foreach (DataTable oDT in _oDataSet.Tables)
            {
                rptViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(oDT.DataSet.DataSetName + "_" + oDT.TableName, oDT));
            }
            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.LocalReport.ReportPath = _sReportName;
            rptViewer.LocalReport.SetParameters(getparams());
            //if (Request.QueryString.Count > 0)
            //{
            //    if (Convert.ToInt32(Request.QueryString["Menu"]) == 50)
            //    {
            //        rptViewer.LocalReport.SetParameters(getparams());
            //    }
            //}

            rptViewer.LocalReport.Refresh();
        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }

    }

    private List<ReportParameter> getparams()
    {
        List<ReportParameter> _parameters = new List<ReportParameter>();

        string TicketNo = (string)Session["TicketNo"];
        _rParam = new ReportParameter("TicketNo", TicketNo);
        _parameters.Add(_rParam);

        string TicketDate = (string)Session["TicketDate"];
        _rParam = new ReportParameter("TicketDate", TicketDate);
        _parameters.Add(_rParam);

        string DeliveryDate = (string)Session["DeliveryDate"];
        _rParam = new ReportParameter("DeliveryDate", DeliveryDate);
        _parameters.Add(_rParam);

        string TotalTax = (string)Session["TotalTax"];
        _rParam = new ReportParameter("TotalTax", TotalTax);
        _parameters.Add(_rParam);

        string TicketTotal = (string)Session["TicketTotal"];
        _rParam = new ReportParameter("TicketTotal", TicketTotal);
        _parameters.Add(_rParam);

        string RemainingAmount = (string)Session["RemainingAmount"];
        _rParam = new ReportParameter("RemainingAmount", RemainingAmount);
        _parameters.Add(_rParam);

        return _parameters;

    }

}