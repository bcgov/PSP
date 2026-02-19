-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_ACQUISITION_FILE and PIMS_ACQUISITION_FILE_HIST table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Feb-17  PSP-11224  Alter the FILE_NO and FILE_NO_SUFFIX columns.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the default constraint on FILE_NO.  
PRINT N'Drop the default constraint on FILE_NO.'
GO
DECLARE @sqlQry  VARCHAR(1000)
DECLARE @defName VARCHAR(100)
SET @defName = (SELECT obj.NAME
                FROM   SYSOBJECTS obj                          INNER JOIN
                       SYSCOLUMNS col on obj.ID = col.CDEFAULT INNER JOIN
                       SYSOBJECTS tbl on col.ID = tbl.ID
                WHERE  obj.XTYPE = 'D'
                   AND tbl.NAME = 'PIMS_ACQUISITION_FILE' 
                   AND col.NAME = 'FILE_NO')
SET @sqlQry = 'ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE] DROP CONSTRAINT IF EXISTS [' + @defName + ']'
EXEC (@sqlQry)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the default constraint on FILE_NO_SUFFIX.  
PRINT N'Drop the default constraint on FILE_NO_SUFFIX.'
GO
DECLARE @sqlQry  VARCHAR(1000)
DECLARE @defName VARCHAR(100)
SET @defName = (SELECT obj.NAME
                FROM   SYSOBJECTS obj                          INNER JOIN
                       SYSCOLUMNS col on obj.ID = col.CDEFAULT INNER JOIN
                       SYSOBJECTS tbl on col.ID = tbl.ID
                WHERE  obj.XTYPE = 'D'
                   AND tbl.NAME = 'PIMS_ACQUISITION_FILE' 
                   AND col.NAME = 'FILE_NO_SUFFIX')
SET @sqlQry = 'ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE] DROP CONSTRAINT IF EXISTS [' + @defName + ']'
EXEC (@sqlQry)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Make the FILE_NO column nullable ON PIMS_ACQUISITION_FILE.  
PRINT N'Make the FILE_NO column nullable ON PIMS_ACQUISITION_FILE.'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE]
ALTER COLUMN FILE_NO INTEGER NULL;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Make the FILE_NO_SUFFIX column nullable ON PIMS_ACQUISITION_FILE.  
PRINT N'Make the FILE_NO_SUFFIX column nullable ON PIMS_ACQUISITION_FILE.'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE]
ALTER COLUMN FILE_NO_SUFFIX SMALLINT NULL;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Make the FILE_NO column nullable ON PIMS_ACQUISITION_FILE_HIST.  
PRINT N'Make the FILE_NO column nullable ON PIMS_ACQUISITION_FILE_HIST.'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE_HIST]
ALTER COLUMN FILE_NO INTEGER NULL;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Make the FILE_NO_SUFFIX column nullable ON PIMS_ACQUISITION_FILE_HIST.  
PRINT N'Make the FILE_NO_SUFFIX column nullable ON PIMS_ACQUISITION_FILE_HIST.'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE_HIST]
ALTER COLUMN FILE_NO_SUFFIX SMALLINT NULL;
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
