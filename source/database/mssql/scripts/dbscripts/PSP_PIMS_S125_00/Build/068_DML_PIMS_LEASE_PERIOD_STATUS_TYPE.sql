/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PERIOD_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2021-Jun-18  Renamed table and PK column from TERM to PERIOD.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PERIOD_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_PERIOD_STATUS_TYPE (LEASE_PERIOD_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'EXER',  N'Exercised'),
  (N'NEXER', N'Not Exercised');