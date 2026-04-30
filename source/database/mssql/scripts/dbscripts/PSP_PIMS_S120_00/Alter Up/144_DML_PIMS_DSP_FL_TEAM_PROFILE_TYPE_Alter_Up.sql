-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_DSP_FL_TEAM_PROFILE_TYPE table.
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

SELECT DSP_FL_TEAM_PROFILE_TYPE_CODE
FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE
WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_FL_TEAM_PROFILE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_DSP_FL_TEAM_PROFILE_TYPE (DSP_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, IS_DISABLED)
  VALUES  (N'KEYCNTCT', N'PIMS key contact', 0, 1);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE biz JOIN
       (SELECT DSP_FL_TEAM_PROFILE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE) seq ON seq.DSP_FL_TEAM_PROFILE_TYPE_CODE = biz.DSP_FL_TEAM_PROFILE_TYPE_CODE
WHERE  biz.DSP_FL_TEAM_PROFILE_TYPE_CODE <> 'KEYCNTCT'
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
