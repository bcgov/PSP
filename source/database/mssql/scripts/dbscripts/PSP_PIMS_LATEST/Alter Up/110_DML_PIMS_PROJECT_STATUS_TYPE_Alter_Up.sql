/* -----------------------------------------------------------------------------
Alter the display order of the PIMS_PROJECT_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-04  Initial version.
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
UPDATE PIMS_PROJECT_STATUS_TYPE
SET    DISPLAY_ORDER = CASE DISPOSITION_FILE_STATUS_TYPE_CODE
                         WHEN N'AC'   THEN 1
                         WHEN N'CO'   THEN 2
                         WHEN N'HO'   THEN 3
                         WHEN N'PL'   THEN 4
                         WHEN N'CA'   THEN 5
                         WHEN N'CNCN' THEN 6
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
