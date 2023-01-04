using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;
using BusinessObjects.TicketsManager;
using BusinessObjects.MenusManager;
using ThreeS.Domain.Models.Tickets;
using BusinessObjects.ReportManager;
using Microsoft.Reporting.WebForms;
using System.Threading;
using BusinessObjects;

public partial class UI_ItemWiseSaleTax : System.Web.UI.Page
{
    static int completedProcess = 0;
    static int totalProcess = -1;
    static string errorString = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.CurrentUser))
            Response.Redirect("./");
        if (!IsPostBack)
        {
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.SelectedDate = DateTime.Today; 
            DateErrorMessage.Visible = false;
            TotalPendingProcess.Visible = false;
            btnProcess.Visible = false;
            RunningPendingProcess.Visible = false;
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
        if (validateDateTime())
            return;

        DateErrorMessage.Visible = false;

        DateTime dtFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
        DateTime dtToDate = Convert.ToDateTime(dtpToDate.SelectedDate);
        errorString = string.Empty;
        int pendingProcessOrder = TicketManager.pendingProcessTicketId(dtFromDate, dtToDate).Count();

        if(pendingProcessOrder > 0)
        {
            TotalPendingProcess.Text = $"{pendingProcessOrder} pending process remaining.";
            RunningPendingProcess.Text = $"{completedProcess} process has been completed.";
            
            TotalPendingProcess.Visible = true;
            RunningPendingProcess.Visible = true;
            btnProcess.Visible = true;
            DateErrorMessage.Text = string.Empty;
            DateErrorMessage.Visible = false;
        }
        else
        {
            DateErrorMessage.Text = "No pending process remaining.";
            DateErrorMessage.Visible = true;
        }
        completedProcess = 0; 
    }


    private bool validateDateTime()
    {
        if (string.IsNullOrEmpty(dtpFromDate.SelectedDate.ToString()) || string.IsNullOrEmpty(dtpToDate.SelectedDate.ToString()))
        {
            DateErrorMessage.Text = "Please Select a Valid Date Time.";
            DateErrorMessage.Visible = true;
            return true;
        }
        else if(dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
        {
            DateErrorMessage.Text = "From Date Can Not Be Bigger Than To Date.";
            DateErrorMessage.Visible = true;
            return true;
        }
        return false;
    }


    protected void btnProcess_Click(object sender, EventArgs e)
    {
        DBUtility.Current = HttpContext.Current;
        var thread = new Thread(() =>
        {
            DateTime dtFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            DateTime dtToDate = Convert.ToDateTime(dtpToDate.SelectedDate);


            IEnumerable<CalculationType> oCalculationTypes = TicketManager.GetObjectCalculationTypes();
            List<int> ticketIds = TicketManager.pendingProcessTicketId(dtFromDate, dtToDate);
            totalProcess = ticketIds.Count();
            errorString = string.Empty;
            foreach (int ids in ticketIds)
            {
                List<Ticket> tickets = TicketManager.GetIndividualTicket(ids);

                var ds = TicketManager.GetMenuItemOrderAccountsInfoData(tickets, oCalculationTypes);
                var individualOrderIds = ds.Select(x => x.OrderId).Distinct().ToList();


                foreach (int id in individualOrderIds)
                {
                    var individualCalculationList = ds.Where(x => (x.OrderId == id) && (x.Amount != 0)).Select(y => new
                    {
                        CalculationAmount = y.Amount,
                        CalculationKey = "",
                        CalculationName = y.AccountHead,
                        CalculationSumValue = 0,
                        CalculationTypeID = oCalculationTypes.Where(b => b.Name == y.AccountHead).Select(c => c.Id).FirstOrDefault(),
                        IsDiscount = y.AccountHead == "Discount" ? true : false,
                        IsSC = false,
                        IsSD = false,
                        IsTax = (y.AccountHead == "VAT." || y.AccountHead == "VAT" || y.AccountHead == "VAT Mer-") ? true : false,
                        Price = 0
                    }).ToList();

                    //RunningPendingProcess.Text = $"{completedProcess} process has been completed.";
                    //UpdatePanel1.Update();


                    if (!individualCalculationList.Any())
                    {
                        errorString = string.IsNullOrEmpty(errorString) ? $"{id}" : $"{errorString}, {id}";
                        continue;
                    }


                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(individualCalculationList);
                    int ticketReturn = TicketManager.UpdateOrderByOrderCalculationType(id, data);


                    if (ticketReturn != 0)
                    {
                        errorString = string.IsNullOrEmpty(errorString) ? $"{ticketReturn}" : $"{errorString}, {ticketReturn}";
                    }
                }

                completedProcess++;
            }
        })
        {
            IsBackground = true
        };
        thread.Start();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        RunningPendingProcess.Text = $"{completedProcess} process has been completed.";
        UpdatePanel1.Update();
        if (!string.IsNullOrEmpty(errorString))
        {
            DateErrorMessage.Text = $"{errorString} This Orders couldn't get updated.";
            DateErrorMessage.Visible = true;
            UpdatePanel2.Update();
        }
    }
}