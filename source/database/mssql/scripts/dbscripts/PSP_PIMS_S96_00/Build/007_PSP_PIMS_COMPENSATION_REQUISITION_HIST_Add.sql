-- *****************************************************************************
-- Add the deleted columns back into the PIMS_COMPENSATION_REQUISITION_HIST table
-- *****************************************************************************

-- Alter table dbo.PIMS_COMPENSATION_REQUISITION_HIST
PRINT N'Alter table dbo.PIMS_COMPENSATION_REQUISITION_HIST'
GO
ALTER TABLE [dbo].[PIMS_COMPENSATION_REQUISITION_HIST]
  ADD [ADV_PMT_SERVED_DT] date NULL
GO
