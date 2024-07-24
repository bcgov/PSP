/* -----------------------------------------------------------------------------
Alter the LEASE_LICENSE_TYPE_CODE data in the PIMS_LEASE_LICENSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-12  Initial version per PSP-8505.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the LIOCCUSE type
PRINT N'Update the new LIOCCUSE type'
GO
UPDATE PIMS_LEASE_LICENSE_TYPE
SET    LEASE_LICENSE_TYPE_CODE    = N'OTHER'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_LICENSE_TYPE_CODE = N'LIOCCUSE'
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
