/* -----------------------------------------------------------------------------
Enable the EXPIRED code and delete the INACTIVE code from the
PIMS_LEASE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

UPDATE PIMS_LEASE_STATUS_TYPE
SET    IS_DISABLED = CONVERT([bit],(0))
WHERE  LEASE_STATUS_TYPE_CODE = 'EXPIRED';
GO

DELETE
FROM   PIMS_LEASE_STATUS_TYPE
WHERE  LEASE_STATUS_TYPE_CODE = 'INACTIVE';
GO
