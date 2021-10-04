/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'I', N'Unknown'),
  (N'L', N'Unknown'),
  (N'LI', N'Unknown'),
  (N'R', N'Unknown'),
  (N'RW', N'Unknown'),
  (N'U', N'Unknown'),
  (N'V', N'Unknown');