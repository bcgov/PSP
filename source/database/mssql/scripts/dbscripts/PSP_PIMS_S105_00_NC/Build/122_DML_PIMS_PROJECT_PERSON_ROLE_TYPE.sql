/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_PROJECT_PERSON_ROLE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_PERSON_ROLE_TYPE
GO

INSERT INTO PIMS_PROJECT_PERSON_ROLE_TYPE (PROJECT_PERSON_ROLE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'DISTMGR',   N'District Manager',                 1),
  (N'PROJMGR',   N'Project Manager',                  2),
  (N'CONSUPR',   N'Construction Super',               3),
  (N'PRAQCOORD', N'Property Acquisition Coordinator', 4),
  (N'PRAQMGR',   N'Property Acquisition Manager',     5);
GO
