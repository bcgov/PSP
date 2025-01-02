-- *****************************************************************************
-- Add the DESCRIPTION back into the PIMS_PROPERTY_ACTIVITY_HIST table.
-- *****************************************************************************


-- Drop the DESCRIPTION column from the PIMS_PROPERTY_ACTIVITY_HIST
PRINT N'Drop the DESCRIPTION column from the PIMS_PROPERTY_ACTIVITY_HIST'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY_ACTIVITY_HIST]
	DROP COLUMN IF EXISTS [DESCRIPTION]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add the DESCRIPTION column to PIMS_PROPERTY_ACTIVITY_HIST
PRINT N'Add the DESCRIPTION column to PIMS_PROPERTY_ACTIVITY_HIST'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY_ACTIVITY_HIST]
  ADD [DESCRIPTION] nvarchar(4000) NULL
GO
