CREATE FUNCTION [SQL#].[Util_IsValidCheckRoutingNumber]
(@RoutingNumber NVARCHAR (4000) NULL)
RETURNS BIT
AS
 EXTERNAL NAME [SQL#].[UTILITY].[IsValidCheckRoutingNumber]


GO

