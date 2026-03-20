-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_REGION table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Mar-13  PSP-11302  Add Headquarters region.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable the Headquarters region.
PRINT N'Add/enable the Headquarters region.'
GO
DECLARE @CurrCd SMALLINT
SET     @CurrCd = 0

SELECT REGION_CODE
FROM   PIMS_REGION
WHERE  REGION_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_REGION
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  REGION_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_REGION (REGION_CODE, REGION_NAME)
  VALUES (0, N'Headquarters (HQ)');
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
