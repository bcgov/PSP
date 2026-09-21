-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_NOTIFICATION_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Arturo Reyes  2026-Sep-18  PSP-11980  Add new notification types
-- -------------------------------------------------------------------------------------------
SET XACT_ABORT ON;
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
GO
BEGIN TRANSACTION;
GO
IF @@ERROR <> 0
    SET NOEXEC ON;
GO
PRINT N'Populate the PIMS_NOTIFICATION_TYPE table.';
GO
INSERT INTO PIMS_NOTIFICATION_TYPE (NOTIFICATION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'L_PERIOD_DUEDT',    N'Due date - L/L Periods'),
  (N'TAKE_LAND_ACT',     N'Land Act - take'),
  (N'EXPROPH_ADVPYSVDT', N'Expropriation advanced payment served date'),
  (N'EXPROPH_VESTDT',    N'Expropriation vesting date'),
  (N'EXPROPH_APPREFFDT', N'Expropriation appraisal effective date'),
  (N'AGMT_AGMTDT',       N'Agreement date'),
  (N'AGMT_COMPTDT',      N'Agreement completion date'),
  (N'AGMT_TERMINDT',     N'Agreement termination date');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM,
       biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_NOTIFICATION_TYPE AS biz
       INNER JOIN
       (SELECT NOTIFICATION_TYPE_CODE,
               ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_NOTIFICATION_TYPE) AS seq
       ON seq.NOTIFICATION_TYPE_CODE = biz.NOTIFICATION_TYPE_CODE;
GO
IF @@ERROR <> 0
    SET NOEXEC ON;
GO
COMMIT TRANSACTION;
GO
IF @@ERROR <> 0
    SET NOEXEC ON;
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF;
IF (@Success = 1)
  PRINT 'The database update succeeded';
ELSE
  BEGIN
    IF @@TRANCOUNT > 0
      ROLLBACK TRANSACTION
    PRINT 'The database update failed';
  END
GO