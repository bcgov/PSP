-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_LL_TEAM_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- Doug Filteau  2026-Apr-08  PSP-11395  Rename and disable "Key contact" to "PIMS key contact".
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/disable the KEYCNTCT code.
PRINT N'Add/disable the KEYCNTCT code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'KEYCNTCT'

SELECT LL_TEAM_PROFILE_TYPE_CODE
FROM   PIMS_LL_TEAM_PROFILE_TYPE
WHERE  LL_TEAM_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LL_TEAM_PROFILE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LL_TEAM_PROFILE_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LL_TEAM_PROFILE_TYPE (LL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, IS_DISABLED)
  VALUES  (N'KEYCNTCT', N'PIMS key contact', 0, 1);
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
