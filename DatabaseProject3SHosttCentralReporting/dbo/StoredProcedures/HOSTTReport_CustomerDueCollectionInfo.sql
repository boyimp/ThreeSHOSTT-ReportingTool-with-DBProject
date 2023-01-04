CREATE PROCEDURE [dbo].[HOSTTReport_CustomerDueCollectionInfo](
	@StartDate DATETIME, @EndDate DATETIME, @UserPermittedDepartmentCommaSeparatedIds varchar(max)
	)
	AS 
BEGIN 
	select att.Name Header1 
	,SUM(Amount)  Value1
     from AccountTransactionDocuments atd
     , AccountTransactions a
     ,AccountTransactionTypes att 
     where atd.Id = a.AccountTransactionDocumentId 
     and a.TargetAccountTypeId =3 
     and (a.AccountTransactionTypeId=1 OR a.AccountTransactionTypeId=2) 
     and att.id = a.AccountTransactionTypeId
     and Date BETWEEN @StartDate AND @EndDate
     Group by att.id,att.Name
End

GO

