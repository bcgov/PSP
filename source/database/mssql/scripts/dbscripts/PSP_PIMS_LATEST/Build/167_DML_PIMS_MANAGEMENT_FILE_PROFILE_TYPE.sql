/* -----------------------------------------------------------------------------
Populate the PIMS_MANAGEMENT_FILE_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_MANAGEMENT_FILE_PROFILE_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_PROFILE_TYPE (MANAGEMENT_FILE_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'CONTRACT', N'Contractor', 1),
  (N'MINSTAFF', N'MoTT staff', 2);
GO
