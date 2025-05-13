/* -----------------------------------------------------------------------------
Populate the PIMS_MANAGEMENT_FILE_PURPOSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-01  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_MANAGEMENT_FILE_PURPOSE_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_PURPOSE_TYPE (MANAGEMENT_FILE_PURPOSE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'AGRICULT', N'Agricultural',             1),
  (N'BCFERRY',  N'BC Ferries',               2),
  (N'BCTRANS',  N'BC Transit and TransLink', 3),
  (N'COMMBLDG', N'Commercial building',      4),
  (N'ENCAMP',   N'Encampments',              5),
  (N'ENGINEER', N'Engineering',              6),
  (N'GENERAL',  N'General',                  7),
  (N'GOVERNMT', N'Government',               8),
  (N'MOTTUSE',  N'MOTT use',                 9),
  (N'OILGAS',   N'Oil/Gas',                 10),
  (N'OTHER',    N'Other',                   11),
  (N'PARKING',  N'Parking and/or storage',  12),
  (N'RAIL',     N'Rail',                    13),
  (N'TRAILS',   N'Rail trails',             14),
  (N'RESRENTL', N'Residential rentals',     15),
  (N'UTILITY',  N'Utilities',               16);
GO
