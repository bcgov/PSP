/* -----------------------------------------------------------------------------
Alter the PIMS_LEASE_PAY_RVBL_TYPE table.
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

-- Add/Enable the "PYBLMOTI" type.
PRINT N'Alter the "PYBLMOTI" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'PYBLMOTI'

SELECT LEASE_PAY_RVBL_TYPE_CODE
FROM   PIMS_LEASE_PAY_RVBL_TYPE
WHERE  LEASE_PAY_RVBL_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_PAY_RVBL_TYPE
  SET    DESCRIPTION                = N'Payable (MOTT as tenant)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PAY_RVBL_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PAY_RVBL_TYPE biz JOIN
       (SELECT LEASE_PAY_RVBL_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_PAY_RVBL_TYPE) seq  ON seq.LEASE_PAY_RVBL_TYPE_CODE = biz.LEASE_PAY_RVBL_TYPE_CODE;
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
