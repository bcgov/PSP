 /* ----------------------------------------------------------------------------
Alter the data in the PIMS_TAKE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Apr-22  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update take completion date with acquisition completion date where both 
-- acquisition and take are complete and acquisition date is not null.
PRINT N'Update take completion date'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
UPDATE tak
SET    tak.COMPLETION_DT              = aqf.COMPLETION_DATE
     , tak.CONCURRENCY_CONTROL_NUMBER = tak.CONCURRENCY_CONTROL_NUMBER + 1
     , APP_CREATE_TIMESTAMP           = getutcdate()
     , APP_CREATE_USERID              = N'PSP-8237'
     , APP_CREATE_USER_DIRECTORY      = N'PSP-8237'
     , APP_LAST_UPDATE_TIMESTAMP      = getutcdate()
     , APP_LAST_UPDATE_USERID         = N'PSP-8237'
     , APP_LAST_UPDATE_USER_DIRECTORY = N'PSP-8237'
FROM   PIMS_ACQUISITION_FILE          aqf                                                               JOIN
       PIMS_PROPERTY_ACQUISITION_FILE paf ON paf.ACQUISITION_FILE_ID          = aqf.ACQUISITION_FILE_ID JOIN
       PIMS_TAKE                      tak ON tak.PROPERTY_ACQUISITION_FILE_ID = paf.PROPERTY_ACQUISITION_FILE_ID
WHERE  aqf.ACQUISITION_FILE_STATUS_TYPE_CODE = N'COMPLT'
   AND aqf.COMPLETION_DATE                   IS NOT NULL
   AND tak.TAKE_STATUS_TYPE_CODE             = N'COMPLETE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create a take record for a property associated with a completed acquisition 
-- file that does not have an associated take.
PRINT N'Create a take record for a property'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
INSERT INTO PIMS_TAKE (PROPERTY_ACQUISITION_FILE_ID, TAKE_TYPE_CODE, TAKE_STATUS_TYPE_CODE, COMPLETION_DT, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT paf.PROPERTY_ACQUISITION_FILE_ID
       , N'IMPORTED'
       , N'COMPLETE'
       , aqf.COMPLETION_DATE
       , getutcdate()
       , N'PSP-8237'
       , N'PSP-8237'
       , getutcdate()
       , N'PSP-8237'
       , N'PSP-8237'
  FROM   PIMS_ACQUISITION_FILE          aqf JOIN
         PIMS_PROPERTY_ACQUISITION_FILE paf ON paf.ACQUISITION_FILE_ID = aqf.ACQUISITION_FILE_ID
  WHERE  aqf.ACQUISITION_FILE_STATUS_TYPE_CODE = N'COMPLT'
     AND aqf.COMPLETION_DATE                   IS NOT NULL
     AND paf.PROPERTY_ACQUISITION_FILE_ID      NOT IN (SELECT PROPERTY_ACQUISITION_FILE_ID
                                                       FROM   PIMS_TAKE
                                                       WHERE  TAKE_STATUS_TYPE_CODE = N'COMPLETE')
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
