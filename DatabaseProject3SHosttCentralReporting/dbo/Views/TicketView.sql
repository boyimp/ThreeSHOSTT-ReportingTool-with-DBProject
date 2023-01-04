
CREATE view [dbo].[TicketView] AS
   
	SELECT 
	SyncOutletID, DepartmentId ,DepartmentName, LastUpdateTime, TicketDate, LastOrderDate, LastPaymentDate, ticketid, ticketnumber, isnull(Payment, PaymentAccountType) PaymentAccountType, 
	Payment, AccountTypeName, SortOrder, accountname, Settledby,
	CASE 
	WHEN PaymentAccountType IS NOT NULL AND Payment IS NULL THEN PaymentAccountType
	ELSE accountname
	END UltimateAccount,   
	sum(amount) amount, max(PlainSum) PlainSum, max(TotalAmount)TotalAmount
	FROM 
	(
		SELECT 
		SyncOutletID,DepartmentId, DepartmentName,LastUpdateTime, TicketDate, LastOrderDate, LastPaymentDate, ticketid, ticketnumber, Payment, PaymentAccountType, 
		AccountTypeName, SortOrder, accountname,  SettledBy,
		CASE WHEN Payment IS NOT NULL AND Credit > 0 THEN amount*(-1)
		ELSE amount 
		END amount,
		PlainSum, TotalAmount FROM 
		(
			SELECT t.DepartmentId DepartmentId, d.Name DepartmentName, t.LastUpdateTime, t.[DATE] TicketDate, t.LastOrderDate, t.LastPaymentDate, PaymentAccountType, Payment,	
			t.Id TicketID, t.TicketNumber,t.SettledBy, AccountTypeName, A.SortOrder, Source, Target, AccountName, Amount, Credit, Debit, Exchange, PlainSum, TotalAmount, t.SyncOutletId
			FROM Tickets t LEFT OUTER JOIN 
			(
				SELECT d.Id TranDocId, aat.Name AccountTypeName, aat.SortOrder, a_s.Name Source, a_t.Name Target, ac.Name AccountName,
				'PaymentAccountType' = (SELECT max(Name) FROM PaymentTypes WHERE AccountTransactionType_Id = at.AccountTransactionTypeId),
				'Payment' = (SELECT max(Name) FROM PaymentTypes WHERE AccountTransactionType_Id = at.AccountTransactionTypeId AND Ac.Id = account_id),
				at.Amount, 
				v.Credit, v.Debit,
			    v.Exchange
				FROM 
				AccountTransactionDocuments d			
				LEFT OUTER JOIN AccountTransactions at
				on d.Id = at.AccountTransactionDocumentId
				LEFT OUTER JOIN AccountTransactionValues v
				on at.Id = v.AccountTransactionId
				LEFT OUTER JOIN AccountTypes a_s 
				on at.SourceAccountTypeId = a_s.Id
				LEFT OUTER JOIN AccountTypes a_t 
				on at.TargetAccountTypeId = a_t.Id
				LEFT OUTER JOIN Accounts ac 
				on v.AccountId = ac.Id
				LEFT OUTER JOIN AccountTypes aat
				on aat.Id = ac.AccountTypeId			
				WHERE ac.Name <> 'Receivables'
			)A
			on t.TransactionDocument_Id = A.TranDocId
			LEFT OUTER JOIN departments d
			ON t.departmentid = d.Id
		)Q
	)R
	GROUP BY 
	SyncOutletID,DepartmentId, DepartmentName, LastUpdateTime, TicketDate, LastOrderDate, 
	LastPaymentDate,ticketid, ticketnumber, Payment, 
	PaymentAccountType, AccountTypeName, SortOrder, accountname, settledby

GO

