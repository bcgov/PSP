/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_LICENSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-11  Initial version.
Doug Filteau  2024-Jul-11  Added LIOCCTELE type.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "LIOCCTELE" type
PRINT N'Disable the "LIOCCTELE" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LIOCCTELE'

SELECT LEASE_LICENSE_TYPE_CODE
FROM   PIMS_LEASE_LICENSE_TYPE
WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_LICENSE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE tbl
SET    tbl.DISPLAY_ORDER              = seq.ROW_NUM
     , tbl.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_LICENSE_TYPE tbl JOIN
       (SELECT LEASE_LICENSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_LICENSE_TYPE) seq  ON seq.LEASE_LICENSE_TYPE_CODE = tbl.LEASE_LICENSE_TYPE_CODE
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
