-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_NOTIFICATION_TYPE table.
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

-- Populate the PIMS_NOTIFICATION_TYPE table.
PRINT N'Populate the PIMS_NOTIFICATION_TYPE table.'
GO
INSERT INTO PIMS_NOTIFICATION_TYPE (NOTIFICATION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'TAKE_SRW',         N'SRW - take'),
  (N'TAKE_LAT',         N'Land Act - take'),
  (N'TAKE_LTC',         N'Licence to construct - take'),
  (N'TAKE_LPYBLE',      N'Lease payable - take'),
  (N'L_RENEWAL',        N'L/L renewal'),
  (N'L_INSURANCE',      N'L/L insurance policy expiry'),
  (N'L_CONSULTFN',      N'L/L First Nations consultation'),
  (N'NOC',              N'Notice of claim'),
  (N'ADV_PAY',          N'Advance payment'),
  (N'AGMT_SIGND',       N'Agreement signed'),
  (N'EXPROPH_APPEFFDT', N'Expropriation application effective date');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_NOTIFICATION_TYPE biz JOIN
       (SELECT NOTIFICATION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_NOTIFICATION_TYPE) seq ON seq.NOTIFICATION_TYPE_CODE = biz.NOTIFICATION_TYPE_CODE
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
