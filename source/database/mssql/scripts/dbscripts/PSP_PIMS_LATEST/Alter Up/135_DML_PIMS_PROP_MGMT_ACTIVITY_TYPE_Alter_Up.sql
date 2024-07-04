/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROP_MGMT_ACTIVITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-03  Initial version.  Add/enable PROPERTYMGMT.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/enable the "PROPERTYMGMT" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'PROPERTYMGMT'

SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_PROP_MGMT_ACTIVITY_TYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'PROPERTYMGMT', N'Property Management');
ELSE  
  UPDATE PIMS_PROP_MGMT_ACTIVITY_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
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
