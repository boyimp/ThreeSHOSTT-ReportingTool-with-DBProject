CREATE FUNCTION [SQL#].[INET_URIGetInfo]
(@URI NVARCHAR (4000) NULL)
RETURNS 
     TABLE (
        [AbsolutePath]               NVARCHAR (4000) NULL,
        [AbsoluteUri]                NVARCHAR (4000) NULL,
        [Authority]                  NVARCHAR (4000) NULL,
        [DnsSafeHost]                NVARCHAR (4000) NULL,
        [Fragment]                   NVARCHAR (4000) NULL,
        [HashCode]                   INT             NULL,
        [Host]                       NVARCHAR (4000) NULL,
        [HostNameType]               NVARCHAR (50)   NULL,
        [IsAbsoluteUri]              BIT             NULL,
        [IsDefaultPort]              BIT             NULL,
        [IsFile]                     BIT             NULL,
        [IsLoopback]                 BIT             NULL,
        [IsUnc]                      BIT             NULL,
        [IsWellFormedOriginalString] BIT             NULL,
        [LocalPath]                  NVARCHAR (4000) NULL,
        [PathAndQuery]               NVARCHAR (4000) NULL,
        [Port]                       INT             NULL,
        [Query]                      NVARCHAR (4000) NULL,
        [Scheme]                     NVARCHAR (50)   NULL,
        [UserEscaped]                BIT             NULL,
        [UserInfo]                   NVARCHAR (4000) NULL)
AS
 EXTERNAL NAME [SQL#.Network].[INET].[URIGetInfo]


GO

