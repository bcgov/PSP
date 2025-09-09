/* -----------------------------------------------------------------------------
Populate the PIMS_ACQ_FILE_PROGESS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_FILE_PROGESS_TYPE
GO

INSERT INTO PIMS_ACQ_FILE_PROGESS_TYPE (ACQ_FILE_PROGESS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'NOTREQ',       N'Not required',                          1),
  (N'NOLNGRREQ',    N'No longer required',                    2),
  (N'ACQRDBYOTH',   N'Acquired by others',                    3),
  (N'PRELIMDES',    N'Preliminary design stage',              4),
  (N'APPRVD4ACQ',   N'Approved for acquisition',              5),
  (N'OWNRCNTCTD',   N'Owner contacted',                       6),
  (N'WTG4PAPLN',    N'Waiting for PA plan',                   7),
  (N'PAPLNRCVD',    N'PA plan received',                      8),
  (N'WTG4ANCILPLN', N'Waiting for ancillary plan',            9),
  (N'WTG4APPRSL',   N'Waiting for appraisal / other report', 10),
  (N'NEGOTIATNG',   N'Negotiating',                          11),
  (N'OFFERMADE',    N'Offer made',                           12),
  (N'CNTRCTSGND',   N'Contract signed',                      13),
  (N'OUT4CONVEY',   N'Out for conveyancing',                 14),
  (N'PSN4CNSTRCT',  N'Possession for construction',          15),
  (N'OWNERTENURE',  N'Ownership (tenure)',                   16),
  (N'EXPROPRTD',    N'Expropriated',                         17),
  (N'SETTLED',      N'Settled',                              18),
  (N'FILCOMPLT',    N'File complete (return to MoTT)',       19);
GO
