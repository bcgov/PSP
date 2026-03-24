-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_EXPROP_OWNER_HISTORY_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Mar-13  PSP-11293  Disable APPEFFCTVDT.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "APPEFFCTVDT" type.
PRINT N'Disable the "APPEFFCTVDT" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'APPEFFCTVDT'

SELECT EXPROP_OWNER_HISTORY_TYPE_CODE
FROM   PIMS_EXPROP_OWNER_HISTORY_TYPE
WHERE  EXPROP_OWNER_HISTORY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_EXPROP_OWNER_HISTORY_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  EXPROP_OWNER_HISTORY_TYPE_CODE = @CurrCd;
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
