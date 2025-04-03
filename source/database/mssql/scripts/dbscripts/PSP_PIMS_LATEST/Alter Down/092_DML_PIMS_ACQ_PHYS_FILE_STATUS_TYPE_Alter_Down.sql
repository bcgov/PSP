/* -----------------------------------------------------------------------------
Alter the display order of the PIMS_ACQ_PHYS_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-13  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "UNKNOWN" type
PRINT N'Update the "UNKNOWN" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UNKNOWN'

SELECT ACQ_PHYS_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQ_PHYS_FILE_STATUS_TYPE
WHERE  ACQ_PHYS_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_ACQ_PHYS_FILE_STATUS_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQ_PHYS_FILE_STATUS_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the display order
PRINT N'Alter the display order'
GO
UPDATE PIMS_ACQ_PHYS_FILE_STATUS_TYPE
SET    DISPLAY_ORDER              = NULL
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
