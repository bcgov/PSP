/* -----------------------------------------------------------------------------
Alter the display order of the PIMS_DISPOSITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jan-24  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the display order
PRINT N'Alter the display order'
GO
UPDATE PIMS_DISPOSITION_FILE_STATUS_TYPE
SET    DISPLAY_ORDER = CASE
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'ACTIVE'    THEN 1
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'DRAFT'     THEN 2
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'COMPLETE'  THEN 3
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'HOLD'      THEN 4
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'CANCELLED' THEN 5
                         WHEN DISPOSITION_FILE_STATUS_TYPE_CODE = N'ARCHIVED'  THEN 6
                       END
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
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
