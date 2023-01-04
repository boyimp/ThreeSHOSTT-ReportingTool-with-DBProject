using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class UI_ReportViewer : System.Web.UI.Page
{
    string _sReportName;
    string _sReportTitle;
    private DataSet _oDataSet;
    ReportParameter _rParam = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //_path = Server.MapPath("~") + @"bin" + @"\LOGO_RED.bmp";
        if (!IsPostBack)
        {
            _oDataSet = (DataSet)Session["oDataSet"];
            _sReportName = (string)Session["oReportName"];
            _sReportTitle = (string)Session["oReportTitle"];

            if (_oDataSet.Tables[0].TableName == "OpeningStockTable" || _oDataSet.Tables[0].TableName == "PurchaseStockTable" || _oDataSet.Tables[0].TableName == "ClosingStockTable")
            {
                OpeningPurchaseClosingStockLoadData();
            }
            else
            {
                LoadData();
            }
        }
    }

    private void PurchaseStockLoadData()
    {

    }

    private void OpeningPurchaseClosingStockLoadData()
    {
        try
        {
            rptViewer.ShowPrintButton = true;
            ReportParameter rp = new ReportParameter("ReportTitle", _sReportTitle);
            rptViewer.LocalReport.DataSources.Clear();
            _oDataSet.DataSetName = "rptDataset";
            rptViewer.LocalReport.ReportPath = _sReportName;
            rptViewer.LocalReport.SetParameters(new ReportParameter[] { rp });
            rptViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", _oDataSet.Tables[0]));

            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.LocalReport.Refresh();
        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }
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

            if (_oDataSet.Tables[0].TableName == "TransferredTo")
                rptViewer.LocalReport.SetParameters(getparamsforinventory());
            else
                rptViewer.LocalReport.SetParameters(getparams());
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

    private List<ReportParameter> getparamsforinventory()
    {
        List<ReportParameter> _parameters = new List<ReportParameter>();

        string TicketNo = (string)Session["StartDate"];
        _rParam = new ReportParameter("StartDate", TicketNo);
        _parameters.Add(_rParam);

        string TicketDate = (string)Session["EndDate"];
        _rParam = new ReportParameter("EndDate", TicketDate);
        _parameters.Add(_rParam);

        string DeliveryDate = (string)Session["GroupCode"];
        _rParam = new ReportParameter("GroupCode", DeliveryDate);
        _parameters.Add(_rParam);

        string TotalTax = (string)Session["InventoryName"];
        _rParam = new ReportParameter("InventoryName", TotalTax);
        _parameters.Add(_rParam);

        string TicketTotal = (string)Session["WarehouseName"];
        _rParam = new ReportParameter("WarehouseName", TicketTotal);
        _parameters.Add(_rParam);
        return _parameters;

    }
}