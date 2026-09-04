/* -----------------------------------------------------------------------------
Populate the PIMS_ACQ_FILE_TAKE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_FILE_TAKE_TYPE
GO

INSERT INTO PIMS_ACQ_FILE_TAKE_TYPE (ACQ_FILE_TAKE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'UNKNOWN',   N'Unknown',              1),
  (N'NOTREQ',    N'Not required',         2),
  (N'PARTLTACQ', N'Partial Acquisition',  3),
  (N'PERMSRW',   N'Permanent SRW',        4),
  (N'TEMPSRW',   N'Temporary SRW',        5),
  (N'TTLACQ',    N'Total Acquisition',    6),
  (N'LEASE',     N'Lease',                7),
  (N'LICENCE',   N'Licence',              8),
  (N'ACCSSCLOS', N'Access closure',       9),
  (N'BYDEDCTN',  N'By dedication',       10);
GO
