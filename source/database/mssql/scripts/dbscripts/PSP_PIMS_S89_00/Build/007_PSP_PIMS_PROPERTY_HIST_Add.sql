-- *****************************************************************************
-- Add the PROPERTY_MANAGER_ID and PROP_MGMT_ORG_ID back into the
-- PIMS_PROPERTY_HIST table.
-- *****************************************************************************

-- Alter table dbo.PIMS_PROPERTY_HIST
PRINT N'Alter table dbo.PIMS_PROPERTY_HIST'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY_HIST]
  ADD [PROPERTY_MANAGER_ID]               bigint         NULL,
      [PROP_MGMT_ORG_ID]                  bigint         NULL,
      [PROPERTY_CLASSIFICATION_TYPE_CODE] nvarchar(20)   NULL,
      [NAME]                              nvarchar(250)  NULL,
      [DESCRIPTION]                       nvarchar(2000) NULL,
      [IS_OTHER_INTEREST]                 bit            NULL,
      [IS_SENSITIVE]                      bit            NULL,
      [IS_PROVINCIAL_PUBLIC_HWY]          bit            NULL,
      [IS_VISIBLE_TO_OTHER_AGENCIES]      bit            NULL,
      [IS_DISPOSED]                       bit            NULL,
      [IS_PROPERTY_OF_INTEREST]           bit            NULL,
      [ENCUMBRANCE_REASON]                nvarchar(500)  NULL,
      [ZONING]                            nvarchar(50)   NULL,
      [ZONING_POTENTIAL]                  nvarchar(100)  NULL;
GO
