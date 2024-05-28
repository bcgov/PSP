/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_PURPOSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Mar-27  Initial version
Doug Filteau  2024-Apr-15  Added GEOTECH and LNDSRVY.
Doug Filteau  2024-Apr-23  Edited GARDENING, LNDSRVY and SIDEWALK, added ENVIRON
                           and LNDSCPVEG.
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

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'GARDENING', N'Garden');
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Garden'
       , IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "LNDSRVY" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LNDSRVY'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'LNDSRVY', N'Land Survey');
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Land Survey'
       , IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "SIDEWALK" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SIDEWALK'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'SIDEWALK', N'Sidewalks');
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    DESCRIPTION                = N'Sidewalks'
       , IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Inserts ------------------------------------------------------
-- --------------------------------------------------------------

-- Insert the "ENVIRON" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ENVIRON'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'ENVIRON', N'Environmental');
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON

-- Insert the "LNDSCPVEG" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LNDSCPVEG'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'LNDSCPVEG', N'Landscaping/Vegetation Clearing');
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE lpt
SET    lpt.DISPLAY_ORDER              = seq.ROW_NUM
     , lpt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PURPOSE_TYPE lpt JOIN
       (SELECT LEASE_PURPOSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_PURPOSE_TYPE
        WHERE  LEASE_PURPOSE_TYPE_CODE <> N'OTHER') seq  ON seq.LEASE_PURPOSE_TYPE_CODE = lpt.LEASE_PURPOSE_TYPE_CODE
WHERE  lpt.LEASE_PURPOSE_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_LEASE_PURPOSE_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PURPOSE_TYPE_CODE = N'OTHER'
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

