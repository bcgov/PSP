-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_DSP_FL_TEAM_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Nullify the DISPLAY_ORDER.
PRINT N'Nullify the DISPLAY_ORDER.'
GO
UPDATE PIMS_DSP_FL_TEAM_PROFILE_TYPE
SET    DISPLAY_ORDER              = NULL
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE <> 'KEYCNTCT';
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the KEYCNTCT code.
PRINT N'Disable the KEYCNTCT code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'KEYCNTCT'

SELECT DSP_FL_TEAM_PROFILE_TYPE_CODE
FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE
WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_FL_TEAM_PROFILE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;
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
