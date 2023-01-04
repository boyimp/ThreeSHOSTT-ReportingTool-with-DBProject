using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;

/// <summary>
/// Summary description for EntityManager
/// </summary>
public class EntityManager
{
    public static DataSet GetEntityType()
    {
        DataSet ds = new DataSet();
        try
        {
            string sql = string.Empty;
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            sql = string.Format("select id,EntityName Name FROM EntityTypes");

            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.Fill(ds);
            dbConn.Close();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }

        return ds;
    }

    public static DataSet GetEntityCustomField(int EntityType)
    {
        DataSet ds = new DataSet();
        try
        {
            string sql = string.Empty;
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            sql = string.Format(@"SELECT e.Id, e.Name EntityName, f.Name FieldName 
                                FROM EntityTypes e ,  entitycustomfields f 
                                where e.Id = f.EntityType_Id
                                and e.Id = {0} ", EntityType);

            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.Fill(ds);
            dbConn.Close();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }

        return ds;
    }
    public static DataSet GetEntities(int EntityType)
    {
        DataSet ds = new DataSet();
        try
        {
            string sql = string.Empty;
            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            sql = string.Format(@"Select Id, Name from 
                                  Entities where EntityTypeId= {0}", EntityType);

            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.Fill(ds);
            dbConn.Close();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }

        return ds;
    }
    public static DataSet GetCalculationTypeEntitiesWise(DateTime startDate, DateTime endDate, int CalculationType)
    {
        DataSet ds = new DataSet();
        try
        {
            string sql = string.Empty;
            string clause = string.Empty;
            if (CalculationType > 0)
            {
                clause = $" and ct.Id = {CalculationType}";
            }

            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            sql = $@"
                            SELECT t.Id TicketId,ct.Id CalculationTypeId,
                            ct.Name CalculationName, 
                            CASE ct.CalculationMethod 
                            WHEN 0 THEN  'Rate From Ticket Amount'
                            WHEN 1 THEN 'Rate From Previous Template'
                            WHEN 2 THEN 'Fixed Amount'
                            WHEN 3 THEN 'Fixed Amount From Ticket Total'
                            WHEN 4 THEN 'Round Ticket Total'
                            END AS CalculationMethod, 
                            ct.Amount CalculationAmount,
                            t.SettledBy, t.TicketNumber, t.LastPaymentDate,
                            t.PlainSum-- Plain sum hobe na kon property hobe sheta onno report dekhe bolte hobe
                            ,  trans.Amount, isnull(te.EntityName, 'No Entity Selected')EntityName, te.EntityCustomData CustomData
                            From
                            Calculations c inner join Tickets t
                            on c.TicketId = t.Id
                            inner join CalculationTypes ct
                            on c.CalculationTypeId = ct.Id 
                            inner join accounttransactiondocuments d
                            on  t.TransactionDocument_Id = d.Id 
                            inner join AccountTransactions trans 
                            on ct.AccountTransactionType_Id = trans.AccountTransactionTypeId
                            left outer join 
                            TicketEntities te
                            on T.Id = te.Ticket_Id and te.EntityTypeId = 1
                            WHERE  
                            d.Id = trans.AccountTransactionDocumentId
                            and T.Date between '{startDate.ToString("dd MMM yyyy hh:mm tt")}' and '{endDate.ToString("dd MMM yyyy hh:mm tt")}'
                            {clause}";

            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.Fill(ds);
            dbConn.Close();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }

        return ds;
    }
    public static DataSet GetEntitiesCalculationTypeWise(DateTime startDate, DateTime endDate, int EntityTypeId ,int EntityId)
    {
        DataSet ds = new DataSet();
        try
        {
            string sql = string.Empty;
            string clause = string.Empty;
            if (EntityId > 0)
            {
                clause = string.Format(" and te.EntityId = {0}", EntityId);
            }

            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            dbConn.Open();

            sql = string.Format(@"
                                select EntityId, EntityName,Cast(EntityCustomData as varchar(max)) CustomData, count(*) VisitCount --, to fetch from customdate,
                                ,sum(PlainSum)TotalPurchase, sum(Amount) CalculationAmount from
                                (
	                                SELECT te.EntityName, te.EntityId, te.entitycustomdata, t.Id,
	                                ct.Name CalculationName, 
	                                CASE ct.CalculationMethod 
	                                WHEN 0 THEN  'Rate From Ticket Amount'
	                                WHEN 1 THEN 'Rate From Previous Template'
	                                WHEN 2 THEN 'Fixed Amount'
	                                WHEN 3 THEN 'Fixed Amount From Ticket Total'
	                                WHEN 4 THEN 'Round Ticket Total'
	                                END AS CalculationMethod, 
	                                ct.Amount CalculationAmount,
	                                t.SettledBy, t.TicketNumber, t.LastPaymentDate,
	                                t.PlainSum-- Plain sum hobe na kon property hobe sheta onno report dekhe bolte hobe
	                                ,  trans.Amount, c.AccountTransactionTypeId
	                                FROM 
	                                Calculations c, Tickets t, CalculationTypes ct, accounttransactiondocuments d, AccountTransactions trans, TicketEntities te
	                                WHERE c.TicketId = t.Id and c.CalculationTypeId = ct.Id
	                                AND t.TransactionDocument_Id = d.Id
	                                AND d.Id = trans.AccountTransactionDocumentId
	                                AND ct.AccountTransactionType_Id = trans.AccountTransactionTypeId
	                                AND te.Ticket_Id =  t.Id
                                    and T.Date between '{0}' and '{1}'
                                    and te.EntityTypeId = {2}
                                    {3}
                                )Q
                                group by EntityId, EntityName, Cast(EntityCustomData as varchar(max))", startDate.ToString("dd MMM yyyy hh:mm tt"), endDate.ToString("dd MMM yyyy hh:mm tt"),EntityTypeId ,clause);

            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.Fill(ds);
            dbConn.Close();
        }
        catch (SqlException ex)
        {
            throw new Exception(ex.Message);
        }

        return ds;
    }
    /*SELECT e.Id, e.Name EntityName, f.Name FieldName 
    FROM EntityTypes e ,  entitycustomfields f 
    where e.Id = f.EntityType_Id*/

    public static DataSet GetEntities(DateTime fromDate, DateTime toDate, DateTime fromCreationDate, DateTime toCreationDate, int EntityType, string CompanyCode, bool CreationDateChecked)
    {
        DataSet ds = new DataSet();
        string searchStr = string.Empty;
        try
        {
            if (EntityType != 0)
            {
                searchStr = string.Format(@" and EntityTypeId ={0}", EntityType);
            }
            if (CreationDateChecked)
            {
                searchStr += string.Format(@" and isnull(CreationDate, (select MIN(startdate) from WorkPeriods)) between '{0}' and '{1}'", fromCreationDate.ToString("dd MMM yyyy"), toCreationDate.ToString("dd MMM yyyy"));
            }

            string dbConnString = DBUtility.GetConnectionString();
            SqlConnection dbConn = new SqlConnection(dbConnString);
            
            dbConn.Open();
            string sql = string.Empty;
            #region Old
            //if (!CreationDateChecked)
            //{
            //    sql = string.Format(@"SELECT '{3}'+right('0000000' +cast(Id as nvarchar(10))  , 7) ID, Name, LastUpdateTime,
            //                            'Balance'= Isnull
            //                               (
            //                                (
            //                                 SELECT Balance FROM 
            //                                 (
            //                                  SELECT A.Id, T.Name AccountType,  A.Name AccountName,
            //                                  sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
            //                                  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
            //                                  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
            //                                  cast(V.Date as date) BETWEEN cast('{1}' as date) AND cast('{2}' as date)
            //                                  GROUP BY A.id, V.accounttypeid, T.Name, V.accountid, A.Name
            //                                 ) Q WHERE Q.ID = Entities.AccountId
            //                                  ),0
            //                                 ),
            //                             'TotalTicketAmount'
            //                                     =  isnull((
            //                                            SELECT sum(totalamount) 
            //                                            FROM tickets t, TicketEntities te 
            //                                            WHERE t.Id = te.Ticket_Id
            //                                            AND te.EntityId =  Entities.Id and
            //                                            cast(t.Date as date) BETWEEN cast('{1}' as date) AND cast('{2}' as date)
            //                                        ), 0),
            //                            Entities.CustomData 
            //                            FROM Entities WHERE Entities.Id =  Entities.Id 
            //                            {0}", searchStr, fromDate, toDate, CompanyCode);
            //}
            //else
            //{
            //    sql = string.Format(@"SELECT '{3}'+right('0000000' +cast(Id as nvarchar(10))  , 7) ID, 
            //                          isnull(entities.CreationDate, (SELECT min(startdate) FROM WorkPeriods)) CreationDate,
            //                          Name,  LastUpdateTime,
            //                          'Balance'= Isnull
            //                               (
            //                                (
            //                                 SELECT Balance FROM 
            //                                 (
            //                                  SELECT A.Id, T.Name AccountType,  A.Name AccountName,
            //                                  sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
            //                                  FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
            //                                  V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
            //                                  cast(V.Date as date) BETWEEN cast('{1}' as date) AND cast('{2}' as date)
            //                                  GROUP BY A.id, V.accounttypeid, T.Name, V.accountid, A.Name
            //                                 ) Q WHERE Q.ID = Entities.AccountId
            //                                  ),0
            //                                 ),
            //                            'TotalTicketAmount'
            //                                     =  isnull((
            //                                            SELECT sum(totalamount) 
            //                                            FROM tickets t, TicketEntities te 
            //                                            WHERE t.Id = te.Ticket_Id
            //                                            AND te.EntityId =  Entities.Id and
            //                                            cast(t.Date as date) BETWEEN cast('{1}' as date) AND cast('{2}' as date)
            //                                        ), 0),
            //                            Entities.CustomData 
            //                            FROM Entities WHERE Entities.Id =  Entities.Id
            //                            {0}", searchStr, fromDate, toDate, CompanyCode);
            //} 
            #endregion
            if (!CreationDateChecked)
            {
                sql = string.Format(@"Select *, CASE 
                                                            WHEN Quantity = 0  
                                                            THEN 0 
                                                            ELSE GrossTotal/Quantity
                                                        END as AverageGrossPrice, 
                                    CASE 
                                                            WHEN Quantity = 0  
                                                            THEN 0 
                                                            ELSE NetTicketTotal/Quantity
                                                        END as AverageNetPrice
					
					                                    from
                                    (
                                    SELECT '{3}'+right('0000000' +cast(Id as nvarchar(10))  , 7) ID, Name, LastUpdateTime,
                                    'Quantity' 
	                                    = isnull((
                                                    SELECT sum(TT.Quantity) from
				                                    (
					                                    select  
					                                    CASE 
                                                             WHEN o.CalculatePrice = 1  and o.DecreaseInventory = 1  
                                                            THEN o.Quantity 
                                                            ELSE 0
                                                        END as Quantity
					                                    FROM tickets t, TicketEntities te , Orders o
					                                    WHERE t.Id = te.Ticket_Id
					                                    And O.ticketid = t.id
					                                    AND te.EntityId =  Entities.Id and
					                                    t.Date BETWEEN '{1}' AND '{2}'
				                                    )TT
                                                ), 0),
			                                    'GrossTotal' 
	                                    = isnull((
                                                    SELECT sum(TTT.Price) from
				                                    (
					                                    select  
					                                    CASE 
                                                             WHEN o.CalculatePrice = 1  and o.DecreaseInventory = 1  
                                                            THEN o.Total 
                                                            ELSE 0
                                                        END as Price
					                                    FROM tickets t, TicketEntities te , Orders o
					                                    WHERE t.Id = te.Ticket_Id
					                                    And O.ticketid = t.id
					                                    AND te.EntityId =  Entities.Id and
					                                    t.Date BETWEEN '{1}' AND '{2}'
				                                    )TTT
                                                ), 0),
                                        'NetTicketTotal'
                                                =  isnull((
                                                    SELECT sum(totalamount) 
                                                    FROM tickets t, TicketEntities te 
                                                    WHERE t.Id = te.Ticket_Id
                                                    AND te.EntityId =  Entities.Id and
                                                    t.Date BETWEEN '{1}' AND '{2}'
                                                ), 0),
	
	
                                    'Balance'= Isnull
			                                    (
				                                    (
					                                    SELECT Balance FROM 
					                                    (
						                                    SELECT A.Id, T.Name AccountType,  A.Name AccountName,
						                                    sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
						                                    FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
						                                    V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
						                                    V.Date BETWEEN '{1}' AND '{2}'
						                                    GROUP BY A.id, V.accounttypeid, T.Name, V.accountid, A.Name
					                                    ) Q WHERE Q.ID = Entities.AccountId
			                                        ),0
		                                        ),
                                    Entities.CustomData 
                                    FROM Entities WHERE Entities.Id =  Entities.Id 
                                    {0}
                                    )Q
                                        ", searchStr, fromDate.ToString("dd MMM yyyy hh:mm tt"), toDate.ToString("dd MMM yyyy hh:mm tt"), CompanyCode);
            }
            else
            {
                sql = string.Format(@"Select *, CASE 
                                                            WHEN Quantity = 0  
                                                            THEN 0 
                                                            ELSE GrossTotal/Quantity
                                                        END as AverageGrossPrice, 
                                    CASE 
                                                            WHEN Quantity = 0  
                                                            THEN 0 
                                                            ELSE NetTicketTotal/Quantity
                                                        END as AverageNetPrice
					
					                                    from
                                    (
                                    SELECT '{3}'+right('0000000' +cast(Id as nvarchar(10))  , 7) ID,
                                    isnull(entities.CreationDate, (SELECT min(startdate) FROM WorkPeriods)) CreationDate, 
                                    Name, LastUpdateTime,
                                    'Quantity' 
	                                    = isnull((
                                                    SELECT sum(TT.Quantity) from
				                                    (
					                                    select  
					                                    CASE 
                                                             WHEN o.CalculatePrice = 1  and o.DecreaseInventory = 1  
                                                            THEN o.Quantity 
                                                            ELSE 0
                                                        END as Quantity
					                                    FROM tickets t, TicketEntities te , Orders o
					                                    WHERE t.Id = te.Ticket_Id
					                                    And O.ticketid = t.id
					                                    AND te.EntityId =  Entities.Id and
					                                    t.Date BETWEEN '{1}' AND '{2}'
				                                    )TT
                                                ), 0),
			                                    'GrossTotal' 
	                                    = isnull((
                                                    SELECT sum(TTT.Price) from
				                                    (
					                                    select  
					                                    CASE 
                                                             WHEN o.CalculatePrice = 1  and o.DecreaseInventory = 1  
                                                            THEN o.Total 
                                                            ELSE 0
                                                        END as Price
					                                    FROM tickets t, TicketEntities te , Orders o
					                                    WHERE t.Id = te.Ticket_Id
					                                    And O.ticketid = t.id
					                                    AND te.EntityId =  Entities.Id and
					                                    t.Date BETWEEN '{1}' AND '{2}'
				                                    )TTT
                                                ), 0),
                                        'NetTicketTotal'
                                                =  isnull((
                                                    SELECT sum(totalamount) 
                                                    FROM tickets t, TicketEntities te 
                                                    WHERE t.Id = te.Ticket_Id
                                                    AND te.EntityId =  Entities.Id and
                                                    t.Date BETWEEN '{1}' AND '{2}'
                                                ), 0),
	
	
                                    'Balance'= Isnull
			                                    (
				                                    (
					                                    SELECT Balance FROM 
					                                    (
						                                    SELECT A.Id, T.Name AccountType,  A.Name AccountName,
						                                    sum(Debit) Debit, sum(Credit)Credit, sum(Debit)-sum(Credit) Balance
						                                    FROM AccountTransactionValues V, Accounts A, AccountTypes T WHERE
						                                    V.AccountTypeId = T.Id AND A.Id = V.AccountId AND 
						                                    V.Date BETWEEN '{1}' AND '{2}'
						                                    GROUP BY A.id, V.accounttypeid, T.Name, V.accountid, A.Name
					                                    ) Q WHERE Q.ID = Entities.AccountId
			                                        ),0
		                                        ),
                                    Entities.CustomData 
                                    FROM Entities WHERE Entities.Id =  Entities.Id 
                                    {0}
                                    )Q", searchStr, fromDate.ToString("dd MMM yyyy hh:mm tt"), toDate.ToString("dd MMM yyyy hh:mm tt"), CompanyCode);
            }
            
            SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);
            da.SelectCommand.CommandTimeout = dbConn.ConnectionTimeout;
            da.Fill(ds, "Entities");
            dbConn.Close();


        }
        catch (Exception ex)
        { throw new Exception(ex.Message); }

        return ds;
    }
}