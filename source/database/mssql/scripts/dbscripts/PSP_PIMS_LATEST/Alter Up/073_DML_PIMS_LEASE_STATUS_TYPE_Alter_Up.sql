/* -----------------------------------------------------------------------------
Alter data in the PIMS_LEASE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable the "DUPLICATE" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'DUPLICATE'

SELECT LEASE_STATUS_TYPE_CODE
FROM   PIMS_LEASE_STATUS_TYPE
WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_LEASE_STATUS_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(0))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STATUS_TYPE_CODE = @CurrCd;
  END
ELSE
  BEGIN
  INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES
    (N'DUPLICATE',  N'Duplicate',  CONVERT([bit],(0)));
  END
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
