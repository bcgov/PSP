/* -----------------------------------------------------------------------------
Populate the PIMS_MANAGEMENT_FILE_PROGRAM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-15  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_MANAGEMENT_FILE_PROGRAM_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_PROGRAM_TYPE (MANAGEMENT_FILE_PROGRAM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'AGRICULT', N'Agricultural',            1),
  (N'COMMBLDG', N'Commercial building',     2),
  (N'ENCAMP',   N'Encampments',             3),
  (N'ENGINEER', N'Engineering',             4),
  (N'GOVERNMT', N'Government',              5),
  (N'MOTTUSE',  N'MOTT use',                6),
  (N'OILGAS',   N'Oil/Gas',                 7),
  (N'OTHER',    N'Other',                   8),
  (N'PARKING',  N'Parking and/or storage',  9),
  (N'PUBTRANS', N'Public transportation',  10),
  (N'RAIL',     N'Rail',                   11),
  (N'TRAILS',   N'Rail trails/trails',     12),
  (N'RESRENTL', N'Residential rentals',    13),
  (N'UTILITY',  N'Utilities',              14);
GO
