/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LAND_ACT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Mar-27  Initial version
Doug Filteau  2024-Apr-15  Disabled GEOTECH and LNDSRVY.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "GEOTECH" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'GEOTECH'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "LNDSRVY" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LNDSRVY'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
  END
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

