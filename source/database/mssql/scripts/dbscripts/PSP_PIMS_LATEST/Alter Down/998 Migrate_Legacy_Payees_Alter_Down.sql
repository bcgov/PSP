/* -----------------------------------------------------------------------------
Migrate the legacy payees from the PIMS_COMP_REQ_PAYEE table to the 
PIMS_COMPENSATION_REQUISITION table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-29  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the legacy payee from PIMS_COMP_REQ_PAYEE to PIMS_COMPENSATION_REQUISITION
PRINT N'Migrate the legacy payee from PIMS_COMP_REQ_PAYEE to PIMS_COMPENSATION_REQUISITION'
GO
UPDATE req
SET    req.LEGACY_PAYEE               = pay.LEGACY_PAYEE
     , req.CONCURRENCY_CONTROL_NUMBER = req.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_COMPENSATION_REQUISITION req JOIN
       PIMS_COMP_REQ_PAYEE           pay ON pay.COMPENSATION_REQUISITION_ID = req.COMPENSATION_REQUISITION_ID
WHERE  pay.LEGACY_PAYEE IS NOT NULL
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
