/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_CLASSIFICATION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_CLASSIFICATION_TYPE
GO

INSERT INTO PIMS_PROPERTY_CLASSIFICATION_TYPE (PROPERTY_CLASSIFICATION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'COREOPER',   N'Core Operational', 1),
  (N'CORESTRAT',  N'Core Strategic', 2),
  (N'SURPACTIVE', N'Surplus Active', 3),
  (N'SURPENCUM',  N'Surplus Encumbered', 4),
  (N'DISPOSED',   N'Disposed', 5),
  (N'DEMOLISHED', N'Demolished', 6),
  (N'SUBDIVIDED', N'Subdivided', 7),
  (N'UNKNOWN',    N'Unknown', 8);