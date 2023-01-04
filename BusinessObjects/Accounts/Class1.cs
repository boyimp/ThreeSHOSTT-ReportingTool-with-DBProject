using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects.Accounts
{
    <telerik:RadGrid ID="grdTickets" runat="server"
	AutoGenerateColumns="false" 
                            AllowFilteringByColumn="true" AllowSorting="true" AllowPaging="true">
    <MasterTableView>                            
        <Columns>     
            <telerik:GridBoundColumn DataField="TicketNumber" HeaderText="Ticket Number" />
            <telerik:GridBoundColumn DataField="Date" DataFormatString="{0:d}" HeaderText="Date" DataType="System.DateTime" />
            <telerik:GridBoundColumn DataField="Note" HeaderText="Note" />
            <telerik:GridNumericColumn DataField="TotalAmount" DataType="System.Decimal" HeaderText="Total Amount" />
        </Columns>                                                               
    </MasterTableView>   
    <PagerStyle AlwaysVisible="true" />
    <%--<ClientSettings>
                                <ClientEvents OnGridCreated="GridCreated" />
                                <DataBinding Location="WebService.asmx" SelectMethod="GetDataAndCount" />
                            </ClientSettings>--%>   
</telerik:RadGrid>
}
