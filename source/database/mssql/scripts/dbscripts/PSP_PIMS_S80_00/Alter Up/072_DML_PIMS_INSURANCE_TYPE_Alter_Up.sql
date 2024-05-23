/* -----------------------------------------------------------------------------
Alter the data in the PIMS_INSURANCE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-01  Initial version.  Added ACCIDENT and UAVDRONE.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "ACCIDENT" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ACCIDENT'

SELECT INSURANCE_TYPE_CODE
FROM   PIMS_INSURANCE_TYPE
WHERE  INSURANCE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_INSURANCE_TYPE (INSURANCE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'ACCIDENT', N'Accidental Coverage');
ELSE  
  UPDATE PIMS_INSURANCE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  INSURANCE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON

-- Insert the "UAVDRONE" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UAVDRONE'

SELECT INSURANCE_TYPE_CODE
FROM   PIMS_INSURANCE_TYPE
WHERE  INSURANCE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_INSURANCE_TYPE (INSURANCE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'UAVDRONE', N'Unmanned Air Vehicle (UAV/Drone) Coverage');
ELSE  
  UPDATE PIMS_INSURANCE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  INSURANCE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE lpt
SET    lpt.DISPLAY_ORDER              = seq.ROW_NUM
     , lpt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_INSURANCE_TYPE lpt JOIN
       (SELECT INSURANCE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_INSURANCE_TYPE
        WHERE  INSURANCE_TYPE_CODE <> N'OTHER') seq  ON seq.INSURANCE_TYPE_CODE = lpt.INSURANCE_TYPE_CODE
WHERE  lpt.INSURANCE_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = N'OTHER'
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
