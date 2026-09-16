/* -----------------------------------------------------------------------------
Populate the PIMS_PROPERTY_OPERATION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Feb-07  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_OPERATION_TYPE
GO

INSERT INTO PIMS_PROPERTY_OPERATION_TYPE (PROPERTY_OPERATION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CONSOLIDATE', N'Property Consolidation'),
  (N'SUBDIVIDE',   N'Property Subdivision');
GO
