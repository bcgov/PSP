-- *****************************************************************************
-- Alter the PIMS_EXPROPRIATION_PAYMENT_HIST table.
-- *****************************************************************************

-- Alter the PIMS_EXPROPRIATION_PAYMENT_HIST table.
PRINT N'Alter the PIMS_EXPROPRIATION_PAYMENT_HIST table.'
GO

ALTER TABLE [dbo].[PIMS_EXPROPRIATION_PAYMENT_HIST]
  ADD [ADV_PMT_SERVED_DT] date NULL
GO
