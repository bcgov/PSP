/* -----------------------------------------------------------------------------
Generate the foreign keys to PIMS_CHKLST_ITEM_STATUS_TYPE
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Sep-06  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_OUTCMT_PIM_LESCON_FK
PRINT N'Create foreign key constraint dbo.PIM_OUTCMT_PIM_LESCON_FK'
GO
ALTER TABLE [dbo].[PIMS_LEASE_CONSULTATION]
	ADD CONSTRAINT [PIM_OUTCMT_PIM_LESCON_FK]
	FOREIGN KEY([CONSULTATION_OUTCOME_TYPE_CODE])
	REFERENCES [dbo].[PIMS_CONSULTATION_OUTCOME_TYPE]([CONSULTATION_OUTCOME_TYPE_CODE])
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
