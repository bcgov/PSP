/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_TYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (1, N'Residential'),
  (2, N'Commercial'),
  (3, N'Industrial'),
  (4, N'Nominal');