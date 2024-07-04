/* -----------------------------------------------------------------------------
Generate the foreign keys to various checklist item statuses.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-03  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_ACQCST_PIM_ACQCKI_FK
PRINT N'Create foreign key constraint dbo.PIM_ACQCST_PIM_ACQCKI_FK'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_CHECKLIST_ITEM]
	ADD CONSTRAINT [PIM_ACQCST_PIM_ACQCKI_FK]
	FOREIGN KEY([ACQ_CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_ACQ_CHKLST_ITEM_STATUS_TYPE]([ACQ_CHKLST_ITEM_STATUS_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_DSPCST_PIM_DSPCKI_FK
PRINT N'Create foreign key constraint dbo.PIM_DSPCST_PIM_DSPCKI_FK'
GO
ALTER TABLE [dbo].[PIMS_DISPOSITION_CHECKLIST_ITEM]
	ADD CONSTRAINT [PIM_DSPCST_PIM_DSPCKI_FK]
	FOREIGN KEY([DSP_CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_DSP_CHKLST_ITEM_STATUS_TYPE]([DSP_CHKLST_ITEM_STATUS_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_LCISTY_PIM_LCHKLI_FK
PRINT N'Create foreign key constraint dbo.PIM_LCISTY_PIM_LCHKLI_FK'
GO
ALTER TABLE [dbo].[PIMS_LEASE_CHECKLIST_ITEM]
	ADD CONSTRAINT [PIM_LCISTY_PIM_LCHKLI_FK]
	FOREIGN KEY([LEASE_CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE]([LEASE_CHKLST_ITEM_STATUS_TYPE_CODE])
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
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
