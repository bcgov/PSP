/* -----------------------------------------------------------------------------
Drop foreign key from PIMS_ACQUISITION_FILE to PIMS_INTEREST_HOLDER.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-29  Initial version
----------------------------------------------------------------------------- */

ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER]
	DROP CONSTRAINT IF EXISTS [PIM_ACQNFL_PIM_INTHLD_FK]
GO

ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER] 
  DROP COLUMN IF EXISTS [ACQUISITION_FILE_ID]
GO
