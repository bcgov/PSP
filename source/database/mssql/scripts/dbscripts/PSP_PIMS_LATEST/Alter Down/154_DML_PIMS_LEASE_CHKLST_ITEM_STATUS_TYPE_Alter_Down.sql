/* -----------------------------------------------------------------------------
Alter the PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jun-14  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- ----------------------------------------------------------------
-- Enable/insert the NA code in the PIMS_LEASE_CHECKLIST_ITEM table
-- ----------------------------------------------------------------

DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'NA'

SELECT LEASE_CHKLST_ITEM_STATUS_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE
WHERE  LEASE_CHKLST_ITEM_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE (LEASE_CHKLST_ITEM_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
    VALUES
      (N'NA', N'Not applicable', 3);
ELSE
  UPDATE PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_ITEM_STATUS_TYPE_CODE = N'NA';  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------------
-- Update the PIMS_LEASE_CHECKLIST_ITEMs that reference the NOTAPP code
-- --------------------------------------------------------------------
UPDATE PIMS_LEASE_CHECKLIST_ITEM
SET    LEASE_CHKLST_ITEM_STATUS_TYPE_CODE = N'NA'
     , CONCURRENCY_CONTROL_NUMBER         = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_CHKLST_ITEM_STATUS_TYPE_CODE = N'NOTAPP';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Disable the NOTAPP code in the PIMS_LEASE_CHECKLIST_ITEM table
-- --------------------------------------------------------------
UPDATE PIMS_LEASE_CHKLST_ITEM_STATUS_TYPE
SET    IS_DISABLED                = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_CHKLST_ITEM_STATUS_TYPE_CODE = N'NOTAPP'
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
