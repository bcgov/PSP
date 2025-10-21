/* -----------------------------------------------------------------------------
Alter the PIMS_PROPERTY_ANOMALY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Oct-08  Renamed description for ACCESS
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Rename the "ACCESS" type.
PRINT N'Rename the "ACCESS" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ACCESS'

SELECT PROPERTY_ANOMALY_TYPE_CODE
FROM   PIMS_PROPERTY_ANOMALY_TYPE
WHERE  PROPERTY_ANOMALY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROPERTY_ANOMALY_TYPE
  SET    DESCRIPTION                = 'Access'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_ANOMALY_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY_ANOMALY_TYPE biz JOIN
       (SELECT PROPERTY_ANOMALY_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROPERTY_ANOMALY_TYPE) seq  ON seq.PROPERTY_ANOMALY_TYPE_CODE = biz.PROPERTY_ANOMALY_TYPE_CODE
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
