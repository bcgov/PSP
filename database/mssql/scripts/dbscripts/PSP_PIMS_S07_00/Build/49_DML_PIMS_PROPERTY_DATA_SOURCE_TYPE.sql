/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_DATA_SOURCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_DATA_SOURCE_TYPE
GO

INSERT INTO PIMS_PROPERTY_DATA_SOURCE_TYPE (PROPERTY_DATA_SOURCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PAIMS', N'Property Acquisition and Inventory Management System (PAIMS)'),
  (N'GAZ', N'BC Gazette');