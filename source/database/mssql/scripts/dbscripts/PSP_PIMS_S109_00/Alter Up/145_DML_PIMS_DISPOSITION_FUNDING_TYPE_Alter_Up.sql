/* -----------------------------------------------------------------------------
Alter the PIMS_DISPOSITION_FUNDING_TYPE table.
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

-- Alter the MOTIDIS code
PRINT N'Alter the MOTIDIS code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTIDIS'

SELECT DISPOSITION_FUNDING_TYPE_CODE
FROM   PIMS_DISPOSITION_FUNDING_TYPE
WHERE  DISPOSITION_FUNDING_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DISPOSITION_FUNDING_TYPE
  SET    DESCRIPTION                = N'MoTT District'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DISPOSITION_FUNDING_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO

-- Alter the MOTIREG code
PRINT N'Alter the MOTIREG code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTIREG'

SELECT DISPOSITION_FUNDING_TYPE_CODE
FROM   PIMS_DISPOSITION_FUNDING_TYPE
WHERE  DISPOSITION_FUNDING_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DISPOSITION_FUNDING_TYPE
  SET    DESCRIPTION                = N'MoTT Region'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DISPOSITION_FUNDING_TYPE_CODE = @CurrCd;
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
