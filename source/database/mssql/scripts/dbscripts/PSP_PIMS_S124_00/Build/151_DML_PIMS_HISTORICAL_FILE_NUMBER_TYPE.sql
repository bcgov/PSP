/* -----------------------------------------------------------------------------
Populate the PIMS_HISTORICAL_FILE_NUMBER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Apr-18  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_HISTORICAL_FILE_NUMBER_TYPE
GO

INSERT INTO PIMS_HISTORICAL_FILE_NUMBER_TYPE (HISTORICAL_FILE_NUMBER_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'LISNO',    N'LIS',                       1),
  (N'PROPNEG',  N'Property Negotiation (PN)', 2),
  (N'PSNO',     N'PS',                        3),
  (N'PUBWORKS', N'Public Works (PW)',         4),
  (N'REGLSLIC', N'Regional lease/license',    5),
  (N'RESERVE',  N'Reserve (R)',               6),
  (N'OTHER',    N'Other',                    99);
GO
