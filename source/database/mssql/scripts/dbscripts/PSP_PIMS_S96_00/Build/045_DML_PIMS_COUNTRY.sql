/* -----------------------------------------------------------------------------
Delete all data from the PIMS_COUNTRY table and repopulate.

*** NOTE ***
The PIMS_PROVINCE_STATE table must be empty due to a foreign key dependency.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_COUNTRY
GO

INSERT INTO PIMS_COUNTRY (COUNTRY_ID, COUNTRY_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (1, N'CA', N'Canada', 1),
  (2, N'US', N'United States of America', 2),
  (3, N'MX', N'Mexico', 3),
  (4, N'OTHER', N'Other', 99);