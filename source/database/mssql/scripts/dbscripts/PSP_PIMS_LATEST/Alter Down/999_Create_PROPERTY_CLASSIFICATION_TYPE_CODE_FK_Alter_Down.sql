/* -----------------------------------------------------------------------------
Alter the PROPERTY_CLASSIFICATION_TYPE_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Aug-13  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_PRPCLT_PIM_PRPRTY_FK
PRINT N'Create foreign key constraint dbo.PIM_PRPCLT_PIM_PRPRTY_FK'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY]
	ADD CONSTRAINT [PIM_PRPCLT_PIM_PRPRTY_FK]
	FOREIGN KEY([PROPERTY_CLASSIFICATION_TYPE_CODE])
	REFERENCES [dbo].[PIMS_PROPERTY_CLASSIFICATION_TYPE]([PROPERTY_CLASSIFICATION_TYPE_CODE])
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
