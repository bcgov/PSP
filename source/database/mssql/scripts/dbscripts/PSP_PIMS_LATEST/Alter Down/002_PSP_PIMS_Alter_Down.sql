-- Script generated by Aqua Data Studio Schema Synchronization for MS SQL Server 2016 on Tue Aug 29 16:32:01 PDT 2023
-- Execute this script on:
-- 		PSP_PIMS_S62_00/dbo - This database/schema will be modified
-- to synchronize it with MS SQL Server 2016:
-- 		PSP_PIMS_S61_00/dbo

-- We recommend backing up the database prior to executing the script.

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create table dbo.TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
PRINT N'Create table dbo.TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE'
GO
CREATE TABLE [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE]  ( 
	[PROPERTY_ID]                     	bigint NULL,
	[PROPERTY_ADJACENT_LAND_TYPE_CODE]	nvarchar(50) NULL 
	)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_INTEREST_HOLDER_HIST
PRINT N'Alter table dbo.PIMS_INTEREST_HOLDER_HIST'
GO
ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER_HIST] ADD DEFAULT (N'INTHLDR') FOR [INTEREST_HOLDER_TYPE_CODE]
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