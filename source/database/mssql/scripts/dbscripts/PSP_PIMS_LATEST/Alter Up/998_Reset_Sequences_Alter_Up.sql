/* -----------------------------------------------------------------------------
Reset the sequence generators.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-11  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Reset the PIMS_COMP_REQ_LEASE_PAYEE_ID_SEQ sequence generator
PRINT N'Reset the PIMS_COMP_REQ_LEASE_PAYEE_ID_SEQ sequence generator'
GO

DECLARE @StartVlu bigint;
DECLARE @Qry nvarchar(max);

SET @StartVlu = (SELECT MAX(COMP_REQ_LEASE_PAYEE_ID) + 1 FROM PIMS_COMP_REQ_LEASE_PAYEE)
SET @Qry      = 'ALTER SEQUENCE PIMS_COMP_REQ_LEASE_PAYEE_ID_SEQ RESTART WITH ' + CAST(@StartVlu AS NVARCHAR(20)) + ';'

EXEC SP_EXECUTESQL @Qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Reset the PIMS_COMP_REQ_ACQ_PAYEE_ID_SEQ sequence generator
PRINT N'Reset the PIMS_COMP_REQ_ACQ_PAYEE_ID_SEQ sequence generator'
GO

DECLARE @StartVlu bigint;
DECLARE @Qry nvarchar(max);

SET @StartVlu = (SELECT MAX(COMP_REQ_ACQ_PAYEE_ID) + 1 FROM PIMS_COMP_REQ_ACQ_PAYEE)
SET @Qry      = 'ALTER SEQUENCE PIMS_COMP_REQ_ACQ_PAYEE_ID_SEQ RESTART WITH ' + CAST(@StartVlu AS NVARCHAR(20)) + ';'

EXEC SP_EXECUTESQL @Qry;
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
