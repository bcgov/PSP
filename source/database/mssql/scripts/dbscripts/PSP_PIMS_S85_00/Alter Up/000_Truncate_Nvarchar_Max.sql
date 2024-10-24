/*******************************************************************************
Truncate the NVARCHAR(x) fields to ensure they fit into the new columns.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-23  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMSX_TableDefinitions
UPDATE PIMSX_TableDefinitions
SET    DESCRIPTION                = SUBSTRING(DESCRIPTION, 1, 500)
WHERE  LEN(DESCRIPTION) > 500
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_LEASE
UPDATE PIMS_LEASE
SET    LEASE_DESCRIPTION          = SUBSTRING(LEASE_DESCRIPTION, 1, 2000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(LEASE_DESCRIPTION) > 2000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE
SET    LEASE_NOTES                = SUBSTRING(LEASE_NOTES, 1, 4000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(LEASE_NOTES) > 4000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE
SET    RETURN_NOTES               = SUBSTRING(RETURN_NOTES, 1, 1000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(RETURN_NOTES) > 1000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE
SET    INSPECTION_NOTES            = SUBSTRING(INSPECTION_NOTES, 1, 1000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(INSPECTION_NOTES) > 1000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_PROPERTY
UPDATE PIMS_PROPERTY
SET    LAND_LEGAL_DESCRIPTION     = SUBSTRING(LAND_LEGAL_DESCRIPTION, 1, 2000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(LAND_LEGAL_DESCRIPTION) > 2000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_PROPERTY
SET    NOTES                      = SUBSTRING(NOTES, 1, 4000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(NOTES) > 4000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_RESEARCH_FILE
UPDATE PIMS_RESEARCH_FILE
SET    ROAD_NAME                   = SUBSTRING(ROAD_NAME, 1, 200)
      , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(ROAD_NAME) > 200
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_RESEARCH_FILE
SET    ROAD_ALIAS                 = SUBSTRING(ROAD_ALIAS, 1, 200)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(ROAD_ALIAS) > 200
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_RESEARCH_FILE
SET    REQUEST_DESCRIPTION        = SUBSTRING(REQUEST_DESCRIPTION, 1, 3000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(REQUEST_DESCRIPTION) > 3000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_RESEARCH_FILE
SET    RESEARCH_RESULT            = SUBSTRING(RESEARCH_RESULT, 1, 2000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(RESEARCH_RESULT) > 2000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_RESEARCH_FILE
SET    EXPROPRIATION_NOTES        = SUBSTRING(EXPROPRIATION_NOTES, 1, 1000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(EXPROPRIATION_NOTES) > 1000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_PROPERTY_RESEARCH_FILE
UPDATE PIMS_PROPERTY_RESEARCH_FILE
SET    RESEARCH_SUMMARY           = SUBSTRING(RESEARCH_SUMMARY, 1, 1000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(RESEARCH_SUMMARY) > 1000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_ACCESS_REQUEST
UPDATE PIMS_ACCESS_REQUEST
SET    NOTE                       = SUBSTRING(NOTE, 1, 1000)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(NOTE) > 1000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_ACQUISITION_FILE_FORM
UPDATE PIMS_ACQUISITION_FILE_FORM
SET    FORM_JSON                   = SUBSTRING(FORM_JSON, 1, 4000)
      , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(FORM_JSON) > 4000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_PROPERTY_ACTIVITY_HIST
UPDATE PIMS_PROPERTY_ACTIVITY_HIST
SET    DESCRIPTION                = SUBSTRING(DESCRIPTION, 1, 500)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(DESCRIPTION) > 500
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- PIMS_PROPERTY_ACTIVITY
UPDATE PIMS_PROPERTY_ACTIVITY
SET    DESCRIPTION                = SUBSTRING(DESCRIPTION, 1, 500)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEN(DESCRIPTION) > 500
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
