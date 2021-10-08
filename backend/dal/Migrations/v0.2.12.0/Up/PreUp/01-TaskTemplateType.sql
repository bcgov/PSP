-- Drop primary key constraint dbo.TSKTMT_PK
PRINT N'Drop primary key constraint dbo.TSKTMT_PK'
GO
ALTER TABLE [dbo].[PIMS_TASK_TEMPLATE_TYPE]
	DROP CONSTRAINT [TSKTMT_PK]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO