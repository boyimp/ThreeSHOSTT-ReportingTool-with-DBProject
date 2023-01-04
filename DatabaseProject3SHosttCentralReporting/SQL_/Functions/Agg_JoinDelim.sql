CREATE AGGREGATE [SQL#].[Agg_JoinDelim](@Value NVARCHAR (4000) NULL, @Delimiter NVARCHAR (4000) NULL)
    RETURNS NVARCHAR (MAX)
    EXTERNAL NAME [SQL#.TypesAndAggregates].[Agg_JoinDelim];


GO

