
CREATE PROCEDURE [dbo].[BatchDeduction]
(
	@CurrentConsumption AS DECIMAL(18,4),
	@IsCarryForward AS BIT 
)
AS BEGIN	
	
	DECLARE		
	@TotalStockToDeduct DECIMAL(18,4),
	@BatchCount INT,
	@TempBatchPrice DECIMAL(18,4),
	@TempStock DECIMAL(18,4),
	@TempCreationDate DATETIME,
	@CurrentConsumptionValue DECIMAL(18,4),
	@CarryForward DECIMAL(18,4)
	
	
	
	set @TotalStockToDeduct = isnull(@CurrentConsumption, 0)
	set @CurrentConsumptionValue = 0
	set @BatchCount = (select count(*) from ##tempBatchInfo where QT > 0)				
	WHILE  @TotalStockToDeduct > 0 and @BatchCount > 0
	BEGIN 
		SET @TempBatchPrice = (SELECT TOP 1 BP FROM ##tempBatchInfo WHERE QT > 0 order by cast(CD as datetime))
		SET @TempStock = (SELECT TOP 1 QT FROM ##tempBatchInfo WHERE QT > 0 order by cast(CD as datetime))
		SET @TempCreationDate = (SELECT TOP 1 cast(CD as datetime) FROM ##tempBatchInfo WHERE QT > 0 order by cast(CD as datetime))
		IF @TotalStockToDeduct >= @TempStock
		BEGIN
			SET @TotalStockToDeduct = @TotalStockToDeduct - @TempStock
			SET @CurrentConsumptionValue = @CurrentConsumptionValue + (@TempBatchPrice * @TempStock)
			UPDATE ##tempBatchInfo SET QT = 0 WHERE BP = @TempBatchPrice and cast(CD as datetime) = @TempCreationDate
		END
		ELSE 
		BEGIN
			SET @CurrentConsumptionValue = @CurrentConsumptionValue + (@TempBatchPrice * @TotalStockToDeduct)	   		
			UPDATE ##tempBatchInfo SET QT = @TempStock - @TotalStockToDeduct WHERE  BP = @TempBatchPrice and cast(CD as datetime) = @TempCreationDate
			SET @TotalStockToDeduct = 0
		END
		set @BatchCount = (select count(*) from ##tempBatchInfo where QT > 0)					
	END
	
	IF @IsCarryForward = 0 
	BEGIN 
		SET @CarryForward = @TotalStockToDeduct
	END 
	ELSE 
	BEGIN
		SET @CarryForward = (@CurrentConsumption - @TotalStockToDeduct)* -1
	END 
	

	TRUNCATE TABLE ##ConsumptionInfo
	INSERT INTO ##ConsumptionInfo
	SELECT 	@CarryForward CarryForwardQty, @CurrentConsumptionValue ConsumptionValue

	
	if @TotalStockToDeduct > 0 AND @IsCarryForward = 1 --and @BatchCount > 0
	begin
		SET @TempBatchPrice = (SELECT TOP 1 BP FROM ##tempBatchInfo order by cast(CD as datetime) desc)
		SET @TempCreationDate = (SELECT TOP 1 cast(CD as datetime) FROM ##tempBatchInfo  order by cast(CD as datetime) desc)
		--SET @CurrentConsumptionValue = @CurrentConsumptionValue + (@TempBatchPrice * @TotalStockToDeduct)
		UPDATE ##tempBatchInfo SET QT = QT - @TotalStockToDeduct WHERE  BP = @TempBatchPrice and cast(CD as datetime) = @TempCreationDate
		SET @TotalStockToDeduct = 0
	end
		
END 
GRANT EXECUTE ON dbo.BatchDeduction TO PUBLIC

GO

GRANT EXECUTE
    ON OBJECT::[dbo].[BatchDeduction] TO PUBLIC
    AS [dbo];


GO

