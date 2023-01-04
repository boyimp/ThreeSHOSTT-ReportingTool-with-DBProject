CREATE view [dbo].[TicketItemView] AS
SELECT DepartmentID, DepartmentName, LastUpdateTime, TicketDate, LastOrderDate, LastPaymentDate,SyncOutletId, o.ticketid, ticketnumber, PaymentAccountType, 
Payment, AccountTypeName, SortOrder, accountname, 
CASE 
WHEN PaymentAccountType IS NOT NULL AND Payment IS NULL THEN PaymentAccountType
ELSE accountname
END UltimateAccount,
amount,
totalamount,PlainSum, ordercreateddatetime, GroupCode, menuitemname, portionname, OrderID, MenuItemID, OrderedQuantity, Price, Total, PlainTotal ,
CASE WHEN PlainSum = 0 THEN 0
ELSE Total/PlainSum*TotalAmount 
END OrderTotal, 
CASE WHEN PlainSum = 0 THEN 0
ELSE Total/PlainSum*RemainingAmount 
END RemainingAmount, 
(PlainTotal-Total) Gift,
CASE plainsum 
WHEN 0 THEN 0  
ELSE total/plainsum*amount  
END as DistinctAmount
FROM 
(
	SELECT 
	LastUpdateTime, TicketDate, LastOrderDate, LastPaymentDate,SyncOutletId, ticketid, ticketnumber, isnull(Payment, PaymentAccountType) PaymentAccountType, 
	Payment, AccountTypeName, SortOrder, accountname,
	CASE 
	WHEN PaymentAccountType IS NOT NULL AND Payment IS NULL THEN PaymentAccountType
	ELSE accountname
	END UltimateAccount,   
	sum(amount) amount, max(PlainSum) PlainSum, max(TotalAmount)TotalAmount, max(RemainingAmount)RemainingAmount
	FROM 
	(
		SELECT 
		LastUpdateTime, TicketDate, LastOrderDate, LastPaymentDate,SyncOutletId, ticketid, ticketnumber, RemainingAmount, Payment, PaymentAccountType, 
		AccountTypeName, SortOrder, accountname,   
		CASE WHEN Payment IS NOT NULL AND Credit > 0 THEN amount*(-1)
		ELSE amount 
		END amount,
		PlainSum, TotalAmount FROM 
		(
			SELECT  t.LastUpdateTime, t.[DATE] TicketDate, 
			t.LastOrderDate, t.LastPaymentDate, PaymentAccountType, Payment,t.SyncOutletId,	
			t.Id TicketID, t.TicketNumber, t.RemainingAmount, AccountTypeName, 
			A.SortOrder, Source, Target, AccountName, Amount, Credit, Debit, 
			Exchange, PlainSum, TotalAmount
			FROM Tickets t LEFT OUTER JOIN 
			(
				SELECT d.Id TranDocId, aat.Name AccountTypeName, aat.SortOrder, a_s.Name Source, a_t.Name Target, ac.Name AccountName,
				'PaymentAccountType' = (SELECT max(Name) FROM PaymentTypes WHERE AccountTransactionType_Id = at.AccountTransactionTypeId),
				'Payment' = (SELECT Name FROM PaymentTypes WHERE AccountTransactionType_Id = at.AccountTransactionTypeId AND Ac.Id = account_id),
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
		)Q
	)R
	GROUP BY 
	LastUpdateTime, TicketDate, LastOrderDate, 
	LastPaymentDate,SyncOutletId,ticketid, ticketnumber, Payment, 
	PaymentAccountType, AccountTypeName, SortOrder, accountname
) Acc,
(
	SELECT 
	d.ID DepartmentID, 
	d.Name DepartmentName, CreatedDateTime OrderCreatedDateTime, ticketid, Orders.Id OrderID, m.GroupCode,  MenuItemID, 
	MenuItemName, PortionName, Quantity OrderedQuantity, Price, Total, PlainTotal 
	FROM Orders, departments d, MenuItems m
	WHERE orders.departmentid = d.Id
	AND DecreaseInventory = 1
	AND Orders.MenuItemId = m.Id
)o
WHERE acc.ticketid = o.ticketid

GO

