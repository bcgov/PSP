-- *****************************************************************************
-- Add the COMPLETION_DATE column back into the PIMS_ACQUISITION_FILE_HIST table
-- *****************************************************************************

-- Alter table dbo.PIMS_ACQUISITION_FILE_HIST
PRINT N'Alter table dbo.PIMS_ACQUISITION_FILE_HIST'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_FILE_HIST]
  ADD [COMPLETION_DATE] datetime     NULL,
      [FILE_NUMBER]     nvarchar(18) NULL
GO
