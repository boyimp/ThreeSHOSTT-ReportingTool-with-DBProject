/*******************************************************************************

	SQLsharp_UnloadAppDomain

	Copyright (C) 2017 Sql Quantum Lift, LLC. All rights reserved.

	This stored procedure is pure T-SQL (no SQLCLR) and is intended to
	unload any App Domains that are currently loaded in the database in
	which SQL# is installed.

	This stored procedure will help when an external resource (usually a file)
	gets locked by sqlservr.exe and cannot be accessed. This can happen if a
	process / session is killed while a SQLCLR object has that resource locked.
	This rarely happens, but if it does, try this stored procedure before
	doing something more impacting, such as restarting the Instance.

 *******************************************************************************/

 CREATE PROCEDURE SQL#.SQLsharp_UnloadAppDomain
 (
	@DatabaseName sysname = NULL
 )
 AS
 SET NOCOUNT ON;

	DECLARE @DatabaseID SMALLINT;

	BEGIN TRY

	IF (@DatabaseName IS NULL)
	BEGIN
		SET @DatabaseName = DB_NAME();
	END;

	-- Get DB_ID for queries:
	SET @DatabaseID = DB_ID(@DatabaseName);

	IF (@DatabaseID IS NULL)
	BEGIN
		RAISERROR(N'Database [%s] does not exist!', 16, 1, @DatabaseName);
	END;

	-- Quote the DB_NAME:
	SET @DatabaseName = QUOTENAME(@DatabaseName);


	SELECT  'sys.dm_clr_appdomains' AS [DMV], 'BEFORE' AS [BEFORE], *
	FROM    sys.dm_clr_appdomains
	WHERE   [db_id] = @DatabaseID

	IF (EXISTS(
		SELECT  sd.*
		FROM    sys.databases sd
		WHERE   sd.[database_id] = @DatabaseID
		AND     sd.[is_trustworthy_on] = 0
	   ))
	BEGIN
		PRINT 'Enabling then disabling TRUSTWORTHY...';
		EXEC(N'ALTER DATABASE ' + @DatabaseName + N' SET TRUSTWORTHY ON;');
		EXEC(N'ALTER DATABASE ' + @DatabaseName + N' SET TRUSTWORTHY OFF;');
	END;
	ELSE
	BEGIN
		PRINT 'Disabling then enabling TRUSTWORTHY...';
		EXEC(N'ALTER DATABASE ' + @DatabaseName + N' SET TRUSTWORTHY OFF;');
		EXEC(N'ALTER DATABASE ' + @DatabaseName + N' SET TRUSTWORTHY ON;');
	END;

	SELECT  'sys.dm_clr_appdomains' AS [DMV], 'AFTER' AS [AFTER], *
	FROM    sys.dm_clr_appdomains
	WHERE   [db_id] = @DatabaseID;


	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK;
		END;

		DECLARE @ErrorMessage NVARCHAR(4000),
			   @ErrorLevel INT,
			   @ErrorState INT,
			   @ErrorNumber INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			  @ErrorLevel = ERROR_SEVERITY(),
			  @ErrorState = ERROR_STATE(),
			  @ErrorNumber = ERROR_NUMBER();

		RAISERROR(N'Error %d: %s.', @ErrorLevel, @ErrorState, @ErrorNumber, @ErrorMessage);
		RETURN;

	END CATCH;

GO

