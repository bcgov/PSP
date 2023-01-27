/* -----------------------------------------------------------------------------
Disable EXPIRED code and insert/enable in the PIMS_LEASE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Feb-04  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

-- Disable the Expired code value
UPDATE PIMS_LEASE_STATUS_TYPE
SET    IS_DISABLED                = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_STATUS_TYPE_CODE     = N'EXPIRED';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES (N'EXPIRED', N'Expired', CONVERT([bit],(1)));
  END

-- Enabled the Discarded code value
UPDATE PIMS_LEASE_STATUS_TYPE
SET    IS_DISABLED                = CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_STATUS_TYPE_CODE     = N'DISCARD';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES (N'DISCARD', N'Discarded', CONVERT([bit],(0)));
  END

COMMIT TRANSACTION;
