-- *****************************************************************************
-- Add the LEASE_CATEGORY_TYPE_CODE and LEASE_PURPOSE_TYPE_CODE back into the
-- PIMS_LEASE_HIST table.
-- *****************************************************************************

-- Alter table dbo.PIMS_LEASE_HIST
PRINT N'Alter table dbo.PIMS_LEASE_HIST'
GO
ALTER TABLE [dbo].[PIMS_LEASE_HIST]
  ADD [LEASE_PURPOSE_TYPE_CODE]   nvarchar(20)  NULL,
      [LEASE_PURPOSE_OTHER_DESC]  nvarchar(200) NULL,
      [LEASE_CATEGORY_TYPE_CODE]  nvarchar(20)  NULL,
      [LEASE_CATEGORY_OTHER_DESC] nvarchar(200) NULL
GO
