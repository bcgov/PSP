-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_NOTIFICATION_OUTPUT_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Mar-18  PSP-11294  Create reminder for key date
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the PIMS_NOTIFICATION_OUTPUT_TYPE table.
PRINT N'Populate the PIMS_NOTIFICATION_OUTPUT_TYPE table.'
GO
INSERT INTO PIMS_NOTIFICATION_OUTPUT_TYPE (NOTIFICATION_OUTPUT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'EMAIL', N'E-mail'),
  (N'PIMS',  N'PIMS application notification');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_NOTIFICATION_OUTPUT_TYPE biz JOIN
       (SELECT NOTIFICATION_OUTPUT_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_NOTIFICATION_OUTPUT_TYPE) seq ON seq.NOTIFICATION_OUTPUT_TYPE_CODE = biz.NOTIFICATION_OUTPUT_TYPE_CODE
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
