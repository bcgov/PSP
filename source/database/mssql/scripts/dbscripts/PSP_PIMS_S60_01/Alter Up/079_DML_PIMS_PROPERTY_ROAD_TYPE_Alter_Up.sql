/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROPERTY_ROAD_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-22  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "BYLAW" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'BYLAW'

SELECT PROPERTY_ROAD_TYPE_CODE
FROM   PIMS_PROPERTY_ROAD_TYPE
WHERE  PROPERTY_ROAD_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_ROAD_TYPE (PROPERTY_ROAD_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'BYLAW', N'Bylaw');
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
