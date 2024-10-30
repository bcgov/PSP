/* -----------------------------------------------------------------------------
Alter the data in the PIMS_CONSULTATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/enable the "ENGINEERG" type
PRINT N'Insert/enable the "ENGINEERG" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ENGINEERG'

SELECT CONSULTATION_TYPE_CODE
FROM   PIMS_CONSULTATION_TYPE
WHERE  CONSULTATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_CONSULTATION_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  CONSULTATION_TYPE_CODE = @CurrCd;
ELSE  
  INSERT INTO PIMS_CONSULTATION_TYPE (CONSULTATION_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'ENGINEERG', N'Engineering');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "1STNATION" type
PRINT N'Update the "1STNATION" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'1STNATION'

SELECT CONSULTATION_TYPE_CODE
FROM   PIMS_CONSULTATION_TYPE
WHERE  CONSULTATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_CONSULTATION_TYPE
  SET    DESCRIPTION                = N'First Nation'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  CONSULTATION_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prt
SET    prt.DISPLAY_ORDER              = seq.ROW_NUM
     , prt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_CONSULTATION_TYPE prt JOIN
       (SELECT CONSULTATION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_CONSULTATION_TYPE
        WHERE  CONSULTATION_TYPE_CODE <> N'OTHER') seq  ON seq.CONSULTATION_TYPE_CODE = prt.CONSULTATION_TYPE_CODE
WHERE  prt.CONSULTATION_TYPE_CODE <> N'OTHER'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_CONSULTATION_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  CONSULTATION_TYPE_CODE = N'OTHER'
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
