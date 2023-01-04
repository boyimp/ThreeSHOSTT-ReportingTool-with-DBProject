<%@ Page Language="C#"  Title="Time wise Sales Chart" MasterPageFile="~/Views/ReportingTool.master"  AutoEventWireup="true" CodeFile="TimeWiseSalesChart.aspx.cs" Inherits="UI_TimeWiseSalesChart" ClientIDMode="Static" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <style>
       body {
  padding-top: 50px;
}
.bootstrap-select {
  max-width: 200px;
}
.bootstrap-select .btn {
  background-color: #141414;
  border-style: solid;
  border-left-width: 3px;
  border-left-color: #00DDDD;
  border-top: none;
  border-bottom: none;
  border-right: none;
  color: white;
  font-weight: 200;
  padding: 12px 12px;
  font-size: 16px;
  margin-bottom: 10px;
  -webkit-appearance: none;
  -moz-appearance: none;
  appearance: none;
}
.bootstrap-select .dropdown-menu {
  margin: 15px 0 0;
}
select::-ms-expand {
  /* for IE 11 */
  display: none;
}
   </style>
   
    <div style="padding-left: 25%;">
        <table style="width: 90%;">
            <tr style="width: 100%;">
                <td style="width: 50%;">
                    <span>From Date:</span>
                    <div class="row">
                        <div class='col-sm-6'>
                            <div class="form-group">
                                <div class='input-group date' id='datetimepicker1'>
                                    <input type='text' class="form-control" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>

                    </div>

                </td>

                <td style="width: 50%;">
                    <span>To Date:</span>
                    <div class="row">
                        <div class='col-sm-6'>
                            <div class="form-group">
                                <div class='input-group date' id='datetimepicker2'>
                                    <input type='text' class="form-control" id="fromdate" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="width: 100%;">
                <td style="width: 50%;">
                    <span>Outlet :</span>
                    <select id="ddOutlet" class="form-control bootstrap-select  show-tick">
                    </select>
                </td>
                <td style="width: 50%;">
                    <div class="form-check mb-2 mr-sm-2 mb-sm-0">
                        <label class="form-check-label">
                            <input class="form-check-input" id="exactTimeCheckbox" type="checkbox">
                            Exact Time
                        </label>
                    </div>
                    <div class="form-check mb-2 mr-sm-2 mb-sm-0">
                        <label class="form-check-label">
                            <input class="form-check-input" id="currentWordPeriodCheckBox" type="checkbox">
                            Current Work Period
                        </label>
                    </div>
                </td>
            </tr>
            <tr style="width: 100%;">
                <td>
                    <span>Department:</span>
                    <select id="ddDepartments" class="form-control bootstrap-select show-tick">
                        <option value="0" selected>All</option>
                    </select>
                </td>
                <td style="padding-left: 8%;">
                    <button type="button" class="btn btn-primary btn-lg" onclick="search()">Search</button>
                </td>
            </tr>
        </table>
    </div>
    <div class="row">
        <div class="col-lg-6 col-md-6 col-md-offset-3">
            <div class="panel-heading">
                <h2><i class="fa red"></i><strong id="workPeriodHeader">Work Period Considered from 05 Nov 2017 12:35 PM to 05 Nov 2017 05:19 PM</strong></h2>
            </div>
            <div class="panel panel-default">
                <div class="panel-body">
                    <table style="width: 100%" id="movies" class="metrotable">
                        <thead style="width: 100%">
                            <tr style="width: 100%">
                                <td style="width: 20%; text-align: left;">TimeRange </td>
                                <td style="width: 20%; text-align: right;">Sales </td>
                                <td style="width: 20%; text-align: right;">GrandTotal</td>
                                <td style="width: 20%; text-align: right;">NumberOfTickets</td>
                                <td style="width: 20%; text-align: right;">NumberOfGuests</td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>

                </div>
            </div>
    </div>
    </div>
    <script id="template" type="text/x-kendo-template">
            <tr style="width:100%;">
                <td style="width:20%;text-align:left;">#= TimeRange #</td>
                <td style="width:20%;text-align:right;">#= Sales #</td>
                <td style="width:20%;text-align:right;">#= GrandTotal #</td>
                <td style="width:20%;text-align:right;">#= NumberOfTickets #</td>
                <td style="width:20%;text-align:right;">#= NumberOfGuests #</td>
            </tr>
    </script>
    <div class="row">
        <div class="col-lg-6 col-md-6">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div>
                        <div class="demo-section k-content wide">
                            <div id="saleschart" runat="server">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-md-6">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="demo-section k-content wide">
                        <div id="grandtotalchart" runat="server">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-lg-6 col-md-6">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="demo-section k-content wide">
                        <div id="ticketcountchart" runat="server">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-md-6">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="demo-section k-content wide">
                        <div id="guestcount" runat="server">
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <script>
        $(function () {
            $('#datetimepicker2').datetimepicker({
                date: new Date()
            });
            $('#datetimepicker1').datetimepicker({
                date: new Date()
            });
        });
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var data = JSON.parse(JSON.parse(this.responseText));
                var sel = document.getElementById('ddOutlet');
                for (var i = 0; i < data.length; i++) {
                    var opt = document.createElement('option');
                    opt.innerHTML = data[i].Name;
                    opt.value = data[i].id;
                    sel.appendChild(opt);
                }
            }
        };
        xhttp.open("GET", "../api/timewisechart/getAllOutlets", false);
        xhttp.send();

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var data = JSON.parse(JSON.parse(this.responseText));
                var sel = document.getElementById('ddDepartments');
                for (var i = 0; i < data.length; i++) {
                    var opt = document.createElement('option');
                    opt.innerHTML = data[i].Name;
                    opt.value = data[i].id;
                    sel.appendChild(opt);
                }
            }
        };
        xhttp.open("GET", "../api/timewisechart/getAllDepartments", false);
        xhttp.send();
    </script>
    <script>

          var obj;
          function createChart() {
              $.getJSON("../api/timewisechart/dailyNetSales?outletid="+OutletValue+"&departmentvalue="+DepartmentValue+"&fromdate="+fromDate+"&todate="+toDate,
                  function (data) {
                      obj = jQuery.parseJSON(data);
                      console.log(data);
                      var template = kendo.template($("#template").html());
                      var dataSource = new kendo.data.DataSource({
                          data: obj,
                          change: function () { // subscribe to the CHANGE event of the data source
                              $("#movies tbody").html(kendo.render(template, this.view())); // populate the table
                          }
                      });

                      // read data from the "movies" array
                      dataSource.read();
                      var categories = new Array();
                      var sales = new Array();
                      var grandtotal = new Array();
                      var ticketcount = new Array();
                      var guestcount = new Array();
                      for (var i = 0; i < obj.length; i++) {
                          categories.push(obj[i].TimeRange);
                          sales.push(obj[i].Sales);
                          grandtotal.push(obj[i].GrandTotal);
                          ticketcount.push(obj[i].NumberOfTickets);
                          guestcount.push(obj[i].NumberOfGuests);
                      }


                      $("#saleschart").kendoChart({
                          title: {
                              text: "Time VS Sales"
                          },
                          legend: {
                              position: "top"
                          },
                          seriesDefaults: {
                              type: "column"
                          },
                          series: [{
                              name: "Sales",
                              data: sales,
                              color: function (point) {
                                  if (point.value < 20000) {
                                      return "red";
                                  }
                                  else return "blue"

                                  // use the default series theme color
                              }
                          }],
                          valueAxis: {
                              labels: {
                                  format: "{0}৳"
                              },
                              line: {
                                  visible: false
                              },
                              axisCrossingValue: 0
                          },
                          categoryAxis: {
                              categories: categories,
                              line: {
                                  visible: false

                              },
                              labels: {
                                  rotation: -90,
                                  padding: { top: 10 }
                              }
                          },
                          tooltip: {
                              visible: true,
                              format: "{0}",
                              template: "#= series.name #: #= value #"
                          }
                      });

                      $("#grandtotalchart").kendoChart({
                          title: {
                              text: "Time VS Grand Total"
                          },
                          legend: {
                              position: "top"
                          },
                          seriesDefaults: {
                              type: "column"
                          },
                          series: [{
                              name: "Grand Total",
                              data: grandtotal,
                              color: function (point) {
                                  if (point.value < 20000) {
                                      return "red";
                                  }
                                  else return "blue"

                                  // use the default series theme color
                              }
                          }],
                          valueAxis: {
                              labels: {
                                  format: "{0}৳"
                              },
                              line: {
                                  visible: false
                              },
                              axisCrossingValue: 0
                          },
                          categoryAxis: {
                              categories: categories,
                              line: {
                                  visible: false

                              },
                              labels: {
                                  rotation: -90,
                                  padding: { top: 10 }
                              }
                          },
                          tooltip: {
                              visible: true,
                              format: "{0}%",
                              template: "#= series.name #: #= value #"
                          }
                      });

                      $("#ticketcountchart").kendoChart({
                          title: {
                              text: "Time VS Number Of Tickets"
                          },
                          legend: {
                              position: "top"
                          },
                          seriesDefaults: {
                              type: "column"
                          },
                          series: [{
                              name: "Number Of Tickets",
                              data: ticketcount,
                              color: function (point) {
                                  if (point.value < 20000) {
                                      return "red";
                                  }
                                  else return "blue"

                                  // use the default series theme color
                              }
                          }],
                          valueAxis: {
                              labels: {
                                  format: "{0}৳"
                              },
                              line: {
                                  visible: false
                              },
                              axisCrossingValue: 0
                          },
                          categoryAxis: {
                              categories: categories,
                              line: {
                                  visible: false

                              },
                              labels: {
                                  rotation: -90,
                                  padding: { top: 10 }
                              }
                          },
                          tooltip: {
                              visible: true,
                              format: "{0}%",
                              template: "#= series.name #: #= value #"
                          }
                      });


                      $("#guestcount").kendoChart({
                          title: {
                              text: "Time VS Number Of Tickets"
                          },
                          legend: {
                              position: "top"
                          },
                          seriesDefaults: {
                              type: "column"
                          },
                          series: [{
                              name: "Guest Count",
                              data: guestcount,
                              color: function (point) {
                                  if (point.value < 20000) {
                                      return "red";
                                  }
                                  else return "blue"

                                  // use the default series theme color
                              }
                          }],
                          valueAxis: {
                              labels: {
                                  format: "{0}৳"
                              },
                              line: {
                                  visible: false
                              },
                              axisCrossingValue: 0
                          },
                          categoryAxis: {
                              categories: categories,
                              line: {
                                  visible: false

                              },
                              labels: {
                                  rotation: -90,
                                  padding: { top: 10 }
                              }
                          },
                          tooltip: {
                              visible: true,
                              format: "{0}%",
                              template: "#= series.name #: #= value #"
                          }
                      });
                  });
          }
        
          $(document).bind("kendo:skinChange", createChart);
          $(window).resize(function () {
              $("#grandtotalchart").data("kendoChart").refresh();
              $("#saleschart").data("kendoChart").refresh();
              $("#ticketcountchart").data("kendoChart").refresh();
              $("#guestcount").data("kendoChart").refresh();
          });

    </script>
    <script>
          function search() {
              document.getElementById("currentWordPeriodCheckBox").checked = false;
              var isExactTimeChecked = ($('input[id="exactTimeCheckbox"]:checked').length > 0);
              console.log(isExactTimeChecked);
              if (isExactTimeChecked) {
                  fromDate = $("#datetimepicker1").find("input").val();
                  toDate = $("#datetimepicker2").find("input").val();
                  var Outletelement = document.getElementById("ddOutlet");
                  OutletValue = Outletelement.options[Outletelement.selectedIndex].value;
                  var Departmentelement = document.getElementById("ddDepartments");
                  DepartmentValue = Departmentelement.options[Departmentelement.selectedIndex].value;
                  document.getElementById("workPeriodHeader").innerText = "Work Period Considered from " + fromDate + " to " + toDate;
                  createChart();
              }
              else {
                  var xhttp = new XMLHttpRequest();
                  xhttp.onreadystatechange = function () {
                      if (this.readyState == 4 && this.status == 200) {
                          var data = JSON.parse(JSON.parse(this.responseText));
                          console.log(data.StartAndEndDate[0]);
                          fromDate = data.StartAndEndDate[0].StartDate;
                          toDate = data.StartAndEndDate[0].EndDate;
                          var Outletelement = document.getElementById("ddOutlet");
                          OutletValue = Outletelement.options[Outletelement.selectedIndex].value;
                          var Departmentelement = document.getElementById("ddDepartments");
                          DepartmentValue = Departmentelement.options[Departmentelement.selectedIndex].value;
                        
                          document.getElementById("workPeriodHeader").innerText = "Work Period Considered from " + fromDate + " to " + toDate;
                          createChart();
                      }
                  };
                  xhttp.open("GET", "../api/timewisechart/GetWorkPeriodStartEnd?FromDate=" + $("#datetimepicker1").find("input").val() + "&ToDate=" + $("#datetimepicker2").find("input").val() + "&isForCurrentWorkPeriod=" + document.getElementById("currentWordPeriodCheckBox").checked, false);
                  xhttp.send();
              }
          }
         
    </script>
    <script>
          var fromDate = "";
          var toDate = "";
          var OutletValue = 0;
          var DepartmentValue = 0;
          $(document).ready(function () {
              InitializeData();
          });
          function InitializeData() {
              document.getElementById("currentWordPeriodCheckBox").checked = true;
              var xhttp = new XMLHttpRequest();
              xhttp.onreadystatechange = function () {
                  if (this.readyState == 4 && this.status == 200) {
                      var data = JSON.parse(JSON.parse(this.responseText));
                      console.log(data.StartAndEndDate[0]);
                      fromDate = data.StartAndEndDate[0].StartDate;
                      toDate = data.StartAndEndDate[0].EndDate;
                      var Outletelement = document.getElementById("ddOutlet");
                      OutletValue = Outletelement.options[Outletelement.selectedIndex].value;
                      var Departmentelement = document.getElementById("ddDepartments");
                      DepartmentValue = Departmentelement.options[Departmentelement.selectedIndex].value;
                      console.log(DepartmentValue);
                      document.getElementById("workPeriodHeader").innerText = "Work Period Considered from "+fromDate+" to "+toDate;
                      createChart();
                  }
              };
              xhttp.open("GET", "../api/timewisechart/GetWorkPeriodStartEnd?FromDate=" + $("#datetimepicker1").find("input").val() + "&ToDate=" + $("#datetimepicker2").find("input").val() + "&isForCurrentWorkPeriod=" + document.getElementById("currentWordPeriodCheckBox").checked, false);
              xhttp.send();

          }
          
    </script>
    <script type="text/javascript">

          $(document).ready(function () {
              $("#movies").kendoGrid({
                  sortable: true
              });
          });

          
    </script>


</asp:Content>