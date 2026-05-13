-- *****************************************************************************
-- Add the deleted columns back into the PIMS_COMPENSATION_REQUISITION_HIST table
-- *****************************************************************************

-- Alter table dbo.PIMS_COMPENSATION_REQUISITION_HIST
PRINT N'Alter table dbo.PIMS_COMPENSATION_REQUISITION_HIST'
GO
ALTER TABLE [dbo].[PIMS_COMPENSATION_REQUISITION_HIST]
  ADD [ADV_PMT_SERVED_DT]        date           NULL,
      [ACQUISITION_OWNER_ID]     bigint         NULL,
      [INTEREST_HOLDER_ID]       bigint         NULL,
      [ACQUISITION_FILE_TEAM_ID] bigint         NULL,
      [EXPROP_NOTICE_SERVED_DT]  date           NULL,
      [EXPROP_VESTING_DT]        date           NULL,
      [LEGACY_PAYEE]             nvarchar(1000) NULL
GO
