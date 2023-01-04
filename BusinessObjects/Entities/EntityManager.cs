using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ThreeS.Domain.Models.Entities;
using ThreeS.Domain.Models.Accounts;

namespace BusinessObjects.EntitiesManager
{
    public class EntityManager
    {
        public static DataSet GetEntities(DateTime fromDate, DateTime toDate, int EntityType, bool isOnlyOpenEntity)
        {
            DataSet ds = new DataSet();
            string searchStr = string.Empty;
            try
            {
                if (EntityType != 0)
                {
                    searchStr = string.Format(@"tktType.Id={0} AND ", EntityType);
                }
                if (isOnlyOpenEntity)
                {
                    searchStr += @" tkt.IsClosed=0 AND ";
                }
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"

                    declare @stmt nvarchar(max)
                    select @stmt =
                          isnull(@stmt + ', ', '') +
                          'max(case when Name = ''' + Name + ''' then EntityName else null end) as ' + quotename(name)
                    from EntityTypes

                    select @stmt = 'select
                            t.id,t.EntityNumber,' + @stmt + ',t.Date, LTRIM(RIGHT(CONVERT(VARCHAR(20), t.Date, 100), 7)) as Opening,
                            LTRIM(RIGHT(CONVERT(VARCHAR(20), t.LastUpdateTime, 100), 7)) as Closing, t.TotalAmount, t.Note, t.Isclosed from (
                    select
                        tkt.Id,tkt.EntityNumber,tkt.Date,tkt.TotalAmount,CAST(tkt.Note AS NVARCHAR(100)) Note,tkt.IsClosed, tkt.LastUpdateTime, EntType.Name,tktEnt.EntityName    
                        from Entities as tkt 
                        inner join  EntityEntities as tktEnt on tktEnt.Entity_Id=tkt.Id
                        inner join EntityTypes as EntType on EntType.Id = tktEnt.EntityTypeId 
                        inner join EntityTypes as tktType on tktType.Id = tkt.EntityTypeId 
                        where {0} cast(tkt.Date as Date) between cast(''{1}'' as Date) and cast(''{2}'' as Date))t 
	                    group by t.id,t.EntityNumber,t.Date,t.LastUpdateTime,t.TotalAmount, t.Note,t.Isclosed order by t.Isclosed'

	                    exec dbo.sp_executesql
                        @stmt 

                    ", searchStr, fromDate, toDate);

                SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);

                da.Fill(ds, "Entities");
                dbConn.Close();


            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return ds;
        }

        public static DataTable GetEntityEntities(int EntityId)
        {
            DataTable ds = new DataTable("EntityEntities");
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();
                string sql = string.Format(@"select entType.EntityName as EntityType,tktEnt.EntityName from Entityentities tktEnt
                            inner join EntityTypes entType
                            on entType.Id=tktEnt.EntityTypeId
                            where tktEnt.Entity_Id={0}", EntityId);

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

        public static DataSet GetEntityType()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                sql = string.Format("select id,Name FROM EntityTypes");

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

        public static Entity GetEntity(int EntityIdid)
        {
            Entity Entity = null;
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT  * FROM Entities where id={0}", EntityIdid);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                SqlDataReader reader = dbCommand.ExecuteReader();
                while (reader.Read())
                {
                    Entity = new Entity();
                    Entity.Id = (int)reader["Id"];
                    Entity.LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]);
                    Entity.EntityTypeId = (int)(reader["EntityTypeId"]);
                    Entity.CustomData = (reader["CustomData"] == DBNull.Value) ? String.Empty : (string)reader["CustomData"];
                    Entity.WarehouseId = (int)reader["WarehouseId"];
                    Entity.SearchString = (reader["SearchString"] == DBNull.Value) ? String.Empty : (string)reader["SearchString"];
                    Entity.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
                }
                reader.Close();
                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return Entity;
        }


        private static AccountTransactionDocument GetTransactionDocument(SqlConnection dbConn, int TransactionDocument_Id)
        {
            string sqlString = string.Format(@"select * from AccountTransactionDocuments where id={0}", TransactionDocument_Id);
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            SqlDataReader reader = dbCommand.ExecuteReader();

            AccountTransactionDocument obj = null;
            while (reader.Read())
            {
                obj = new AccountTransactionDocument();
                obj.Id = (int)reader["Id"];
                obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
                obj.Date = (DateTime)reader["Date"];
            }
            reader.Close();
            obj.AccountTransactions = GetAccountTransactions(dbConn, obj.Id);

            return obj;
        }

        private static List<AccountTransaction> GetAccountTransactions(SqlConnection dbConn, int id)
        {
            string sqlString = string.Format(@"select * from AccountTransactions where AccountTransactionDocumentId={0}", id);
            SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            SqlDataReader reader = dbCommand.ExecuteReader();

            List<AccountTransaction> objects = new List<AccountTransaction>();
            AccountTransaction obj = null;
            while (reader.Read())
            {
                obj = new AccountTransaction();
                obj.Id = (int)reader["Id"];
                obj.AccountTransactionDocumentId = (int)(reader["AccountTransactionDocumentId"]);
                obj.Amount = (decimal)(reader["Amount"]);
                obj.ExchangeRate = (decimal)(reader["ExchangeRate"]);
                obj.AccountTransactionTypeId = (int)(reader["AccountTransactionTypeId"]);
                obj.SourceAccountTypeId = (int)(reader["SourceAccountTypeId"]);
                obj.TargetAccountTypeId = (int)(reader["TargetAccountTypeId"]);
                obj.IsReversed = (bool)reader["IsReversed"];
                obj.Reversable = (bool)reader["Reversable"];
                obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
                
                objects.Add(obj);

            }
            reader.Close();

            return GetAccountTransactionValue(dbConn, objects);
        }

        private static List<AccountTransaction> GetAccountTransactionValue(SqlConnection dbConn, List<AccountTransaction> acTrns)
        {
            foreach (AccountTransaction acTrn in acTrns)
            {
                string sqlString = string.Format(@"select * from AccountTransactionValues where AccountTransactionId={0}", acTrn.Id);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                SqlDataReader reader = dbCommand.ExecuteReader();

                List<AccountTransactionValue> objects = new List<AccountTransactionValue>();
                AccountTransactionValue obj = null;
                while (reader.Read())
                {
                    obj = new AccountTransactionValue();
                    obj.Id = (int)reader["Id"];
                    obj.AccountTransactionId = (int)(reader["AccountTransactionId"]);
                    obj.AccountTransactionDocumentId = (int)(reader["AccountTransactionDocumentId"]);
                    obj.AccountTypeId = (int)reader["AccountTypeId"];
                    obj.AccountId = (int)(reader["AccountId"]);
                    obj.Date = (DateTime)(reader["Date"]);
                    obj.Debit = (decimal)(reader["Debit"]);
                    obj.Credit = (decimal)(reader["Credit"]);
                    obj.Exchange = (decimal)(reader["Exchange"]);
                    obj.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
                    objects.Add(obj);
                }
                reader.Close();
                acTrn.AccountTransactionValues = objects;
            }

            return acTrns;
        }


        public static List<Entity> GetEntities(DateTime fromDate, DateTime toDate)
        {
            List<Entity> Entities = new List<Entity>();
            //try
            //{
            //    string dbConnString = DBUtility.GetConnectionString();
            //    SqlConnection dbConn = new SqlConnection(dbConnString);
            //    dbConn.Open();

            //    string sqlString = string.Format(@"SELECT  * FROM Entities where cast(Date as Date) between cast('{0}' as Date) and cast('{1}' as Date)", fromDate, toDate);
            //    SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
            //    SqlDataReader reader = dbCommand.ExecuteReader();

                
            //    Entity Entity = null;

            //    while (reader.Read())
            //    {
            //        Entity = new Entity();
            //        Entity.Id = (int)reader["Id"];
            //        Entity.LastUpdateTime = Convert.ToDateTime(reader["LastUpdateTime"]);
            //        Entity.EntityNumber = (string)(reader["EntityNumber"]);
            //        Entity.Date = Convert.ToDateTime(reader["Date"]);
            //        Entity.LastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
            //        Entity.LastPaymentDate = Convert.ToDateTime(reader["LastPaymentDate"]);
            //        Entity.IsClosed = (bool)(reader["IsClosed"]);
            //        Entity.IsLocked = (bool)(reader["IsLocked"]);
            //        Entity.RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]);
            //        Entity.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
            //        Entity.DepartmentId = (int)(reader["DepartmentId"]);
            //        Entity.EntityTypeId = (int)(reader["EntityTypeId"]);
            //        Entity.Note = (reader["Note"] == DBNull.Value) ? String.Empty : (string)reader["Note"];
            //        Entity.EntityTags = (reader["EntityTags"] == DBNull.Value) ? String.Empty : (string)reader["EntityTags"];
            //        Entity.Entitiestates = (reader["Entitiestates"] == DBNull.Value) ? String.Empty : (string)reader["Entitiestates"];
            //        Entity.ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
            //        Entity.TaxIncluded = (bool)(reader["TaxIncluded"]);
            //        Entity.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)reader["Name"];
            //        Entity.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);

            //        Entities.Add(Entity);
            //    }
            //    reader.Close();
            //    foreach (Entity tkt in Entities)
            //    {
            //        tkt.Orders = GetOrders(dbConn, tkt.Id);
            //        tkt.Calculations = GetCalculations(dbConn, tkt.Id);
            //        tkt.Payments = GetPayments(dbConn, tkt.Id);
            //    }

            //    dbConn.Close();
            //}
            //catch (Exception ex)
            //{ throw new Exception(ex.Message); }

            return Entities;
        }
        public static List<EntityCustomField> GetEntityCustomFields(int EntityTypeId)
        {
            List<EntityCustomField> EntityCustomFields = new List<EntityCustomField>();
            try
            {
                string dbConnString = DBUtility.GetConnectionString();
                SqlConnection dbConn = new SqlConnection(dbConnString);
                dbConn.Open();

                string sqlString = string.Format(@"SELECT * FROM [EntityCustomFields] WHERE EntityType_Id = {0}", EntityTypeId);
                SqlCommand dbCommand = new SqlCommand(sqlString, dbConn);
                SqlDataReader reader = dbCommand.ExecuteReader();


                EntityCustomField EntityCustomField = null;

                while (reader.Read())
                {
                    EntityCustomField = new EntityCustomField();
                    EntityCustomField.Id = (int)reader["Id"];
                    EntityCustomField.FieldType = (int)reader["FieldType"];
                    EntityCustomField.EditingFormat = (reader["EditingFormat"] == DBNull.Value) ? String.Empty : (string)(reader["EditingFormat"]);
                    EntityCustomField.ValueSource = (reader["ValueSource"] == DBNull.Value) ? String.Empty : (string)(reader["ValueSource"]);
                    EntityCustomField.Name = (reader["Name"] == DBNull.Value) ? String.Empty : (string)(reader["Name"]);
                    EntityCustomFields.Add(EntityCustomField);
                }
                reader.Close();
                dbConn.Close();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return EntityCustomFields;
        }

    }
}
