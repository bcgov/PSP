/* -----------------------------------------------------------------------------
Migrate the data for the compensation requisition payees.
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

-- Migrate the data for the compensation requisition payees
PRINT N'Migrate the data for the compensation requisition payees'
GO
UPDATE req
SET    req.ACQUISITION_OWNER_ID       = pay.ACQUISITION_OWNER_ID
     , req.INTEREST_HOLDER_ID         = pay.INTEREST_HOLDER_ID
     , req.ACQUISITION_FILE_TEAM_ID   = pay.ACQUISITION_FILE_TEAM_ID
     , req.CONCURRENCY_CONTROL_NUMBER = req.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_COMP_REQ_PAYEE           pay JOIN
       PIMS_COMPENSATION_REQUISITION req ON req.COMPENSATION_REQUISITION_ID = pay.COMPENSATION_REQUISITION_ID
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
