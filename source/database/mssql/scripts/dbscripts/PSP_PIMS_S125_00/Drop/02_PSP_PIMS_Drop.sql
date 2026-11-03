/* -----------------------------------------------------------------------------
Drop all objects in the database.  Adding @prmDebugMode=1 to the parameters will 
print the created SQL instead of executing it.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Oct-29  Original version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove all objects associated with the etl schema
PRINT N'Remove all objects associated with the etl schema'
GO
EXECUTE dbo.PIM_DROP_SCHEMA @prmSchemaNm='etl' --, @prmDebugMode=0
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove all objects associated with the pmbc schema
PRINT N'Remove all objects associated with the pmbc schema'
GO
EXECUTE dbo.PIM_DROP_SCHEMA @prmSchemaNm='pmbc' --, @prmDebugMode=0
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove all objects associated with the dbo schema
PRINT N'Remove all objects associated with dbo pmbc schema'
GO
EXECUTE dbo.PIM_DROP_SCHEMA @prmSchemaNm='dbo' --, @prmDebugMode=0
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

