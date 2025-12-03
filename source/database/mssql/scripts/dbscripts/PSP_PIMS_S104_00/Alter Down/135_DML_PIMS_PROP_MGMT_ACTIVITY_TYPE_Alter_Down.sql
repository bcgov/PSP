/* -----------------------------------------------------------------------------
Alter the PIMS_PROP_MGMT_ACTIVITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-07  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "CONSULTATION" type
PRINT N'Disable the "CONSULTATION" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CONSULTATION'

SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "TRAILMTC" type
PRINT N'Disable the "TRAILMTC" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'TRAILMTC'

SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE
WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROP_MGMT_ACTIVITY_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROP_MGMT_ACTIVITY_TYPE_CODE = @CurrCd;
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
