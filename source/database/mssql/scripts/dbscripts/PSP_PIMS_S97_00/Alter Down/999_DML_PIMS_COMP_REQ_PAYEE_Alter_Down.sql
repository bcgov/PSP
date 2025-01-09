/* -----------------------------------------------------------------------------
Drop the PIMS_COMP_REQ_PAYEE table.
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

-- Drop table dbo.PIMS_COMP_REQ_PAYEE
PRINT N'Drop table dbo.PIMS_COMP_REQ_PAYEE'
GO
DROP TABLE IF EXISTS [dbo].[PIMS_COMP_REQ_PAYEE]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop sequence dbo.PIMS_COMP_REQ_PAYEE_ID_SEQ
PRINT N'Drop sequence dbo.PIMS_COMP_REQ_PAYEE_ID_SEQ'
GO
DROP SEQUENCE IF EXISTS [dbo].[PIMS_COMP_REQ_PAYEE_ID_SEQ]
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
