/* -----------------------------------------------------------------------------
Alter the data in the PIMS_RESEARCH_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-02  Initial version.  Renamed "Closed" to "Complete" and 
                           renamed "Inactive" to "Hold"
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "CLOSED" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CLOSED'

SELECT RESEARCH_FILE_STATUS_TYPE_CODE
FROM   PIMS_RESEARCH_FILE_STATUS_TYPE
WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_RESEARCH_FILE_STATUS_TYPE
  SET    DESCRIPTION                = N'Complete'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "INACTIVE" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'INACTIVE'

SELECT RESEARCH_FILE_STATUS_TYPE_CODE
FROM   PIMS_RESEARCH_FILE_STATUS_TYPE
WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_RESEARCH_FILE_STATUS_TYPE
  SET    DESCRIPTION                = N'Hold'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;
  END
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

