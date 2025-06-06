/* -----------------------------------------------------------------------------
Alter the PIMS_PROP_MGMT_ACTIVITY_SUBTYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-15  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "CORRESPOND" type
PRINT N'Disable the "CORRESPOND" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CORRESPOND'

SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE    = N'CORRESPOND'
   AND PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE    = N'CORRESPOND'
     AND PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;
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
