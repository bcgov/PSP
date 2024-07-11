/* -----------------------------------------------------------------------------
Reset the sequence generator to the maximum current value in the table + 1
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-10  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Set the new sequence value
PRINT N'Set the new sequence value'
GO
DECLARE @StartVlu bigint;
DECLARE @Qry nvarchar(max);

SET @StartVlu = (SELECT MAX(LEASE_PERIOD_ID) + 1 FROM PIMS_LEASE_PERIOD)
SET @Qry      = 'ALTER SEQUENCE PIMS_LEASE_PERIOD_ID_SEQ RESTART WITH ' + CAST(@StartVlu AS NVARCHAR(20)) + ';'
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
