/* -----------------------------------------------------------------------------
Alter the data in the PIMS_CONSULTATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Oct-03  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "REGPLANG" type
PRINT N'Update the "REGPLANG" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'REGPLANG'

SELECT CONSULTATION_TYPE_CODE
FROM   PIMS_CONSULTATION_TYPE
WHERE  CONSULTATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_CONSULTATION_TYPE
  SET    DESCRIPTION                = N'Regional Planning'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  CONSULTATION_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "REGPRPSVC" type
PRINT N'Update the "REGPRPSVC" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'REGPRPSVC'

SELECT CONSULTATION_TYPE_CODE
FROM   PIMS_CONSULTATION_TYPE
WHERE  CONSULTATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_CONSULTATION_TYPE
  SET    DESCRIPTION                = N'Regional Property Services'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  CONSULTATION_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE tbl
SET    tbl.DISPLAY_ORDER              = seq.ROW_NUM
     , tbl.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_CONSULTATION_TYPE tbl JOIN
       (SELECT CONSULTATION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_CONSULTATION_TYPE) seq  ON seq.CONSULTATION_TYPE_CODE = tbl.CONSULTATION_TYPE_CODE
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
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
