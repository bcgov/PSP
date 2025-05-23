-- Script generated by Aqua Data Studio Schema Synchronization for MS SQL Server 2016 on Tue Mar 04 11:53:20 PST 2025
-- Execute this script on:
-- 		PSP_PIMS_S100_00/dbo - This database/schema will be modified
-- to synchronize it with MS SQL Server 2016:
-- 		PSP_PIMS_S101_00/dbo

-- We recommend backing up the database prior to executing the script.

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_FORM_TYPE
PRINT N'Alter table dbo.PIMS_FORM_TYPE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Foreign key to the PIMS_DOCUMENT table.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'DOCUMENT_ID'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'CONCURRENCY_CONTROL_NUMBER'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'The date and time the record was created.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'DB_CREATE_TIMESTAMP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'The user or proxy account that created the record.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'DB_CREATE_USERID'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'The date and time the record was created or last updated.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'DB_LAST_UPDATE_TIMESTAMP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'The user or proxy account that created or last updated the record.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_FORM_TYPE', 
	@level2type = N'Column', @level2name = N'DB_LAST_UPDATE_USERID'
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
