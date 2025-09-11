/* -----------------------------------------------------------------------------
Alter the PIMS_SURVEY_PLAN_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "S107S10MOTX" type.
PRINT N'Alter the "S107S10MOTX" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'S107S10MOTX'

SELECT SURVEY_PLAN_TYPE_CODE
FROM   PIMS_SURVEY_PLAN_TYPE
WHERE  SURVEY_PLAN_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_SURVEY_PLAN_TYPE
  SET    DESCRIPTION                = N'S.107 LTA/ s.10 TA/ s.6 EA ref - MoTT Expropriation'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  SURVEY_PLAN_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "S113S10MOTX" type.
PRINT N'Alter the "S113S10MOTX" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'S113S10MOTX'

SELECT SURVEY_PLAN_TYPE_CODE
FROM   PIMS_SURVEY_PLAN_TYPE
WHERE  SURVEY_PLAN_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_SURVEY_PLAN_TYPE
  SET    DESCRIPTION                = N'S.113 LTA/ s.10 TA/s.6 EA ref - MoTT Expropriation SRW'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  SURVEY_PLAN_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_SURVEY_PLAN_TYPE biz JOIN
       (SELECT SURVEY_PLAN_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_SURVEY_PLAN_TYPE) seq  ON seq.SURVEY_PLAN_TYPE_CODE = biz.SURVEY_PLAN_TYPE_CODE
WHERE  biz.SURVEY_PLAN_TYPE_CODE <> (N'OTHER')
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order for the OTHER code value
-- --------------------------------------------------------------
UPDATE PIMS_SURVEY_PLAN_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  SURVEY_PLAN_TYPE_CODE = N'OTHER';
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
