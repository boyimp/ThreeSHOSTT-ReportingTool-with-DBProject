CREATE PROCEDURE [SQL#].[DB_BulkCopy]
@SourceType NVARCHAR (4000) NULL=NULL, @SourceConnection NVARCHAR (4000) NULL=NULL, @SourceQuery NVARCHAR (4000) NULL, @DestinationConnection NVARCHAR (4000) NULL=NULL, @DestinationTableName NVARCHAR (4000) NULL, @BatchSize INT NULL=0, @NotifyAfterRows INT NULL=0, @TimeOut INT NULL=30, @ColumnMappings NVARCHAR (4000) NULL=NULL, @BulkCopyOptionsList NVARCHAR (4000) NULL=NULL, @SourceCommandTimeout INT NULL=30, @RowsCopied BIGINT NULL=-1 OUTPUT
AS EXTERNAL NAME [SQL#.DB].[DB].[BulkCopy]


GO

