using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class UI_ReportViewerAlter : System.Web.UI.Page
{
    string _sReportName;
    string _sReportTitle;
    string _sDataSetName;
    private DataSet _oDataSet;
    List<ReportParameter> _rParam = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //_path = Server.MapPath("~") + @"bin" + @"\LOGO_RED.bmp";
        if (!IsPostBack)
        {
            _oDataSet = (DataSet)Session["oDataSet"];
            _sReportName = (string)Session["oReportName"];
            _sReportTitle = (string)Session["oReportTitle"];
            _sDataSetName = (string)Session["DataSetName"];
            _rParam = (List<ReportParameter>)Session["ReportParams"];

            LoadData();
        }
    }

    public void LoadData()
    {
        try
        {
            rptViewer.ShowPrintButton = true;
            rptViewer.LocalReport.DataSources.Clear();
            //_oDataSet.DataSetName = "rptDataset";

            if (_oDataSet.Tables.Count > 1)
            {
                foreach (DataTable oDT in _oDataSet.Tables)
                {
                    rptViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(_sDataSetName + "_" + oDT.TableName, oDT));
                }
            }
            else
            {
                rptViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(_sDataSetName, _oDataSet.Tables[0]));
            }

            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.LocalReport.ReportPath = _sReportName;

            if (_rParam != null)
            {
                rptViewer.LocalReport.SetParameters(_rParam);
            }
            rptViewer.LocalReport.Refresh();
        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }

    }

    private void RenderReport(ReportViewer reportViewer, HttpResponse response)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string extension;
        reportViewer.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        response.Buffer = true;
        response.Clear();
        response.ContentType = mimeType;
        response.AppendHeader("content-Disposition", "inline: filename=TrainingOfficialRecord." + extension);
        response.BinaryWrite(bytes);
        response.Flush();
    }
}