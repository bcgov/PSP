/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROP_MGMT_ACTIVITY_SUBTYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-03  Initial version.  Add/enable PROPADMIN and remove the 
                           leading space from WATERANDSEWER.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/enable the "PROPADMIN" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'PROPADMIN'

SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_PROP_MGMT_ACTIVITY_SUBTYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, PROP_MGMT_ACTIVITY_SUBTYPE_CODE, DESCRIPTION)
    VALUES
      (N'PROPERTYMGMT', N'PROPADMIN', N'Administration');
ELSE  
  UPDATE PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Correct the "WATERANDSEWER" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'WATERANDSEWER'

SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
  SET    DESCRIPTION                = N'Water and Sewer'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE pmas
SET    pmas.DISPLAY_ORDER              = seq.ROW_NUM
     , pmas.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE pmas JOIN
       (SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE) seq  ON seq.PROP_MGMT_ACTIVITY_SUBTYPE_CODE = pmas.PROP_MGMT_ACTIVITY_SUBTYPE_CODE
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
