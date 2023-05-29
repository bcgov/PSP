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
                   AND tbl.NAME = 'PIMS_COMP_REQ_H120' 
                   AND col.NAME = 'FINANCIAL_ACTIVITY_CODE_ID')
SET @sqlQry = 'ALTER TABLE [dbo].[PIMS_COMP_REQ_H120] DROP CONSTRAINT IF EXISTS [' + @defName + ']'
EXEC (@sqlQry) 
IF @@ERROR <> 0 SET NOEXEC ON
GO

DECLARE @sqlQry  VARCHAR(1000)
DECLARE @defName VARCHAR(100)
SET @defName = (SELECT obj.NAME
                FROM   SYSOBJECTS obj                          INNER JOIN
                       SYSCOLUMNS col on obj.ID = col.CDEFAULT INNER JOIN
                       SYSOBJECTS tbl on col.ID = tbl.ID
                WHERE  obj.XTYPE = 'D'
                   AND tbl.NAME = 'PIMS_COMP_REQ_H120_HIST' 
                   AND col.NAME = 'FINANCIAL_ACTIVITY_CODE_ID')
SET @sqlQry = 'ALTER TABLE [dbo].[PIMS_COMP_REQ_H120] DROP CONSTRAINT IF EXISTS [' + @defName + ']'
EXEC (@sqlQry) 
IF @@ERROR <> 0 SET NOEXEC ON
GO
