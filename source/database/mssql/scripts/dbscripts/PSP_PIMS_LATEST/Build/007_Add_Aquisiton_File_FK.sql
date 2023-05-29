/* -----------------------------------------------------------------------------
Add foreign key from PIMS_ACQUISITION_FILE to PIMS_INTEREST_HOLDER.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-29  Initial version
----------------------------------------------------------------------------- */

ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER] 
  ADD [ACQUISITION_FILE_ID] BIGINT NOT NULL
GO

ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER] ADD CONSTRAINT [PIM_ACQNFL_PIM_INTHLD_FK] 
    FOREIGN KEY ([ACQUISITION_FILE_ID]) REFERENCES [dbo].[PIMS_ACQUISITION_FILE] ([ACQUISITION_FILE_ID])
GO
