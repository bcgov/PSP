/*******************************************************************************
Set the current rows for PIMS_ACQUISITION_FILE_PERSON to copy to
PIMS_ACQUISITION_FILE_TEAM post-Alter_Down.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Oct-12  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the existing data to PIMS_ACQUISITION_FILE_TEAM
INSERT INTO [dbo].[PIMS_ACQUISITION_FILE_TEAM] (ACQUISITION_FILE_ID, PERSON_ID, ACQ_FL_TEAM_PROFILE_TYPE_CODE, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT ACQUISITION_FILE_ID
       , PERSON_ID
       , ACQ_FL_PERSON_PROFILE_TYPE_CODE
       , APP_CREATE_TIMESTAMP
       , APP_CREATE_USERID
       , APP_CREATE_USER_GUID
       , APP_CREATE_USER_DIRECTORY
       , APP_LAST_UPDATE_TIMESTAMP
       , APP_LAST_UPDATE_USERID
       , APP_LAST_UPDATE_USER_GUID
       , APP_LAST_UPDATE_USER_DIRECTORY
  FROM   [dbo].[TMP_ACQUISITION_FILE_PERSON]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for TMP_ACQUISITION_FILE_SEQ_VAL
DROP TABLE IF EXISTS [dbo].[TMP_ACQUISITION_FILE_PERSON] 
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
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
