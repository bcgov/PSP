/* -----------------------------------------------------------------------------
Create a temporary table to manage the transfer of lease improvements to 
property improvements.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Nov-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_PRPRTY_PIM_PIMPRV_FK
PRINT N'Create foreign key constraint dbo.PIM_PRPRTY_PIM_PIMPRV_FK'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY_IMPROVEMENT]
	ADD CONSTRAINT [PIM_PRPRTY_PIM_PIMPRV_FK]
	FOREIGN KEY([PROPERTY_ID])
	REFERENCES [dbo].[PIMS_PROPERTY]([PROPERTY_ID])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
