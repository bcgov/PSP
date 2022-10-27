/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_IMPROVEMENT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_IMPROVEMENT_TYPE
GO

INSERT INTO PIMS_PROPERTY_IMPROVEMENT_TYPE (PROPERTY_IMPROVEMENT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'RTA',      N'Residential Tenancy Act'),
  (N'COMMBLDG', N'Commercial Building'),
  (N'OTHER',    N'Other');