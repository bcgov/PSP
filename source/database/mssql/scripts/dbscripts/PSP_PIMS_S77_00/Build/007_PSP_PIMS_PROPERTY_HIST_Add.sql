-- *****************************************************************************
-- Add the PROPERTY_MANAGER_ID and PROP_MGMT_ORG_ID back into the
-- PIMS_PROPERTY_HIST table.
-- *****************************************************************************

-- Alter table dbo.PIMS_PROPERTY_HIST
PRINT N'Alter table dbo.PIMS_PROPERTY_HIST'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY_HIST]
  ADD [PROPERTY_MANAGER_ID]     bigint NULL,
      [PROP_MGMT_ORG_ID]        bigint NULL,
      [IS_DISPOSED]             bit    NULL,
      [IS_PROPERTY_OF_INTEREST] bit    NULL,
      [IS_OTHER_INTEREST]       bit    NULL
GO
