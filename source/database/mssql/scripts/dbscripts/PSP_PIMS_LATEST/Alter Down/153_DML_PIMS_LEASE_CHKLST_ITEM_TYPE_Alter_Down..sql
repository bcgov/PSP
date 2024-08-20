/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Aug-09  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable the "REFERAPPR" section items
PRINT N'Enable the "REFERAPPR" section items'
GO
UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    IS_DISABLED                = 0
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = N'REFERAPPR';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "APPRCOND" section types
PRINT N'Disable the "APPRCOND" section types'
GO
UPDATE PIMS_LEASE_CHKLST_SECTION_TYPE
SET    IS_DISABLED                = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = N'APPRCOND';
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
