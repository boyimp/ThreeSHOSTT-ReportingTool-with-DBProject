CREATE PROCEDURE [SqlQueryNotificationStoredProcedure-d0f4344f-5822-48ed-8a48-0076af352fda] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda') > 0)   DROP SERVICE [SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda]; if (OBJECT_ID('SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-d0f4344f-5822-48ed-8a48-0076af352fda]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-d0f4344f-5822-48ed-8a48-0076af352fda]; END COMMIT TRANSACTION; END

GO
