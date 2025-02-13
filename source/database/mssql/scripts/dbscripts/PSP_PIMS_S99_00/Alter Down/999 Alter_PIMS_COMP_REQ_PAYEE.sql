/* -----------------------------------------------------------------------------
Alter PIMS_COMP_REQ_PAYEE
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jan-29  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop index dbo.CMPRQP_ACQUISITION_OWNER_ID_IDX
PRINT N'Drop index dbo.CMPRQP_ACQUISITION_OWNER_ID_IDX'
GO
DROP INDEX IF EXISTS [dbo].[PIMS_COMP_REQ_PAYEE].[CMPRQP_ACQUISITION_OWNER_ID_IDX]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_COMP_REQ_PAYEE
PRINT N'Alter table dbo.PIMS_COMP_REQ_PAYEE'
GO
ALTER TABLE [dbo].[PIMS_COMP_REQ_PAYEE]
	DROP COLUMN IF EXISTS [LEGACY_PAYEE]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_COMP_REQ_PAYEE_HIST
PRINT N'Alter table dbo.PIMS_COMP_REQ_PAYEE_HIST'
GO
ALTER TABLE [dbo].[PIMS_COMP_REQ_PAYEE_HIST]
	DROP COLUMN IF EXISTS [LEGACY_PAYEE]
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
