/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_IMPROVEMENT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_IMPROVEMENT_TYPE
GO

INSERT INTO PIMS_PROPERTY_IMPROVEMENT_TYPE (PROPERTY_IMPROVEMENT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'RTA',      N'Residential Tenancy Act', 2),
  (N'COMMBLDG', N'Commercial Building',     1),
  (N'OTHER',    N'Other',                  99);