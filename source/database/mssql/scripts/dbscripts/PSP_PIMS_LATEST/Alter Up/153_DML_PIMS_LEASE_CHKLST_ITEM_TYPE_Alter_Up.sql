/* -----------------------------------------------------------------------------
Alter the PIMS_LEASE_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the RAFIRSTNTN code
PRINT N'Alter the RAFIRSTNTN code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'RAFIRSTNTN'

SELECT LEASE_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_TYPE
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
  SET    HINT                       = N'Required if proposed use could impact aboriginal rights (i.e. long term, non-exclusive use agreement, ground disturbance or tree removal). Verify with MOTT IR if unsure.'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON 
GO

-- Alter the RARGNLPLAN code
PRINT N'Alter the RARGNLPLAN code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'RARGNLPLAN'

SELECT LEASE_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_TYPE
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
  SET    HINT                       = N'Required if proposed use may conflict with future MOTT project or land use'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;
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
