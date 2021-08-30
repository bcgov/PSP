/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_CLASSIFICATION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_TYPE
GO

INSERT INTO PIMS_PROPERTY_TYPE (PROPERTY_TYPE_CODE, DESCRIPTION)
VALUES
  (N'LAND', N'Land'),
  (N'BUILD', N'Buiding'),
  (N'SUBDIV', N'Subdivison');