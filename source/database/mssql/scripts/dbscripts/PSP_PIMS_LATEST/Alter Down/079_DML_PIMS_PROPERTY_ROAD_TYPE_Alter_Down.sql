/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROPERTY_ROAD_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-19  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete multiple types
DELETE
FROM   PIMS_PROPERTY_ROAD_TYPE
WHERE  PROPERTY_ROAD_TYPE_CODE IN (N'CMMNLAW', N'S107PLN')
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "GAZMOTI" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'GAZMOTI'

SELECT PROPERTY_ROAD_TYPE_CODE
FROM   PIMS_PROPERTY_ROAD_TYPE
WHERE  PROPERTY_ROAD_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_PROPERTY_ROAD_TYPE
  SET    DESCRIPTION                = N'Gazetted (MoTI Plan)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_ROAD_TYPE_CODE = N'GAZMOTI';
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable multiple types
UPDATE PIMS_PROPERTY_ROAD_TYPE
SET    IS_DISABLED                = CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_ROAD_TYPE_CODE IN (N'GAZSURVD', N'GAZUNSURVD', N'107REF', N'107EXP');
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
