/* -----------------------------------------------------------------------------
Migrate the notes column in the PIMS_PROPERTY table to the PIMS_NOTE table and 
populate the PIMS_PROPERTY_NOTE to reference the new note location.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jun-24  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the data for the property notes
PRINT N'Migrate the data for the property notes'
GO
DECLARE @PKVlu    BIGINT,
        @NoteValu VARCHAR(4000)

DECLARE crsr CURSOR FOR 
          SELECT PROPERTY_ID
               , NOTES
          FROM   PIMS_PROPERTY
          WHERE  NOTES IS NOT NULL
          ORDER BY PROPERTY_ID;

OPEN crsr
FETCH NEXT FROM crsr INTO @PKVlu, @NoteValu
WHILE @@FETCH_STATUS = 0
  BEGIN
  INSERT INTO PIMS_NOTE (NOTE_TXT, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
  VALUES
    ('>>' + CONVERT(VARCHAR, @PKVlu) + '<<' + @NoteValu, 
    1, getutcdate(), 'DOFILTEA', '107_Migration', getutcdate(), 'DOFILTEA', '107_Migration')
     
  FETCH NEXT FROM crsr INTO @PKVlu, @NoteValu
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the data for the property notes
PRINT N'Migrate the data for the property notes'
GO
INSERT INTO PIMS_PROPERTY_NOTE (PROPERTY_ID, NOTE_ID, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT SUBSTRING(NOTE_TXT, CHARINDEX('>>', NOTE_TXT) + 2, CHARINDEX('<<', NOTE_TXT) - 3)
       , NOTE_ID
       , 1, getutcdate(), 'DOFILTEA', '107_Migration', getutcdate(), 'DOFILTEA', '107_Migration'
  FROM   PIMS_NOTE
  WHERE  APP_LAST_UPDATE_USER_DIRECTORY = '107_Migration'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Clean up prior to exiting
PRINT N'Clean up prior to exiting'
GO
CLOSE      crsr
DEALLOCATE crsr
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove the PROPERTY_ID PK from the affected NOTE_TXT column for the migrated rows.
PRINT N'Remove the PROPERTY_ID PK from the affected NOTE_TXT column for the migrated rows.'
GO
UPDATE PIMS_NOTE
SET    NOTE_TXT = STUFF(NOTE_TXT, 1, CHARINDEX('<<', NOTE_TXT) + 1, '')
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  APP_LAST_UPDATE_USER_DIRECTORY = '107_Migration'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
