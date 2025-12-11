/* -----------------------------------------------------------------------------
Alter the PIMS_ORGANIZATION table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the Organization Name
PRINT N'Alter the Organization Name'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTI2'

SELECT ORGANIZATION_IDENTIFIER
FROM   PIMS_ORGANIZATION
WHERE  ORGANIZATION_IDENTIFIER = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_ORGANIZATION
  SET    ORGANIZATION_NAME          = N'Ministry of Transportation and Transit'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ORGANIZATION_IDENTIFIER = @CurrCd;
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
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
