/* -----------------------------------------------------------------------------
Generate the foreign keys to PIMS_CHKLST_ITEM_STATUS_TYPE
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

-- Create foreign key constraint dbo.PIM_CHKLIS_PIM_ACQCKI_FK
PRINT N'Create foreign key constraint dbo.PIM_CHKLIS_PIM_ACQCKI_FK'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_CHECKLIST_ITEM]
	ADD CONSTRAINT [PIM_CHKLIS_PIM_ACQCKI_FK]
	FOREIGN KEY([CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_CHKLST_ITEM_STATUS_TYPE]([CHKLST_ITEM_STATUS_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_CHKLIS_PIM_DSPCKI_FK
PRINT N'Create foreign key constraint dbo.PIM_CHKLIS_PIM_DSPCKI_FK'
GO
ALTER TABLE [dbo].[PIMS_DISPOSITION_CHECKLIST_ITEM]
	ADD CONSTRAINT [PIM_CHKLIS_PIM_DSPCKI_FK]
	FOREIGN KEY([CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_CHKLST_ITEM_STATUS_TYPE]([CHKLST_ITEM_STATUS_TYPE_CODE])
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
	FOREIGN KEY([CHKLST_ITEM_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_CHKLST_ITEM_STATUS_TYPE]([CHKLST_ITEM_STATUS_TYPE_CODE])
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
