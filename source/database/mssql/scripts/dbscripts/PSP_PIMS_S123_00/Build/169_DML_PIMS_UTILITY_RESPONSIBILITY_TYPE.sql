/* -----------------------------------------------------------------------------
Populate the PIMS_UTILITY_RESPONSIBILITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-01  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_UTILITY_RESPONSIBILITY_TYPE
GO

INSERT INTO PIMS_UTILITY_RESPONSIBILITY_TYPE (UTILITY_RESPONSIBILITY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'BCTFA',   N'BCTFA',            1),
  (N'PROJECT', N'Project',          2),
  (N'PROPMGR', N'Property Manager', 3),
  (N'REGION',  N'Region',           4),
  (N'TENANT',  N'Tenant',           5),
  (N'OTHER',   N'Other',           99);
GO
