-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_LEASE_STAKEHOLDER_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Feb-03  PSP-11108  Added OTHPAY and altered UNK.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable the "OTHPAY" type.
PRINT N'Add/enable the "OTHPAY" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'OTHPAY'

SELECT LEASE_STAKEHOLDER_TYPE_CODE
FROM   PIMS_LEASE_STAKEHOLDER_TYPE
WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_STAKEHOLDER_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_STAKEHOLDER_TYPE (LEASE_STAKEHOLDER_TYPE_CODE, DESCRIPTION, IS_PAYABLE_RELATED)
  VALUES (N'OTHPAY', N'Other', 1);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the "UNK" type.
PRINT N'Alter the "UNK" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UNK'

SELECT LEASE_STAKEHOLDER_TYPE_CODE
FROM   PIMS_LEASE_STAKEHOLDER_TYPE
WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_STAKEHOLDER_TYPE
  SET    DESCRIPTION                = N'Other'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -------------------------------------------------------------------------------------------
-- Update the display order for the non-Other codes by DESCRIPTION
-- -------------------------------------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_STAKEHOLDER_TYPE biz JOIN
       (SELECT LEASE_STAKEHOLDER_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_STAKEHOLDER_TYPE
        WHERE  DESCRIPTION <> N'Other') seq ON seq.LEASE_STAKEHOLDER_TYPE_CODE = biz.LEASE_STAKEHOLDER_TYPE_CODE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -------------------------------------------------------------------------------------------
-- Update the display order for the Other codes
-- -------------------------------------------------------------------------------------------
UPDATE PIMS_LEASE_STAKEHOLDER_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  DESCRIPTION = N'Other'
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
