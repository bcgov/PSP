/* -----------------------------------------------------------------------------
Delete all data from the PIMS_REGION table and repopulate.

*** NOTE ***
The PIMS_DISTRICT table must be empty due to a foreign key dependency.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_REGION;
GO

INSERT INTO PIMS_REGION (REGION_CODE, REGION_NAME) 
VALUES
  (1, N'South Coast Region'),
  (2, N'Southern Interior Region'),
  (3, N'Northern Region');
GO
