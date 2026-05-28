/* -----------------------------------------------------------------------------
Drop the temporary table to manage the transfer of lease improvements to 
property improvements.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Nov-19  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table.
PRINT N'Drop the temporary table.'
GO
DROP TABLE IF EXISTS dbo.TMP_ACQUISITION_PAYEE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table.
PRINT N'Drop the temporary table.'
GO
DROP TABLE IF EXISTS dbo.TMP_INTEREST_HOLDER
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table.
PRINT N'Drop the temporary table.'
GO
DROP TABLE IF EXISTS dbo.TMP_PROPERTY_IMPROVEMENT
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
