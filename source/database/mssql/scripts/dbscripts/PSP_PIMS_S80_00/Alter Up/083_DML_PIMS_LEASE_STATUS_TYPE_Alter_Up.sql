/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-02  Initial version.  Enable/insert "Archived", rename
                           "Discarded" to "Cancelled" and "Inactive" to "Hold"
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "DISCARD" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'DISCARD'

SELECT LEASE_STATUS_TYPE_CODE
FROM   PIMS_LEASE_STATUS_TYPE
WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_STATUS_TYPE
  SET    DESCRIPTION                = N'Cancelled'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "INACTIVE" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'INACTIVE'

SELECT LEASE_STATUS_TYPE_CODE
FROM   PIMS_LEASE_STATUS_TYPE
WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_STATUS_TYPE
  SET    DESCRIPTION                = N'Hold'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable or insert the "ARCHIVED" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'ARCHIVED'

SELECT LEASE_STATUS_TYPE_CODE
FROM   PIMS_LEASE_STATUS_TYPE
WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_STATUS_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES
      (N'ARCHIVED',   N'Archived',   CONVERT([bit],(0)));
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

