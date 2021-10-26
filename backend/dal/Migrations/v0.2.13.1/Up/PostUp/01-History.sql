PRINT N'Create history tables'
GO

-- Create table dbo.PIMS_PROPERTY_IMPROVEMENT_HIST
PRINT N'Create table dbo.PIMS_PROPERTY_IMPROVEMENT_HIST'
GO
CREATE TABLE [dbo].[PIMS_PROPERTY_IMPROVEMENT_HIST]  ( 
	[_PROPERTY_IMPROVEMENT_HIST_ID]     	bigint NOT NULL DEFAULT (NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ]),
	[APP_CREATE_TIMESTAMP]              	datetime NOT NULL,
	[APP_CREATE_USER_DIRECTORY]         	nvarchar(30) NOT NULL,
	[APP_CREATE_USER_GUID]              	uniqueidentifier NULL,
	[APP_CREATE_USERID]                 	nvarchar(30) NOT NULL,
	[APP_LAST_UPDATE_TIMESTAMP]         	datetime NOT NULL,
	[APP_LAST_UPDATE_USER_DIRECTORY]    	nvarchar(30) NOT NULL,
	[APP_LAST_UPDATE_USER_GUID]         	uniqueidentifier NULL,
	[APP_LAST_UPDATE_USERID]            	nvarchar(30) NOT NULL,
	[CONCURRENCY_CONTROL_NUMBER]        	bigint NOT NULL,
	[DB_CREATE_TIMESTAMP]               	datetime NOT NULL,
	[DB_CREATE_USERID]                  	nvarchar(30) NOT NULL,
	[DB_LAST_UPDATE_TIMESTAMP]          	datetime NOT NULL,
	[DB_LAST_UPDATE_USERID]             	nvarchar(30) NOT NULL,
	[EFFECTIVE_DATE_HIST]               	datetime NOT NULL DEFAULT (getutcdate()),
	[END_DATE_HIST]                     	datetime NULL,
	[IMPROVEMENT_DESCRIPTION]           	nvarchar(2000) NOT NULL,
	[PROPERTY_IMPROVEMENT_ID]           	bigint NOT NULL,
	[PROPERTY_IMPROVEMENT_TYPE_CODE]    	nvarchar(20) NOT NULL,
	[PROPERTY_LEASE_ID]                 	bigint NOT NULL,
	[STRUCTURE_SIZE]                    	nvarchar(2000) NULL,
	[UNIT]           											nvarchar(2000) NULL,
	PRIMARY KEY CLUSTERED([_PROPERTY_IMPROVEMENT_HIST_ID])
 ON [PRIMARY])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_LEASE_HIST
PRINT N'Alter table dbo.PIMS_LEASE_HIST'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Number of months included in lease renewal' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_HIST', 
	@level2type = N'Column', @level2name = N'RENEWAL_TERM_MONTHS'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[PIMS_LEASE_HIST]
	ADD [LEASE_INITIATOR_TYPE_CODE] nvarchar(20), 
	[LEASE_RESPONSIBILITY_TYPE_CODE] nvarchar(20), 
	[RESPONSIBILITY_EFFECTIVE_DATE] datetime NULL, 
	[IS_SUBJECT_TO_RTA] bit NULL, 
	[IS_COMM_BLDG] bit NULL, 
	[IS_OTHER_IMPROVEMENT] bit NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[PIMS_LEASE_HIST]
	DROP COLUMN [TERM_START_DATE], [TERM_EXPIRY_DATE], [TERM_RENEWAL_DATE]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_LEASE_TENANT_HIST
PRINT N'Alter table dbo.PIMS_LEASE_TENANT_HIST'
GO
ALTER TABLE [dbo].[PIMS_LEASE_TENANT_HIST]
	ADD [NOTE] nvarchar(2000) NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
