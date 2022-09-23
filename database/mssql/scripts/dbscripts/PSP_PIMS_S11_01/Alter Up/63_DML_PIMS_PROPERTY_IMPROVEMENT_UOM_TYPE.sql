/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_IMPROVEMENT_UOM_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_IMPROVEMENT_UOM_TYPE
GO

INSERT INTO PIMS_PROPERTY_IMPROVEMENT_UOM_TYPE (PROPERTY_IMPROVEMENT_UOM_TYPE_CODE, DESCRIPTION)
VALUES
  (N'SQFT',  N'Square feet'),
  (N'SQM',   N'Square meters'),
  (N'OTHER', N'Other');