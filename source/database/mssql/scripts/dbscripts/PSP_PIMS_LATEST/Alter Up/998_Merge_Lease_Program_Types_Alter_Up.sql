/* -----------------------------------------------------------------------------
Update the PIMS_LEASE table.  Merge BC Ferries, BC Transit, and Belleville 
Terminal Lease Purpose types into a single value 'Public Transportation'
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Nov-10  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the PIMS_LEASE table.
PRINT N'Update the PIMS_LEASE table.'
GO
UPDATE PIMS_LEASE
SET    LEASE_PROGRAM_TYPE_CODE    = N'PUBTRANS'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PROGRAM_TYPE_CODE IN (N'BCFERRIES', N'BCTRANSIT', N'BELLETERM');
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
