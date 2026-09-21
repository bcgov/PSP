-- -------------------------------------------------------------------------------------------
-- Remove notification types.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Arturo Reyes  2026-Sep-18  PSP-11980  Remove new notification types
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
DELETE PIMS_NOTIFICATION_TYPE
WHERE  NOTIFICATION_TYPE_CODE IN (N'L_PERIOD_DUEDT', N'TAKE_LAND_ACT', N'EXPROPH_ADVPYSVDT', N'EXPROPH_VESTDT', N'EXPROPH_APPREFFDT', N'AGMT_AGMTDT', N'AGMT_COMPTDT', N'AGMT_TERMINDT');


GO
IF @@ERROR <> 0
    SET NOEXEC ON;


GO
-- Restore the display order after removing the new values.
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
DECLARE @Success AS BIT;

SET @Success = 1;

SET NOEXEC OFF;

IF (@Success = 1)
    PRINT 'The database update succeeded';
ELSE
    BEGIN
        IF @@TRANCOUNT > 0
            ROLLBACK;
        PRINT 'The database update failed';
    END
GO
