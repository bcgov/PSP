/* -----------------------------------------------------------------------------
Migrate the NOTES_TXT column in the PIMS_PROPERTY_NOTE table to the 
PIMS_PROPERTY table.
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
UPDATE prop
SET    prop.NOTES                      = note.NOTE_TXT
     , prop.CONCURRENCY_CONTROL_NUMBER = prop.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY_NOTE pnot                                        JOIN
       PIMS_PROPERTY      prop ON prop.PROPERTY_ID = pnot.PROPERTY_ID JOIN
       PIMS_NOTE          note ON note.NOTE_ID     = pnot.NOTE_ID
WHERE  note.APP_LAST_UPDATE_USER_DIRECTORY = N'107_Migration'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove the migrated notes from PIMS_PROPERTY_NOTE
PRINT N'Remove the migrated notes from PIMS_PROPERTY_NOTE'
GO
DELETE pnot
FROM   PIMS_PROPERTY_NOTE pnot JOIN
       PIMS_NOTE          note ON note.NOTE_ID = pnot.NOTE_ID
WHERE  note.APP_LAST_UPDATE_USER_DIRECTORY = N'107_Migration'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove the migrated property notes
PRINT N'Remove the migrated property notes'
GO
DELETE 
FROM  PIMS_NOTE 
WHERE APP_LAST_UPDATE_USER_DIRECTORY = N'107_Migration'
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
