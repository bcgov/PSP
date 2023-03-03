/* -----------------------------------------------------------------------------
Convert data into the PIMS_ACQUISITION_FILE_STATUS_TYPE table from "CLOSED" to 
"COMPLT" status.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Oct-31 Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Convert "CLOSED" to "COMPLT"
UPDATE PIMS_ACQUISITION_FILE
SET    ACQUISITION_FILE_STATUS_TYPE_CODE = N'COMPLT'
     , CONCURRENCY_CONTROL_NUMBER        = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = N'CLOSED'
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
