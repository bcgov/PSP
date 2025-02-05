/* -----------------------------------------------------------------------------
Delete the newly superfluous rows from PIMS_COMPENSATION_REQUISITION
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jan-27  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_COMPENSATION_REQUISITION
PRINT N'Alter table dbo.PIMS_COMPENSATION_REQUISITION'
GO
ALTER TABLE [dbo].[PIMS_COMPENSATION_REQUISITION]
	DROP COLUMN IF EXISTS [LEGACY_PAYEE], [EXPROP_NOTICE_SERVED_DT], [EXPROP_VESTING_DT]
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
