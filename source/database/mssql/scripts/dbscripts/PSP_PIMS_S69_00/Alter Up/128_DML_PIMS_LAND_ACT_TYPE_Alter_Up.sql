/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LAND_ACT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Dec-14  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "Crown Grant" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'Crown Grant'

SELECT LAND_ACT_TYPE_CODE
FROM   PIMS_LAND_ACT_TYPE
WHERE  LAND_ACT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_LAND_ACT_TYPE (LAND_ACT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
    VALUES
      (N'Crown Grant', N'Crown Grant', 6);
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
