CREATE PROCEDURE [SqlQueryNotificationStoredProcedure-89361ada-53e7-4888-a804-1aba7cd81c2b] AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM [SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b]; IF (SELECT COUNT(*) FROM [SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b] WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = 'SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b') > 0)   DROP SERVICE [SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b]; if (OBJECT_ID('SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b', 'SQ') IS NOT NULL)   DROP QUEUE [SqlQueryNotificationService-89361ada-53e7-4888-a804-1aba7cd81c2b]; DROP PROCEDURE [SqlQueryNotificationStoredProcedure-89361ada-53e7-4888-a804-1aba7cd81c2b]; END COMMIT TRANSACTION; END

GO
