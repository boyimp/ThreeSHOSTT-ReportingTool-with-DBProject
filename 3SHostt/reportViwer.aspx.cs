using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class reportViwer : System.Web.UI.Page
{
    string _path;
    string _sReportName;
    private DataSet _oDataSet;
    ReportParameter _rParam = null;
    DateTime _SalaryMonth;

    double _LoanAmount;
    double _InterestRate;
    int _noOfInstallment;
    string _LoanName;
    string totaltaka;
    DateTime _IssueDate;
    DateTime _dFromDate;
    DateTime _dToDate;
    List<ReportParameter> _parameters = new List<ReportParameter>();
    string _PATH = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        _path = Server.MapPath("~") + @"bin" + @"\Logo.jpg";
        if (!IsPostBack)
        {
            //CustomizeRV((System.Web.UI.Control)sender);

            _oDataSet = (DataSet)Session["oDataSet"];
            _sReportName = (string)Session["oReportName"];
            if (_oDataSet != null && !_sReportName.Equals(string.Empty))
                LoadData();
        }
    }

    public void LoadData()
    {
        ReportViewer rptViewer = new ReportViewer();
        rptViewer.ShowPrintButton = true;
        rptViewer.LocalReport.DataSources.Clear();
        _oDataSet.DataSetName = "rptDataSet";

        foreach (DataTable oDT in _oDataSet.Tables)
        {
            rptViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(oDT.DataSet.DataSetName + "_" + oDT.TableName, oDT));
        }
        rptViewer.LocalReport.EnableExternalImages = true;
        rptViewer.LocalReport.ReportPath = _sReportName;

        //if (Request.QueryString.Count > 0)
        //{
        //    if (Convert.ToInt32(Request.QueryString["Menu"]) == 50)
        //    {
        //        rptViewer.LocalReport.SetParameters(getparams());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 80)
        //    {
        //        rptViewer.LocalReport.SetParameters(getParmsForNewTaxCard());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 53)
        //    {
        //        rptViewer.LocalReport.SetParameters(getparamsForOPIPaySlip());
        //    }

        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 54)
        //    {
        //        rptViewer.LocalReport.SetParameters(getparamsForPayslip());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"] == "" ? "0" : Request.QueryString["Menu"]) == 51)
        //    {
        //        rptViewer.LocalReport.SetParameters(getAllParamsForLoanIssue());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"] == "" ? "0" : Request.QueryString["Menu"]) == 52)
        //    {
        //        rptViewer.LocalReport.SetParameters(getAllCommonParamsForIndividual());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 60)
        //    {
        //        rptViewer.LocalReport.SetParameters(getparams(_path));
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 70)
        //    {
        //        rptViewer.LocalReport.SetParameters(getParmsForBonus());
        //    }
        //    else if (Convert.ToInt32(Request.QueryString["Menu"]) == 81)
        //    {
        //        rptViewer.LocalReport.SetParameters(getParamsForSalaryCertificate());
        //    }
        //    else
        //    {
        //        rptViewer.LocalReport.SetParameters(getCommonParams());
        //    }
        //}

        //SessionManager.CurrentEmployee = SessionManager.CurrentEmployee;
        RenderReport(rptViewer, Response);
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

    //private List<ReportParameter> getParmsForBonus()
    //{
    //    Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();
    //    List<ReportParameter> _parameters = new List<ReportParameter>();

    //    ObjectsTemplate<PayrollType> oPRTypes = PayrollType.Get();

    //    PayrollType oPT = oPRTypes.Find(delegate(PayrollType oItemPT) { return oItemPT.ID == Payroll.BO.SystemInformation.CurrentSysInfo.PayrollTypeID; });

    //    string logoPath1 = string.Empty;
    //    logoPath1 = Server.MapPath("images/Logo.jpg");
    //    _rParam = new ReportParameter("Logo", logoPath1);

    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Address", Payroll.BO.SystemInformation.CurrentSysInfo.corporateAddress);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("SearchCriteria", " ");
    //    _parameters.Add(_rParam);
    //    return _parameters;
    //}

    //private List<ReportParameter> getParmsForTaxCard()
    //{
    //    List<ReportParameter> _parameters = new List<ReportParameter>();
    //    string logoPath1 = string.Empty;
    //    logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";

    //    ObjectsTemplate<PayrollType> oPRTypes = PayrollType.Get();

    //    PayrollType oPT = oPRTypes.Find(delegate(PayrollType oItemPT) { return oItemPT.ID == Payroll.BO.SystemInformation.CurrentSysInfo.PayrollTypeID; });

    //    _rParam = new ReportParameter("Logo", logoPath1);
    //    _parameters.Add(_rParam);

    //    //_rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //    //_parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("CompanyInfo", "Citi Group");
    //    //_parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("Address", Payroll.BO.SystemInformation.CurrentSysInfo.corporateAddress);
    //    //_parameters.Add(_rParam);

    //    if (oPT != null)
    //    {
    //        if (oPT.ID.Integer == 1)
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = " Bay's Galleria (1st floor)\n 57 Gulshan Avenue\n Dhaka 1212";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 883 4990 (Auto Hunting) \nFacsimile: +880 (2) 883 4377";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);

    //        }
    //        else if (oPT.ID.Integer == 2)
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = "36 Dilkusha C/A (13th floor)\n Dhaka 1000 ";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 957 1842 (Auto Hunting) \nFacsimile: +880 (2) 716 1544";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);
    //        }
    //        else
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = "Eunoos Trade Centre (Level 21) \n52-53 Dilkusha C/A \nDhaka 1000 ";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 957 1170 (Auto Hunting)\nFacsimile: +880 (2) 957 1171";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);
    //        }
    //    }



    //    _rParam = new ReportParameter("SearchCriteria", " ");
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("EmpName", SessionManager.CurrentEmployee.Name);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("EmpNo", SessionManager.CurrentEmployee.EmployeeNo);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("TINNo", SessionManager.CurrentEmployee.TinNo);
    //    _parameters.Add(_rParam);

    //    string AssessmentYear = (string)Session["AssessmentYear"];

    //    string FiscalYear = (string)Session["FiscalYear"];

    //    _rParam = new ReportParameter("AssesmentYear", AssessmentYear);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("FiscalYear", FiscalYear);
    //    _parameters.Add(_rParam);

    //    return _parameters;
    //}

    //private List<ReportParameter> getParamsForSalaryCertificate()
    //{
    //    Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();

    //    List<ReportParameter> _parameters = new List<ReportParameter>();
    //    string logoPath1 = string.Empty;
    //    logoPath1 = Server.MapPath("images/Citi Logo.JPG");

    //    _rParam = new ReportParameter("Logo", logoPath1);
    //    _parameters.Add(_rParam);

    //    string signature = Server.MapPath("images/Faisal Signature.JPG");

    //    _rParam = new ReportParameter("SignatureLogo", signature);
    //    _parameters.Add(_rParam);

    //    string bar = Server.MapPath("images/Citi Banner.JPG");

    //    _rParam = new ReportParameter("BarLogo", bar);
    //    _parameters.Add(_rParam);

    //    string FromYear = (string)Session["FromYear"];
    //    string FromDate = (string)Session["FromDate"];
    //    string ToYear = (string)Session["ToYear"];
    //    string ToDate = (string)Session["ToDate"];

    //    _rParam = new ReportParameter("FromYear", FromYear);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("ToYear", ToYear);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("FromDate", FromDate);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("ToDate", ToDate);
    //    _parameters.Add(_rParam);

    //    return _parameters;
    //}

    //private List<ReportParameter> getParmsForNewTaxCard()
    //{
    //    Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();

    //    List<ReportParameter> _parameters = new List<ReportParameter>();
    //    string logoPath1 = string.Empty;
    //    logoPath1 = Server.MapPath("images/Logo.jpg");

    //    _rParam = new ReportParameter("Logo", logoPath1);
    //    _parameters.Add(_rParam);

    //    //_rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //    //_parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("CompanyInfo", "Novartis Bangladesh Limeted");
    //    //_parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("Address", Payroll.BO.SystemInformation.CurrentSysInfo.corporateAddress);
    //    //_parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("SearchCriteria", " ");
    //    //_parameters.Add(_rParam);
    //    _rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("EmpName", SessionManager.CurrentEmployee.Name);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("EmpNo", SessionManager.CurrentEmployee.EmployeeNo);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("TINNo", string.IsNullOrEmpty(SessionManager.CurrentEmployee.TinNo) ? string.Empty : SessionManager.CurrentEmployee.TinNo);
    //    _parameters.Add(_rParam);



    //    string AssessmentYear = (string)Session["AssessmentYear"];

    //    string FiscalYear = (string)Session["FiscalYear"];

    //    _rParam = new ReportParameter("AssesmentYear", AssessmentYear);
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("FiscalYear", FiscalYear);
    //    _parameters.Add(_rParam);

    //    string Designation = (string)Session["Designation"];
    //    _rParam = new ReportParameter("Designation", Designation);
    //    _parameters.Add(_rParam);

    //    string Department = (string)Session["Department"];
    //    _rParam = new ReportParameter("Department", Department);
    //    _parameters.Add(_rParam);

    //    string RC = (string)Session["RC"];
    //    _rParam = new ReportParameter("RC", RC);
    //    _parameters.Add(_rParam);

    //    string Location = (string)Session["Location"];
    //    _rParam = new ReportParameter("Location", Location);
    //    _parameters.Add(_rParam);

    //    string Division = (string)Session["Division"];
    //    _rParam = new ReportParameter("Division", Division);
    //    _parameters.Add(_rParam);

    //    string Gender = (string)Session["Gender"];
    //    _rParam = new ReportParameter("Gender", Gender);
    //    _parameters.Add(_rParam);

    //    return _parameters;
    //}

    //private List<ReportParameter> getAllCommonParamsForIndividual()
    //{
    //    List<ReportParameter> _parameters = new List<ReportParameter>();
    //    string logoPath1 = string.Empty;
    //    logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
    //    _rParam = new ReportParameter("Logo", logoPath1);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("CompanyInfo", "Citi Group");
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Address", Payroll.BO.SystemInformation.CurrentSysInfo.corporateAddress);
    //    _parameters.Add(_rParam);

    //    HREmployee _HREmp = new HREmployee();
    //    HREmployee employee = new HREmployee();
    //    employee = _HREmp.Get(SessionManager.CurrentEmployee.EmployeeNo);
    //    _HREmp = employee;

    //    ObjectsTemplate<PhotoPath> _oPhotoPaths = PhotoPath.Get();
    //    string SearchCriteria;
    //    if (employee.PhotoPath != string.Empty)
    //        SearchCriteria = _oPhotoPaths[0].EmployeePhoto + "\\" + employee.PhotoPath;

    //    else SearchCriteria = "";

    //    _rParam = new ReportParameter("SearchCriteria", SearchCriteria);
    //    _parameters.Add(_rParam);
    //    return _parameters;
    //}

    //private List<ReportParameter> getAllParamsForLoanIssue()
    //{
    //    Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();
    //    _InterestRate = Convert.ToDouble(Request.QueryString[1]);
    //    _IssueDate = Convert.ToDateTime(Request.QueryString[2]);
    //    _LoanAmount = Convert.ToDouble(Request.QueryString[3]);
    //    _noOfInstallment = Convert.ToInt32(Request.QueryString[4]);
    //    _LoanName = Convert.ToString(Request.QueryString[5]);
    //    _dFromDate = Convert.ToDateTime(Request.QueryString[6]);
    //    _dToDate = Convert.ToDateTime(Request.QueryString[7]);
    //    List<ReportParameter> _parameters = new List<ReportParameter>();
    //    string logoPath1 = string.Empty;
    //    string logoPath2 = string.Empty;

    //    logoPath1 = Server.MapPath("images/Logo.jpg");
    //    //_rParam = new ReportParameter("Logo", logoPath1);//
    //    //_parameters.Add(_rParam);

    //    //_rParam = new ReportParameter("CompanyInfo" + _systemInfo.name.ToString());//
    //    //_parameters.Add(_rParam);

    //    // _rParam = new ReportParameter("CompanyInfo", "Novartis Bangladesh Limeted");
    //    // _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Address", Payroll.BO.SystemInformation.CurrentSysInfo.corporateAddress);
    //    _parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //    //_parameters.Add(_rParam);
    //    _rParam = new ReportParameter("Name", " " + SessionManager.CurrentEmployee.Name);//
    //    _parameters.Add(_rParam);
    //    _rParam = new ReportParameter("EmpNo", " " + SessionManager.CurrentEmployee.EmployeeNo);//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("Amount", " " + _LoanAmount.ToString());//
    //    _parameters.Add(_rParam);
    //    //_rParam = new ReportParameter("SearchCriteria", " ");//
    //    //_parameters.Add(_rParam);

    //    _rParam = new ReportParameter("NoOfInstallment", " " + _noOfInstallment.ToString());//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("LoanName", _LoanName);//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("ToDate", _dToDate.ToString("MMM yyyy"));//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("FromDate", _dFromDate.ToString("MMM yyyy"));//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("InterestRate", _InterestRate.ToString());//
    //    _parameters.Add(_rParam);

    //    // _rParam = new ReportParameter("IssueDate", _IssueDate.ToString("MMM yyyy"));
    //    // _parameters.Add(_rParam);

    //    Employee omp = SessionManager.CurrentEmployee;

    //    _rParam = new ReportParameter("Designation", omp.Designation.Name);//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("Department", omp.Department.Name);//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("Location", omp.Location.Name);//
    //    _parameters.Add(_rParam);

    //    _rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //    _parameters.Add(_rParam);

    //    if (File.Exists(logoPath1) == true)
    //    {
    //        _rParam = new ReportParameter("Logo", logoPath1);
    //        _parameters.Add(_rParam);
    //    }

    //    //ObjectsTemplate<EmployeeCostCenter> obRCs = EmployeeCostCenter.GetByEmpID(omp.ID);

    //    //var query = obRCs.Where(ob => ob.IsCurrentCC == true);

    //    //if (query.Count() == 1)
    //    //{
    //    //    _rParam = new ReportParameter("RC", query.First().Costcenter.Name);//
    //    //    _parameters.Add(_rParam);
    //    //}

    //    //else
    //    //{
    //    //    _rParam = new ReportParameter("RC", "");//
    //    //    _parameters.Add(_rParam);
    //    //}

    //    return _parameters;
    //}

    //private List<ReportParameter> getparamsForPayslip()
    //{
    //    try
    //    {
    //        Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();
    //        List<ReportParameter> _parameters = new List<ReportParameter>();
    //        string logoPath1 = string.Empty;
    //        string logoPath2 = string.Empty;
    //        //logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
    //        //logoPath2 = Server.MapPath("~") + @"\bin" + @"\Logo1.jpg";

    //        logoPath1 = Server.MapPath("images/Logo.jpg");
    //        // logoPath2 = Server.MapPath("images/Logo1.jpg");

    //        _parameters = new List<ReportParameter>();
    //        if (File.Exists(logoPath1) == true)
    //        {
    //            _rParam = new ReportParameter("Logo", logoPath1);
    //            _parameters.Add(_rParam);
    //        }
    //        //if (File.Exists(logoPath2) == true)
    //        //{
    //        //    _rParam = new ReportParameter("Logo1", logoPath2);
    //        //    _parameters.Add(_rParam);
    //        //}
    //        //_SalaryMonth = GlobalFunctions.LastDateOfMonth(Convert.ToDateTime(Request.QueryString[1]));
    //        //_rParam = new ReportParameter("SalaryMonth", _SalaryMonth.ToString("MMM yyyy"));
    //        //_parameters.Add(_rParam);

    //        _rParam = new ReportParameter("Address", _systemInfo.corporateAddress.ToString());
    //        _parameters.Add(_rParam);

    //        _rParam = new ReportParameter("Phone", _systemInfo.TelephoneNo.ToString());
    //        _parameters.Add(_rParam);

    //        _rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //        _parameters.Add(_rParam);

    //        return _parameters;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}
    //private List<ReportParameter> getparams()
    //{
    //    try
    //    {
    //        Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();

    //        ObjectsTemplate<PayrollType> oPRTypes = PayrollType.Get();

    //        PayrollType oPT = oPRTypes.Find(delegate(PayrollType oItemPT) { return oItemPT.ID == Payroll.BO.SystemInformation.CurrentSysInfo.PayrollTypeID; });

    //        string logoPath1 = Server.MapPath("images/GroupLogo.png"); ;
    //        string logoPath2 = string.Empty;
    //        //logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
    //        //logoPath2 = Server.MapPath("~") + @"\bin" + @"\Logo1.jpg";

    //        // logoPath1 = Server.MapPath("images/LOGO_IDLC.bmp");
    //        //logoPath2 = Server.MapPath("images/LOGO_IDLC.bmp");

    //        _parameters = new List<ReportParameter>();

    //        //_rParam = new ReportParameter("Logo", logoPath1);
    //        //_parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //        //_parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("CompanyInfo",_systemInfo.name.ToString());
    //        //_parameters.Add(_rParam);

    //        _rParam = new ReportParameter("IncomeTaxTotal", Session["incomeTaxTotal"].ToString());
    //        _parameters.Add(_rParam);

    //        if (oPT != null)
    //        {
    //            if (oPT.ID.Integer == 1)
    //            {
    //                //_PATH = Server.MapPath("images/citylogo2.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\nBay's Galleria (1st floor)\n57 Gulshan Avenue\nDhaka 1212";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 883 4990 (Auto Hunting) \nFacsimile: +880 (2) 883 4377";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);

    //            }
    //            else if (oPT.ID.Integer == 2)
    //            {
    //                //_PATH = Server.MapPath("images/citylogo2.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\n36 Dilkusha C/A (13th floor)\n Dhaka 1000 ";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 957 1842 (Auto Hunting) \nFacsimile: +880 (2) 716 1544";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);
    //            }
    //            else
    //            {
    //                //_PATH = Server.MapPath("images/IDLCIL.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\nEunoos Trade Centre (Level 21) \n52-53 Dilkusha C/A \nDhaka 1000 ";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 957 1170 (Auto Hunting)\nFacsimile: +880 (2) 957 1171";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);
    //            }
    //        }


    //        _rParam = new ReportParameter("SearchCriteria", " ");
    //        _parameters.Add(_rParam);

    //        if (File.Exists(logoPath1) == true)
    //        {
    //            _rParam = new ReportParameter("Logo", logoPath1);
    //            _parameters.Add(_rParam);
    //        }
    //        //if (File.Exists(logoPath2) == true)
    //        //{
    //        //    _rParam = new ReportParameter("Logo1", logoPath2);
    //        //    _parameters.Add(_rParam);
    //        //}
    //        _SalaryMonth = GlobalFunctions.LastDateOfMonth(Convert.ToDateTime(Request.QueryString[1]));

    //        totaltaka = Request.QueryString[2];

    //        _rParam = new ReportParameter("TotalTaka", totaltaka);
    //        _parameters.Add(_rParam);
    //        _rParam = new ReportParameter("SalaryMonth", _SalaryMonth.ToString("MMMM yyyy"));
    //        _parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //        //_parameters.Add(_rParam);

    //        return _parameters;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //private List<ReportParameter> getparamsForOPIPaySlip()
    //{
    //    try
    //    {
    //        Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();

    //        ObjectsTemplate<PayrollType> oPRTypes = PayrollType.Get();

    //        PayrollType oPT = oPRTypes.Find(delegate(PayrollType oItemPT) { return oItemPT.ID == Payroll.BO.SystemInformation.CurrentSysInfo.PayrollTypeID; });

    //        string logoPath1 = Server.MapPath("images/GroupLogo.png"); ;
    //        string logoPath2 = string.Empty;
    //        //logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
    //        //logoPath2 = Server.MapPath("~") + @"\bin" + @"\Logo1.jpg";

    //        // logoPath1 = Server.MapPath("images/LOGO_IDLC.bmp");
    //        //logoPath2 = Server.MapPath("images/LOGO_IDLC.bmp");

    //        _parameters = new List<ReportParameter>();

    //        //_rParam = new ReportParameter("Logo", logoPath1);
    //        //_parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("Phone", Payroll.BO.SystemInformation.CurrentSysInfo.TelephoneNo);
    //        //_parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //        //_parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("IncomeTaxTotal", Session["incomeTaxTotal"].ToString());
    //        //_parameters.Add(_rParam);

    //        if (oPT != null)
    //        {
    //            if (oPT.ID.Integer == 1)
    //            {
    //                //_PATH = Server.MapPath("images/citylogo2.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\nBay's Galleria (1st floor)\n57 Gulshan Avenue\nDhaka 1212";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 883 4990 (Auto Hunting) \nFacsimile: +880 (2) 883 4377";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);

    //            }
    //            else if (oPT.ID.Integer == 2)
    //            {
    //                //_PATH = Server.MapPath("images/citylogo2.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\n36 Dilkusha C/A (13th floor)\n Dhaka 1000 ";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 957 1842 (Auto Hunting) \nFacsimile: +880 (2) 716 1544";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);
    //            }
    //            else
    //            {
    //                //_PATH = Server.MapPath("images/IDLCIL.jpg");
    //                //_rParam = new ReportParameter("Logo", _PATH);
    //                //_parameters.Add(_rParam);

    //                _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //                _parameters.Add(_rParam);

    //                string address = oPT.Description + "\nEunoos Trade Centre (Level 21) \n52-53 Dilkusha C/A \nDhaka 1000 ";
    //                _rParam = new ReportParameter("Address", address);
    //                _parameters.Add(_rParam);

    //                string phone = "+880 (2) 957 1170 (Auto Hunting)\nFacsimile: +880 (2) 957 1171";
    //                _rParam = new ReportParameter("Phone", phone);
    //                _parameters.Add(_rParam);
    //            }
    //        }


    //        _rParam = new ReportParameter("SearchCriteria", " ");
    //        _parameters.Add(_rParam);

    //        if (File.Exists(logoPath1) == true)
    //        {
    //            _rParam = new ReportParameter("Logo", logoPath1);
    //            _parameters.Add(_rParam);
    //        }
    //        //if (File.Exists(logoPath2) == true)
    //        //{
    //        //    _rParam = new ReportParameter("Logo1", logoPath2);
    //        //    _parameters.Add(_rParam);
    //        //}
    //        _SalaryMonth = GlobalFunctions.LastDateOfMonth(Convert.ToDateTime(Request.QueryString[1]));

    //        totaltaka = Request.QueryString[2];

    //        _rParam = new ReportParameter("TotalTaka", totaltaka);
    //        _parameters.Add(_rParam);
    //        _rParam = new ReportParameter("SalaryMonth", _SalaryMonth.ToString("MMMM yyyy"));
    //        _parameters.Add(_rParam);

    //        //_rParam = new ReportParameter("CompanyInfo", _systemInfo.name.ToString());
    //        //_parameters.Add(_rParam);

    //        return _parameters;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public void GetParameters()
    //{
    //    Payroll.BO.SystemInformation _systemInfo = Payroll.BO.SystemInformation.Get();
    //    _parameters = new List<ReportParameter>();

    //    ObjectsTemplate<PayrollType> oPRTypes = PayrollType.Get();

    //    PayrollType oPT = oPRTypes.Find(delegate(PayrollType oItemPT) { return oItemPT.ID == Payroll.BO.SystemInformation.CurrentSysInfo.PayrollTypeID; });

    //    if (oPT != null)
    //    {
    //        if (oPT.ID.Integer == 1)
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = " Bay's Galleria (1st floor)\n 57 Gulshan Avenue\n Dhaka 1212";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 883 4990 (Auto Hunting) \nFacsimile: +880 (2) 883 4377";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);

    //        }
    //        else if (oPT.ID.Integer == 2)
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = "36 Dilkusha C/A (13th floor)\n Dhaka 1000 ";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 957 1842 (Auto Hunting) \nFacsimile: +880 (2) 716 1544";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);
    //        }
    //        else
    //        {
    //            _PATH = Server.MapPath("images/Logo.jpg");
    //            _rParam = new ReportParameter("Logo", _PATH);
    //            _parameters.Add(_rParam);

    //            _rParam = new ReportParameter("CompanyInfo", oPT.Description);
    //            _parameters.Add(_rParam);

    //            string address = "Eunoos Trade Centre (Level 21) \n52-53 Dilkusha C/A \nDhaka 1000 ";
    //            _rParam = new ReportParameter("Address", address);
    //            _parameters.Add(_rParam);

    //            string phone = "+880 (2) 957 1170 (Auto Hunting)\nFacsimile: +880 (2) 957 1171";
    //            _rParam = new ReportParameter("Phone", phone);
    //            _parameters.Add(_rParam);
    //        }
    //    }


    //    _rParam = new ReportParameter("SearchCriteria", " ", false);
    //    _parameters.Add(_rParam);






    //    //rParam = new ReportParameter("Fax", "++03423232323");
    //    //_parameters.Add(rParam);

    //    //rParam = new ReportParameter("Email", _systemInfo.webAddress);
    //    //_parameters.Add(rParam);
    //}

    private List<ReportParameter> getCommonParams()
    {
        List<ReportParameter> _parameters = new List<ReportParameter>();
        string logoPath1 = string.Empty;
        string logoPath2 = string.Empty;
        logoPath1 = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
        _parameters = new List<ReportParameter>();

        _rParam = new ReportParameter("Logo", logoPath1);
        _parameters.Add(_rParam);

        //_SalaryMonth = GlobalFunctions.LastDateOfMonth(Convert.ToDateTime(Request.QueryString[0]));
        //_rParam = new ReportParameter("SalaryMonth", _SalaryMonth.ToString("MMM yyyy"));
        //_parameters.Add(_rParam);
        return _parameters;
    }

    private List<ReportParameter> getparams(string ImagePath)
    {
        //ImagePath = Server.MapPath("~") + @"\bin" + @"\Logo.jpg";
        ImagePath = Server.MapPath("images/Logo.jpg");
        List<ReportParameter> _params = new List<ReportParameter>();
        //if (File.Exists(ImagePath) == true)
        //{
        //    _params.Add(new ReportParameter("ImagePath", ImagePath));
        //}
        if (ImagePath != null)
        {
            _params.Add(new ReportParameter("ImagePath", ImagePath));
        }
        return _params;
    }

    private void CustomizeRV(System.Web.UI.Control reportControl)
    {
        foreach (System.Web.UI.Control childControl in reportControl.Controls)
        {
            if (childControl.GetType() == typeof(System.Web.UI.WebControls.Literal))
            {
                System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)childControl;
                ddList.PreRender += new EventHandler(ddList_PreRender);
            }
            if (childControl.Controls.Count > 0)
            {
                CustomizeRV(childControl);
            }
        }
    }

    void ddList_PreRender(object sender, EventArgs e)
    {
        if (sender.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
        {
            System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)sender;
            System.Web.UI.WebControls.ListItemCollection listItems = ddList.Items;

            if ((listItems != null) && (listItems.Count > 0) && (listItems.FindByText("Excel") != null))
            {
                foreach (System.Web.UI.WebControls.ListItem list in listItems)
                {
                    if (list.Text.Equals("Excel"))
                    {
                        list.Enabled = false;
                    }
                }
            }
        }
    }

    protected void rptViewer_Load(object sender, EventArgs e)
    {

    }
}