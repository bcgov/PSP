 /* ----------------------------------------------------------------------------
Alter the data in the PIMS_TAKE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Apr-22  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Remove the take records that were created for a property associated with a 
-- completed acquisition file that does not have an associated take.
PRINT N'Remove the take records that were created'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DELETE
FROM   PIMS_TAKE
WHERE  TAKE_TYPE_CODE                 = N'IMPORTED'
   AND APP_LAST_UPDATE_USER_DIRECTORY = N'PSP-8237'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update take completion date with acquisition completion date where both 
-- acquisition and take are complete and acquisition date is not null.
PRINT N'Update take completion date'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
UPDATE PIMS_TAKE
SET    COMPLETION_DT                  = NULL
     , CONCURRENCY_CONTROL_NUMBER     = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  APP_LAST_UPDATE_USER_DIRECTORY = N'PSP-8237'
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
