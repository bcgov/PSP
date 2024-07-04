/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROP_MGMT_ACTIVITY_SUBTYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-03  Initial version.  Disable PROPADMIN and restore the 
                           leading space in WATERANDSEWER.
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

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Restore the "WATERANDSEWER" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'WATERANDSEWER'

SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
  SET    DESCRIPTION                = N' Water and Sewer'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_SUBTYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Nullify the display order.
-- --------------------------------------------------------------
UPDATE pmas
SET    pmas.DISPLAY_ORDER              = NULL
     , pmas.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROP_MGMT_ACTIVITY_SUBTYPE pmas
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
