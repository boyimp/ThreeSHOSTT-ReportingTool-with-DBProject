
CREATE VIEW [dbo].[OrderCost] as
select Tickets.Id, Tickets.TicketNumber, Tickets.Date, LastUpdateTime, LastOrderDate, LastPaymentDate, Orders.Id OrderID,
MenuItemID, MenuItemName, PortionName, Orders.Quantity,
'UnitProductionCost'
=isnull(
			(
				SELECT Sum(Quantity*Price) TotalPrice  FROM 
				(    
					SELECT mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName, i.Name InventoryItemName, ri.Quantity, i.BaseUnit,
					'Price' =(
								SELECT max(
											CASE  WHEN Unit = BaseUnit THEN Price 
											  Else Price / TransactionUnitMultiplier
											  END 
										   )as Price FROM 
											(  
													SELECT s.*
													FROM InventoryTransactionSummary s
													WHERE s.rk = 1

											)t, inventoryitems ini WHERE t.InventoryItem_Id = ini.Id AND ini.Id = i.Id
							  )
					FROM MenuItems mi, menuitemportions mip,
					Recipes r, recipeitems ri, inventoryitems i
					WHERE
					mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id
					AND  r.Id = ri.RecipeId AND ri.InventoryItem_Id = i.Id
					AND mip.Name = Orders.PortionName AND orders.MenuItemId = mi.Id
				) T
			), 0
		),
'UnitFixedCost'
=
isnull(	
			(
				SELECT mAx(Price) FixedProductionCost  FROM 
	            (    
	                SELECT mi.Id MenuItemId,mi.Name MenuItemName, mip.Name PortionName, r.Name RecipeName,   r.FixedCost    Price
	                FROM MenuItems mi, menuitemportions mip, Recipes r
	                WHERE
	                mi.Id = mip.MenuItemId AND mip.Id = r.Portion_Id   
	               AND mip.Name = Orders.PortionName AND orders.MenuItemId = mi.Id
	
	            ) T
	            group by MenuItemId,MenuItemName
	        ), 0
	  )

 from orders, Tickets
 WHERE Orders.TicketId = tickets.Id
 AND Orders.DecreaseInventory = 1

GO

