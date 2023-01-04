CREATE AGGREGATE [SQL#].[Agg_Join](@value NVARCHAR (4000) NULL)
    RETURNS NVARCHAR (MAX)
    EXTERNAL NAME [SQL#.TypesAndAggregates].[Agg_Join];


GO

