SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop dynamically-named default constraints
PRINT N'Drop dynamically-named default constraints'
GO
DECLARE @sqlQry  VARCHAR(1000)
DECLARE @defName VARCHAR(100)
SET @defName = (SELECT obj.NAME
                FROM   SYSOBJECTS obj                          INNER JOIN
                       SYSCOLUMNS col on obj.ID = col.CDEFAULT INNER JOIN
                       SYSOBJECTS tbl on col.ID = tbl.ID
                WHERE  obj.XTYPE = 'D'
                   AND tbl.NAME = 'PIMS_AGREEMENT_HIST' 
                   AND col.NAME = 'AGREEMENT_STATUS_TYPE_CODE')
SET @sqlQry = 'ALTER TABLE [dbo].[PIMS_AGREEMENT_HIST] DROP CONSTRAINT IF EXISTS [' + @defName + ']'
EXEC (@sqlQry)
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

