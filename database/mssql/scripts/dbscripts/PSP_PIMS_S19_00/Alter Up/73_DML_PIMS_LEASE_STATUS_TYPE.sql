/* -----------------------------------------------------------------------------
Disable the EXPIRED code and insert the INACTIVE code into the
PIMS_LEASE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

UPDATE PIMS_LEASE_STATUS_TYPE
SET    IS_DISABLED = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_STATUS_TYPE_CODE = 'EXPIRED';
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'INACTIVE',    N'Inactive');
GO
