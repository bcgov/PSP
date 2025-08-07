/* -----------------------------------------------------------------------------
Alter the PIMS_PROP_MGMT_ACTIVITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-15  Initial version.
Doug Filteau  2025-Jun-27  Added UNKNOWN activity type.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "CORRESPOND" type
PRINT N'Add/Enable the "CORRESPOND" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CORRESPOND'

SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_PROP_MGMT_ACTIVITY_TYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION)
  VALUES (N'CORRESPOND', N'Correspondence');  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
  
-- --------------------------------------------------------------
-- Insert the disabled 'UNKNOWN' activity type.
-- --------------------------------------------------------------
PRINT N'Insert the disabled "UNKNOWN" activity type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UNKNOWN'

SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_PROP_MGMT_ACTIVITY_TYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES
    (N'UNKNOWN', N'Unknown', 1);
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE pmat
SET    pmat.DISPLAY_ORDER              = seq.ROW_NUM
     , pmat.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE pmat JOIN
       (SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE) seq  ON seq.PROP_MGMT_ACTIVITY_TYPE_CODE = pmat.PROP_MGMT_ACTIVITY_TYPE_CODE
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
