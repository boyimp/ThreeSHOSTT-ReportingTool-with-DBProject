CREATE TYPE [dbo].[IDList] AS TABLE (
    [ID] INT NULL);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User Defined Type for Table parameter ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TYPE', @level1name = N'IDList';


GO

