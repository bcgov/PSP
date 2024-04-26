/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LAND_ACT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Mar-27  Initial version
Doug Filteau  2024-Apr-15  Disabled GEOTECH and LNDSRVY.
Doug Filteau  2024-Apr-23  Edited GARDENING, LNDSRVY and SIDEWALK, disabled 
                           ENVIRON and LNDSCPVEG.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Updates ------------------------------------------------------
-- --------------------------------------------------------------

-- Update the "GARDENING" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'GARDENING'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Gardening'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "LNDSRVY" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LNDSRVY'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Land Survey Purposes'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "SIDEWALK" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SIDEWALK'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Sidewalks/Landscaping'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Deletions ----------------------------------------------------
-- --------------------------------------------------------------

-- Disable the "ENVIRON" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ENVIRON'

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

-- Disable the "LNDSCPVEG" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LNDSCPVEG'

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

