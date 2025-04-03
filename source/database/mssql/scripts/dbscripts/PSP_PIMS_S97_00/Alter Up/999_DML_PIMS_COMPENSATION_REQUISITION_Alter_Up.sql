/* -----------------------------------------------------------------------------
Alter the PIMS_COMPENSATION_REQUISITION table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-24  Initial version.
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
	DROP COLUMN IF EXISTS [ACQUISITION_OWNER_ID], [INTEREST_HOLDER_ID], [ACQUISITION_FILE_TEAM_ID]
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
