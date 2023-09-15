/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROPERTY_CLASSIFICATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "OTHER" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'OTHER'

SELECT PROPERTY_CLASSIFICATION_TYPE_CODE
FROM   PIMS_PROPERTY_CLASSIFICATION_TYPE
WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_PROPERTY_CLASSIFICATION_TYPE
  WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE = N'OTHER';
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
