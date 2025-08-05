/* -----------------------------------------------------------------------------
Perform a late rebuild of selected foreign key indices.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-08  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_MASTST_PIM_MGMTAC_FK
PRINT N'Create foreign key constraint dbo.PIM_MASTST_PIM_MGMTAC_FK'
GO
ALTER TABLE [dbo].[PIMS_MANAGEMENT_ACTIVITY]
	ADD CONSTRAINT [PIM_MASTST_PIM_MGMTAC_FK]
	FOREIGN KEY([MGMT_ACTIVITY_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_MGMT_ACTIVITY_STATUS_TYPE]([MGMT_ACTIVITY_STATUS_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_MATYPE_PIM_MGMTAC_FK
PRINT N'Create foreign key constraint dbo.PIM_MATYPE_PIM_MGMTAC_FK'
GO
ALTER TABLE [dbo].[PIMS_MANAGEMENT_ACTIVITY]
	ADD CONSTRAINT [PIM_MATYPE_PIM_MGMTAC_FK]
	FOREIGN KEY([MGMT_ACTIVITY_TYPE_CODE])
	REFERENCES [dbo].[PIMS_MGMT_ACTIVITY_TYPE]([MGMT_ACTIVITY_TYPE_CODE])
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
