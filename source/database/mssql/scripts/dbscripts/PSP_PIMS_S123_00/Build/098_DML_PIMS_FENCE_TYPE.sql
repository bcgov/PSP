/* -----------------------------------------------------------------------------
Delete all data from the PIMS_FENCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_FENCE_TYPE
GO

INSERT INTO PIMS_FENCE_TYPE (FENCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ALUMBARB', N'Aluminum, Barbwire (Type C)'),
  (N'CHAINLNK', N'Chain Link (Type D)'),
  (N'CONCRETE', N'Concrete'),
  (N'ELECTRIC', N'Electric'),
  (N'IRON',     N'Iron'),
  (N'WIREWOOD', N'Page Wire, Wood'),
  (N'OTHER',    N'Other');
GO
